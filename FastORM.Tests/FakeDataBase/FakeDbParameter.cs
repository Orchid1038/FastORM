using System.Data;
using System.Data.Common;

namespace FastORM.Tests.FakeDataBase
{
    public class FakeDbParameter : DbParameter
    {
        public override DbType DbType { get; set; }
        public override ParameterDirection Direction { get; set; }
        public override bool IsNullable { get; set; } = true;
        public override string ParameterName { get; set; } = string.Empty;
        public override string SourceColumn { get; set; } = string.Empty;
        public override bool SourceColumnNullMapping { get; set; }
        public override object? Value { get; set; }
        public override int Size { get; set; } = 0;
        public override void ResetDbType() { }
    }
}
