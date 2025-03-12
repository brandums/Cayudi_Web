using AutoMapper;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DTO;
using EPlatformWebApp.Model;
using EPlatformWebApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Stripe;
using System.Linq.Expressions;
using System.Net;

namespace EPlatformWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesHandlerController : ControllerBase
    {
        protected APIResponse _responce;
        private readonly Repository<Video> _vRepository;
        private readonly Repository<User> _uRepository;
        private readonly Repository<PDFFile> _pdfRepository;
        private readonly Repository<UserImage> _uImageRepository;
        private readonly Repository<CourseCategory> _catRepo;
        private readonly Repository<Course> _courseRepo;
        private readonly Repository<Cert> _certRepo;




        public FilesHandlerController( Repository<Video> vRepository, Repository<User> uRepository,
                                       Repository<PDFFile> pdfRepository, Repository<UserImage> uImageRepository,
                                        Repository<CourseCategory> catRepo, Repository<Course> courseRepo,
                                        Repository<Cert> certRepo)
        {
            _responce = new();
            _vRepository = vRepository;
            _uRepository = uRepository;
            _pdfRepository = pdfRepository;
            _uImageRepository = uImageRepository;
            _catRepo = catRepo;
            _courseRepo = courseRepo;
            _certRepo = certRepo;
        }
              
        [HttpPut]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<APIResponse>> UpdateFile([FromForm] FileDataDTO fileData)
        {
            try
            {
                await FilesHandler.SaveFile(fileData);
                _responce.StatusCode = HttpStatusCode.OK;
                return Ok(_responce);
            }
            catch (Exception ex)
            {
                _responce.isSucessfull = false;
                _responce.errorMessages = new List<string>() { ex.ToString() };
            }
            return Ok(_responce);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestFormLimits(ValueCountLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<APIResponse>> upload([FromForm] FileDataDTO fileData)
        {
            try
            {
                bool fileSaved = await FilesHandler.SaveFile(fileData);

                if (fileSaved)
                {
                   await UpdateData(fileData);
                    _responce.StatusCode = HttpStatusCode.OK;
                    return Ok(_responce);
                }
                else 
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return BadRequest(_responce);
                }
            }
            catch (Exception ex)
            {
                _responce.isSucessfull = false;
                _responce.errorMessages = new List<string>() { ex.ToString() };
            }
            return Ok(_responce);
        }

        private async Task UpdateData(FileDataDTO fileData)
        {
            switch (int.Parse(fileData.SourceType)) {
                case ((int)FilePathHelper.SourceType.User):
                    await updateUserData(fileData);
                    break;
                case ((int)FilePathHelper.SourceType.Course):
                    await updateCourseData(fileData);
                    break;
                case ((int)FilePathHelper.SourceType.Category):
                    await updateCategoryData(fileData);
                    break;

            }            
        }

        private async Task updateCourseData(FileDataDTO fileData)
        {
            switch (int.Parse(fileData.CourseSource))
            {
                case 1:
                    await updateVideoData(fileData);
                    break;
                case 2:
                    await updateCourseAssetsData(fileData);
                    break;
                case 3:
                    await updatePDFileData(fileData);
                    break;
                case 4:
                    await updateCourseCertsData(fileData);
                    break;
            }
        }

        private async Task updateUserData(FileDataDTO fileData)
        {
            if (int.Parse(fileData.imageType) == (int)FilePathHelper.ImageType.UserProf)
            {
                User user = await _uRepository.getFirstOrDefaultBy(O => O.ID == int.Parse(fileData.SourceID));

                if (!String.IsNullOrEmpty(user.LoginImagePath))
                {
                    await FilesHandler.deleteFile(user.LoginImagePath);
                }
                user.LoginImagePath = String.Format(FilePathHelper.GetSourcePath(int.Parse(fileData.SourceID),
                int.Parse(fileData.SourceType),0, int.Parse(fileData.imageType)) + "\\{0}", fileData.File.FileName);
                await _uRepository.update(user);
            }
            else if (int.Parse(fileData.imageType) == (int)FilePathHelper.ImageType.UserCert)
            {
                UserImage uImage = new UserImage();
                uImage.UserID = int.Parse(fileData.SourceID);
                uImage.Path = String.Format(FilePathHelper.GetSourcePath(int.Parse(fileData.SourceID),
                int.Parse(fileData.SourceType),0, int.Parse(fileData.imageType)) + "\\{0}", fileData.File.FileName);
                uImage.Tittle = Path.GetFileNameWithoutExtension(fileData.File.FileName);
                await _uImageRepository.create(uImage);
            }
        }

        private async Task updateVideoData(FileDataDTO fileData)
        {
            Video vDTO = new Video(int.Parse(fileData.SourceID),
                                        Path.GetFileNameWithoutExtension(fileData.File.FileName),
                                        "",
                                        String.Format(FilePathHelper.GetSourcePath
                                        (int.Parse(fileData.SourceID), int.Parse(fileData.SourceType),
                                        int.Parse(fileData.CourseSource)) + "\\{0}", fileData.File.FileName));
            await _vRepository.create(vDTO);
            string videoURL = URLHelper.GetServiceURL(vDTO.ID, 1);
            vDTO.Source = videoURL;
            await _vRepository.update(vDTO);
        }

        private async Task updatePDFileData(FileDataDTO fileData)
        {
            PDFFile PDFile = new PDFFile(int.Parse(fileData.SourceID),
                                        Path.GetFileNameWithoutExtension(fileData.File.FileName),
                                        "",
                                        String.Format(FilePathHelper.GetSourcePath
                                        (int.Parse(fileData.SourceID), int.Parse(fileData.SourceType),
                                        int.Parse(fileData.CourseSource)) + "\\{0}", fileData.File.FileName));

            await _pdfRepository.create(PDFile);
            string fileURL = URLHelper.GetServiceURL(PDFile.ID, 3);
            PDFile.Source = fileURL;
            await _pdfRepository.update(PDFile);
        }
        private async Task updateCourseAssetsData(FileDataDTO fileData)
        {
            Course course = await _courseRepo.getFirstOrDefaultBy(O => O.ID == int.Parse(fileData.SourceID));
            string ImagePath = String.Format(FilePathHelper.GetSourcePath(int.Parse(fileData.SourceID),
                int.Parse(fileData.SourceType), int.Parse(fileData.CourseSource), int.Parse(fileData.imageType)) + "\\{0}", fileData.File.FileName);

            if (int.Parse(fileData.imageType) == (int)FilePathHelper.ImageType.CourseBanner)
            { 
                course.BannerPath = ImagePath;
            }
            else if (int.Parse(fileData.imageType) == (int)FilePathHelper.ImageType.CourseImg)
            {
                course.ImagePath = ImagePath;
            }
            await _courseRepo.update(course);
        }

        private async Task updateCourseCertsData(FileDataDTO fileData)
        {
            Cert cert = await _certRepo.getFirstOrDefaultBy(O => O.ID == int.Parse(fileData.SourceID));
            string ImagePath = String.Format(FilePathHelper.GetSourcePath(int.Parse(fileData.SourceID),
               int.Parse(fileData.SourceType), int.Parse(fileData.CourseSource), int.Parse(fileData.imageType)) + "\\{0}", fileData.File.FileName);
            cert.Path = ImagePath;
            await _certRepo.update(cert);
        }
        private async Task updateCategoryData(FileDataDTO fileData)
        { 
            CourseCategory cCat =  await _catRepo.getFirstOrDefaultBy(O => O.ID == int.Parse(fileData.SourceID));
            cCat.ImagePath = String.Format(FilePathHelper.GetSourcePath(int.Parse(fileData.SourceID),
                int.Parse(fileData.SourceType), 0, int.Parse(fileData.imageType)) + "\\{0}", fileData.File.FileName);
            await _catRepo.update(cCat);
        }
    }
}
