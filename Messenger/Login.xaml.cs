using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        //Registration registration = new Registration();
        MainWindow welcome = new MainWindow();

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //if (textBoxEmail.Text.Length == 0)
            //{
            //    errormessage.Text = "Enter an email.";
            //    textBoxEmail.Focus();
            //}
            //else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            //{
            //    errormessage.Text = "Enter a valid email.";
            //    textBoxEmail.Select(0, textBoxEmail.Text.Length);
            //    textBoxEmail.Focus();
            //}
            //else
            //{
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                SqlConnection con = new SqlConnection("Data Source=127.0.0.1; Initial Catalog=id72140_practicals; User ID=id72140_callumspractical; Password=1234qwer");
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from customerLogins where cusername='" + email + "'  and cpassword='" + password + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    string username = dataSet.Tables[0].Rows[0]["FirstName"].ToString() + " " + dataSet.Tables[0].Rows[0]["LastName"].ToString();
                    //welcome.TextBlockName.Text = username;//Sending value from one form to another form.
                    welcome.Show();
                    Close();
                }
                else
                {
                    errormessage.Text = "Sorry! Please enter existing emailid/password.";
                }
                con.Close();
            //}
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            //registration.Show();
            Close();
        }

    }
}

