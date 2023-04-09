using System;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Java.Util;

namespace TheStateOfTheState
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnCompleteListener
    {
        private EditText emailInput;
        private EditText passwordInput;
        private Button loginButton;
        private TextView registerLink;

        private FB_Data fbd;
        private User user;

        private Task tskLogin, tskReset;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set view
            SetContentView(Resource.Layout.activity_main);

            // Get the UI elements
            emailInput = FindViewById<EditText>(Resource.Id.email_input);
            passwordInput = FindViewById<EditText>(Resource.Id.password_input);
            loginButton = FindViewById<Button>(Resource.Id.login_button);
            registerLink = FindViewById<TextView>(Resource.Id.register_link);

            // Initialize Firebase
            fbd = new FB_Data();
            user = new User(this);

            // Add click listeners
            loginButton.Click += LoginButton_Click;
            registerLink.Click += RegisterLink_Click;

            if (user.Exist)
            {
                ShowUserData();
            }
            else
            {
                OpenRegisterActivity();
            }
        }

        private void ShowUserData()
        {
            emailInput.Text = user.Mail;
            passwordInput.Text = user.Pwd;
        }

        private void OpenRegisterActivity()
        {
            Intent i = new Intent(this, typeof(Register_Activity));
            StartActivityForResult(i, General.REQUEST_REGISTER);
        }

        private void RegisterLink_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(Register_Activity));
            StartActivityForResult(i, General.REQUEST_REGISTER);
        }

        private void LoginButton_Click(object sender, System.EventArgs e)
        {
            if (passwordInput.Text != string.Empty)
            {
                tskLogin = fbd.SignIn(emailInput.Text, passwordInput.Text);
                tskLogin.AddOnCompleteListener(this);

                user.Pwd = passwordInput.Text;
                if (!user.Save())
                {
                    Toast.MakeText(this, "Error", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Enter Password", ToastLength.Long).Show();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == General.REQUEST_REGISTER)
            {
                if (resultCode == Result.Ok)
                {
                    user.Name = data.GetStringExtra(General.KEY_NAME);
                    user.Mail = data.GetStringExtra(General.KEY_MAIL);
                    user.Pwd = data.GetStringExtra(General.KEY_PWD);
                    user.Age = data.GetIntExtra(General.KEY_AGE, 0);
                    user.City = data.GetStringExtra(General.KEY_CITY);
                    user.Orientation = (General.OrientationTypes)data.GetIntExtra(General.KEY_ORI, 0);
                    user.Religion = (General.ReligionTypes)data.GetIntExtra(General.KEY_REL, 0);

                    ShowUserData();
                    SaveToFirebase();
                }
            }
        }

        public void OnComplete(Task task)
        {
            string msg = string.Empty;
            if (task.IsSuccessful)
            {
                if (task == tskLogin)
                {
                    msg = "Login Successful";
                    Intent i = new Intent(this, typeof(Home_Activity));
                    i.PutExtra(General.KEY_NAME, user.Name);
                    StartActivity(i);
                }
                else if (task == tskReset)
                {
                    msg = "Reset Successful";
                }
            }
            else
                msg = task.Exception.Message;
            Toast.MakeText(this, msg, ToastLength.Long).Show();
        }

        private void Reset_Click(object sender, System.EventArgs e)
        {
            tskReset = fbd.ResetPassword(user.Mail);
            tskReset.AddOnCompleteListener(this);
        }

        private void SaveToFirebase()
        {
            // Get a reference to the 'users' child node under the root node
            FirebaseDatabase firebase = FirebaseDatabase.GetInstance("https://the-state-of-the-state-default-rtdb.firebaseio.com");
            DatabaseReference usersRef = firebase.GetReference("users");

            // Generate a new unique ID for the user
            string userId = usersRef.Push().Key;

            // Set the user data at the newly generated user ID node
            usersRef.Child(userId).Child("mail").SetValue(user.Mail);
            usersRef.Child(userId).Child("password").SetValue(user.Pwd);
            usersRef.Child(userId).Child("name").SetValue(user.Name);
            usersRef.Child(userId).Child("age").SetValue(user.Age);
            usersRef.Child(userId).Child("city").SetValue(user.City);
            usersRef.Child(userId).Child("religion").SetValue((int)user.Religion);
            usersRef.Child(userId).Child("orientation").SetValue((int)user.Orientation);
            usersRef.Child(userId).Child("score").SetValue(user.Score);

            // Save userId to SharedPreferences
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("userId", userId);
            editor.Apply();

            Toast.MakeText(this, "User saved successfuly", ToastLength.Long).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}