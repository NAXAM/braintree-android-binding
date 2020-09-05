using System;
using Android.Content;
using Android.Preferences;

namespace BraintreeDropInQs
{
    public class Settings
    {
        protected static String ENVIRONMENT = "environment";

        static String VERSION = "version";

        static String SANDBOX_BASE_SERVER_URL = "https://braintree-sample-merchant.herokuapp.com";
        static String PRODUCTION_BASE_SERVER_URL = "https://executive-sample-merchant.herokuapp.com";

        static String SANDBOX_TOKENIZATION_KEY = "sandbox_tmxhyf7d_dcpspy2brwdjr3qn";
        static String PRODUCTION_TOKENIZATION_KEY = "production_t2wns2y2_dfy45jdj3dxkmz5m";
        static ISharedPreferences sSharedPreferences;

        public static ISharedPreferences getPreferences(Context context)
        {
            if (sSharedPreferences == null)
            {
                sSharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context.ApplicationContext);
            }

            return sSharedPreferences;
        }

        public static int getVersion(Context context)
        {
            return getPreferences(context).GetInt(VERSION, 0);
        }

        public static void setVersion(Context context)
        {
            getPreferences(context).Edit().PutInt(VERSION, BuildConfig.VERSION_CODE).Apply();
        }

        public static int getEnvironment(Context context)
        {
            return getPreferences(context).GetInt(ENVIRONMENT, 0);
        }

        public static void SetEnvironment(Context context, int environment)
        {
            getPreferences(context)
                    .Edit()
                    .PutInt(ENVIRONMENT, environment)
                    .Apply();

            DemoApplication.ResetApiClient();

        }

        public static String GetSandboxUrl()
        {
            return SANDBOX_BASE_SERVER_URL;
        }

        public static String GetEnvironmentUrl(Context context)
        {
            int environment = getEnvironment(context);
            if (environment == 0)
            {
                return SANDBOX_BASE_SERVER_URL;
            }
            else if (environment == 1)
            {
                return PRODUCTION_BASE_SERVER_URL;
            }
            else
            {
                return "";
            }
        }

        public static String GetCustomerId(Context context)
        {
            return getPreferences(context).GetString("customer", null);
        }

        public static String GetMerchantAccountId(Context context)
        {
            return getPreferences(context).GetString("merchant_account", null);
        }

        public static bool ShouldCollectDeviceData(Context context)
        {
            return getPreferences(context).GetBoolean("collect_device_data", false);
        }

        public static String GetThreeDSecureMerchantAccountId(Context context)
        {
            if (isThreeDSecureEnabled(context) && getEnvironment(context) == 1)
            {
                return "test_AIB";
            }
            else
            {
                return null;
            }
        }

        public static String GetUnionPayMerchantAccountId(Context context)
        {
            if (getEnvironment(context) == 0)
            {
                return "fake_switch_usd";
            }
            else
            {
                return null;
            }
        }

        public static bool UseTokenizationKey(Context context)
        {
            return getPreferences(context).GetBoolean("tokenization_key", false);
        }

        public static String GetEnvironmentTokenizationKey(Context context)
        {
            int environment = getEnvironment(context);
            if (environment == 0)
            {
                return SANDBOX_TOKENIZATION_KEY;
            }
            else if (environment == 1)
            {
                return PRODUCTION_TOKENIZATION_KEY;
            }
            else
            {
                return "";
            }
        }

        public static bool IsAndroidPayShippingAddressRequired(Context context)
        {
            return getPreferences(context).GetBoolean("android_pay_require_shipping_address", false);
        }

        public static bool IsAndroidPayPhoneNumberRequired(Context context)
        {
            return getPreferences(context).GetBoolean("android_pay_require_phone_number", false);
        }

        public static String GetAndroidPayCurrency(Context context)
        {
            return getPreferences(context).GetString("android_pay_currency", "USD");
        }

        public static String[] GetAndroidPayAllowedCountriesForShipping(Context context)
        {
            String[] countries = getPreferences(context).GetString("android_pay_allowed_countries_for_shipping", "US").Split(",".ToCharArray());
            for (int i = 0; i < countries.Length; i++)
            {
                countries[i] = countries[i].Trim();
            }

            return countries;
        }

        public static String getPayPalPaymentType(Context context)
        {
            return getPreferences(context).GetString("paypal_payment_type", context.GetString(Resource.String.paypal_billing_agreement));
        }

        public static bool isPayPalAddressScopeRequested(Context context)
        {
            return getPreferences(context).GetBoolean("paypal_request_address_scope", false);
        }

        public static bool isPayPalSignatureVerificationDisabled(Context context)
        {
            return getPreferences(context).GetBoolean("paypal_disable_signature_verification", true);
        }

        public static bool useHardcodedPayPalConfiguration(Context context)
        {
            return getPreferences(context).GetBoolean("paypal_use_hardcoded_configuration", false);
        }

        public static bool isThreeDSecureEnabled(Context context)
        {
            return getPreferences(context).GetBoolean("enable_three_d_secure", false);
        }

        public static bool isThreeDSecureRequired(Context context)
        {
            return getPreferences(context).GetBoolean("require_three_d_secure", true);
        }


    }
}