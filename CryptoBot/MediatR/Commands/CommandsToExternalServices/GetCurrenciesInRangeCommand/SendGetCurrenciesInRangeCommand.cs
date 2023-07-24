using CryptoBot.ExternalStorageDataAccessors;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.CommandsToExternalServices.GetCurrenciesInRangeCommand
{
    internal class SendGetCurrenciesInRangeCommand : CommandToExternalServiceBase, IRequest<Message>
    {
        public int From;
        public int To;

        public SendGetCurrenciesInRangeCommand(ITelegramBotClient botClient,
            Message message, 
            DataFetcher fetcher,
            int from,
            int to,
            CancellationToken cancellationToken) : base(botClient, message, fetcher, cancellationToken)
        {
            From = from;
            To = to;
        }
    }
}
