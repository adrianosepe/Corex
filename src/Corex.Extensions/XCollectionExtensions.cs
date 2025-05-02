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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Corex.Library;

// ReSharper disable CheckNamespace
public static class XCollectionExtensions
// ReSharper restore CheckNamespace
{
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        values?.ForEach(collection.Add);
    }

    /// <summary>
    ///     Adds a range of value uniquely to a collection and returns the amount of values added.
    /// </summary>
    /// <typeparam name="T">The generic collection value type.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="values">The values to be added.</param>
    /// <returns>The amount if values that were added.</returns>
    public static int AddRangeUnique<T>(this ICollection<T> collection, IEnumerable<T> values)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        return values.Count(collection.AddUnique);
    }

    /// <summary>
    ///     Adds a value uniquely to to a collection and returns a value whether the value was added or not.
    /// </summary>
    /// <typeparam name="T">The generic collection value type</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="value">The value to be added.</param>
    /// <returns>Indicates whether the value was added or not</returns>
    /// <example>
    ///     <code>
    /// 		list.AddUnique(1); // returns true;
    /// 		list.AddUnique(1); // returns false the second time;
    /// 	</code>
    /// </example>
    public static bool AddUnique<T>(this ICollection<T> collection, T value)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        var alreadyHas = collection.Contains(value);
        if (!alreadyHas)
        {
            collection.Add(value);
        }

        return alreadyHas;
    }

    /// <summary>
    ///     .
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="comparison">The comparison.</param>
    /// <returns>The item index</returns>
    public static int IndexOf<T>(this IList<T> list, Func<T, bool> comparison)
    {
        for (var i = 0; i < list.Count; i++)
        {
            if (comparison(list[i]))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool IsEmpty(this ICollection collection)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        return collection.Count == 0;
    }

    public static bool IsNotEmpty(this ICollection collection) => !IsEmpty(collection);

    /// <summary>
    ///     Join all the elements in the list and create a string seperated by the specified char.
    /// </summary>
    /// <param name="list">
    ///     The list.
    /// </param>
    /// <param name="joinChar">
    ///     The join char.
    /// </param>
    /// <typeparam name="T">
    /// </typeparam>
    /// <returns>
    ///     The resulting string of the elements in the list.
    /// </returns>
    /// <remarks>
    ///     Contributed by Michael T, http://about.me/MichaelTran
    /// </remarks>
    public static string Join<T>(this IList<T> list, char joinChar) => list.Join(joinChar.ToString());

    /// <summary>
    ///     Join all the elements in the list and create a string seperated by the specified string.
    /// </summary>
    /// <param name="list">
    ///     The list.
    /// </param>
    /// <param name="joinString">
    ///     The join string.
    /// </param>
    /// <typeparam name="T">
    /// </typeparam>
    /// <returns>
    ///     The resulting string of the elements in the list.
    /// </returns>
    /// <remarks>
    ///     Contributed by Michael T, http://about.me/MichaelTran
    ///     Optimised by Mario Majcica
    /// </remarks>
    public static string Join<T>(this IList<T> list, string joinString)
    {
        if (list == null || !list.Any())
        {
            return string.Empty;
        }

        var result = new StringBuilder();

        var listCount = list.Count;
        var listCountMinusOne = listCount - 1;

        if (listCount > 1)
        {
            for (var i = 0; i < listCount; i++)
            {
                if (i != listCountMinusOne)
                {
                    result.Append(list[i]);
                    result.Append(joinString);
                }
                else
                {
                    result.Append(list[i]);
                }
            }
        }
        else
        {
            result.Append(list[0]);
        }

        return result.ToString();
    }

    /// <summary>
    ///     Removes all.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="predicate">The predicate.</param>
    public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        var items = collection.ToArray();

        XHelper.Enumerable.ForEach(items, c =>
            {
                if (predicate(c))
                {
                    collection.Remove(c);
                }
            });
    }

    /// <summary>
    ///     Removes the first.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The list.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns></returns>
    public static bool RemoveFirst<T>(this ICollection<T> collection, Func<T, bool> predicate)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        var item = collection.FirstOrDefault(predicate);
        if (Equals(item, null) || item.Equals(default(T)))
        {
            return false;
        }

        collection.Remove(item);

        return true;
    }

    /// <summary>
    ///     Remove an item from the collection with predicate
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    /// <remarks>
    ///     Contributed by Michael T, http://about.me/MichaelTran
    ///     Renamed by James Curran, to match corresponding HashSet.RemoveWhere()
    /// </remarks>
    public static void RemoveWhere<T>(this ICollection<T> collection, Predicate<T> predicate)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        var deleteList = collection.Where(child => predicate(child)).ToList();
        deleteList.ForEach(t => collection.Remove(t));
    }

    public static DataTable ToDataTable<T>(this IList<T> list)
    {
        var entityType = typeof(T);

        // Lists of type System.string and System.Enum (which includes enumerations and structs) must be handled differently
        // than primitives and custom objects (e.g. an object that is not type System.Object).
        if (entityType == typeof(string))
        {
            var dataTable = new DataTable(entityType.Name);
            dataTable.Columns.Add(entityType.Name);

            // Iterate through each item in the list. There is only one cell, so use index 0 to set the value.
            foreach (var item in list)
            {
                var row = dataTable.NewRow();
                row[0] = item;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        if (entityType.BaseType == typeof(Enum))
        {
            var dataTable = new DataTable(entityType.Name);
            dataTable.Columns.Add(entityType.Name);

            // Iterate through each item in the list. There is only one cell, so use index 0 to set the value.
            foreach (var namedConstant in Enum.GetNames(entityType))
            {
                var row = dataTable.NewRow();
                row[0] = namedConstant;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        // Check if the type of the list is a primitive type or not. Note that if the type of the list is a custom
        // object (e.g. an object that is not type System.Object), the underlying type will be null.
        var underlyingType = Nullable.GetUnderlyingType(entityType);
        var primitiveTypes = new List<Type>
            {
                typeof(byte),
                typeof(char),
                typeof(decimal),
                typeof(double),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(sbyte),
                typeof(float),
                typeof(ushort),
                typeof(uint),
                typeof(ulong)
            };

        var typeIsPrimitive = primitiveTypes.Contains(underlyingType);

        // If the type of the list is a primitive, perform a simple conversion.
        // Otherwise, map the object's properties to columns and fill the cells with the properties' values.
        if (typeIsPrimitive)
        {
            var dataTable = new DataTable(underlyingType.Name);
            dataTable.Columns.Add(underlyingType.Name);

            // Iterate through each item in the list. There is only one cell, so use index 0 to set the value.
            foreach (var item in list)
            {
                var row = dataTable.NewRow();
                row[0] = item;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        else
        {
            var dataTable = new DataTable(entityType.Name);
            var propertyDescriptorCollection = TypeDescriptor.GetProperties(entityType);

            // Iterate through each property in the object and add that property name as a new column in the data table.
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
            {
                // Data tables cannot have nullable columns. The cells can have null values, but the actual columns themselves cannot be nullable.
                // Therefore, if the current property type is nullable, use the underlying type (e.g. if the type is a nullable int, use int).
                var propertyType = Nullable.GetUnderlyingType(propertyDescriptor.PropertyType) ??
                                   propertyDescriptor.PropertyType;
                dataTable.Columns.Add(propertyDescriptor.Name, propertyType);
            }

            // Iterate through each object in the list adn add a new row in the data table.
            // Then iterate through each property in the object and add the property's value to the current cell.
            // Once all properties in the current object have been used, add the row to the data table.
            foreach (var item in list)
            {
                var row = dataTable.NewRow();

                foreach (PropertyDescriptor propertyDescriptor in propertyDescriptorCollection)
                {
                    var value = propertyDescriptor.GetValue(item);
                    row[propertyDescriptor.Name] = value ?? DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}