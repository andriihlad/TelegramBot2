using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




var botClient = new TelegramBotClient("5227473730:AAFsMATRsqAIApMHnQJz3a19LVoioK4-mNs");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};
botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type != UpdateType.Message)
        return;
    if (update.Message!.Type != MessageType.Text)
        return;

    var chatId = update.Message.Chat.Id;
    var messageText = update.Message.Text;
    var messageToSend = "s";
    if (messageText != null)
    {
        if (messageText== "Бомбосховища")
        {
            messageToSend=  "https://www.google.com/maps/d/u/0/viewer?mid=14VU7WtHmDj3DF2nPVbMBuDM7meXg3Vo6&ll=48.61261115098762%2C22.308921709863295&z=13&fbclid=IwAR3pfeaX4uLCCbZDtrkwlYMaTemXJfit4L86FDfGWcPDGYnMlUETNNPabeY";
        }
        else if (messageText=="SOS")
        {

        }

    }

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    // Echo received message text

    Message sentMessage = await botClient.SendTextMessageAsync(
       chatId: chatId,
       text: messageToSend,
       replyMarkup: GetButtons(),
       cancellationToken: cancellationToken);
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
static IReplyMarkup GetButtons()
{

    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
     {
        new KeyboardButton[] { "Бомбосховища", "Волонтерська допомога" },
        new KeyboardButton[] { "SOS", "Пункти прийому біженців" }
    })
    {
        ResizeKeyboard = true
    };
    return replyKeyboardMarkup;

}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.Run();