using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace TheStateOfTheState
{
    internal class User
    {
        string name, mail, pwd, city;
        int age;
        General.ReligionTypes religion;
        General.OrientationTypes orientation;
        bool exist;
        readonly SP_data spd;

        int score;
        Dictionary<string, string> answers;

        public User(Context ctx)
        {
            spd = new SP_data(ctx);
            this.Name = spd.GetStringValue(General.KEY_NAME);
            this.Exist = this.name != String.Empty;

            if (this.exist)
            {
                this.Mail = spd.GetStringValue(General.KEY_MAIL);
                this.Pwd = spd.GetStringValue(General.KEY_PWD);
            }
        }

        public User(string name, string mail, string pwd, string city, int age, General.ReligionTypes religion, General.OrientationTypes orientation, bool exist)
        {
            this.name = name.Trim();
            this.mail = mail.Trim();
            this.pwd = pwd.Trim();
            this.city = city.Trim();
            this.age = age;
            this.religion = religion;
            this.orientation = orientation;
            this.exist = exist;

            score = 0;
            answers = new Dictionary<string, string>();
        }

        public User(User user)
        {
            this.name = user.Name;
            this.mail = user.Mail;
            this.pwd = user.Pwd;
            this.city = user.City;
            this.age = user.Age;
            this.religion = user.Religion;
            this.orientation = user.Orientation;
            this.exist = user.Exist;
            this.score = user.Score;
            this.answers = user.answers;
        }

        public User()
        {
            name = "";
            mail = "";
            pwd = "";
            city = "";
            age = 0;
            religion = 0;
            orientation = 0;
            exist = false;
            spd = null;
            score = 0;
            answers = new Dictionary<string, string>();
        }

        public string Name { get => name; set => name = value; }
        public string Mail { get => mail; set => mail = value; }
        public string Pwd { get => pwd; set => pwd = value; }
        public string City { get => city; set => city = value; }
        public int Age { get => age; set => age = value; }
        public General.ReligionTypes Religion { get => religion; set => religion = value; }
        public General.OrientationTypes Orientation { get => orientation; set => orientation = value; }
        public int Score { get => score; set => score = value; }
        public Dictionary<string, string> Answers { get => answers; set => answers = value; }

        public bool Exist { get => exist; set => exist = value; }

        public bool Save()
        {
            bool success = spd.PutStringValue(General.KEY_NAME, this.Name);
            success = success && spd.PutStringValue(General.KEY_PWD, this.Pwd);
            return success && spd.PutStringValue(General.KEY_MAIL, this.Mail);
        }
    }
}