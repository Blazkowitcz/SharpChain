using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace SharpChain.Classes
{
    public class Server: WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer webSocketServer = null;

        public void Start(int port)
        {
            webSocketServer = new WebSocketServer($"ws://127.0.0.1:12000");
            webSocketServer.AddWebSocketService<Server>("/Blockchain");
            webSocketServer.Start();
            Console.WriteLine(this.GetHashCode());
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                Send("Hi Client");
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                if (newChain.IsValid() && newChain.Chain.Count > Program.myBlockchain.Chain.Count)
                {
                    List<Transaction> newTransaction = new List<Transaction>();
                    newTransaction.AddRange(newChain.PendingTransactions);
                    newTransaction.AddRange(Program.myBlockchain.PendingTransactions);
                    newChain.PendingTransactions = newTransaction;
                    Program.myBlockchain = newChain;
                }
                if (!chainSynched)
                {
                    Console.WriteLine(this.GetHashCode());
                    Send(JsonConvert.SerializeObject(Program.myBlockchain));
                    chainSynched = true;
                }
            }
        }
    }
}
