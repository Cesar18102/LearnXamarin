using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class Lesson : IDbParsable
    {
        public int id { get; set; }
        public string title { get; set; }

        private List<ITask> Tasks = new List<ITask>();

        public int TasksCount { get { return Tasks.Count; } }
        public void AddTasks(IEnumerable<ITask> TS) { Tasks.AddRange(TS); /*Tasks.Sort();*/ }
        public ITask this[int i] { get { return i >= 0 && i < Tasks.Count ? Tasks[i] : null; } }

        public Lesson() { }

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
