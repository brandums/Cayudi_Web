using AutoMapper;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DTO;
using EPlatformWebApp.Model;
using EPlatformWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace EPlatformWebApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CourseController : ControllerBase
    {
        private readonly Repository<Course> _repository;
        private readonly IMapper _mapper;
        protected APIResponse _responce;


        public CourseController(Repository<Course> repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _responce = new();
        }

        // GET: api/<CourseController>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                IEnumerable<Course> list = await _repository.getAll();

                if (list.Count() == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }
                else 
                {
                    foreach (Course course in list)
                    {
                        course.ImagePath = await FilesHandler.loadFileToArray(course.ImagePath);
                    }
                }

                _responce.Result = _mapper.Map<IEnumerable<CourseDTO>>(list);
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

        // GET api/<CourseController>/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                Course course = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (course == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }
                else 
                {
                    course.BannerPath = await FilesHandler.loadFileToArray(course.BannerPath);
                    course.ImagePath = await FilesHandler.loadFileToArray(course.ImagePath);
                }

                _responce.Result = _mapper.Map<CourseDTO>(course);
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

        // GET api/<CourseController>/5
        [HttpGet("getBySearch/{text}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetBySearch(string text)
        {
            try
            {
                if (text == "")
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                var courses = await _repository.getAllBy(O => O.Tittle.Contains(text));

                if (courses == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<IEnumerable<CourseDTO>>(courses);
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

        // GET api/<CourseController>/"category"
        [HttpGet("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetByCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                List<Course> courses = await _repository.getAllBy(c => c.CourseCategoryID == id);

                if (courses.Count == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }
                else
                {
                    foreach (Course course in courses)
                    {
                        course.ImagePath = await FilesHandler.loadFileToArray(course.ImagePath);
                    }
                }

                _responce.Result = _mapper.Map<List<CourseDTO>>(courses);
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

        // GET api/<CourseController>/"date"
        [HttpGet("date/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllByDate(DateTime date)
        {
            try
            {
                if (date == DateTime.MinValue)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                List<Course> courses = await _repository.getAllBy(c => c.StartDate.Month == date.Month);

                if (courses.Count == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<List<CourseDTO>>(courses);
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


        // POST api/<CourseController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create(CourseCreationDTO courseDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                if (courseDTO == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                Course course = _mapper.Map<Course>(courseDTO);

                await _repository.create(course);

                FilesHandler.CreteSourceFolders((int) FilePathHelper.SourceType.Course, course.ID);
                _responce.Result = _mapper.Map<CourseDTO>(course);
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
        public async Task<ActionResult<APIResponse>> PatchCourse(int id, JsonPatchDocument<Course> coursePatch)
        {
            try
            {
                if (coursePatch == null || id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                Course course = await _repository.getFirstOrDefaultBy(O => O.ID == id, tracked: true);

                if (course == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                coursePatch.ApplyTo(course, ModelState);

                await _repository.update(course);

                _responce.Result = _mapper.Map<CourseCreationDTO>(course);
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


        // DELETE api/<CourseController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                Course course = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (course == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                await _repository.Remove(course);
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

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNextMonthsCourses()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime limitDateTime = currentDate.AddMonths(12);


                List<Course> courses = await _repository.getAllBy(c => c.StartDate > currentDate && c.StartDate < limitDateTime);

                if (courses.Count == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<List<CourseDTO>>(courses);
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
