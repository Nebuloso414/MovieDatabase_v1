using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using MovieDatabase.Repository.IRepository;
using MovieDatabase.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MovieDatabase.Controllers
{
    [ApiController]
    [Route("api/MovieAPI")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IMovieRepository _movieContext;
        private readonly IGenreRepository _genreContext;
        private readonly IMapper _mapper;
        private readonly IMovieService _movieService;

        public MoviesController(IMovieRepository movieRepository, IGenreRepository genreRepository, IMapper mapper, IMovieService movieService)
        {
            _movieContext = movieRepository;
            _genreContext = genreRepository;
            _mapper = mapper;
            _response = new();
            _movieService = movieService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        public async Task<ActionResult<APIResponse>> GetMovies()
        {
            try
            {
                var movies = await _movieService.GetAllAsync(filter: null, includeProperties: "Genres");
                _response.Result = _mapper.Map<List<MovieDto>>(movies);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
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
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIResponseNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        public async Task<ActionResult<APIResponse>> GetMovie(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID provided.");
                    return BadRequest(_response);
                }

                var movie = await _movieService.GetByIdAsync(x => x.Id == id, false, "Genres");

                if (movie == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Movie with ID {id} not found.");
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<MovieDto>(movie);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
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
        public async Task<ActionResult<APIResponse>> CreateMovie([FromBody] MovieCreateDto createMovieDto)
        {
            try
            {
                if (await _movieContext.GetByIdAsync(x => x.Title == createMovieDto.Title) != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Movie already exists.");
                    return BadRequest(_response);
                }

                if (createMovieDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid movie data provided.");
                    return BadRequest(_response);
                }

                var movie = await _movieService.CreateAsync(createMovieDto);

                _response.Result = _mapper.Map<MovieDto>(movie);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetMovie", new { id = movie.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
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
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID provided.");
                    return BadRequest(_response);
                }

                var movie = await _movieContext.GetByIdAsync(x => x.Id == id);

                if (movie == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Movie with ID {id} not found.");
                    return NotFound(_response);
                }

                await _movieContext.RemoveAsync(movie);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
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
        public async Task<ActionResult<APIResponse>> UpdateMovie(int id, MovieUpdateDto updatedMovie)
        {
            try
            { 
                if (id <= 0 || updatedMovie == null || id != updatedMovie.Id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID or movie data provided.");
                    return BadRequest(_response);
                }

                Movie movie = _mapper.Map<Movie>(updatedMovie);

                await _movieContext.UpdateAsync(movie);

                _response.Result = _mapper.Map<MovieDto>(movie);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialMovie")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIResponseNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        [Consumes("application/json-patch+json")]
        public async Task<ActionResult<APIResponse>> UpdatePartialMovie(int id, [FromBody] JsonPatchDocument<MovieUpdateDto> patchDto)
        {
            try
            {
                if (id <= 0 || patchDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid patch document or Id provided.");
                    return BadRequest(_response);
                }
                var movie = await _movieContext.GetByIdAsync(x => x.Id == id, tracked: false);

                if (movie == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Movie with ID {id} not found.");
                    return NotFound(_response);
                }

                var movieUpdateDto = _mapper.Map<MovieUpdateDto>(movie);
                patchDto.ApplyTo(movieUpdateDto, (error) =>
                {
                    if (error.AffectedObject != null)
                    {
                        ModelState.AddModelError(error.AffectedObject.ToString() ?? string.Empty, error.ErrorMessage);
                    }
                });

                if (!ModelState.IsValid)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.AddRange(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    _response.Result = ModelState;
                    return BadRequest(_response);
                }

                movie = _mapper.Map<Movie>(movieUpdateDto);

                await _movieContext.UpdateAsync(movie);

                _response.Result = _mapper.Map<MovieDto>(movie);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Errors.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }
    }
}