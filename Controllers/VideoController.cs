using AutoMapper;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DTO;
using EPlatformWebApp.Model;
using EPlatformWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EPlatformWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VideoController : ControllerBase
    {
        private readonly Repository<Video> _repository;
        private readonly IMapper _mapper;
        protected APIResponse _responce;


        public VideoController(Repository<Video> vRepository, IMapper mapper)
        {
            _repository = vRepository;
            _mapper = mapper;
            _responce = new();
        }

        // GET: api/<VideoController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                IEnumerable<Video> videoList = await _repository.getAll();

                if (videoList.Count() == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<IEnumerable<VideoDTO>>(videoList);
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

        // GET api/<VideoController>/5
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

                var video = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (video == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<VideoDTO>(video);
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

        [HttpGet("[action]/{courseId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetByCourse(int courseId)
        {
            try
            {
                if (courseId == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                IEnumerable<Video> videoList = await _repository.getAllBy(O => O.CourseID == courseId);

                if (videoList.Count() == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<IEnumerable<VideoPlayerFormatDTO>>(videoList);
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

        // POST api/<VideoController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] VideoDTO videoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                if (videoDto == null) 
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                Video video = _mapper.Map<Video>(videoDto);


                await _repository.create(video);
                video.Source = URLHelper.GetServiceURL(video.ID, 1);
                _responce.Result = _mapper.Map<VideoDTO>(video);
                _responce.StatusCode = HttpStatusCode.OK;
                return Ok(_responce);
            }
            catch (Exception ex )
            {
                _responce.isSucessfull = false;
                _responce.errorMessages = new List<string>() { ex.ToString() };
            }
            return _responce;
        }

        [HttpPatch("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Put(int id, JsonPatchDocument<VideoUpdateDTO> videoDtoUpdated)
        {
            try
            {

                if (videoDtoUpdated == null || id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                Video video = await _repository.getFirstOrDefaultBy(O => O.ID == id, tracked:false);

                if (video == null) 
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                VideoUpdateDTO vi = _mapper.Map<VideoUpdateDTO>(video);
                videoDtoUpdated.ApplyTo(vi, ModelState);
                await _repository.update(_mapper.Map<Video>(vi));
                _responce.Result = _mapper.Map<VideoUpdateDTO>(vi);
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

        // DELETE api/<VideoController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                Video video = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (video == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                if (await FilesHandler.deleteFile(video.Path))
                {
                    await _repository.Remove(video);
                    _responce.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_responce);
                }
                else
                {
                    _responce.isSucessfull = false;
                    _responce.userMessage = "Could not remove file";
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _responce.isSucessfull = false;
                _responce.errorMessages = new List<string>() { ex.ToString() };
            }

            return _responce;
        }

        [HttpGet]
        [Route("[action]/{videoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> PlayVideo(int videoId)
        {
            Video video = null;
            try
            {
                if (videoId == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return Results.BadRequest();
                }

                video = await _repository.getFirstOrDefaultBy(O => O.ID == videoId);

                if (video == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return Results.BadRequest();
                }
                string path = video.Path;

                if (String.IsNullOrEmpty(path))
                    return Results.BadRequest();

                var fileName = "";

                var filestream = System.IO.File.OpenRead(path);
                return Results.File(filestream, contentType: "video/mp4", fileDownloadName: fileName, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return Results.BadRequest();
            }
        }
    }
}
