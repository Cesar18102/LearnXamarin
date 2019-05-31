using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class Theory : ITask
    {
        public int id { get; set; }
        public string text { get; set; }
        public int lesson_id { get; set; }
        public int task_num { get; set; }

        public Theory() { }

        public string TableName { get { return "Theory"; } }
        public string IdFieldName { get { return "id"; } }

        public Dictionary<string, object> Fields
        {
            get
            {
                Dictionary<string, object> FS = new Dictionary<string, object>();

                FS.Add("id", id);
                FS.Add("text", text);
                FS.Add("lesson_id", lesson_id);
                FS.Add("task_num", task_num);

                return FS;
            }
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
