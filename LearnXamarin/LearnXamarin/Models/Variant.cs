using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Variant : IDbParsable
    {
        [JsonProperty]
        public int id { get; set; }

        [JsonProperty]
        public string text { get; set; }

        [JsonProperty]
        public int test_id { get; set; }

        [JsonProperty]
        public bool is_right { get; set; }

        public Variant() { }

        public string TableName { get { return "Variants"; } }
        public string IdFieldName { get { return "id"; } }

        public Dictionary<string, object> Fields
        {
            get
            {
                Dictionary<string, object> FS = new Dictionary<string, object>();

                FS.Add("id", id);
                FS.Add("text", text);
                FS.Add("test_id", test_id);
                FS.Add("is_right", is_right);

                return FS;
            }
        }
    }
}
