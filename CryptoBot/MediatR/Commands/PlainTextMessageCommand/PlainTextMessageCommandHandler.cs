using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.PlainTextMessageCommand
{
    internal class PlainTextMessageCommandHandler : IRequestHandler<SendPlainTextMessageCommand, Message>
    {
        public Task<Message> Handle(SendPlainTextMessageCommand request, CancellationToken cancellationToken)
        {
            return request.BotClient.SendTextMessageAsync(request.Message.Chat.Id, request.Message.Text!, cancellationToken: cancellationToken);
        }
    }
}
