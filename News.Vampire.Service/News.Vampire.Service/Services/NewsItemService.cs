using Grpc.Core;
using Microsoft.Extensions.Logging;
using News.Vampire.Service.BusinessLogic.Interfaces;
using System.Threading.Tasks;
using News.Vampire.Service.Protos;
using System.Linq;
using Google.Protobuf.WellKnownTypes;

namespace News.Vampire.Service.Services
{
    public class NewsItemService: NewsItemServiceGrpc.NewsItemServiceGrpcBase
    {
        private readonly IAuthLogic _authLogic;
        private readonly INewsItemLogic _newsItemLogic;
        private readonly ISourceLogic _sourceLogic;
        private readonly ILogger<UserGroupService> _logger;

        public NewsItemService(ILogger<UserGroupService> logger, IAuthLogic authLogic, INewsItemLogic newsItemLogic, ISourceLogic sourceLogic)
        {
            _logger = logger;
            _authLogic = authLogic;
            _newsItemLogic = newsItemLogic;
            _sourceLogic = sourceLogic;
        }


        public override async Task<NewsItemsGrpc> GetNewsItems(NewsItemRequest request, ServerCallContext context)
        {
            var result = new NewsItemsGrpc();
            var user = await _authLogic.Authentificate(request.Login, request.Password);
            if (user != null)
            {
                var sources = await _sourceLogic.GetAll();
                var newsItems = await _newsItemLogic.GetLatestNewsBySubscriptionAsync(user.Id);


                foreach (var ni in newsItems)
                {
                    var source = sources.FirstOrDefault(s => s.Id == ni.SourceId);

                    var niGrpc = new NewsItemGrpc
                    {
                        Id = ni.Id,
                        SourceId = ni.SourceId,
                        Source = source?.Name,
                        ImageUrl = ni.Url.Count > 1 ? ni.Url.Last() : null,
                        Title = ni.Title,
                        Description = ni.Description,
                        NewsUrl = ni.Url.FirstOrDefault(),
                        PublicationDate = ni.PublicationDate
                    };

                    niGrpc.Categories.AddRange(ni.Category.Select(c => new CategoryGrpc { Name = c }));
                    result.Result.Add(niGrpc);
                }
            }
            return result;
        }
    }
}
