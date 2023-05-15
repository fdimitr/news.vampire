using Grpc.Core;
using Microsoft.Extensions.Logging;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Protos;
using System.Threading.Tasks;

namespace News.Vampire.Service.Services
{
    public class GroupService: GroupServiceGrpc.GroupServiceGrpcBase
    {
        private readonly ILogger<GroupService> _logger;
        private readonly IGroupLogic _groupLogic;

        public GroupService(ILogger<GroupService> logger, IGroupLogic groupLogic)
        {
            _logger = logger;
            _groupLogic = groupLogic;
        }

        public override async Task<GroupsGrpc> GetGroups(EmptyRequest request, ServerCallContext context)
        {
            var groups = await _groupLogic.GetAllAsync();
            var groupsGrpc = new GroupsGrpc();
            foreach(var group in groups)
            {
                groupsGrpc.Result.Add(new GroupGrpc
                {
                    Id = (int)group.Id,
                    IsActive = group.isActive,
                    Name = group.Name
                });
            }
            return groupsGrpc;
        }
    }
}
