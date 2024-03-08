using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

// TODO: Add here your credentials
var kernelBuilder = Kernel.CreateBuilder()
    .AddMistralChatCompletion(
        apiKey: "",
        modelId: "mistral-small",
        serviceId: "mistral");

kernelBuilder.Services.AddLogging(c =>
{
    c.AddDebug().SetMinimumLevel(LogLevel.Trace);
    c.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        options.SingleLine = true;
        options.TimestampFormat = "hh:mm:ss ";
    });
});

var kernel = kernelBuilder.Build();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

ChatHistory history = new ChatHistory();

history.AddUserMessage("How much will be 10 + 2?");

var result = await chatCompletionService.GetChatMessageContentAsync(
    history,
    kernel: kernel);

string fullMessage = result.Content;

Console.WriteLine(fullMessage);

var streamingResult = chatCompletionService.GetStreamingChatMessageContentsAsync(
    history,
    kernel: kernel);

var resultStringBuilder = new StringBuilder();
await foreach (var content in streamingResult)
{
    resultStringBuilder.Append(content);
}

Console.WriteLine(resultStringBuilder.ToString());