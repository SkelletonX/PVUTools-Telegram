using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NLogWrapper;
using PVU_Tools.config;
using PVU_Tools.Telegram;

namespace PVU_Tools.PVUService
{
    
    class PlantsService
    {
        static ILogger Logger = LogManager.CreateLogger(typeof(PlantsService));

        public static long _idtelegram;
        public static string _token;
        /// <summary>
        /// Get Farm Info
        /// </summary>
        /// <returns></returns>
        public static Models.PVU.GetFarmResponse getFarmInfo(long id)
        {
            try
            {
        
                var jsonString = getRequest("https://backend-farm.plantvsundead.com/farms?limit=10&offset=0", "GET");
                var objRes = JsonSerializer.Deserialize<Models.PVU.GetFarmResponse>(jsonString);
                return objRes;
            }
            catch (Exception ex)
            {
                Logger.Error("getFarmInfo:" + ex);
                TelegramBot.Bot.SendTextMessageAsync(id, "Ops, Seu Token esta incorreto!");
                return null;
            }

        }
        /// <summary>
        /// Use Tool Method
        /// </summary>
        /// <param name="farmId">_id</param>
        /// <param name="toolId">1 Small Port, 2 Big POT, 3 water, 4 scarecrow, 5 greenhouse</param>
        /// <returns></returns>
        public static Models.PVU.GetFarmResponse UseTool(string farmId, int toolId)
        {
            try
            {
                var rq = new Models.PVU.ApplyToolRequest()
                {
                    farmId = farmId,
                    toolId = toolId,
                    token = new Models.PVU.Token()
                    {
                        challenge = "default",
                        seccode = "default",
                        validate = "default"
                    }
                };
                var jsonString = getRequest("https://backend-farm.plantvsundead.com/farms/apply-tool", "POST", JsonSerializer.Serialize(rq).ToString());
                var objRes = JsonSerializer.Deserialize<Models.PVU.GetFarmResponse>(jsonString);
                return objRes;
            }
            catch (Exception ex)
            {
                Logger.Error("UseTool "+ex);
                return null;
            }

        }
        /// <summary>
        /// WordTree Response
        /// </summary>
        /// <returns></returns>
        public static Models.PVU.WordTreeResponse GetWordTreeInfo()
        {
            try
            {

                var jsonString = getRequest("https://backend-farm.plantvsundead.com/world-tree/datas", "GET");
                var objRes = JsonSerializer.Deserialize<Models.PVU.WordTreeResponse>(jsonString);
                return objRes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// Buy Sun Flowers
        /// </summary>
        /// <param name="farmId">amount</param>
        /// <param name="toolId">1 Sunflower Sapling, 2 Sunflower mama, 3 Sun Box</param>
        /// <returns></returns>
        public static Models.PVU.BuySunFlowerResponse BuySunFlowers(int amount, int sunFlowerType)
        {
            try
            {
                var rq = new Models.PVU.BuySunFlowerRequest()
                {
                    amount = amount,
                    sunflowerId = sunFlowerType
                };
                var jsonString = getRequest("https://backend-farm.plantvsundead.com/buy-sunflowers", "POST", JsonSerializer.Serialize(rq).ToString());
                var objRes = JsonSerializer.Deserialize<Models.PVU.BuySunFlowerResponse>(jsonString);
                return objRes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        /// <summary>
        /// Buy Sun Flowers
        /// </summary>
        /// <param name="farmId">amount</param>
        /// <param name="toolId">1 Sunflower Sapling, 2 Sunflower mama, 3 Sun Box</param>
        /// <returns></returns>
        public static Models.PVU.BuySunFlowerResponse HarvestAll()
        {
            try
            {
                var jsonString = getRequest("https://backend-farm.plantvsundead.com/farms/harvest-all", "POST");
                var objRes = JsonSerializer.Deserialize<Models.PVU.BuySunFlowerResponse>(jsonString);
                return objRes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static string checkUserAgent()
        {
            return getRequest("https://www.whatismybrowser.com/es/detect/what-is-my-user-agent", "GET");
        }

        private static string getRequest(string url, string method, string dataToPost = "")
        {
            WebClient client = new WebClient();

            // Add a user agent header in case the 
            // requested URI contains a query.

            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.61 Safari/537.36");
            client.Headers.Add("Origin", "https://marketplace.plantvsundead.com");
            client.Headers.Add("Referer", "https://marketplace.plantvsundead.com/");
            client.Headers.Add("Authorization", "Bearer Token: "+_token); //localStorage.getItem("token"); HTML5
            client.Headers[HttpRequestHeader.ContentType] = "application/json";


            var b1 = client.DownloadString(url);
            JObject json = JObject.Parse(b1);

            if(json["status"].ToString() == "1")
            {
                TelegramBot.Bot.SendTextMessageAsync(_idtelegram, "Seu Token esta incorreto!, caso o erro persistir entre contato com suporte!.");
            }

            if (method == "GET")
            {
                return client.DownloadString(url);
            }
            else if (method == "POST")
            {
                return client.UploadString(url, dataToPost);
            }
            else return "";
        }
    }
}
