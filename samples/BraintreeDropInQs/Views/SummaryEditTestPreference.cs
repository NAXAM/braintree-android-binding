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
using Android.Util;
using Java.Lang;

namespace BraintreeDropInQs.Views
{
    public class SummaryEditTestPreference : EditTextPreference
    {
        private  string mSummaryString;

        public SummaryEditTestPreference(Context context) : base(context)
        {
            init();

        }

        public SummaryEditTestPreference(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            init();

        }

        public SummaryEditTestPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            init();

        }

        public SummaryEditTestPreference(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            init();

        }

        protected SummaryEditTestPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            init();
        }

        private void init()
        {
            mSummaryString = base.SummaryFormatted.ToString();
        }

        public override ICharSequence SummaryFormatted
        {
            get
            {
                return new Java.Lang.String( string.Format((string)mSummaryString, Text == null ? "" : Text));
            }
            set => base.SummaryFormatted = value;
        }
    }
}