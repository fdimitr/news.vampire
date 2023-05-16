using Microsoft.Extensions.Logging;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.DataAccess;
using News.Vampire.Service.Managers.Interfaces;
using News.Vampire.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace News.Vampire.Service.Managers
{
    public class DownloadManager : IDownloadManager
    {
        private readonly ISourceLogic _sourceLogic;
        private readonly INewsItemLogic _newsItemLogic;
        private readonly ILogger<DownloadManager> _logger;

        public DownloadManager(ILogger<DownloadManager> logger, ISourceLogic sourceLogic, INewsItemLogic newsItemLogic)
        {
            _sourceLogic = sourceLogic;
            _newsItemLogic = newsItemLogic;
            _logger = logger;
        }

        public async Task StartSessionAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Started new downloading session (Console output)");
            _logger.LogDebug("Started new downloading session");

            var sources = await _sourceLogic.GetSourcesReadyToLoadAsync();

            _logger.LogDebug($"{sources.Count} sources ready to download");

            int maxConcurrency = 15;
            using (SemaphoreSlim semaphore = new SemaphoreSlim(0, maxConcurrency))
            {
                List<Task> tasks = new List<Task>();
                foreach (var source in sources)
                {
                    tasks.Add(DownloadingProcessAsync(source, semaphore, stoppingToken));
                }
                semaphore.Release(maxConcurrency);
                await Task.WhenAll(tasks);
            }

            _logger.LogDebug($"Downloading session completed");
        }

        private async Task DownloadingProcessAsync(Source source, SemaphoreSlim semaphore, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Task {0} begins and waits for the semaphore.", source.Id);
            await semaphore.WaitAsync(cancellationToken);
            HttpClient client = new();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            _logger.LogDebug("Task {0} enters the semaphore.", source.Id);
            try
            {
                SyndicationFeed feed;
                using (XmlTextReader xmlReader = new XmlTextReader(source.Url))
                {
                    feed = await Task.Run(() => SyndicationFeed.Load(xmlReader), cancellationToken);
                }

                int numberItemsAdded = 0;
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

                        await _newsItemLogic.CreateAsync(newItem);
                        numberItemsAdded++;
                    }
                }
                if (numberItemsAdded > 0)
                {
                    _logger.LogDebug($"Added {numberItemsAdded} news items into <{source.Name}> source");
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
                _logger.LogDebug("Task {0} releases the semaphore; previous count: {1}.",
                                  source.Id, semaphoreCount);
            }
        }

    }
}
