using System;

namespace Pharmacy.Domain
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public bool IsConfirmed { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
    }
}
