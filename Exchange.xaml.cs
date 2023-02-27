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

        private async void checkCAD()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();

                Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
                Currency currency = jsonObj.currency;

                if (double.TryParse(cad_amount.Text, out double cadAmount))
                {
                    cad_amount.Text = cadAmount.ToString("F2");
                    usd_amount.Text = (cadAmount * currency.USD).ToString("F2");
                    eur_amount.Text = (cadAmount * currency.EUR).ToString("F2");
                    cny_amount.Text = (cadAmount * currency.CNY).ToString("F2");
                    jpy_amount.Text = (cadAmount * currency.JPY).ToString("F2");
                    aud_amount.Text = (cadAmount * currency.AUD).ToString("F2");
                    mxn_amount.Text = (cadAmount * currency.MXN).ToString("F2");
                }
                else
                {
                    MessageBox.Show("Please only input numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_usd(object sender, RoutedEventArgs e)
        {
            this.checkUSD();
        }

        private async void checkUSD()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();

                Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
                Currency currency = jsonObj.currency;

                if (double.TryParse(usd_amount.Text, out double usdAmount))
                {
                    usd_amount.Text = usdAmount.ToString("F2");
                    cad_amount.Text = (usdAmount / currency.USD).ToString("F2");
                    eur_amount.Text = (usdAmount * currency.EUR / currency.USD).ToString("F2");
                    cny_amount.Text = (usdAmount * currency.CNY / currency.USD).ToString("F2");
                    jpy_amount.Text = (usdAmount * currency.JPY / currency.USD).ToString("F2");
                    aud_amount.Text = (usdAmount * currency.AUD / currency.USD).ToString("F2");
                    mxn_amount.Text = (usdAmount * currency.MXN / currency.USD).ToString("F2");
                }
                else
                {
                    MessageBox.Show("Please only input numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_eur(object sender, RoutedEventArgs e)
        {
            this.checkEUR();
        }

        private async void checkEUR()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();

                Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
                Currency currency = jsonObj.currency;

                if (double.TryParse(eur_amount.Text, out double eurAmount))
                {
                    eur_amount.Text = eurAmount.ToString("F2");
                    cad_amount.Text = (eurAmount / currency.EUR).ToString("F2");
                    usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
                    cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
                    jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
                    aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
                    mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
                }
                else
                {
                    MessageBox.Show("Please only input numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_cny(object sender, RoutedEventArgs e)
        {
            this.checkCNY();
        }

        private async void checkCNY()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();

                Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
                Currency currency = jsonObj.currency;

                if (double.TryParse(cny_amount.Text, out double cnyAmount))
                {
                    cny_amount.Text = cnyAmount.ToString("F2");
                    cad_amount.Text = (cnyAmount / currency.CNY).ToString("F2");
                    usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
                    eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
                    jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
                    aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
                    mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
                }
                else
                {
                    MessageBox.Show("Please only input numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button_jpy(object sender, RoutedEventArgs e)
        {
            this.checkJPY();
        }

        private async void checkJPY()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();

                Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
                Currency currency = jsonObj.currency;

                if (double.TryParse(jpy_amount.Text, out double jpyAmount))
                {
                    jpy_amount.Text = jpyAmount.ToString("F2");
                    cad_amount.Text = (jpyAmount / currency.JPY).ToString("F2");
                    usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
                    eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
                    cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
                    aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
                    mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
                }
                else
                {
                    MessageBox.Show("Please only input numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_aud(object sender, RoutedEventArgs e)
        {
            this.checkAUD();
        }

        private async void checkAUD()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();

                Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
                Currency currency = jsonObj.currency;

                if (double.TryParse(aud_amount.Text, out double audAmount))
                {
                    aud_amount.Text = audAmount.ToString("F2");
                    cad_amount.Text = (audAmount / currency.AUD).ToString("F2");
                    usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
                    eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
                    cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
                    jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
                    mxn_amount.Text = (double.Parse(cad_amount.Text) * currency.MXN).ToString("F2");
                }
                else
                {
                    MessageBox.Show("Please only input numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_mxn(object sender, RoutedEventArgs e)
        {
            this.checkMXN();
        }

        private async void checkMXN()
        {
            try
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:7026/api/Currency/GetAllCurrencyByID/" + today);
                responseMessage.EnsureSuccessStatusCode();
                string response = await responseMessage.Content.ReadAsStringAsync();

                Response jsonObj = JsonConvert.DeserializeObject<Response>(response);
                Currency currency = jsonObj.currency;

                if (double.TryParse(mxn_amount.Text, out double mxnAmount))
                {
                    mxn_amount.Text = mxnAmount.ToString("F2");
                    cad_amount.Text = (mxnAmount / currency.MXN).ToString("F2");
                    usd_amount.Text = (double.Parse(cad_amount.Text) * currency.USD).ToString("F2");
                    eur_amount.Text = (double.Parse(cad_amount.Text) * currency.EUR).ToString("F2");
                    cny_amount.Text = (double.Parse(cad_amount.Text) * currency.CNY).ToString("F2");
                    jpy_amount.Text = (double.Parse(cad_amount.Text) * currency.JPY).ToString("F2");
                    aud_amount.Text = (double.Parse(cad_amount.Text) * currency.AUD).ToString("F2");
                }
                else
                {
                    MessageBox.Show("Please only input numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
