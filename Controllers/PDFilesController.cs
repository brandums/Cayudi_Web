using AutoMapper;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DTO;
using EPlatformWebApp.Model;
using EPlatformWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;

namespace EPlatformWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PDFilesController : ControllerBase
    {
        private readonly Repository<PDFFile> _repository;
        private readonly IMapper _mapper;
        protected APIResponse _responce;


        public PDFilesController(Repository<PDFFile> vRepository, IMapper mapper)
        {
            _repository = vRepository;
            _mapper = mapper;
            _responce = new();
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

                IEnumerable<PDFFile> list = await _repository.getAllBy(O => O.CourseID == courseId);

                if (list.Count() == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<IEnumerable<PDFileDTO>>(list);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] PDFileDTO pdfFileDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                if (pdfFileDTO == null) 
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }
                PDFFile pdfFile = _mapper.Map<PDFFile>(pdfFileDTO);


                await _repository.create(pdfFile);
                pdfFile.Source = URLHelper.GetServiceURL(pdfFile.ID, 3 );
                _responce.Result = _mapper.Map<PDFileDTO>(pdfFile);
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

        [HttpPut("[action]/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> switchReadOnly(int id)
        {
            try
            {

                if (id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                PDFFile pdfFileOb = await _repository.getFirstOrDefaultBy(O => O.ID == id, tracked: false);

                if (pdfFileOb == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                pdfFileOb.ReadOnly = !pdfFileOb.ReadOnly;
                await _repository.update(pdfFileOb);
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                PDFFile pdfFile = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (pdfFile == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                if (await FilesHandler.deleteFile(pdfFile.Path))
                {
                    await _repository.Remove(pdfFile);
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
        [Route("[action]/{pdfFileID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> ReadPdf(int pdfFileID)
        {
            PDFFile pdfFile = null;
            try
            {
                if (pdfFileID == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return Results.BadRequest();
                }

                pdfFile = await _repository.getFirstOrDefaultBy(O => O.ID == pdfFileID);

                if (pdfFile == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return Results.BadRequest();
                }
                string path = pdfFile.Path;

                if (String.IsNullOrEmpty(path))
                    return Results.BadRequest();

                var fileName = "";

                var filestream = System.IO.File.OpenRead(path);
                return Results.File(filestream, contentType: "application/pdf", fileDownloadName: fileName, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return Results.BadRequest();
            }
        }
    }
}
