using System;
using System.Collections.Generic;
using Android.Runtime;
using Java.Interop;

namespace Com.Braintreepayments.Api.Dropin.Adapters {
    public partial class VaultManagerPaymentMethodsAdapter  {
        public override unsafe global::AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder OnCreateViewHolder (global::Android.Views.ViewGroup parent, int viewType)
		{
            return OnCreateVaultManagerPaymentMethodsAdapterViewHolder(parent, viewType);
        }
    }
}