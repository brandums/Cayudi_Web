namespace EPlatformWebApp.DTO
{
    public class BlogPostCreationDTO
    {
        public int BlogID { get; set; }
        public String Message { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public int LikesNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Highlighted { get; set; }
        public int UserID { get; set; }
    }
}
