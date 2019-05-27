using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace LearnXamarin.DB
{
    public class DbContext
    {
        private const string QUERY_PATH = "http://learnxamarin.zzz.com.ua/query_select.php";
        private static UTF8Encoding UTF8 = new UTF8Encoding();
        private const string HASH = "27052019";

        public static List<T> Select<T>() where T : IDbParsable, new()
        {
            string sqlQuery = $"SELECT * FROM {new T().TableName}";
            string query = $"hash={HASH}&query={sqlQuery}";
            byte[] queryData = UTF8.GetBytes(query);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(QUERY_PATH);
            request.Method = WebRequestMethods.Http.Post;

            request.Headers.Add(HttpRequestHeader.ContentLength, queryData.Length.ToString());
            request.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");

            using (Stream str = request.GetRequestStream())
                str.Write(queryData, 0, queryData.Length);

            string JSONResponse = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader str = new StreamReader(response.GetResponseStream()))
                JSONResponse = str.ReadToEnd();

            return JsonConvert.DeserializeObject<List<T>>(JSONResponse);
        }
    }
}
