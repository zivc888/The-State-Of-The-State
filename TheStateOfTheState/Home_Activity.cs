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

        private TextView name, score, answers;
        private FB_Data fbd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Home_Screen);
            Title = "";

            name = FindViewById<TextView>(Resource.Id.text_user_name);
            score = FindViewById<TextView>(Resource.Id.text_user_score);
            answers = FindViewById<TextView>(Resource.Id.text_anum_weekly);

            // Set up the toolbar
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

            // Retrieve the user data
            GetUserAsync(fbd, userId);
        }

        private async void GetUserAsync(FB_Data fbd, string userId)
        {
            // Retrieve user object from Firebase
            var user = await fbd.RetrieveUser(userId);

            name.Text = name.Text + user.Name;
            score.Text = score.Text + user.Score;
            answers.Text = answers.Text + user.Answers;
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
    }
}