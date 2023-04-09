using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.AppCompat.App;
using Android.Preferences;
using Firebase.Database;
using Android.Service.Autofill;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;

namespace TheStateOfTheState
{
    [Activity(Label = "Home_Activity")]
    public class Home_Activity : AppCompatActivity
    {
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private AndroidX.AppCompat.Widget.Toolbar toolbar;

        private Button submit, exit;
        private TextView name, score;
        private FB_Data fbd;

        private List<QuestionClass> questions_info;
        private Dictionary<int, RadioGroup> questions;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Home_Screen);
            Title = "";

            name = FindViewById<TextView>(Resource.Id.text_user_name);
            score = FindViewById<TextView>(Resource.Id.text_user_score);
            submit = FindViewById<Button>(Resource.Id.button_submit);
            exit = FindViewById<Button>(Resource.Id.button_exit);

            // Initialize questions from server
            InitializeQuestions();
            questions = new Dictionary<int, RadioGroup>();
            for (int i = 1; i <= General.Q_NUM; i++)
            {
                var resourceId = Resources.GetIdentifier("question_" + i + "_options", "id", PackageName);
                questions.Add(i, FindViewById<RadioGroup>(resourceId));
            }

            // Set up the toolbar - menu + navigation
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.navigation_view);
            toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);

            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawerLayout.AddDrawerListener(toggle);
            toggle.SyncState();

            navigationView.SetNavigationItemSelectedListener(new NavigationItemSelectedListener(this));

            // Retrieve userId from SharedPreferences
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            string userId = prefs.GetString("userId", "");
            fbd = new FB_Data();

            // Retrieve user data
            GetUserAsync(fbd, userId);

            /*
            for (int i = 1; i <= General.Q_NUM; i++)
            {
                InitializeDB(i);
            }
            */

            submit.Click += Submit_Click;
            exit.Click += Exit_Click;
        }

        private void InitializeQuestions()
        {
            throw new NotImplementedException();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private async void UpdateDB(Results_Structure data, int questionId, string type, int answerId, User user)
        {
            // Get a reference to the 'Results' child node under the root node
            FirebaseDatabase firebase = FirebaseDatabase.GetInstance("https://the-state-of-the-state-default-rtdb.firebaseio.com");
            DatabaseReference DBRef = firebase.GetReference("Results/Q" + questionId + "/" + type + "/answer_" + answerId);
            Dictionary<string, int> tmp_update = new Dictionary<string, int>();

            switch (type)
            {
                case General.KEY_ORI:
                    tmp_update = data.Ori_Matrix["answer_"+answerId];
                    DBRef.Child("option_" + (int)user.Orientation).SetValue(tmp_update["option_" + (int)user.Orientation] + 1);
                    break;
                case General.KEY_REL:
                    tmp_update = data.Rel_Matrix["answer_" + answerId];
                    DBRef.Child("option_" + (int)user.Religion).SetValue(tmp_update["option_" + (int)user.Religion] + 1);
                    break;
                case General.KEY_GEN:
                    tmp_update = data.General_Matrix;
                    DBRef.SetValue(tmp_update["answer_" + answerId] + 1);
                    break;
            }
        }
        private async void Submit_Click(object sender, EventArgs e)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            string userId = prefs.GetString("userId", "");
            fbd = new FB_Data();
            User user = await fbd.RetrieveUser(userId);

            foreach (var question in questions)
            {
                int a_num = question.Value.ChildCount;
                for (int i = 0; i < a_num; i++)
                {
                    RadioButton answer = (RadioButton)question.Value.GetChildAt(i);
                    if (answer.Checked)
                    {
                        Console.WriteLine("q index: " + question.Key);
                        Results_Structure tmp = await fbd.RetrieveResults(question.Key);

                        UpdateDB(tmp, question.Key, General.KEY_ORI, i+1, user);
                        UpdateDB(tmp, question.Key, General.KEY_REL, i+1, user);
                        UpdateDB(tmp, question.Key, General.KEY_GEN, i+1, user);
                        user.Score = user.Score + 1;
                    }

                }
            }

            FirebaseDatabase firebase = FirebaseDatabase.GetInstance("https://the-state-of-the-state-default-rtdb.firebaseio.com");
            DatabaseReference usersRef = firebase.GetReference("users");
            usersRef.Child(userId).Child("score").SetValue(user.Score);
            score.Text = "Score: " + user.Score;
        }

        private async void GetUserAsync(FB_Data fbd, string userId)
        {
            // Retrieve user object from Firebase
            User user = await fbd.RetrieveUser(userId);

            name.Text = "Name: " + user.Name;
            score.Text = "Score: " + user.Score;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.Home_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent i;
            switch (item.ItemId)
            {
                case Resource.Id.menu_surveys:
                    return true;
                case Resource.Id.menu_results:
                    // Handle "Results" menu item click
                    i = new Intent(this, typeof(Results_Activity));
                    StartActivity(i);
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void InitializeDB(int questionId)
        {
            // Get a reference to the 'Results' child node under the root node
            FirebaseDatabase firebase = FirebaseDatabase.GetInstance("https://the-state-of-the-state-default-rtdb.firebaseio.com");
            DatabaseReference DBRef = firebase.GetReference("Results/Q" + questionId);

            for (int j = 1; j <= 3; j++)
            {
                for (int i = 0; i < (int)General.OrientationTypes.Length; i++)
                {
                    DBRef.Child(General.KEY_ORI + "/answer_" + j + "/option_" + i).SetValue(0);
                }
                for (int i = 0; i < (int)General.ReligionTypes.Length; i++)
                {
                    DBRef.Child(General.KEY_REL + "/answer_" + j + "/option_" + i).SetValue(0);
                }
                DBRef.Child("general/answer_" + j).SetValue(0);
            }
        }
    }
}