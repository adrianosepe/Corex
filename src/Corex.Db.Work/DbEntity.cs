#region License

// Copyright(c) 2023 GrappTec
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Corex.Exceptions.Bussines;
using Corex.Library;
using Corex.Library.ComponentModel.Model;

namespace Corex.Db.Work;

public class DbEntity : Entity
{
    private bool _dynamicScheme;

    public DbEntity()
    {
        _dynamicScheme = true;
    }

    public DbEntity(DataRow data, bool dynamicScheme = false)
    {
        Data = data;

        _dynamicScheme = dynamicScheme;
    }

    [JsonIgnore]
    public DataRow Data { get; set; }

    [JsonIgnore]
    public bool IsDataNull => Data == null;

    [JsonIgnore]
    private DataTable Table => Data.Table;

    public static IEnumerable<T> Create<T>(DataTable dt) where T : DbEntity, new()
    {
        if (dt == null || dt.Rows.Count <= 0)
        {
            yield break;
        }

        foreach (var row in dt.Rows.Cast<DataRow>())
        {
            var entity = new T();
            entity.Update(row);

            yield return entity;
        }
    }

    public static TModel Create<TModel>(DataRow row) where TModel : DbEntity, new()
    {
        var model = new TModel();
        model.Update(row);

        return model;
    }

    public static TModel New<TModel>() where TModel : DbEntity, new()
    {
        var model = new TModel();

        model.ConfigureAsNew();

        return model;
    }

    public T Read<T>(string fieldName, T defaultValue)
    {
        if (IsDataNull)
        {
            return defaultValue;
        }

        var value = Data[fieldName];

        return XHelper.Obj.IsNull(value) ? defaultValue : value.CastTo<T>();
    }

    public T Read<T>(string fieldName)
    {
        if (IsDataNull)
        {
            throw XFlowException.Create($"Os dados deste {nameof(DbEntity)} não foram informados.");
        }

        var value = InternalReadField<T>(fieldName);

        return value.CastTo<T>();
    }

    public void Update(DataRow data) => Data = data;

    public void Write<T>(string fieldName, T value)
    {
        CheckExistsColumn<T>(fieldName);

        Data[fieldName] = XHelper.Obj.IsNull(value) ? (object) DBNull.Value : value;

        OnPropertyChanged(fieldName);
    }

    protected T AutoRead<T>([CallerMemberName] string fieldName = null) => Read(fieldName, default(T));

    protected T AutoReadEnum<T>([CallerMemberName] string fieldName = null) => (T) Enum.ToObject(typeof(T), Read(fieldName, default(byte)));

    protected void AutoWrite<T>(T value, [CallerMemberName] string fieldName = null) => Write(fieldName, value);

    protected void AutoWriteEnum<T>(T value, [CallerMemberName] string fieldName = null) => Write(fieldName, Convert.ChangeType(value, typeof(byte)));

    private void CheckExistsColumn<T>(string fieldName)
    {
        if (!_dynamicScheme)
        {
            return;
        }

        if (Data == null)
        {
            ConfigureAsNew();
        }
        else if (!_dynamicScheme || Table.Columns.Contains(fieldName))
        {
            return;
        }

        Table.Columns.Add(fieldName, typeof(T));
    }

    private void ConfigureAsNew()
    {
        _dynamicScheme = true;

        var table = new DataTable();
        var row = table.NewRow();

        table.Rows.Add(row);

        Data = row;
    }

    private object InternalReadField<T>(string fieldName)
    {
        CheckExistsColumn<T>(fieldName);

        return Data[fieldName];
    }
}