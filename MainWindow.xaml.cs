using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        private readonly string apiKey = "6683b118ea220b11e80c14c362d1beb1";
        private string url = "https://api.openweathermap.org/data/2.5/weather";

        public MainWindow()
        {        
            InitializeComponent();
            GetWeatherData("munich");
        }

        private void GetWeatherData(string cityname)
        {

            HttpClient httpClient = new HttpClient();
            string finalUrl = url + "?q=" + cityname + "&appid=" + apiKey + "&units=metric";
            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUrl).Result;
            string response = httpResponse.Content.ReadAsStringAsync().Result;

            WeatherMapResponse result = JsonConvert.DeserializeObject<WeatherMapResponse>(response);

            SetBackgroundPicture(result);
            SetTemperatureLabel(result);
            SetInfoLabel(result);
        }

        private void SetBackgroundPicture(WeatherMapResponse result)
        {
            string finalImage = "Sun.png";
            string currentWeather = result.weather[0].main.ToLower();

            if (currentWeather.Contains("clouds"))
            {
                finalImage = "Cloud.png";
            }
            else if (currentWeather.Contains("rain"))
            {
                finalImage = "Rain.png";
            }
            else if (currentWeather.Contains("snow"))
            {
                finalImage = "Snow.png";
            }

            backgroundImage.ImageSource = new BitmapImage(new Uri("images/" + finalImage, UriKind.Relative));
        }

        void SetTemperatureLabel(WeatherMapResponse result)
        {
            labelTemperature.Content = result.main.temp.ToString("F1") + "°C";
        }

        void SetInfoLabel(WeatherMapResponse result)
        {
            labelInfo.Content = result.weather[0].main;
        }

  
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = textBox.Text;
            GetWeatherData(query);
        }
    }
}