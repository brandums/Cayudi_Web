using AutoMapper;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DTO;
using EPlatformWebApp.Model;
using EPlatformWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EPlatformWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserFavoriteCourseController : ControllerBase
    {
        private readonly Repository<UserFavoriteCourse> _repository;
        private readonly IMapper _mapper;
        protected APIResponse _responce;


        public UserFavoriteCourseController(Repository<UserFavoriteCourse> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _responce = new();
        }

        // GET api/<UserFavoriteCourse>/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetByUser(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                List<UserFavoriteCourse> userFavoriteCourse = await _repository.getAllBy(O => O.UserID == id);

                if (userFavoriteCourse == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<List<UserFavoriteCourseDTO>>(userFavoriteCourse);
                _responce.StatusCode = HttpStatusCode.OK;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.isSucessfull = false;
                _responce.errorMessages = new List<string>() { ex.ToString() };
            }

            return _responce;
        }

        // GET api/<UserFavoriteCourse>/5,5
        [HttpGet("{userID:int}/{courseID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetByCourse(int userID, int courseID)
        {
            try
            {
                if (userID == 0 || courseID == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                UserFavoriteCourse userFavoriteCourse = await _repository.getFirstOrDefaultBy(O => O.UserID == userID && O.CourseID == courseID);

                if (userFavoriteCourse == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<UserFavoriteCourseDTO>(userFavoriteCourse);
                _responce.StatusCode = HttpStatusCode.OK;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.isSucessfull = false;
                _responce.errorMessages = new List<string>() { ex.ToString() };
            }

            return _responce;
        }


        // POST api/<UserFavoriteCourse>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] UserFavoriteCourseCreationDTO userFavoriteCourseDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                if (userFavoriteCourseDTO == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                UserFavoriteCourse userFavoriteCourse = _mapper.Map<UserFavoriteCourse>(userFavoriteCourseDTO);

                await _repository.create(userFavoriteCourse);
                _responce.Result = _mapper.Map<UserFavoriteCourseDTO>(userFavoriteCourse);
                _responce.StatusCode = HttpStatusCode.OK;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.isSucessfull = false;
                _responce.errorMessages = new List<string>() { ex.ToString() };
            }
            return _responce;
        }

        // DELETE api/<UserFavoriteCourse>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                UserFavoriteCourse userFavoriteCourse = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (userFavoriteCourse == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                await _repository.Remove(userFavoriteCourse);
                _responce.StatusCode = HttpStatusCode.NoContent;
                return Ok(_responce);

            }
            catch (Exception ex)
            {
                _responce.isSucessfull = false;
                _responce.errorMessages = new List<string>() { ex.ToString() };
            }

            return _responce;
        }
    }
}