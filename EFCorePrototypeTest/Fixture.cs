using Microsoft.EntityFrameworkCore;

namespace EFCorePrototypeTest;

[Collection("Test")]
public class SchoolEntityFixture
{
    [Fact]
    public void SchoolEntity_GetById()
    {
        Console.WriteLine("-------------------");
        Console.WriteLine("Start Test School GetById");
        Console.WriteLine("-------------------");
        var database = new EFCorePrototypeDatabase();
        database.Database.EnsureDeleted();
        database.Database.EnsureCreated();
        var repository = new SchoolRespository(database);
        // Create sample entry
        SchoolEntity sampleEntity = new SchoolEntity();
        sampleEntity.Id = Guid.NewGuid();
        sampleEntity.Name = "Test";

        database.SchoolEntities.Add(sampleEntity);
        database.SaveChanges();

        string test = repository.GetByPrimaryKey(sampleEntity).ToQueryString();
        Console.WriteLine(test);

        ICollection<SchoolEntity> result = repository.GetByPrimaryKey(sampleEntity).ToList();
        Assert.Single(result);
    }

    [Fact]
    public void ClassroomEntity_GetById()
    {
        Console.WriteLine("-------------------");
        Console.WriteLine("Start Test Classroom GetById");
        Console.WriteLine("-------------------");
        var database = new EFCorePrototypeDatabase();
        database.Database.EnsureDeleted();
        database.Database.EnsureCreated();
        var repository = new ClassroomRespository(database);
        SchoolEntity schoolEntity = new SchoolEntity();
        schoolEntity.Id = Guid.NewGuid();
        schoolEntity.Name = "Test";

        database.SchoolEntities.Add(schoolEntity);

        // Create sample entry
        ClassroomEntity sampleEntity = new ClassroomEntity();
        sampleEntity.Floor = 1;
        sampleEntity.Room = "Room 42";
        sampleEntity.School = schoolEntity;

        database.ClassroomEntities.Add(sampleEntity);
        database.SaveChanges();

        string test = repository.GetByPrimaryKey(sampleEntity).ToQueryString();
        Console.WriteLine(test);

        ICollection<ClassroomEntity> result = repository.GetByPrimaryKey(sampleEntity).ToList();
        Assert.Single(result);
    }
}
