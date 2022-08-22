using System;
using System.ComponentModel.DataAnnotations;

namespace EFCorePrototype.Model
{
	public class SchoolEntity
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		public string Name { get; set; }
	}
}

