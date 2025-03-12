using System.Net;

namespace EPlatformWebApp.Controllers.Utils
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool isSucessfull { get; set; } = true;

        public List<string> errorMessages { get; set; }

        public String userMessage { get; set; }

        public object Result { get; set; }

        public String token { get; set; }
    }
}
