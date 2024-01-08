using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ccstudioToGame
{
    internal class Program
    {
        private static void MovePlist(List<string> tempPlistList)
        {

            const string basePath = @"D:\work\szj-game\Resources\res\plist";

            var updatePlistList = new List<string>();
            foreach (var fileName in tempPlistList)
            {
                var filePath_png = Path.Combine(basePath,  Path.ChangeExtension(fileName, ".png"));
                var filePath_plist = Path.Combine(basePath,  Path.ChangeExtension(fileName, ".plist"));
                updatePlistList.Add(filePath_png);
                updatePlistList.Add(filePath_plist);
            }



            var targetPaths = new List<string>()
            {
                @"D:\szj-game\Resources\res\plist",
                @"D:\szj-game\ResourcesWin\res\plist"
            };

            foreach (var filePath in updatePlistList)
            {
                var relativePath = filePath.Replace(@"D:\work\szj-game\Resources\res\plist\", "");

                foreach (var targetPath in targetPaths)
                {
                    var destinationPath = Path.Combine(targetPath, relativePath);

                    var destinationFolder = Path.GetDirectoryName(destinationPath);
                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    File.Copy(filePath, destinationPath, true);
                    Console.WriteLine(filePath + "  to  " + destinationPath);
                }

            }

            Console.WriteLine();
        }

        private static void MoveCsb(List<string> updateList)
        {

            const string basePath = @"D:\work\szj-game\Resources\res\game";

            //lingQ用法。。
            var updateCsbList = updateList.Select(fileName => Path.Combine(basePath, fileName)).ToList();

            var targetPaths = new List<string>()
            {
                @"D:\szj-game\Resources\res\game",
                @"D:\szj-game\ResourcesWin\res\game",
            };



            foreach (var filePath in updateCsbList)
            {
                var relativePath = filePath.Replace(@"D:\work\szj-game\Resources\res\game\", "");

                foreach (var targetPath in targetPaths)
                {
                    var destinationPath = Path.Combine(targetPath, relativePath);

                    var destinationFolder = Path.GetDirectoryName(destinationPath);

                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("不存在: "+ filePath);
                        continue;
                    }

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                        Console.WriteLine("创建文件夹: " +  destinationFolder);

                    }

                    File.Copy(filePath, destinationPath, true);
                    Console.WriteLine(filePath + "  to  " + destinationPath);
                }


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

        private static void Movefile(List<string> fileList)
        {
            const string basePath = @"D:\work\szj-game\Resources\res\game";

            foreach (var filePath in fileList)
            {
                var tempList = new List<string>();
                var curPath = Path.Combine(basePath, filePath);
                // 检查目录是否存在
                if (Directory.Exists(curPath))
                {
                    string[] files = Directory.GetFiles(curPath);
                    foreach (string csbpath in files)
                    {
                        var csbName = csbpath.Replace(@"D:\work\szj-game\Resources\res\game\", "");
                        tempList.Add(csbName);
                    }

                    MoveCsb(tempList);
                }
                else
                {
                    Console.WriteLine("目录不存在:" + curPath);
                }
            }
        }

        public static void Main(string[] args)
        {
            var updateList = new List<string>()
            {
                @"main\mainView.csb",
                // @"chouka\chouka_main.csb",
                // @"chouka\chou_reward.csb",
                // @"Task\NewNewTaskView.csb",
                // @"login\LoginPhoneView.csb",
                // @"minimap\mapUnLockView.csb",
                // @"buildView\selectCraftView.csb",
                // @"licheng\main_1.csb",
                // @"commonPop\MineReward.csb",
                // @"commonPop\LoginWaitView.csb",
                // @"commonPop\buySomethingView.csb",
                // @"buildView\BuildView.csb",
            };

            var tempPlistList = new List<string>()
            {
                // "chouka",

                "mainUI",
                // "selectCardUI",
                // "common",
                // "task",
                // "buildUI"
            };

            var fileList = new List<string>()
            {
                // @"Task",
            };

            MovePlist(tempPlistList);
             MoveCsb(updateList);
            Movefile(fileList);
            // SjzPublishr();
            Console.WriteLine("done!");
            DateTime currentTime = DateTime.Now;
            Console.WriteLine(currentTime);

        }
    }
}