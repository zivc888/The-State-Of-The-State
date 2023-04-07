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
        FB_Data fbd;

        public User(string name, string mail, string pwd, bool exist)
        {
            this.name = name.Trim();
            this.mail = mail.Trim();
            this.pwd = pwd.Trim();
            this.exist = exist;
        }

        public string Name { get => name; set => name = value; }
        public string Mail { get => mail; set => mail = value; }
        public string Pwd { get => pwd; set => pwd = value; }
        public string City { get => city; set => city = value; }
        public int Age { get => age; set => age = value; }
        public General.ReligionTypes Religion { get => religion; set => religion = value; }
        public General.OrientationTypes Orientation { get => orientation; set => orientation = value; }

        public bool Exist { get => exist; set => exist = value; }

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
        }

        public bool Save()
        {
            bool success = spd.PutStringValue(General.KEY_NAME, this.Name);
            success = success && spd.PutStringValue(General.KEY_PWD, this.Pwd);
            return success && spd.PutStringValue(General.KEY_MAIL, this.Mail);
        }
    }
}