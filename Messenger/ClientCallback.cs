using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]

    // server to client contract

    public class ClientCallback : IClient
    {
        // operation to get the message being sent from the server
        public void GetMessage(string message, string userName, bool important)
        {

            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, userName, important);
        }

        // operation for the client to recieve the private message being sent over the server
        public void GetMessage2(string message, string userName, bool important)
        {

            userName = "(PRIVATE) " + userName;
            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, userName, important);
            
        }

        // operation run to send new users to the server to client windows
        public void GetUpdate(int value, string userName)
        {
            switch(value)
                {
                    case 0:
                        {
                            ((MainWindow)Application.Current.MainWindow).AddUserToList(userName);
                            break;
                        }
                    case 1:
                        {
                            ((MainWindow)Application.Current.MainWindow).RemoveUserFromList(userName);
                            break;
                        }
                }
        }

        // mistake, couldnt remove
        public void Placeholder()
        {
            throw new NotImplementedException();
        }
    }

}
