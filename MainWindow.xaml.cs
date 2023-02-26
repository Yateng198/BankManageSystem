using BankManageSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
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

namespace BankManageSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            Register rg = new Register();
            rg.Show();
            this.Close();
        }

        private async void login_Click(object sender, RoutedEventArgs e)
        {

            string password = txtpwd.Password.ToString();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes;
            
            using (SHA256 sha256 = SHA256.Create())
            {
                hashBytes = sha256.ComputeHash(passwordBytes);
            }
            string hashedPassword = Convert.ToBase64String(hashBytes);

            HttpClient client = new HttpClient();
            //Instead of exposing user information in the link, pass user information through an json body  for data safety
            var data = new { email = email.Text, password = hashedPassword };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7026/api/UserInfo/login", content);

            string response = await responseMessage.Content.ReadAsStringAsync();
            UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);
            //Take out the objects from the Json response message object
            UserInfo user = userInfoResponse.user;
            UserAccount account = userInfoResponse.accout;
            //Make sure success status code returned
            if (responseMessage.IsSuccessStatusCode)
            {
                if (account != null && user != null)
                {
                    //setup after login window components with loging in user infos
                    MyAccount myAccount = new MyAccount();
                    myAccount.useremail.Text = user.email;
                    myAccount.username.Text = user.firstName + " " + user.lastName;
                    myAccount.userage.Text = user.DOB.Date.ToString("d");
                    myAccount.usercardnumber.Text = account.cardNumber.ToString();
                    myAccount.usercountry.Text = user.addressCountry;
                    myAccount.userphonenumber.Text = user.phoneNumber;
                    myAccount.amount.Text = account.balance.ToString() + "$";
                    myAccount.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please Check Your Email & Password, and Try Again!");
                }
                
            }
            else
            {
                MessageBox.Show("Please Check Your Email & Password, and Try Again!");
            }

        }
        //Go to exchange window
        private void exchange_Click(object sender, RoutedEventArgs e)
        {
            Exchange ex = new Exchange();
            ex.Show();
            this.Close();
        }
        //Quit the system
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
