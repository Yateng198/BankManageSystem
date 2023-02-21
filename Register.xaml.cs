using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        SqlConnection con;
        DateTime MinimumDate = new DateTime(1900, 1, 1);
        DateTime MaximumDate = DateTime.Today.AddYears(-18);


        public Register()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=DESKTOP-1AHTENP\\MSSQLSERVER01;Initial Catalog=BankManageSystemNewDB;Integrated Security=True");
            con.Open();

            dobPicker.DisplayDateStart = MinimumDate;
            dobPicker.DisplayDateEnd = MaximumDate;


        }

        private void registe_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {

                string newEmail = email.Text;
                string selectQuery = "SELECT COUNT(*) FROM UserInfo WHERE Email = @Email";
                SqlCommand selectCmd = new SqlCommand(selectQuery, con);
                selectCmd.Parameters.AddWithValue("@Email", newEmail);
                int count = (int)selectCmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Email already exists. Please enter a different email address.");
                }
                else
                {
                    string newPwd = pwd.Password.ToString();
                    string pattern = @"^(?=.*[a-z].*[a-z])(?=.*[A-Z].*[A-Z])(?=.*\d.*\d)(?=.*[^a-zA-Z0-9].*[^a-zA-Z0-9]).{8,15}$";
                    Regex regex = new Regex(pattern);
                    if (regex.IsMatch(newPwd))
                    {
                        string password = pwd.Password.ToString();
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                        byte[] hashBytes;
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            hashBytes = sha256.ComputeHash(passwordBytes);
                        }
                        string hashedPassword = Convert.ToBase64String(hashBytes);

                        // Password meets the criteria
                        string qury = "INSERT INTO UserInfo OUTPUT INSERTED.UserId values(@Password, @FName, @LName, @Dob, @Email, @PhoneNumber, @occupation, @street, @city, @province, @country, @postalcode)";
                        SqlCommand cmd = new SqlCommand(qury, con);
                        //We need to call the textbox by name to grab the text in it
                        cmd.Parameters.AddWithValue("@FName", fname.Text);
                        cmd.Parameters.AddWithValue("@LName", lname.Text);
                        cmd.Parameters.AddWithValue("@Dob", dobPicker.SelectedDate.Value.Date);
                        cmd.Parameters.AddWithValue("@Country", country.Text);
                        cmd.Parameters.AddWithValue("@PhoneNumber", mobile.Text);
                        cmd.Parameters.AddWithValue("@Email", email.Text);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.Parameters.AddWithValue("@occupation", occupation.Text);
                        cmd.Parameters.AddWithValue("@street", street.Text);
                        cmd.Parameters.AddWithValue("@city", city.Text);
                        cmd.Parameters.AddWithValue("@province", province.Text);
                        cmd.Parameters.AddWithValue("@postalcode", postalcode.Text);
                        //we now need to excute our qury and Retrieve the newly inserted user's id
                        int userId = (int)cmd.ExecuteScalar();
                        //Generete the account number for this user
                        Random rand = new Random();
                        long accountNumber = (long)(rand.NextDouble() * (9832789 - 3) + rand.NextDouble() * (89732 - 298)) * 100;
                        string accountQuery = "Insert Into UserAccount values (@UserId, @CardNumber, 0.0)";
                        SqlCommand accountCmd = new SqlCommand(accountQuery, con);
                        accountCmd.Parameters.AddWithValue("@UserId", userId);
                        accountCmd.Parameters.AddWithValue("@CardNumber", accountNumber);
                        accountCmd.ExecuteNonQuery();
                        MessageBox.Show("Registe Successfully, click ok go back to log in page!");
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.email.Text = this.email.Text;
                        this.Close();
                        mainWindow.Show();
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Password must contain: 2 Uppercase letters, 2 Lowercase letters, 2 digits and 2 special charactors, length 8-15!");
                    }



                   
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow= new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
