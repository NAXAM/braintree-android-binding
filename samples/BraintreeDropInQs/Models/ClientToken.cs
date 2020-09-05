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
using GoogleGson.Annotations;
using Newtonsoft.Json;

namespace BraintreeDropInQs.Models
{
    public class ClientToken: Java.Lang.Object
    {
        [SerializedName(Value = "client_token")]
        [JsonProperty("client_token")]
        public string Token { get; set; }
    }
}