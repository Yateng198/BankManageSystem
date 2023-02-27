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
        DateTime MinimumDate = new DateTime(1900, 1, 1);
        DateTime MaximumDate = DateTime.Today.AddYears(-18);
        private static HttpClient client;


        public Register()
        {
            InitializeComponent();
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
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
