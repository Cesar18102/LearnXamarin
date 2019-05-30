using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LearnXamarin
{
    public class Constants
    {
        public const int IdDefault = 0;
        public static MD5 mD5 = MD5.Create();
        public static Random R = new Random();
        public static UTF8Encoding UTF8 = new UTF8Encoding();

        public static string Hash(string Str, Encoding E, HashAlgorithm HA)
        {
            byte[] data = E.GetBytes(Str);
            byte[] hashData = HA.ComputeHash(data);
            return BitConverter.ToString(hashData).Replace("-", "").ToLower();
        }

        public static List<T> GetAllInvalid<T>(Dictionary<T, Predicate<T>> Validators) where T : new()
        {
            List<T> Invalids = new List<T>();
            foreach (KeyValuePair<T, Predicate<T>> Validator in Validators)
                if (!Validator.Value(Validator.Key))
                    Invalids.Add(Validator.Key);
            return Invalids;
        }

        public static List<T> Substract<T>(List<T> Super, List<T> Sub)
        {
            List<T> Substraction = new List<T>();
            foreach (T item in Super)
                if (!Sub.Contains(item))
                    Substraction.Add(item);
            return Substraction;
        }

        public delegate Task DisplayMethod(string Header, string Text, string Close);
        public class Alert
        {
            public string Header { get; private set; }
            public string Text { get; private set; }
            public string Close { get; private set; }

            private DisplayMethod displayHandler;

            public Alert(string Header, string Text, string Close, DisplayMethod DisplayHandler)
            {
                this.Header = Header;
                this.Text = Text;
                this.Close = Close;
                this.displayHandler = DisplayHandler;
            }

            public Task Display()
            {
                return displayHandler(Header, Text, Close);
            }
        }

        public static bool Validate<T>(List<T> Inputs, Dictionary<T, Predicate<T>> Validators,
                                       Dictionary<T, Constants.Alert> Alerts, 
                                       Action<T> InvalidHandle, Action<T> ValidHandle, bool Messages) where T : new() {

            List<T> Invalids = GetAllInvalid<T>(Validators);
            List<T> Valids = Substract<T>(Inputs, Invalids);

            if (Messages)
                foreach (T Invalid in Invalids)
                {
                    InvalidHandle(Invalid);
                    Alerts[Invalid].Display();
                }
            else
                foreach (T Invalid in Invalids)
                    InvalidHandle(Invalid);


            foreach (T Valid in Valids)
                ValidHandle(Valid);

            return Invalids.Count == 0;
        }
    }
}
