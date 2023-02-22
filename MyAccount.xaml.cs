
using MahApps.Metro.Controls.Dialogs;
using System;
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
            con.Open();
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            TransferWindow transferWindow = new TransferWindow(useremail.Text);
            transferWindow.balance.Text = amount.Text;
          //  transferWindow.loggedUserEmail.Text = useremail.Text;
            transferWindow.Show();
            this.Close();
        }

        private void deposit_Click(object sender, RoutedEventArgs e)
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
                    string query = "UPDATE UserAccount SET Balance = Balance + @depositAmount WHERE CardNumber = @accountNumber";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@depositAmount", input);
                    cmd.Parameters.AddWithValue("@accountNumber", usercardnumber.Text);
                    cmd.ExecuteNonQuery();

                    // Retrieve the new balance value from the database
                    query = "SELECT Balance FROM UserAccount WHERE CardNumber = @accountNumber";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@accountNumber", usercardnumber.Text);
                    var newAmount = cmd.ExecuteScalar().ToString();
                    amount.Text= newAmount + " $";
                    MessageBox.Show($"You have deposited {depositAmount:C} Successfully!");
                }
                else
                {
                    // If the amount is not valid, display an error message
                    MessageBox.Show("Please enter a valid deposit amount and try again!");
                }
            }
        }

        private void withdrawal_Click(object sender, RoutedEventArgs e)
        {
            // string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the withdrawal amount:", "Deposit", "Enter your withdrawal amount please");

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
                        cmd.ExecuteNonQuery();

                        // Retrieve the new balance value from the database
                        string balanceQuery = "SELECT Balance FROM UserAccount WHERE CardNumber = @accountNumber";
                        cmd = new SqlCommand(balanceQuery, con);
                        cmd.Parameters.AddWithValue("@accountNumber", usercardnumber.Text);
                        var newAmount = cmd.ExecuteScalar().ToString();
                        amount.Text = newAmount + " $";
                        MessageBox.Show($"You have windrawaled {withdrawalAmount:C} Successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Sorry you don't have enough amount on your account for this withdrawal!");
                    }
                }
                else
                {
                    // If the amount is not valid, display an error message
                    MessageBox.Show("Please enter a valid withdrawal amount and try again!");
                }
            }
        }




    }

}


