using System;
using EFCorePrototype.Model;

namespace EFCorePrototype.Database
{
    public class PerformanceRepository : CoreRespository<PerformanceEntity>
    {
        public PerformanceRepository(EFCorePrototypeDatabase database)
            : base(database)
        {
        }
    }
}

