namespace CryptoBot
{
    internal class BotApiSettings
    {
        public string BotApiKey { get; set; } = null!;
        public string CommandToStopBot { get; set; } = null!;
        public string AdminNotificationOnStart { get; set; } = null!;
        public string AdminNotificationOnStop { get; set; } = null!;
        public string UserNotificationOnNoMatchesFound { get; set; } = null!;
        public string ShowButtonsUserNotification { get; set; } = null!;
        public string HideButtonsUserNotification { get; set; } = null!;
        public string ExternalStorage { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public CommandNames DefaultCommandNames { get; set; } = null!;
    }

    internal class CommandNames
    {
        public string GetTopCurrencies { get; set; } = null!;
        public string GetPreviousPage { get; set; } = null!;
        public string PostData { get; set; } = null!;
        public string GetNextPage { get; set; } = null!;
        public string ShowButtonsCommandDescription { get; set; } = null!;

        public string HideButtons { get; set; } = null!;
        public string HideButtonsCommand { get; set; } = null!;
        public string HideButtonsCommandDescription { get; set; } = null!;

        public string ShowButtonsCommand { get; set;} = null!;
        public string BeginConversation { get; set; } = null!;
    }
}
