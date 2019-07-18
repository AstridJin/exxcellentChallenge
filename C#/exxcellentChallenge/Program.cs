using System;
using System.Data;
using System.IO;
using System.Text;

namespace exxcellentChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            printHeader();

           var filePath = @"..\..\..\resources\weather.csv";
            string dayWithSmallestTempSpread = findMinimumFormFile(filePath, "MxT", "MnT", "Day");
            Console.WriteLine("Day with smallest temperature spread : " + dayWithSmallestTempSpread);

            filePath = @"..\..\..\resources\football.csv";
            string teamWithSmallestGoalSpread = findMinimumFormFile(filePath, "Goals", "Goals Allowed", "Team");
            Console.WriteLine("Team with smallest goal spread       : " + teamWithSmallestGoalSpread);

            Console.WriteLine();
            Console.WriteLine("Mission Complete!");
            Console.ReadKey();

        }

        private static void printHeader()
        {
            Console.WriteLine("exxcellent Programming Challenge - Yujin Wang - 18.07.2019");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine();
        }

        /// <summary>
        /// Find the row with minimal absolute difference of two columns of a CSV file
        /// </summary>
        /// <param name="filePath">input CSV file path</param>
        /// <param name="col1">One of the column to calculate the subtraction</param>
        /// <param name="col2">Another column to do the subtraction</param>
        /// <param name="outputCol">Name of the output column</param>
        /// <returns> Return the string of the column of the desired row</returns>
        private static string findMinimumFormFile(string filePath, string col1, string col2, string outputCol)
        {
            var inputDatatable = new DataTable();
            if (File.Exists(filePath))
            {
                inputDatatable = readCSV(filePath);
                return findMinimum(inputDatatable, col1, col2, outputCol);
            }
            else
                return "Error: Input file does not exist!";

        }

        private static string findMinimum(DataTable inputDatatable, string col1, string col2, string outputCol)
        {
            var rowCount = inputDatatable.Rows.Count;
            var result = new object();
            // If data found
            if (rowCount != 0 )
            {
                // Initial result with the first row
                var num1 = getNumber(inputDatatable.Rows[0][col1].ToString());
                var num2 = getNumber(inputDatatable.Rows[0][col2].ToString());
                var miniDiff = num1 - num2;
                result = inputDatatable.Rows[0][outputCol];

                // Enumerate each line for the minimal absolute difference
                for (int i = 1; i < inputDatatable.Rows.Count; i++)
                {
                    var currentRow = inputDatatable.Rows[i];
                    num1 = getNumber(currentRow[col1].ToString());
                    num2 = getNumber(currentRow[col2].ToString());
                    // Get the absolute difference 
                    var abDiff = num1 - num2 < 0 ? num2 - num1 : num1 - num2;
                    // Replace the result if the absolute difference of the new row is lower
                    if (abDiff < miniDiff)
                    {
                        miniDiff = abDiff;
                        result = currentRow[outputCol];
                    }

                    // miniDiff = miniDiff < num1 - num2 ? miniDiff : num1 - num2;
                    // Console.WriteLine(currentRow["Day"]+"  "+num1+" "+num2 + " " + (num1 - num2) + " " + miniDiff);
                }
                return result.ToString();
            }

            return "Error: No data found in the input file!";

        }

        private static int getNumber(string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        /// <summary>
        /// Import data from CSV file into DataTable
        /// </summary>
        /// <param name="fileName">CSV file path</param>
        /// <returns> Return DataTable with data from input CSV file</returns>
        public static DataTable readCSV(string filePath)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            // Record content from each row 
            string strLine = "";
            // Record fields of each row
            string[] aryLine = null;
            string[] tableHead = null;
            // Indicate column number
            int columnCount = 0;
            // Flag if the first row
            bool IsFirst = true;
            // Read data row by row into the DataTable 
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    // Initial columns
                    for (int i = 0; i < columnCount; i++)
                    {
                        tableHead[i] = tableHead[i].Replace("\"", "");
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j].Replace("\"", "");
                    }
                    dt.Rows.Add(dr);
                }
            }
            sr.Close();
            fs.Close();
            return dt;
        }
    }
}
