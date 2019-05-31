using System;
using System.Collections.Generic;
using System.Text;

using LearnXamarin.DB;

namespace LearnXamarin.Models
{
    public interface ITask : IDbParsable, IComparable
    {
        int id { get; set; }
        string text { get; set; }
        int lesson_id { get; set; }
        int task_num { get; set; }
    }
}
