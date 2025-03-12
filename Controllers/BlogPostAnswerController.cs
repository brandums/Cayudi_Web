using AutoMapper;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DTO;
using EPlatformWebApp.Model;
using EPlatformWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace EPlatformWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [AllowAnonymous]
    public class BlogPostAnswerController : ControllerBase
    {
        private readonly Repository<BlogPostAnswer> _repository;
        private readonly IMapper _mapper;
        protected APIResponse _responce;


        public BlogPostAnswerController(Repository<BlogPostAnswer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _responce = new();
        }

        // GET: api/<BlogPostAnswerController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                IEnumerable<BlogPostAnswer> list = await _repository.getAll();

                if (list.Count() == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<IEnumerable<BlogPostAnswerDTO>>(list);
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

        // GET api/<BlogPostAnswerController>/5
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

                IEnumerable<BlogPostAnswer> blogPostAnswer = await _repository.getAllBy(O => O.BlogPostID == id);

                if (blogPostAnswer.Count() == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<IEnumerable<BlogPostAnswerDTO>>(blogPostAnswer);
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

        // POST api/<BlogPostAnswerController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] BlogPostAnswerCreationDTO blogPostDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                if (blogPostDTO == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                BlogPostAnswer blogPost = _mapper.Map<BlogPostAnswer>(blogPostDTO);


                await _repository.create(blogPost);
                _responce.Result = _mapper.Map<BlogPostAnswerDTO>(blogPost);
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