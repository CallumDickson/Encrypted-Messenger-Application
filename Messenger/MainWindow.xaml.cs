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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger
{
    public partial class MainWindow : Window
    {
        // server variables declared
        public static IChattingService Server;
        private static DuplexChannelFactory<IChattingService> _channelFactory;

        // this is run when the messenger window is opened
        public MainWindow()
        {
            InitializeComponent();
            // channel is setup to allow communication to the server
            _channelFactory = new DuplexChannelFactory<IChattingService>(new ClientCallback(), "ChattingServiceEndPoint");
            Server = _channelFactory.CreateChannel();

            // When the window is initialised all of the textbox/buttons that cannot be used yet are unenabled
            TextDisplayTextBox.IsEnabled = false;
            MessageTextBox.IsEnabled = false;
            SendButton.IsEnabled = false;
            checkBox.IsEnabled = false;
            UsersListBox.IsEnabled = false;

        }

        // This function takes a message and the username who posted it and prints the message to the text display box
        public void TakeMessage(string message, string userName, bool important)
        {
            // Decrypts the sent in message
            string message2 = StringCipher.Decrypt(message, "1q2w3e4r");

            // Checks if the important check box has been ticked, if it has then a sound is played
            if (important == true)
            {
                System.Media.SystemSounds.Beep.Play();
            }

            // sets the variable for the current user to whoever is logged in on the current window
            string currentUser = (string)UsernameTextBox.Text;

            // check done to see if the username passed in was from a private message
            if (userName.Contains("(PRIVATE)"))
            {
                // changes the message variable to include who send the message, to do and the date and time
                message = currentUser + " to " + userName + ": " + message + " at " + System.DateTime.Now;
                //saves the message to the file
                SaveToFile(message, currentUser, userName);
                // displays the message within the textbox
                TextDisplayTextBox.Text += userName + ": " + message2 + "\n";
                // scrolls to the end of the text box
                TextDisplayTextBox.ScrollToEnd();
            }
            // check done to ensure the message is to the group chat and not to the user themselfs
            else if (!userName.Contains("You"))
            {
                // changes the message variable to include date and time
                message = userName + ": " + message + " at " + System.DateTime.Now;
                SaveToFileGroup(message, currentUser);
                // displays the message within the textbox
                TextDisplayTextBox.Text += userName + ": " + message2 + "\n";
                // scrolls to the end of the text box
                TextDisplayTextBox.ScrollToEnd();
            }
            // this is only run for users to see their own messages
            else
            {
                // displays the message within the textbox
                TextDisplayTextBox.Text += userName + ": " + message2 + "\n";
                // scrolls to the end of the text box
                TextDisplayTextBox.ScrollToEnd();
            }
        }

        // this function saves the line about to be displayed on the window
        public void SaveToFileGroup(string printline, string userName)
        {

            // the path on a computer where the file could be found
            string subPath = @"c:\savedConversations";
            // check if the directory exists
            bool exists = System.IO.Directory.Exists(subPath);
            // if the directory does not exist
            if (!exists)
            {
                // directory created
                System.IO.Directory.CreateDirectory(subPath);
            }

            // creates a string for the file name
            string whosConversation = userName + " conversation";
            // creates the full file location, name and file type
            string filePath = @"c:\savedConversations\" + whosConversation + ".txt";
            // checks if the file already exists
            bool fileexists = File.Exists(filePath);
            // if the file does not exist, it is created
            if (!fileexists)
            {
                //file is created
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    // line is printed to file
                    sw.WriteLine(printline);
                }
            }
            // if the file does exist
            else
            {
                // the file is setup to write to
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    // the line is writen to the file
                    file.WriteLine(printline);
                }
            }

        }







        // this function saves the line about to be displayed on the window
        public void SaveToFile(string printline, string userName, string theiruserName)
        {
            // checks the theiruserName that is passed in
            theiruserName = theiruserName.Substring(10, theiruserName.Length - 10);
            // the path on a computer where the file could be found
            string subPath = @"c:\savedConversations";
            // check if the directory exists
            bool exists = System.IO.Directory.Exists(subPath);
            // if the directory does not exist
            if (!exists)
            {
                // directory created
                System.IO.Directory.CreateDirectory(subPath);
            }

            // creates a string for the file name
            string whosConversation = userName + " " + theiruserName + " conversation";
            // creates the full file location, name and file type
            string filePath = @"c:\savedConversations\" + whosConversation + ".txt";
            // checks if the file already exists
            bool fileexists = File.Exists(filePath);
            // if the file does not exist, it is created
            if (!fileexists)
            {
                //file is created
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    // line is printed to file
                    sw.WriteLine(printline);
                }
            }
            // if the file does exist
            else
            {
                // the file is setup to write to
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    // the line is writen to the file
                    file.WriteLine(printline);
                }
            }

        }




        // function that checks/reads previous group conversations into the display window
        private void fileExists()
        {

            // textbox is cleared of text
            TextDisplayTextBox.Text = "";
            // variable setup to state who the active user is
            string userName = (string)UsernameTextBox.Text;
            // variable setup to state what the file name is
            string whosConversation = userName + " conversation";
            string filePath = @"c:\savedConversations\" + whosConversation + ".txt";
            // check to see if the file exists
            bool fileexists = File.Exists(filePath);
            // if the file exists
            if (fileexists)
            {
                // set counted to 0
                int counter = 0;
                //create a string called line
                string line;

                //reads in the file path
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                // while loop to read through each line of the file until there are no more lines to read
                while ((line = file.ReadLine()) != null)
                {
                    //creates an array to hold each section of the line
                    string[] words = line.Split(' ');
                    // decrypts the 2nd part in the message
                    words [1] = StringCipher.Decrypt(words[1], "1q2w3e4r");
                    // for each loop to print the word from "words" 
                    foreach (string word in words)
                    {
                        // prints the word out to the test display box
                        TextDisplayTextBox.Text += word + " ";
                    }
                    // scrolls to the end of the text box
                    TextDisplayTextBox.ScrollToEnd();
                    // adds 1 onto the counter
                    counter++;
                    // displays a new line in the textbox
                    TextDisplayTextBox.Text += "\n";
                }
            }
        }




        // function that checks/reads previous private conversations into the display window
        private void fileExistsPrivate(string theirUserName)
        {
            // textbox is cleared of text
            TextDisplayTextBox.Text = "";
            // variable setup to state who the active user is
            string userName = (string)UsernameTextBox.Text;
            // variable setup to state what the file name is
            string whosConversation = userName + " " + theirUserName + " conversation";
            string filePath = @"c:\savedConversations\" + whosConversation + ".txt";
            // check to see if the file exists
            bool fileexists = File.Exists(filePath);
            // if the file exists
            if (fileexists)
            {
                int counter = 0;
                string line;

                //reads in the file path
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    //creates an array to hold each section of the line
                    string[] words = line.Split(' ');
                    if (words[2] == theirUserName + ":")
                    {
                        // decrypts the 4th part in the message
                        words[3] = StringCipher.Decrypt(words[3], "1q2w3e4r");
                    }
                    else
                    {
                        // decrypts the 5th part in the message
                        words[4] = StringCipher.Decrypt(words[4], "1q2w3e4r");
                    }
                    // prints all the words to the display box
                    foreach (string word in words)
                    {
                        TextDisplayTextBox.Text += word + " ";
                    }
                    // scrolls the textbox to the end
                    TextDisplayTextBox.ScrollToEnd();
                    counter++;
                    TextDisplayTextBox.Text += "\n";
                }
            }
        }



        // function setup to record the user selection different usernames from the online users list
        private void UsersListBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            // if the username selected equals the active user
            if ((string)UsersListBox.SelectedItem == (string)UsernameTextBox.Text)
            {
                // checks to see if there are any previous group conversations
                fileExists();
                WelcomeLabel.Content = "";
                // prints a message saying this is group chat
                WelcomeLabel.Content = "Send a group message";

            }
            // else the user has selected another online user
            else
            {
                string theirUserName = (string)UsersListBox.SelectedItem;
                // checks to see if the username selected has had any previous conversations with the active user
                fileExistsPrivate(theirUserName);
                WelcomeLabel.Content = "";
                // informs the user that they are sending private messages to a selected user
                WelcomeLabel.Content = "Send a private message to " + (string)UsersListBox.SelectedItem;

            }
        }

        // function setup to record the user select the send button or press enter
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // cancles the message if the message is blank
            if (MessageTextBox.Text.Length == 0)
            {
                return;
            }

            
            bool important = false;
            // checks to see if the important check box was check
            if (checkBox.IsChecked.Value)
            {
                important = true;
            }

            // if the user has not not selected a user form the online users
            if ((string)UsersListBox.SelectedItem != null)
            {
                // if the user has selected their own name
                if ((string)UsersListBox.SelectedItem == (string)UsernameTextBox.Text)
                {
                    // encrypt the message
                    string message = StringCipher.Encrypt(MessageTextBox.Text, "1q2w3e4r");
                    // send the message to everyone
                    Server.SendMessageToALL(message, UsernameTextBox.Text, important);
                    // display message in textbox
                    TakeMessage(message, "You", false);
                    // clear the input textbox
                    MessageTextBox.Text = "";
                }
                // if the user has selected anyone else but themselfs
                else
                {
                    
                    string friendName = (string)UsersListBox.SelectedItem;
                    List<string> usersChatting = new List<string>();
                    usersChatting.Add(UsernameTextBox.Text);
                    usersChatting.Add(friendName);
                    // encrypt the message
                    string message = StringCipher.Encrypt(MessageTextBox.Text, "1q2w3e4r");
                    string userName = usersChatting[0];
                    string theiruserName = usersChatting[1];
                    // send the message to everyone
                    Server.SendMessageToPrivate(message, userName, theiruserName, important);
                    // display message in textbox
                    TakeMessage(message, "You to " + theiruserName, false);
                    // clear the input textbox
                    MessageTextBox.Text = "";
                }

            }
            // else the user has selected no one
            else
            {
                //TextDisplayTextBox.Text = "";
                string message = StringCipher.Encrypt(MessageTextBox.Text, "1q2w3e4r");
                Server.SendMessageToALL(message, UsernameTextBox.Text, important);
                TakeMessage(message, "You", false);
                MessageTextBox.Text = "";
            }

        }

















        // function setup to record the user clicking the login button, allowing them to join the server
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // attempts to join the server
            int returnValue = Server.Login(UsernameTextBox.Text);
            // if the username is already in use display a message
            if (returnValue == 1)
            {
                MessageBox.Show("You are already logged in. Try again");
         
            }
            // login the user and enable all messagin features
            else if (returnValue == 0)
            {
                MessageBox.Show("You logged in!");
                WelcomeLabel.Content = "Welcome " + UsernameTextBox.Text + " !";
                //means you cant log in twice
                UsernameTextBox.IsEnabled = false;
                LoginButton.IsEnabled = false;
                TextDisplayTextBox.IsEnabled = true;
                MessageTextBox.IsEnabled = true;
                SendButton.IsEnabled = true;
                checkBox.IsEnabled = true;
                UsersListBox.IsEnabled = true;
                TextDisplayTextBox.Text = "Want to send a private message? Select a user from 'Who's Online' and enter your message as normal below. If you want to send a group message again, select your own username. \n \n";
                // checks if there has been a previous conversation and the user has used the service before
                fileExists();

                //load online users list
                LoadUserList(Server.GetCurrentUsers());
            }
        }

        // if the messenger is closed this is run
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // logout of server
            Server.Logout();
        }

        // adds username to online user list
        public void AddUserToList(string userName)
        {
            if (UsersListBox.Items.Contains(userName))
            {
                return;
            }
            UsersListBox.Items.Add(userName);
        }


        // removes username when user logs out
        public void RemoveUserFromList(string userName)
        {
            if (UsersListBox.Items.Contains(userName))
            {
                UsersListBox.Items.Remove(userName);
            }
        }

        // loads the online user list
        private void LoadUserList(List<string> users)
        {
            foreach (var user in users)
            {
                AddUserToList(user);
            }
        }

        // mistake but couldnt remove
        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        

    }


    // Encryption code found online by StackExchange user, please see the Portfolio Report for more details

    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }



}
