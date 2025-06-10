using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using MovieDatabase.Services;

namespace MovieDatabase.Controllers
{
    [ApiController]
    [Route("api/people")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class PeopleController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IPeopleService _peopleService;
        private readonly IMapper _mapper;

        public PeopleController(IPeopleService peopleService, IMapper mapper)
        {
            _peopleService = peopleService;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetPeople([FromQuery] string? name = null)
        {
            try
            {
                IEnumerable<People?> peopleList;
                if (string.IsNullOrEmpty(name))
                {
                    peopleList = await _peopleService.GetAllAsync();
                }
                else
                {
                    peopleList = await _peopleService.GetByNameAsync(name);
                }
                _response.Result = _mapper.Map<IEnumerable<PeopleDto>>(peopleList);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<APIResponse>> GetPeopleById(int id)
        {
            try
            {
                var person = await _peopleService.GetByIdAsync(p => p.Id == id, false);
                if (person == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { "Person not found" };
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<PeopleDto>(person);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return StatusCode((int)_response.StatusCode, _response);
            }
        }
    }
}
