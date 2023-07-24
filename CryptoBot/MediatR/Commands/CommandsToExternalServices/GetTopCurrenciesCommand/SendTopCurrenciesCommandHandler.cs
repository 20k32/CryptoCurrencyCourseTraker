using MediatR;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.CommandsToExternalServices.GetTopCurrenciesCommand
{
    internal class SendTopCurrenciesCommandHandler : IRequestHandler<SendGetTopCurrenciesCommand, Message>
    {
        public async Task<Message> Handle(SendGetTopCurrenciesCommand request, CancellationToken cancellationToken)
        {
            StringBuilder builder = new();

            var currencyList = await request.Fetcher.GetEntriesRange(0, 50);

            int counter = 1;

            foreach (var currency in currencyList)
            {
                builder.AppendLine($"{counter++}. {currency.ToShortStringInTopCurrencies()}");
            }

            var echo = await request.BotClient.SendTextMessageAsync(
                request.Message.Chat.Id,
                builder.ToString(),
                cancellationToken: cancellationToken);

            return echo;
        }
    }
}
