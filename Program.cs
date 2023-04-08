using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace botTG;

static class Program
{
    
    const string tokenAPI = "6128362453:AAE8QuDtHSJb5ppNavmMhDgDusYO3ExbxYM";
    

    public static void Main()
    {
        
        // 6041584270:AAFwrVr3QCauOUAr6oCRSPdXaFfK_OX-xPQ - витя
        // 1001660063:AAH9cT_zOycKA7TqzY2IiywIdp-9_rKySuY
        // 6261177727:AAF8js_sluqbRtMCYLv4DFLQBknS00PsokE
        // 6128362453:AAE8QuDtHSJb5ppNavmMhDgDusYO3ExbxYM - витя 2


        var botClient = new TelegramBotClient(tokenAPI);

        botClient.StartReceiving(Update, Error);

        Console.ReadLine();
    }

    static ReplyKeyboardMarkup replyKeyboardMarkupNewEvent = new(new[]
        {
            new KeyboardButton("Отослать всем"), new KeyboardButton("Отмена"),
            new KeyboardButton("Отослать Некоторым(?)")
        })
        { ResizeKeyboard = true };

    static ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {
            new KeyboardButton("Создать мероприятие"),
            new KeyboardButton("Инфо")
        })
        { ResizeKeyboard = true };

    static ReplyKeyboardMarkup replyKeyboardMarkupComeBack = new(new[]
        {
            new KeyboardButton("Вернуться"),
            new KeyboardButton("Инфо")
        })
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

        if (message != null && message.Type == MessageType.Text)
        {
            // логи сообщений боту
            var logLine =
                $"Получено сообщение от |\t {DefineWhatTheName(message.Chat.Id)} |\t текст: \"{message.Text}\" " +
                $"|\t время: {message.Date} |{message.Chat.Username}";

            // вывод на консоль
            await Console.Out.WriteLineAsync(logLine);

            // путь до логов
            const string fileName = @"C:\Users\vita2\source\repos\botTG\botTG\logs.txt";

            // запись в файл
            await using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                await writer.WriteLineAsync(logLine);
            }

            switch (message.Text)
            {
                case @"/start":
                    await botClient.SendTextMessageAsync(message.Chat.Id,
                        "Приветсвтую комаровец! \n Рад что вижу тебя здесь! " +
                        "\n Открой меню и выбери то что хочешь сделать", replyMarkup: replyKeyboardMarkup);
                    break;
                
                case @"/help":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Функция помощи еще разрабаывается... ",
                        replyMarkup: replyKeyboardMarkup);
                    break;
                
                case @"/create":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Переходим к процессу создания тусовки... \n \n \n" +
                                                                          "Нажми на кнопку Создать мероприятие ",
                        replyMarkup: replyKeyboardMarkup);
                    break;
                
                case @"/thanks":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "угу", replyMarkup: replyKeyboardMarkup);
                    break;
                
                case "Создать мероприятие":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите название мероприятия:", replyMarkup: new ForceReplyMarkup(), cancellationToken: token);
                    break;
                default:
                    if (message.ReplyToMessage != null
                        && message.ReplyToMessage.Type == MessageType.Text
                        && message.ReplyToMessage.Text == "Введите название мероприятия:")
                    {
                        string eventName = message.Text ?? throw new InvalidOperationException();

                        var nameAndText = $"Мероприятие {eventName} успешно создано!";
                        // Здесь можно сохранить название мероприятия в базу данных или выполнить другие действия с ним.
                        var rd = new Random();
                        var number = rd.NextInt64(0, 8);
                        const string locatioin = @"C:\photos_bot\";
                        var path = $"{locatioin}{number}.png";
                        await SendPhoto(tokenAPI, message.Chat.Id, path, nameAndText);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, 
                            "чето блят занесло куда то занесло конечно, я не знаю как ты сюда попал но значит ты тут не правльино оказался", 
                            cancellationToken: token);
                    }
                    break;
            }


            
                
            
        }
    }


    public static async Task SendPhoto(string botToken, long chatId, string photoPath, string caption)
    {
        var botClient = new TelegramBotClient(botToken);

        using (var stream = new FileStream(photoPath, FileMode.Open))
        {
            var file = new InputOnlineFile(stream);

            caption = "Ахуенное мероприятие братишка. лучше просто некуда \n" + caption;
            await botClient.SendPhotoAsync(chatId, file, caption: caption);
        }
    }

    public class Event
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public InputOnlineFile Photo { get; set; }

        public Event(string description, string title, InputOnlineFile photo)
        {
            Description = description;
            Title = title;
            Photo = photo;
        }
    }

    public class EventManager
    {
        public List<Event?> events = new List<Event?>();

        public void AddEvent(Event ev)
        {
            events.Add(ev);
        }

        public Event? GetLastEvent()
        {
            return events.Count > 0 ? events[^1] : null;
        }
    }


    private static InlineKeyboardMarkup GetInlineKeyboardMarkup()
    {
        // Создаем inline-кнопки
        var createEventButton = InlineKeyboardButton.WithCallbackData("Создать мероприятие", "create_event");
        var joinEventButton = InlineKeyboardButton.WithCallbackData("Присоединиться к мероприятию", "join_event");

        // Создаем массив кнопок и добавляем их в inline-клавиатуру
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { createEventButton },
            new[] { joinEventButton }
        });

        return inlineKeyboard;
    }

    private static string DefineWhatTheName(long chatId)
    {
        var dictNames = new Dictionary<long, string>()
        {
            { 541629575, "АНЯ" },
            { 1421928229, "КЛЕВАНОВИЧ" },
            { 1773298820, "РОБЕРТ" },
            { 351666992, "ВИТЯ" },
            { 788497109, "ВАЛЕРА" },
            { 818843347, "СОНЯ" },
            { 1058420542, "ДЮКСЕР БМХ" },
            { 773435858, "РУСЛАКАНТУС" },
            { 835245013, "ВАНЕЧЕК" },
            { 511563830, "САША ДЕВОЧКА" },
            { 901364239, "МАША СУЕТА" },
            { 981528344, "МАРГО" }
        };
        foreach (var dict in dictNames.Where(dict => dict.Key == chatId))
        {
            return dict.Value;
        }

        return $"Неизвестный{chatId}";
    }
}