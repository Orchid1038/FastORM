using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FastORM.Repository;

namespace FastORM.Dapper
{
    public class DapperRepository<T> : BaseRepository<T> where T : class
    {
        public DapperRepository(IDbConnection dbConnection) : base(dbConnection) { }

        public override async Task<T?> GetByIdAsync(int id)
        {
            string sql = $"SELECT * FROM {_tableName} WHERE {_primaryKey} = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id }, _transaction);
        }

        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            string sql = $"SELECT * FROM {_tableName}";
            return await _dbConnection.QueryAsync<T>(sql, transaction: _transaction);
        }

        public override async Task AddAsync(T entity)
        {
            string sql = $"INSERT INTO {_tableName} ({GetColumns()}) VALUES ({GetValues()})";
            await _dbConnection.ExecuteAsync(sql, entity, _transaction);
        }

        public override async Task UpdateAsync(T entity)
        {
            string sql = $"UPDATE {_tableName} SET {GetUpdateColumns()} WHERE {_primaryKey} = @Id";
            int affectedRows = await _dbConnection.ExecuteAsync(sql, entity, _transaction);

            if (affectedRows == 0)
            {
                throw new InvalidOperationException($"Update failed: No record found in {_tableName} with {_primaryKey} = {TryGetProperty(entity, _primaryKey)}.");
            }
        }

        public override async Task DeleteAsync(int id)
        {
            string sql = $"DELETE FROM {_tableName} WHERE {_primaryKey} = @Id";
            int affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id }, _transaction);

            if (affectedRows == 0)
            {
                throw new InvalidOperationException($"Delete failed: No record found in {_tableName} with {_primaryKey} = {id}.");
            }
        }

        private string GetColumns() => string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));
        private string GetValues() => string.Join(", ", typeof(T).GetProperties().Select(p => $"@{p.Name}"));
        private string GetUpdateColumns() => string.Join(", ", typeof(T).GetProperties().Where(p => p.Name != _primaryKey).Select(p => $"{p.Name} = @{p.Name}"));

        private static object? TryGetProperty(T entity, string propertyName)
        {
            var prop = typeof(T).GetProperty(propertyName);
            return prop?.GetValue(entity);
        }
    }
}
