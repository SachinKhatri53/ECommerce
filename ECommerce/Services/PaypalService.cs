using ECommerce.Services;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;

public class PaypalService : IPaypalService
{
    private const bool IsSandbox = true;
    private const string ApiUsername = "sb-wu6tr29496800_api1.business.example.com";
    private const string ApiPassword = "ZRVWXNKD3WCY596Q";
    private const string ApiSignature = "Az6lLldYrVA-5Z8QTDTRqSXxlicGAIZto9z6MFHHP.Xn48zVE3Jor4Of";

    public string CreatePayment(decimal amount)
    {
        return GetRedirectUrl(amount);
    }

    public string ExecutePayment(string paymentID, string payerID)
    {
        return "Payment executed successfully!";
    }
    private string GetRedirectUrl(decimal amount)
    {
        /*string endpointUrl = IsSandbox ? "https://api-3t.sandbox.paypal.com/nvp" : "https://api-3t.paypal.com/nvp";*/
        string endpointUrl = "https://api-3t.sandbox.paypal.com/nvp";

        var encoder = new NameValueCollection
        {
            ["METHOD"] = "SetExpressCheckout",
            ["USER"] = ApiUsername,
            ["PWD"] = ApiPassword,
            ["SIGNATURE"] = ApiSignature,
            ["VERSION"] = "88.0",
            ["RETURNURL"] = "https://localhost:7178/Checkout/Success", // Replace with your success URL
            ["CANCELURL"] = "https://localhost:7178/Checkout/Cancel"   // Replace with your cancel URL
        };

        // Add payment details
        encoder["PAYMENTREQUEST_0_AMT"] = amount.ToString("0.00");
        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "USD";

        string requestString = Encode(encoder);
        string responseString = HttpCall(endpointUrl, requestString);

        // Parse the response and extract the redirect URL
        var responseParams = HttpUtility.ParseQueryString(responseString);
        return $"https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token={responseParams["TOKEN"]}";
    }
    private string Encode(NameValueCollection nvp)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string key in nvp.AllKeys)
        {
            sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvp[key]));
        }
        return sb.ToString().TrimEnd('&');
    }
    private string HttpCall(string url, string requestString)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                return client.UploadString(url, "POST", requestString);
            }
        }
        catch (WebException ex)
        {
            Console.WriteLine($"WebException: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error making HTTP call: {ex.Message}");
            return null;
        }
    }
    /*public string GetTransactionDetails(string transactionId)
    {
        string endpointUrl = "https://api-3t.sandbox.paypal.com/nvp";

        var encoder = new NameValueCollection
        {
            ["METHOD"] = "GetTransactionDetails",
            ["USER"] = ApiUsername,
            ["PWD"] = ApiPassword,
            ["SIGNATURE"] = ApiSignature,
            ["VERSION"] = "88.0",
            ["TRANSACTIONID"] = transactionId
        };

        string requestString = Encode(encoder);
        string responseString = HttpCall(endpointUrl, requestString);

        return responseString;
    }*/
}
