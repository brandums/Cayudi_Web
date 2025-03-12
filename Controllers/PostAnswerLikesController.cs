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
    [AllowAnonymous]
    public class PostAnswerLikesController : ControllerBase
    {
        private readonly Repository<PostAnswerLikes> _repository;
        private readonly IMapper _mapper;
        protected APIResponse _responce;


        public PostAnswerLikesController(Repository<PostAnswerLikes> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _responce = new();
        }

        // GET api/<PostAnswerLikes>/5
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

                List<PostAnswerLikes> postAnswerLikes = await _repository.getAllBy(O => O.BlogPostAnswerID == id);

                if (postAnswerLikes == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<List<PostAnswerLikesDTO>>(postAnswerLikes);
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


        // POST api/<PostAnswerLikes>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] PostAnswerLikesCreationDTO postAnswerLikeDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                if (postAnswerLikeDTO == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                PostAnswerLikes postAnswerLikes = _mapper.Map<PostAnswerLikes>(postAnswerLikeDTO);

                await _repository.create(postAnswerLikes);
                _responce.Result = _mapper.Map<PostAnswerLikesDTO>(postAnswerLikes);
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

        // DELETE api/<PostLike>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                PostAnswerLikes postLike = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (postLike == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                await _repository.Remove(postLike);
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