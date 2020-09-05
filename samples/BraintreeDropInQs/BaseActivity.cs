using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using static Android.Support.V4.App.ActivityCompat;
using Com.Braintreepayments.Api.Interfaces;
using Com.Braintreepayments.Api.Models;
using Com.Braintreepayments.Api;
using BraintreeDropInQs.ApiInternal;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Text;
using Com.Paypal.Android.Sdk.Onetouch.Core;

namespace BraintreeDropInQs
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity : 
        AppCompatActivity, 
        IOnRequestPermissionsResultCallback, 
        IPaymentMethodNonceCreatedListener, 
        IBraintreeCancelListener, 
        IBraintreeErrorListener, 
        Android.Support.V7.App.ActionBar.IOnNavigationListener, 
        IDialogInterfaceOnClickListener
    {
        public static string WRITE_EXTERNAL_STORAGE = "android.permission.WRITE_EXTERNAL_STORAGE";

        static string KEY_AUTHORIZATION = "com.braintreepayments.demo.KEY_AUTHORIZATION";
        Android.App.AlertDialog dialog;
        protected string mAuthorization;
        protected string mCustomerId;
        protected BraintreeFragment mBraintreeFragment;

        bool mActionBarSetup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (savedInstanceState != null && savedInstanceState.ContainsKey(KEY_AUTHORIZATION))
            {
                mAuthorization = savedInstanceState.GetString(KEY_AUTHORIZATION);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (!mActionBarSetup)
            {
                SetupActionBar();
                mActionBarSetup = true;
            }

            SignatureVerificationOverrides.disableAppSwitchSignatureVerification(
                    Settings.isPayPalSignatureVerificationDisabled(this));
            PayPalOneTouchCore.UseHardcodedConfig(this, Settings.useHardcodedPayPalConfiguration(this));

            if (BuildConfig.DEBUG && ContextCompat.CheckSelfPermission(this, WRITE_EXTERNAL_STORAGE) != 0)
            {
                ActivityCompat.RequestPermissions(this, new string[] { (string)WRITE_EXTERNAL_STORAGE }, 1);
            }
            else
            {
                HandleAuthorizationState();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            HandleAuthorizationState();
        }

        void HandleAuthorizationState()
        {
            if (mAuthorization == null ||
                    (Settings.UseTokenizationKey(this) && !mAuthorization.Equals(Settings.GetEnvironmentTokenizationKey(this))) ||
                    !TextUtils.Equals(mCustomerId, Settings.GetCustomerId(this)))
            {
                PerformReset();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            if (mAuthorization != null)
            {
                outState.PutString(KEY_AUTHORIZATION, mAuthorization);
            }
        }

        void PerformReset()
        {
            mAuthorization = null;
            mCustomerId = Settings.GetCustomerId(this);

            if (mBraintreeFragment == null)
            {
                mBraintreeFragment = (BraintreeFragment)FragmentManager
                        .FindFragmentByTag("com.braintreepayments.api.BraintreeFragment");// change this line later
            }

            if (mBraintreeFragment != null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                {
                    FragmentManager.BeginTransaction().Remove(mBraintreeFragment).CommitNow();
                }
                else
                {
                    FragmentManager.BeginTransaction().Remove(mBraintreeFragment).Commit();
                    FragmentManager.ExecutePendingTransactions();
                }

                mBraintreeFragment = null;
            }

            Reset();
            FetchAuthorization();
        }

        protected abstract void Reset();
        protected abstract void OnAuthorizationFetched();

        protected void FetchAuthorization()
        {
            if (mAuthorization != null)
            {
                OnAuthorizationFetched();
            }
            else if (Settings.UseTokenizationKey(this))
            {
                mAuthorization = Settings.GetEnvironmentTokenizationKey(this);
                OnAuthorizationFetched();
            }
            else
            {
                DemoApplication.getApiClient(this).GetClientToken(
                        Settings.GetCustomerId(this),
                        Settings.GetMerchantAccountId(this))
                        .ContinueWith(t =>
                        {
                            if (t.IsFaulted || TextUtils.IsEmpty(t.Result))
                            {
                                ShowDialog("Client token was empty");
                            }
                            else
                            {
                                mAuthorization = t.Result;
                                OnAuthorizationFetched();
                            }
                        });
            }
        }

        void SetupActionBar()
        {
            Android.Support.V7.App.ActionBar actionBar = SupportActionBar;
            actionBar.SetDisplayShowTitleEnabled(false);
            actionBar.NavigationMode = (int)ActionBarNavigationMode.List;

            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this,
                    Resource.Array.environments, Android.Resource.Layout.SimpleDropDownItem1Line);
            actionBar.SetListNavigationCallbacks(adapter, this);
            actionBar.SetSelectedNavigationItem(Settings.getEnvironment(this));
        }

        public virtual void OnCancel(int requestCode)
        {
            System.Diagnostics.Debug.WriteLine("Cancel received: " + requestCode);
        }

        public virtual void OnError(Java.Lang.Exception error)
        {
            System.Diagnostics.Debug.WriteLine("Error received (" + error.GetType() + "): " + error.Message);
            System.Diagnostics.Debug.WriteLine(error.StackTrace);

            ShowDialog("An error occurred ");
        }

        public bool OnNavigationItemSelected(int itemPosition, long itemId)
        {
            if (Settings.getEnvironment(this) != itemPosition)
            {
                Settings.SetEnvironment(this, itemPosition);
                PerformReset();
            }
            return true;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                case Resource.Id.reset:
                    PerformReset();
                    return true;
                case Resource.Id.settings:
                    StartActivity(new Intent(this, typeof(SettingsActivity)));
                    return true;
                default:
                    return false;
            }
        }

        public virtual void OnPaymentMethodNonceCreated(PaymentMethodNonce p0)
        {
            System.Diagnostics.Debug.WriteLine("Payment Method Nonce received: " + p0.TypeLabel);
        }

        protected void ShowDialog(string message)
        {
            dialog = new Android.App.AlertDialog.Builder(this)
                    .SetMessage(message)
                    .SetPositiveButton(Android.Resource.String.Ok, this)
                    .Show();
        }

        protected void SetUpAsBack()
        {
            if (ActionBar != null)
            {
                ActionBar.SetDisplayHomeAsUpEnabled(true);
            }
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            dialog.Dismiss();
        }
    }
}
