namespace EPlatformWebApp.Controllers.Utils
{
    public static class DateHandler
    {
        public static string normailizeDateForUI(DateTime date) {
            return date.ToString("MM/dd/YYYY");
        }
    }
}
