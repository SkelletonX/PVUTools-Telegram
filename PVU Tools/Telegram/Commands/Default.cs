using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PVU_Tools.Telegram;

namespace PVU_Tools.Telegram.Commands
{
    public class Default
    {
        
        public static void Run(long messageid, string User)
        {
            TelegramBot.Bot.SendTextMessageAsync(messageid,"Ola, " + User+ " Seja Bem vindo ao PVU Tools, Acesse nossos Comandos em /help");

        }
    }
}
