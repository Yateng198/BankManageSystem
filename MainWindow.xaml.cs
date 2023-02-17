﻿using System;
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
        SqlCommand cmd, cmd1;
        SqlDataReader reader;
        public MainWindow()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=LAPTOP-DT6BMRBG;Initial Catalog=final;Integrated Security=True");
            con.Open();
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            Register rg = new Register();
            rg.Show();
            this.Close();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmd = new SqlCommand("select count(1) from customerTable where Email = @email and Password = @Pwd COLLATE SQL_Latin1_General_CP1_CS_AS", con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@email", email.Text);
                cmd.Parameters.AddWithValue("@Pwd", txtpwd.Password);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    cmd1 = new SqlCommand("select FName, LName, DateOfBirth, Country, PhoneNumber, Email from customerTable where email = @email",con);
                    cmd1.Parameters.AddWithValue("@email", email.Text);
                    reader = cmd1.ExecuteReader();
                    MyAccount myAccount = new MyAccount();
                    while (reader.Read())
                    {
                        myAccount.username.Content = reader.GetValue(0).ToString() + " " + reader.GetValue(1).ToString();
                        myAccount.userage.Content = reader.GetValue(2).ToString();
                        myAccount.usercountry.Content= reader.GetValue(3).ToString();
                        myAccount.userphonenumber.Content= reader.GetValue(4).ToString();
                        myAccount.usercardnumber.Content = "TBD";
                        myAccount.amount.Content = "TBD";
                        myAccount.useremail.Content = reader.GetValue(5).ToString();
                    }
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
           

        }

        private void exchange_Click(object sender, RoutedEventArgs e)
        {
            Exchange ex = new Exchange();
            ex.Show();
            this.Close();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
