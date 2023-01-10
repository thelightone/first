using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Telegram.Bot.Types.Enums;

namespace app8
{
    class Program
    {
        static ParseMode pm = new ParseMode();
        static void Main(string[] args)

        {
            while (true)
            {
                try

                {
                    // СОЗДАНИЕ БОТА

                    //  указываем своё место и  работаем по защищённому протоколу, иначе будет падать
                    Directory.SetCurrentDirectory(AppContext.BaseDirectory);
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    // вызов бота
                    var botClient = new TelegramBotClient("5469591033:AAFNTWouLXFX4Ywd72k2YNatwCEJVmnCvDo");
                    botClient.StartReceiving(Update, Error);
                    Console.WriteLine("1");
                    Console.ReadLine();


                    // основное тело бота - обновления
                    async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
                    {


                        // зацикливаем его работу
                        while (true)
                        {

                            try
                            {

                                // делаем получение сообщения
                                var message = update.Message;



                                // СОХРАНЕНИЕ И ЗАГРУЗКА 
                                string ChatId = Convert.ToString(message.Chat.Id);


                                Console.WriteLine("2");

                                // ПОДКЛЮЧЕНИЕ БАЗЫ ДАННЫХ & открытие базы
                                IDbConnection dbcon4 = new SqliteConnection("Data Source = Savings.db");
                                dbcon4.Open();
                                Console.WriteLine("3");





                                //создание команды
                                IDbCommand firstsave = dbcon4.CreateCommand();
                                firstsave.CommandText = "SELECT count(*) FROM Savings WHERE ChatId='" + ChatId + "'";
                                Console.WriteLine("4");



                                int count = Convert.ToInt32(firstsave.ExecuteScalar());
                                if (count == 0)
                                {
                                    try
                                    {
                                        firstsave.CommandText = "INSERT INTO Savings (ChatId, Stagequest, HaveFire, HaveKey, SolvedQuest, havestick1, havestick2, havestick0, CurGame, invitation, paid, oskolok, stranger, sdelkaotkaz, monsterdead, pay_link, payday, url, " +
                                        "havestones, haveshovel, havemeet, fisher, mistake, haveplant, ask, findfigure, checktable, checkbed, checkkomod, stayhome, checkkitch,checkall)" +
                                        "VALUES ('" + ChatId + "','0', 'false', 'false', 'false', 'false', 'false', 'false', 0, '0', '0','false','false','false','false','false' ,'false','false'" +
                                        ",'false','false' ,'false','false','false','false' ,'false','false','false','false','false','false' ,'false','false')";

                                        firstsave.ExecuteNonQuery();
                                        Console.WriteLine(5);

                                        firstsave.Dispose();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;
                                    }

                                }

                                IDbCommand loading = dbcon4.CreateCommand();
                                loading.CommandText =
                                    "SELECT * FROM Savings WHERE ChatId ='" + ChatId + "' ";

                                //что то типо выполни команду
                                IDataReader reader2 = loading.ExecuteReader();
                                Console.WriteLine("6");


                                // прочитывание результата формата sql и манипуляции с ним в (задача переменных)
                                reader2.Read();

                                //Не прыгай в бездну
                                string CurGame = reader2.GetString(8);
                                bool havefire = Convert.ToBoolean(reader2.GetString(2));
                                bool havekey = Convert.ToBoolean(reader2.GetString(3));
                                bool solvedquest = Convert.ToBoolean(reader2.GetString(4));
                                bool havestick1 = Convert.ToBoolean(reader2.GetString(5));
                                bool havestick2 = Convert.ToBoolean(reader2.GetString(6));
                                bool havestick0 = Convert.ToBoolean(reader2.GetString(7));
                                string stagequest1 = reader2.GetString(1);
                                string invitation = reader2.GetString(9);
                                string paid = reader2.GetString(10);
                                bool oskolok = Convert.ToBoolean(reader2.GetString(11));
                                bool stranger = Convert.ToBoolean(reader2.GetString(12));
                                bool sdelkaotkaz = Convert.ToBoolean(reader2.GetString(13));
                                bool monsterdead = Convert.ToBoolean(reader2.GetString(14));
                                //Оплата
                                string pay_link = reader2.GetString(15);
                                string payday = reader2.GetString(16);
                                string url = reader2.GetString(17);
                                //Легенда о страннике 1
                                bool havestones = Convert.ToBoolean(reader2.GetString(18));
                                bool haveshovel = Convert.ToBoolean(reader2.GetString(19));
                                bool havemeet = Convert.ToBoolean(reader2.GetString(20));
                                bool fisher = Convert.ToBoolean(reader2.GetString(21));
                                bool mistake = Convert.ToBoolean(reader2.GetString(22));
                                bool haveplant = Convert.ToBoolean(reader2.GetString(23));
                                bool ask = Convert.ToBoolean(reader2.GetString(24));
                                bool findfigure = Convert.ToBoolean(reader2.GetString(25));
                                //Шарманщик
                                bool checktable = Convert.ToBoolean(reader2.GetString(26));
                                bool checkbed = Convert.ToBoolean(reader2.GetString(27));
                                bool checkkomod = Convert.ToBoolean(reader2.GetString(28));
                                bool stayhome = Convert.ToBoolean(reader2.GetString(29));
                                bool checkkitch = Convert.ToBoolean(reader2.GetString(30));
                                bool checkall = Convert.ToBoolean(reader2.GetString(31));

                                Console.WriteLine("13");
                                firstsave.Dispose();
                                reader2.Dispose();
                                loading.Dispose();

                                dbcon4.Close();

                                //АВТООПЛАТА
                                string curDate = (Convert.ToString(DateTime.Today)).Substring(0, 10);
                                curDate = curDate.Trim();

                                Console.WriteLine(checkall);

                                SaveProgress();




                                //ТЕХНИЧЕСКАЯ БД

                                IDbConnection dbcon7 = new SqliteConnection("Data Source=technic.db");
                                dbcon7.Open();

                                IDbCommand technicadd1 = dbcon7.CreateCommand();
                                technicadd1.CommandText = "SELECT count(*) FROM technic WHERE ChatId='" + ChatId + "'";
                                int count2 = Convert.ToInt32(technicadd1.ExecuteScalar());
                                Console.WriteLine("14");
                                if (count2 == 0)
                                {
                                    try
                                    {
                                        Console.WriteLine("15");
                                        technicadd1.CommandText = "INSERT INTO technic (ChatId, FindingsNum, Finding, subnumberQuest1, TextQuest1, photolinkQuest1, HalfBut1TextQuest1, HalfBut2TextQuest1, HalfBut3TextQuest1, HalfBut4TextQuest1, But1TextQuest1, But2TextQuest1, But3TextQuest1, But4TextQuest1, condition, AddText1, AddText2, AddText3, AddText4, NumBut1, NumBut2, NumBut3, NumBut4, ChosedBut, PlaceChosBut) VALUES ( '" + ChatId + "','0','nofindings', '1', '1', '1', '0', '0', '0', '0', '1', '1', '1', '1', 'false', '1', '1', '1', '1', '1', '1', '1', '1', '0', '0')";
                                        Console.WriteLine("151");
                                        technicadd1.ExecuteNonQuery();
                                        Console.WriteLine("160");
                                        technicadd1.Dispose();
                                        Console.WriteLine("16");
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;
                                    }
                                }
                                IDbCommand technikload = dbcon7.CreateCommand();

                                //команда на языке sql и некий ее результат

                                technikload.CommandText =
                                    "SELECT * FROM technic WHERE ChatId ='" + ChatId + "' ";


                                //что то типо выполни команду
                                IDataReader reader3 = technikload.ExecuteReader();

                                // прочитывание результата формата sql и манипуляции с ним в (задача переменных)
                                reader3.Read();

                                string FindingsNum = reader3.GetString(1);
                                string Finding = reader3.GetString(2);
                                int subnumberQuest1 = Convert.ToInt32(reader3.GetString(3));
                                string TextQuest1 = reader3.GetString(4);
                                string photolinkQuest1 = reader3.GetString(5);
                                string HalfBut1TextQuest1 = reader3.GetString(6).Trim();
                                string HalfBut2TextQuest1 = reader3.GetString(7).Trim();
                                string HalfBut3TextQuest1 = reader3.GetString(8).Trim();
                                string HalfBut4TextQuest1 = reader3.GetString(9).Trim();
                                string But1TextQuest1 = reader3.GetString(10).Trim();
                                string But2TextQuest1 = reader3.GetString(11).Trim();
                                string But3TextQuest1 = reader3.GetString(12).Trim();
                                string But4TextQuest1 = reader3.GetString(13).Trim();
                                string condition = reader3.GetString(14);

                                string AddText1 = reader3.GetString(15);
                                string AddText2 = reader3.GetString(16);
                                string AddText3 = reader3.GetString(17);
                                string AddText4 = reader3.GetString(18);
                                int NumBut1 = Convert.ToInt32(reader3.GetString(19));
                                int NumBut2 = Convert.ToInt32(reader3.GetString(20));
                                int NumBut3 = Convert.ToInt32(reader3.GetString(21));
                                int NumBut4 = Convert.ToInt32(reader3.GetString(22));
                                string ChosedBut = reader3.GetString(23);
                                string PlaceChosBut = reader3.GetString(24);


                                technikload.Dispose();
                                reader3.Dispose();
                                technicadd1.Dispose();
                                dbcon7.Close();

                                SaveTechnic();
                                Console.WriteLine("14,5");









                                //ГЛАВНОЕ МЕНЮ
                                if (message.Text.Contains("/start") || message.Text.Contains("🔘 Вернуться в Главное меню"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingTechnik();
                                        LoadingProgress();
                                        Сontiniue();

                                        SaveProgress();

                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }

                                else if (message.Text.Contains("💪 Подписался"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingTechnik();
                                        LoadingProgress();
                                        invitation = "true";
                                        StartbotChoosegame();

                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("👿 Не хочу поддерживать"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingTechnik();
                                        LoadingProgress();

                                        StartbotChoosegame();

                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }


                                }
                                else if (message.Text.Contains("🔘 Начать новую игру"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        if (stagequest1 != "0")
                                        {
                                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                                {

                                 new KeyboardButton[] { "🔘 Да, начать новую игру" },
                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню" },

                                })
                                            {
                                                ResizeKeyboard = true
                                            };

                                            Message sentMessage = await botClient.SendTextMessageAsync(
                                                    chatId: message.Chat.Id,
                                                    "Вы уверены? Текущий прогресс будет потерян.",
                                                    replyMarkup: replyKeyboardMarkup);
                                        }

                                        else
                                        {
                                            LoadingTechnik();

                                            LoadingProgress();

                                            havefire = false;
                                            havekey = false;
                                            solvedquest = false;
                                            havestick1 = false;
                                            havestick2 = false;
                                            havestick0 = false;
                                            oskolok = false;
                                            stranger = false;
                                            sdelkaotkaz = false;
                                            monsterdead = false;

                                            havestones = false;
                                            haveshovel = false;
                                            havemeet = false;
                                            fisher = false;
                                            mistake = false;
                                            haveplant = false;
                                            ask = false;
                                            findfigure = false;

                                            checktable = false;
                                            checkbed = false;
                                            checkkomod = false;
                                            stayhome = false;
                                            checkkitch = false;
                                            checkall = false;

                                            FindingsNum = "0";
                                            Finding = "nofindings";
                                            subnumberQuest1 = 1;
                                            TextQuest1 = "1";
                                            photolinkQuest1 = "1";
                                            HalfBut1TextQuest1 = "0";
                                            HalfBut2TextQuest1 = "0";
                                            HalfBut3TextQuest1 = "0";
                                            HalfBut4TextQuest1 = "0";
                                            But1TextQuest1 = "1";
                                            But2TextQuest1 = "1";
                                            But3TextQuest1 = "1";
                                            But4TextQuest1 = "1";
                                            condition = "false";

                                            AddText1 = "1";
                                            AddText2 = "1";
                                            AddText3 = "1";
                                            AddText4 = "1";
                                            NumBut1 = 1;
                                            NumBut2 = 1;
                                            NumBut3 = 1;
                                            NumBut4 = 1;
                                            ChosedBut = "0";
                                            PlaceChosBut = "0";
                                            SaveProgress();
                                            SaveTechnic();

                                            if (invitation != "true")
                                            {
                                                InvitCheck();
                                            }
                                            else StartbotChoosegame();

                                            SaveProgress();
                                            SaveTechnic();
                                        }
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔘 Да, начать новую игру"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingTechnik();

                                        LoadingProgress();



                                        stagequest1 = "0";
                                        havefire = false;
                                        havekey = false;
                                        solvedquest = false;
                                        havestick1 = false;
                                        havestick2 = false;
                                        havestick0 = false;
                                        oskolok = false;
                                        stranger = false;
                                        sdelkaotkaz = false;
                                        monsterdead = false;

                                        havestones = false;
                                        haveshovel = false;
                                        havemeet = false;
                                        fisher = false;
                                        mistake = false;
                                        haveplant = false;
                                        ask = false;
                                        findfigure = false;

                                        checktable = false;
                                        checkbed = false;
                                        checkkomod = false;
                                        stayhome = false;
                                        checkkitch = false;
                                        checkall = false;

                                        FindingsNum = "0";
                                        Finding = "nofindings";
                                        subnumberQuest1 = 1;
                                        TextQuest1 = "1";
                                        photolinkQuest1 = "1";
                                        HalfBut1TextQuest1 = "0";
                                        HalfBut2TextQuest1 = "0";
                                        HalfBut3TextQuest1 = "0";
                                        HalfBut4TextQuest1 = "0";
                                        But1TextQuest1 = "1";
                                        But2TextQuest1 = "1";
                                        But3TextQuest1 = "1";
                                        But4TextQuest1 = "1";
                                        condition = "false";

                                        AddText1 = "1";
                                        AddText2 = "1";
                                        AddText3 = "1";
                                        AddText4 = "1";
                                        NumBut1 = 1;
                                        NumBut2 = 1;
                                        NumBut3 = 1;
                                        NumBut4 = 1;
                                        ChosedBut = "0";
                                        PlaceChosBut = "0";
                                        SaveProgress();
                                        SaveTechnic();

                                        if (invitation != "true")
                                        {
                                            InvitCheck();
                                        }
                                        else StartbotChoosegame();










                                        SaveProgress();
                                        SaveTechnic();
                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔘 Продолжить игру"))

                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingProgress();
                                        LoadingTechnik();

                                        Quest1();

                                        SaveProgress();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }

                                }
                                else if (message.Text.Contains("🔘 К выбору категорий"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        StartbotChoosegame();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("☎️ Контакты"))
                                {
                                    try
                                    {
                                        SubCheck();

                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                           {

                                 new KeyboardButton[] { "🔘 Об авторах"},
                                 new KeyboardButton[] { "🔘 Помощь"},
                                 new KeyboardButton[] { "🔘 Связаться с нами"},
                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню"},

                                })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                "По какому поводу вы обращаетесь?",
                                                replyMarkup: replyKeyboardMarkup);

                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔘 Об авторах"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                           {


                                 new KeyboardButton[] { "🔘 Помощь"},
                                 new KeyboardButton[] { "🔘 Связаться с нами"},
                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню"},

                                })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                "Наша команда:" + "\n" + "\n" +
                                                "🐵@Khachapuri666 - тех.лид, автор квестов" + "\n" +
                                                "🐰@SnezhkaBond - автор квестов" + "\n" +
                                                "🐯@tematibr - тех.консультант" + "\n" +
                                                "🐻@adhhda - тех. консультант" + "\n" +
                                                "🦁@echoscomplex - python-программист" + "\n" +
                                                "🐙@ex_future - автор музыки" + "\n" +
                                                "🤖DALL-E 2 - художник"
                                                ,
                                                replyMarkup: replyKeyboardMarkup);

                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔘 Помощь"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                           {

                                 new KeyboardButton[] { "🔘 Об авторах"},
                                 new KeyboardButton[] { "🔘 Связаться с нами"},
                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню"},

                                })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                "🔘 Если у вас что-то зависло/не работает:" + "\n" +
                                                "Нажмите 'Меню'(синяя кнопка, слева над клавиатурой) -> '/start' -> 'Продолжить'" + "\n" +
                                                "Если ошибка сохраняется, попробуйте удалить и заново войти в бота." + "\n" + "\n" +
                                                "🔘 В случае, если ошибка связана с оплатой - напишите нам на @Khachapuri666, и вышлите ваш ChatId: '" + ChatId + "'.",
                                                replyMarkup: replyKeyboardMarkup);

                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔘 Связаться с нами"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                           {

                                 new KeyboardButton[] { "🔘 Об авторах"},
                                 new KeyboardButton[] { "🔘 Помощь"},
                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню"},

                                })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                "С вопросами, пожеланиями и предложениями по сотрудничеству, вы можете обратиться сюда - @Khachapuri666",
                                                replyMarkup: replyKeyboardMarkup);

                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("Secret"))
                                {
                                    try
                                    {
                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                             {

                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню"},

                                })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                "Ваш Id: " + ChatId + "\n" + "Пожалуйста, отправьте его в тех.поддержку",
                                                replyMarkup: replyKeyboardMarkup);
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }


                                //КАТЕГОРИИ
                                else if (message.Text.Contains("🏰 RPG"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingTechnik();
                                        LoadingProgress();


                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                            {

                                 new KeyboardButton[] {"🏰 'Не прыгай в Пропасть'",},
                                 new KeyboardButton[] { "🗡 'Легенда о Страннике. Часть I'", }
                                 ,
                                 new KeyboardButton[] { "🔘 К выбору категорий" }
                    })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendPhotoAsync(
                                              chatId: message.Chat.Id,
                                              photo: "https://github.com/thelightone/questgame/raw/main/RPG.jpg",
                                              caption: "Доступные игры:" +
                                                "\n" + "\n" +
                                                "🏰 <b>'Не прыгай в Пропасть'</b>" + "\n" + "\n" +
                                                "Длительность: 20-40 мин." + "\n" + "\n" +
                                                "Описание: Найдите путь из загадочного подземелья, но опасайтесь древнего ужаса, прячущегося в глубине! Классический квест-лабиринт с загадками и поиском предметов." +
                                                "\n" + "\n" + "\n" +
                                                "🗡 <b>'Легенда о Страннике." + "\n" +"Часть I'</b>" + "\n" + "\n" +
                                                 "Длительность: 60-90 мин." + "\n" + "\n" +
                                                "Описание: Эпическая история о спасении мира в духе 'Властелина колец' и 'Ведьмака'.",
                                             pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);






                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("☠️ Хоррор"))
                                {
                                    try
                                    {
                                       

                                        SubCheck();
                                        LoadingTechnik();
                                        LoadingProgress();


                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                            {

                                 new KeyboardButton[] {"🎩 'Шарманщик'"},


                                 new KeyboardButton[] { "🔘 К выбору категорий" }
                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        Message sentMessage = await botClient.SendPhotoAsync(
                                                                                  chatId: message.Chat.Id,
                                                                                  photo: "https://github.com/thelightone/questgame/raw/main/horror.jpg",
                                                                                  caption: "Доступные игры:" + "\n" + "\n" +
                                                "🎩 <b>'Шарманщик'</b>" + "\n" + "\n" +
                                                "Длительность: 30-60 мин." + "\n" + "\n" +
                                                "Описание: Ужасающая история от лица маленького мальчика.",
                                                                                  pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);




                                        SaveProgress();
                                        SaveTechnic();

                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔍 Детектив"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingProgress();
                                        LoadingTechnik();

                                        Igra();

                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("💋 18+"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingProgress();
                                        LoadingTechnik();

                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                            {


                                 new KeyboardButton[] { "🔘 Да, я старше 18 лет" },
                                 new KeyboardButton[] {"🔘 Нет, мне нет 18 лет"},
                                  new KeyboardButton[] {"🔘 Вернуться в Главное меню" },

                                })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                "Вход в этот раздел доступен только лицам старше 18 лет. Вам есть 18 лет?",
                                                replyMarkup: replyKeyboardMarkup);

                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔘 Да, я старше 18 лет"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingTechnik();
                                        LoadingProgress();


                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                            {

                                 new KeyboardButton[] {"🌸 'Знакомство с Лили'"},


                                 new KeyboardButton[] { "🔘 К выбору категорий" }
                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        Message sentMessage = await botClient.SendPhotoAsync(
                                                                                  chatId: message.Chat.Id,
                                                                                  photo: "https://github.com/thelightone/questgame/raw/main/18+.jpg",
                                                                                  caption: "Доступные игры:" +
                                                "\n" + "\n" +
                                                "🌸 <b>'Знакомство с Лили'</b>" + "\n" + "\n" +
                                                "Длительность: 30-60 мин." + "\n" + "\n" +
                                                "Описание: Что делать, если к вам в руки попал телефон очаровательной незнакомки?",
                                                                                  pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);




                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔘 Нет, мне нет 18 лет"))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingProgress();
                                        LoadingTechnik();

                                        StartbotChoosegame();

                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }

                                // ИГРЫ
                                else if (message.Text.Contains("🏰 'Не прыгай в Пропасть'"))
                                {
                                    try
                                    {
                                        LoadingTechnik();
                                        LoadingProgress();

                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                            {

                                 new KeyboardButton[] {" ",},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        LoadingTechnik();
                                        LoadingProgress();
                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: "❗️<b>Внимание:</b> Не нажимайте на кнопки в одной комнате несколько раз - это может привести к поломкам." + "\n" + "\n" +
                                       "При возникновении проблем, перезагрузите игру через 'Меню' -> 'Главное меню' -> 'Продолжить', либо через удаление/установку бота." + "\n" + "Ваш прогресс сохранится.",
                                              pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);

                                        CurGame = "1";
                                        stagequest1 = "14";



                                        await Task.Delay(1500);

                                        sentMessage = await botClient.SendTextMessageAsync(
                                        chatId: message.Chat.Id,
                                       "🎧 Для более полного погружения, мы написали специальную композицию к этому квесту: ");

                                        sentMessage = await botClient.SendAudioAsync(chatId: message.Chat.Id, audio: "https://github.com/thelightone/questgame/raw/main/Don" + "'" + "t%20jump%20in%20the%20Abyss.mp3",

                                    performer: "[da.net]",
                                    title: "Don't jump in the Abyss"




                                            );
                                        await Task.Delay(1500);
                                        sentMessage = await botClient.SendTextMessageAsync(
                                                                          chatId: message.Chat.Id,
                                                                         "Загрузка игры... ");

                                        await Task.Delay(1500);
                                        Quest1();






                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🗡 'Легенда о Страннике. Часть I'"))
                                {
                                    try
                                    {
                                        LoadingTechnik();
                                        LoadingProgress();

                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                            {

                                 new KeyboardButton[] {" ",},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        LoadingTechnik();
                                        LoadingProgress();
                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: "❗️<b>Внимание:</b> Не нажимайте на кнопки в одной комнате несколько раз - это может привести к поломкам." + "\n" + "\n" +
                                       "При возникновении проблем, перезагрузите игру через 'Меню' -> 'Главное меню' -> 'Продолжить', либо через удаление/установку бота." + "\n" + "Ваш прогресс сохранится.",
                                              pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);

                                        CurGame = "2";
                                        stagequest1 = "99";



                                        await Task.Delay(1500);

                                        sentMessage = await botClient.SendTextMessageAsync(
                                        chatId: message.Chat.Id,
                                       "🎧 Для более полного погружения, мы написали специальную композицию к этому квесту: ");
                                        string textwanderer = "https://github.com/thelightone/questgame/raw/main/%5Bda.net%5D%20" + "-" + "%20The%20Legend%20of%20Wanderer%20Part%20I%20.mp3";
                                        Message sentMessage2 = await botClient.SendAudioAsync(chatId: message.Chat.Id, audio: "https://github.com/thelightone/questgame/raw/main/The%20Legend%20of%20Wanderer.%20Part%20I.mp3",

                                    performer: "[da.net]",
                                    title: "The Legend of Wanderer. Part I"




                                            );
                                        await Task.Delay(1500);
                                        sentMessage = await botClient.SendTextMessageAsync(
                                                                          chatId: message.Chat.Id,
                                                                         "Загрузка игры... ");

                                        await Task.Delay(1500);
                                        Quest1();






                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;






                                    }
                                }
                                else if (message.Text.Contains("🎩 'Шарманщик'"))
                                {
                                    try
                                    {
                                        LoadingTechnik();
                                        LoadingProgress();

                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                            {

                                 new KeyboardButton[] {" ",},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        LoadingTechnik();
                                        LoadingProgress();
                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: "❗️<b>Внимание:</b> Не нажимайте на кнопки в одной комнате несколько раз - это может привести к поломкам." + "\n" + "\n" +
                                       "При возникновении проблем, перезагрузите игру через 'Меню' -> 'Главное меню' -> 'Продолжить', либо через удаление/установку бота." + "\n" + "Ваш прогресс сохранится.",
                                               pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);

                                        CurGame = "3";
                                        stagequest1 = "701";

                                        await Task.Delay(1500);

                                        sentMessage = await botClient.SendTextMessageAsync(
                                        chatId: message.Chat.Id,
                                       "🎧 Для более полного погружения, мы написали специальную композицию к этому квесту: ");

                                        sentMessage = await botClient.SendAudioAsync(chatId: message.Chat.Id, audio: "https://github.com/thelightone/questgame/raw/main/Hurdy-Gurdy.mp3",

                                    performer: "[da.net]",
                                    title: "Hurdy-Gurdy"




                                            );

                                        await Task.Delay(1500);
                                        sentMessage = await botClient.SendTextMessageAsync(
                                                                          chatId: message.Chat.Id,
                                                                         "Загрузка игры... ");

                                        await Task.Delay(1500);
                                        Quest1();






                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🌸 'Знакомство с Лили'"))
                                {
                                    try
                                    {
                                        LoadingTechnik();
                                        LoadingProgress();

                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                            {

                                 new KeyboardButton[] {" ",},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        LoadingTechnik();
                                        LoadingProgress();
                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: "❗️<b>Внимание:</b> Не нажимайте на кнопки в одной комнате несколько раз - это может привести к поломкам." + "\n" + "\n" +
                                       "При возникновении проблем, перезагрузите игру через 'Меню' -> 'Главное меню' -> 'Продолжить', либо через удаление/установку бота." + "\n" + "Ваш прогресс сохранится.",
                                                pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);

                                        CurGame = "4";
                                        stagequest1 = "900";







                                        await Task.Delay(1500);
                                        sentMessage = await botClient.SendTextMessageAsync(
                                                                          chatId: message.Chat.Id,
                                                                         "Загрузка игры... ");

                                        await Task.Delay(1500);
                                        Quest1();






                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }

                                // ОПЛАТА
                                else if (message.Text.Contains("👑 Подписка"))
                                {
                                    try
                                    {
                                        SubCheck();

                                        if (paid != "1")
                                        {
                                            ReplyKeyboardMarkup replyKeyboardMarkup3 = new(new[]
                                                                            {


                                 new KeyboardButton[] { "🔘 Пользовательское соглашение" },
                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню" },
                                })
                                            {
                                                ResizeKeyboard = true
                                            };

                                            Message sentMessage = await botClient.SendTextMessageAsync(
                                                    chatId: message.Chat.Id,
                                                   "Загрузка...",
                                                    replyMarkup: replyKeyboardMarkup3);

                                            //

                                            Message sentMessage2 = await botClient.SendPhotoAsync(
                                                  chatId: message.Chat.Id,
                                                  photo: "https://github.com/thelightone/questgame/raw/main/podpiska.jpg",
                                                  caption: "👑 Премиум-подписка дает полный доступ ко всем квестам, включая новые квесты каждый месяц." + "\n" + "\n" +
                                                    "☕️ Стоимость подписки всего 87 руб/месяц - дешевле, чем чашка кофе!" + "\n" + "\n" +
                                                    "⏱ Удобная оплата банковской картой всего за 2 минуты.",


                                            replyMarkup: new InlineKeyboardMarkup(
                                            InlineKeyboardButton.WithUrl(
                                            text: "Оформить подписку",
                                            url: "https://t.me/book_of_quests_paymentsbot")));



                                        }
                                        else
                                        {
                                            ReplyKeyboardMarkup replyKeyboardMarkup2 = new(new[]
                                                                                                                       {


                                 new KeyboardButton[] { "🔘 Пользовательское соглашение" },
                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню" },
                                })
                                            {
                                                ResizeKeyboard = true
                                            };

                                            Message sentMessage = await botClient.SendTextMessageAsync(
                                                    chatId: message.Chat.Id,
                                                   "Загрузка...",
                                                    replyMarkup: replyKeyboardMarkup2);

                                            //

                                            Message sentMessage2 = await botClient.SendPhotoAsync(
                                                  chatId: message.Chat.Id,
                                                  photo: "https://github.com/thelightone/questgame/raw/main/podpiska.jpg",
                                                  caption: "👑 Ваша подписка активна и дает вам полный доступ ко всем квестам, включая новые квесты каждый месяц!",
replyMarkup: new InlineKeyboardMarkup(
InlineKeyboardButton.WithUrl(
text: "Управлять подпиской",
url: "https://t.me/book_of_quests_paymentsbot")));

                                        }
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains("🔘 Пользовательское соглашение"))
                                {
                                    try
                                    {
                                        message = await botClient.SendDocumentAsync(
                                           chatId: message.Chat.Id,
                                           document: "https://github.com/thelightone/questgame/raw/main/%D0%9F%D0%BE%D0%BB%D1%8C%D0%B7%D0%BE%D0%B2%D0%B0%D1%82%D0%B5%D0%BB%D1%8C%D1%81%D0%BA%D0%BE%D0%B5%20%D1%81%D0%BE%D0%B3%D0%BB%D0%B0%D1%88%D0%B5%D0%BD%D0%B8%D0%B5%20Book%20of%20Quests.pdf");
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }

                                // ПРОВЕРКА ОПЛАТЫ
                                else if ((paid != "1")
                                    && (((Convert.ToInt16(stagequest1) > 18) && (Convert.ToInt16(stagequest1) < 50))
                                    || ((Convert.ToInt16(stagequest1) > 156) && (Convert.ToInt16(stagequest1) < 700))
                                    || ((Convert.ToInt16(stagequest1) > 724) && (Convert.ToInt16(stagequest1) < 800))
                                    || ((Convert.ToInt16(stagequest1) > 924) && (Convert.ToInt16(stagequest1) < 1001))))
                                {
                                    try
                                    {
                                        SubCheck();
                                        LoadingProgress();
                                        LoadingTechnik();


                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                                                             {


                                 new KeyboardButton[] { "🔘 Продолжить игру" },
                                 new KeyboardButton[] { "🔘 Вернуться в Главное меню" },
                                })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                               "Конец пробной части. Для продолжения, пожалуйста, оформите подписку." + "\n" + "\n" +
                                                  "Вы также можете попробовать другие квесты в Главном меню. Если вы оплатили подписку, нажмите 'Продолжить.'",
                                                replyMarkup: replyKeyboardMarkup);



                                        message = await botClient.SendTextMessageAsync(
                                          chatId: message.Chat.Id,
                                          text:
                                          "👑 Премиум-подписка дает полный доступа ко всем текущим и будущим квестам." + "\n" + "\n" +
                                                  "☕️ Стоимость подписки всего 87 руб/месяц - дешевле, чем чашка кофе!",

                                          replyMarkup: new InlineKeyboardMarkup(
                                          InlineKeyboardButton.WithUrl(
                                          text: "Оформить подписку",
                                          url: "https://t.me/book_of_quests_paymentsbot")));


                                        SaveProgress();
                                        SaveTechnic();
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }



                                //ОСНОВНЫЕ КНОПКИ
                                else if (message.Text.Contains(But1TextQuest1))
                                {

                                    try
                                    {
                                        Console.WriteLine(payday);
                                        Console.WriteLine(curDate);
                                        ChosedBut = "1";
                                        SaveTechnic();
                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                             {

                                 new KeyboardButton[] {" ",},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        LoadingTechnik();
                                        LoadingProgress();

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: AddText1,
                                                replyMarkup: replyKeyboardMarkup);

                                        ChosedBut = "1";
                                        FindingCheck();
                                        SaveProgress();
                                        SaveTechnic();
                                        LoadingTechnik();
                                        LoadingProgress();

                                        await Task.Delay(500);
                                        if (NumBut1 == 7)
                                        {
                                            Checkpoint();
                                        }
                                        ComplexFindings();
                                        await Task.Delay(1500);
                                        stagequest1 = Convert.ToString(NumBut1);

                                        Quest1();

                                        SaveProgress();
                                        SaveTechnic();
                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains(But2TextQuest1))
                                {

                                    try
                                    {
                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                     {

                                 new KeyboardButton[] {" ",},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        LoadingTechnik();
                                        LoadingProgress();

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: AddText2,
                                                replyMarkup: replyKeyboardMarkup);

                                        ChosedBut = "2";
                                        FindingCheck();
                                        SaveProgress();
                                        SaveTechnic();
                                        LoadingTechnik();
                                        LoadingProgress();

                                        await Task.Delay(500);
                                        if (NumBut2 == 7)
                                        {
                                            Checkpoint();
                                        }
                                        ComplexFindings();
                                        await Task.Delay(1500);
                                        stagequest1 = Convert.ToString(NumBut2);

                                        Console.WriteLine(message.Chat.Id);

                                        Quest1();

                                        SaveProgress();
                                        SaveTechnic();
                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains(But3TextQuest1))
                                {

                                    try
                                    {
                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                      {

                                 new KeyboardButton[] {" ",},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        LoadingTechnik();
                                        LoadingProgress();

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: AddText3,
                                                replyMarkup: replyKeyboardMarkup);

                                        ChosedBut = "3";
                                        FindingCheck();
                                        SaveProgress();
                                        SaveTechnic();
                                        LoadingTechnik();
                                        LoadingProgress();

                                        await Task.Delay(500);
                                        if (NumBut3 == 7)
                                        {
                                            Checkpoint();
                                        }
                                        ComplexFindings();
                                        await Task.Delay(1500);
                                        stagequest1 = Convert.ToString(NumBut3);

                                        Quest1();

                                        SaveProgress();
                                        SaveTechnic();
                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                else if (message.Text.Contains(But4TextQuest1))
                                {

                                    try
                                    {
                                        LoadingTechnik();
                                        LoadingProgress();

                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                         {

                                 new KeyboardButton[] {" ",},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                text: AddText4,
                                                replyMarkup: replyKeyboardMarkup);

                                        ChosedBut = "4";
                                        FindingCheck();
                                        SaveProgress();
                                        SaveTechnic();
                                        LoadingTechnik();
                                        LoadingProgress();

                                        await Task.Delay(500);
                                        if (NumBut4 == 7)
                                        {
                                            Checkpoint();
                                        }
                                        ComplexFindings();
                                        await Task.Delay(1500);
                                        stagequest1 = Convert.ToString(NumBut4);

                                        Quest1();

                                        SaveProgress();
                                        SaveTechnic();
                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }

                                else
                                {
                                    try
                                    {
                                        Message sentMessage = await botClient.SendPhotoAsync(
                                                                      chatId: message.Chat.Id,
                                                                      photo: "https://github.com/thelightone/questgame/raw/main/IMG_3030.jpg",
                                                                     caption: "Пожалуйста, используйте кнопки." + "\n" + "\n" +
                                                                      "Для переключения клавиатуры, нажмите сюда."
                                                                      );
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }


                                //методы игры

                                //блок сборных артефактов
                                async void ComplexFindings()
                                {
                                    try
                                    {
                                        if (havestick1 == true && havestick2 == true && havestick0 == false)
                                        {
                                            await botClient.SendPhotoAsync(
                                            chatId: message.Chat.Id,
                                            photo: "https://github.com/thelightone/questgame/raw/main/stick.png",
                                            caption: "Вы собрали Волшебный Посох ⚡️");
                                            havestick0 = true;
                                            await Task.Delay(1000);
                                            SaveProgress();

                                        }
                                        else if (checkbed == true && checktable == true && checkkomod == true && checkall == false)
                                        {
                                            checkall = true;
                                        }
                                        await Task.Delay(1000);
                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }


                                //комнаты главного меню

                                async void StartbotChoosegame()
                                {

                                    try
                                    {
                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] {
                                new KeyboardButton[] { "🏰 RPG", "🔍 Детектив" },
                                new KeyboardButton[] { "☠️ Хоррор", "💋 18+" },
                                new KeyboardButton[] { "🔘 Вернуться в Главное меню" },
                            })
                                        {
                                            ResizeKeyboard = true
                                        };
                                        Message sentMessage = await botClient.SendPhotoAsync(
                                            chatId: message.Chat.Id,
                                              photo: "https://github.com/thelightone/questgame/raw/main/categories.jpg",
                                                  caption: "Выберите категорию:",
                                            replyMarkup: replyKeyboardMarkup, cancellationToken: token);
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }

                                }
                                async void Igra()
                                {
                                    try
                                    {
                                        await botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "🏗 В этой категории пока нет игр, но они скоро появятся!");

                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                async void Сontiniue()
                                {
                                    try
                                    {

                                        if (stagequest1 == "0")
                                        {

                                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                             {

                            new KeyboardButton[] {"🔘 Начать новую игру"},
                            new KeyboardButton[] {"👑 Подписка","☎️ Контакты"},

                    })
                                            {
                                                ResizeKeyboard = true
                                            };

                                            try
                                            {
                                                Message sentMessage = await botClient.SendPhotoAsync(
                                                         chatId: message.Chat.Id,
                                                                                                        photo: "https://github.com/thelightone/questgame/raw/main/mainmenu.jpg",
                                                                                                        caption: "Добро пожаловать в игру!" + "\n" + "\n" +
                                                                                                        "<b>Book of Quests</b> - первый сборник текстовых квестов и визуальных новелл в Телеграм:" + "\n" + "\n" +
                                                                                                        " ⚜️ RPG, Хорроры, 18+ и другие" + "\n" +
                                                                                                        " ⚜️ Авторские арты и музыка" + "\n" +
                                                                                                        " ⚜️ Без скачиваний и установок" + "\n" + "\n" +
                                                                                                        "<b>Новые квесты каждый месяц!</b>",
                                                                                                        pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);

                                            }
                                            catch (Exception exp)
                                            {
                                                Console.WriteLine(exp);
                                                await Task.Delay(2000);

                                                return;
                                            }


                                        }



                                        else
                                        {
                                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                               {
                            new KeyboardButton[] {"🔘 Продолжить игру"},
                            new KeyboardButton[] {"🔘 Начать новую игру"},
                            new KeyboardButton[] {"👑 Подписка","☎️ Контакты"},

                            })
                                            {
                                                ResizeKeyboard = true
                                            };

                                            try
                                            {
                                                Message sentMessage = await botClient.SendPhotoAsync(
                                                      chatId: message.Chat.Id,
                                                                                                     photo: "https://github.com/thelightone/questgame/raw/main/mainmenu.jpg",
                                                                                                     caption: "Добро пожаловать в игру!" + "\n" + "\n" +
                                                                                                     "<b>Book of Quests</b> - первый сборник текстовых квестов и визуальных новелл в Телеграм:" + "\n" + "\n" +
                                                                                                     " ⚜️ RPG, Хорроры, 18+ и другие" + "\n" +
                                                                                                     " ⚜️ Авторские арты и музыка" + "\n" +
                                                                                                     " ⚜️ Без скачиваний и установок" + "\n" + "\n" +
                                                                                                     "<b>Новые квесты каждый месяц!</b>",
                                                                                                     pm = ParseMode.Html, replyMarkup: replyKeyboardMarkup);
                                            }
                                            catch (Exception exp)
                                            {
                                                Console.WriteLine(exp);
                                                await Task.Delay(2000);

                                                return;
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }


                                //тело игры

                                async void Quest1()
                                {
                                    try
                                    {
                                        Database();



                                        if (photolinkQuest1 != "1")
                                        {
                                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                                {
                            new KeyboardButton[] { But1TextQuest1, But3TextQuest1 },
                            new KeyboardButton[] { But2TextQuest1, But4TextQuest1 },
                        })

                                            {
                                                ResizeKeyboard = true
                                            };

                                            Message sentMessage = await botClient.SendPhotoAsync(
                                                chatId: message.Chat.Id,
                                                photo: photolinkQuest1,
                                                caption: TextQuest1,
                                                replyMarkup: replyKeyboardMarkup);


                                            return;
                                        }
                                        else
                                        {
                                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                                {
                            new KeyboardButton[] { But1TextQuest1, But3TextQuest1 },
                            new KeyboardButton[] { But2TextQuest1, But4TextQuest1 },
                        })


                                            {
                                                ResizeKeyboard = true
                                            };

                                            Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,

                                                TextQuest1,
                                                replyMarkup: replyKeyboardMarkup);


                                            return;


                                        }
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }

                                }
                                async void FindingCheck()
                                {
                                    await Task.Delay(1000);

                                    try
                                    {
                                        if (Finding == nameof(havefire) && ChosedBut == PlaceChosBut) havefire = true;
                                        if (Finding == nameof(havekey) && ChosedBut == PlaceChosBut) havekey = true;
                                        if (Finding == nameof(solvedquest) && ChosedBut == PlaceChosBut) solvedquest = true;
                                        if (Finding == nameof(havestick1) && ChosedBut == PlaceChosBut) havestick1 = true;
                                        if (Finding == nameof(havestick2) && ChosedBut == PlaceChosBut) havestick2 = true;
                                        if (Finding == nameof(oskolok) && ChosedBut == PlaceChosBut) oskolok = true;
                                        if (Finding == nameof(stranger) && ChosedBut == PlaceChosBut) stranger = true;
                                        if (Finding == nameof(sdelkaotkaz) && ChosedBut == PlaceChosBut) sdelkaotkaz = true;
                                        if (Finding == nameof(monsterdead) && ChosedBut == PlaceChosBut) monsterdead = true;

                                        if (Finding == nameof(havestones) && ChosedBut == PlaceChosBut) havestones = true;
                                        if (Finding == nameof(haveshovel) && ChosedBut == PlaceChosBut) haveshovel = true;
                                        if (Finding == nameof(havemeet) && ChosedBut == PlaceChosBut) havemeet = true;
                                        if (Finding == nameof(fisher) && ChosedBut == PlaceChosBut) fisher = true;
                                        if (Finding == nameof(mistake) && ChosedBut == PlaceChosBut) mistake = true;
                                        if (Finding == nameof(haveplant) && ChosedBut == PlaceChosBut) haveplant = true;
                                        if (Finding == nameof(ask) && ChosedBut == PlaceChosBut) ask = true;
                                        if (Finding == nameof(findfigure) && ChosedBut == PlaceChosBut) findfigure = true;

                                        if (Finding == nameof(checktable) && ChosedBut == PlaceChosBut) checktable = true;
                                        if (Finding == nameof(checkbed) && ChosedBut == PlaceChosBut) checkbed = true;
                                        if (Finding == nameof(checkkomod) && ChosedBut == PlaceChosBut) checkkomod = true;
                                        if (Finding == nameof(stayhome) && ChosedBut == PlaceChosBut) stayhome = true;
                                        if (Finding == nameof(checkkitch) && ChosedBut == PlaceChosBut) checkkitch = true;
                                        if (Finding == nameof(checkall) && ChosedBut == PlaceChosBut) checkall = true;




                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }

                                }
                                async void Database()

                                {
                                    try
                                    {
                                        subnumberQuest1 = 1;

                                        // ПОДКЛЮЧЕНИЕ БАЗЫ ДАННЫХ

                                        //открытие базы
                                        IDbConnection dbcon = new SqliteConnection("Data Source=databaseforapp8.db");
                                        dbcon.Open();
                                        //создание какой то команды
                                        IDbCommand dbcmd = dbcon.CreateCommand();
                                        //команда на языке sql и некий ее результат

                                        string sql =
                                            "SELECT * FROM app8database WHERE Number ='" + stagequest1 + "' ";
                                        dbcmd.CommandText = sql;

                                        //что то типо выполни команду
                                        IDataReader reader = dbcmd.ExecuteReader();

                                        // прочитывание результата формата sql и манипуляции с ним в (задача переменных)
                                        reader.Read();


                                        condition = reader.GetString(20);
                                        subnumberQuest1 = 1;

                                        if ((condition == nameof(havefire) && havefire == true)
                                                || (condition == nameof(havekey) && havekey == true)
                                                 || (condition == nameof(solvedquest) && solvedquest == true)
                                                 || (condition == nameof(havestick1) && havestick1 == true)
                                                 || (condition == nameof(havestick2) && havestick2 == true)
                                                 || (condition == nameof(havestick0) && havestick0 == true)
                                                 || (condition == nameof(oskolok) && oskolok == true)
                                                 || (condition == nameof(stranger) && stranger == true)
                                                 || (condition == nameof(sdelkaotkaz) && sdelkaotkaz == true)
                                            || (condition == nameof(monsterdead) && monsterdead == true)

                                            || (condition == nameof(havestones) && havestones == true)
                                            || (condition == nameof(haveshovel) && haveshovel == true)
                                            || (condition == nameof(havemeet) && havemeet == true)
                                            || (condition == nameof(fisher) && fisher == true)
                                            || (condition == nameof(mistake) && mistake == true)
                                            || (condition == nameof(haveplant) && haveplant == true)
                                            || (condition == nameof(ask) && ask == true)
                                            || (condition == nameof(findfigure) && findfigure == true)

                                            || (condition == nameof(checktable) && checktable == true)
                                            || (condition == nameof(checkbed) && checkbed == true)
                                            || (condition == nameof(checkkomod) && checkkomod == true)
                                            || (condition == nameof(stayhome) && stayhome == true)
                                            || (condition == nameof(checkkitch) && checkkitch == true)
                                            || (condition == nameof(checkall) && checkall == true)


                                            )
                                            subnumberQuest1 = 2;

                                        else subnumberQuest1 = 1;

                                        //заново

                                        dbcmd = dbcon.CreateCommand();

                                        if (subnumberQuest1 == 1)
                                        {
                                            sql =
                                        "SELECT * FROM app8database WHERE Number LIKE'" + stagequest1 + "' AND SubNumber LIKE 1";
                                            dbcmd.CommandText = sql;
                                        }
                                        else if (subnumberQuest1 == 2)
                                        {
                                            sql =
                                            "SELECT * FROM app8database WHERE Number LIKE'" + stagequest1 + "' AND SubNumber LIKE 2";
                                            dbcmd.CommandText = sql;
                                        }





                                        //что то типо выполни команду
                                        reader = dbcmd.ExecuteReader();
                                        // прочитывание результата формата sql и манипуляции с ним в (задача переменных)
                                        reader.Read();

                                        subnumberQuest1 = reader.GetInt32(1);


                                        TextQuest1 = reader.GetString(4);

                                        HalfBut1TextQuest1 = reader.GetString(6);
                                        if (HalfBut1TextQuest1 == "0")
                                        {
                                            But1TextQuest1 = "";
                                            AddText1 = "1";
                                            NumBut1 = Convert.ToInt32(stagequest1);

                                        }
                                        else
                                        {
                                            But1TextQuest1 = HalfBut1TextQuest1;
                                            AddText1 = reader.GetString(7);
                                            NumBut1 = reader.GetInt32(5);

                                        }


                                        HalfBut2TextQuest1 = reader.GetString(9);
                                        if (HalfBut2TextQuest1 == "0")
                                        {
                                            But2TextQuest1 = "";
                                            AddText2 = "1";

                                            NumBut2 = Convert.ToInt32(stagequest1);
                                        }
                                        else
                                        {
                                            But2TextQuest1 = HalfBut2TextQuest1;
                                            AddText2 = reader.GetString(10);
                                            NumBut2 = reader.GetInt32(8);
                                        }


                                        HalfBut3TextQuest1 = reader.GetString(12);
                                        if (HalfBut3TextQuest1 == "0")
                                        {
                                            But3TextQuest1 = "";
                                            AddText3 = "1";
                                            NumBut3 = Convert.ToInt32(stagequest1);

                                        }
                                        else
                                        {
                                            But3TextQuest1 = HalfBut3TextQuest1;
                                            AddText3 = reader.GetString(13);
                                            NumBut3 = reader.GetInt32(11);
                                        }


                                        HalfBut4TextQuest1 = reader.GetString(15);
                                        if (HalfBut4TextQuest1 == "0")
                                        {
                                            But4TextQuest1 = "";
                                            AddText4 = "1";
                                            NumBut4 = Convert.ToInt32(stagequest1);

                                        }
                                        else
                                        {
                                            But4TextQuest1 = HalfBut4TextQuest1;
                                            AddText4 = reader.GetString(16);
                                            NumBut4 = reader.GetInt32(14);
                                        }


                                        photolinkQuest1 = reader.GetString(17);
                                        FindingsNum = reader.GetString(18);
                                        Finding = reader.GetString(19);


                                        if (FindingsNum == Convert.ToString(NumBut1)) PlaceChosBut = "1";
                                        else if (FindingsNum == Convert.ToString(NumBut2)) PlaceChosBut = "2";
                                        else if (FindingsNum == Convert.ToString(NumBut3)) PlaceChosBut = "3";
                                        else if (FindingsNum == Convert.ToString(NumBut4)) PlaceChosBut = "4";


                                        FindingCheck();

                                        //записываем номер игры
                                        IDbCommand addgamenum = dbcon.CreateCommand();


                                        string sql2 =
                                            "UPDATE app8database SET But2Link = '" + CurGame + "' WHERE Number = 7";
                                        addgamenum.CommandText = sql2;

                                        addgamenum.ExecuteNonQuery();

                                        //записываем номер игры




                                        //clean up

                                        addgamenum.Dispose();
                                        reader.Dispose();
                                        dbcmd.Dispose();
                                        dbcon.Close();


                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                async void Checkpoint()
                                {

                                    try
                                    {
                                        //открытие базы
                                        IDbConnection dbcon = new SqliteConnection("Data Source=databaseforapp8.db");
                                        dbcon.Open();
                                        IDbCommand checkpoint = dbcon.CreateCommand();


                                        string sql3 =
                                            "UPDATE app8database SET But1Link = '" + (Convert.ToInt32(stagequest1)) + "' WHERE Number = 7";

                                        checkpoint.CommandText = sql3;

                                        checkpoint.ExecuteNonQuery();
                                        checkpoint.Dispose();
                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                async void SaveProgress()

                                {
                                    try
                                    {
                                        ChatId = Convert.ToString(message.Chat.Id);
                                        // ПОДКЛЮЧЕНИЕ БАЗЫ ДАННЫХ


                                        //открытие базы
                                        IDbConnection dbcon20 = new SqliteConnection("Data Source=Savings.db");

                                        dbcon20.Open();
                                        //создание какой то команды


                                        IDbCommand firstsave2 = dbcon20.CreateCommand();
                                        firstsave2.CommandText = "SELECT count(*) FROM Savings WHERE ChatId='" + ChatId + "'";
                                        Console.WriteLine("4");

                                        int count2 = Convert.ToInt32(firstsave2.ExecuteScalar());
                                        if (count2 == 0)
                                        {
                                            Console.WriteLine("XXX");
                                            firstsave2.CommandText = "INSERT INTO Savings (ChatId, Stagequest, HaveFire, HaveKey, SolvedQuest, havestick1, havestick2, havestick0, CurGame, invitation, paid, oskolok, stranger, sdelkaotkaz, monsterdead, pay_link, payday, url, " +
                                    "havestones, haveshovel, havemeet, fisher, mistake, haveplant, ask, findfigure, checktable, checkbed, checkkomod, stayhome, checkkitch,checkall)" +
                                    "VALUES ('" + ChatId + "','0', 'false', 'false', 'false', 'false', 'false', 'false', 0, '0', '0','false','false','false','false','false' ,'false','false'" +
                                    ",'false','false' ,'false','false','false','false' ,'false','false','false','false','false','false' ,'false','false')";

                                            firstsave.ExecuteNonQuery();
                                            Console.WriteLine("5");

                                            firstsave2.Dispose();
                                        }


                                        IDbCommand savegame = dbcon20.CreateCommand();


                                        savegame.CommandText = "DELETE FROM Savings WHERE ChatId = '" + ChatId + "'";
                                        savegame.ExecuteNonQuery();
                                        Console.WriteLine(havestones);

                                        IDbCommand savegame2 = dbcon20.CreateCommand();

                                        savegame2.CommandText = "INSERT INTO Savings (ChatId, Stagequest, HaveFire, HaveKey, SolvedQuest, havestick1, havestick2, havestick0, CurGame, invitation, paid, oskolok, stranger, sdelkaotkaz, monsterdead, pay_link, payday, url, " +
                                    "havestones, haveshovel, havemeet, fisher, mistake, haveplant, ask, findfigure, checktable, checkbed, checkkomod, stayhome, checkkitch,checkall) " +
                                    "VALUES ('" + ChatId + "','" + stagequest1 + "','" + havefire + "','" + havekey + "','" + solvedquest + "','" + havestick1 + "','" + havestick2 + "','" + havestick0 + "','" + CurGame + "','" + invitation + "','" + paid + "','" + oskolok + "','" + stranger + "','" + sdelkaotkaz + "','" + monsterdead + "','" + pay_link + "','" + payday + "','" + url + "','" + havestones + "','" + haveshovel + "','" + havemeet + "','" + fisher + "','" + mistake + "','" + haveplant + "','" + ask + "','" + findfigure + "','" + checktable + "','" + checkbed + "','" + checkkomod + "','" + stayhome + "','" + checkkitch + "','" + checkall + "')";

                                        savegame2.ExecuteNonQuery();
                                        savegame2.Dispose();
                                        //записываем номер игры




                                        IDbCommand stats = dbcon20.CreateCommand();
                                        stats.CommandText = "DELETE FROM stats WHERE ChatId = '" + ChatId + "' AND Date = '" + DateTime.Today + "'";
                                        stats.ExecuteNonQuery();
                                        stats.CommandText = "INSERT INTO stats (ChatId, Date, CurGame, Stagequest, paid) VALUES ('" + ChatId + "','" + DateTime.Today + "','" + CurGame + "','" + stagequest1 + "','" + paid + "')";

                                        stats.ExecuteNonQuery();



                                        //clean up


                                        stats.Dispose();
                                        dbcon20.Close();



                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                async void LoadingProgress()

                                {
                                    try
                                    {
                                        SubCheck();
                                        ChatId = Convert.ToString(message.Chat.Id);

                                        // ПОДКЛЮЧЕНИЕ БАЗЫ ДАННЫХ


                                        //открытие базы
                                        IDbConnection dbcon4 = new SqliteConnection("Data Source=Savings.db");
                                        dbcon4.Open();
                                        //создание какой то команды
                                        IDbCommand loading = dbcon4.CreateCommand();
                                        //команда на языке sql и некий ее результат

                                        string sql4 =
                                            "SELECT * FROM Savings WHERE ChatId ='" + ChatId + "' ";
                                        loading.CommandText = sql4;

                                        //что то типо выполни команду
                                        IDataReader reader2 = loading.ExecuteReader();

                                        // прочитывание результата формата sql и манипуляции с ним в (задача переменных)
                                        reader2.Read();

                                        CurGame = reader2.GetString(8);
                                        havefire = Convert.ToBoolean(reader2.GetString(2));
                                        havekey = Convert.ToBoolean(reader2.GetString(3));
                                        solvedquest = Convert.ToBoolean(reader2.GetString(4));
                                        havestick1 = Convert.ToBoolean(reader2.GetString(5));
                                        havestick2 = Convert.ToBoolean(reader2.GetString(6));
                                        havestick0 = Convert.ToBoolean(reader2.GetString(7));
                                        stagequest1 = reader2.GetString(1);
                                        invitation = reader2.GetString(9);
                                        paid = reader2.GetString(10);
                                        oskolok = Convert.ToBoolean(reader2.GetString(11));
                                        stranger = Convert.ToBoolean(reader2.GetString(12));
                                        sdelkaotkaz = Convert.ToBoolean(reader2.GetString(13));
                                        monsterdead = Convert.ToBoolean(reader2.GetString(14));
                                        pay_link = reader2.GetString(15);
                                        payday = reader2.GetString(16);
                                        url = reader2.GetString(17);

                                        havestones = Convert.ToBoolean(reader2.GetString(18));
                                        haveshovel = Convert.ToBoolean(reader2.GetString(19));
                                        havemeet = Convert.ToBoolean(reader2.GetString(20));
                                        fisher = Convert.ToBoolean(reader2.GetString(21));
                                        mistake = Convert.ToBoolean(reader2.GetString(22));
                                        haveplant = Convert.ToBoolean(reader2.GetString(23));
                                        ask = Convert.ToBoolean(reader2.GetString(24));
                                        findfigure = Convert.ToBoolean(reader2.GetString(25));

                                        checktable = Convert.ToBoolean(reader2.GetString(26));
                                        checkbed = Convert.ToBoolean(reader2.GetString(27));
                                        checkkomod = Convert.ToBoolean(reader2.GetString(28));
                                        stayhome = Convert.ToBoolean(reader2.GetString(29));
                                        checkkitch = Convert.ToBoolean(reader2.GetString(30));
                                        checkall = Convert.ToBoolean(reader2.GetString(31));




                                        //clean up


                                        reader2.Dispose();
                                        loading.Dispose();
                                        dbcon4.Close();


                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                async void SaveTechnic()

                                {
                                    try
                                    {
                                        ChatId = Convert.ToString(message.Chat.Id);
                                        // ПОДКЛЮЧЕНИЕ БАЗЫ ДАННЫХ


                                        //где лежит база
                                        //открытие базы
                                        IDbConnection dbcon3 = new SqliteConnection("Data Source=technic.db");

                                        dbcon3.Open();
                                        //создание какой то команды

                                        IDbCommand technicadd1 = dbcon3.CreateCommand();
                                        technicadd1.CommandText = "SELECT count(*) FROM technic WHERE ChatId='" + ChatId + "'";
                                        int count2 = Convert.ToInt32(technicadd1.ExecuteScalar());
                                        Console.WriteLine("14");
                                        if (count2 == 0)
                                        {

                                            Console.WriteLine("15");
                                            technicadd1.CommandText = "INSERT INTO technic (ChatId, FindingsNum, Finding, subnumberQuest1, TextQuest1, photolinkQuest1, HalfBut1TextQuest1, HalfBut2TextQuest1, HalfBut3TextQuest1, HalfBut4TextQuest1, But1TextQuest1, But2TextQuest1, But3TextQuest1, But4TextQuest1, condition, AddText1, AddText2, AddText3, AddText4, NumBut1, NumBut2, NumBut3, NumBut4, ChosedBut, PlaceChosBut) VALUES ( '" + ChatId + "','0','nofindings', '1', '1', '1', '0', '0', '0', '0', '1', '1', '1', '1', 'false', '1', '1', '1', '1', '1', '1', '1', '1', '0', '0')";
                                            Console.WriteLine("151");
                                            technicadd1.ExecuteNonQuery();
                                            Console.WriteLine("160");
                                            technicadd1.Dispose();
                                            Console.WriteLine("16");

                                            technicadd1.Dispose();
                                        }

                                        IDbCommand savetechnic = dbcon3.CreateCommand();


                                        savetechnic.CommandText = "DELETE FROM technic WHERE ChatId = '" + ChatId + "'";
                                        savetechnic.ExecuteNonQuery();
                                        savetechnic.CommandText = "INSERT INTO technic (ChatId, FindingsNum, Finding, subnumberQuest1, TextQuest1, photolinkQuest1, HalfBut1TextQuest1, HalfBut2TextQuest1, HalfBut3TextQuest1, HalfBut4TextQuest1, But1TextQuest1, But2TextQuest1, But3TextQuest1, But4TextQuest1, condition, AddText1, AddText2, AddText3, AddText4, NumBut1, NumBut2, NumBut3, NumBut4, ChosedBut, PlaceChosBut)" +
                                            " VALUES ('" + ChatId + "','" + FindingsNum + "','" + Finding + "','" + subnumberQuest1 + "','" + TextQuest1 + "','" + photolinkQuest1 + "','" + HalfBut1TextQuest1 + "','" + HalfBut2TextQuest1 + "', '" + HalfBut3TextQuest1 + "','" + HalfBut4TextQuest1 + "','" + But1TextQuest1 + "','" + But2TextQuest1 + "', '" + But3TextQuest1 + "','" + But4TextQuest1 + "','" + condition + "','" + AddText1 + "','" + AddText2 + "','" + AddText3 + "','" + AddText4 + "', '" + NumBut1 + "', '" + NumBut2 + "', '" + NumBut3 + "', '" + NumBut4 + "','" + ChosedBut + "', '" + PlaceChosBut + "')";
                                        savetechnic.ExecuteNonQuery();

                                        //записываем номер игры




                                        //clean up

                                        savetechnic.Dispose();
                                        dbcon3.Close();


                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }
                                }
                                async void LoadingTechnik()

                                {
                                    try
                                    {

                                        //открытие базы
                                        IDbConnection dbcon7 = new SqliteConnection("Data Source=technic.db");
                                        dbcon7.Open();

                                        //создание какой то команды
                                        IDbCommand technicadd1 = dbcon7.CreateCommand();
                                        technicadd1.CommandText = "SELECT count(*) FROM technic WHERE ChatId='" + ChatId + "'";
                                        int count2 = Convert.ToInt32(technicadd1.ExecuteScalar());
                                        Console.WriteLine("14");
                                        if (count2 == 0)
                                        {

                                            Console.WriteLine("15");
                                            technicadd1.CommandText = "INSERT INTO technic (ChatId, FindingsNum, Finding, subnumberQuest1, TextQuest1, photolinkQuest1, HalfBut1TextQuest1, HalfBut2TextQuest1, HalfBut3TextQuest1, HalfBut4TextQuest1, But1TextQuest1, But2TextQuest1, But3TextQuest1, But4TextQuest1, condition, AddText1, AddText2, AddText3, AddText4, NumBut1, NumBut2, NumBut3, NumBut4, ChosedBut, PlaceChosBut) VALUES ( '" + ChatId + "','0','nofindings', '1', '1', '1', '0', '0', '0', '0', '1', '1', '1', '1', 'false', '1', '1', '1', '1', '1', '1', '1', '1', '0', '0')";
                                            Console.WriteLine("151");
                                            technicadd1.ExecuteNonQuery();
                                            Console.WriteLine("160");
                                            technicadd1.Dispose();
                                            Console.WriteLine("16");
                                        }
                                        Console.WriteLine("14.1");
                                        IDbCommand technikload = dbcon7.CreateCommand();

                                        //команда на языке sql и некий ее результат

                                        technikload.CommandText =
                                            "SELECT * FROM technic WHERE ChatId ='" + ChatId + "' ";

                                        Console.WriteLine("14.2");
                                        //что то типо выполни команду
                                        IDataReader reader3 = technikload.ExecuteReader();

                                        // прочитывание результата формата sql и манипуляции с ним в (задача переменных)
                                        reader3.Read();

                                        string FindingsNum = reader3.GetString(1);
                                        string Finding = reader3.GetString(2);
                                        int subnumberQuest1 = Convert.ToInt32(reader3.GetString(3));
                                        string TextQuest1 = reader3.GetString(4);
                                        string photolinkQuest1 = reader3.GetString(5);
                                        string HalfBut1TextQuest1 = reader3.GetString(6).Trim();
                                        string HalfBut2TextQuest1 = reader3.GetString(7).Trim();
                                        string HalfBut3TextQuest1 = reader3.GetString(8).Trim();
                                        string HalfBut4TextQuest1 = reader3.GetString(9).Trim();
                                        string But1TextQuest1 = reader3.GetString(10).Trim();
                                        string But2TextQuest1 = reader3.GetString(11).Trim();
                                        string But3TextQuest1 = reader3.GetString(12).Trim();
                                        string But4TextQuest1 = reader3.GetString(13).Trim();
                                        string condition = reader3.GetString(14);

                                        string AddText1 = reader3.GetString(15);
                                        string AddText2 = reader3.GetString(16);
                                        string AddText3 = reader3.GetString(17);
                                        string AddText4 = reader3.GetString(18);
                                        int NumBut1 = Convert.ToInt32(reader3.GetString(19));
                                        int NumBut2 = Convert.ToInt32(reader3.GetString(20));
                                        int NumBut3 = Convert.ToInt32(reader3.GetString(21));
                                        int NumBut4 = Convert.ToInt32(reader3.GetString(22));
                                        string ChosedBut = reader3.GetString(23);
                                        string PlaceChosBut = reader3.GetString(24);


                                        technikload.Dispose();
                                        reader3.Dispose();
                                        technicadd1.Dispose();
                                        dbcon7.Close();
                                        Console.WriteLine("14.3");
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;

                                    }

                                }
                                async void InvitCheck()
                                {
                                    try
                                    {
                                        LoadingTechnik();
                                        LoadingProgress();

                                        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                                        {

                                 new KeyboardButton[] {"💪 Подписался", "👿 Не хочу поддерживать"},

                    })
                                        {
                                            ResizeKeyboard = true
                                        };

                                        Message sentMessage = await botClient.SendTextMessageAsync(
                                                chatId: message.Chat.Id,
                                                "💡Присоединяйтесь к официальному каналу игры - @book_of_quests, чтобы не пропустить выход новых квестов!",
                                                replyMarkup: replyKeyboardMarkup);


                                        SaveProgress();
                                        SaveTechnic();


                                        return;
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;
                                    }

                                }

                                async void SubCheck()
                                {
                                    try
                                    {
                                        var user_channel_status = await botClient.GetChatMemberAsync(-1001620051798, long.Parse(ChatId), default);
                                        if (user_channel_status.Status.ToString().ToLower() == "left"
                                         || user_channel_status.Status.ToString().ToLower() == "kicked")
                                        {
                                            paid = "0";
                                        }
                                        else
                                        {
                                            paid = "1";
                                        }
                                        SaveProgress();
                                        Console.WriteLine(user_channel_status.Status.ToString());
                                    }
                                    catch
                                    {
                                        await Task.Delay(1000);
                                        return;
                                    }
                                }



                                SaveProgress();
                                SaveTechnic();

                                return;
                            }






                            catch (Exception exception)
                            {
                                Console.WriteLine(exception);
                                await Task.Delay(2000);

                                return;


                            }
                        }

                    }


                    // обработка ошибок
                    static Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
                    {
                        
                            Console.WriteLine($"\n\n================= {DateTime.Now} ================= \n\n{exception}");
                            Console.WriteLine("Перезапускаю сервер...");

                            throw new NotImplementedException();
                        
                       
                    }
                }
                catch
                {
                    Task.Delay(1000);
                    return;
                }
            }


        }


    }
}




















