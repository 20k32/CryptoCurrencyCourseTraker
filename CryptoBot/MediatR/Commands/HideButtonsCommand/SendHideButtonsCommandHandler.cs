using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.HideButtonsCommand
{
    internal class SendHideButtonsCommandHandler : IRequestHandler<SendHideButtonsCommand, Message>
    {
        public Task<Message> Handle(SendHideButtonsCommand request, CancellationToken cancellationToken)
        {
            return request.BotClient.SendTextMessageAsync(
                request.Message.Chat.Id,
                text: request.Settings.HideButtonsUserNotification,
                replyMarkup: request.RemoveMarkup,
                cancellationToken: cancellationToken);
        }
    }
}
