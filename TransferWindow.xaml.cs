﻿using MahApps.Metro.Converters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
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
        SqlConnection con;
        SqlCommand cmd, cmd1, cmd2;
        int userId;
        string firstName;
        string lastName;
        public string loggedUserEmail { get; set; }
        public float currenBalance;

        float senderNewAmount;
        long senderCardNumber;
        int loggedUserId;



        public TransferWindow(string userEmail)
        {
            loggedUserEmail = userEmail;
            InitializeComponent();
            con = new SqlConnection("Data Source=DESKTOP-1AHTENP\\MSSQLSERVER01;Initial Catalog=BankManageSystemNewDB;Integrated Security=True");
        }

        private void emailButton_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            currenBalance = float.Parse(balance.Text.Trim().TrimEnd('$'));
            if (!amountTransfer.Text.Equals("") && float.TryParse(amountTransfer.Text, out float transferAmount) && transferAmount > 0)
            {
                if (transferAmount > currenBalance)
                {
                    MessageBox.Show("We are sorry, but you don't have enough amount on you account for this transfer, try again please!");
                }
                else
                {
                    InputDialog inputDialog = new InputDialog("Please enter the receiver Email Address:", "Email");
                    string input = "";
                    if (inputDialog.ShowDialog() == true)
                    {
                        input = inputDialog.Answer;
                        if (input != "")
                        {
                            if (input.Equals("Email"))
                            {
                                MessageBox.Show("Please enter a valide Email Address and try again!");
                            }
                            else
                            {
                                cmd = new SqlCommand("select count(1) from UserInfo where Email = @email COLLATE SQL_Latin1_General_CP1_CS_AS", con);
                                cmd.CommandType = System.Data.CommandType.Text;
                                cmd.Parameters.AddWithValue("@email", input);
                                int count = Convert.ToInt32(cmd.ExecuteScalar());
                                if (count == 1)
                                {
                                    cmd = new SqlCommand("select UserId, F_Name, L_Name from UserInfo where Email = @email COLLATE SQL_Latin1_General_CP1_CS_AS", con);
                                   
                                    cmd.Parameters.AddWithValue("@email", input);
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        userId = reader.GetInt32(0);
                                        firstName = reader.GetString(1);
                                        lastName = reader.GetString(2);
                                    }
                                    reader.Close();
                                    summary.Text = "You are Making a Tranfer of Amount: " + amountTransfer.Text + "\r\n" + "To: " + firstName + " " + lastName +
                                                   "\r\nConfirm or Cancel?";
                                    con.Close();

                                }
                                else
                                {
                                    MessageBox.Show("No user found! Check the email entered and try again please");
                                    con.Close();
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Please enter a valide Email Address and try again!");
                            con.Close();
                        }
                    }
                    else
                    {
                        con.Close();
                    }
                }
               

            }
            else
            {
                MessageBox.Show("Enter a valide amount first please!");
                con.Close();
            }

        }



        private void accountNumberButton_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            currenBalance = float.Parse(balance.Text.Trim().TrimEnd('$'));
            if (!amountTransfer.Text.Equals("") && float.TryParse(amountTransfer.Text, out float transferAmount) && transferAmount > 0)
            {
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
                            cmd = new SqlCommand("select UserId from UserAccount where CardNumber = @cardnumber", con);
                            cmd.Parameters.AddWithValue("@cardnumber", input);
                            var checkID = cmd.ExecuteScalar();
                            if (checkID != null)
                            {
                                try
                                {
                                    userId = (int)cmd.ExecuteScalar();
                                    if (userId != 0)
                                    {
                                        cmd = new SqlCommand("select F_Name, L_Name from UserInfo where UserId = @userid", con);
                                    
                                        cmd.Parameters.AddWithValue("@userid", userId);
                                        SqlDataReader reader = cmd.ExecuteReader();
                                        while (reader.Read())
                                        {
                                            firstName = reader.GetString(0);
                                            lastName = reader.GetString(1);
                                        }

                                        reader.Close();
                                        summary.Text = "You are Making a Tranfer of Amount: " + amountTransfer.Text + "\r\n" + "To: " + firstName + " " + lastName +
                                                       "\r\nConfirm or Cancel?";
                                        con.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("No Account found! Check the Account Nuber entered and try again please");
                                        con.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Please enter a valide EXCEPTION Account Number and try again!");
                                    con.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show("No Account found! Check the Account Nuber entered and try again please");
                                con.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valide Account Number and try again!");
                            con.Close();
                        }
                    }
                    else
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter a valide amount first please!");
                con.Close();
            }

        }
        private async void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            if(userId == 0)
            {
                MessageBox.Show("Click on \"Email Address\" or \"Account Number\" to enter receiver infomation please!");
                con.Close();
            }
            else
            {
                currenBalance = float.Parse(balance.Text.Trim().TrimEnd('$'));
                if (float.TryParse(amountTransfer.Text, out float transferAmount) && amountTransfer.Text != "")
                {
                    //update receivers balance to database
                    cmd2 = new SqlCommand("Select Balance from UserAccount where UserId = @ID", con);
                    cmd2.Parameters.AddWithValue("@ID", userId);
                    double receiverCurrentBalance = (double)cmd2.ExecuteScalar() + transferAmount;
                    cmd = new SqlCommand("UPDATE UserAccount SET Balance = @balance WHERE UserId = @ID", con);
                    cmd.Parameters.AddWithValue("@balance", receiverCurrentBalance);
                    cmd.Parameters.AddWithValue("@ID", userId);
                    await cmd.ExecuteNonQueryAsync();

                    //update senders balance to database
                    cmd = new SqlCommand("Select UserId from UserInfo where Email = @email COLLATE SQL_Latin1_General_CP1_CS_AS", con);
                    cmd.Parameters.AddWithValue("@email", loggedUserEmail);
                    loggedUserId = (int)cmd.ExecuteScalar();
                    senderNewAmount = currenBalance - transferAmount;
                    cmd1 = new SqlCommand("UPDATE UserAccount SET Balance = @newBalance WHERE UserId = @ID", con);
                    cmd1.Parameters.AddWithValue("@newBalance", senderNewAmount);
                    cmd1.Parameters.AddWithValue("@ID", loggedUserId);
                    await cmd1.ExecuteNonQueryAsync();
                    MessageBox.Show("Transfer " + transferAmount.ToString() + "$ to: " + firstName + " " + lastName + " Successful!");
                    balance.Text = senderNewAmount.ToString() + "$";



                    //save this transaction into dataase, for both sneder and receiver

                    //Get sender's card number
                    cmd = new SqlCommand("select CardNumber from UserAccount where UserId IN (@senderId, @receiverId)", con);
                    cmd.Parameters.AddWithValue("@senderId", loggedUserId);
                    cmd.Parameters.AddWithValue("@receiverId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    senderCardNumber = 0;
                    long receiverCardNumber = 0;
                    while (reader.Read())
                    {
                        string cardNumber = reader.GetValue(0).ToString();
                        if (senderCardNumber == 0)
                        {
                            senderCardNumber = long.Parse(cardNumber);
                        }
                        else
                        {
                            receiverCardNumber = long.Parse(cardNumber);
                        }

                    }
                    reader.Close();
                    //Insert record into sender database
                    cmd = new SqlCommand("insert into UserTransaction values (@userid, @CardNumber, @TransactionType, @TransactionTime, @TransactionAmount)", con);
                    cmd.Parameters.AddWithValue("@userid", loggedUserId);
                    cmd.Parameters.AddWithValue("@CardNumber", senderCardNumber);
                    cmd.Parameters.AddWithValue("@TransactionType", "Transfer OUT");
                    DateTime currrentDateTime = DateTime.Now;
                    cmd.Parameters.AddWithValue("@TransactionTime", currrentDateTime);
                    cmd.Parameters.AddWithValue("@TransactionAmount", transferAmount);
                    await cmd.ExecuteNonQueryAsync();

                    //Insert record into receiver database
                    cmd = new SqlCommand("insert into UserTransaction values (@userid, @CardNumber, @TransactionType, @TransactionTime, @TransactionAmount)", con);
                    cmd.Parameters.AddWithValue("@userid", userId);
                    cmd.Parameters.AddWithValue("@CardNumber", receiverCardNumber);
                    cmd.Parameters.AddWithValue("@TransactionType", "Transfer IN");
                    cmd.Parameters.AddWithValue("@TransactionTime", currrentDateTime);
                    cmd.Parameters.AddWithValue("@TransactionAmount", transferAmount);
                    await cmd.ExecuteNonQueryAsync();



                    MessageBoxResult result = MessageBox.Show("Do you want to make another transfer?", "Another Transfer?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Update the balance displaying in this window, and clear the transfer amount
                        balance.Text = senderNewAmount.ToString() + "$";
                        amountTransfer.Text = "";
                        userId = 0;

                    }
                    else
                    {
                        // Close this window, show the myAccount window and update the balance to the textblock named amount
                        MyAccount myAccount = new MyAccount();
                        myAccount.amount.Text = senderNewAmount.ToString() + "$";
                        myAccount.useremail.Text = loggedUserEmail;
                        myAccount.usercardnumber.Text = senderCardNumber.ToString();
                        cmd = new SqlCommand("select F_Name, L_Name, Date_Of_Birth, Mobile, Address_Country from UserInfo where UserId = @id ", con);
                        cmd.Parameters.AddWithValue("@id", loggedUserId);
                        reader= await cmd.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            myAccount.username.Text = reader.GetString(0) + " " + reader.GetString(1);
                            myAccount.userage.Text = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                            myAccount.userphonenumber.Text = reader.GetString(3);
                            myAccount.usercountry.Text = reader.GetString(4);
                        }
                        reader.Close();
                        myAccount.Show();
                        con.Close();
                        this.Close();
                        
                    }

                }
                else
                {
                    MessageBox.Show("Enter the amount you want transfer please!");
                    con.Close();
                }

            }
            con.Close();
        }

        private async void cancel_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            // Close this window, show the myAccount window and update the balance to the textblock named amount
            MyAccount myAccount = new MyAccount();

           
            cmd = new SqlCommand("select F_Name, L_Name, Date_Of_Birth, Mobile, Address_Country, UserId from UserInfo where Email = @email COLLATE SQL_Latin1_General_CP1_CS_AS ", con);
            cmd.Parameters.AddWithValue("@email", loggedUserEmail);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                myAccount.username.Text = reader.GetString(0) + " " + reader.GetString(1);
                myAccount.userage.Text = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                myAccount.userphonenumber.Text = reader.GetString(3);
                myAccount.usercountry.Text = reader.GetString(4);
                loggedUserId = reader.GetInt32(5);
            }
            reader.Close();
            cmd = new SqlCommand("select CardNumber, Balance from UserAccount where UserId = @id", con);
            cmd.Parameters.AddWithValue("@id", loggedUserId);
            reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                myAccount.usercardnumber.Text = reader.GetInt64(0).ToString();
                myAccount.amount.Text = reader.GetDouble(1).ToString() + "$";

            }
            reader.Close();
            myAccount.useremail.Text = loggedUserEmail;
            myAccount.Show();
            this.Close();
            con.Close() ;
        }
    }
}
