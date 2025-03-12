using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace FastORM.Tests.FakeDataBase
{
    public class FakeDbParameterCollection : DbParameterCollection
    {
        private readonly List<DbParameter> _parameters = new();

        public override int Count => _parameters.Count;
        public override object SyncRoot => this;

        public override int Add(object value)
        {
            if (value is not DbParameter param)
                throw new ArgumentException("Value must be a DbParameter.");

            if (!param.ParameterName.StartsWith("@"))
                param.ParameterName = "@" + param.ParameterName;

            _parameters.Add(param);
            return _parameters.Count - 1;
        }

        public override void AddRange(Array values)
        {
            foreach (var param in values.OfType<DbParameter>())
            {
                _parameters.Add(param);
            }
        }

        public override void Clear() => _parameters.Clear();
        public override bool Contains(object value) => _parameters.Contains(value as DbParameter);
        public override bool Contains(string value) => _parameters.Any(p => p.ParameterName == value);
        public override void CopyTo(Array array, int index) => _parameters.CopyTo((DbParameter[])array, index);
        public override IEnumerator GetEnumerator() => _parameters.GetEnumerator();
        protected override DbParameter GetParameter(int index) => _parameters[index];

        protected override DbParameter GetParameter(string parameterName)
        {
            if (!parameterName.StartsWith("@"))
                parameterName = "@" + parameterName; // ✅ 確保名稱格式

            var param = _parameters.FirstOrDefault(p => p.ParameterName == parameterName);
            if (param == null)
            {
                Debug.WriteLine($"FakeDbParameterCollection: Parameter {parameterName} not found."); // ✅ 只在 Debug 模式記錄
                throw new KeyNotFoundException($"Parameter {parameterName} not found.");
            }
            return param;
        }

        public override int IndexOf(object value) => _parameters.IndexOf(value as DbParameter);
        public override int IndexOf(string parameterName) => _parameters.FindIndex(p => p.ParameterName == parameterName);
        public override void Insert(int index, object value) => _parameters.Insert(index, value as DbParameter);
        public override bool IsFixedSize => false;
        public override bool IsReadOnly => false;
        public override bool IsSynchronized => false;
        public override void Remove(object value) => _parameters.Remove(value as DbParameter);
        public override void RemoveAt(int index) => _parameters.RemoveAt(index);
        public override void RemoveAt(string parameterName) => _parameters.RemoveAll(p => p.ParameterName == parameterName);
        protected override void SetParameter(int index, DbParameter value) => _parameters[index] = value;

        protected override void SetParameter(string parameterName, DbParameter value)
        {
            int index = _parameters.FindIndex(p => p.ParameterName == parameterName);
            if (index >= 0) _parameters[index] = value;
        }

        public new object this[string parameterName]
        {
            get => GetParameter(parameterName);
            set
            {
                if (value is not DbParameter param)
                    throw new ArgumentException("Value must be a DbParameter.");

                int index = _parameters.FindIndex(p => p.ParameterName == parameterName);
                if (index >= 0)
                    _parameters[index] = param;
                else
                    _parameters.Add(param);
            }
        }
    }
}
