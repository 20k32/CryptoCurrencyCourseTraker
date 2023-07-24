using CryptoBot.ExternalStorageDataAccessors;
using MediatR;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.CommandsToExternalServices.GetDerivedCurrencyCommand
{
    internal class SendGetDerviedCurrencyCommandHandler : IRequestHandler<SendGetDerivedCurrencyCommand, Message>
    {
        public async Task<Message> Handle(SendGetDerivedCurrencyCommand request, CancellationToken cancellationToken)
        {
            var resultList = await request.Fetcher.GetEntriesByNameOrFullName(request.Message.Text!);

            var response = new StringBuilder();

            if(resultList!.CurrencyList.Length != 0)
            {
                foreach( var result in resultList.CurrencyList.Take(40))
                {
                    response.AppendLine(result.ToStringInPagination() + "\n");
                }
            }
            else
            {
                response.AppendLine(request.Settings.UserNotificationOnNoMatchesFound);
            }

            var echo = await request.BotClient.SendTextMessageAsync(
                request.Message.Chat.Id,
                response.ToString(),
                cancellationToken: cancellationToken);

            return echo;
        }
    }
}
