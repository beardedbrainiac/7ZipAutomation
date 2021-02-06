using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7ZipAutomation.Extensions
{
    public static class IEnumerableExtension
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> rows)
        {
            if (rows == null || rows.Count() == 0) return null;

            DataTable table = new DataTable();
            var row_tmp = rows.First();
            table.Columns.AddRange(row_tmp.GetType().GetProperties().Select(field => new DataColumn(field.Name, field.PropertyType)).ToArray());
            int fieldCount = row_tmp.GetType().GetProperties().Count();

            rows.All(row =>
            {
                var s = Enumerable.Range(0, fieldCount).Select(index => row.GetType().GetProperties()[index].GetValue(row)).ToArray();

                table.Rows.Add(Enumerable.Range(0, fieldCount).Select(index => row.GetType().GetProperties()[index].GetValue(row)).ToArray());
                return true;
            });

            return table;
        }
    }
}
