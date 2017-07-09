using ChattingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public static IChattingService Server;
        private static DuplexChannelFactory<IChattingService> _channelFactory;
        private List<string> usersChatting;

        //public Window1()
        //{
         //   InitializeComponent();
         //   _channelFactory = new DuplexChannelFactory<IChattingService>(new ClientCallback(), "ChattingServiceEndPoint");
         //   Server = _channelFactory.CreateChannel();  
        //}

        public Window1(List<string> usersChatting)
        {
            InitializeComponent();
            _channelFactory = new DuplexChannelFactory<IChattingService>(new ClientCallback(), "ChattingServiceEndPoint");
            Server = _channelFactory.CreateChannel();

            this.usersChatting = usersChatting;
            //Server.GetCurrentUsers();
            WelcomeLabel.Content = "Private chat with " + usersChatting[1] + " !";
        }

        public void TakeMessage(string message, string userName)
        {
            string message2 = StringCipher.Decrypt(message, "1q2w3e4r");
            TextDisplayTextBox.Text += userName + ": " + message2 + "\n";
            TextDisplayTextBox.ScrollToEnd();
        }



        private void SendButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageTextBox.Text.Length == 0)
            {
                return;
            }

            string message = StringCipher.Encrypt(MessageTextBox.Text, "1q2w3e4r");
            string userName = usersChatting[0];
            string theiruserName = usersChatting[1];
            Server.SendMessageToPrivate(message, userName, theiruserName, false);
            TakeMessage(message, "You");
            MessageTextBox.Text = "";
        }
    }
}
