using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace H_W_9._4_ConsoleApp
{
    public static class Program
    {

        private static TelegramBotClient Bot;

        public static async Task Main(string[] args)
        {
            string token = System.IO.File.ReadAllText(@"");

            Bot = new TelegramBotClient(token);

            var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };
            Bot.StartReceiving(
                UpdateHandler,
                ErrorHandler,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = Bot.GetMeAsync();

            Console.ReadLine();

            cts.Cancel();

            Bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);

            Console.ReadLine();
        }

        static Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception.Message;
            {
                ApiRequestException apiRequestException, apiRequestException1, requestException;
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {

            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    Console.WriteLine(update.Message.Document.FileId);
                    Console.WriteLine(update.Message.Document.FileName);
                    Console.WriteLine(update.Message.Document.FileSize);

                    DownLoad(update.Message.Document.FileId, update.Message.Document.FileName);
                }

                if (update.Message.Type == MessageType.Audio)
                {
                    Console.WriteLine(update.Message.Audio.FileId);
                    Console.WriteLine(update.Message.Audio.FileName);
                    Console.WriteLine(update.Message.Audio.FileSize);

                    DownLoad(update.Message.Audio.FileId, update.Message.Audio.FileName);
                }

                if (update.Message.Type == MessageType.Video)
                {
                    Console.WriteLine(update.Message.Video.FileId);
                    Console.WriteLine(update.Message.Video.FileName);
                    Console.WriteLine(update.Message.Video.FileSize);

                    DownLoad(update.Message.Video.FileId, update.Message.Video.FileName);
                }

                if (update.Message.Type == MessageType.Photo)
                {
                    Console.WriteLine(update.Message.Photo.Length);

                    DownLoad(update.Message.Photo.ToString(), update.Message.Photo.ToString());
                }


                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    var message = update.Message;

                    if (message.Text.ToLower() == "/start")
                    {
                        await Bot.SendTextMessageAsync(message.Chat, "Добрый день, мы рады, что вы с нами!");
                        return;
                    }

                    await Bot.SendTextMessageAsync(message.Chat, "Ky");
                }

                if (update.Message.Text == "/getFile")
                {
                    var directory = new DirectoryInfo("");

                    FileInfo[] files = directory.GetFiles();

                    foreach (var file in files)
                    {
                        FileStream str = System.IO.File.OpenRead(file.FullName);

                        InputOnlineFile iof = new InputOnlineFile(str);

                        iof.FileName = file.Name;

                        await Bot.SendDocumentAsync(update.Message.Chat.Id, iof, file.Name);

                        Thread.Sleep(300);

                        str.Close();

                        str.Dispose();
                    }
                }


            }
        }

        static async void DownLoad(string fileId, string path)
        {
            var file = await Bot.GetFileAsync(fileId);

            FileStream fs = new FileStream("_" + path, FileMode.Create);

            await Bot.DownloadFileAsync(file.FilePath, fs);

            fs.Close();

            fs.Dispose();
        }
    }

}
