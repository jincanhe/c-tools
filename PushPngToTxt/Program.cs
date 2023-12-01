using System;
using System.Collections.Generic;
using System.IO;

namespace PushPngToTxt
{
    internal class Program
    {
        static void ProcessDirectory(string dir)
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
                    var relPath = file.Replace(@"..\Resources\", "").Replace(@"..\Resources", "").Replace("\\", "/");

                    if (ext.Equals(".png"))
                        loadList.Add(relPath);
                }


                string directory = dir;
                string fileName = Path.GetFileNameWithoutExtension(directory);
                string newDirectory = fileName + ".txt";

                File.WriteAllLines(Path.Combine(@"..\Resources\image", newDirectory), loadList);

                foreach (var png in loadList)
                {
                    Console.WriteLine(newDirectory + " : " + png);
                }
                Console.WriteLine("\n");
            }

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Resources");
            ProcessDirectory(@"..\Resources\image");

        }
    }
}


