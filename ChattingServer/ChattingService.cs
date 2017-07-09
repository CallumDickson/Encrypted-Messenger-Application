using ChattingInterfaces;
using ChattingServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace ChattingServer
{
    // client to server contract
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]

    public class ChattingService : IChattingService
    {

        // dictionary of all active users on the server
        ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        // operation to allow the user to login into the server
        public int Login(string userName)
        {

            foreach (var client in _connectedClients)
            {
                if(client.Key.ToLower() == userName.ToLower())
                {
                    return 1;
                }
            }

            var establishedUserConnection = OperationContext.Current.GetCallbackChannel <IClient>();

            ConnectedClient newClient = new ConnectedClient();
            newClient.connection = establishedUserConnection;
            newClient.UserName = userName;

            _connectedClients.TryAdd(userName, newClient);

            updateHelper(0, userName);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Client Login: {0} at {1}", newClient.UserName, System.DateTime.Now);
            Console.ResetColor();

            //if no return 0
            return 0;
        }

        // operation to allow the user to logout into the server
        public void Logout()
        {
            ConnectedClient client = getMyClient();
            if (client != null)
            {
                ConnectedClient removedClient;
                _connectedClients.TryRemove(client.UserName, out removedClient);

                updateHelper(1, removedClient.UserName);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Client logoff: {0} at {1}", removedClient.UserName, System.DateTime.Now);
                Console.ResetColor();
            }
        }


        // operation to get the active users key from the dictionary
        ConnectedClient getMyClient()
        {
            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            foreach(var client in _connectedClients)
            {
                if(client.Value.connection == establishedUserConnection)
                {
                    return client.Value;
                }
            }
            return null;
        }





        // operation to allow the user to send a message to all users
        public void SendMessageToALL(string message, string userName, bool important)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string printline = userName + ": " + message + " at " + System.DateTime.Now;
            Console.WriteLine(printline);
            Console.ResetColor();

            SaveToFileGroup(printline, userName);

            foreach (var client in _connectedClients)
            {  
                if (client.Key.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetMessage(message, userName, important);
                }
            }
        }

        // operation to save the message about to be sent to the conversation file
        public void SaveToFile(string printline, string userName, string theiruserName)
        {
            string subPath = @"c:\savedConversations";
            bool exists = System.IO.Directory.Exists(subPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(subPath);
            }

            string whosConversation = userName + " " + theiruserName + " conversation";
            string filePath = @"c:\savedConversations\" + whosConversation + ".txt";
            bool fileexists = File.Exists(filePath);
            if (!fileexists)
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(printline);
                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    file.WriteLine(printline);
                }
            }

        }





        // operation to check if all users have been loaded to the online user list
        private void updateHelper(int value, string userName)
        {
            foreach (var client in _connectedClients)
            {
                if(client.Value.UserName.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetUpdate(value, userName);
                }
            }
        }

        // operation to create a list of all current users on the online user listr
        public List<string> GetCurrentUsers()
        {
            List<string> listofUsers = new List<string>();
            foreach (var client in _connectedClients)
            {
                listofUsers.Add(client.Value.UserName);
            }

            return listofUsers;
        }


        // operation to save the group conversation to the conversationf ile
        public void SaveToFileGroup(string printline, string userName)
        {
            

            string subPath = @"c:\savedConversations";
            bool exists = System.IO.Directory.Exists(subPath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(subPath);
            }

            string whosConversation = userName + " conversation";
            string filePath = @"c:\savedConversations\" + whosConversation + ".txt";
            bool fileexists = File.Exists(filePath);
            if (!fileexists)
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(printline);
                }
            }
            else
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    file.WriteLine(printline);
                }
            }

        }




        // operation to allow the user to save private messages sent to the private message conversation file
        public void SendMessageToPrivate(string message, string userName, string theiruserName, bool important)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string printline = userName + " to " + theiruserName + ": " + message + " at " + System.DateTime.Now;
            Console.WriteLine(printline);
            Console.ResetColor();

            SaveToFile(printline, userName, theiruserName);

            foreach (var client in _connectedClients)
            {
                if (client.Value.UserName.ToLower() == theiruserName.ToLower())
                {
                    client.Value.connection.GetMessage2(message, userName, important);
                }
            }
        }



    }


}


