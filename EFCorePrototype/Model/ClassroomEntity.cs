using System;
using System.ComponentModel.DataAnnotations;

namespace EFCorePrototype.Model
{
	public class ClassroomEntity
	{
		//[Key] - See EFCorePrototypeDatabase
		public int Floor { get; set; }
        //[Key] - See EFCorePrototypeDatabase
        public string Room { get; set; }
		public SchoolEntity School { get; set; }
	}
}
