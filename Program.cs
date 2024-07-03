using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    Given two words, beginWord and endWord, and a word list, find a possible transformation sequence from
    beginWord to endWord, such that: Only one letter can be changed at a time.

    e.g. given the beginWord "hit" and the endWord "cog" and the list "hads, dds, hog, hig, doc", one possible transformation is: 
    hit => hig => hog => cog
*/

namespace Word_Ladder_II
{
    class Program
    {
        class WordList
        {
            private List<string> result = new List<string>();
            private List<string> names = new List<string>();
            private string beginWord;
            private string endWord;
            private string currentWord;
            private string previousWord;


            public WordList(string beginWord, string endWord)
            {
                this.beginWord = beginWord;
                this.endWord = endWord;
                previousWord = beginWord;
            }

            public string this[int index]
            {
                get
                {
                    return names[index];
                }

                set
                {
                    names.Add(value);
                }
            }
            // checks if two words are compatible, meaning one can become the other using maximum one character change
            private bool Compatible(string nameOne, string nameTwo) 
            {
                int count = 0;
                char[] tmp = nameTwo.ToCharArray();
                foreach (char c in nameOne)
                {
                   if (!tmp.Contains(c))
                    {
                        count++;
                    }
                }
                
                if (count < 2)
                {
                    return true;
                }
                return false;
            }
            public void ShowTheLadder() // calculates the possible transformation sequence
            {
                int count = 0;
                bool flag = false;
                if (names.Count == 0)
                {
                    Console.WriteLine("The list is empty!");
                }
                else
                {                   
                    for (int i = 0; i < names.Count; i++)
                    {
                        currentWord = names[i];
                        if (!result.Contains(currentWord))
                        {
                            if (Compatible(previousWord, currentWord))
                            {
                                previousWord = currentWord;
                                result.Add(previousWord);
                                
                                if (Compatible(previousWord, endWord))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (result.Count == 0)
                    {
                        Console.WriteLine("Coult not find a path :(");
                    }
                    else if (!flag) {

                        string tmp = result[0];
                        if (count == names.Count)
                        {

                            Console.WriteLine("Coult not find a path :(");
                        }
                        else
                        {
                            result.Clear();
                            int ix = names.IndexOf(tmp);
                            if (ix == names.Count - 1)
                            {
                                ix = 0;
                            }
                            names.Remove(tmp);
                            names.Insert(ix, tmp);
                            count++;
                            ShowTheLadder();
                        }                  
                    }
                    else
                    {
                        Console.WriteLine("\nGreat! A path has been found: ");
                        Console.Write($"{beginWord} => ");
                        foreach (string name in result)
                        {
                            Console.Write($"{name} => ");
                        }
                        Console.Write(endWord);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            
            string beginWord;
            string endWord;
            string input;
            int i = 0;
            Console.WriteLine("Please enter the begin word and end word in the following format: \"beginword, endword\"");
            input = Console.ReadLine();
            beginWord = input.Split(',')[0].Trim(' ');
            endWord = input.Split(',')[1].Trim(' ');
            WordList list = new WordList(beginWord, endWord);
            Console.WriteLine("Now enter the word list separated by comma (e.g., word1, word2, word3, ...): ");
            input = Console.ReadLine();
            string[] wordGroup = input.Split(',');
            List<string> clearedGroup = new List<string>();
            foreach (string word in wordGroup)
            {
                if (!clearedGroup.Contains(word))
                {
                    clearedGroup.Add(word);
                }
            }
            foreach (var word in clearedGroup) // removes repeated entries 
            {
                list[i] = word.Trim(' ');
                i++;
            }

            list.ShowTheLadder();
            Console.ReadLine();
            
        }
    }
}
