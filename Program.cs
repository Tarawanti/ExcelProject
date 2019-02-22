using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csvproject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating a dictionary: key - guid, value - pair of the counts of guid repeats and the list of pairs with (val1 + val2) and val3 
            Dictionary<string, Tuple<int, List<Tuple<int, string>>>> guidDict = new Dictionary<string, Tuple<int, List<Tuple<int, string>>>>();
            int averageLength = 0;
            String guidWithLargestSum = "";
            int largestSum = 0;

            int counter = 0;
            string line;

            // creating the list with duplicated guids
            List<string> duplicateGuids = new List<string>();

            // Opening the csv file and reading line by line  
            System.IO.StreamReader file = new System.IO.StreamReader(@"test.csv");
            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    // getting parts of the line separated by ','
                    var lineParts = line.Split(',');

                    // getting guid, val1, val2, val3 from the string with deleting '"' drom it
                    String guid = lineParts[0].Replace("\"", "");
                    int val1 = Int32.Parse(lineParts[1].Replace("\"", ""));
                    int val2 = Int32.Parse(lineParts[2].Replace("\"", ""));
                    String val3 = lineParts[3].Replace("\"", "");

                    int valsSum = val1 + val2;

                    // searching for the largest (val1 + val2) sum
                    if (valsSum > largestSum) 
                    {
                        largestSum = valsSum;
                        guidWithLargestSum = guid;
                    }

                    if (guidDict.ContainsKey(guid))
                    {
                        // adding values for a duplicated guid
                        guidDict[guid].Item2.Add(new Tuple<int, string>(val1 + val2, val3));

                        guidDict[guid] = new Tuple<int, List<Tuple<int, string>>>(guidDict[guid].Item1 + 1, guidDict[guid].Item2);
                    }
                    else // if the guid is met the first time
                    {
                        List<Tuple<int, string>> lst = new List<Tuple<int, string>>();
                        lst.Add(new Tuple<int, string>(val1 + val2, val3));

                        // adding to our dictionary val1 + val2 and val3 and matching that it was met the first time(1)
                        guidDict.Add(guid, new Tuple<int, List<Tuple<int, string>>>(1, lst));
                    }

                    // accumulating the val3 lengths 
                    averageLength += val3.Length;

                    counter++;
                }
                catch 
                {
                }

            }

            file.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            
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

            // compairing the output file
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

                    fd.WriteLine("\"{0}\", \"{1}\", \"{2}\", \"{3}\"", item.Key, item.Value.Item2.First().Item1, (isDublicate) ? "Y" : "N", (item.Value.Item2.First().Item2.Length > averageLength) ? "Y" : "N");
                }
            }

            System.Console.ReadLine();  
        }
    }
}
