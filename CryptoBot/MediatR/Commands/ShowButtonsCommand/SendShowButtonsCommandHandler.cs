using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.ShowButtonsCommand
{
    internal class SendShowButtonsCommandHandler : IRequestHandler<SendShowButtonsCommand, Message>
    {
        public Task<Message> Handle(SendShowButtonsCommand request, CancellationToken cancellationToken)
        {
            return request.BotClient.SendTextMessageAsync(
                request.Message.Chat.Id,
                text: request.Settings.ShowButtonsUserNotification,
                replyMarkup: request.MainMenuButtons,
                cancellationToken: cancellationToken);
        }
    }
}
