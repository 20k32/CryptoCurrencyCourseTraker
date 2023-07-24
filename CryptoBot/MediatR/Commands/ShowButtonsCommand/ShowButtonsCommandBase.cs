using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoBot.MediatR.Commands.ShowButtonsCommand
{
    internal class ShowButtonsCommandBase : CommandToExternalServiceBase
    {
        public ReplyKeyboardMarkup MainMenuButtons = null!;
        public BotApiSettings Settings = null!;

        public ShowButtonsCommandBase(ITelegramBotClient botClient,
            Message message,
            CancellationToken cancellationToken,
            BotApiSettings settings) : base(botClient, message, cancellationToken)
        {
            MainMenuButtons = new(new KeyboardButton[][]
            {
                new KeyboardButton[]
                {
                    new KeyboardButton(settings.DefaultCommandNames.GetTopCurrencies),
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(settings.DefaultCommandNames.GetPreviousPage),
                    new KeyboardButton(settings.DefaultCommandNames.GetNextPage)
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(settings.DefaultCommandNames.HideButtons)
                }
            })
            {
                ResizeKeyboard = true
            };

            Settings = settings;
        }
    }
}
