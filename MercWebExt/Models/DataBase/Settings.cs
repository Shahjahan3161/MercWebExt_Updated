using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MercWebExt.Models.DataBase
{
    public class Settings
    {
        [Key]
        public int ID { get; set; }

        [NotMapped]
        public IFormFile? LogoImage { get; set; }

        public string? LogoImagePath { get; set; }

        [NotMapped]
        public IFormFile? BackgroundImage { get; set; }
        public string? BackgroundImagePath { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        public string? FacebookLink { get; set; }

        public string? TwitterLink { get; set; }

        [NotMapped]
        public IFormFile? FrontAdImage { get; set; }

        public string? FrontAdImagePath { get; set; }
    }
}
