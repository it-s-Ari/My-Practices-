using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 Calculates the best path from one location to the other in an infinite grid using A* algorithm
 */
namespace PathFinder
{
    class Program
    {
        static int HF(int[] a, int[] b) //heuristic function to estimates the potential cost from current place to the next.
        {
            return Math.Abs(b[0] - a[0]) + Math.Abs(b[1] - a[1]); //potential cost aka h(x,y)
        }
        static List<int[]> GetLocations()  // gets the source and destination from the user
        {
            Console.WriteLine("Enter x of the 1st node: ");
            int x1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter y of the 1st node: ");
            int y1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter x of the 2nd node: ");
            int x2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter y of the 2nd node: ");
            int y2 = Convert.ToInt32(Console.ReadLine());
            return new List<int[]>
            {
                new int[] {x1, y1, Math.Abs(y2-y1) + Math.Abs(x2-x1)},
                new int[] {x2, y2, 0}
            };

        }
        static List<int[]> GetObstacles(int n)  // gets the source and destination from the user
        {
            Console.WriteLine("Enter the number of forbidden nodes: ");
            List<int[]> obstacles = new List<int[]>();
            int[] node = new int[3];
            while (n > 0)
            {
                Console.WriteLine("Enter x of an obstacle: ");
                node[0] = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter y of the corrosponding node: ");
                node[1] = Convert.ToInt32(Console.ReadLine());
                node[2] = Convert.ToInt32((0));
                obstacles.Add(node);
                n--;

            }
            return obstacles;
        }
            static List<int[]> FindNeighbours(int[] node, List<int[]> closedList, int[] destination, List<int[]> obstacles) // calculates and return neighbours 
        {
            List<int[]> toBeRemoved = new List<int[]>();
            List<int[]> neighbours = new List<int[]>
            {
                new int[] {node[0]+1, node[1], 0},
                new int[] { node[0] - 1, node[1], 0},
                new int[] { node[0], node[1] + 1, 0},
                new int[] { node[0], node[1] - 1, 0}

        };
            
            foreach (int[] item in neighbours)
            {
                if (closedList.Exists(n => n[0] == item[0] && n[1] == item[1]) ||
                    obstacles.Exists(n => n[0] == item[0] && n[1] == item[1]))
                {
                    toBeRemoved.Add(item);
                }
            }
            foreach (int[] item in toBeRemoved) 
            {
               
                neighbours.Remove(item);
                
            }
            
            foreach (int[] item in neighbours)
            {
                int g = Math.Abs((item[1] - node[1])) + Math.Abs((item[0] - node[0]));
                int f = g + Math.Abs(HF(item, destination));
                item[2] = g + f;
            }
            return neighbours;
    }
        static int[] findBestNeighbour(int[] a, List<int[]> openList, List<int[]> neighbours)
        {

            int[] bestNeighbour = neighbours[neighbours.Count - 1];
            //openList.Remove(bestNeighbour);
            foreach (int[] neighbour in neighbours)
            {

                if ((neighbour[2] < bestNeighbour[2]) && (neighbour != a) && (!openList.Contains(neighbour))) // finds the lowest f in neighbours
                {
                    if (bestNeighbour[2] > neighbour[2])
                    {
                        bestNeighbour = neighbour;
                        //Console.Read();
                    }
                }
            }
            return bestNeighbour;
        }
        static void Main(string[] args)
        {
            
            List<int[]> closedList = new List<int[]>(); //the chosen path
            List<int[]> openList = new List<int[]>();
            bool destReached = false; // declares if we have reached the destination
            List<int[]> locations = GetLocations();
            Console.WriteLine("enter the number of forbidden nodes: ");
            int n = Convert.ToInt32(Console.ReadLine());
            List<int[]> obstacles = new List<int[]>(); //forbidden nodes

            if (n > 0)
            {
                obstacles = GetObstacles(n); //forbidden nodes
            }
           
            closedList.Add(locations[0]); //the first element of the chosen path is the source
            openList.Add(locations[0]); //the first element of the chosen path is the source

            while (!destReached)
            {
               
                int[] currentNode = closedList[closedList.Count - 1];
                if ((currentNode[0] == locations[1][0]) && currentNode[1] == locations[1][1]){
                    break;
                }
                List<int[]> neighbours = FindNeighbours(currentNode, closedList, locations[1], obstacles); // finds all neighbours of the current node
                int[] bestNeighbour = findBestNeighbour(currentNode, openList, neighbours); // finds the best neighbour 
                foreach (int[] neighbour in neighbours)
                {
                    if (!closedList.Contains(neighbour)){
                        openList.Add(neighbour);
                    }
                }
                closedList.Add(bestNeighbour);
            }
            Console.WriteLine("Destination reached :) the suggested shortest path is: ");
            string tmp = ") -> ";
            foreach (int[] node in closedList)
            {
                if ((node[0] == locations[1][0]) && node[1] == locations[1][1])
                {
                    tmp = ")";
                }
                Console.Write('(' + Convert.ToString(node[0]) + ' ' + Convert.ToString(node[1]) + tmp);
            }
            Console.Read();

        }
    }
}
