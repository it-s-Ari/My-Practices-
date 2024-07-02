using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 This code calculates the critical points of a skyline. considering in a 2D plane there are multiple buildings,
 the critical points are were the height changes, either higher, lower or 0 (no buildings). The input is a single line/string 
 of buildings, and the output is the calculated skyline.
*/
namespace Skyline
{
    class Program
    {
        class Buildings
        {
            public Dictionary<int, int> skyline = new Dictionary<int, int>();

            private List<int[]> buildings = new List<int[]>();
            public int[] this[int index]
            {
                get
                {
                    return buildings[index];
                }
                set
                {
                    buildings.Add(value);
                    AddRoof(value);
                }
            }
            private void AddRoof(int[] roof)    // Constructs the skyline
            {
                for (int i = roof[0]; i < roof[1]; i++)
                {
                    if (skyline.ContainsKey(i)) { 

                        skyline[i] = Math.Max(skyline[i], roof[2]); 

                    }
                    else
                    {
                        skyline.Add(i, roof[2]);                      
                    }
                }
                AddZeroHeight(roof[1]);
            }

            private void AddZeroHeight(int rightWall)    // Adds zero height when there is empty space between two buildings
            {
                if (!skyline.ContainsKey(rightWall)) { 
                    skyline.Add(rightWall, 0);
                }

            }
            public List<int[]> ShowSkyLine()    //sorts the messy skyline and returns the clean version.
            {
               
                var sortedSkyLine = skyline.OrderBy(k => k.Key).ToDictionary(k => k.Key, k => k.Value);
                int currentHeight = 0;

                List<int[]> result = new List<int[]>();

                int[] firstSpot = new int[2];
                int tmp = sortedSkyLine.Keys.Min();
                result.Add(new int[] { tmp, sortedSkyLine[tmp] });
                currentHeight = sortedSkyLine[tmp];

                foreach (int key in sortedSkyLine.Keys)
                {
                    if (currentHeight != sortedSkyLine[key])
                    {
                        currentHeight = sortedSkyLine[key];
                        result.Add(new int[] { key, currentHeight });
                    }
                }
                return result;
            }
        }

        static void Main(string[] args)
        {
            Buildings buildings = new Buildings();
            int index = 0;
            Console.WriteLine("Please enter the buildings (x1, y1, z1) (x2, y2, z2) ...: ");
            string input = Console.ReadLine();
            foreach (string item in input.Split(new[] { ')' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int[] tmp = new int[3];
                tmp[0] = Convert.ToInt32((item.Trim('(', ' ').Split(','))[0]);
                tmp[1] = Convert.ToInt32((item.Trim('(', ' ').Split(','))[1]);
                tmp[2] = Convert.ToInt32((item.Trim('(', ' ').Split(','))[2]);
                buildings[index] = tmp;
                index++;
            }
            Console.WriteLine("\nthe calculated skyline is as follows (starting point, height): \n");
                
            foreach (int[] item in buildings.ShowSkyLine())
            {
                Console.WriteLine($"({Convert.ToString(item[0])}, {Convert.ToString(item[1])})");
            }
            Console.ReadLine();
        }
    }
}

