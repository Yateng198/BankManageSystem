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
        SqlConnection con;
        SqlCommand cmd, cmd1, cmd2;
        SqlDataReader userInfoReader, userAccountReader;
        public MainWindow()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=DESKTOP-1AHTENP\\MSSQLSERVER01;Initial Catalog=BankManageSystemNewDB;Integrated Security=True");
            con.Open();
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
            var data = new { email = email.Text, password = hashedPassword };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await client.PostAsync("https://localhost:7026/api/UserInfo/login", content);

            string response = await responseMessage.Content.ReadAsStringAsync();
            UserInfoResponse userInfoResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response);


            //Take out the product object from the Json response message object
            UserInfo user = userInfoResponse.user;
            UserAccount account = userInfoResponse.accout;
            if (responseMessage.IsSuccessStatusCode)
            {
                MyAccount myAccount  = new MyAccount();
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
                MessageBox.Show("Login Failed!");
            }





            /* try
             {

                 cmd = new SqlCommand("select count(1) from UserInfo where Email = @email and Password = @Pwd COLLATE SQL_Latin1_General_CP1_CS_AS", con);
                 cmd.CommandType = System.Data.CommandType.Text;
                 cmd.Parameters.AddWithValue("@email", email.Text);
                 cmd.Parameters.AddWithValue("@Pwd", hashedPassword);
                 int count = Convert.ToInt32(cmd.ExecuteScalar());
                 if (count == 1)
                 {
                     cmd1 = new SqlCommand("select UserId, F_Name, L_Name, Date_Of_Birth, Address_Country, Mobile, Email from UserInfo where email = @email", con);
                     cmd1.Parameters.AddWithValue("@email", email.Text);
                     userInfoReader = cmd1.ExecuteReader();

                     int logInUserId = 0;

                     MyAccount myAccount = new MyAccount();
                     while (userInfoReader.Read())
                     {
                         logInUserId = (int)userInfoReader.GetValue(0);
                         myAccount.username.Text = userInfoReader.GetValue(1).ToString() + " " + userInfoReader.GetValue(2).ToString();
                         DateTime dateTime = (DateTime)userInfoReader.GetValue(3);
                         myAccount.userage.Text = dateTime.Date.ToString("d");

                         //  myAccount.userage.Content = userInfoReader.GetValue(3).ToString();
                         myAccount.usercountry.Text = userInfoReader.GetValue(4).ToString();
                         myAccount.userphonenumber.Text = userInfoReader.GetValue(5).ToString();
                         myAccount.useremail.Text = userInfoReader.GetValue(6).ToString();

                     }
                     userInfoReader.Close();
                     cmd2 = new SqlCommand("select CardNumber, Balance from UserAccount where UserId = @id", con);
                     cmd2.Parameters.AddWithValue("@id", logInUserId);
                     userAccountReader = cmd2.ExecuteReader();
                     while (userAccountReader.Read())
                     {
                         myAccount.usercardnumber.Text = userAccountReader.GetValue(0).ToString();
                         myAccount.amount.Text = userAccountReader.GetValue(1).ToString() + "$";
                     }
                     userAccountReader.Close();
                     myAccount.Show();
                     this.Close();
                 }
                 else
                 {
                     MessageBox.Show("Please Check Your Email & Password, and Try Again!");
                 }
             }catch (SqlException ex)
             {
                 MessageBox.Show(ex.Message);
             }
 */

        }

        private void exchange_Click(object sender, RoutedEventArgs e)
        {
            Exchange ex = new Exchange();
            con.Close();
            ex.Show();
            this.Close();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            con.Close();
            this.Close();
        }
    }
}
