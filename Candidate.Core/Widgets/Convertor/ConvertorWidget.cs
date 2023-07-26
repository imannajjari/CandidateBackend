using System.Data;
using System.Reflection;

namespace Candidate.Core.Widgets.Convertor;

public static class ConvertorWidget
{

    public static DataTable ToDataTable(string dtName, List<object> list)
    {
        var data = list.Select(x => (IDictionary<string, object>)x).ToList();
        DataTable table = new DataTable();
        foreach (var pair in data[0])
            table.Columns.Add(pair.Key);
        foreach (IDictionary<string, object> row in data)
        {
            DataRow tableRow = table.NewRow();
            int index = 0;
            foreach (var pair in row)
            {
                tableRow[index] = pair.Value;
                index++;
            }
            table.Rows.Add(tableRow);
        }
        return table;
    }

    public static DataTable GetDataTableFromObjects(object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            Type t = objects[0].GetType();
            DataTable dt = new DataTable(t.Name);
            foreach (PropertyInfo pi in t.GetProperties())
            {
                dt.Columns.Add(new DataColumn(pi.Name));
            }
            foreach (var o in objects)
            {
                DataRow dr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    dr[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        return null;
    }

    public static byte[] ToByteArray(string StringToConvert)
    {

        char[] CharArray = StringToConvert.ToCharArray();

        byte[] ByteArray = new byte[CharArray.Length];

        for (int i = 0; i < CharArray.Length; i++)
        {

            ByteArray[i] = Convert.ToByte(CharArray[i]);

        }

        return ByteArray;

    }

    public static ulong ToUlong(this string number)
    {
        decimal decNumber = Convert.ToDecimal(number);
        return Convert.ToUInt64(decNumber);
    }
    public static long ToLong(this string number)
    {
        decimal decNumber = Convert.ToDecimal(number);
        return Convert.ToInt64(decNumber);
    }
    public static int ToInt(this string number)
    {
        decimal decNumber = Convert.ToDecimal(number);
        return Convert.ToInt32(decNumber);
    }


    /// <summary>
    /// سازنده یک لیست از دیکشنری کی ولو بصورت جیسون
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ToKeyValueJSON(Dictionary<string, string> data = null)
    {
        string result = string.Empty;
        if (data == null)
            return "[]";

        var list = data.Select(x =>
            "{" + $"\"Key\": \"{x.Key}\", \"Value\": \"{x.Value}\"" + "}");

        result = "[" + string.Join(",", result) + "]";

        return result;
    }

}
