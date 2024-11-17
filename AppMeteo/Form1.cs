using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace AppMeteo
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient client = new HttpClient();
        private string apiKey = "VOTRE_CLÉ_API";

        public Form1()
        {
            InitializeComponent();
            this.searchButton.Click += new EventHandler(SearchButton_Click);
            this.refreshButton.Click += new EventHandler(RefreshButton_Click);
            LoadDefaultWeatherData();
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            string city = cityTextBox.Text;
            if (!string.IsNullOrEmpty(city))
            {
                await FetchWeatherData(city);
            }
            else
            {
                MessageBox.Show("Veuillez entrer une ville.");
            }
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            await FetchWeatherData(cityLabel.Text);
        }

        private async void LoadDefaultWeatherData()
        {
            await FetchWeatherData("Paris");
        }

        private async Task FetchWeatherData(string city)
        {
            try
            {
                string currentWeatherUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&lang=fr&appid={apiKey}&units=metric";
                HttpResponseMessage currentResponse = await client.GetAsync(currentWeatherUrl);

                if (currentResponse.IsSuccessStatusCode)
                {
                    string currentResponseBody = await currentResponse.Content.ReadAsStringAsync();
                    JObject weatherData = JObject.Parse(currentResponseBody);

                    string cityName = weatherData["name"].ToString();
                    string temperature = weatherData["main"]["temp"].ToString() + "°C";
                    string description = weatherData["weather"][0]["description"].ToString();
                    string iconCode = weatherData["weather"][0]["icon"].ToString();
                    string iconUrl = $"http://openweathermap.org/img/wn/{iconCode}@2x.png";

                    var iconImage = await DownloadImageAsync(iconUrl);
                    weatherIcon.Image = iconImage;

                    cityLabel.Text = cityName;
                    temperatureLabel.Text = temperature;
                    descriptionLabel.Text = char.ToUpper(description[0]) + description.Substring(1);

                    await FetchWeatherForecast(city);
                }
                else
                {
                    MessageBox.Show("Ville introuvable");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }

        private async Task FetchWeatherForecast(string city)
        {
            try
            {
                string forecastUrl = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&lang=fr&appid={apiKey}&units=metric";
                HttpResponseMessage forecastResponse = await client.GetAsync(forecastUrl);

                if (forecastResponse.IsSuccessStatusCode)
                {
                    string forecastResponseBody = await forecastResponse.Content.ReadAsStringAsync();
                    JObject forecastData = JObject.Parse(forecastResponseBody);

                    forecastPanel.Controls.Clear();

                    var forecasts = forecastData["list"].Take(3);
                    int xOffset = 0;

                    foreach (var item in forecasts)
                    {
                        string dateTime = item["dt_txt"].ToString();
                        string temp = item["main"]["temp"].ToString() + "°C";
                        string weatherDesc = item["weather"][0]["description"].ToString();
                        string iconCode = item["weather"][0]["icon"].ToString();
                        string iconUrl = $"http://openweathermap.org/img/wn/{iconCode}@2x.png";

                        Panel forecastCard = await CreateForecastCard(dateTime, temp, weatherDesc, iconUrl);

                        forecastCard.Location = new Point(xOffset, 0);
                        xOffset += forecastCard.Width + 20;

                        forecastPanel.Controls.Add(forecastCard);
                    }
                }
                else
                {
                    MessageBox.Show("Erreur lors du chargement des prévisions.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }


        private async Task<Panel> CreateForecastCard(string dateTime, string temp, string description, string iconUrl)
        {
            Panel card = new Panel();
            card.Size = new System.Drawing.Size(110, 150);
            card.BackColor = System.Drawing.Color.FromArgb(20, 56, 66);
            card.Margin = new Padding(5);

            PictureBox icon = new PictureBox();
            icon.Size = new System.Drawing.Size(50, 50);
            icon.Location = new System.Drawing.Point(30, 5);
            icon.SizeMode = PictureBoxSizeMode.Zoom;
            icon.Image = await DownloadImageAsync(iconUrl);

            Label tempLabel = new Label();
            tempLabel.Text = temp;
            tempLabel.ForeColor = System.Drawing.Color.White;
            tempLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            tempLabel.Location = new System.Drawing.Point(10, 60);
            tempLabel.Size = new System.Drawing.Size(90, 25);
            tempLabel.TextAlign = ContentAlignment.MiddleCenter;

            Label descLabel = new Label();
            descLabel.Text = char.ToUpper(description[0]) + description.Substring(1);
            descLabel.ForeColor = System.Drawing.Color.LightGray;
            descLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            descLabel.Location = new System.Drawing.Point(10, 85);
            descLabel.Size = new System.Drawing.Size(90, 25);
            descLabel.TextAlign = ContentAlignment.MiddleCenter;

            Label dateLabel = new Label();
            dateLabel.Text = DateTime.Parse(dateTime).ToString("dd MMM, HH:mm");
            dateLabel.ForeColor = System.Drawing.Color.LightGray;
            dateLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            dateLabel.Location = new System.Drawing.Point(10, 110);
            dateLabel.Size = new System.Drawing.Size(90, 15);
            dateLabel.TextAlign = ContentAlignment.MiddleCenter;

            card.Controls.Add(icon);
            card.Controls.Add(tempLabel);
            card.Controls.Add(descLabel);
            card.Controls.Add(dateLabel);

            return card;
        }

        private async Task<Image> DownloadImageAsync(string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return Image.FromStream(stream);
        }
    }
}
