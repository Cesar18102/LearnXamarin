using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class Theory : LessonTask
    {
        public Theory() { }

        public override string TableName { get { return "Theory"; } }
        public override string IdFieldName { get { return "id"; } }

        public override Dictionary<string, object> Fields
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
