using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
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
    static ReplyKeyboardMarkup replyKeyboardMarkupNewEvent = new(new[] { new KeyboardButton("Отослать всем"), new KeyboardButton("Отмена"),
        new KeyboardButton("Отослать Некоторым(?)") }) { ResizeKeyboard = true };

    static ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton("Создать Мероприятие"), 
        new KeyboardButton("Инфо") }) { ResizeKeyboard = true };

    static ReplyKeyboardMarkup replyKeyboardMarkupComeBack = new(new[] { new KeyboardButton("Вернуться"),
        new KeyboardButton("Инфо") })
    { ResizeKeyboard = true };

    static List<BotCommand> commands = new List<BotCommand>();
    

    private static Task Error(ITelegramBotClient botClient, Exception ex, CancellationToken token)
    {
        throw new NotImplementedException();
    }



    private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
    {
        var message = update.Message;


        commands.Add(new BotCommand { Command = @"start", Description = @"Стартуем комаровцы" });
        commands.Add(new BotCommand { Command = @"help", Description = @"Макака не знает функций" });
        commands.Add(new BotCommand { Command = @"create", Description = @"Создаем тусу джусу" });
        commands.Add(new BotCommand { Command = @"thanks", Description = @"Сказать спасибо разработчикам" });
        await botClient.SetMyCommandsAsync(commands);
        
        if(message.Type == MessageType.Text)
        {
            // логи сообщений боту
            string logLine = $"Получено сообщение от |\t {DefineWhatTheName(message.Chat.Id)} |\t текст: \"{message.Text}\" " +
            $"|\t время: {message.Date} |{message.Chat.Username}";

            // вывод на консоль
            await Console.Out.WriteLineAsync(logLine);

            // вывод в файл
            string fileName = @"C:\Users\vita2\source\repos\botTG\botTG\logs.txt";
            



            using (StreamWriter writer = new StreamWriter(fileName, true)) // the second parameter indicates we want to append to the file
            {
                writer.WriteLine(logLine);
            }


            // ответ на слово привет
            if (message.Text == @"/start")
            {
                await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Приветсвтую комаровец! \n Рад что вижу тебя здесь! " +
                    "\n Открой меню и выбери то что хочешь сделать", replyMarkup: replyKeyboardMarkup);

                return;
            }
            if (message.Text == @"/help")
            {
                await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                await botClient.SendTextMessageAsync(message.Chat.Id, "я если че тоже не знаю функций", replyMarkup: replyKeyboardMarkup);

                return;
            }
            if (message.Text == @"/create")
            {
                await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Лееее мужик че тусу хочешь сделать?", replyMarkup: replyKeyboardMarkup);

                return;
            }
            if (message.Text == @"/thanks")
            {
                await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                await botClient.SendTextMessageAsync(message.Chat.Id, "угу", replyMarkup: replyKeyboardMarkup);

                return;
            }

            else if (message.Text.ToLower() == "создать мероприятие")
            {
                await botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Создем меропиятие\n\n Кому будем отсылать рассылку? \n Выбери 1 из 3 вариантов",
                    replyMarkup: replyKeyboardMarkupNewEvent);

            }
            if (message.Text.ToLower() == "отослать всем")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Впиши сюда то что будешь рассылать и отправь");
            }


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