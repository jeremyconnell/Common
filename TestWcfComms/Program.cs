// See https://aka.ms/new-console-template for more information
using Framework;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using TestWcfComms;

var client = new CPushUpgradeClient_Wcf("localhost:4362");

try
{
    string s = client.RequestLogFile("asdf"); //error: wcf has no httpcontext
    Console.WriteLine(s);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
Console.ReadLine();
