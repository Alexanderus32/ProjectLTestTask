using System;
using WindowsService1;

namespace ConsoleApp1
{
    //Test server console app
    
    class Program
    {
        static void Main(string[] args)
        {
            VolumeControlService service = new VolumeControlService();
            service.OnStartDebug();
            Console.ReadLine();
        }
    }
}
