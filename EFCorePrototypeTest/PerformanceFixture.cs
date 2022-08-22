using System;
namespace EFCorePrototypeTest
{
	[Collection("Test")]
	public class PerformanceFixture
	{
		protected int AmountSamples    { get; } = 100_000;
        protected int AmountTestSample { get; } = 1_000;

		[Fact]
		public void Test_Performance_Check_Setup()
		{
            Console.WriteLine("-------------------");
            Console.WriteLine("Start Test Check Setup");
            Console.WriteLine("-------------------");

            var database = new EFCorePrototypeDatabase();
            database.Database.EnsureDeleted();
            database.Database.EnsureCreated();

			CreateEntries(database);
						
			Assert.Equal(AmountSamples, database.PerformanceEntities.Count());
        }

		[Fact]
		public void Test_Performance_Single()
		{
            Console.WriteLine("-------------------");
            Console.WriteLine("Start Test Single");
            Console.WriteLine("-------------------");
            var database = new EFCorePrototypeDatabase();
            database.Database.EnsureDeleted();
            database.Database.EnsureCreated();
			var repository = new PerformanceRepository(database);

            CreateEntries(database);

			// Repository
			long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			PerformanceEntity? t1Entity = repository.GetByPrimaryKey(new PerformanceEntity() {
				KeyOne = "KeyOne50000",
				KeyTwo = "KeyTwo50000",
				KeyThree = "KeyThree50000",
				KeyFour = "KeyFour50000",
				KeyFive = "KeyFive50000"

			}).FirstOrDefault();
            long stopTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			Assert.NotNull(t1Entity);
            Console.WriteLine($"Loaded entry in {stopTime - startTime}ms");

			// Manuall
			startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			PerformanceEntity? t2Entity = database.PerformanceEntities.Where(w =>
				w.KeyOne == "KeyOne50001" &&
                w.KeyTwo == "KeyTwo50001" &&
                w.KeyThree == "KeyThree50001" &&
                w.KeyFour == "KeyFour50001" &&
                w.KeyFive == "KeyFive50001"
			).FirstOrDefault();
			stopTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			Assert.NotNull(t2Entity);
            Console.WriteLine($"Loaded entry in {stopTime - startTime}ms");
        }

        [Fact]
        public void Test_Performance_Multiple()
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("Start Test Multiple");
            Console.WriteLine("-------------------");
            var database = new EFCorePrototypeDatabase();
            database.Database.EnsureDeleted();
            database.Database.EnsureCreated();
            var repository = new PerformanceRepository(database);

            CreateEntries(database);
            ICollection<PerformanceEntity> samples = CreateSample();

            // Repository
            long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            foreach (PerformanceEntity sample in samples)
            {
                PerformanceEntity? t1Entity = repository.GetByPrimaryKey(sample).FirstOrDefault();
                Assert.NotNull(t1Entity);
            }
            long stopTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine($"Loaded entries in {stopTime - startTime}ms");

            // Manuall
            startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            foreach (PerformanceEntity sample in samples)
            {
                PerformanceEntity? t2Entity = database.PerformanceEntities.Where(w =>
                    w.KeyOne == sample.KeyOne &&
                    w.KeyTwo == sample.KeyTwo &&
                    w.KeyThree == sample.KeyThree &&
                    w.KeyFour == sample.KeyFour &&
                    w.KeyFive == sample.KeyFive
                ).FirstOrDefault();
                Assert.NotNull(t2Entity);
            }
            stopTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine($"Loaded entries in {stopTime - startTime}ms");
        }

        private ICollection<PerformanceEntity> CreateSample()
        {
            ICollection<PerformanceEntity> samples = new List<PerformanceEntity>();

            int start = (AmountSamples - AmountTestSample) / 2;
            int stop  = ((AmountSamples - AmountTestSample) / 2) + AmountTestSample;

            // Start in the middle of the entries
            for (int i = start; i < stop; i++) 
            {
                PerformanceEntity sample = new PerformanceEntity();
                sample.KeyOne = $"KeyOne{i}";
                sample.KeyTwo = $"KeyTwo{i}";
                sample.KeyThree = $"KeyThree{i}";
                sample.KeyFour = $"KeyFour{i}";
                sample.KeyFive = $"KeyFive{i}";

                samples.Add(sample);
            }

            return samples;
        }

        private void CreateEntries(EFCorePrototypeDatabase database)
		{
            long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            for (int i = 0; i < AmountSamples; i++)
			{
				if (i % 1_000 == 0)
				{
					database.SaveChangesAsync();
					Console.WriteLine($"Added {i} entries");
				}

				PerformanceEntity sample = new PerformanceEntity();
				sample.KeyOne   = $"KeyOne{i}"  ;
                sample.KeyTwo   = $"KeyTwo{i}"  ;
                sample.KeyThree = $"KeyThree{i}";
                sample.KeyFour  = $"KeyFour{i}" ;
                sample.KeyFive  = $"KeyFive{i}" ;

				database.PerformanceEntities.Add(sample);
            }
			database.SaveChanges();
            long stopTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine($"Created {AmountSamples} entries in {(stopTime - startTime) / 1_000}s");
        }
	}
}

