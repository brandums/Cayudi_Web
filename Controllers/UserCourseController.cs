using AutoMapper;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DTO;
using EPlatformWebApp.Model;
using EPlatformWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Net;

namespace EPlatformWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AllowAnonymous]
    public class UserCourseController : ControllerBase
    {
        private readonly Repository<UserCourse> _repository;
        private readonly Repository<Course> _repositoryCourse;
        private readonly Repository<User> _repositoryUser;
        private readonly IMapper _mapper;
        protected APIResponse _responce;


        public UserCourseController(Repository<UserCourse> repository, Repository<Course> repositoryCourse, Repository<User> repositoryUser, IMapper mapper)
        {
            _repository = repository;
            _repositoryCourse = repositoryCourse;
            _repositoryUser = repositoryUser;
            _mapper = mapper;
            _responce = new();
        }

        [HttpGet("{userID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllByUserID(int userID)
        {
            try
            {
                if (userID == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                IEnumerable<UserCourse> userCourses = await _repository.getAllBy(O => O.UserID == userID);
                foreach (var userCourse in userCourses)
                {
                    userCourse.Course = await _repositoryCourse.getFirstOrDefaultBy(c => c.ID == userCourse.CourseID);
                }

                if (userCourses == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map< IEnumerable<UserCourseDTO>>(userCourses);
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

        [HttpGet("getAllUsersByCourseID/{courseID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllUsers(int courseID)
        {
            try
            {
                if (courseID == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                IEnumerable<UserCourse> userCourse = await _repository.getAllBy(O => O.CourseID == courseID);

                if (userCourse == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                IEnumerable<UserCourseDTO> userCourseDTOs = _mapper.Map<IEnumerable<UserCourseDTO>>(userCourse);

                foreach (var userCourseDTO in userCourseDTOs)
                {
                    User user = await _repositoryUser.getFirstOrDefaultBy(O => O.ID == userCourseDTO.UserID);
                    userCourseDTO.User = _mapper.Map<UserDTO>(user);
                }

                _responce.Result = userCourseDTOs.ToList();
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

        [HttpGet("{courseID:int}/{userID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Get(int courseID, int userID)
        {
            try
            {
                if (courseID == 0 || userID == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                UserCourse userCourse = await _repository.getFirstOrDefaultBy(O => O.CourseID == courseID && O.UserID == userID);

                if (userCourse == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<UserCourseDTO>(userCourse);
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

        // Obtener el rating de cada curso por id
        [HttpGet("getRating/{courseID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetRating(int courseID)
        {
            if (courseID <= 0)
            {
                _responce.isSucessfull = false;
                _responce.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_responce);
            }

            IEnumerable<UserCourse> userCourses = await _repository.getAllBy(O => O.CourseID == courseID && O.Rating.HasValue);

            if (!userCourses.Any())
            {
                return NotFound("No se encontraron calificaciones para este curso.");
            }

            try
            {
                double totalRating = userCourses.Average(uc => uc.Rating.Value);
                double normalizedRating = Math.Min(totalRating, 5.0);

                CourseRatingResponse courseRating = new CourseRatingResponse
                {
                    Rating = normalizedRating,
                    TotalVotes = userCourses.Count()
                };

                _responce.Result = courseRating;
                _responce.StatusCode = HttpStatusCode.OK;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Se produjo un error al calcular la calificación del curso.");
            }
        }


        // POST api/<UserCourseController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] UserCourseCreationDTO userCourseDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                if (userCourseDTO == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                UserCourse userCourse = _mapper.Map<UserCourse>(userCourseDTO);

                await _repository.create(userCourse);
                _responce.Result = _mapper.Map<UserCourseDTO>(userCourse);
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

        [HttpGet("GetEndTime/{userCourseId:int}")]
        public async Task<ActionResult<APIResponse>> GetEndTime(int userCourseId) 
        {
            try
            {
                if (userCourseId == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                UserCourse userCourse = await _repository.getFirstOrDefaultBy(O => O.ID == userCourseId);
                Course course = await _repositoryCourse.getFirstOrDefaultBy(O => O.ID == userCourse.CourseID);
                userCourse.EndTime = userCourse.StartTime.AddDays(course.DurationTime ?? 0);

                if (userCourse == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                await _repository.update(userCourse);
                _responce.Result = _mapper.Map<UserCourseDTO>(userCourse);
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

        [HttpGet("closeCourse/{userCourseId:int}")]
        public async Task<ActionResult<APIResponse>> CloseCourse(int userCourseId)
        {
            try
            {
                if (userCourseId == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                UserCourse userCourse = await _repository.getFirstOrDefaultBy(O => O.ID == userCourseId);
                userCourse.IsOppened = false;

                if (userCourse == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                await _repository.update(userCourse);
                _responce.Result = _mapper.Map<UserCourseDTO>(userCourse);
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

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Put(int id, JsonPatchDocument<UserCourseDTO> userCourseDtoUpdated)
        {
            try
            {
                if (userCourseDtoUpdated == null || id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                UserCourse userCourse = await _repository.getFirstOrDefaultBy(O => O.ID == id, tracked: false);

                if (userCourse == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                UserCourseDTO originalDto = _mapper.Map<UserCourseDTO>(userCourse);
                userCourseDtoUpdated.ApplyTo(originalDto, ModelState);
                UserCourse updatedUserCourse = _mapper.Map<UserCourse>(originalDto);

                await _repository.update(updatedUserCourse);
                _responce.Result = _mapper.Map<UserCourseDTO>(originalDto);
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
    }
}
