
using XamarinAndroidApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(FileWriter))]
namespace XamarinAndroidApp.Droid
{
    public class FileWriter: IFileWriter
    {

        public string getPath()
        {


            return Android.OS.Environment.ExternalStorageDirectory.ToString();
        }
    }
}