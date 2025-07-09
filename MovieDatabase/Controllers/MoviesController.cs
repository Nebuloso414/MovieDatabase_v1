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
        private readonly IMapper _mapper;
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;

        public MoviesController(IMovieService movieService, IMapper mapper, IGenreService genreService)
        {
            _movieService = movieService;
            _mapper = mapper;
            _response = new();
            _genreService = genreService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetMovies([FromQuery] bool includeCast = false)
        {
            try
            {
                var movies = await _movieService.GetMoviesAsync(includeCast: includeCast);
                _response.IsSuccess = true;
                _response.Result = movies;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpGet("{id:int}", Name = "GetMovie")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetMovie(int id, [FromQuery] bool includeCast = false)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID provided.");
                    return BadRequest(_response);
                }

                var movie = await _movieService.GetMoviesAsync(filter: m => m.Id == id, includeCast);

                if (movie.ToList().Count == 0)
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
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        public async Task<ActionResult<APIResponse>> CreateMovie([FromBody] MovieCreateDto request)
        {
            try
            {
                if (await _movieService.MovieExistsAsync(request.Title))
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Movie already exists.");
                    return BadRequest(_response);
                }

                if (request == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid movie data provided.");
                    return BadRequest(_response);
                }

                var genres = await _genreService.ProcessGenreNamesAsync(request.Genres);

                if (genres.NotFoundGenres.Count > 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string>
                    {
                        "The following genres do not exist: " + string.Join(", ", genres.NotFoundGenres)
                    };
                    return BadRequest(_response);
                }

                var movie = _mapper.Map<Movie>(request);
                movie.Genres = genres.FoundGenres;
                await _movieService.CreateAsync(movie);

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<MovieDto>(movie);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetMovie", new { id = movie.Id }, _response);
            }
            catch (BadHttpRequestException ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIResponseNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        public async Task<ActionResult<APIResponse>> DeleteMovie(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID provided.");
                    return BadRequest(_response);
                }

                var movie = await _movieService.GetMoviesAsync(x => x.Id == id);

                if (movie.Count() == 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Movie with ID {id} not found.");
                    return NotFound(_response);
                }

                await _movieService.DeleteAsync(_mapper.Map<Movie>(movie.SingleOrDefault()));

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        public async Task<ActionResult<APIResponse>> UpdateMovie(int id, MovieUpdateDto request)
        {
            try
            { 
                if (id <= 0 || request == null || id != request.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID or movie data provided.");
                    return BadRequest(_response);
                }
                // inject genre service. Create new method in genre service to retrieve the genres in the updated movie and the genres not found in the db
                var genres = await _genreService.ProcessGenreNamesAsync(request.Genres);

                if (genres.NotFoundGenres.Count > 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors = new List<string>
                    {
                        "The following genres do not exist: " + string.Join(", ", genres.NotFoundGenres)
                    };
                    return BadRequest(_response);
                }

                var movie = _mapper.Map<Movie>(request);
                movie.Genres = genres.FoundGenres;
                var UpdatedMovie = await _movieService.UpdateAsync(movie);

                _response.Result = _mapper.Map<MovieDto>(UpdatedMovie);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
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
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }
    }
}