using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class Variant : IDbParsable
    {
        public int id { get; set; }
        public string text { get; set; }
        public int test_id { get; set; }
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
