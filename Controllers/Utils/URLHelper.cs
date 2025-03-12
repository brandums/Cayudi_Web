namespace EPlatformWebApp.Controllers.Utils
{
    public static class URLHelper
    {
        //public static string _APISERVICE { get; set; } = "https://localhost:7079/";
        public static string _APISERVICE { get; set; } = "https://cayudi-001-site1.anytempurl.com/";

        public static string _VIDEOSTREAMENDPOINT { get; set; } = "api/Video/PlayVideo/";
        public static string _PDFREADDERENDPOINT { get; set; } = "api/PDFiles/ReadPdf/";

        public static string GetServiceURL(int id, int source)
        {
            string endPoint = "";
            switch (source)
            {
                case 1:
                    endPoint = _VIDEOSTREAMENDPOINT;
                    break;
                case 2:
                    endPoint = "";
                    break;
                case 3:
                    endPoint = _PDFREADDERENDPOINT;
                    break;
                case 4:
                    endPoint = "";
                    break;
            }

            return String.Format("{0}{1}{2}", _APISERVICE, endPoint, id.ToString());

        }       
    }
}
