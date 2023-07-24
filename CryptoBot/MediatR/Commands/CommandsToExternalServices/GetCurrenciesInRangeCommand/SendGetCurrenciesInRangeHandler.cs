using CryptoBot.ExternalStorageDataAccessors.Models;
using MediatR;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.CommandsToExternalServices.GetCurrenciesInRangeCommand
{
    internal class SendGetCurrenciesInRangeHandler : IRequestHandler<SendGetCurrenciesInRangeCommand, Message>
    {
        public async Task<Message> Handle(SendGetCurrenciesInRangeCommand request, CancellationToken cancellationToken)
        {
            var data = await request.Fetcher.GetEntriesRange(request.From, request.To)
                ?? new(Array.Empty<CurrencyListItemModel>());

            var builder = new StringBuilder();


            int counter = request.From + 1;

            foreach (var item in data)
            {
                builder.AppendLine(counter++ + ". " + item.ToStringInPagination() + "\n");
            }

            var echo = await request.BotClient.SendTextMessageAsync(
                request.Message.Chat.Id,
                builder.ToString(),
                cancellationToken: cancellationToken);

            return echo;
        }
    }
}
