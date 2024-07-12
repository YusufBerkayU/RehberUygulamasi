using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RehberUygulamasi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Email { get; set; }
        //public string? Password { get; set; }

        //public string ConfirmPassword { get; set; }
    }
}
