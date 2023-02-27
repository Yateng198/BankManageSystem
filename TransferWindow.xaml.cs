using BankManageSystem.Models;
using ControlzEx.Standard;
using MahApps.Metro.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
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
    /// Interaction logic for TransferWindow.xaml
    /// </summary>
    /// 

    public partial class TransferWindow : Window
    {
        //Global variables
        public string loggedUserEmail { get; set; }

        public float currenBalance;
        double senderNewAmount;
        long senderCardNumber;
        int senderUserId;
        int receiverUserId;
        string firstName;
        string lastName;

        HttpClient client;



        public TransferWindow(string userEmail)
        {
            loggedUserEmail = userEmail;
            InitializeComponent();
        }

        private async void emailButton_Click(object sender, RoutedEventArgs e)
        {
            //Read user input the transfer amount first
            currenBalance = float.Parse(balance.Text.Trim().TrimEnd('$'));
            //Check if it is a valid input
            if (!amountTransfer.Text.Equals("") && float.TryParse(amountTransfer.Text, out float transferAmount) && transferAmount > 0)
            {
                //Check if there is sufficient current amount on sender account
                if (transferAmount > currenBalance)
                {
                    MessageBox.Show("We are sorry, but you don't have sufficient amount on you account for this transfer, try again please!");
                }
                else
                {
                    //Ask user to enter receiver email address
                    InputDialog inputDialog = new InputDialog("Please enter the receiver Email Address:", "Email");
                    //string input = "";
                    if (inputDialog.ShowDialog() == true)
                    {
                        string input = inputDialog.Answer;
                        //Check if receiver email address entered is valid or not
                        if (input != "")
                        {
                            if (input.Equals("Email"))
                            {
                                MessageBox.Show("Please enter a valide Email Address and try again!");
                            }
                            else
                            {
                                //Create serialized json body to pass the value through http request(data safty)
                                var data = new { emailAddress = input, amountTran = amountTransfer.Text };
                                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                                client = new HttpClient();
                                HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7026/api/Transfer/eamil", content);
                                //Make sure success code received
                                if (responseMessage.IsSuccessStatusCode)
                                {
                                    string response = await responseMessage.Content.ReadAsStringAsync();
                                    UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);
                                    //Take out the object from the Json response message object
                                    UserInfo user = userInfoResponse.user;
                                    string msg = userInfoResponse.StatusMessage;
                                    int statusCode = userInfoResponse.StatusCode;

                                    if (statusCode == 200)
                                    {
                                        //set up neccesary data for confirm button event
                                        summary.Text = msg;
                                        receiverUserId = user.userId;
                                        firstName = user.firstName;
                                        lastName = user.lastName;
                                    }
                                    else if (statusCode == 100)
                                    {
                                        MessageBox.Show(msg);
                                    }
                                    //All else error messages
                                }
                                else
                                {
                                    MessageBox.Show("Bad request!");
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Please enter a valide Email Address and try again!");

                        }
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                MessageBox.Show("Enter a valide amount first please!");

            }

        }



        private async void accountNumberButton_Click(object sender, RoutedEventArgs e)
        {
            currenBalance = float.Parse(balance.Text.Trim().TrimEnd('$'));
            if (!amountTransfer.Text.Equals("") && float.TryParse(amountTransfer.Text, out float transferAmount) && transferAmount > 0)
            {
                //Check if there is sufficient current amount on sender account
                if (transferAmount > currenBalance)
                {
                    MessageBox.Show("We are sorry, but you don't have enough amount on you account for this transfer, try again please!");
                }
                else
                {
                    InputDialog inputDialog = new InputDialog("Please enter the receiver Account Number:", "Account Number");
                    string input = "";
                    if (inputDialog.ShowDialog() == true)
                    {
                        input = inputDialog.Answer;
                        if (input != "" && long.TryParse(input, out long accNum))
                        {
                            //Create serialized json body to pass the value through http request(data safty)
                            var data = new { cardNumber = input, amountTran = amountTransfer.Text };
                            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                            client = new HttpClient();
                            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7026/api/Transfer/card", content);
                            //Make sure success code received
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                string response = await responseMessage.Content.ReadAsStringAsync();
                                UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);
                                //Take out the objects from the Json response message object
                                UserInfo user = userInfoResponse.user;
                                string msg = userInfoResponse.StatusMessage;
                                int statusCode = userInfoResponse.StatusCode;

                                if (statusCode == 200)
                                {
                                    //set up neccesary data for confirm button event
                                    summary.Text = msg;
                                    receiverUserId = user.userId;
                                    firstName = user.firstName;
                                    lastName = user.lastName;
                                }
                                else if (statusCode == 100)
                                {
                                    MessageBox.Show(msg);
                                }
                                //All else error messages
                            }
                            else
                            {
                                MessageBox.Show("Bad request!");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Please enter a valide Account Number and try again!");
                        }
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter a valide amount first please!");
            }

        }



        private async void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (receiverUserId == 0)
            {
                MessageBox.Show("Click on \"Email Address\" or \"Account Number\" to enter receiver infomation please!");
            }
            else
            {
                currenBalance = float.Parse(balance.Text.Trim().TrimEnd('$'));
                if (float.TryParse(amountTransfer.Text, out float transferAmount) && amountTransfer.Text != "")
                {
                    //Create serialized json body to pass the value through http request(data safty)
                    var data = new
                    {
                        senderCurrentBalance = currenBalance.ToString(),
                        recipientUserId = receiverUserId.ToString(),
                        amountTrans = transferAmount.ToString(),
                        senderEmail = loggedUserEmail

                    };
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    client = new HttpClient();
                    HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7026/api/Transfer/confirm", content);
                    //Make sure success code received
                    responseMessage.EnsureSuccessStatusCode();
                    //Read response as a string
                    string response = await responseMessage.Content.ReadAsStringAsync();
                    //Deserialize the response to be UserInfoResponse object
                    UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);
                    //Take out the objects from the Json response message object
                    UserInfo senderUserInfo = userInfoResponse.user;
                    UserAccount SenderAccount = userInfoResponse.accout;
                    //Setup neccesary arguments from reponse objects
                    senderNewAmount = SenderAccount.balance;
                    senderCardNumber = SenderAccount.cardNumber;

                    string senderFName = senderUserInfo.firstName;
                    string senderLName = senderUserInfo.lastName;
                    string senderDOB = senderUserInfo.DOB.ToString("yyyy-MM-dd");
                    string senderMobile = senderUserInfo.phoneNumber;
                    string senderCountry = senderUserInfo.addressCountry;

                    //Updated current balance in window and success information popout
                    balance.Text = senderNewAmount.ToString() + "$";
                    MessageBox.Show("Transfer " + transferAmount.ToString() + "$ to: " + firstName + " " + lastName + " Successful!");

                    //Prompt user to make another transfer or not
                    MessageBoxResult result = MessageBox.Show("Do you want to make another transfer?", "Another Transfer?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Update the balance displaying in this window, and clear the transfer amount
                        balance.Text = senderNewAmount.ToString() + "$";
                        amountTransfer.Text = "";
                        summary.Text = "";
                        receiverUserId = 0;

                    }
                    else
                    {
                        //If user click No, go back to myAccount window
                        int userIdPass = senderUserInfo.userId;
                        MyAccount myAccount = new MyAccount(userIdPass);
                        myAccount.amount.Text = senderNewAmount.ToString() + "$";
                        myAccount.useremail.Text = loggedUserEmail;
                        myAccount.usercardnumber.Text = senderCardNumber.ToString();
                        myAccount.username.Text = senderFName + " " + senderLName;
                        myAccount.userage.Text = senderDOB;
                        myAccount.userphonenumber.Text = senderMobile;
                        myAccount.usercountry.Text = senderCountry;

                        myAccount.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Enter the amount you want transfer please!");
                }

            }
        }

        private async void cancel_Click(object sender, RoutedEventArgs e)
        {
            // Close this window, show the myAccount window and update the balance to the textblock named amount
            
            //Create serialized json body to pass the value through http request(data safty)
            var data = new { email = loggedUserEmail };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            client = new HttpClient();
            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7026/api/Transfer/cancel", content);
            //Make sure success code received
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();
            UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);

            //Take out the objects from the Json response message object and setup logged in window information with updated logged in user infomation
            UserInfo user = userInfoResponse.user;
            UserAccount account = userInfoResponse.accout;

            int userIdPass = user.userId;
            MyAccount myAccount = new MyAccount(userIdPass);

            myAccount.useremail.Text = loggedUserEmail;
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
