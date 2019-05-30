using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public interface ITask : IDbParsable
    {
        Int32 id { get; set; }
        String text { get; set; }
        Int32 lesson_id { get; set; }
        Int32 task_num { get; set; }
    }
}
