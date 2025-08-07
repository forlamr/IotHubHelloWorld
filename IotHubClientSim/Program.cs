using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace IotHubClientSim
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Client Sim");

            var iothubConnectionString = "<iothub-connection-string>";
            var deviceId = "Device01";

            var methodInvocation = new CloudToDeviceMethod("myMethod")
            {
                ResponseTimeout = TimeSpan.FromSeconds(30)
            };
            methodInvocation.SetPayloadJson("{\"messaggio\": \"hello device!\"}");

            var serviceClient = ServiceClient.CreateFromConnectionString(iothubConnectionString);

            try
            {
                Console.WriteLine($"Send direct method to {deviceId} ...");
                Thread.Sleep(5000);
                var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);
                Console.WriteLine($"Status: {response.Status}, Payload: {response.GetPayloadAsJson()}");
            }
            catch (DeviceNotFoundException)
            {
                Console.WriteLine("Device not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}