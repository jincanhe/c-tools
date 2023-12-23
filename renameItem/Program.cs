using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace renameItem
{
    internal class Program
    {
        static void clearDirectory(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);

            foreach (var file in files)
            {
                File.Delete(file);
            }

            Console.WriteLine("删除文件夹：" + directoryPath);

        }
        private static void cope(string sourcePath, string desPath)
        {
            clearDirectory(desPath);
            var files = Directory.GetFiles(sourcePath);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destinationPath = Path.Combine(desPath, fileName);
                File.Copy(file, destinationPath, true); // true 表示允许覆盖同名文件
            }

            Console.WriteLine("文件拷贝完成" + sourcePath + "to" + desPath);
        }

        static void renameHandler()
        {
            HashSet<long> existingSet = new HashSet<long>();
            Dictionary<string, long> nameIdMap = new Dictionary<string, long>();

            var url = "http://192.168.1.8:3000/items";
            string json = null;

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "GET";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                json = streamReader.ReadToEnd();
            }

            var items = JsonSerializer.Deserialize<List<ItemData>>(json);

            Parallel.ForEach(items, item =>
            {
                long itemId = item.itemId;
                string name = item.name;
                string resName = "zs_" + itemId;
                string destinationDirectory = @"Z:/小何/spine/装饰建筑（单个动画）";

                lock (existingSet)
                {
                    existingSet.Add(itemId);
                    nameIdMap[name] = itemId;
                }

                var fileSuffix = new List<string>(){ ".atlas", ".json", ".png" };

                foreach (var suffix in fileSuffix)
                {
                    var tempFile = name + suffix;
                    var sourcePath = Path.Combine(destinationDirectory, tempFile);
                    if (File.Exists(sourcePath))
                    {
                        var tempResName = resName + suffix;
                        var desPath = Path.Combine(destinationDirectory, tempResName);
                        File.Move(sourcePath, desPath);
                        Console.WriteLine(sourcePath + "        ====rename====      " + desPath);

                        if (suffix == ".atlas")
                        {
                            // 读取原始文本文件内容
                            string fileContent = File.ReadAllText(desPath);
                            // 在文件内容中进行字符串替换
                            string replacedContent = fileContent.Replace(name, resName);
                            // 写入替换后的内容到同一文件
                            File.WriteAllText(desPath, replacedContent);
                        }


                    }
                }

            });
        }

        static void Main(string[] args)
        {
            cope(@"Z:/享耳/场景物品导出/装饰建筑（单个动画）", @"Z:/小何/spine/装饰建筑（单个动画）");
            renameHandler();
        }
    }
}