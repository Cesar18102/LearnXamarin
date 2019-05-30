using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class Lesson : IDbParsable
    {
        public Int32 id { get; set; }
        public String title { get; set; }

        public List<ITask> Tasks = new List<ITask>();

        public Lesson() { }

        public void LoadTasks()
        {
            
        }

        public string TableName { get { return "Lesson"; } }
        public string IdFieldName { get { return "id"; } }
        public Dictionary<string, object> Fields
        {
            get
            {
                Dictionary<string, object> FS = new Dictionary<string, object>();

                FS.Add(IdFieldName, id);
                FS.Add("title", title);

                return FS;
            }
        }
    }
}
