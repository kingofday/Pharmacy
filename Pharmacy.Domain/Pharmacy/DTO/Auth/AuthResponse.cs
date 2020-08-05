namespace Pharmacy.Domain
{
    public class AuthResponse
    {
        public bool IsConfirmed { get; set; }
        public string Token { get; set; }

        public string Fullname { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }
    }
}
