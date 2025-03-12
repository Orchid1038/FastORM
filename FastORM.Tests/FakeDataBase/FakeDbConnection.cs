using System;
using System.Data;
using System.Data.Common;

namespace FastORM.Tests.FakeDataBase
{
    public class FakeDbConnection : DbConnection
    {
        public override string ConnectionString { get; set; } = "Fake Connection String";
        public override string Database => "FakeDatabase";
        public override string DataSource => "FakeDataSource";
        public override string ServerVersion => "1.0";
        public override ConnectionState State => ConnectionState.Open;

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => throw new NotImplementedException();
        public override void ChangeDatabase(string databaseName) => throw new NotImplementedException();
        public override void Close() { }
        public override void Open() { }
        protected override void Dispose(bool disposing) { }
        protected override DbCommand CreateDbCommand() => new FakeDbCommand();
    }
}
