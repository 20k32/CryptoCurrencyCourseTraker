using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot.MediatR.Commands.ShowButtonsCommand
{
    internal class SendShowButtonsCommand : ShowButtonsCommandBase, IRequest<Message>
    {
        public SendShowButtonsCommand(ITelegramBotClient botClient,
            Message message,
            CancellationToken cancellationToken,
            BotApiSettings settings) : base(botClient, message, cancellationToken, settings)
        {}
    }
}
