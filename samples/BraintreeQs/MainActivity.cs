using Android.App;
using Android.Widget;
using Android.OS;
using AndroidX.AppCompat.App;
using Com.Braintreepayments.Api.Interfaces;
using Com.Braintreepayments.Api.Models;
using Android.Runtime;
using Android.Content;
using Com.Braintreepayments.Api.Dropin;
using Java.Lang;

namespace BraintreeQs
{
    [Activity(Label = "BraintreeQs", MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        int REQUEST_CODE = 1111;
        int count = 1;
        IThreeDSecurePrepareLookupListener x;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var button = FindViewById<Button>(Resource.Id.myButton);
            button.Text = "Show Drop In";

            button.Click += delegate {
                var clientToken = "sandbox_9dbg82cq_dcpspy2brwdjr3qn";
                DropInRequest dropInRequest = new DropInRequest()
                        .ClientToken(clientToken);
                StartActivityForResult(dropInRequest.GetIntent(this), REQUEST_CODE);
            };

            ThreeDSecureLookup x;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
            if (requestCode == REQUEST_CODE) {
                if (resultCode == Result.Ok) {
                    var result = data.GetParcelableExtra(DropInResult.ExtraDropInResult);
                    // use the result to update your UI and send the payment method nonce to your server
                }
                else if (resultCode == Result.Canceled) {
                    // the user canceled
                }
                else {
                    // handle errors here, an exception may be available in
                    var error = (Exception)data.GetSerializableExtra(DropInActivity.ExtraError);
                }
            }
        }
    }
}

