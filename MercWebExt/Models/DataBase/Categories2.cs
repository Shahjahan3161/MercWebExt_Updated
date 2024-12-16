using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercWebExt.Models.DataBase
{
	public class Categories2
	{
		[Key]
		public int ID { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		[Required]
		[StringLength(100)]
		public string CategoryName { get; set; }

		[Required]
		[StringLength(255)]
		public string Description { get; set; }

		[NotMapped]
		public IFormFile? Image { get; set; }

		public string? ImagePath { get; set; }
	}
}
