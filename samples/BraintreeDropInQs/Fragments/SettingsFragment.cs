using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;
using BraintreeDropInQs.Views;

namespace BraintreeDropInQs.fragments
{
    public class SettingsFragment : PreferenceFragment, ISharedPreferencesOnSharedPreferenceChangeListener
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AddPreferencesFromResource(Resource.Xml.settings);

            ISharedPreferences preferences = PreferenceManager.SharedPreferences;
            OnSharedPreferenceChanged(preferences, "paypal_payment_type");
            OnSharedPreferenceChanged(preferences, "android_pay_currency");
            OnSharedPreferenceChanged(preferences, "android_pay_allowed_countries_for_shipping");
            preferences.RegisterOnSharedPreferenceChangeListener(this);
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            PreferenceManager.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
        }
        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            Preference preference = FindPreference(key);
            if (preference.GetType() == typeof(ListPreference))
            {
                preference.Summary = ((ListPreference)preference).Entry;
            }
            else if (preference.GetType() == typeof(SummaryEditTestPreference))
            {
                preference.Summary = preference.Summary;
            }

        }
        }
    }
