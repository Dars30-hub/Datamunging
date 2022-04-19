using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soccer_Data_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dt = Program.SplitColumns();
            var dtNew = dt.AsEnumerable().Skip(1).CopyToDataTable();
            var res = (from DataRow dr in dtNew.Rows
                       select new
                       {
                           Team = dr["col1"],
                           SmallDifference = Math.Abs(Convert.ToInt32(dr["col6"].ToString().Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries)[0]) - Convert.ToInt32(dr["col7"].ToString().Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries)[0]))
                       }).OrderBy(x => x.SmallDifference).Select(x => x.Team).Take(1);

            foreach (var item in res)
            {
                Console.WriteLine(" Team with the smallest difference in ‘for’ and ‘against’ goals=" + item);

            }
        }
        private static DataTable SplitColumns()
        {
            DataTable table = new DataTable("datafromtable");
            string filePath = "../../football.dat";
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                int rowsCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("--"))
                        continue;
                    string[] data = line.Split(new string[] { "\t"," ","-"}, StringSplitOptions.RemoveEmptyEntries);
                    
                    if(data.Length == 9)
                    {
                        data=data.Skip(1).ToArray();
                    }
                    if (table.Columns.Count == 0)
                    {
                        for (int i = 1; i <= data.Length; i++)
                        {

                            table.Columns.AddRange(new DataColumn[] { new DataColumn("col" + (i).ToString(), typeof(string)) });
                        }
                    }
                    table.Rows.Add();
                    for (int i = 0; i < data.Length; i++)
                    {
                       
                        if (data[i].Contains(" "))
                            data[i] = data[i].Replace(" ", "");
                        if (!data[i].Equals(""))
                            table.Rows[rowsCount][i] = data[i];
                    }
                    rowsCount++;
                }
            }
            return table;
        }
    }
}
