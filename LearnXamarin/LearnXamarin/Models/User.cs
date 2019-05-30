using System;
using System.Collections.Generic;
using System.Text;
using LearnXamarin.DB;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace LearnXamarin.Models
{
    public class User : IDbParsable
    {
        public Int32 id { get; set; }
        public String login { get; set; }
        public String password_md5 { get; set; }
        public String email { get; set; }

        private static Regex LoginPattern = new Regex("^[0-9a-zA-Z]{3,20}$");
        private static Regex PasswordPattern = new Regex("^[0-9a-zA-Z]{8,12}$");
        private static Regex EmailPattern = new Regex("^[0-9a-zA-Z_\\.]+@[a-z0-9\\-\\.]+$");

        public static readonly Predicate<String> LoginValidator = new Predicate<String>(data => data != null && LoginPattern.IsMatch(data));
        public static readonly Predicate<String> PasswordValidator = new Predicate<String>(data => data != null && PasswordPattern.IsMatch(data));
        public static readonly Predicate<String> EmailValidator = new Predicate<String>(data => data != null && EmailPattern.IsMatch(data));

        public User() { }
        public User(String Login, String Password, String Email, Int32 Id = Constants.IdDefault)
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
