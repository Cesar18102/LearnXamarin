using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

using Newtonsoft.Json;

namespace LearnXamarin.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class LessonTask : IDbParsable, IComparable
    {
        public abstract string TableName { get; }
        public abstract string IdFieldName { get; }
        public abstract Dictionary<string, object> Fields { get; }

        [JsonProperty]
        public int id { get; set; }

        [JsonProperty]
        public string text { get; set; }

        [JsonProperty]
        public int lesson_id { get; set; }

        [JsonProperty]
        public int task_num { get; set; }

        public int CompareTo(object obj)
        {
            Type TP = obj.GetType();
            if (!TP.Equals(typeof(LessonTask)) && !TP.Equals(typeof(Test)) && !TP.Equals(typeof(Theory)))
                return -1;

            LessonTask T = (LessonTask)obj;
            return task_num - T.task_num;
        }

        public override bool Equals(object obj)
        {
            Type TP = obj.GetType();
            if (!TP.Equals(typeof(LessonTask)) && !TP.Equals(typeof(Test)) && !TP.Equals(typeof(Theory)))
                return false;

            LessonTask T = (LessonTask)obj;
            return T.id == id && T.task_num == task_num;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
