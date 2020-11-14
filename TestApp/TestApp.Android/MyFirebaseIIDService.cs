using Android.App;
using Android.Util;
using Firebase.Messaging;

namespace FCMClient
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseMessagingService
    {
        private const string TAG = "MyFirebaseIIDService";

        public override void OnNewToken(string newToken)
        {
            base.OnNewToken(newToken);
            Log.Debug(TAG, "RefreshedToken: " + newToken);
            SendRegistrationToServer(newToken);
        }

        private void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }
}