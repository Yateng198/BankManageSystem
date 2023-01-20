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
        SqlCommand cmd;
        SqlDataReader reader;
        public MainWindow()
        {
            InitializeComponent();
            con = new SqlConnection("Data Source=desktop-1ahtenp\\mssqlserver01;Initial Catalog=BankManageSystem;Integrated Security=True;Pooling=False");
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
            MessageBox.Show("You are connected!");
            cmd = new SqlCommand("select count(1) from customerTable where Email = @email and Password = @Pwd", con);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@email", email.Text);
            cmd.Parameters.AddWithValue("@Pwd", txtpwd.Password);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if(count == 1)
            {
                MyAccount myAccount= new MyAccount();   
                myAccount.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Check Your Email & Password, and Try Again!");
            }
           

        }
    }
}
