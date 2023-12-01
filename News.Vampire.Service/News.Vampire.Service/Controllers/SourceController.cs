using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using News.Vampire.Service.BusinessLogic;
using News.Vampire.Service.BusinessLogic.Interfaces;
using News.Vampire.Service.Models;
using News.Vampire.Service.Models.Dto;

namespace News.Vampire.Service.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SourceController: ODataController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SourceController> _logger;
        private readonly ISourceLogic _sourceLogic;

        public SourceController(ILogger<SourceController> logger, ISourceLogic sourceLogic, IMapper mapper)
        {
            _logger = logger;
            _sourceLogic = sourceLogic;
            _mapper = mapper;
        }

        // GET: api/<SourceController>
        [HttpGet]
        [EnableQuery]
        public IQueryable<SourceDto> Get()
        {
            _logger.LogDebug($"There was a {nameof(Get)} method call of {nameof(SourceController)}. Call from {HttpContext.Request.Host.Host}");
            return _mapper.ProjectTo<SourceDto>(_sourceLogic.GetAll());
        }
    }
}
