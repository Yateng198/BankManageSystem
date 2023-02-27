using BankManageSystem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
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

namespace BankManageSystem
{
    /// <summary>
    /// Interaction logic for Transactions.xaml
    /// </summary>
    public partial class Transactions : Window
    {
        SqlConnection con;
        //Global variable

        string firstName;
        string lastName;
        public string loggedUserEmail { get; set; }
        public float currenBalance;

        private int userId;

        private static HttpClient client;

        public Transactions(int userIdPass) //Constructor
        {
            InitializeComponent();
            this.userId = userIdPass;
            //con = new SqlConnection("Data Source=LAPTOP-DT6BMRBG;Initial Catalog=final;Integrated Security=True");
        }

        private void thisMonth_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            thisMonth_record(userId);
        }

        private void lastMonth_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            lastMonth_record(userId);
        }

        private void in3Month_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            in3Month_record(userId);
        }

        private void thisYear_Click(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            thisYear_record(userId);
        }

        private async void thisMonth_record(int userIdPass)
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Transaction/GetTransactionThisMonth?userId=" + userId);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();
                UserTransactionResponse jsonObj = JsonConvert.DeserializeObject<UserTransactionResponse>(response);
                //Get the product list in the json response message object
                List<UserTransaction> listTransaction = jsonObj.listTransaction;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    //Fill up the datagrid with the product list information
                    record.ItemsSource = listTransaction;
                }));

            });
        }

        private async void lastMonth_record(int userIdPass)
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Transaction/GetTransactionLastMonth?userId=" + userId);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();
                UserTransactionResponse jsonObj = JsonConvert.DeserializeObject<UserTransactionResponse>(response);
                //Get the product list in the json response message object
                List<UserTransaction> listTransaction = jsonObj.listTransaction;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    //Fill up the datagrid with the product list information
                    record.ItemsSource = listTransaction;
                }));

            });
        }

        private async void in3Month_record(int userIdPass)
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Transaction/GetTransactionIn3Month?userId=" + userId);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();
                UserTransactionResponse jsonObj = JsonConvert.DeserializeObject<UserTransactionResponse>(response);
                //Get the product list in the json response message object
                List<UserTransaction> listTransaction = jsonObj.listTransaction;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    //Fill up the datagrid with the product list information
                    record.ItemsSource = listTransaction;
                }));

            });
        }

        private async void thisYear_record(int userIdPass)
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Transaction/GetTransactionThisYear?userId=" + userId);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();
                UserTransactionResponse jsonObj = JsonConvert.DeserializeObject<UserTransactionResponse>(response);
                //Get the product list in the json response message object
                List<UserTransaction> listTransaction = jsonObj.listTransaction;
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    //Fill up the datagrid with the product list information
                    record.ItemsSource = listTransaction;
                }));

            });
        }

        private async void goBack_Click(object sender, RoutedEventArgs e)
        {
            //con.Open();
            // Close this window, show the myAccount window and update the balance to the textblock named amount
            MyAccount myAccount = new MyAccount(userId);
            //Create serialized json body to pass the value through http request(data safty)
            var data = new { email = loggedUserEmail };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            client = new HttpClient();
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Transaction/GoBack?userId="+userId);
            //Make sure success code received
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();
            UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);

            //Take out the objects from the Json response message object
            UserInfo user = userInfoResponse.user;
            UserAccount account = userInfoResponse.accout;

            myAccount.useremail.Text = user.email;
            myAccount.username.Text = user.firstName + " " + user.lastName;
            myAccount.userage.Text = user.DOB.ToString("yyyy-MM-dd");
            myAccount.userphonenumber.Text = user.phoneNumber;
            myAccount.usercountry.Text = user.addressCountry;
            myAccount.usercardnumber.Text = account.cardNumber.ToString();
            myAccount.amount.Text = account.balance.ToString() + "$";
            myAccount.Show();
            this.Close();
        }
    }
}
