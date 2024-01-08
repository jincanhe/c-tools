using System;
using System.Diagnostics;
using System.IO;

namespace ffmpegTools
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // ffmpeg -i 1_2.tga -compression_level 0 1_2.png

            const string inputFolderPath = @"C:\Users\59822\Desktop\33108";
            const string outputPath = @"C:\Users\59822\Desktop\33108";
            string[] tgaFiles = Directory.GetFiles(inputFolderPath, "*.tga");
            foreach (var file in tgaFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string outputFilePath = Path.Combine(outputPath, $"{fileName}.png");

                // 执行FFmpeg命令
                string ffmpegCommand = $"-i \"{file}\" -compression_level 0 \"{outputFilePath}\"";
                RunFFmpegCommand(ffmpegCommand);
            }

            Console.WriteLine("转换完成！");
            Console.ReadLine();

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
    }
}