using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ffmpegTools
{
    internal class Program
    {

        private static void tgaConvertToPng(string topFolder)
        {


            foreach (var folder in Directory.GetDirectories(topFolder))
            {
                convertHandler(folder);
            }



        }

        private static void convertHandler(string filePath)
        {
            string[] tgaFiles = Directory.GetFiles(filePath, "*.tga");
            var newPath = filePath.Replace("HXTga", "HXPng");

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            string id = filePath.Substring(filePath.LastIndexOf('\\') + 1);



            foreach (var file in tgaFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);

                string outputFilePath = Path.Combine(newPath, $"{id + "_" + fileName}.png");

                if (File.Exists(outputFilePath))
                {
                    continue;
                }
                string ffmpegCommand = $"-y -i \"{file}\" -compression_level 0 \"{outputFilePath}\"";
                RunFFmpegCommand(ffmpegCommand);
            }


            string[] txtFiles = Directory.GetFiles(filePath, "*.txt");
            foreach (var file in txtFiles)
            {
                var newP = file.Replace(filePath, @"D:\test_cocos_proj\ResourcesWin\HXoffset").Replace("偏移",id + "_offset");
                File.Copy(file, newP, true);
            }


            Console.WriteLine(filePath + "转换完成！");
        }


        private static void ReadPathToTxt(string dir)
        {
            var files = Directory.GetFiles(dir);

            var loadList = new List<string>();

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var path = Path.GetDirectoryName(file);
                    var ext = Path.GetExtension(file);
                    var relPath = file.Replace(@"D:\test_cocos_proj\ResourcesWin\", "").Replace("\\", "/");

                    if (ext.Equals(".txt"))
                        loadList.Add(relPath);
                }

                string directory = dir;
                string fileName = Path.GetFileNameWithoutExtension(directory);
                string newDirectory = fileName + ".txt";

                File.WriteAllLines(Path.Combine(@"D:\test_cocos_proj\ResourcesWin\", newDirectory), loadList);
                foreach (var png in loadList)
                {
                    Console.WriteLine(newDirectory + " : " + png);
                }

                Console.WriteLine("\n");
            }
        }

        private static void RunFFmpegCommand(string ffmpegCommand)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegCommand,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }

        public static void Main(string[] args)
        {

            var file = @"D:\test_cocos_proj\Resources\HXTga";
            tgaConvertToPng(file);
            ReadPathToTxt(@"D:\test_cocos_proj\ResourcesWin\HXoffset");

        }
    }
}