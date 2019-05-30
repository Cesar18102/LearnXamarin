using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class Theory : ITask
    {
        public Int32 id { get; set; }
        public String text { get; set; }
        public Int32 lesson_id { get; set; }
        public Int32 task_num { get; set; }

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

    }
}
