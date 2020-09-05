using GoogleGson.Annotations;

namespace BraintreeDropInQs.Models
{
    public class Transaction: Java.Lang.Object
    {
        [SerializedName(Value = "message")]
        public string Message { get; set; }
    }
}