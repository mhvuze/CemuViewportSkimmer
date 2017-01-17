using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CemuViewportSkimmer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variables
            string input = "";
            List<string> unfiltered_lines = new List<string>();
            List<string> filtered_lines = new List<string>();

            // Print header
            Console.WriteLine("=============================");
            Console.WriteLine("CemuViewportSkimmer by MHVuze");
            Console.WriteLine("=============================");

            // Handle arguments
            if (args.Length < 1) { Console.WriteLine("ERROR: Please specify input file."); return; }
            input = args[0];

            // Check file
            if (!File.Exists(input)) { Console.WriteLine("ERROR: Specified input file doesn't exist."); return; }

            // Warning
            Console.WriteLine("Depending on your CPU and the log size, the processing might take some time.\n");

            try
            {
                // Skim through log file
                foreach (string line in File.ReadLines(input))
                {
                    if (line.Contains("GX2SetScissor"))
                    {
                        string cut_line = line.Substring(25);
                        cut_line = cut_line.Remove(cut_line.Length - 1);
                        unfiltered_lines.Add(cut_line);
                    }
                }

                // Filter for duplicates
                filtered_lines = unfiltered_lines.Distinct().ToList();

                // Print final results
                if (filtered_lines.Count == 0)
                {
                    Console.WriteLine("No viewport resolutions were found. Did you enable GX2 API logging in Cemu?");
                }
                else
                {
                    foreach (string line in filtered_lines)
                    {
                        Console.WriteLine(line);
                    }

                    Console.WriteLine("\nFound {0} unique viewport resolutions and a total of {1}.", filtered_lines.Count, unfiltered_lines.Count);
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Something went wrong. Is Cemu still opened?");
            }
        }
    }
}
