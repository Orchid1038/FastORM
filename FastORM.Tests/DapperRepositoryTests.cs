using FastORM.Dapper;
using FastORM.Repository;
using FastORM.Tests.FakeDataBase;
using Xunit;

namespace FastORM.Tests
{
    public class DapperRepositoryTests : BaseRepositoryTests<TestEntity>
    {
        public DapperRepositoryTests() : base(new DapperRepository<TestEntity>(new FakeDbConnection()))
        {
            FakeDataReader.ResetData(); // 確保每次測試都是乾淨的環境
        }

        protected override TestEntity CreateTestEntity() => new() { Id = 1, Name = "Test" };
        protected override int GetEntityId(TestEntity entity) => entity.Id;
        protected override void ModifyEntity(TestEntity entity) => entity.Name = "UpdatedName";
        protected override void VerifyUpdatedEntity(TestEntity entity) => Assert.Equal("UpdatedName", entity.Name);
    }
}
