using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public class Test : LessonTask
    {
        private List<Variant> Variants = new List<Variant>();

        public int VariantsCount { get { return Variants.Count; } }
        public int RightVariantsCount { get { return Variants.Sum(V => V.is_right ? 1 : 0); } }
        public Variant this[int i] { get { return i >= 0 && i < Variants.Count ? Variants[i] : null; } }
        public void AddVariants(IEnumerable<Variant> VS) { Variants.AddRange(VS); }

        public Test() { }

        public override string TableName { get { return "Test"; } }
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
