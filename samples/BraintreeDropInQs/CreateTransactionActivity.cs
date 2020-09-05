
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Com.Braintreepayments.Api.Models;
using BraintreeDropInQs.Models;
using Android.Text;
using System.Threading.Tasks;

namespace BraintreeDropInQs
{
    [Activity(Label = "CreateTransactionActivity", Theme = "@style/Theme.AppCompat.Light")]
    public class CreateTransactionActivity : AppCompatActivity
    {
        public static Java.Lang.String EXTRA_PAYMENT_METHOD_NONCE = new Java.Lang.String("nonce");

        private ProgressBar mLoadingSpinner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.create_transaction_activity);
            mLoadingSpinner = FindViewById<ProgressBar>(Resource.Id.loading_spinner);
            SetTitle(Resource.String.processing_transaction);
            SendNonceToServer((PaymentMethodNonce)Intent.GetParcelableExtra(EXTRA_PAYMENT_METHOD_NONCE.ToString()));
        }

        async void SendNonceToServer(PaymentMethodNonce nonce)
        {
            Task<Transaction> task;

            if (Settings.isThreeDSecureEnabled(this) && Settings.isThreeDSecureRequired(this))
            {
                task = DemoApplication.getApiClient(this).CreateTransaction(nonce.Nonce, Settings.GetThreeDSecureMerchantAccountId(this), true);
            }
            else if (Settings.isThreeDSecureEnabled(this))
            {
                task = DemoApplication.getApiClient(this).CreateTransaction(nonce.Nonce, Settings.GetThreeDSecureMerchantAccountId(this));
            }
            else if (nonce is CardNonce && ((CardNonce)nonce).CardType.Equals("UnionPay"))
            {
                task = DemoApplication.getApiClient(this).CreateTransaction(nonce.Nonce, Settings.GetUnionPayMerchantAccountId(this));
            }
            else
            {
                task = DemoApplication.getApiClient(this).CreateTransaction(nonce.Nonce, Settings.GetMerchantAccountId(this));
            }

            var transaction = await task;

            if (transaction == null)
            {
                SetStatus(Resource.String.transaction_failed);
                SetMessage(new Java.Lang.String("Unable to create a transaction"));

                return;
            }

            if (transaction.Message != null &&
                   transaction.Message.StartsWith("created"))
            {
                SetStatus(Resource.String.transaction_complete);
                SetMessage(new Java.Lang.String(transaction.Message));
            }
            else
            {
                SetStatus(Resource.String.transaction_failed);
                if (TextUtils.IsEmpty(transaction.Message))
                {
                    SetMessage(new Java.Lang.String("Server response was empty or malformed"));
                }
                else
                {
                    SetMessage(new Java.Lang.String(transaction.Message));
                }
            }
        }

        void SetStatus(int message)
        {
            mLoadingSpinner.Visibility = ViewStates.Gone;
            SetTitle(message);
            TextView status = FindViewById<TextView>(Resource.Id.transaction_status);
            status.SetText(message);
            status.Visibility = ViewStates.Gone;
        }

        void SetMessage(Java.Lang.String message)
        {
            mLoadingSpinner.Visibility = ViewStates.Gone;
            TextView textView = FindViewById<TextView>(Resource.Id.transaction_message);
            textView.Text = message.ToString();
            textView.Visibility = ViewStates.Visible;
        }
    }
}