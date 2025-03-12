namespace EPlatformWebApp.Controllers.Utils
{
    public static class FilePathHelper
    {
        #region "Constants"
        public enum SourceType
        {
            User = 1,
            Course = 2,
            Category = 3
        }

        public enum CourseSourcePath
        {
            Videos = 1,
            Assets = 2,
            Files = 3,
            Certs = 4
        }

        public enum ImageType
        {
            UserProf = 1,
            UserCert = 2,
            CourseBanner = 3,
            CourseImg= 4,
            CategoryImge = 5
        }

        public enum fileType
        {
            img = 1,
            video = 2,
            pdf = 3
        }

        public static string _APPDATAROUTE  = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EP-Data");

        public static string _ROUTEBUILDER  = "{0}\\{1}\\{2}\\{3}";




        #endregion


        #region "Static methods"
        public static string GetSourcePath(int sourceID, int sourceType, int courseSource = 0, int imgType = 0)
        {
            string path = String.Empty;
            string fileFolder = String.Empty;
            string contentRoute = String.Empty;

            switch (sourceType)
            {
                case ((int)SourceType.User):
                    contentRoute = "USER";
                    switch (imgType)
                    {
                        case 1:
                            fileFolder = "ASSETS";
                            break;
                        case 2:
                            fileFolder = "DOCS";
                            break;
                        default:
                            fileFolder = String.Empty;
                            break;
                    }
                    break;
                case ((int)SourceType.Course):
                    contentRoute = "COURSE";
                    switch (courseSource)
                    {
                        case 1:
                            fileFolder = "VIDEOS";
                            break;
                        case 2:
                            fileFolder = "ASSETS";
                            break;
                        case 3:
                            fileFolder = "FILES";
                            break;
                        case 4:
                            fileFolder = "CERTS";
                            break;
                        default:
                            fileFolder = String.Empty;
                            break;
                    }
                    break;
                case ((int)SourceType.Category):
                    contentRoute = "CATEGORY";
                    fileFolder = "ASSETS";
                    break;
            }
                        

            return String.Format(_ROUTEBUILDER, _APPDATAROUTE, contentRoute,sourceID, fileFolder); ;
        }
        #endregion
    }
}
