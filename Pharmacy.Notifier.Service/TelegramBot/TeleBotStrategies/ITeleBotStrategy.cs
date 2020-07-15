using Telegram.Bot;
using Telegram.Bot.Args;

namespace Pharmacy.Notifier.Service
{
    public interface ITeleBotStrategy
    {
        void ProcessRequest(TelegramBotClient botClient, object sender, MessageEventArgs eventArgs);
    }
}
