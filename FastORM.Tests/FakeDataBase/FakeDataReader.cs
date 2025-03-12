using FastORM.Tests;
using System.Collections;
using System.Data.Common;

public class FakeDataReader : DbDataReader
{
    private int _currentIndex = -1;
    private static Dictionary<int, TestEntity> _data = new()
    {
        { 1, new TestEntity { Id = 1, Name = "Test" } }
    };
    public static void ResetData()
    {
        _data = new Dictionary<int, TestEntity>
    {
        { 1, new TestEntity { Id = 1, Name = "Test" } }
    };
    }
    public FakeDataReader()
    {
        RefreshResults();
    }

    private List<TestEntity> _currentResults = new();

    private void RefreshResults()
    {
        _currentResults = _data.Values.ToList();
    }

    public override bool Read()
    {
        RefreshResults(); // 確保讀取前有最新資料
        Console.WriteLine($"🔄 讀取資料: {_currentResults.Count} 筆");
        _currentIndex++;
        return _currentIndex < _currentResults.Count;
    }

    public override bool HasRows => _currentResults.Count > 0;

    public override object this[string name] => name switch
    {
        "Id" => GetValue(0),
        "Name" => GetValue(1),
        _ => throw new IndexOutOfRangeException()
    };

    public override object this[int i] => GetValue(i);
    public static bool AddEntity(int id, string name)
{
    if (_data.ContainsKey(id))
    {
        Console.WriteLine($"⚠️ FakeDataReader: 插入失敗，Id={id} 已存在");
        return false;
    }

    _data[id] = new TestEntity { Id = id, Name = name };
    Console.WriteLine($"✅ FakeDataReader: 插入成功 Id={id}, Name={name}");
    return true;
}

    public static bool UpdateEntity(int id, string newName)
    {
        if (_data.TryGetValue(id, out var entity))
        {
            Console.WriteLine($"🔄 更新 Id={id} 的 Name 為 {newName}");
            entity.Name = newName;
            return true;
        }
        Console.WriteLine($"❌ 更新失敗，找不到 Id={id}");
        return false;
    }


    public static bool DeleteEntity(int id)
    {
        if (_data.ContainsKey(id))
        {
            Console.WriteLine($"🗑️ 刪除 Id={id}");
            _data.Remove(id);
            return true;
        }
        Console.WriteLine($"❌ 刪除失敗，找不到 Id={id}");
        return false;
    }


    public override void Close() { }
    public override int Depth => 0;
    public override bool IsClosed => false;
    public override bool NextResult() => false;
    public override int RecordsAffected => _data.Count;
    public override int FieldCount => 2;

    public override bool GetBoolean(int i) => Convert.ToBoolean(GetValue(i));
    public override byte GetByte(int i) => Convert.ToByte(GetValue(i));
    public override long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length) => 0;
    public override char GetChar(int i) => Convert.ToChar(GetValue(i));
    public override long GetChars(int i, long fieldOffset, char[]? buffer, int bufferoffset, int length) => 0;
    public override string GetDataTypeName(int i) => GetValue(i).GetType().Name;
    public override DateTime GetDateTime(int i) => Convert.ToDateTime(GetValue(i));
    public override decimal GetDecimal(int i) => Convert.ToDecimal(GetValue(i));
    public override double GetDouble(int i) => Convert.ToDouble(GetValue(i));
    public override float GetFloat(int i) => Convert.ToSingle(GetValue(i));
    public override Type GetFieldType(int i) => GetValue(i).GetType();
    public override short GetInt16(int i) => Convert.ToInt16(GetValue(i));
    public override int GetInt32(int i) => Convert.ToInt32(GetValue(i));
    public override long GetInt64(int i) => Convert.ToInt64(GetValue(i));
    public override Guid GetGuid(int i) => Guid.Empty; // 給個預設值
    public override string GetName(int i)
    {
        return i switch
        {
            0 => "Id",
            1 => "Name",
            _ => throw new IndexOutOfRangeException()
        };
    }

    public override int GetOrdinal(string name)
    {
        return name switch
        {
            "Id" => 0,
            "Name" => 1,
            _ => throw new IndexOutOfRangeException()
        };
    }

    public override string GetString(int i) => GetValue(i).ToString()!;
    public override object GetValue(int i)
    {
        if (_currentIndex < 0 || _currentIndex >= _currentResults.Count)
        {
            return DBNull.Value;
        }
        return i switch
        {
            0 => _currentResults[_currentIndex].Id,
            1 => _currentResults[_currentIndex].Name,
            _ => DBNull.Value
        };
    }

    public override int GetValues(object[] values)
    {
        int count = Math.Min(values.Length, FieldCount);
        for (int i = 0; i < count; i++)
        {
            values[i] = GetValue(i);
        }
        return count;
    }
    public override bool IsDBNull(int i) => GetValue(i) == DBNull.Value;
    public override IEnumerator GetEnumerator() => ((IEnumerable)_currentResults).GetEnumerator();
}
