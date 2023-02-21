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
    /// Interaction logic for TransferWindow.xaml
    /// </summary>
    public partial class TransferWindow : Window
    {
        SqlConnection con;
        SqlCommand cmd, cmd1, cmd2;
        int userId;
        string firstName;
        string lastName;

       

        public TransferWindow()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=DESKTOP-1AHTENP\\MSSQLSERVER01;Initial Catalog=BankManageSystemNewDB;Integrated Security=True");
            con.Open();
        }

        

        private void emailButton_Click(object sender, RoutedEventArgs e)
        {
            if (!amountTranfer.Text.Equals("") && float.TryParse(amountTranfer.Text, out float transferAmount) && transferAmount > 0)
            {
                // string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the receiver Email Address:", "Email", "Email Address");
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
                                //cmd.CommandType = System.Data.CommandType.Text;
                                cmd.Parameters.AddWithValue("@email", input);
                                SqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {
                                    userId = reader.GetInt32(0);
                                    firstName = reader.GetString(1);
                                    lastName = reader.GetString(2);
                                }
                                reader.Close();
                                summary.Text = "You are Making a Tranfer of Amount: " + amountTranfer.Text + "\r\n" + "To: " + firstName + " " + lastName +
                                               "\r\nConfirm or Cancel?";

                            }
                            else
                            {
                                MessageBox.Show("No user found! Check the email entered and try again please");
                            }
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valide Email Address and try again!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter a valide amount first please!");
            }

        }

        private void accountNumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (!amountTranfer.Text.Equals("") && float.TryParse(amountTranfer.Text, out float transferAmount) && transferAmount > 0)
            {
                // string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the receiver Email Address:", "Email", "Email Address");
                InputDialog inputDialog = new InputDialog("Please enter the receiver Account Number:", "Account Number");
                string input = "";
                if (inputDialog.ShowDialog() == true)
                {
                    input = inputDialog.Answer;
                    if (input != "" && long.TryParse(input, out long accNum))
                    {
                        cmd = new SqlCommand("select UserId from UserAccount where CardNumber = @cardnumber", con);
                       // cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@cardnumber", input);
                        var checkID = cmd.ExecuteScalar();
                        if(checkID != null)
                        {
                            try
                            {
                                //userId = Convert.ToInt32(cmd.ExecuteScalar());
                                userId = (int)cmd.ExecuteScalar();
                                if (userId != 0)
                                {
                                    cmd = new SqlCommand("select F_Name, L_Name from UserInfo where UserId = @userid", con);
                                    //cmd.CommandType = System.Data.CommandType.Text;
                                    cmd.Parameters.AddWithValue("@userid", userId);
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        firstName = reader.GetString(0);
                                        lastName = reader.GetString(1);
                                    }

                                    reader.Close();
                                    summary.Text = "You are Making a Tranfer of Amount: " + amountTranfer.Text + "\r\n" + "To: " + firstName + " " + lastName +
                                                   "\r\nConfirm or Cancel?";

                                }
                                else
                                {
                                    MessageBox.Show("No Account found! Check the Account Nuber entered and try again please");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Please enter a valide EXCEPTION Account Number and try again!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Account found! Check the Account Nuber entered and try again please");
                        }
                        
                        
                        
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valide Account Number and try again!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter a valide amount first please!");
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MyAccount account = new MyAccount();
            account.Show();
            this.Close();
            con.Close();
        }
    }
}
