using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        public static void Main()
        {
            Process process = new Process();
            process.StartInfo.FileName = "minizinc.exe";
            process.StartInfo.Arguments = " -I D:\\FreightThreading D:\\FreightThreading\\model.mzn";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            // Synchronously read the standard output of the spawned process. 
            StreamReader reader = process.StandardOutput;
            //string output = reader.ReadToEnd();
            string output;
            while ((output = reader.ReadLine()) != null)
            {
                // Write the redirected output to this application's window.
                Console.WriteLine(output);
                System.Threading.Thread.Sleep(1000);
            }
            
            process.WaitForExit();
            process.Close();

            Console.WriteLine("\n\nPress any key to exit.");
            Console.ReadLine();
        }
    }
}
