using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MercWebExt.Models.DataBase
{
	public class Refrigerated
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

	}
}
