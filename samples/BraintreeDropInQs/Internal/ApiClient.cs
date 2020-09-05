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
using Square.Retrofit.Http;
using Square.Retrofit.Client;
using Square.Retrofit;
using System.Net.Http;
using BraintreeDropInQs.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BraintreeDropInQs.Internal
{
    public interface ApiClient
    {
        [GET(Value = "/client_token")]
        Task<string> GetClientToken([Query(Value = "customer_id")] String customerId, [Query(Value = "merchant_account_id")] String merchantAccountId);

        [FormUrlEncoded]
        [POST(Value = "/nonce/transaction")]
        Task<Transaction> CreateTransaction([Field(Value = "nonce")] String nonce);

        [FormUrlEncoded]
        [POST(Value = "/nonce/transaction")]
        Task<Transaction> CreateTransaction([Field(Value = "nonce")] String nonce, [Field(Value = "merchant_account_id")] String merchantAccountId);

        [FormUrlEncoded]
        [POST(Value = "/nonce/transaction")]
        Task<Transaction> CreateTransaction([Field(Value = "nonce")] String nonce, [Field(Value = "merchant_account_id")] String merchantAccountId, [Field(Value = "three_d_secure_required")] bool requireThreeDSecure);
    }

    public class ApiClientImpl : ApiClient
    {
        readonly HttpClient client;

        public ApiClientImpl(string baseUrl)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }
        
        public async Task<Transaction> CreateTransaction([Field(Value = "nonce")] string nonce)
        {
            var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string> ("nonce", nonce)
            });

            var response = await client.PostAsync("/nonce/transaction", content);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<Transaction>(await response.Content.ReadAsStringAsync());

                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task<Transaction> CreateTransaction([Field(Value = "nonce")] string nonce, [Field(Value = "merchant_account_id")] string merchantAccountId)
        {
            var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string> ("nonce", nonce),
                new KeyValuePair<string, string> ("merchant_account_id", merchantAccountId)
            });

            var response = await client.PostAsync("/nonce/transaction", content);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<Transaction>(await response.Content.ReadAsStringAsync());

                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task<Transaction> CreateTransaction([Field(Value = "nonce")] string nonce, [Field(Value = "merchant_account_id")] string merchantAccountId, [Field(Value = "three_d_secure_required")] bool requireThreeDSecure)
        {
            var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string> ("nonce", nonce),
                new KeyValuePair<string, string> ("merchant_account_id", merchantAccountId),
                new KeyValuePair<string, string> ("three_d_secure_required", requireThreeDSecure.ToString().ToLower())
            });

            var response = await client.PostAsync("/nonce/transaction", content);

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<Transaction>(await response.Content.ReadAsStringAsync());

                return data;
            }
            else
            {
                return null;
            }
        }
        

        public async Task<string> GetClientToken([Query(Value = "customer_id")] string customerId, [Query(Value = "merchant_account_id")] string merchantAccountId)
        {
            var content = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string> ("customer_id", customerId),
                new KeyValuePair<string, string> ("merchant_account_id", merchantAccountId)
            };

            var response = await client.GetAsync("/client_token?" + string.Join("&", content.Select(x => $"{x.Key}={x.Value}")));

            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<ClientToken>(await response.Content.ReadAsStringAsync());

                return data.Token;
            }
            else
            {
                throw new Exception("Unable to get token.");
            }
        }
    }

}