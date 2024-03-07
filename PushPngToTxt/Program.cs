using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PushPngToTxt
{
    internal class Program
    {
        private static void ReadPathToTxt(string dir)
        {
            var files = Directory.GetFiles(dir);
            var dirs = Directory.GetDirectories(dir);
            foreach (var subdir in dirs)
                ReadPathToTxt(subdir);
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

        private static void DeleteAndCopyToWin(List<string> orginPaths)
        {
            foreach (var orginPath in orginPaths)
            {
                var targetPath = orginPath.Replace("Resources", "ResourcesWin");

                if (Directory.Exists(targetPath))
                {
                    Directory.Delete(targetPath, true);
                    Console.WriteLine("Delete: " + targetPath);
                }
                else
                {
                    Directory.CreateDirectory(targetPath);
                    Console.WriteLine("Create: " + targetPath);
                }

                copyHandler(orginPath);
                Console.WriteLine("copyToWin: " + orginPath);
                foreach (var file in Directory.GetDirectories(orginPath)) // 遍历源文件夹中的文件
                {
                    copyHandler(file);
                    Console.WriteLine("copyToWin: " + file);
                }
            }

            Console.WriteLine();
        }

        private static void copyHandler(string orginPath)
        {
            var targetPath = orginPath.Replace("Resources", "ResourcesWin");

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }


            foreach (var file in Directory.GetFiles(orginPath)) // 遍历源文件夹中的文件
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(targetPath, fileName); // 目标文件路径
                File.Copy(file, destFile, true); // 复制文件到目标文件夹
            }
        }

        private static void Main(string[] args)
        {
            bool isPublishr = false;
            var orginPaths = new List<string>
            {
                @"D:\szj-game\Resources\IGF",
                @"D:\szj-game\Resources\shader",
            };
            ReadPathToTxt(@"D:\szj-game\Resources\IGF");
            DeleteAndCopyToWin(orginPaths);
            if (isPublishr)
            {
                SjzPublishr();
            }
            Console.WriteLine("done!");
        }
    }
}