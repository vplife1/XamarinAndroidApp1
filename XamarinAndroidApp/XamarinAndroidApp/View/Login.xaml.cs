using Newtonsoft.Json;
using SQLite;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAndroidApp.Model;

namespace XamarinAndroidApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        SQLiteAsyncConnection dataBase;
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var databasePath = Path.Combine(basePath, "SQLite.db3");

            dataBase = new SQLiteAsyncConnection(databasePath);
            await dataBase.CreateTableAsync(typeof(Results));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            LoadData();
        }

        public async void LoadData()
        {
            var data = new
            {
                phleboMobileNumber = number.Text,
                mpinCode = pin.Text
            };

            var RestURL = "https://tcdevapi.iworktech.net/v1api/Phlebotomists/phlebotomistLogin";
            HttpClient client = new HttpClient();
            string jsonData = JsonConvert.SerializeObject(data);
            client.BaseAddress = new Uri(RestURL);

            StringContent content1 = new StringContent(jsonData, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("apptoken", "72f303a7-f1f0-45a0-ad2b-e6db29328b1a");
            client.DefaultRequestHeaders.Add("usertoken", "jsDNLpSUttMEdVXkAvp8KCKxpvp8WUsgpLmkgi+IIZ7JVj5sPJgcNfmN2qpjQQEZNjZV/BqM7Yh7WIkDhtSiY3YpAipyokmixrpkqzADq+j4G4tILCEj0IsAe/i1x6ZIiOq6yzgaUA5fRq3HU4p2Xg==");
            HttpResponseMessage response = await client.PostAsync(RestURL, content1);
            var result = await response.Content.ReadAsStringAsync();
            Response responseData = JsonConvert.DeserializeObject<Response>(result);

            if (responseData.Status == true )
            {
                
                if (dataBase.InsertAsync(responseData.Results) == null)
                {
                    await dataBase.InsertAsync(responseData.Results);
                }
                else
                {
                    var lab = await dataBase.Table<Results>().ToListAsync();
                    await Navigation.PushAsync(new Dashboard());
                    
                }
            }
            else
            {
                await DisplayAlert("Result", responseData.Message, "OK");
            }

        }
    }
}
