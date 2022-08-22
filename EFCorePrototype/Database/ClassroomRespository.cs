using System;
using EFCorePrototype.Model;

namespace EFCorePrototype.Database
{
	public class ClassroomRespository : CoreRespository<ClassroomEntity>
	{
		public ClassroomRespository(EFCorePrototypeDatabase database)
			: base(database)
		{
		}
	}
}

