namespace EPlatformWebApp.DTO
{
    public class PaymentRequestDTO
    {
        public int Amount { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public int CourseID { get; set; }
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public string Title { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public int Nit { get; set; }
        public string RazonSocial { get; set; }
        public int Price { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public bool IsOnline { get; set; } = false;
        public bool IsOppened { get; set; } = false;
    }
}
