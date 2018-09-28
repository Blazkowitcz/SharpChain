using Newtonsoft.Json;
using SharpChain.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpChain
{
    class Program
    {
        public static Blockchain myBlockchain = new Blockchain();
        public static Server server;
        public static Client client = new Client();
        static void Main(string[] args)
        {
            myBlockchain.InitializeChain();
            myBlockchain.AddGenesisBlock();
            string type = args[0];
            if(type == "-server")
            {
                server = new Server();
                server.Start(12000);
            }
            else if(type == "-client")
            {
                client.Connect("");
            }

            Console.WriteLine("=========================");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Add a transaction");
            Console.WriteLine("3. Display Blockchain");
            Console.WriteLine("4. Exit");
            Console.WriteLine("=========================");
            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 2:
                        Console.WriteLine("Please enter the receiver name");
                        string receiverName = Console.ReadLine();
                        Console.WriteLine("Please enter the amount");
                        string amount = Console.ReadLine();
                        myBlockchain.CreateTransaction(new Transaction("Jacky", "Marley", 10));
                        myBlockchain.ProcessPendingTransaction("Bill");
                        client.Broadcast(JsonConvert.SerializeObject(myBlockchain));
                        break;
                    case 3:
                        Console.WriteLine("Blockchain");
                        Console.WriteLine(JsonConvert.SerializeObject(myBlockchain, Formatting.Indented));
                        break;
                }
                Console.WriteLine("Please select an action");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }
        }
    }
}
