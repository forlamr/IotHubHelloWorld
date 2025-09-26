using Microsoft.Azure.Devices.Client;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace IotHubDeviceSim
{
    internal class Program
    {
        public enum DeviceAuthType
        {
            Sas,
            SelfSigned
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Device Sim");

            DeviceAuthType deviceAuthType = DeviceAuthType.Sas; // Change to SelfSigned to use X.509 certificate authentication
            DeviceClient client = null;

            if(deviceAuthType == DeviceAuthType.Sas)
            {
                Console.WriteLine("Using SAS authentication");

                // Device01 SAS Authentication
                var deviceConnectionString = "<device01-iothub-connection-string>";
                client = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
            }
            else
            {
                Console.WriteLine("Using Self-signed authentication");

                // Device02 Self-signed
                var iotHubHostname = "<iothub-host-name>";
                var deviceId = "Device02";
                var certPath = "device02.pfx";  // device02 certificate path
                var certPassword = "";

                var certificate = new X509Certificate2(certPath, certPassword);
                var auth = new DeviceAuthenticationWithX509Certificate(deviceId, certificate);

                client = DeviceClient.Create(
                    iotHubHostname,
                    auth,
                    TransportType.Mqtt
                );
            }

            Console.WriteLine("Connection ...");
            await client.OpenAsync();
            Console.WriteLine("Client connected. Waiting of Direct Method...");

            // Direct method handler
            client.SetMethodHandlerAsync("myMethod", async (methodRequest, context) =>
            {
                Console.WriteLine($"Received direct method: {methodRequest.Name}, payload: {methodRequest.DataAsJson}");

                string responsePayload = "{\"answer\": \"ok\"}";
                return new MethodResponse(Encoding.UTF8.GetBytes(responsePayload), 200);
            }, null);

            // Send telemetry data every 20 seconds
            _ = Task.Run(async () =>
            {
                var rnd = new Random();
                while (true)
                {
                    var telemetry = new
                    {
                        temperature = 20 + rnd.NextDouble() * 10,
                        humidity = 50 + rnd.NextDouble() * 20
                    };

                    string json = JsonSerializer.Serialize(telemetry);
                    using var message = new Message(Encoding.UTF8.GetBytes(json));
                    message.ContentType = "application/json";
                    message.ContentEncoding = "utf-8";

                    await client.SendEventAsync(message);
                    Console.WriteLine($"[Telemetry] sent: {json}");

                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            });

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
            await client.CloseAsync();
        }
    }
}