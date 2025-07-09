using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using MovieDatabase.Core.Services;
using Swashbuckle.AspNetCore.Filters;

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
        private readonly IMapper _mapper;

        public GenresController(IGenreService genreService, IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        public async Task<ActionResult<APIResponse>> GetGenres()
        {
            try
            {
                var genres = await _genreService.GetAllAsync();
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<List<GenreDto>>(genres);
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

        [HttpGet("{id:int}", Name = "GetGenre")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIResponseNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        public async Task<ActionResult<APIResponse>> GetGenre(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID provided.");
                    return BadRequest(_response);
                }

                var genre = await _genreService.GetByIdAsync(x => x.Id == id, tracked: false);

                if (genre == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Genre with ID {id} not found.");
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<GenreDto>(genre);
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
        public async Task<ActionResult<APIResponse>> CreateGenre([FromBody] GenreCreateDto createGenreDto)
        {
            try
            {
                if (await _genreService.GetByIdAsync(x => x.Name == createGenreDto.Name) != null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Genre already exists.");
                    return BadRequest(_response);
                }

                if (createGenreDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid genre data provided.");
                    return BadRequest(_response);
                }

                var genre = _mapper.Map<Genre>(createGenreDto);
                await _genreService.CreateAsync(genre);

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<GenreDto>(genre);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetGenre", new { id = genre.Id }, _response);
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
        public async Task<ActionResult<APIResponse>> DeleteGenre(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID provided.");
                    return BadRequest(_response);
                }

                var genre = await _genreService.GetByIdAsync(x => x.Id == id);

                if (genre == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Genre with ID {id} not found.");
                    return NotFound(_response);
                }

                await _genreService.DeleteAsync(genre);

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
        public async Task<ActionResult<APIResponse>> UpdateGenre(int id, GenreUpdateDto updatedGenre)
        {
            try
            { 
                if (id <= 0 || updatedGenre == null || id != updatedGenre.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID or genre data provided.");
                    return BadRequest(_response);
                }

                var genre = _mapper.Map<Genre>(updatedGenre);

                await _genreService.UpdateAsync(genre);

                _response.Result = _mapper.Map<GenreDto>(genre);
                _response.StatusCode = HttpStatusCode.OK;
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
    }
}