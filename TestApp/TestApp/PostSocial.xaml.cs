using Acr.UserDialogs;
using Plugin.Media;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostSocial : ContentPage
    {
        private string Media;
        private string Token;
        private byte[] imageArray = null;

        public PostSocial(string media, string token)
        {
            InitializeComponent();
            Media = media;
            Token = token;
        }

        private void PhotoButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                .SetTitle("Select options:")
                .SetCancel()
                .Add("Camera", async () => await OpenCam())
                .Add("Gallery", async () => await OpenGallery())
                );
        }

        private async Task OpenGallery()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            if (file == null)
                return;

            using (MemoryStream memory = new MemoryStream())
            {
                Stream stream = file.GetStream();
                stream.CopyTo(memory);
                imageArray = memory.ToArray();
            }

            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private async Task OpenCam()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            using (MemoryStream memory = new MemoryStream())
            {
                Stream stream = file.GetStream();
                stream.CopyTo(memory);
                imageArray = memory.ToArray();
            }

            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        //Not tested
        private void PostButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PostText.Text))
            {
                UserDialogs.Instance.Alert("Please Enter Text!");
                return;
            }
            if (PhotoImage.Source is null)
            {
                UserDialogs.Instance.Alert("Please Pick Image!");
                return;
            }
            UserDialogs.Instance.Alert(PostText.Text, Media);

            //if (Media.Equals("facebook"))
            //{
            //IFacebookClient _facebookService = CrossFacebookClient.Current;
            //await _facebookService.SharePhotoAsync(imageArray, PostText.Text);
            //UserDialogs.Instance.Alert(PostText.Text, Media);
            //}
        }
    }
}