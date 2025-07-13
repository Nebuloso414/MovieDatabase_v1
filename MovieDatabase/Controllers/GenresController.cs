using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MovieDatabase.Controllers
{
    [ApiController]
    [Route("api/genre")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class GenresController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        public async Task<ActionResult<APIResponse>> GetGenres()
        {
                var genres = await _genreService.GetAllAsync();

                _response.IsSuccess = true;
                _response.Result = genres;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetGenre")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIResponseNotFoundExample))]
        public async Task<ActionResult<APIResponse>> GetGenre(int id)
        {
                var genre = await _genreService.GetByIdAsync(id, tracked: false);

                if (genre == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Genre with ID {id} not found.");
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.Result = genre;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        public async Task<ActionResult<APIResponse>> CreateGenre([FromBody] GenreCreateDto request)
        {
                var genre = await _genreService.CreateAsync(request);

                _response.IsSuccess = true;
                _response.Result = genre;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetGenre", new { id = genre.Id }, _response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIResponseNotFoundExample))]
        public async Task<ActionResult<APIResponse>> DeleteGenre(int id)
        {
            var deleted = await _genreService.DeleteAsync(id);

            if (!deleted)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add($"Genre with ID {id} not found.");
                return NotFound(_response);
            }

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;

            return Ok(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        public async Task<ActionResult<APIResponse>> UpdateGenre(int id, GenreUpdateDto request)
        {
            request.SetId(id);
            var response = await _genreService.UpdateAsync(request);

            _response.Result = response;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;

            return Ok(_response);
        }
    }
}