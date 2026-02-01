using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        PlayFabSettings.TitleId = "A126"; // <-- your Title ID
        string webhook = "https://discord.com/api/webhooks/1467480545360347280/ir-6D39z8rCvBUdLYJ03lv0q_Q3uijHCeJCpt8kl51R_fYCklJwjPt7aQzR0iCAwbQcF; // <-- your webhook

        int count = 0;
        var cts = new CancellationTokenSource();
        HttpClient httpClient = new HttpClient();

        _ = Task.Run(() =>
        {
            Console.ReadKey(true);
            cts.Cancel();
        });

        while (!cts.IsCancellationRequested)
        {
            count++;

            string customId = "TEST_" + Guid.NewGuid().ToString("N")[..12];

            var request = new LoginWithCustomIDRequest
            {
                CustomId = customId,
                CreateAccount = true
            };

            try
            {
                var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

                if (result.Error == null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{count,6}] SUCCESS → {result.Result.PlayFabId}");
                    Console.ResetColor();

                    var payload = new
                    {
                        username = "PlayFab Logger",
                        embeds = new[]
                        {
                            new
                            {
                                title = "New PlayFab Account Created",
                                color = 0x00ff00,
                                fields = new[]
                                {
                                    new { name = "Title ID", value = PlayFabSettings.TitleId, inline = true },
                                    new { name = "Custom ID", value = customId, inline = false },
                                    new { name = "PlayFab ID", value = result.Result.PlayFabId, inline = false },
                                    new { name = "Session Ticket", value = $"```{result.Result.SessionTicket}```", inline = false }
                                }
                            }
                        }
                    };

                    string json = JsonSerializer.Serialize(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    await httpClient.PostAsync(webhook, content);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{count,6}] FAILED → {result.Error.ErrorMessage}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{count,6}] EXCEPTION → {ex.Message}");
            }

            await Task.Delay(5000, cts.Token);
        }

        Console.WriteLine($"\nSTOPPED. Total accounts attempted: {count}");
        Console.ReadKey();
    }
}
