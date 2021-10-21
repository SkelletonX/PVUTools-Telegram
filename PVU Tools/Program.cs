using NLogWrapper;
using PVU_Tools.Telegram;
using System;

namespace PVU_Tools
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>


        [STAThread]
        [Obsolete]
        static void Main()
        {

            ILogger Logger = LogManager.CreateLogger(typeof(Program));
            Logger.Info("PVU Tools Iniciando...");
            TelegramBot bot = new TelegramBot();
            bot.InitializeBotClient(); 

            Console.ReadKey();
        }

    }
}
