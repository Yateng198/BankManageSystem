
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
        SqlConnection con;
        SqlCommand cmd, cmd1, cmd2;

        public MyAccount()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=DESKTOP-1AHTENP\\MSSQLSERVER01;Initial Catalog=BankManageSystemNewDB;Integrated Security=True");
            
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            TransferWindow transferWindow = new TransferWindow(useremail.Text);
            transferWindow.balance.Text = amount.Text;
            transferWindow.Show();
            this.Close();
        }

        private async void deposit_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
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
                    string query = "UPDATE UserAccount SET Balance = Balance + @depositAmount WHERE CardNumber = @accountNumber";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@depositAmount", input);
                    cmd.Parameters.AddWithValue("@accountNumber", usercardnumber.Text);
                    cmd.ExecuteNonQuery();

                    // Retrieve the new balance value from the database
                    query = "SELECT UserId, Balance FROM UserAccount WHERE CardNumber = @accountNumber";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@accountNumber", usercardnumber.Text);
                    SqlDataReader reader =await cmd.ExecuteReaderAsync();
                    int loggedUserId = 0;
                    double newAmount = 0;
                    while (reader.Read())
                    {
                        loggedUserId = reader.GetInt32(0);
                        newAmount = reader.GetDouble(1);
                    }
                    reader.Close();
                    amount.Text= newAmount.ToString() + "$";
                    MessageBox.Show($"You have deposited {depositAmount:C} Successfully!");

                    //Update deposit record into database
                    cmd = new SqlCommand("Insert into UserTransaction values (@userid, @cardNum, @type, @time, @amount)", con);
                    DateTime currentDateTime = DateTime.Now;
                    cmd.Parameters.AddWithValue("@userid", loggedUserId.ToString());
                    cmd.Parameters.AddWithValue("@cardNum", usercardnumber.Text);
                    cmd.Parameters.AddWithValue("@type", "Deposit");
                    cmd.Parameters.AddWithValue("@time", currentDateTime);
                    cmd.Parameters.AddWithValue("@amount", depositAmount);
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                }
                else
                {
                    // If the amount is not valid, display an error message
                    MessageBox.Show("Please enter a valid deposit amount and try again!");
                    con.Close();
                }
            }
            con.Close();
        }

        private async void withdrawal_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
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

                    float currentAmount = 0;
                    string amountNow = amount.Text.Substring(0, amount.Text.Length - 2);
                    try
                    {
                        currentAmount = float.Parse(amountNow);
                    }catch (FormatException)
                    {
                        MessageBox.Show("Not working!");
                    }
                    if(currentAmount >= withdrawalAmount)
                    {
                        string updateQuery = "UPDATE UserAccount SET Balance = Balance - @windrawalAmount WHERE CardNumber = @accountNumber";
                        cmd = new SqlCommand(updateQuery, con);
                        cmd.Parameters.AddWithValue("@windrawalAmount", input);
                        cmd.Parameters.AddWithValue("@accountNumber", usercardnumber.Text);
                        await cmd.ExecuteNonQueryAsync();

                     //   Retrieve the new balance value from the database
                        string query = "SELECT UserId, Balance FROM UserAccount WHERE CardNumber = @accountNumber";
                        cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@accountNumber", usercardnumber.Text);
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        int loggedUserId = 0;
                        double newAmount = 0;
                        while (reader.Read())
                        {
                            loggedUserId = reader.GetInt32(0);
                            newAmount = reader.GetDouble(1);
                        }
                        reader.Close();
                        amount.Text = newAmount.ToString() + "$";
                        MessageBox.Show($"You have deposited {withdrawalAmount:C} Successfully!");

                        //Update deposit record into database
                        cmd = new SqlCommand("Insert into UserTransaction values (@userid, @cardNum, @type, @time, @amount)", con);
                        DateTime currentDateTime = DateTime.Now;
                        cmd.Parameters.AddWithValue("@userid", loggedUserId.ToString());
                        cmd.Parameters.AddWithValue("@cardNum", usercardnumber.Text);
                        cmd.Parameters.AddWithValue("@type", "Withdrawal");
                        cmd.Parameters.AddWithValue("@time", currentDateTime);
                        cmd.Parameters.AddWithValue("@amount", withdrawalAmount);
                        await cmd.ExecuteNonQueryAsync();
                        con.Close();


                    }
                    else
                    {
                        MessageBox.Show("Sorry you don't have enough amount on your account for this withdrawal!");
                        con.Close();
                    }
                }
                else
                {
                    // If the amount is not valid, display an error message
                    MessageBox.Show("Please enter a valid withdrawal amount and try again!");
                    con.Close();
                }
            }
            con.Close();
        }




    }

}


