using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FastORM.Repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IDbConnection _dbConnection;
        protected readonly IDbTransaction? _transaction;
        protected readonly string _tableName;  
        protected readonly string _primaryKey; 

        protected BaseRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

            // 取得類別名稱（表名稱）
            _tableName = GetTableName();

            // 取得主鍵名稱
            _primaryKey = GetPrimaryKeyName();
        }

        public abstract Task<T?> GetByIdAsync(int id);
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task AddAsync(T entity);
        public abstract Task UpdateAsync(T entity);
        public abstract Task DeleteAsync(int id);

        private string GetTableName()
        {
            var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
            return tableAttr?.Name ?? typeof(T).Name;
        }

        private string GetPrimaryKeyName()
        {
            var keyProp = typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
            return keyProp?.Name ?? "Id"; // 預設主鍵為 "Id"
        }
    }
}
