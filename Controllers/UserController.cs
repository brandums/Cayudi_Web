using AutoMapper;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.DTO;
using EPlatformWebApp.Model;
using EPlatformWebApp.Model.Utils;
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
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly Repository<User> _repository;
        private readonly IMapper _mapper;
        protected APIResponse _responce;
        private JWTHandler _tokenHandler;
        private readonly IEmailSender _emailSender;
        private readonly Repository<UserImage> _imageRepository;



        public UserController(Repository<User> vRepository, IMapper mapper, JWTHandler tokenHandler, IEmailSender emailSender, Repository<UserImage> imageRepository)
        {
            _repository = vRepository;
            _mapper = mapper;
            _responce = new();
            _tokenHandler = tokenHandler;
            _emailSender = emailSender;
            _imageRepository = imageRepository;
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Get()
        {
            try
            {
                IEnumerable<User> UserList = await _repository.getAll();

                if (UserList.Count() == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                _responce.Result = _mapper.Map<IEnumerable<UserDTO>>(UserList);
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

        // GET api/<UserController>/5
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

                User user = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (user == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                UserDTO userDTO = _mapper.Map<UserDTO>(user);

                if (!String.IsNullOrEmpty(user.LoginImagePath))
                {
                    string imageIcon = await FilesHandler.loadFileToArray(user.LoginImagePath);
                    userDTO.ProfImage = imageIcon;
                }
                            

                _responce.Result = userDTO;
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


        // POST api/<UserController>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] UserRegisterDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                if (userDTO == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                User usr = await _repository.getFirstOrDefaultBy(O => O.Email == userDTO.Email);

                if (usr != null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.userMessage = "El email ya esta en uso";
                    return BadRequest(_responce);
                }

                User user = _mapper.Map<User>(userDTO);

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                await _repository.create(user);

                var confirmationToken = await GenerateConfirmationToken(user);
                await SendConfirmationEmail(user.Email, confirmationToken);

                FilesHandler.CreteSourceFolders((int)FilePathHelper.SourceType.User, user.ID);
                _responce.Result = _mapper.Map<UserRegisterDTO>(user);
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

        private async Task<string> GenerateConfirmationToken(User user)
        {
            string confirmationToken = Guid.NewGuid().ToString();
            user.ConfirmationToken = confirmationToken;
            await _repository.update(user);

            return confirmationToken;
        }

        private async Task SendConfirmationEmail(string userEmail, string confirmationToken)
        {
            //string confirmationLink = $"https://localhost:7079/api/User/{userEmail}/{confirmationToken}";
            string confirmationLink = $"https://cayudi-001-site1.anytempurl.com/api/User/{userEmail}/{confirmationToken}";
            string emailSubject = "Verficación  de cuenta Cayudi";
            string emailBody = $"Por favor, haz clic en el siguiente enlace para confirmar tu cuenta: <a href=\"{confirmationLink}\">Confirmar cuenta</a>";

            await _emailSender.SendEmailAsync(userEmail, emailSubject, emailBody);
        }

        [HttpGet("{userEmail}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> SendChangePasswordEmail(string userEmail)
        {
            try
            {
                User user = await _repository.getFirstOrDefaultBy(O => O.Email == userEmail);

                if (user != null)
                {
                    string confirmationToken = await GenerateConfirmationToken(user);
                    //string confirmationLink = $"https://localhost:7079/api/User/changePassword/{user.ID}/{confirmationToken}";
                    string confirmationLink = $"https://cayudi-001-site1.anytempurl.com/api/User/changePassword/{user.ID}/{confirmationToken}";
                    string emailSubject = "Olvidaste tu contraseña?";
                    string emailBody = $"Por favor, haz clic en el siguiente enlace para restablecer la contraseña de su cuenta: <a href=\"{confirmationLink}\">Cambiar contraseña</a>";

                    await _emailSender.SendEmailAsync(userEmail, emailSubject, emailBody);
                }
                else
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.userMessage = "No se encontro una cuenta enlazada a ese correo";
                    return BadRequest(_responce);
                }

                return Ok("Correo de cambio de contraseña enviado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al enviar el correo de cambio de contraseña." + ex);
            }
        }

        [HttpGet("{email}/{token}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> ConfirmEmail(string email, string token)
        {
            var user = await _repository.getFirstOrDefaultBy(u => u.Email == email && u.ConfirmationToken == token);

            if (user == null)
            {
                return BadRequest("La confirmación de correo electrónico no pudo ser completada.");
            }

            user.IsActivated = true;
            user.ConfirmationToken = null;
            await _repository.update(user);

            //return Redirect("https://localhost:44451/");
            return Redirect("https://cayudi-001-site1.anytempurl.com");
        }


        [HttpGet("changePassword/{id}/{token}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> RedirectChangePassword(int id, string token)
        {
            var user = await _repository.getFirstOrDefaultBy(u => u.ID == id && u.ConfirmationToken == token);

            if (user == null)
            {
                return BadRequest("La confirmación de correo electrónico no pudo ser completada.");
            }

            //return Redirect($"https://localhost:44451/change-password/{id}/{token}");
            return Redirect($"https://cayudi-001-site1.anytempurl.com/change-password/{id}/{token}");
        }

        // POST api/<UserController>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Login([FromBody] UserLoginDTO userDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                if (userDTO == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                User usr = await _repository.getFirstOrDefaultBy(O => O.Email == userDTO.Email);

                if (usr == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.userMessage = "El usuario no existe";
                    return BadRequest(_responce);
                }

                if (usr.IsActivated != true)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.userMessage = "Su cuenta no esta activada";
                    return BadRequest(_responce);
                }

                if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, usr.Password))
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    _responce.userMessage = "Contraseña incorrecta";
                    return BadRequest(_responce);
                }

                _responce.token = _tokenHandler.createJWT(usr);
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
        public async Task<ActionResult<APIResponse>> Put(int id, JsonPatchDocument<User> userDtoUpdated)
        {
            try
            {
                if (userDtoUpdated == null || id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                User user = await _repository.getFirstOrDefaultBy(O => O.ID == id, tracked: false);

                if (user == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                userDtoUpdated.ApplyTo(user, ModelState);
                await _repository.update(_mapper.Map<User>(user));
                _responce.Result = _mapper.Map<UserDTO>(user);
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

        //UpdatePassword
        [HttpPatch("[action]/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Password(int id, [FromBody] JsonPatchDocument<User> patchPassword)
        {
            try
            {
                if (patchPassword == null || id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                User user = await _repository.getFirstOrDefaultBy(O => O.ID == id, tracked: false);

                if (user == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                // Aplicar la operación de reemplazo al objeto User
                patchPassword.ApplyTo(user, ModelState);

                if (!ModelState.IsValid)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.ConfirmationToken = null;

                await _repository.update(user);
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


        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                User user = await _repository.getFirstOrDefaultBy(O => O.ID == id);

                if (user == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                await _repository.Remove(user);
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

        [HttpGet("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetImagesByUser(int id)
        {
            try
            {
                if (id == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_responce);
                }

                IEnumerable<UserImage> list = await _imageRepository.getAllBy(O => O.UserID == id);

                if (list.Count() == 0)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }
                else
                {
                    foreach (UserImage image in list)
                    {
                        image.Source = await FilesHandler.loadFileToArray(image.Path);
                    }
                }
                
                _responce.Result = _mapper.Map<IEnumerable<UserImagesDTO>>(list);
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

        // DELETE api/<UserController>/5
        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteImage(int id)
        {
            try
            {
                UserImage uImage = await _imageRepository.getFirstOrDefaultBy(O => O.ID == id);

                if (uImage == null)
                {
                    _responce.isSucessfull = false;
                    _responce.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_responce);
                }

                if (await FilesHandler.deleteFile(uImage.Path))
                {
                    await _imageRepository.Remove(uImage);
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

    }

}
