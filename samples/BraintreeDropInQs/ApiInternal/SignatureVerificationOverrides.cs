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
using Com.Braintreepayments.Api.Internal;

namespace BraintreeDropInQs.ApiInternal
{
    public class SignatureVerificationOverrides
    {
        /**
     * WARNING: signature verification is disable based on a setting for testing in this demo app only. You should
     * never do this as it opens a security hole.
     */
        public static void disableAppSwitchSignatureVerification(bool disable)
        {
            SignatureVerification.SEnableSignatureVerification = !disable;

        }


    }
}