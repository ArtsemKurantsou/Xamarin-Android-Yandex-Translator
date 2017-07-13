using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Yandex_Translator
{
    class Translator
    {
        private static string API_KEY = "trnsl.1.1.20170708T113101Z.abe19304b3f99709.15651973473318e10a1b477508c0e582c3563cb4";
        private static string API_URL = "https://translate.yandex.net/api/v1.5/tr.json/translate?";
        private static string EN_LNG = "ru-en";
        private static string RU_LNG = "en-ru";

        public static string Translate(string ruText, bool isRuToEng)
        {
            string query;
            using (var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]{
                new KeyValuePair<string, string>("key", API_KEY),
                new KeyValuePair<string, string>("text", ruText),
                new KeyValuePair<string, string>("lang", (isRuToEng ? EN_LNG : RU_LNG)),
            }))
            query = content.ReadAsStringAsync().Result;

            var request = HttpWebRequest.Create(API_URL + query);
            request.ContentType = "application/json";
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response.StatusCode != HttpStatusCode.OK)
                return "Error";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                var content = reader.ReadToEnd();
                if (string.IsNullOrEmpty(content))
                    return "";
                else
                {
                    var answer = Newtonsoft.Json.JsonConvert.DeserializeObject<TranslatorAnswer>(content);
                    return answer.Text[0];
                }

            }
        }
    }
}