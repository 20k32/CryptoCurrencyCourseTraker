using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBot.MediatR.Commands
{
    internal class CommandToExternalServiceBase
    {
        public ITelegramBotClient BotClient = null!;
        public Message Message = null!;
        public CancellationToken CancellationToken;

        public CommandToExternalServiceBase(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken) =>
            (BotClient, Message, CancellationToken) = (botClient, message, cancellationToken);
    }
}
