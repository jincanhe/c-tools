using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using szjRenameChrName;

namespace ChrImageRenamer
{
    internal class Program
    {
        static string formatFilename(string filename)
        {
            // Missing
            //
            // cagethrow
            // place
            // retrieve
            // seed

            return filename
                        .Replace(" ", "")
                        .Replace("路", "")
                        .Replace("停", "")
                        .Replace("前", "d")
                        .Replace("右", "r")
                        .Replace("后", "u")
                        .Replace("背", "u")
                        .Replace("左", "l")
                        .Replace("举东西走", "lift_")
                        .Replace("打水", "refill_")
                        .Replace("收割", "sickle_")
                        .Replace("收获", "harvest_")
                        .Replace("敲石头", "pickaxe_")
                        .Replace("洒水", "watercan_")
                        .Replace("砍树", "axe_")
                        .Replace("耕地", "hoe_")
                        .Replace("钓鱼提竿", "finishfish_")
                        .Replace("钓鱼提杆", "finishfish_")
                        .Replace("钓鱼放竿", "fishcast_")
                        .Replace("钓鱼放杆", "fishcast_")
                        .Replace("钓鱼等待", "fishing_")
                        .Replace("铲掉", "shovel_")
                        .Replace("闲逛", "walk_")
                        .Replace("晕倒呼吸", "lie_")
                        .Replace("醒来呼吸", "lieb_")
                        .Replace("醒来睁眼", "lie_eye_")
                        .Replace("走", "")

                        ;
        }

        static void Chr()
        {
            var url = "http://192.168.1.8:3000/cards";
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

            var cardList = JsonConvert.DeserializeObject<List<Card>>(json);

            Dictionary<string, long> cardIdMap = new Dictionary<string, long>();
            Dictionary<string, long> npcIdMap = new Dictionary<string, long>();

            foreach (var card in cardList)
                cardIdMap[card.name] = card.id;

            cardIdMap["男主"] = 1;
            cardIdMap["女主"] = 2;

            httpRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.8:3000/npcList");
            httpRequest.Method = "GET";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";

            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                json = streamReader.ReadToEnd();
            }

            var npcList = JsonConvert.DeserializeObject<List<Npc>>(json);

            foreach (var npc in npcList)
                npcIdMap[npc.name] = npc.npcId;


            var targetPath = @"\\192.168.1.8\十洲记\小何\a_chr_output";

            var dirList = Directory.GetDirectories(@"\\192.168.1.8\十洲记\小何\a_orgin_chr_1.6");
            foreach (var dir in dirList)
            {
                var cardName = Path.GetFileName(dir).Replace("1.6倍PNG", "");

                if (!cardIdMap.ContainsKey(cardName))
                {
                    Console.WriteLine("不存在的卡牌：{0}", cardName);
                    continue;
                }
                var chrNo = cardIdMap[cardName];

                var chrDir = Path.Combine(targetPath, string.Format("chr{0}", chrNo));

                if (Directory.Exists(chrDir))
                    Directory.Delete(chrDir, true);

                Directory.CreateDirectory(chrDir);

                var files = Directory.GetFiles(dir, "*.png");

                Parallel.ForEach(files, file =>
                {
                    string orgName = Path.GetFileNameWithoutExtension(file);
                    string newNamePart = formatFilename(orgName);
                    string newName = String.Format("chr{0}_{1}.png", chrNo, newNamePart);
                    string newFullPath = Path.Combine(chrDir, newName);

                    File.Copy(file, newFullPath, true);
                });
            }
        }

        static void Npc()
        {
            var url = "http://192.168.1.8:3000/cards";
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

            var cardList = JsonConvert.DeserializeObject<List<Card>>(json);

            Dictionary<string, long> cardIdMap = new Dictionary<string, long>();
            Dictionary<string, long> npcIdMap = new Dictionary<string, long>();

            foreach (var card in cardList)
                cardIdMap[card.name] = card.id;

            cardIdMap["男主"] = 1;
            cardIdMap["女主"] = 2;

            httpRequest = (HttpWebRequest)WebRequest.Create("http://192.168.1.8:3000/npcList");
            httpRequest.Method = "GET";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";

            httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                json = streamReader.ReadToEnd();
            }

            var npcList = JsonConvert.DeserializeObject<List<Npc>>(json);

            foreach (var npc in npcList)
                npcIdMap[npc.name] = npc.npcId;


            var targetPath = @"\\192.168.1.8\十洲记\小何\a_npc_output";

            var dirList = Directory.GetDirectories(@"\\192.168.1.8\十洲记\小何\a_orgin_npc_1.6");
            foreach (var dir in dirList)
            {
                var cardName = Path.GetFileName(dir).Replace("1.6倍PNG", "");

                if (!cardIdMap.ContainsKey(cardName))
                {
                    Console.WriteLine("不存在的卡牌：{0}", cardName);
                    continue;
                }
                var chrNo = cardIdMap[cardName];

                var chrDir = Path.Combine(targetPath, string.Format("npc{0}", chrNo));

                if (Directory.Exists(chrDir))
                    Directory.Delete(chrDir, true);

                Directory.CreateDirectory(chrDir);

                var files = Directory.GetFiles(dir, "*.png");

                Parallel.ForEach(files, file =>
                {
                    string orgName = Path.GetFileNameWithoutExtension(file);
                    string newNamePart = formatFilename(orgName);
                    string newName = String.Format("npc{0}_{1}.png", chrNo, newNamePart);
                    string newFullPath = Path.Combine(chrDir, newName);

                    File.Copy(file, newFullPath, true);
                });
            }
        }

        static void Main(string[] args)
        {
            // Chr();
            // Npc();
        }
    }
}
