using Newtonsoft.Json.Schema;
using System.Drawing;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


class Program
{
    private static void Main(string[] args)
    {
        var tokenBot = "6261177727:AAF8js_sluqbRtMCYLv4DFLQBknS00PsokE";
        var botClient = new TelegramBotClient(tokenBot);

        botClient.StartReceiving(Update, Error);

        Console.ReadLine();

        

    }
    static ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton("Создать Мероприятие"), new KeyboardButton("Инфо") }) { ResizeKeyboard = true };

    private static Task Error(ITelegramBotClient botClient, Exception ex, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;
        // логи сообщений боту
        await Console.Out.WriteLineAsync($"Получено сообщение от | {DefineWhatTheName(message.Chat.Id)} | текст: \"{message.Text}\" " +
            $"| время: {message.Date} |{message.Chat.Username}" ); 

        // ответ на слово привет
        if (message.Text == "123")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id,"privet",replyMarkup: replyKeyboardMarkup);

            return;
        }
        else if (message.Text.ToLower() == "создать мероприятие")
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Создем меропиятие", replyMarkup: replyKeyboardMarkup);
        }
        

    }
    

    public static string DefineWhatTheName(long chatId)
    {
        var dictNames = new Dictionary<long, string>()
        {
            { 541629575 , "АНЯ" },
            { 1421928229 , "КЛЕВАНОВИЧ"},
            { 1773298820 , "РОБЕРТ"},
            { 351666992 , "ВИТЯ"},
            { 788497109 , "ВАЛЕРА"},
            { 818843347 , "СОНЯ"},
            { 1058420542 ,"ДЮКСЕР БМХ" },
            { 773435858 , "РУСЛАКАНТУС"},
            { 835245013 , "ВАНЕЧЕК"},
            { 511563830 , "САША ДЕВОЧКА"},
            { 901364239 ,"МАША СУЕТА" },
            { 981528344 ,"МАРГО" }
        };
        foreach (var dict in dictNames)
        {
            if (dict.Key == chatId)
                return dict.Value;
        }
        return $"Неизвестный{chatId}";
    }

}