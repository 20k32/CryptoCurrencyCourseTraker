using Amazon.Runtime.Internal;
using CryptoBot.ExternalStorageDataAccessors;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.CommandsToExternalServices.GetDerivedCurrencyCommand
{
    internal class SendGetDerivedCurrencyCommand : CommandToExternalServiceBase, IRequest<Message>
    {
        public BotApiSettings Settings = null!;

        public SendGetDerivedCurrencyCommand(
            ITelegramBotClient botClient,
            Message message,
            DataFetcher fetcher,
            BotApiSettings settings,
            CancellationToken cancellationToken) : base(botClient, message, fetcher, cancellationToken)
        {
            Settings = settings;
        }
    }
}
