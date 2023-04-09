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
using Firebase.Database;

namespace TheStateOfTheState
{
    [Activity(Label = "Register_Activity")]
    public class Register_Activity : Activity, IOnCompleteListener
    {
        private EditText name, password, mail, city, age;
        private Button register, cancel;
        private User user;
        private FB_Data fbd;
        private RadioButton r1,r2,r3,r4,r5,r6,o1,o2,o3,o4,o5,o6,o7;
        private General.ReligionTypes religion;
        private General.OrientationTypes orientation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Register_Screen);

            name = FindViewById<EditText>(Resource.Id.et_name);
            password = FindViewById<EditText>(Resource.Id.et_password);
            mail = FindViewById<EditText>(Resource.Id.et_mail);
            city = FindViewById<EditText>(Resource.Id .et_city);
            age = FindViewById<EditText>(Resource.Id.et_age);
            register = FindViewById<Button>(Resource.Id.btn_register);
            cancel = FindViewById<Button>(Resource.Id.btn_cancel);

            r1 = FindViewById<RadioButton>(Resource.Id.radio_R1);
            r2 = FindViewById<RadioButton>(Resource.Id.radio_R2);
            r3 = FindViewById<RadioButton>(Resource.Id.radio_R3);
            r4 = FindViewById<RadioButton>(Resource.Id.radio_R4);
            r5 = FindViewById<RadioButton>(Resource.Id.radio_R5);
            r6 = FindViewById<RadioButton>(Resource.Id.radio_R6);

            o1 = FindViewById<RadioButton>(Resource.Id.radio_O1);
            o2 = FindViewById<RadioButton>(Resource.Id.radio_O2);
            o3 = FindViewById<RadioButton>(Resource.Id.radio_O3);
            o4 = FindViewById<RadioButton>(Resource.Id.radio_O4);
            o5 = FindViewById<RadioButton>(Resource.Id.radio_O5);
            o6 = FindViewById<RadioButton>(Resource.Id.radio_O6);
            o7 = FindViewById<RadioButton>(Resource.Id.radio_O7);

            fbd = new FB_Data();

            register.Click += Register_Click;
            cancel.Click += Cancel_Click;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void Register_Click(object sender, EventArgs e)
        {
            bool tmp1 = UpdateReligion(sender, e);
            bool tmp2 = UpdateOrientation(sender, e);
            if(tmp1 && tmp2)
            {
                user = new User(name.Text, mail.Text, password.Text, city.Text, int.Parse(age.Text), religion, orientation, false);
                if (user.Name != string.Empty && user.Mail != string.Empty && user.Pwd != string.Empty && city.Text != string.Empty && age.Text != string.Empty)
                {
                    fbd.CreateUser(user.Mail, user.Pwd).AddOnCompleteListener(this);
                }
                else
                {
                    Toast.MakeText(this, "Enter all values", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Enter all values", ToastLength.Short).Show();
            }
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Intent i = new Intent();
                i.PutExtra(General.KEY_NAME, user.Name);
                i.PutExtra(General.KEY_MAIL, user.Mail);
                i.PutExtra(General.KEY_PWD, user.Pwd);
                i.PutExtra(General.KEY_AGE, user.Age);
                i.PutExtra(General.KEY_CITY, user.City);
                i.PutExtra(General.KEY_ORI, (int)user.Orientation);
                i.PutExtra(General.KEY_REL, (int)user.Religion);
                SetResult(Result.Ok, i);
                Finish();
            }
            else
            {
                Toast.MakeText(this, task.Exception.Message, ToastLength.Long).Show();
            }
        }

        private bool UpdateReligion(object sender, EventArgs e)
        {
            RadioButton[] ReligionRadioButtons = { r1, r2, r3, r4, r5, r6 };

            for (int i = 0; i < 6; i++)
            {
                if (ReligionRadioButtons[i].Checked)
                {
                    religion = (General.ReligionTypes)i;
                    return true;
                }
            }
            return false;
        }

        private bool UpdateOrientation(object sender, EventArgs e)
        {
            RadioButton[] OrientationRadioButtons = { o1, o2, o3, o4, o5, o6, o7 };

            for (int i = 0; i < 7; i++)
            {
                if (OrientationRadioButtons[i].Checked)
                {
                    orientation = (General.OrientationTypes)i;
                    return true;
                }
            }
            return false;

        }
    }
}