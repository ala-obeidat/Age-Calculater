using System;
using System.Linq;
using AgeCalculater.Service;
using Grpc.Net.Client;

Console.WriteLine("Tring to connect to gRPC server ....");
// The port number(5001) must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Ager.AgerClient(channel);
Console.WriteLine("Connected");
while (true)
{
    Console.WriteLine("**************************************************");
    Console.WriteLine("Enter your age in format: dd-MM-yyyy");
    var birthDateString = Console.ReadLine();
    if (birthDateString == "x")
        break;
    var birthDateInfo = birthDateString.Split('-').Select(c => Convert.ToInt32(c)).ToArray();
    var dateTime = new DateTime(birthDateInfo[2], birthDateInfo[1], birthDateInfo[0]);


    Console.WriteLine("Calculating your age ...");
    var reply = await client.CalculateAsync(
                      new AgeRequest { BithDate = dateTime.Ticks });
    Console.WriteLine("Response:");
    Console.WriteLine($"{reply.FullAge}");
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();