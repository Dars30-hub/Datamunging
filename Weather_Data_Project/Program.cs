using System;
using System.Linq;
using System.IO;
using System.Data;

namespace Weather_Data_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dt=Program.SplitColumns();
            var  dtNew = dt.AsEnumerable().Skip(2).CopyToDataTable();
            var dtFinal = dtNew.AsEnumerable().Take(dtNew.Rows.Count - 1).CopyToDataTable();
            //*********Test the smallest temperatur spread*********//
            // var result = dtFinal.AsEnumerable().Select(row => Convert.ToInt32(row["col2"])-Convert.ToInt32(row["col3"])).Min();

            var res = (from DataRow dr in dtFinal.Rows
                       select new
                       {
                           DayNumber = dr["col1"],
                           MinTempSpread = Convert.ToInt32(dr["col2"].ToString().Split(new string[]{"*"}, StringSplitOptions.RemoveEmptyEntries)[0]) - Convert.ToInt32(dr["col3"].ToString().Split(new string[] { "*"}, StringSplitOptions.RemoveEmptyEntries)[0])
                       }).OrderBy(x => x.MinTempSpread).Select(x => x.DayNumber).Take(1);

            foreach (var item in res)
            {
                Console.WriteLine("DayNumber for the smallest Temperature spread="+item);
                
            }

        }

        private static DataTable SplitColumns()
        {
            DataTable table = new DataTable("datafromtable");
            string filePath = "../../weather.dat";
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                int rowsCount = 0;
                while ((line = sr.ReadLine()) != null)
                {

                    string[] data = line.Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
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
