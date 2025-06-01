using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using MovieDatabase.Repository.IRepository;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MovieDatabase.Controllers
{
    [ApiController]
    [Route("api/GenreAPI")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class GenresController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IGenreRepository _genreDb;
        private readonly IMapper _mapper;

        public GenresController(IGenreRepository genreDb, IMapper mapper)
        {
            _genreDb = genreDb;
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
                var genres = await _genreDb.GetAllAsync();
                _response.Result = _mapper.Map<List<GenreDto>>(genres);
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
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID provided.");
                    return BadRequest(_response);
                }

                var genre = await _genreDb.GetByIdAsync(x => x.Id == id, tracked: false);

                if (genre == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Genre with ID {id} not found.");
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<GenreDto>(genre);
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
        public async Task<ActionResult<APIResponse>> CreateGenre([FromBody] GenreCreateDto createGenreDto)
        {
            try
            {
                if (await _genreDb.GetByIdAsync(x => x.Name == createGenreDto.Name) != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Genre already exists.");
                    return BadRequest(_response);
                }

                if (createGenreDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid genre data provided.");
                    return BadRequest(_response);
                }

                var genre = _mapper.Map<Genre>(createGenreDto);
                await _genreDb.CreateAsync(genre);

                _response.Result = _mapper.Map<GenreDto>(genre);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetGenre", new { id = genre.Id }, _response);
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
        public async Task<ActionResult<APIResponse>> DeleteGenre(int id)
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

                var genre = await _genreDb.GetByIdAsync(x => x.Id == id);

                if (genre == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Genre with ID {id} not found.");
                    return NotFound(_response);
                }

                await _genreDb.RemoveAsync(genre);

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
        public async Task<ActionResult<APIResponse>> UpdateGenre(int id, GenreUpdateDto updatedGenre)
        {
            try
            { 
                if (id <= 0 || updatedGenre == null || id != updatedGenre.Id)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Errors.Add("Invalid ID or genre data provided.");
                    return BadRequest(_response);
                }

                Genre genre = _mapper.Map<Genre>(updatedGenre);

                await _genreDb.UpdateAsync(genre);

                _response.Result = _mapper.Map<GenreDto>(genre);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialGenre")]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status500InternalServerError)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(APIResponseOkExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(APIResponseBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(APIResponseNotFoundExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(APIResponseInternalServerErrorExample))]
        [Consumes("application/json-patch+json")]
        public async Task<ActionResult<APIResponse>> UpdatePartialGenre(int id, [FromBody] JsonPatchDocument<GenreUpdateDto> patchDto)
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
                var genre = await _genreDb.GetByIdAsync(x => x.Id == id, tracked: false);

                if (genre == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Errors.Add($"Genre with ID {id} not found.");
                    return NotFound(_response);
                }

                var genreUpdateDto = _mapper.Map<GenreUpdateDto>(genre);
                patchDto.ApplyTo(genreUpdateDto, (error) =>
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

                genre = _mapper.Map<Genre>(genreUpdateDto);

                await _genreDb.UpdateAsync(genre);

                _response.Result = _mapper.Map<GenreDto>(genre);
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