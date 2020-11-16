using Newtonsoft.Json;
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

        private IFacebookClient _facebookService = CrossFacebookClient.Current;

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
            //not tested
            await Navigation.PushAsync(new PostSocial("Facebook", FbToken.Text));
        }

        private async void TwitPost(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PostSocial("Twitter", TwitToken.Text));
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

                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    if (e == null) return;

                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
                            LoginFb.Text = $"{facebookProfile.FirstName} {facebookProfile.LastName}";
                            FbToken.Text = _facebookService.ActiveToken;
                            FbButton.IsEnabled = false;
                            FbPostButton.IsEnabled = true;
                            FbLogoutButton.IsEnabled = true;
                            Debug.WriteLine("Facebook login success");
                            break;

                        case FacebookActionStatus.Canceled:
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                };

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "gender", "last_name" };
                string[] fbPermisions = { "email" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}