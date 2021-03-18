using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateXF.Widget.Charts.Models;
using UltimateXF.Widget.Charts.Models.Formatters;
using UltimateXF.Widget.Charts.Models.LineChart;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAndroidApp.Model;

namespace XamarinAndroidApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LinePage : ContentPage
    {
        String[] products = new string[] {"Mobiles","Tablets","Earphones","Headphones","Speakers",
                    "USB Cables","Laptops","Backcase","Screencover" };
        public LinePage()
        {
            InitializeComponent();
        }
        public void LoadChart()
        {
            try
            {
                var entries = new List<EntryChart>();
                var entries2 = new List<EntryChart>();
                var labels = new List<string>();

                var random = new Random();
                for (int i = 0; i < 7; i++)
                {
                    entries.Add(new EntryChart(i, random.Next(1000, 50000)));
                    entries2.Add(new EntryChart(i, random.Next(1000, 50000)));
                    labels.Add(products[i]);
                }
                var FontFamily = "";
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        FontFamily = "Pacifico-Regular";
                        break;
                    case Device.Android:
                        FontFamily = "Fonts/Pacifico-Regular.ttf";
                        break;
                    default:
                        break;
                }
                var dataSet4 = new LineDataSetXF(entries, "Product Summary 1")
                {
                    CircleRadius = 10,
                    CircleHoleRadius = 4f,
                    CircleColors = new List<Color>(){
                    Color.Accent, Color.Red, Color.Bisque, Color.Gray, Color.Green, Color.Chocolate, Color.Black
                    },
                    CircleHoleColor = Color.Green,

                    ValueColors = new List<Color>(){
                    Color.FromHex("#3696e0"), Color.FromHex("#9958bc"),
                    Color.FromHex("#35ad54"), Color.FromHex("#2d3e52"),
                    Color.FromHex("#e55137"), Color.FromHex("#ea9940"),
                    Color.Black
                    },
                    Mode = LineDataSetMode.CUBIC_BEZIER,
                    ValueFormatter = new CustomDataSetValueFormatter(),
                    ValueFontFamily = FontFamily
                };

                var dataSet5 = new LineDataSetXF(entries2, "Product Summary 2")
                {
                    Colors = new List<Color>{
                        Color.Green
                            },
                    CircleHoleColor = Color.Blue,
                    CircleColors = new List<Color>{
                        Color.Blue
                        },
                    CircleRadius = 3,
                    DrawValues = false,

                };

                var data4 = new LineChartData(new List<ILineDataSetXF>() { dataSet4, dataSet5 });

                chart.ChartData = data4;
                chart.DescriptionChart.Text = "Product chart description";
                chart.AxisLeft.DrawGridLines = false;
                chart.AxisLeft.DrawAxisLine = true;
                chart.AxisLeft.Enabled = true;

                chart.AxisRight.DrawAxisLine = false;
                chart.AxisRight.DrawGridLines = false;
                chart.AxisRight.Enabled = false;

                chart.AxisRight.FontFamily = FontFamily;
                chart.AxisLeft.FontFamily = FontFamily;
                chart.XAxis.FontFamily = FontFamily;

                chart.XAxis.XAXISPosition = XAXISPosition.BOTTOM;
                chart.XAxis.DrawGridLines = false;
                chart.XAxis.AxisValueFormatter = new TextByIndexXAxisFormatter(labels);

            }
            catch (Exception ex)
            {


            }
        }

        private void BtnLoad_Clicked(object sender, EventArgs e)
        {
            LoadChart();
        }
    }
}