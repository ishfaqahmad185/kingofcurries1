using Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace _Helper
{
    public class GoogleCaptchaService
    {

        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
                var root = builder.Build();
                var wowzaConfig = root.GetSection("GoogleCapchaSettings");
                string secretKey = wowzaConfig["secretKey"];
             
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}";

                using (var client = new HttpClient())
                {
                    var httpResult = await client.GetAsync(url);
                    if (httpResult.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }
                    string responseString = await httpResult.Content.ReadAsStringAsync();

                    var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                    return googleResult.success && googleResult.score >= 0.5;



                }
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}