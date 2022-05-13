using System;
using System.Threading;
using System.Text;

namespace ServerFramework
{
    class Program
    {
        static ServerObject server; 
        static Thread listenThread; 
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); 
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }

        }
    }
}
