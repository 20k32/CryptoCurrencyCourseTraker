using AngleSharp.Html;
using CryptoBot.Controllers;
using CryptoBot.ExternalStorageDataAccessors;
using CryptoBot.MediatR.Commands;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CryptoBot
{
    internal sealed class TelegramBotController : TelegramBotControllerBase
    {
        private const int ENTITIES_PER_PAGE_COUNT = 20;
        private const int MAX_PAGES = 908;

        private int From = -1;

        private TelegramBotClient BotClient = null!;
        private DataFetcher Fetcher = null!;

        public TelegramBotController(
            IOptionsSnapshot<BotApiSettings> settings,
            CommandController commandController,
            DataFetcher fetcher) : base(settings, commandController)
        {
            BotClient = new(Settings.BotApiKey);
            Fetcher = fetcher;
        }

        public override Task ConfigureBot()
        {
            return BotClient.SetMyCommandsAsync(new BotCommand[]
            {
                new BotCommand()
                {
                    Command = Settings.DefaultCommandNames.ShowButtonsCommand,
                    Description = Settings.DefaultCommandNames.ShowButtonsCommandDescription
                },

                new BotCommand()
                {
                    Command = Settings.DefaultCommandNames.HideButtonsCommand,
                    Description = Settings.DefaultCommandNames.HideButtonsCommandDescription
                }
            });
        }

        public override async Task StartBot()
        {
            await LogToAdminUI(Settings.AdminNotificationOnStart);

            using (var cts = new CancellationTokenSource())
            {
                BotClient.StartReceiving(HandleUpdateAsync, PollingErrorHandler, Options, cts.Token);

                await ConditionToStopBot();

                cts.Cancel();

                await LogToAdminUI(Settings.AdminNotificationOnStop);
            }
        }

        private Task PollingErrorHandler(ITelegramBotClient botClient, Exception ex, CancellationToken cancellationToken)
        {
            return LogToAdminUI(ex.Message);
        }

        private Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is null)
            {
                return Task.CompletedTask;
            }

            if (update.Message.Text is not null)
            {
                return HandleTextMessage(botClient, update.Message, cancellationToken);
            }

            return Task.CompletedTask;
        }

        private Task HandleTextMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            if (message.Text == Settings.DefaultCommandNames.GetTopCurrencies)
                return Commands.TrySendGetTopCurrenciesCommand(botClient, message, cancellationToken, Fetcher);
            if (message.Text == Settings.DefaultCommandNames.GetNextPage)
            {
                From = From < MAX_PAGES ? From + 1 : MAX_PAGES;
                
                return Commands.TrySendGetCurrenciesInRangeCommand(botClient, message, cancellationToken, Fetcher, From * ENTITIES_PER_PAGE_COUNT, ENTITIES_PER_PAGE_COUNT);
            }
            if (message.Text == Settings.DefaultCommandNames.PostData)
                return Commands.TrySendPlainTextMessageCommand(botClient, message, cancellationToken);
            if (message.Text == Settings.DefaultCommandNames.GetPreviousPage)
            {
                From = From < 1 ? 0 : From - 1;

                return Commands.TrySendGetCurrenciesInRangeCommand(botClient, message, cancellationToken, Fetcher, From * ENTITIES_PER_PAGE_COUNT, ENTITIES_PER_PAGE_COUNT);
            }
            if (message.Text == Settings.DefaultCommandNames.ShowButtonsCommand)
            {
                return Commands.TrySendShowButtonsCommand(botClient, message, cancellationToken, Settings);
            }
            if (message.Text == Settings.DefaultCommandNames.HideButtons
               || message.Text == Settings.DefaultCommandNames.HideButtonsCommand)
            {
                return Commands.TrySendHideButtonsCommand(botClient, message, cancellationToken, Settings);
            }
            else
            {
                return Commands.TrySendGetDerivedCurrencyCommand(botClient, message, cancellationToken, Fetcher, Settings);
            }
        }
    }
}
