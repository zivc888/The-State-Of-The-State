using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheStateOfTheState
{
    static class General
    {
        public const string SP_FILE_NAME = "data.sp";
        public const string KEY_NAME = "Name";
        public const string KEY_MAIL = "mail";
        public const string KEY_PWD = "pwd";
        public const string KEY_AGE = "age";
        public const string KEY_CITY = "city";
        public const string KEY_REL = "religion";
        public const string KEY_ORI = "orientation";
        public const string KEY_CAMERA_IMAGE = "data";
        public const int REQUEST_REGISTER = 1;
        public enum ReligionTypes { Secular_Jew, Traditional_Jew, Religious_Jew, Orthodox_Jew, Secular_Arab, Religious_Arab };
        public enum OrientationTypes { Extreme_Left, Left, Center_Left, Center, Center_Right, Right, Extreme_Right };

        private const string FirebaseEndpoint = "https://the-state-of-the-state-default-rtdb.firebaseio.com";
        private const string FirebaseAuthToken = "MCcuulDEqpm8exd8IzoogoUe5O0uh0tKgbi1DhYI";
    }
}