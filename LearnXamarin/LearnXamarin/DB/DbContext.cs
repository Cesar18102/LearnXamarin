using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace LearnXamarin.DB
{
    public class DbContext
    {
        private const string QUERY_PATH = "http://learnxamarin.zzz.com.ua/query_select.php";
        private const string Code = "27052019";

        /// <summary>
        /// Selects all records from spcified table
        /// </summary>
        private static List<T> Select<T>() where T : IDbParsable, new()
        {
            string sqlQuery = $"SELECT * FROM {new T().TableName}";
            string JSONResponse = SendQuery(sqlQuery, QUERY_PATH);

            return JsonConvert.DeserializeObject<List<T>>(JSONResponse);
        }

        /// <summary>
        /// Async version of simple select
        /// </summary>
        public static async Task<List<T>> SelectAsync<T>() where T : IDbParsable, new()
        {
            return await Task.Run(() => Select<T>());
        }

        //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Selects records from specified table according to some constraints
        /// </summary>
        private static List<T> Select<T>(params Predicate<T>[] PS) where T : IDbParsable, new()
        {
            List<T> All = Select<T>();
            foreach (Predicate<T> P in PS)
                All = All.Where(item => P(item)).ToList();
            return All;
        }

        /// <summary>
        /// Async version of select with constraints
        /// </summary>
        public static async Task<List<T>> SelectAsync<T>(params Predicate<T>[] PS) where T : IDbParsable, new()
        {
            return await Task.Run(() => Select<T>(PS));
        }

        //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Asynchronously selects single record from specified table according to some constraints
        /// </summary>
        public static async Task<T> SingleAsync<T>(params Predicate<T>[] PS) where T : IDbParsable, new()
        {
            return await Task.Run(() => { 

                List<T> R = Select<T>(PS);
                return R.Count == 0 ? new T() : R.First();
            });
        }

        //////////////////////////////////////////////////////////////////////////////////////
        
        private static List<T> SelectWhere<T>(string FieldName, string FieldValue) where T : IDbParsable, new()
        {
            string sqlQuery = $"SELECT * FROM {new T().TableName} WHERE {FieldName}={FieldValue}";
            string JSONResponse = SendQuery(sqlQuery, QUERY_PATH);

            return JsonConvert.DeserializeObject<List<T>>(JSONResponse);
        }

        public static async Task<List<T>> SelectWhereAsync<T>(string FieldName, string FieldValue) where T : IDbParsable, new()
        {
            return await Task.Run(() => SelectWhere<T>(FieldName, FieldValue));
        }

        //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Inserts a new record to a specified table
        /// </summary>
        private static void Insert<T>(T item) where T : IDbParsable, new()
        {
            string sqlQuery = $"INSERT INTO {item.TableName} VALUES ({string.Join(", ", item.Fields.Values.Select(O => "'" + O.ToString() + "'").ToList())})";
            SendQuery(sqlQuery, QUERY_PATH);
        }

        /// <summary>
        /// Async version of insert
        /// </summary>
        public static async Task InsertAsync<T>(T item) where T : IDbParsable, new()
        {
            await Task.Run(() => Insert<T>(item));
        }

        //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Http request sending method
        /// </summary>
        private static string SendQuery(string sqlQuery, string path)
        {
            try
            {
                string Rand = Constants.R.Next(1000, 9999).ToString();
                string Hash = Constants.Hash(Code + Rand, Constants.UTF8, Constants.mD5);

                string query = $"hash={Hash}&rand={Rand}&query={sqlQuery}";
                byte[] queryData = Constants.UTF8.GetBytes(query);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(path);

                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = queryData.Length;

                using (Stream str = request.GetRequestStream())
                    str.Write(queryData, 0, queryData.Length);

                string JSONResponse = "";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader str = new StreamReader(response.GetResponseStream()))
                    JSONResponse = str.ReadToEnd();

                return JSONResponse.Replace("\t", "");
            }
            catch
            {
                return "[ { } ]";
            }
        }
    }
}
