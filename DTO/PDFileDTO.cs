namespace EPlatformWebApp.DTO
{
    public class PDFileDTO
    {
        public int ID { get; set; }
        public String Source { get; set; }
        public String Path { get; set; }
        public String Tittle { get; set; }
        public String Description { get; set; } = "application/pdf";
        public bool ReadOnly { get; set; }
    }
}
