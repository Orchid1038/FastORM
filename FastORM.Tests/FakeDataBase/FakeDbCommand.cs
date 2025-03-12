using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace FastORM.Tests.FakeDataBase
{
    public class FakeDbCommand : DbCommand
    {
        public override string CommandText { get; set; } = string.Empty;
        public override int CommandTimeout { get; set; } = 30;
        public override CommandType CommandType { get; set; } = CommandType.Text;
        public override UpdateRowSource UpdatedRowSource { get; set; } = UpdateRowSource.Both;
        protected override DbConnection? DbConnection { get; set; }
        protected override DbTransaction? DbTransaction { get; set; }
        public override bool DesignTimeVisible { get; set; } = false;

        private readonly FakeDbParameterCollection _parameters = new();
        protected override DbParameterCollection DbParameterCollection => _parameters;
        public new DbParameterCollection Parameters => _parameters;

        private readonly Dictionary<string, Func<int>> _commandHandlers;

        public FakeDbCommand()
        {
            _commandHandlers = new Dictionary<string, Func<int>>(StringComparer.OrdinalIgnoreCase)
            {
                { "INSERT", HandleInsert },
                { "UPDATE", HandleUpdate },
                { "DELETE", HandleDelete }
            };
        }

        public override void Cancel() { }

        public override int ExecuteNonQuery()
        {
            if (Parameters.Count == 0) return 0;

            string commandType = CommandText.Split(' ', 2)[0].ToUpperInvariant();

            if (_commandHandlers.TryGetValue(commandType, out var handler))
            {
                return handler(); // 直接執行對應方法
            }

            Console.WriteLine($"⚠️ Unsupported SQL Command: {CommandText}"); // 重要 LOG
            return 0;
        }

        private int HandleInsert()
        {
            var idParam = Parameters.Cast<DbParameter>().FirstOrDefault(p => p.ParameterName == "@Id");
            var nameParam = Parameters.Cast<DbParameter>().FirstOrDefault(p => p.ParameterName == "@Name");

            if (idParam == null || nameParam == null)
            {
                Console.WriteLine($"⚠️ INSERT Failed: Missing @Id or @Name");
                return 0;
            }

            int id = Convert.ToInt32(idParam.Value);
            string name = nameParam.Value.ToString()!;

            return FakeDataReader.AddEntity(id, name) ? 1 : 0;
        }

        private int HandleUpdate()
        {
            var idParam = Parameters.Cast<DbParameter>().FirstOrDefault(p => p.ParameterName == "@Id");
            var nameParam = Parameters.Cast<DbParameter>().FirstOrDefault(p => p.ParameterName == "@Name");

            if (idParam == null || nameParam == null)
            {
                Console.WriteLine($"⚠️ UPDATE Failed: Missing @Id or @Name");
                return 0;
            }

            int id = Convert.ToInt32(idParam.Value);
            string newName = nameParam.Value.ToString()!;
            return FakeDataReader.UpdateEntity(id, newName) ? 1 : 0;
        }

        private int HandleDelete()
        {
            var idParam = Parameters.Cast<DbParameter>().FirstOrDefault(p => p.ParameterName == "@Id");

            if (idParam == null)
            {
                Console.WriteLine($"⚠️ DELETE Failed: Missing @Id");
                return 0;
            }

            int id = Convert.ToInt32(idParam.Value);
            return FakeDataReader.DeleteEntity(id) ? 1 : 0;
        }

        public override object ExecuteScalar() => 1;
        protected override DbParameter CreateDbParameter() => new FakeDbParameter();
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => new FakeDataReader();
        public override void Prepare() { }

        protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            return Task.FromResult<DbDataReader>(new FakeDataReader());
        }
    }
}
