using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace SharpChain.Classes
{
    public class Client
    {
        IDictionary<String, WebSocket> webSocketDictionnary = new Dictionary<string, WebSocket>();
        public void Connect(string url)
        {
            if (!webSocketDictionnary.ContainsKey("ws://127.0.0.1:12000"))
            {
                url = "ws://127.0.0.1:12000/Blockchain";
                WebSocket webSocket = new WebSocket(url);
                webSocket.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hi Client")
                    {
                        Console.WriteLine(e.Data);
                    }
                    else
                    {
                        Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);
                        if (newChain.IsValid() && newChain.Chain.Count > Program.myBlockchain.Chain.Count)
                        {
                            List<Transaction> newTransactions = new List<Transaction>();
                            newTransactions.AddRange(newChain.PendingTransactions);
                            newTransactions.AddRange(Program.myBlockchain.PendingTransactions);

                            newChain.PendingTransactions = newTransactions;
                            Program.myBlockchain = newChain;
                        }
                    }
                };
                webSocket.Connect();
                webSocket.Send("Hi Server");
                webSocket.Send(JsonConvert.SerializeObject(Program.myBlockchain));
                webSocketDictionnary.Add(url, webSocket);

            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in webSocketDictionnary)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in webSocketDictionnary)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetSetver()
        {
            IList<string> servers = new List<string>();
            foreach (var item in webSocketDictionnary)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in webSocketDictionnary)
            {
                item.Value.Close();
            }
        }
    }
}
