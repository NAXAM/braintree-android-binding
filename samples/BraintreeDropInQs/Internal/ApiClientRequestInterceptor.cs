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
using Square.Retrofit;

namespace BraintreeDropInQs.Internal
{
    public class ApiClientRequestInterceptor :Java.Lang.Object, IRequestInterceptor
    {
        public void Intercept(IRequestInterceptorRequestFacade request)
        {
            request.AddHeader("User-Agent", "braintree/android-demo-app/" + BuildConfig.VERSION_NAME);
            request.AddHeader("Accept", "application/json");
        }
    }
}