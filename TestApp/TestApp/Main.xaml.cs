using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : ContentPage
    {
        public Main()
        {
            InitializeComponent();
        }

        private async void KeypadButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Keypad());
        }

        private async void ScannerButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Scanner());
        }

        private async void MediaButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Media());
        }

        private async void ListViewButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListViewTutor());
        }

        private async void SocialButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SocialMediaPage());
        }
    }
}