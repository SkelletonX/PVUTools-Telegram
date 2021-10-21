using NLogWrapper;
using PVU_Tools.PVUService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;


namespace PVU_Tools.Telegram
{
    public class TelegramBot
    {
        public static TelegramBotClient Bot;

        readonly ILogger Logger = LogManager.CreateLogger(typeof(TelegramBot));
        [Obsolete]
        public void InitializeBotClient()
        {
            try
            {

                Logger.Debug("Iniciando Sistema de TelegramBot");
                Bot = new TelegramBotClient("TOKEN DO SEU BOT TELEGRAM");
                Bot.OnMessage += Bot_OnMessageReceived;
                Bot.OnCallbackQuery += BotOnCallbackQueryRecieved;
                Bot.StartReceiving();
            }catch(Exception e)
            {
                Logger.Error("InitializeBotClient: " + e);
                //Bot.StopReceiving();
            }
           
        }

        [Obsolete]
        private static async void BotOnCallbackQueryRecieved(object sender, CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Você apertou o botão { buttonText}");
        }
        

        [Obsolete]
        private async void Bot_OnMessageReceived(object sender, MessageEventArgs e)
        {
            try
            {
                var message = e.Message;

                if (message == null || message.Type != MessageType.Text)
                    return;

                string name = $"{message.From.FirstName} {message.From.LastName}";

               
                switch (message.Text)
                {
                    case "/newaccount":
                        new Thread(() => { Commands.Newaccount.Run(message.From.Id, name); }).Start();
                        break;
                    case "/start":
                        Bot.StartReceiving();
                        break;
                    case "/token":
                        new Thread(() => { Commands.Token.Run(message.From.Id); }).Start();
                        break;
                    case "/startpvu":
                        new Thread(() => { Managers.PvuToolMgr.RunTool(message.From.Id, name); }).Start();
                        break;
                    case "/help":
                        break;

                    default:
                        Commands.Default.Run(message.From.Id, name);
                        Logger.Info(message.From.Username + " Escreveu " + message.Text);
                        break;

                }
            }
            catch (Exception ex)
            {
                Logger.Error("Bot_OnMessageReceived" + ex);
            }
        }
    }
}
