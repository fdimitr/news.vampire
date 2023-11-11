using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Models;

namespace News.Vampire.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ODataController
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IGroupLogic _groupLogic;


        public GroupController(ILogger<GroupController> logger, IGroupLogic groupLogic)
        {
            _logger = logger;
            _groupLogic = groupLogic;
        }

        // GET: api/<GroupController>
        [HttpGet]
        [EnableQuery]
        public async Task<IList<Group>> Get()
        {
            _logger.LogDebug($"There was a {nameof(Get)} method call of {nameof(GroupController)}. Call from {HttpContext.Request.Host.Host}");
            return await _groupLogic.GetAllAsync();
        }

        // POST api/<GroupController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GroupController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GroupController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
