using Newtonsoft.Json;
using BankManageSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BankManageSystem
{
    /// <summary>
    /// Interaction logic for Exchange.xaml
    /// </summary>
    public partial class Exchange : Window
    {
        HttpClient client = new HttpClient();
        public Exchange()
        {

            client.BaseAddress = new Uri("https://localhost:7026/api/Currency/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void Button_cad(object sender, RoutedEventArgs e)
        {
            this.checkCAD();
        }
        
        private async void checkCAD() {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
            Currency currency = jsonObj.currency;

            cad_amount.Text = (double.Parse(cad_amount.Text)).ToString("F2");
            usd_amount.Text = (double.Parse(cad_amount.Text)*currency.USD).ToString("F2");
            eur_amount.Text = (double.Parse(cad_amount.Text)*currency.EUR).ToString("F2");
            cny_amount.Text = (double.Parse(cad_amount.Text)*currency.CNY).ToString("F2");
            jpy_amount.Text = (double.Parse(cad_amount.Text)*currency.JPY).ToString("F2");
            aud_amount.Text = (double.Parse(cad_amount.Text)*currency.AUD).ToString("F2");
            mxn_amount.Text = (double.Parse(cad_amount.Text)*currency.MXN).ToString("F2");

        }

        private void Button_usd(object sender, RoutedEventArgs e)
        {
            this.checkUSD();
        }

        private async void checkUSD()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
            Currency currency = jsonObj.currency;

            usd_amount.Text = (double.Parse(usd_amount.Text)).ToString("F2");
            cad_amount.Text = (double.Parse(usd_amount.Text) / currency.USD).ToString("F2");
            eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
            cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
            jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
            aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
            mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
        }

        private void Button_eur(object sender, RoutedEventArgs e)
        {
            this.checkEUR();
        }

        private async void checkEUR()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
            Currency currency = jsonObj.currency;

            eur_amount.Text = (double.Parse(eur_amount.Text)).ToString("F2");
            cad_amount.Text = (double.Parse(eur_amount.Text) / currency.EUR).ToString("F2");
            usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
            cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
            jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
            aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
            mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
        }

        private void Button_cny(object sender, RoutedEventArgs e)
        {
            this.checkCNY();
        }

        private async void checkCNY()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
            Currency currency = jsonObj.currency;

            cny_amount.Text = (double.Parse(cny_amount.Text)).ToString("F2");
            cad_amount.Text = (double.Parse(cny_amount.Text) / currency.CNY).ToString("F2");
            usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
            eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
            jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
            aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
            mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
        }


        private void Button_jpy(object sender, RoutedEventArgs e)
        {
            this.checkJPY();
        }

        private async void checkJPY()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
            Currency currency = jsonObj.currency;

            jpy_amount.Text = (double.Parse(jpy_amount.Text)).ToString("F2");
            cad_amount.Text = (double.Parse(jpy_amount.Text) / currency.JPY).ToString("F2");
            usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
            eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
            cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
            aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
            mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
        }

        private void Button_aud(object sender, RoutedEventArgs e)
        {
            this.checkAUD();
        }

        private async void checkAUD()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
            Currency currency = jsonObj.currency;

            aud_amount.Text = (double.Parse(aud_amount.Text)).ToString("F2");
            cad_amount.Text = (double.Parse(aud_amount.Text) / currency.AUD).ToString("F2");
            usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
            eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
            cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
            jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
            mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
        }

        private void Button_mxn(object sender, RoutedEventArgs e)
        {
            this.checkMXN();
        }

        private async void checkMXN()
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
            responseMessage.EnsureSuccessStatusCode();
            string response = await responseMessage.Content.ReadAsStringAsync();

            Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
            Currency currency = jsonObj.currency;

            mxn_amount.Text = (double.Parse(mxn_amount.Text)).ToString("F2");
            cad_amount.Text = (double.Parse(mxn_amount.Text) / currency.MXN).ToString("F2");
            usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
            eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
            cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
            jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
            aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
        }

        private void Button_go_back(object sender, RoutedEventArgs e)
        {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();          
        }

        private void Button_clear(object sender, RoutedEventArgs e)
        {
            cad_amount.Text = "0";
            usd_amount.Text = "0";
            eur_amount.Text = "0";
            cny_amount.Text = "0";
            jpy_amount.Text = "0";
            aud_amount.Text = "0";
            mxn_amount.Text = "0";

            MessageBox.Show("Currency cleaned, now you could convert again!");
        }
    }


}
