using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MercWebExt.Models.DataBase
{
    public class Certified
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [NotMapped]
        public List<IFormFile>? Images { get; set; } = new List<IFormFile>();

        public string? ImagePaths { get; set; } // Comma-separated image paths
        


    
    }



}
