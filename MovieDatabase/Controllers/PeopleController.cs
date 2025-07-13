using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using MovieDatabase.Core.Services;
using System.Net;

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
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<IEnumerable<PeopleDto>>(peopleList);
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<APIResponse>> GetPersonById(int id)
        {
            try
            {
                var person = await _peopleService.GetByIdAsync(id, false);
                if (person == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { "Person not found" };
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<PeopleDto>(person);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreatePerson([FromBody] PeopleCreateDto peopleDto)
        {
            try
            {
                if (peopleDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Invalid data" };
                    return BadRequest(_response);
                }

                var person = await _peopleService.CreateAsync(peopleDto);

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<PeopleDto>(person);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> DeletePerson(int id)
        {
            try
            {
                var person = await _peopleService.GetByIdAsync(id, false);

                if (person == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string> { "Person not found" };
                    return NotFound(_response);
                }

                await _peopleService.DeleteAsync(person);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return NoContent();
            }
            catch (Exception ex)
            {
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> UpdatePerson(int id, [FromBody] PeopleUpdateDto updatedPeople)
        {
            try
            {
                if (updatedPeople == null || updatedPeople.Id != id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors = new List<string> { "Invalid data" };
                    return BadRequest(_response);
                }

                var updatedPerson = await _peopleService.UpdateAsync(updatedPeople);

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<PeopleDto>(updatedPerson);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (BadHttpRequestException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return StatusCode((int)_response.StatusCode, _response);
            }
        }
    }
}
