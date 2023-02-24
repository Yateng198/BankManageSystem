using BankManageSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
        private static HttpClient client;


        public Register()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=DESKTOP-1AHTENP\\MSSQLSERVER01;Initial Catalog=BankManageSystemNewDB;Integrated Security=True");
            con.Open();

            dobPicker.DisplayDateStart = MinimumDate;
            dobPicker.DisplayDateEnd = MaximumDate;


        }
        private async void registe_Click(object sender, RoutedEventArgs e)
        {
            if (fname.Text == "")
            {
                fnamelabel.Foreground = Brushes.Red;
                MessageBox.Show("Enter your First Name please!");
                return;
            }
            if (lname.Text == "")
            {
                lnamelabel.Foreground = Brushes.Red;
                MessageBox.Show("Enter your Last Name please!");
                return;
            }
            if (email.Text == "")
            {
                emaillabel.Foreground = Brushes.Red;
                MessageBox.Show("Enter your Email please!");
                return;
            }
            if (pwd.Password.ToString() == "")
            {
                pwdlabel.Foreground = Brushes.Red;
                MessageBox.Show("Enter your Password please!");
                return;
            }
            if (mobile.Text == "")
            {
                mobilelabel.Foreground = Brushes.Red;
                MessageBox.Show("Enter your Phone Number please!");
                return;
            }
            if (postalcode.Text == "")
            {
                postalcodelabel.Foreground = Brushes.Red;
                MessageBox.Show("Enter your Postal Code please!");
                return;
            }
            UserInfo newUser = new UserInfo();
            newUser.password = pwd.Password.ToString();
            newUser.firstName = fname.Text.Trim();
            newUser.lastName = lname.Text.Trim();
            newUser.email = email.Text.Trim();
            try
            {
                DateTime currentDay= DateTime.Now.Date;
                DateTime userDOB = dobPicker.SelectedDate.Value.Date;
                TimeSpan age = currentDay - userDOB;
                if (age.TotalDays >= 18 * 365)
                {
                    newUser.DOB = userDOB;
                }
                else
                {
                    MessageBox.Show("We are sorry, your age is not sufficient to open bank account yet!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select or enter a valide date please (yyyy-mm-dd)");
                return;
            }
            newUser.phoneNumber = mobile.Text.Trim();
            newUser.occupation = occupation.Text.Trim();
            newUser.addressStreet = street.Text.Trim();
            newUser.addressCity = city.Text.Trim();
            newUser.addressProvince = province.Text.Trim();
            newUser.addressCountry = country.Text.Trim();
            newUser.postalCode = postalcode.Text.Trim();

            client = new HttpClient();
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("https://localhost:7026/api/UserInfo/register", newUser);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);
            //Take out the product object from the Json response message object
            UserInfo user = userInfoResponse.user;
            string msg = userInfoResponse.StatusMessage;
            int statusCode = userInfoResponse.StatusCode;
            

            if (statusCode == 200)
            {
                MessageBox.Show(msg);
                MainWindow mainWindow = new MainWindow();
                mainWindow.email.Text = this.email.Text;
                this.Close();
                mainWindow.Show();
            }
            else if(statusCode == 100)
            {
                MessageBox.Show(msg);
            }

            
            /* try
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

                         try
                         {
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
                         catch (Exception ex)
                         {
                             MessageBox.Show("Check your information entered and try again please!");
                             if (fname.Text == "")
                             {
                                 fnamelabel.Foreground = Brushes.Red;
                             }
                             if(lname.Text == "")
                             {
                                 lnamelabel.Foreground = Brushes.Red;
                             }
                             if(email.Text == "")
                             {
                                 emaillabel.Foreground = Brushes.Red;
                             }
                             if(pwd.Password.ToString() == "")
                             {
                                 pwdlabel.Foreground = Brushes.Red;
                             }
                             if(mobile.Text == "")
                             {
                                 mobilelabel.Foreground = Brushes.Red;
                             }
                             if(postalcode.Text == "")
                             {
                                 postalcodelabel.Foreground = Brushes.Red;
                             }

                         }

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
             }*/
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            con.Close();
            this.Close();
        }
    }
}
