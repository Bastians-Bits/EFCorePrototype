using System;
using EFCorePrototype.Model;

namespace EFCorePrototype.Database
{
	public class SchoolRespository : CoreRespository<SchoolEntity>
	{
		public SchoolRespository(EFCorePrototypeDatabase database)
			: base(database)
		{
		}
	}
}
