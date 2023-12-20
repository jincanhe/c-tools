using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PushPngToTxt
{
    internal class Program
    {
        private static void ProcessDirectory(string dir)
        {
            var files = Directory.GetFiles(dir);
            var dirs = Directory.GetDirectories(dir);
            foreach (var subdir in dirs)
                ProcessDirectory(subdir);
            var loadList = new List<string>();

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var path = Path.GetDirectoryName(file);
                    var ext = Path.GetExtension(file);
                    var relPath = file.Replace(@"D:\szj-game\Resources\", "").Replace("\\", "/");

                    if (ext.Equals(".png"))
                        loadList.Add(relPath);
                }


                string directory = dir;
                string fileName = Path.GetFileNameWithoutExtension(directory);
                string newDirectory = fileName + ".txt";

                File.WriteAllLines(Path.Combine(@"D:\szj-game\Resources\IGF", newDirectory), loadList);

                foreach (var png in loadList)
                {
                    Console.WriteLine(newDirectory + " : " + png);
                }
                Console.WriteLine("\n");
            }

        }

        private static void SjzPublishr()
        {
            const string exePath = @"D:\szj-game\publishtool\SzjPublisher.exe";
            var originalDirectory = Environment.CurrentDirectory;

            try
            {
                var exeDirectory = Path.GetDirectoryName(exePath);
                Environment.CurrentDirectory = exeDirectory;

                var process = new Process();
                process.StartInfo.FileName = Path.GetFileName(exePath);
                process.Start();
            }
            finally
            {
                Environment.CurrentDirectory = originalDirectory;
            }
        }

        private static void Main(string[] args)
        {
            ProcessDirectory( @"D:\szj-game\Resources\IGF");
            SjzPublishr();
            Console.WriteLine("done!");
        }
    }
}


