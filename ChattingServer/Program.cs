using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingServer
{
    class Program
    {

        // the server
        public static ChattingService _server;

        static void Main(string[] args)
        {
            _server = new ChattingService();
            // start the server
            using (System.ServiceModel.ServiceHost host = new System.ServiceModel.ServiceHost(_server))
            {
                host.Open();
                Console.WriteLine("Server is running...");
                Console.ReadLine();

            }
        }
    }
}
