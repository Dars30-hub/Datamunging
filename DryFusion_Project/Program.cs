using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DryFusion_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.Weather.data\n2.Soccer.data");
            Console.WriteLine("Enter 1 or 2:");
            
            string i= Console.ReadLine();
            string file;
            if (Convert.ToInt32(i) == 1)
            {
                file = "weather.dat";
            }
            else
            {
                file = "football.dat";
            }
            var dt = Program.SplitColumns(file);
            if (file == "weather.dat")
            {

                Program.DisplayWeatherDataOutput(dt);
            }
            else
            {
                Program.DisplaySoccerDataOutput(dt);
            }

           


        }
        public static void DisplaySoccerDataOutput(DataTable dt)
        {
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

        public static void DisplayWeatherDataOutput(DataTable dt)
        {
            var dtNew = dt.AsEnumerable().Skip(2).CopyToDataTable();
            var dtFinal = dtNew.AsEnumerable().Take(dtNew.Rows.Count - 1).CopyToDataTable();
            //*********Test the smallest temperatur spread*********//
            // var result = dtFinal.AsEnumerable().Select(row => Convert.ToInt32(row["col2"])-Convert.ToInt32(row["col3"])).Min();

            var res = (from DataRow dr in dtFinal.Rows
                       select new
                       {
                           DayNumber = dr["col1"],
                           MinTempSpread = Convert.ToInt32(dr["col2"].ToString().Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries)[0]) - Convert.ToInt32(dr["col3"].ToString().Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries)[0])
                       }).OrderBy(x => x.MinTempSpread).Select(x => x.DayNumber).Take(1);

            foreach (var item in res)
            {
                Console.WriteLine("DayNumber for the smallest Temperature spread=" + item);

            }
        }
        private static DataTable SplitColumns(string path)
        {
            DataTable table = new DataTable("datafromtable");
            string filePath = "../../"+path;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                int rowsCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("--"))
                        continue;
                    string[] data = line.Split(new string[] { "\t", " ", "-" }, StringSplitOptions.RemoveEmptyEntries);

                    if (data.Length == 9)
                    {
                        data = data.Skip(1).ToArray();
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
