using Android.App;
using Android.Widget;
using Android.OS;
using Com.Braintreepayments.Api.Models;
using Android.Support.V7.Widget;
using Com.Braintreepayments.Api.Exceptions;
using Com.Braintreepayments.Api.Dropin.Utils;
using Com.Braintreepayments.Api.Dropin;
using Android.Gms.Identity.Intents.Model;
using Android.Views;
using Java.Util;
using Com.Braintreepayments.Api;
using System.Collections.Generic;
using Android.Gms.Wallet;
using Android.Content;
using Com.Braintreepayments.Api.Interfaces;
using Java.Lang;
using Android.Runtime;
using Java.Interop;
using System;

namespace BraintreeDropInQs
{
    [Activity(Label = "BraintreeDropInQs", MainLauncher = true, Theme = "@style/Theme.AppCompat.Light")]
    public class MainActivity : BaseActivity, IPaymentMethodNonceCreatedListener, IBraintreeCancelListener, IBraintreeErrorListener, DropInResult.IDropInResultListener
    {
        static int DROP_IN_REQUEST = 100;

        static string KEY_NONCE = "nonce";

        PaymentMethodType mPaymentMethodType;
        PaymentMethodNonce mNonce;

        CardView mPaymentMethod;
        ImageView mPaymentMethodIcon;
        TextView mPaymentMethodTitle;
        TextView mPaymentMethodDescription;
        TextView mNonceString;
        TextView mNonceDetails;
        TextView mDeviceData;

        Button mAddPaymentMethodButton;
        Button mPurchaseButton;
        ProgressDialog mLoading;

        bool mShouldMakePurchase = false;

        bool mPurchased = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_activity);

            mPaymentMethod = FindViewById<CardView>(Resource.Id.payment_method);
            mPaymentMethodIcon = FindViewById<ImageView>(Resource.Id.payment_method_icon);
            mPaymentMethodTitle = FindViewById<TextView>(Resource.Id.payment_method_title);
            mPaymentMethodDescription = FindViewById<TextView>(Resource.Id.payment_method_description);
            mNonceString = FindViewById<TextView>(Resource.Id.nonce);
            mNonceDetails = FindViewById<TextView>(Resource.Id.nonce_details);
            mDeviceData = FindViewById<TextView>(Resource.Id.device_data);

            mAddPaymentMethodButton = FindViewById<Button>(Resource.Id.add_payment_method);
            mAddPaymentMethodButton.Click += MAddPaymentMethodButton_Click;

            mPurchaseButton = FindViewById<Button>(Resource.Id.purchase);

            if (savedInstanceState != null)
            {
                if (savedInstanceState.ContainsKey(KEY_NONCE))
                {
                    mNonce = (PaymentMethodNonce)savedInstanceState.GetParcelable(KEY_NONCE);
                }
            }
        }

        void MAddPaymentMethodButton_Click(object sender, System.EventArgs e)
        {
            DropInRequest dropInRequest = new DropInRequest()
                  .ClientToken(mAuthorization)
                  .Amount("1.00")
                  .RequestThreeDSecureVerification(Settings.isThreeDSecureEnabled(this))
                  .CollectDeviceData(Settings.ShouldCollectDeviceData(this))
                  .AndroidPayCart(getAndroidPayCart())
                  .AndroidPayShippingAddressRequired(Settings.IsAndroidPayShippingAddressRequired(this))
                  .AndroidPayPhoneNumberRequired(Settings.IsAndroidPayPhoneNumberRequired(this))
                  .AndroidPayAllowedCountriesForShipping(Settings.GetAndroidPayAllowedCountriesForShipping(this));

            if (Settings.isPayPalAddressScopeRequested(this))
            {
                dropInRequest.PaypalAdditionalScopes(new List<string> { PayPal.ScopeAddress });
            }

            StartActivityForResult(dropInRequest.GetIntent(this), DROP_IN_REQUEST);
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (mPurchased)
            {
                mPurchased = false;
                clearNonce();

                try
                {
                    if (ClientToken.FromString(mAuthorization) is ClientToken)
                    {
                        DropInResult.FetchDropInResult(this, mAuthorization, this);
                    }
                    else
                    {
                        mAddPaymentMethodButton.Visibility = Android.Views.ViewStates.Visible;
                    }
                }
                catch (InvalidArgumentException e)
                {
                    mAddPaymentMethodButton.Visibility = Android.Views.ViewStates.Visible;
                }
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            if (mNonce != null)
            {
                outState.PutParcelable(KEY_NONCE, mNonce);
            }
        }
        public override void OnPaymentMethodNonceCreated(PaymentMethodNonce paymentMethodNonce)
        {
            System.Diagnostics.Debug.WriteLine("Payment Method Nonce received: " + paymentMethodNonce.TypeLabel);
        }

        public override void OnCancel(int requestCode)
        {
            System.Diagnostics.Debug.WriteLine("Cancel received: " + requestCode);
        }

        public override void OnError(Java.Lang.Exception error)
        {
            System.Diagnostics.Debug.WriteLine("Error received (" + error.GetType() + "): " + error.Message);
            //mLogger.debug(error.toString());

            ShowDialog("An error occurred ");
        }

        [ExportAttribute("purchase")]
        public void Purchase(View v)
        {
            if (mPaymentMethodType == PaymentMethodType.AndroidPay && mNonce == null)
            {
                List<Android.Gms.Identity.Intents.Model.CountrySpecification> countries = new List<Android.Gms.Identity.Intents.Model.CountrySpecification>();
                foreach (string countryCode in Settings.GetAndroidPayAllowedCountriesForShipping(this))
                {
                    countries.Add(new Android.Gms.Identity.Intents.Model.CountrySpecification(countryCode));
                }

                mShouldMakePurchase = true;

                AndroidPay.RequestAndroidPay(mBraintreeFragment, getAndroidPayCart(),
                        Settings.IsAndroidPayShippingAddressRequired(this),
                        Settings.IsAndroidPayPhoneNumberRequired(this), countries);
            }
            else
            {
                Intent intent = new Intent(this, typeof(CreateTransactionActivity))
                    .PutExtra("nonce", mNonce);
                StartActivity(intent);

                mPurchased = true;
            }
        }


        [ExportAttribute("launchDropIn")]
        public void launchDropIn(View v)
        {
            MAddPaymentMethodButton_Click(v, EventArgs.Empty);
        }

        public void OnResult(DropInResult result)
        {
            if (result.PaymentMethodType == null)
            {
                mAddPaymentMethodButton.Visibility = Android.Views.ViewStates.Visible;
            }
            else
            {
                mAddPaymentMethodButton.Visibility = Android.Views.ViewStates.Gone;

                mPaymentMethodType = result.PaymentMethodType;

                mPaymentMethodIcon.SetImageResource(result.PaymentMethodType.Drawable);
                if (result.PaymentMethodNonce != null)
                {
                    DisplayResult(result.PaymentMethodNonce, result.DeviceData);
                }
                else if (result.PaymentMethodType == PaymentMethodType.AndroidPay)
                {
                    mPaymentMethodTitle.SetText(PaymentMethodType.AndroidPay.LocalizedName);
                    mPaymentMethodDescription.Text = "";
                    mPaymentMethod.Visibility = Android.Views.ViewStates.Visible;
                }

                mPurchaseButton.Enabled = true;
            }

        }

        private void clearNonce()
        {
            mPaymentMethod.Visibility = Android.Views.ViewStates.Gone;
            mNonceString.Visibility = Android.Views.ViewStates.Gone;
            mNonceDetails.Visibility = Android.Views.ViewStates.Gone;
            mDeviceData.Visibility = Android.Views.ViewStates.Gone;
            mPurchaseButton.Enabled = false;
        }

        private string formatAddress(PostalAddress address)
        {
            return address.RecipientName + " " + address.StreetAddress + " " +
                address.ExtendedAddress + " " + address.Locality + " " + address.Region +
                    " " + address.PostalCode + " " + address.CountryCodeAlpha2;
        }

        private string formatAddress(UserAddress address)
        {
            if (address == null)
            {
                return "null";
            }
            return address.Name + " " + address.Address1 + " " + address.Address2 + " " +
                    address.Address3 + " " + address.Address4 + " " + address.Address5 + " " +
                    address.Locality + " " + address.AdministrativeArea + " " + address.PostalCode + " " +
                    address.SortingCode + " " + address.CountryCode;
        }
        private Cart getAndroidPayCart()
        {
            return Cart.NewBuilder()
                    .SetCurrencyCode(Settings.GetAndroidPayCurrency(this))
                    .SetTotalPrice("1.00")
                    .AddLineItem(LineItem.NewBuilder()
                            .SetCurrencyCode("USD")
                            .SetDescription("Description")
                            .SetQuantity("1")
                            .SetUnitPrice("1.00")
                            .SetTotalPrice("1.00")
                            .Build())
                    .Build();
        }

        protected override void OnAuthorizationFetched()
        {
            try
            {
                mBraintreeFragment = BraintreeFragment.NewInstance(this, mAuthorization);

                if (ClientToken.FromString(mAuthorization) is ClientToken)
                {
                    DropInResult.FetchDropInResult(this, mAuthorization, this);
                }
                else
                {
                    mAddPaymentMethodButton.Visibility = ViewStates.Visible;
                }
            }
            catch (InvalidArgumentException e)
            {
                ShowDialog(e.Message);
            }
        }

        protected override void Reset()
        {
            mPurchaseButton.Enabled = false;

            mAddPaymentMethodButton.Visibility = ViewStates.Gone;

            clearNonce();
        }

        private void DisplayResult(PaymentMethodNonce paymentMethodNonce, string deviceData)
        {
            mNonce = paymentMethodNonce;
            mPaymentMethodType = PaymentMethodType.ForType(mNonce);

            mPaymentMethodIcon.SetImageResource(PaymentMethodType.ForType(mNonce).Drawable);
            mPaymentMethodTitle.Text = paymentMethodNonce.TypeLabel;
            mPaymentMethodDescription.Text = paymentMethodNonce.Description;
            mPaymentMethod.Visibility = Android.Views.ViewStates.Visible;

            mNonceString.Text = GetString(Resource.String.nonce) + ": " + mNonce.Nonce;
            mNonceString.Visibility = Android.Views.ViewStates.Visible;

            string details = "";
            if (mNonce is CardNonce)
            {
                CardNonce cardNonce = (CardNonce)mNonce;

                details = "Card Last Two: " + cardNonce.LastTwo + "\n";
                details += "3DS isLiabilityShifted: " + cardNonce.ThreeDSecureInfo.IsLiabilityShifted + "\n";
                details += "3DS isLiabilityShiftPossible: " + cardNonce.ThreeDSecureInfo.IsLiabilityShiftPossible;
            }
            else if (mNonce is PayPalAccountNonce)
            {
                PayPalAccountNonce paypalAccountNonce = (PayPalAccountNonce)mNonce;

                details = "First name: " + paypalAccountNonce.FirstName + "\n";
                details += "Last name: " + paypalAccountNonce.LastName + "\n";
                details += "Email: " + paypalAccountNonce.Email + "\n";
                details += "Phone: " + paypalAccountNonce.Phone + "\n";
                details += "Payer id: " + paypalAccountNonce.PayerId + "\n";
                details += "Client metadata id: " + paypalAccountNonce.ClientMetadataId + "\n";
                details += "Billing address: " + formatAddress(paypalAccountNonce.BillingAddress) + "\n";
                details += "Shipping address: " + formatAddress(paypalAccountNonce.ShippingAddress);
            }
            else if (mNonce is AndroidPayCardNonce)
            {
                AndroidPayCardNonce androidPayCardNonce = (AndroidPayCardNonce)mNonce;

                details = "Underlying Card Last Two: " + androidPayCardNonce.LastTwo + "\n";
                details += "Email: " + androidPayCardNonce.Email + "\n";
                details += "Billing address: " + formatAddress(androidPayCardNonce.BillingAddress) + "\n";
                details += "Shipping address: " + formatAddress(androidPayCardNonce.ShippingAddress);
            }
            else if (mNonce is VenmoAccountNonce)
            {
                VenmoAccountNonce venmoAccountNonce = (VenmoAccountNonce)mNonce;

                details = "Username: " + venmoAccountNonce.Username;
            }

            mNonceDetails.Text = details;
            mNonceDetails.Visibility = Android.Views.ViewStates.Visible;

            mDeviceData.Text = "Device Data: " + deviceData;
            mDeviceData.Visibility = Android.Views.ViewStates.Visible;

            mAddPaymentMethodButton.Visibility = Android.Views.ViewStates.Gone;
            mPurchaseButton.Enabled = true;
        }

        void SafelyCloseLoadingView()
        {
            if (mLoading != null && mLoading.IsShowing)
            {
                mLoading.Dismiss();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            SafelyCloseLoadingView();

            if (resultCode == Result.Ok)
            {
                DropInResult result = (DropInResult)data.GetParcelableExtra(DropInResult.ExtraDropInResult);
                DisplayResult(result.PaymentMethodNonce, result.DeviceData);
                mPurchaseButton.Enabled = (true);
            }
            else if (resultCode != Result.Canceled)
            {
                SafelyCloseLoadingView();
                var error = data.GetSerializableExtra(DropInActivity.ExtraError);

                ShowDialog(((Java.Lang.Exception)error)
                        .Message);
            }
        }

    }
}

