using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zembil.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is Required!")]
        [StringLength(60, ErrorMessage = "Username can't be longer then 60 characters!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email address is required!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password length must at least be 6 characters long!")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public string Phone { get; set; }

    }
}