using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using MovieDatabase.Core.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MovieDatabase.Controllers
{
    [ApiController]
    [Route("api/movie")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMovies([FromQuery] bool includeCast = false)
        {
                var movies = await _movieService.GetAllAsync(includeCast: includeCast);
                _response.IsSuccess = true;
                _response.Result = movies;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
        }

        [HttpGet("{id:int}", Name = "GetMovie")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMovie(int id, [FromQuery] bool includeCast = false)
        {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID provided.");
                    return BadRequest(_response);
                }

                var movie = await _movieService.GetByIdAsync(id, includeCast);

                if (movie is null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Movie with ID {id} not found.");
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.Result = movie;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        public async Task<ActionResult<APIResponse>> CreateMovie([FromBody] MovieCreateDto request)
        {
            var createdMovie = await _movieService.CreateAsync(request);

            _response.IsSuccess = true;
            _response.Result = createdMovie;
            _response.StatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetMovie", new { id = createdMovie.Id }, _response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIResponseNotFoundExample))]
        public async Task<ActionResult<APIResponse>> DeleteMovie(int id)
        {
            var deleted = await _movieService.DeleteAsync(id);

            if (!deleted)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add($"Movie with ID {id} not found.");
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
        public async Task<ActionResult<APIResponse>> UpdateMovie(int id, MovieUpdateDto request)
        {
            request.SetId(id);
            var updatedMovie = await _movieService.UpdateAsync(request);

            if (updatedMovie is null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.Errors.Add($"Movie with ID {id} not found.");
                return NotFound(_response);
            }

            _response.Result = updatedMovie;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}