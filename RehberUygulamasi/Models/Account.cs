using System.ComponentModel.DataAnnotations;

namespace RehberUygulamasi.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string ConfirmPassword { get; set; }
    }

}
