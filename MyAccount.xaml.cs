
using BankManageSystem.Models;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
    /// Interaction logic for MyAccount.xaml
    /// </summary>
    /// 
    public partial class MyAccount : Window
    {
        private int userId;

        HttpClient client;

        public MyAccount(int userIdPass)
        {
            InitializeComponent();
            this.userId = userIdPass;

        }
        //Go to transfer window to make transfer
        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            //Pass current logged user email to transfer window
            TransferWindow transferWindow = new TransferWindow(useremail.Text);
            transferWindow.balance.Text = amount.Text;
            transferWindow.Show();
            this.Close();
        }
        //Back to Main window
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.email.Text = useremail.Text;
            mw.Show();
            this.Close();
        }
        //Deposit function
        private async void deposit_Click(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog = new InputDialog("Please enter the deposit amount:", "Amount");
            string input = "";
            if (inputDialog.ShowDialog() == true)
            {
                input = inputDialog.Answer;
            }

            // Check if the user clicked OK or Cancel
            if (input != "")
            {
                // If the user clicked OK, attempt to parse the input value as a float
                if (float.TryParse(input, out float depositAmount) && depositAmount > 0)
                {
                    string cardNum = usercardnumber.Text;
                    //Also pass user info through json body for data safety
                    var data = new { amountAdding = input, userCardNum = cardNum };
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    client = new HttpClient();
                    HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7026/api/UserInfo/deposit", content);

                    //If response statues code is success, convert to object we need
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        string response = await responseMessage.Content.ReadAsStringAsync();
                        UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);
                        //Take out the object from the Json response message object
                        UserAccount account = userInfoResponse.accout;
                        string msg = userInfoResponse.StatusMessage;

                        amount.Text = account.balance.ToString() + "$";
                        MessageBox.Show(msg);
                    }
                    else
                    {
                        MessageBox.Show("Bad request!");
                    }
                }
                else
                {
                    // If the amount is not valid, display an error message
                    MessageBox.Show("Please enter a valid deposit amount and try again!");

                }
            }
        }



        //Withdrawa function
        private async void withdrawal_Click(object sender, RoutedEventArgs e)
        {
            InputDialog inputDialog = new InputDialog("Please enter the withdrawal amount:", "Amount");
            string input = "";
            if (inputDialog.ShowDialog() == true)
            {
                input = inputDialog.Answer;
            }
            // Check if the user clicked OK or Cancel
            if (input != "")
            {

                // If the user clicked OK, attempt to parse the input value as a float
                if (float.TryParse(input, out float withdrawalAmount) && withdrawalAmount > 0)
                {
                    string amountNow = amount.Text.Substring(0, amount.Text.Length - 1);
                    float currentAmount = float.Parse(amountNow);
                    //Check if there is sufficient amount for this withdrwal
                    if (currentAmount >= withdrawalAmount)
                    {
                        string cardNum = usercardnumber.Text;
                        var data = new { amountAdding = input, userCardNum = cardNum };
                        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                        client = new HttpClient();
                        HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7026/api/UserInfo/withdraw", content);

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            string response = await responseMessage.Content.ReadAsStringAsync();

                            UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);
                            //Take out the object from the Json response message object
                            UserAccount account = userInfoResponse.accout;
                            string msg = userInfoResponse.StatusMessage;

                            amount.Text = account.balance.ToString() + "$";
                            MessageBox.Show(msg);
                        }
                        else
                        {
                            MessageBox.Show("Bad request!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry you don't have sufficient amount on your account for this withdrawal!");
                      
                    }
                }
                else
                {
                    // If the amount is not valid, display an error message
                    MessageBox.Show("Please enter a valid withdrawal amount and try again!");
                   
                }
            }
        }

        private void list_Click(object sender, RoutedEventArgs e)
        {
            Transactions tw = new Transactions(userId);
            tw.Show();
            this.Close();
        }
    }
}


