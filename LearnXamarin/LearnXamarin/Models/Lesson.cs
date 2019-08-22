using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Lesson : IDbParsable, IEnumerable<LessonTask>
    {
        [JsonProperty]
        public int id { get; set; }

        [JsonProperty]
        public string title { get; set; }

        [JsonProperty]
        public bool is_exam { get; set; }

        private List<LessonTask> Tasks = new List<LessonTask>();
        public int TasksCount { get { return Tasks.Count; } }
        public void AddTasks(IEnumerable<LessonTask> TS) { Tasks.AddRange(TS); Tasks.Sort(); }        

        public LessonTask this[int num] { get { return Tasks.SingleOrDefault(T => T.task_num == num); } }

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

        public IEnumerator<LessonTask> GetEnumerator() => new Constants.Enumerator<LessonTask>(Tasks);
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(typeof(Lesson)))
                return false;

            return (obj as Lesson).id == id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
