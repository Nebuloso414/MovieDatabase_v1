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

        public PeopleController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAll([FromQuery] string? name = null)
        {
            IEnumerable<PeopleDto> response;
            if (string.IsNullOrEmpty(name))
            {
                response = await _peopleService.GetAllAsync();
            }
            else
            {
                response = await _peopleService.GetByNameAsync(name);
            }
            _response.IsSuccess = true;
            _response.Result = response;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<APIResponse>> GetById(int id)
        {
            var response = await _peopleService.GetByIdAsync(id, tracked: false);
            if (response == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add("Person not found");
                return NotFound(_response);
            }

            _response.IsSuccess = true;
            _response.Result = response;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> Create([FromBody] PeopleCreateDto peopleDto)
        {
            var response = await _peopleService.CreateAsync(peopleDto);

            _response.IsSuccess = true;
            _response.Result = response;
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, _response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            var deleted = await _peopleService.DeleteAsync(id);

            if (!deleted)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add("Person not found");
                return NotFound(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.NoContent;
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Update(int id, [FromBody] PeopleUpdateDto request)
        {
            request.SetId(id);
            var updatedPerson = await _peopleService.UpdateAsync(request);

            if (updatedPerson == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add("Person not found");
                return NotFound(_response);
            }

            _response.IsSuccess = true;
            _response.Result = updatedPerson;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
