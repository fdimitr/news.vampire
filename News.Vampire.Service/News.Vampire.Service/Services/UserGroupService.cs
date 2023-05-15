using Grpc.Core;
using Microsoft.Extensions.Logging;
using News.Vampire.Service.BusinessLogic.Interfaces;
using System.Threading.Tasks;
using News.Vampire.Service.Protos;

namespace News.Vampire.Service.Services
{
    public class UserGroupService : UserGroupServiceGrpc.UserGroupServiceGrpcBase
    {
        private readonly ILogger<UserGroupService> _logger;
        private readonly IUserGroupLogic _userGroupLogic;

        public UserGroupService(ILogger<UserGroupService> logger, IUserGroupLogic userGroupLogic)
        {
            _logger = logger;
            _userGroupLogic = userGroupLogic;
        }

        public override async Task<UserGroupsGrpc> GetUserGroups(UserGroupRequest request, ServerCallContext context)
        {
            var userGroups = await _userGroupLogic.GetAllByUserAsync(request.UserId);
            var userGroupsGrpc = new UserGroupsGrpc();
            foreach (var userGroup in userGroups)
            {
                var userGroupGrpc = new UserGroupGrpc
                {
                    Id = userGroup.Id,
                    Name = userGroup.Name
                };
                foreach (var subscription in userGroup.Subscriptions)
                {
                    userGroupGrpc.Sources.Add(new SourceGrpc
                    {
                        Id = subscription.SourceId,
                        Name = subscription.Source.Name
                    });
                }

                userGroupsGrpc.Result.Add(userGroupGrpc);
            }
            return userGroupsGrpc;
        }
    }
}
