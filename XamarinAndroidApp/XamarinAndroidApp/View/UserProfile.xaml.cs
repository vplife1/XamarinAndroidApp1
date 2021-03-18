using Newtonsoft.Json;
using SQLite;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAndroidApp.Model;

namespace XamarinAndroidApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : ContentPage
    {
       
        public UserProfile()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            this.Title = "XamFile Writer";
            LoadData();

            LabTestData.RefreshCommand=new Command(() => {
                LoadData();
                LabTestData.IsRefreshing = false;

            });

           
        }
        SQLiteAsyncConnection dataBase;
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var databasePath = Path.Combine(basePath, "SQLite.db3");

            dataBase = new SQLiteAsyncConnection(databasePath);
            await dataBase.CreateTableAsync(typeof(LabTestData));
        }

        public async void LoadData()
        {

            string myData = "{\"filter\": {\"labtestName\": [{\"labtestName\": \"Ada\"}]}}";
            var RestURL = "https://tcdevapi.iworktech.net/v1api/LabTest/HSCLabTests";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(RestURL);

            StringContent content1 = new StringContent(myData, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Add("apptoken", "72f303a7-f1f0-45a0-ad2b-e6db29328b1a");
            HttpResponseMessage response = await client.PostAsync(RestURL, content1);
            var result = await response.Content.ReadAsStringAsync();
            UserData responseData = JsonConvert.DeserializeObject<UserData>(result);
            var lab = await dataBase.Table<LabTestData>().ToListAsync();

            if (lab.Count == 0)
            {
                await dataBase.InsertAllAsync(responseData.Results.LabTestData);
                LabTestData.ItemsSource = lab;
            }
            else
            {
                //Get The Data From Database                
                LabTestData.ItemsSource = lab;                
            }

          
        }
     


        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            
            
            try
            {
               
            
                var ll = await dataBase.Table<LabTestData>().ToListAsync();
               

                if (string.IsNullOrWhiteSpace(Convert.ToString(txtFileName.Text)))
                {
                    await DisplayAlert("Error", "Please enter File Name", "Ok");
                    return;
                }

                //else if (string.IsNullOrWhiteSpace(Convert.ToString(ll)))
                //{
                //    await DisplayAlert("Error", "Please enter File Name", "Ok");
                //    return;
                //}


                string filePath = "";

                if (Device.RuntimePlatform == Device.Android)
                    filePath = DependencyService.Get<IFileWriter>().getPath() + "/Documents";

                else
                    filePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                var filename = System.IO.Path.Combine(filePath, txtFileName.Text.ToString() + ".txt");
                string AllData = string.Empty;
                foreach (var li in ll)
                {
                    string TestId ="TestId :"+ li.TestId.ToString();
                    string TestName = "TestName :" + li.TestName.ToString();
                    string Amount = "Amount :" + li.Amount.ToString();
                    string ServiceSubGroupName = "ServiceSubGroupName :" + li.ServiceSubGroupName.ToString();
                    string IsPopular = "IsPopular :" + li.IsPopular.ToString();
                    string TestType = "TestType :" + li.TestType.ToString();


                    AllData = AllData + "\n " + TestId + "\n "+ TestName +  "\n "+ Amount +  "\n "+ ServiceSubGroupName +  "\n "+ IsPopular +  "\n " + TestType + "\n\n ---------------";
                    
                }
                File.WriteAllText(filename, Convert.ToString(AllData));
               
                await DisplayAlert("File saved to:", System.IO.Path.Combine(filePath, txtFileName.Text.ToString()).ToString() + ".txt", "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error:", ex.Message.ToString(), "Ok");
            }
        }

    }
}