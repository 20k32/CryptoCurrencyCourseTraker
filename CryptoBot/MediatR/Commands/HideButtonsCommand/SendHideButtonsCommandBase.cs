using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBot.MediatR.Commands.HideButtonsCommand
{
    internal class SendHideButtonsCommandBase : CommandToExternalServiceBase
    {
        public ReplyKeyboardRemove RemoveMarkup = null!;
        public BotApiSettings Settings = null!;

        public SendHideButtonsCommandBase(
            ITelegramBotClient botClient,
            Message message,
            CancellationToken
            cancellationToken,
            BotApiSettings botApiSettings,
            ReplyKeyboardRemove removeMarkup) : base(botClient, message, cancellationToken)
        {
            RemoveMarkup = removeMarkup;
            Settings = botApiSettings;
        }
    }
}
