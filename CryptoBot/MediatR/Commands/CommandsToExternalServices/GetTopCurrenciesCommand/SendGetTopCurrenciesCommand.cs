using CryptoBot.ExternalStorageDataAccessors;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.CommandsToExternalServices.GetTopCurrenciesCommand
{
    internal class SendGetTopCurrenciesCommand : CommandToExternalServiceBase, IRequest<Message>
    {
        public SendGetTopCurrenciesCommand(
            ITelegramBotClient botClient,
            Message message,
            DataFetcher fetcher,
            CancellationToken cancellationToken) : base(botClient, message, fetcher, cancellationToken)
        {
        }
    }
}
