using System;
using EFCorePrototype.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCorePrototype.Database
{
	public class EFCorePrototypeDatabase : DbContext
	{
		public DbSet<SchoolEntity> SchoolEntities { get; set; }
        public DbSet<ClassroomEntity> ClassroomEntities { get; set; }
        public DbSet<PerformanceEntity> PerformanceEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("Filename=:memory");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClassroomEntity>()
                .HasKey(k => new { k.Room, k.Floor });

            modelBuilder.Entity<PerformanceEntity>()
                .HasKey(k => new
                {
                    k.KeyOne,
                    k.KeyTwo,
                    k.KeyThree,
                    k.KeyFour,
                    k.KeyFive
                });
        }
    }
}
