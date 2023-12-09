using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Configuration;
using News.Vampire.Service.Managers.Interfaces;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.Managers
{
    public class DownloadManager : IDownloadManager
    {
        private readonly ISourceLogic _sourceLogic;
        private readonly INewsItemLogic _newsItemLogic;
        private readonly ILogger<DownloadManager> _logger;
        private readonly IAmazonLogic _amazonLogic;
        private readonly AppSettings _appSettings;

        public DownloadManager(ILogger<DownloadManager> logger, IConfiguration configuration, ISourceLogic sourceLogic, INewsItemLogic newsItemLogic, IAmazonLogic amazonLogic)
        {
            _sourceLogic = sourceLogic;
            _newsItemLogic = newsItemLogic;
            _logger = logger;
            _amazonLogic = amazonLogic;

            _appSettings = new AppSettings();
            configuration.GetSection(AppSettings.Section).Bind(_appSettings);
        }

        public async Task StartSessionAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Started new downloading session (Console output)");
            _logger.LogInformation("Started new downloading session");

            var sources = await _sourceLogic.GetSourcesReadyToLoadAsync();

            _logger.LogInformation($"{sources.Count} sources ready to download");

            using (var semaphore = new SemaphoreSlim(0, _appSettings.MaxConcurrencyDownloadSessions))
            {
                List<Task> tasks = new();
                foreach (var source in sources)
                {
                    tasks.Add(DownloadingProcessAsync(source, semaphore, stoppingToken));
                }
                semaphore.Release(_appSettings.MaxConcurrencyDownloadSessions);
                await Task.WhenAll(tasks);
            }

            _logger.LogInformation($"Downloading session completed");
        }

        private async Task DownloadingProcessAsync(Source source, SemaphoreSlim semaphore, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Task {0} begins and waits for the semaphore.", source.Id);
            await semaphore.WaitAsync(cancellationToken);
            HttpClient client = new();
            var httpRequestMessage = new HttpRequestMessage();

            _logger.LogInformation("Task {0} enters the semaphore.", source.Id);
            try
            {
                SyndicationFeed feed;
                using (XmlTextReader xmlReader = new XmlTextReader(source.Url))
                {
                    feed = await Task.Run(() => SyndicationFeed.Load(xmlReader), cancellationToken);
                }

                int numberItemsAdded = 0;
                var databaseId = _newsItemLogic.GetDatabaseId();

                foreach (SyndicationItem item in feed.Items)
                {
                    string externalId = string.IsNullOrWhiteSpace(item.Id) ? item.BaseUri.ToString() : item.Id;
                    if (!await _newsItemLogic.ExistsAsync(source.Id, externalId))
                    {
                        // Get rid of the tags
                        var description = Regex.Replace(item.Summary.Text, @"<.+?>", String.Empty);
                        // Then decode the HTML entities
                        description = WebUtility.HtmlDecode(description).Trim();

                        var newItem = new NewsItem
                        {
                            SourceId = source.Id,
                            ExternalId = externalId,
                            Url = item.Links.Select(l => l.Uri.ToString()).ToList(),
                            PublicationDate = item.PublishDate.ToUnixTimeSeconds(),
                            TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                            Title = item.Title.Text,
                            Category = item.Categories.Select(c => c.Name).ToList(),
                            Description = description,
                            Author = item.Authors.Select(a => a.Name).ToList()
                        };

                        // Upload news item image to S3 and get S3 Url
                        await ProcessNewsItemImage(item, databaseId, newItem, source.Code);

                        await _newsItemLogic.CreateAsync(newItem);
                        numberItemsAdded++;
                    }
                }
                if (numberItemsAdded > 0)
                {
                    _logger.LogInformation($"Added {numberItemsAdded} news items into <{source.Name}> source");
                }

                source.NextLoadedTime = DateTimeOffset.UtcNow.AddMinutes(source.UpdateFrequencyMinutes).ToUnixTimeSeconds();
                await _sourceLogic.UpdateAsync(source);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred in DownloadingProcess");
            }
            finally
            {
                var semaphoreCount = semaphore.Release();
                _logger.LogInformation("Task {0} releases the semaphore; Count of free semaphores: {1}.",
                                  source.Id, semaphoreCount);
            }
        }

        private async Task ProcessNewsItemImage(SyndicationItem item, string databaseId, NewsItem newItem, string sourceCode)
        {
            var link = item.Links?.FirstOrDefault(l => IsImageMediaType(l.MediaType));
            if (link != null)
            {
                var s3Response = await _amazonLogic.DownloadImageAndSaveToS3Async(databaseId, $"article.images/{sourceCode}", link.Uri);
                if (s3Response.IsSucceed)
                {
                    newItem.ImageUrl = s3Response.Response;
                }
            }
        }

        private bool IsImageMediaType(string mediaType)
        {
            return mediaType switch
            {
                "image/jpeg" or "image/jpg" or "image/gif" or "image/png" or "image/svg" => true,
                _ => false,
            };
        }

    }
}
