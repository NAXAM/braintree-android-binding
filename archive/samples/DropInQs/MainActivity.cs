using Android.App;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using Com.Braintreepayments.Api;
using Android.Gms.Wallet;
using Java.Interop;
using Android.Widget;

namespace DropInQs
{
    partial class MainActivity : IDropInListener
    {
        public void OnDropInFailure(Java.Lang.Exception p0)
        {
            lblResult.SetTextColor(Android.Graphics.Color.DarkRed);
            lblResult.Text = p0.Message;
            System.Diagnostics.Debug.WriteLine(p0.Message);
        }

        public void OnDropInSuccess(DropInResult p0)
        {
            lblResult.SetTextColor(Android.Graphics.Color.DarkGreen);
            lblResult.Text = p0.PaymentMethodNonce.ToString();
        }
    }

    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme.NoActionBar",
        MainLauncher = true,
        Exported = true
        )]
    public partial class MainActivity : AppCompatActivity
    {
        DropInClient dropInClient;
        DropInRequest dropInRequest;
        TextView lblResult;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            lblResult = FindViewById<TextView>(Resource.Id.lblResult);

            dropInRequest = new DropInRequest();
            dropInRequest.PayPalRequest = new PayPalVaultRequest();

            var googlePayRequest = new GooglePayRequest();
            googlePayRequest.TransactionInfo = TransactionInfo.NewBuilder()
                .SetTotalPrice("10.0")
                .SetTotalPriceStatus(WalletConstants.TotalPriceStatusFinal)
                .SetCurrencyCode("USD")
                .Build();
            googlePayRequest.BillingAddressRequired = true;
            dropInRequest.GooglePayRequest = googlePayRequest;

            dropInRequest.VenmoRequest = new VenmoRequest(VenmoPaymentMethodUsage.MultiUse);

            dropInRequest.ThreeDSecureRequest = new ThreeDSecureRequest
            {
                Amount = "10.0"
            };

            dropInClient = new DropInClient(this, "sandbox_tmxhyf7d_dcpspy2brwdjr3qn");
            dropInClient.SetListener(this);
        }

        [Export("openDropIn")]
        public void OpenDropIn(View view)
        {
            lblResult.Text = "Waiting...";
            dropInClient.LaunchDropIn(dropInRequest);
        }
    }
}

