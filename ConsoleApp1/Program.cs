using System;
using WindowsService1;

namespace ConsoleApp1
{
    //Test server console app
    
    class Program
    {
        static void Main(string[] args)
        {
            Service1 service = new Service1();
            service.OnStartDebug();
            Console.ReadLine();
        }
    }
}
