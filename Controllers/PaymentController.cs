using EPlatformWebApp.DTO;
using EPlatformWebApp.Model.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using EPlatformWebApp.Controllers.Utils;
using EPlatformWebApp.Model;
using System.Net;
using AutoMapper;
using EPlatformWebApp.Repository;
using NuGet.Protocol.Core.Types;

namespace EPlatformWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly Repository<UserCourse> _repository;
        private readonly IMapper _mapper;

        public PaymentController(Repository<UserCourse> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarDeuda([FromBody] PaymentRequestDTO paymentRequest)
        {
            try
            {
                var parametros = new
                {
                    appkey = "11bb10ce-68ba-4af1-8eb7-4e6624fed729",
                    email_cliente = paymentRequest.Email,
                    identificador = $"{paymentRequest.UserID}{paymentRequest.CourseID}{DateTime.Now.ToString("yyyyMMddHHmm")}",
                    callback_url = $"https://localhost:7079/api/Payment/registrar_pago?UserID={paymentRequest.UserID}&CourseID={paymentRequest.CourseID}&IsOnline={paymentRequest.IsOnline}&IsOppened={paymentRequest.IsOppened}",
                    url_retorno = $"https://localhost:44451/thanks-page/{paymentRequest.CourseID}",
                    descripcion = "Cursos Online Cayudi",
                    nit = paymentRequest.Nit,
                    razon_social = paymentRequest.RazonSocial,
                    fecha_vencimiento = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm"),
                    lineas_detalle_deuda = new[]
                    {
                        new { concepto= paymentRequest.Title, cantidad=1, costo_unitario= paymentRequest.Price, descuento_unitario=0 },
                    },
                    lineas_metadatos = new[]
                    {
                        new { nombre="Tienda", dato="Cayudi.com" }
                    }
                };

                string jsonData = JsonConvert.SerializeObject(parametros);
                string apiUrl = "https://api.libelula.bo/rest/deuda/registrar";

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return Ok(responseContent);
                    }
                    else
                    {
                        return BadRequest($"Código de estado: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("registrar_pago")]
        public async Task<IActionResult> RegistrarPagoExitoso([FromQuery] UserCourseCreationDTO userCourseDTO)
        {
            try
            {
                if (!ModelState.IsValid || userCourseDTO == null)
                {
                    return BadRequest();
                }

                UserCourse userCourse = _mapper.Map<UserCourse>(userCourseDTO);
                await _repository.create(userCourse);

                return Ok(new { success = true, message = "El pago se ha registrado con éxito" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}