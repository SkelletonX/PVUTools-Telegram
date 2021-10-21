using NLogWrapper;
using PVU_Tools.PVUService;
using PVU_Tools.PVUService.Models.PVU;
using PVU_Tools.Telegram;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PVU_Tools.Managers
{
    public class PvuToolMgr
    {
        public static void RunTool(long id, string user)
        {
            ILogger Logger = LogManager.CreateLogger(typeof(TelegramBot));
            try
            {
                List<string> plantsWaterNeed = new List<string>();
                List<string> plantscrow = new List<string>();
                List<string> plantWithSeed = new List<string>();
                List<string> plantNeedHarvest = new List<string>();

         

                PlantsService._token = ""; //SEU TOKEN!!!
                PlantsService._idtelegram = id;

                var res = PlantsService.getFarmInfo(id);

                Console.WriteLine(res.data);

                
                while (true)
                {
                    TelegramBot.Bot.SendTextMessageAsync(id, "Verificando plantação "+ DateTime.Now);
                    Logger.Debug("Verificando plantação do usuario "+ user);

                    if (res != null)
                    {
                        foreach (var plant in res.data)
                        {
                            
                            if (plant.needWater)
                            {
                                if (plantsWaterNeed.FirstOrDefault(x => x == plant._id) == null)
                                {
                                    TelegramBot.Bot.SendTextMessageAsync(id,"A " + plant._id + " precisa de água");
                                    plantsWaterNeed.Add(plant._id);
                                    PlantsService.UseTool(plant._id,3);
                                }
                            }
                            else
                            {
                                if (plantsWaterNeed.FirstOrDefault(x => x == plant._id) != null)
                                    plantsWaterNeed.Remove(plant._id);
                            }
                            if (plant.stage == "paused")
                            {
                                if (plantscrow.FirstOrDefault(x => x == plant._id) == null)
                                {
                                    TelegramBot.Bot.SendTextMessageAsync(id, "Na planta " + plant._id + " tem um corvo");
                                    PlantsService.UseTool(plant._id, 4);
                                    plantscrow.Add(plant._id);
                                }
                            }
                            else
                            {
                                if (plantscrow.FirstOrDefault(x => x == plant._id) != null)
                                    plantscrow.Remove(plant._id);
                            }
                            if (plant.hasSeed)
                            {
                                if (plantWithSeed.FirstOrDefault(x => x == plant._id) == null)
                                {
                                    TelegramBot.Bot.SendTextMessageAsync(id, "A planta " + plant._id + " tem uma semente");
                                    plantWithSeed.Add(plant._id);
                                }
                            }
                            else
                            {
                                if (plantWithSeed.FirstOrDefault(x => x == plant._id) != null)
                                    plantWithSeed.Remove(plant._id);
                            }
                            if (plant.totalHarvest != 0)
                            {
                                if (plantNeedHarvest.FirstOrDefault(x => x == plant._id) == null)
                                {
                                    TelegramBot.Bot.SendTextMessageAsync(id, "A planta " + plant._id + " está pronto para colher os LE");
                                    plantNeedHarvest.Add(plant._id);

                                    if(plantNeedHarvest.Count > 0) {
                                        PlantsService.HarvestAll();
                                        TelegramBot.Bot.SendTextMessageAsync(id, "A planta " + plant._id + " foi colhido o LE");
                                    }
                                    
                                }
                            }
                            else
                            {
                                if (plantNeedHarvest.FirstOrDefault(x => x == plant._id) != null)
                                    plantNeedHarvest.Remove(plant._id);

                            }
                        } 
                    }
                    Random rd = new Random();
                    Thread.Sleep(rd.Next(120000, 220000));// 3min - 4min
                } 
            }
            catch (Exception e)
            {
                Logger.Error("PvuToolMgr: " + e);   
            }
        }
    }
}
