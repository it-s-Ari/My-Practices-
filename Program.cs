using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 given a random matrix with elements of either 0 or 1, this program calculates the area of the largest rectangle 
 in the matrix. A rectangle in this problem is referred to an area where all the elements are 1.

 e.g., given the following matrix : 

        1 1 0 0
        1 1 1 0
        1 1 1 1
        0 0 1 1

    the area of the largest rectangle found in the 2nd and 3rd rows is 6.
*/

namespace Maximum_Rectangle
{
    class Program
    {

        class Matrix
        {
            private int column;
            private int row;
            private List<int[]> matrix = new List<int[]>();

            public Matrix(int row, int column)
            {
                this.row = row;
                this.column = column;
            }

            public int[] this[int index]
            {
                get
                {
                    return matrix[index];
                }

                set
                {
                    matrix.Add(value);
                }
            }

            private List<int[]> FindAllSubGroups(int i)
            {

                List<int[]> subGroups = new List<int[]>();
                int count = 0;
                int start = -1;
                int end = -1;

                for (int j = 0; j < column; j++)
                {
                    if (matrix[i][j] == 1)
                    {
                        count++;
                        if (count > 1)
                        {
                            if (start == -1)
                            {
                                start = j - 1;
                            }
                        }
                        if (j == column - 1)
                        {
                            end = j;
                            subGroups.Add(new int[2] { start, end });

                        }
                    }
                    else if (count > 1)
                    {
                        end = j - 1;
                        subGroups.Add(new int[2] { start, end });
                        count = 0;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                return subGroups;
            }
            private int[] HasANeighbour(int[] candidate, int i)
            {
                foreach (int[] neighbour in FindAllSubGroups(i))
                {
                    if (neighbour[1] - neighbour [0] > 0)
                    {
                        return new int[] { Math.Max(neighbour[0], candidate[0]), Math.Min(neighbour[1], candidate[1])};
                    }
                }
                return new int[] { 0, 0 };
            }
            public void MaxArea()
            {
                int area = 0; // maxium area
                int[] range;
                int[] pRange = new int[2];
                bool firstRow = true;
                int count;
                List<int> areas = new List<int>(); // all areas
                for (int i = 0; i < matrix.Count; i++)
                {
                    foreach (int[] candidate in FindAllSubGroups(i))
                    {
                        count = 0;
                        for (int j = i + 1; j < matrix.Count; j++)
                        {
                            if (firstRow)
                            {
                                pRange = HasANeighbour(candidate, i + 1);
                                firstRow = false;
                            }
                            range = HasANeighbour(candidate, j);
                            if ((range[1] != 0) && (pRange[0] <= range[0]) && (pRange[1] >= range[1]))
                            {
                                count++;
                                areas.Add((range[1] - range[0] + 1) * (count + 1));
                            }
                            else
                            {
                                count = 0;
                                firstRow = true;
                                break;
                            }
                        }
                    }
                    area = areas.Max();
                    
                }
                Console.WriteLine($"The area of the largest rectangle is: {area}.");
            }
        }
        static void Main(string[] args)
        {
            string[] input = new string[2];
            Console.WriteLine("Please enter the number of rows and columns seperated by space: ");
            input = Console.ReadLine().Split();
            Matrix matrix = new Matrix(Convert.ToInt32(input[0]), Convert.ToInt32(input[1]));
            int[] row = new int[Convert.ToInt32(input[1])];
            for (int i = 0; i < Convert.ToInt32(input[0]); i++)
            {
                int j = 0;
                Console.WriteLine($"Enter line number {i+1} a list of bits separated by space: ");
                foreach (var bitt in Console.ReadLine().Split())
                {
                    row[j] = Convert.ToInt32(bitt);
                    j++;
                }
                matrix[i] = row;
                row = new int[Convert.ToInt32(input[1])];
            }
            matrix.MaxArea();
            Console.Read();
        }
        
    }
}
