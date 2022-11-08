using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;


namespace app8
{
    class Program
    {

        // при перезапуске бота это все стриается и игра идет с начала
        // еще не загружал картинки (см EnterRoom)


        static int roomnumber = 0;
        static bool havefire = false;
        static bool havekey = false;
        static bool solvedquest = false;



        // вызов бота
        static void Main(string[] args)


        {
            

                //  указываем своё место
                Directory.SetCurrentDirectory(AppContext.BaseDirectory);

                //  работаем по защищённому протоколу, иначе будет падать
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


                var botClient = new TelegramBotClient("5469591033:AAFNTWouLXFX4Ywd72k2YNatwCEJVmnCvDo");
                botClient.StartReceiving(Update, Error);
                Console.ReadLine();
            }



        
                async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            try { 


                var message = update.Message;


                //Варианты блока 1
                if (message.Text.Contains("/start")) StartbotChoosegame();
                else if (message.Text.Contains("Квест")) Сontiniue();
                else if (message.Text.Contains("Игра")) Igra();


                //комнаты блока 1

                async void StartbotChoosegame()
                {

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Квест", "Игра 2" }, new KeyboardButton[] { "Игра 3", "Игра 4" }, })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Выберите игру",
                        replyMarkup: replyKeyboardMarkup, cancellationToken: token);

                }
                async void Igra()
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Это " + message.Text, cancellationToken: token);
                    return;
                }

                //Варианты блока 2

                if (message.Text.Contains("Пойти вглубь пещеры")) Perekrestok();
                else if (message.Text.Contains("Открыть дверь")) finish();
                else if (message.Text.Contains("Пойти налево") && (roomnumber == 2))
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Пошел налево", cancellationToken: token);
                    Fireroom();
                    return;
                }
                else if (message.Text.Contains("Пойти направо") && (roomnumber == 2))
                {
                    if (havefire)
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты зажигаешь факел и идешь вглубь");
                        Monsterroom();
                        return;
                    }

                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты пытаешься пойти, но в темноте натыкаешься на камни и решаешь пойти обратно");

                        Perekrestok();
                        return;

                    }

                } //лучше делать через if с комнатами но тогда сохр ломается
                else if (message.Text.Contains("Вернуться ко входу") && roomnumber == 2)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты вернулся ко входу");
                    EnterRoom();
                    return;
                }
                else if (message.Text.Contains("Вернуться на перекресток") && roomnumber == 3)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты возвращешься на перекресток");
                    Perekrestok();
                    return;
                }
                else if (message.Text.Contains("Обыскать комнату") && roomnumber == 3)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты находишь факел");

                    havefire = true;
                    Fireroom();
                    return;
                }
                else if (message.Text.Contains("Вернуться на перекресток") && roomnumber == 4)
                {


                    Perekrestok();
                    return;
                }
                else if (message.Text.Contains("Обыскать комнату") && roomnumber == 4)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты идешь вглубь темного зала");
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "На тебя нападает огромный монстр и убивает");

                    Death();
                    return;
                }
                else if (message.Text.Contains("Полезть в пролом") && roomnumber == 4)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты лезешь в пролом");

                    Questroom();
                    return;
                }
                else if (message.Text.Contains("Вернуться в пролом") && roomnumber == 5)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты лезешь обратно в пролом");

                    Monsterroom();
                    return;
                }
                else if (message.Text.Contains("Подойти к надписи") && roomnumber == 5)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты подходишь к загадке на стене");

                    Zagadka();
                    return;
                }
                else if (message.Text.Contains("1") && roomnumber == 7)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Неверно");

                    Zagadka();
                    return;
                }
                else if (message.Text.Contains("2") && roomnumber == 7)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Неверно");

                    Zagadka();
                    return;
                }
                else if (message.Text.Contains("3") && roomnumber == 7)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Дверь открывается и ты проходишь внутрь");
                    solvedquest = true;
                    KeyRoom();
                    return;
                }
                else if (message.Text.Contains("Вернуться назад") && roomnumber == 6)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты идешь в комнату с загадкой");

                    Questroom();
                    return;
                }
                else if (message.Text.Contains("Обыскать комнату") && roomnumber == 6)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты находишь ключ");
                    havekey = true;
                    KeyRoom();
                    return;
                }
                else if (message.Text.Contains("Главное меню"))
                {


                    StartbotChoosegame();
                    return;
                }
                else if (message.Text.Contains("Начать")) //заново или игру
                {

                    havekey = false;
                    havefire = false;
                    solvedquest = false;
                    EnterRoom();
                    return;
                }
                else if (message.Text.Contains("Пройти через тайный проход") && roomnumber == 5)
                {


                    KeyRoom();
                    return;
                }
                else if (message.Text.Contains("Продолжить") && (roomnumber == 1)) EnterRoom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 2)) Perekrestok();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 3)) Fireroom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 4)) Monsterroom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 5)) Questroom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 6)) KeyRoom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 7)) Zagadka();
                else if (message.Text.Contains("Контрольная точка")) Perekrestok();

                //комнаты блока 2
                async void EnterRoom()
                {


                    roomnumber = 1;


                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Открыть дверь", "Пойти вглубь пещеры" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId: message.Chat.Id,
                       photo: "https://i.gifer.com/origin/57/57a12e5e6588203cb55bd0ddbf052c35_w200.gif",
                        caption: "Ты у входа в пещеру. Сзади тебя дверь наружу, но она заперта. Также можно пойти исследовать подземелье",
                        replyMarkup: replyKeyboardMarkup, cancellationToken: token);
                }

                async void Perekrestok()
                {
                    roomnumber = 2;
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Пойти налево", "Пойти направо", "Вернуться ко входу" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Ты на перекрестке.Можно пойти налево, направо и назад.Налево все видно, направо очень темно.",
                        replyMarkup: replyKeyboardMarkup);
                }

                async void finish()
                {

                    if (havekey)
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты открываешь дверь ключом и выходишь", cancellationToken: token);



                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Главное меню", "Начать заново" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты выиграл",
                            replyMarkup: replyKeyboardMarkup);
                        return;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Дверь заперта на ключ", cancellationToken: token);


                        return;
                    }

                }

                async void Fireroom()
                {
                    if (!havefire)
                    {
                        roomnumber = 3;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться на перекресток", "Обыскать комнату" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в небольшой комнате заваленной всяким хламом",
                            replyMarkup: replyKeyboardMarkup);
                    }
                    else
                    {
                        roomnumber = 3;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться на перекресток" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в небольшой комнате заваленной всяким хламом",
                            replyMarkup: replyKeyboardMarkup);
                    }
                }

                async void Monsterroom()
                {
                    roomnumber = 4;
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться на перекресток", "Обыскать комнату", "Полезть в пролом" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Ты в огромном темном зале. Вдалеке виден пролом в стене",
                        replyMarkup: replyKeyboardMarkup);
                }

                async void Questroom()
                {
                    if (!solvedquest)
                    {
                        roomnumber = 5;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться в пролом", "Подойти к надписи" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в в каком то святилище. Дальше пути нет. На стене что то написано",
                            replyMarkup: replyKeyboardMarkup);
                    }
                    else
                    {
                        roomnumber = 5;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться в пролом", "Пройти через тайный проход" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в в каком то святилище. В стене под надписью открыт тайный проход",
                            replyMarkup: replyKeyboardMarkup);
                    }
                }

                async void KeyRoom()
                {
                    if (!havekey)
                    {
                        roomnumber = 6;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться назад", "Обыскать комнату" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в каменной камере, похожей на комнату надзирателя. Если где-то есть ключ, то только тут",
                            replyMarkup: replyKeyboardMarkup);
                    }
                    else
                    {
                        roomnumber = 6;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться назад" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в каменной камере, похожей на комнату надзирателя. ",
                            replyMarkup: replyKeyboardMarkup);
                    }
                }

                async void Death()
                {
                    roomnumber = 8;
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Главное меню", "Начать заново", "Контрольная точка" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Ты проиграл. Вернуться к последнему сохранению или начать заново?",
                        replyMarkup: replyKeyboardMarkup);
                }

                async void Zagadka()
                {
                    roomnumber = 7;
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "1", "2", "3" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "На стене написано 1+2=",
                        replyMarkup: replyKeyboardMarkup);
                }
                async void Сontiniue()
                {
                    if (roomnumber == 0)
                    {
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                         {

                        new KeyboardButton[] {"Начать игру"},
                        new KeyboardButton[] {"Главное меню"},
                    })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                "Начать игру?",
                                replyMarkup: replyKeyboardMarkup);
                    }

                    else
                    {
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                       {
                            new KeyboardButton[] {"Продолжить"},
                            new KeyboardButton[] {"Начать заново"},
                            new KeyboardButton[] {"Главное меню"},
                            })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                "Продолжить или начать заново?",
                                replyMarkup: replyKeyboardMarkup);
                    }

                }



            }

            catch
            {
                await Task.Delay(2000);
                var message = update.Message;


                //Варианты блока 1
                if (message.Text.Contains("/start")) StartbotChoosegame();
                else if (message.Text.Contains("Квест")) Сontiniue();
                else if (message.Text.Contains("Игра")) Igra();


                //комнаты блока 1

                async void StartbotChoosegame()
                {

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Квест", "Игра 2" }, new KeyboardButton[] { "Игра 3", "Игра 4" }, })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Выберите игру",
                        replyMarkup: replyKeyboardMarkup, cancellationToken: token);

                }
                async void Igra()
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Это " + message.Text, cancellationToken: token);
                    return;
                }

                //Варианты блока 2

                if (message.Text.Contains("Пойти вглубь пещеры")) Perekrestok();
                else if (message.Text.Contains("Открыть дверь")) finish();
                else if (message.Text.Contains("Пойти налево") && (roomnumber == 2))
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Пошел налево", cancellationToken: token);
                    Fireroom();
                    return;
                }
                else if (message.Text.Contains("Пойти направо") && (roomnumber == 2))
                {
                    if (havefire)
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты зажигаешь факел и идешь вглубь");
                        Monsterroom();
                        return;
                    }

                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты пытаешься пойти, но в темноте натыкаешься на камни и решаешь пойти обратно");

                        Perekrestok();
                        return;

                    }

                } //лучше делать через if с комнатами но тогда сохр ломается
                else if (message.Text.Contains("Вернуться ко входу") && roomnumber == 2)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты вернулся ко входу");
                    EnterRoom();
                    return;
                }
                else if (message.Text.Contains("Вернуться на перекресток") && roomnumber == 3)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты возвращешься на перекресток");
                    Perekrestok();
                    return;
                }
                else if (message.Text.Contains("Обыскать комнату") && roomnumber == 3)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты находишь факел");

                    havefire = true;
                    Fireroom();
                    return;
                }
                else if (message.Text.Contains("Вернуться на перекресток") && roomnumber == 4)
                {


                    Perekrestok();
                    return;
                }
                else if (message.Text.Contains("Обыскать комнату") && roomnumber == 4)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты идешь вглубь темного зала");
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "На тебя нападает огромный монстр и убивает");

                    Death();
                    return;
                }
                else if (message.Text.Contains("Полезть в пролом") && roomnumber == 4)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты лезешь в пролом");

                    Questroom();
                    return;
                }
                else if (message.Text.Contains("Вернуться в пролом") && roomnumber == 5)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты лезешь обратно в пролом");

                    Monsterroom();
                    return;
                }
                else if (message.Text.Contains("Подойти к надписи") && roomnumber == 5)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты подходишь к загадке на стене");

                    Zagadka();
                    return;
                }
                else if (message.Text.Contains("1") && roomnumber == 7)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Неверно");

                    Zagadka();
                    return;
                }
                else if (message.Text.Contains("2") && roomnumber == 7)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Неверно");

                    Zagadka();
                    return;
                }
                else if (message.Text.Contains("3") && roomnumber == 7)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Дверь открывается и ты проходишь внутрь");
                    solvedquest = true;
                    KeyRoom();
                    return;
                }
                else if (message.Text.Contains("Вернуться назад") && roomnumber == 6)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты идешь в комнату с загадкой");

                    Questroom();
                    return;
                }
                else if (message.Text.Contains("Обыскать комнату") && roomnumber == 6)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты находишь ключ");
                    havekey = true;
                    KeyRoom();
                    return;
                }
                else if (message.Text.Contains("Главное меню"))
                {


                    StartbotChoosegame();
                    return;
                }
                else if (message.Text.Contains("Начать")) //заново или игру
                {

                    havekey = false;
                    havefire = false;
                    solvedquest = false;
                    EnterRoom();
                    return;
                }
                else if (message.Text.Contains("Пройти через тайный проход") && roomnumber == 5)
                {


                    KeyRoom();
                    return;
                }
                else if (message.Text.Contains("Продолжить") && (roomnumber == 1)) EnterRoom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 2)) Perekrestok();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 3)) Fireroom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 4)) Monsterroom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 5)) Questroom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 6)) KeyRoom();
                else if (message.Text.Contains("Продолжить") && (roomnumber == 7)) Zagadka();
                else if (message.Text.Contains("Контрольная точка")) Perekrestok();

                //комнаты блока 2
                async void EnterRoom()
                {


                    roomnumber = 1;


                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Открыть дверь", "Пойти вглубь пещеры" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId: message.Chat.Id,
                       photo: "https://i.gifer.com/origin/57/57a12e5e6588203cb55bd0ddbf052c35_w200.gif",
                        caption: "Ты у входа в пещеру. Сзади тебя дверь наружу, но она заперта. Также можно пойти исследовать подземелье",
                        replyMarkup: replyKeyboardMarkup, cancellationToken: token);
                }

                async void Perekrestok()
                {
                    roomnumber = 2;
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Пойти налево", "Пойти направо", "Вернуться ко входу" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Ты на перекрестке.Можно пойти налево, направо и назад.Налево все видно, направо очень темно.",
                        replyMarkup: replyKeyboardMarkup);
                }

                async void finish()
                {

                    if (havekey)
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ты открываешь дверь ключом и выходишь", cancellationToken: token);



                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Главное меню", "Начать заново" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты выиграл",
                            replyMarkup: replyKeyboardMarkup);
                        return;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Дверь заперта на ключ", cancellationToken: token);


                        return;
                    }

                }

                async void Fireroom()
                {
                    if (!havefire)
                    {
                        roomnumber = 3;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться на перекресток", "Обыскать комнату" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в небольшой комнате заваленной всяким хламом",
                            replyMarkup: replyKeyboardMarkup);
                    }
                    else
                    {
                        roomnumber = 3;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться на перекресток" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в небольшой комнате заваленной всяким хламом",
                            replyMarkup: replyKeyboardMarkup);
                    }
                }

                async void Monsterroom()
                {
                    roomnumber = 4;
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться на перекресток", "Обыскать комнату", "Полезть в пролом" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Ты в огромном темном зале. Вдалеке виден пролом в стене",
                        replyMarkup: replyKeyboardMarkup);
                }

                async void Questroom()
                {
                    if (!solvedquest)
                    {
                        roomnumber = 5;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться в пролом", "Подойти к надписи" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в в каком то святилище. Дальше пути нет. На стене что то написано",
                            replyMarkup: replyKeyboardMarkup);
                    }
                    else
                    {
                        roomnumber = 5;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться в пролом", "Пройти через тайный проход" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в в каком то святилище. В стене под надписью открыт тайный проход",
                            replyMarkup: replyKeyboardMarkup);
                    }
                }

                async void KeyRoom()
                {
                    if (!havekey)
                    {
                        roomnumber = 6;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться назад", "Обыскать комнату" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в каменной камере, похожей на комнату надзирателя. Если где-то есть ключ, то только тут",
                            replyMarkup: replyKeyboardMarkup);
                    }
                    else
                    {
                        roomnumber = 6;
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Вернуться назад" }, })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            "Ты в каменной камере, похожей на комнату надзирателя. ",
                            replyMarkup: replyKeyboardMarkup);
                    }
                }

                async void Death()
                {
                    roomnumber = 8;
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Главное меню", "Начать заново", "Контрольная точка" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "Ты проиграл. Вернуться к последнему сохранению или начать заново?",
                        replyMarkup: replyKeyboardMarkup);
                }

                async void Zagadka()
                {
                    roomnumber = 7;
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "1", "2", "3" }, })
                    {
                        ResizeKeyboard = true
                    };

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        "На стене написано 1+2=",
                        replyMarkup: replyKeyboardMarkup);
                }
                async void Сontiniue()
                {
                    if (roomnumber == 0)
                    {
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                         {

                        new KeyboardButton[] {"Начать игру"},
                        new KeyboardButton[] {"Главное меню"},
                    })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                "Начать игру?",
                                replyMarkup: replyKeyboardMarkup);
                    }

                    else
                    {
                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                       {
                            new KeyboardButton[] {"Продолжить"},
                            new KeyboardButton[] {"Начать заново"},
                            new KeyboardButton[] {"Главное меню"},
                            })
                        {
                            ResizeKeyboard = true
                        };

                        Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                "Продолжить или начать заново?",
                                replyMarkup: replyKeyboardMarkup);
                    }

                }


                return;
            }
        }

                // обработка ошибок
                static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
                {
                    throw new NotImplementedException();
                }

            }


        }
    













