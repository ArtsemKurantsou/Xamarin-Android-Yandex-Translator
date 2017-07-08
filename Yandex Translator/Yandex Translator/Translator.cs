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
using OkHttpClient;
using System.Net;
using System.IO;

namespace Yandex_Translator
{
    class Translator
    {
        private static string API_KEY = "trnsl.1.1.20170708T113101Z.abe19304b3f99709.15651973473318e10a1b477508c0e582c3563cb4";
        private static string API_URL = "https://translate.yandex.net/api/v1.5/tr.json/translate?key={0}&text={1}&lang={2}";
        private static string EN_LNG = "ru-en";

        public static string Translate(string ruText)
        {
            var request = HttpWebRequest.Create(string.Format(API_URL, API_KEY, ruText, EN_LNG));
            request.ContentType = "application/json";
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response.StatusCode != HttpStatusCode.OK)
                return "Error";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                var content = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return "";
                }
                else
                {
                    var answer = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslatorAnswer>(content);
                    return answer.Text[0];
                }
                
            }
        }
    }
}