using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace TestApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scanner : ContentPage
    {
        public Scanner()
        {
            InitializeComponent();
        }

        public void OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Scanned result", "The barcode's text is " + result.Text + ". The barcode's format is " + result.BarcodeFormat, "OK");
            });
        }
    }
}