using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.PlainTextMessageCommand
{
    internal class SendPlainTextMessageCommand : CommandToExternalServiceBase, IRequest<Message>
    {
        public SendPlainTextMessageCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken) : base(botClient, message, cancellationToken)
        { }
    }
}
