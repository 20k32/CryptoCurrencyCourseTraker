using CryptoBot.ExternalStorageDataAccessors;
using CryptoBot.MediatR.Commands.CommandsToExternalServices.GetCurrenciesInRangeCommand;
using CryptoBot.MediatR.Commands.CommandsToExternalServices.GetDerivedCurrencyCommand;
using CryptoBot.MediatR.Commands.CommandsToExternalServices.GetTopCurrenciesCommand;
using CryptoBot.MediatR.Commands.HideButtonsCommand;
using CryptoBot.MediatR.Commands.PlainTextMessageCommand;
using CryptoBot.MediatR.Commands.ShowButtonsCommand;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBot.MediatR.Commands
{
    internal class CommandController
    {
        private SendPlainTextMessageCommand sendPlainTextMessageCommand = null!;
        private SendShowButtonsCommand sendShowButtonsCommand = null!;
        private SendHideButtonsCommand sendHideButtonsCommand = null!;
        private SendGetTopCurrenciesCommand sendGetTopCurrenciesCommand = null!;
        private SendGetCurrenciesInRangeCommand sendGetCurrenciesInRangeCommand = null!;
        private SendGetDerivedCurrencyCommand sendGetDerivedCurrencyCommand = null!;

        private IMediator Mediator;

        public CommandController(IMediator mediator)
        {
            Mediator = mediator;
        }

        public Task TrySendPlainTextMessageCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            if(sendPlainTextMessageCommand is not null)
            {
                sendPlainTextMessageCommand.BotClient = botClient;
                sendPlainTextMessageCommand.Message = message;
                sendPlainTextMessageCommand.CancellationToken = cancellationToken;
            }
            else
            {
                sendPlainTextMessageCommand = new(botClient, message, cancellationToken);
            }
            return Mediator.Send(sendPlainTextMessageCommand);
        }

        public Task TrySendShowButtonsCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, BotApiSettings settings)
        {
            if (sendShowButtonsCommand is not null)
            {
                sendShowButtonsCommand.BotClient = botClient;
                sendShowButtonsCommand.Message = message;
                sendShowButtonsCommand.CancellationToken = cancellationToken;
            }
            else
            {
                sendShowButtonsCommand = new(botClient, message, cancellationToken, settings);
            }
            return Mediator.Send(sendShowButtonsCommand);
        }

        public Task TrySendHideButtonsCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, BotApiSettings settings)
        {
            if (sendHideButtonsCommand is not null)
            {
                sendHideButtonsCommand.BotClient = botClient;
                sendHideButtonsCommand.Message = message;
                sendHideButtonsCommand.CancellationToken = cancellationToken;
            }
            else
            {
                sendHideButtonsCommand = new(botClient, message, cancellationToken, settings, new ReplyKeyboardRemove());
            }
            return Mediator.Send(sendHideButtonsCommand);
        }

        public Task TrySendGetTopCurrenciesCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, DataFetcher fetcher)
        {
            if(sendGetTopCurrenciesCommand is not null)
            {
                sendGetTopCurrenciesCommand.BotClient = botClient;
                sendGetTopCurrenciesCommand.CancellationToken = cancellationToken;
                sendGetTopCurrenciesCommand.Message = message;
            }
            else
            {
                sendGetTopCurrenciesCommand = new(botClient, message, fetcher, cancellationToken);
            }

            return Mediator.Send(sendGetTopCurrenciesCommand);
        }

        public Task TrySendGetCurrenciesInRangeCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, DataFetcher fetcher, int from, int to)
        {
            if (sendGetCurrenciesInRangeCommand is not null)
            {
                sendGetCurrenciesInRangeCommand.BotClient = botClient;
                sendGetCurrenciesInRangeCommand.CancellationToken = cancellationToken;
                sendGetCurrenciesInRangeCommand.Message = message;
                sendGetCurrenciesInRangeCommand.From = from;
                sendGetCurrenciesInRangeCommand.To = to;
            }
            else
            {
                sendGetCurrenciesInRangeCommand = new(botClient, message, fetcher, from, to, cancellationToken);
            }

            return Mediator.Send(sendGetCurrenciesInRangeCommand);
        }

        public Task TrySendGetDerivedCurrencyCommand(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, DataFetcher fetcher, BotApiSettings settings)
        {
            if (sendGetDerivedCurrencyCommand is not null)
            {
                sendGetDerivedCurrencyCommand.BotClient = botClient;
                sendGetDerivedCurrencyCommand.CancellationToken = cancellationToken;
                sendGetDerivedCurrencyCommand.Message = message;
            }
            else
            {
                sendGetDerivedCurrencyCommand = new(botClient, message, fetcher, settings, cancellationToken);
            }

            return Mediator.Send(sendGetDerivedCurrencyCommand);
        }
    }
}
