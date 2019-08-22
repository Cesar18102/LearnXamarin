using System;
using System.Collections.Generic;
using System.Text;
using LearnXamarin.DB;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Linq;

using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LearnXamarin.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class User : IDbParsable
    {
        [JsonProperty]
        public int id { get; set; }

        [JsonProperty]
        public string login { get; set; }

        [JsonProperty]
        public string password_md5 { get; set; }

        [JsonProperty]
        public string email { get; set; }

        private static Regex LoginPattern = new Regex("^[0-9a-zA-Z]{3,20}$");
        private static Regex PasswordPattern = new Regex("^[0-9a-zA-Z]{8,12}$");
        private static Regex EmailPattern = new Regex("^[0-9a-zA-Z_\\.]+@[a-z0-9\\-\\.]+$");

        public static readonly Predicate<string> LoginValidator = new Predicate<string>(data => data != null && LoginPattern.IsMatch(data));
        public static readonly Predicate<string> PasswordValidator = new Predicate<string>(data => data != null && PasswordPattern.IsMatch(data));
        public static readonly Predicate<string> EmailValidator = new Predicate<string>(data => data != null && EmailPattern.IsMatch(data));

        private bool PassedTasksLoaded = false; 
        private Dictionary<Lesson, List<LessonTask>> PassedTasks = new Dictionary<Lesson, List<LessonTask>>();
        public bool IsTaskPassed(Lesson L, LessonTask LT) => PassedTasks[L].Contains(LT);

        public async Task ChangePassword(string NewPasswordMD5)
        {
            password_md5 = NewPasswordMD5;
            await DbContext.ChangePassword(this, NewPasswordMD5);
        }

        public async void Pass(Lesson L, LessonTask LT)
        {
            if (IsTaskPassed(L, LT))
                return;

            PassedTasks[L].Add(LT);
            await DbContext.InsertAsync<PassedTask>(new PassedTask(id, L.id, LT.task_num));
        }

        public async void LoadPassedTasks()
        {
            if (PassedTasksLoaded)
                return;

            PassedTasksLoaded = true;

            foreach (Lesson L in App.Lessons)
                PassedTasks.Add(L, new List<LessonTask>());

            List<PassedTask> PTL = await DbContext.SelectWhereAsync<PassedTask>("user_id", id.ToString());
            List<IGrouping<int, PassedTask>> PTGS = PTL.GroupBy(T => T.lesson_id).ToList();

            foreach(IGrouping<int, PassedTask> PTG in PTGS)
            {
                Lesson LSN = App.Lessons.Single(L => L.id == PTG.Key);
                List<int> PassedTasksNums = PTG.Select(G => G.task_num).ToList();
                PassedTasks[LSN] = LSN.Where(LT => PassedTasksNums.Contains(LT.task_num)).ToList();
            }
        }

        public class PassedTask : IDbParsable
        {
            public int id { get; set; }
            public int user_id { get; set; }
            public int lesson_id { get; set; }
            public int task_num { get; set; }

            public PassedTask() { }
            public PassedTask(int UserId, int LessonId, int TaskNum, int Id = Constants.IdDefault)
            {
                this.id = Id;
                this.user_id = UserId;
                this.lesson_id = LessonId;
                this.task_num = TaskNum;
            }

            public string TableName { get { return "User_Task"; } }
            public string IdFieldName { get { return "id"; } }
            public Dictionary<string, object> Fields
            {
                get
                {
                    Dictionary<string, object> FS = new Dictionary<string, object>();

                    FS.Add(IdFieldName, id);
                    FS.Add("user_id", user_id);
                    FS.Add("lesson_id", lesson_id);
                    FS.Add("task_num", task_num);

                    return FS;
                }
            }
        } 

        public User() { }
        public User(string Login, string Password, string Email, int Id = Constants.IdDefault)
        {
            this.login = Login;
            this.password_md5 = Constants.Hash(Password, Constants.UTF8, Constants.mD5);
            this.email = Email;
            this.id = Id;
        }

        public string TableName { get { return "Users"; } }
        public string IdFieldName { get { return "id"; } }

        public Dictionary<string, object> Fields
        {
            get
            {
                Dictionary<string, object> FS = new Dictionary<string, object>();

                FS.Add(IdFieldName, id);
                FS.Add("login", login);
                FS.Add("password_md5", password_md5);
                FS.Add("email", email);

                return FS;
            }
        }
    }
}
