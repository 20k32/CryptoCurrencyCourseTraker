using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBot.MediatR.Commands.HideButtonsCommand
{
    internal class SendHideButtonsCommand : SendHideButtonsCommandBase, IRequest<Message>
    {
        public SendHideButtonsCommand(
            ITelegramBotClient botClient,
            Message message,
            CancellationToken cancellationToken,
            BotApiSettings botApiSettings,
            ReplyKeyboardRemove removeMarkup) : base(botClient, message, cancellationToken, botApiSettings, removeMarkup)
        { }
    }
}
