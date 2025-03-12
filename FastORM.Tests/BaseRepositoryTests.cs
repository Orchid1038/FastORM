using FastORM.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FastORM.Tests
{
    public abstract class BaseRepositoryTests<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        protected BaseRepositoryTests(IRepository<T> repository)
        {
            _repository = repository;
        }

        protected abstract T CreateTestEntity();
        protected abstract int GetEntityId(T entity);
        protected abstract void ModifyEntity(T entity);
        protected abstract void VerifyUpdatedEntity(T entity);

        [Fact]
        public async Task AddAsync_ShouldInsertEntity()
        {
            var entity = CreateTestEntity();
            await _repository.AddAsync(entity);

            var retrievedEntity = await _repository.GetByIdAsync(GetEntityId(entity));
            Assert.NotNull(retrievedEntity);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
        {
            var entity = CreateTestEntity();
            await _repository.AddAsync(entity);

            var retrievedEntity = await _repository.GetByIdAsync(GetEntityId(entity));
            Assert.NotNull(retrievedEntity);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyEntity()
        {
            var entity = CreateTestEntity();
            await _repository.AddAsync(entity);

            ModifyEntity(entity);
            await _repository.UpdateAsync(entity);

            var updatedEntity = await _repository.GetByIdAsync(GetEntityId(entity));
            Assert.NotNull(updatedEntity);
            VerifyUpdatedEntity(updatedEntity!);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            var entity = CreateTestEntity();
            await _repository.AddAsync(entity);

            await _repository.DeleteAsync(GetEntityId(entity));

            var deletedEntity = await _repository.GetByIdAsync(GetEntityId(entity));
            Assert.Null(deletedEntity);
        }
    }
}
