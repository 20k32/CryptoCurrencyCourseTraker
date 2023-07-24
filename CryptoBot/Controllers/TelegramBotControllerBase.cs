using CryptoBot.MediatR.Commands;
using MediatR;
using Microsoft.Extensions.Options;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CryptoBot.Controllers
{
    internal abstract class TelegramBotControllerBase
    {
        protected BotApiSettings Settings = null!;
        protected ReceiverOptions Options = null!;
        protected CommandController Commands = null!;

        public Func<Task<string>> ServerCommandFromAdminHandler = null!;
        public Func<string, Task> LogToAdminUI = null!;

        public TelegramBotControllerBase(IOptionsSnapshot<BotApiSettings> settings, CommandController commandController) 
        {
            Settings = settings.Value;
            Commands = commandController;

            Options = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
        }

        public abstract Task ConfigureBot();
        public abstract Task StartBot();

        public virtual async Task ConditionToStopBot()
        {
            while (await ServerCommandFromAdminHandler() != Settings.CommandToStopBot)
            {
                Console.Clear();
                await LogToAdminUI(Settings.AdminNotificationOnStart);
            }
        }
    }
}
