
//using PayPalHelper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace _Helper
//{
//    public static class PayPalConfiguration
//    {
//        public readonly static string ClientId;
//        public readonly static string ClientSecret;

//        // Static constructor for setting the readonly static members.
//        static PayPalConfiguration()
//        {
            
//            ClientId = "AdV5SPIOgJFToJ1OWozyv3-0GBNw0_z3equN0hkpWxl_U3Og3TJADlbowSD_8r52ZEtyDGEMcJI45USm";
//            ClientSecret = "EOvoG08OPwqIfmcoNJcrt0NVkBxIhbnc9kAWAC4JJptx7pABYsw3hTpDBe9PPJQLJwUbsS9IN3ZlC8C_";
//        }

//        // Create the configuration map that contains mode and other optional configuration details.
//        public static Dictionary<string, string> GetConfig()
//        {
//            return ConfigManager.Instance.GetProperties();
//        }

//        // Create accessToken
//        private static string GetAccessToken()
//        {
//            // ###AccessToken
//            // Retrieve the access token from
//            // OAuthTokenCredential by passing in
//            // ClientID and ClientSecret
//            // It is not mandatory to generate Access Token on a per call basis.
//            // Typically the access token can be generated once and
//            // reused within the expiry window                
//            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
//            return accessToken;
//        }

//        // Returns APIContext object
//        public static APIContext GetAPIContext(string accessToken = "")
//        {
//            // ### Api Context
//            // Pass in a `APIContext` object to authenticate 
//            // the call and to send a unique request id 
//            // (that ensures idempotency). The SDK generates
//            // a request id if you do not pass one explicitly. 
//            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken);
//            apiContext.Config = GetConfig();

//            // Use this variant if you want to pass in a request id  
//            // that is meaningful in your application, ideally 
//            // a order id.
//            // String requestId = Long.toString(System.nanoTime();
//            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

//            return apiContext;
//        }

//    }
//}