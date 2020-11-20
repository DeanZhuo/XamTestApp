using Acr.UserDialogs;
using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestApp
{
    //socialmedia
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SocialMediaPage : ContentPage
    {
        public SocialMediaPage()
        {
            InitializeComponent();
            LoginFb.Text = "Facebook not logged in";
            LoginTwit.Text = "Twitter not logged in";
            FbToken.Text = "";
            TwitToken.Text = "";
            FbButton.IsEnabled = true;
            TwitButton.IsEnabled = false;
            FbPostButton.IsEnabled = false;
            TwitPostButton.IsEnabled = false;
            FbLogoutButton.IsEnabled = false;
            TwitLogoutButton.IsEnabled = false;
        }

        private static IFacebookClient _facebookService = CrossFacebookClient.Current;

        private async void FbLog(object sender, EventArgs e)
        {
            await LoginFacebookAsync();
        }

        private async void TwitLog(object sender, EventArgs e)
        {
            //
            await Navigation.PushAsync(new ListViewTutor());
        }

        private async void FbPost(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PostSocial("Facebook"));
        }

        private async void TwitPost(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PostSocial("Twitter"));
        }

        private void FbOut(object sender, EventArgs e)
        {
            _facebookService.Logout();
            LoginFb.Text = "Facebook not logged in";
            FbToken.Text = "";
            Debug.WriteLine("Facebook logout");
            FbButton.IsEnabled = true;
            FbPostButton.IsEnabled = false;
            FbLogoutButton.IsEnabled = false;
        }

        private async void TwitOut(object sender, EventArgs e)
        {
            //
            await Navigation.PushAsync(new ListViewTutor());
        }

        private async Task LoginFacebookAsync()
        {
            Debug.WriteLine("Facebook login");
            try
            {
                if (_facebookService.IsLoggedIn)
                    _facebookService.Logout();

                string[] fbPermisions = { "publish_pages" };
                FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(fbPermisions, FacebookPermissionType.Publish);

                switch (response.Status)
                {
                    case FacebookActionStatus.Completed:
                        await LoadData();
                        FbToken.Text = _facebookService.ActiveToken;
                        FbButton.IsEnabled = false;
                        FbPostButton.IsEnabled = true;
                        FbLogoutButton.IsEnabled = true;
                        Debug.WriteLine("Facebook login success");
                        break;

                    case FacebookActionStatus.Canceled:
                        break;

                    case FacebookActionStatus.Unauthorized:
                        UserDialogs.Instance.Alert(response.Message, "Unauthorized", "Ok");
                        break;

                    case FacebookActionStatus.Error:
                        UserDialogs.Instance.Alert(response.Message, "Error", "Ok");
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async Task LoadData()
        {
            var jsonData = await _facebookService.RequestUserDataAsync(new string[] { "email", "name" }, new string[] { });
            var data = JObject.Parse(jsonData.Data);
            LoginFb.Text = data["name"].ToString();
        }

        public static async Task<string> PostFacebook(string text, byte[] imageArray)
        {
            string message = string.Empty;
            try
            {
                FacebookSharePhoto photo = new FacebookSharePhoto(text, imageArray);
                FacebookSharePhoto[] photos = new FacebookSharePhoto[] { photo };
                FacebookSharePhotoContent photoContent = new FacebookSharePhotoContent(photos, null, text);
                Console.WriteLine("Uploading...");
                var ret = await _facebookService.ShareAsync(photoContent);
                switch (ret.Status)
                {
                    case FacebookActionStatus.Completed:
                        message = "Facebook post success";
                        Debug.WriteLine("Facebook post success");
                        break;

                    case FacebookActionStatus.Canceled:
                        message = "Facebook post canceled";
                        Debug.WriteLine("Facebook post success");
                        break;

                    case FacebookActionStatus.Unauthorized:
                        message = ret.Message;
                        Debug.WriteLine("Facebook post success");
                        break;

                    case FacebookActionStatus.Error:
                        message = ret.Message;
                        Debug.WriteLine("Facebook post success");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PostFB: " + ex.ToString());
            }
            return message;
        }
    }
}