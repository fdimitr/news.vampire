using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Models.Dto;

namespace News.Vampire.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserGroupController> _logger;
        private readonly IUserGroupLogic _userGroupLogic;


        public UserGroupController(ILogger<UserGroupController> logger, IUserGroupLogic groupLogic, IMapper mapper)
        {
            _logger = logger;
            _userGroupLogic = groupLogic;
            _mapper = mapper;
        }

        // GET: api/<SourceController>
        [HttpGet]
        [EnableQuery]
        public IQueryable<UserGroupDto> Get(int userId)
        {
            _logger.LogDebug($"There was a {nameof(Get)} method call of {nameof(UserGroupController)}. Call from {HttpContext.Request.Host.Host}");
            return _mapper.ProjectTo<UserGroupDto>(_userGroupLogic.GetAllByUserAsync(userId));
        }

    }
}
