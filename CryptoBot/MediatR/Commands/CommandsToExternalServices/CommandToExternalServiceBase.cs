using CryptoBot.ExternalStorageDataAccessors;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.CommandsToExternalServices
{
    internal class CommandToExternalServiceBase : Commands.CommandToExternalServiceBase
    {
        public DataFetcher Fetcher = null!;

        public CommandToExternalServiceBase(
            ITelegramBotClient botClient,
            Message message,
            DataFetcher fetcher,
            CancellationToken cancellationToken) : base(botClient, message, cancellationToken)
        {
            Fetcher = fetcher;
        }
    }
}
