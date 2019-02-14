using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace ExcelProject
{
    class Program
    {

        static void Main(string[] args)
        {
            Dictionary<string, Tuple<int, List<Tuple<int, string>>>> guidDict = new Dictionary<string, Tuple<int, List<Tuple<int, string>>>>();
            int averageLength = 0;
            String guidWithLargestSum = "";
            int largestSum = 0;

            int counter = 0;
            string line;

            List<string> duplicateGuids = new List<string>();

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(@"test.csv");
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    var lineParts = line.Split(',');

                    String guid = lineParts[0].Replace("\"", "");
                    int val1 = Int32.Parse(lineParts[1].Replace("\"", ""));
                    int val2 = Int32.Parse(lineParts[2].Replace("\"", ""));
                    String val3 = lineParts[3].Replace("\"", "");

                    int valsSum = val1 + val2;

                                      if (valsSum > largestSum)
                    {
                        largestSum = valsSum;
                        guidWithLargestSum = guid;
                    }

                    if (guidDict.ContainsKey(guid))
                    {
                        guidDict[guid].Item2.Add(new Tuple<int, string>(val1 + val2, val3));

                        //.Concat(new Tuple<int, string>(1, val3)
                        guidDict[guid] = new Tuple<int, List<Tuple<int, string>>>(guidDict[guid].Item1 + 1, guidDict[guid].Item2);
                    }
                    else
                    {
                        List<Tuple<int, string>> lst = new List<Tuple<int, string>>();
                        lst.Add(new Tuple<int, string>(val1 + val2, val3));

                        guidDict.Add(guid, new Tuple<int, List<Tuple<int, string>>>(1, lst));
                    }

                    averageLength += val3.Length;

                    //System.Console.WriteLine(line);
                    counter++;
                }
                catch
                {
                }

                // System.Console.WriteLine(line.Split(',').First().Replace("\"", ""));
            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  

            System.Console.WriteLine("Guid {0} with the largest sum {1}", guidWithLargestSum, largestSum);

            averageLength /= counter;

            foreach (var item in guidDict)
            {
                if (item.Value.Item1 > 1)
                {
                    duplicateGuids.Add(item.Key);
                    System.Console.WriteLine("The guid duplicate {0}", item.Key);
                }
            }

            System.Console.WriteLine("The average length {0}", averageLength);

            using (System.IO.StreamWriter fd = new System.IO.StreamWriter("out.csv"))
            {
                foreach (var item in guidDict)
                {
                    bool isDublicate = false;
                    foreach (var d in duplicateGuids)
                    {
                        if (d.Equals(item.Key))
                        {
                            isDublicate = true;
                            break;
                        }
                    }

                    fd.WriteLine("{0} {1} {2} {3}", item.Key, item.Value.Item2.First().Item1, (isDublicate) ? "Y" : "N", (item.Value.Item2.First().Item2.Length > averageLength) ? "Y" : "N");
                }
            }

            System.Console.ReadLine();
        }
    }
}