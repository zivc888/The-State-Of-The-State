using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Android.Preferences;
using Newtonsoft.Json;

namespace TheStateOfTheState
{
    [Activity(Label = "Results_Activity")]
    public class Results_Activity : AppCompatActivity
    {
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private AndroidX.AppCompat.Widget.Toolbar toolbar;

        private TextView name, score, answers;
        private FB_Data fbd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Results_Screen);
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

            LoadData(fbd);
            LoadGraphs();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent i;
            switch (item.ItemId)
            {
                case Resource.Id.menu_surveys:
                    // Handle "Surveys" menu item click
                    i = new Intent(this, typeof(Home_Activity));
                    StartActivity(i);
                    Finish();
                    return true;
                case Resource.Id.menu_results:
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.Home_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        private async void GetUserAsync(FB_Data fbd, string userId)
        {
            // Retrieve user object from Firebase
            var user = await fbd.RetrieveUser(userId);

            name.Text = name.Text + user.Name;
            score.Text = score.Text + user.Score;
            answers.Text = answers.Text + user.Answers;
        }


        public void LoadGraphs()
        {
            // Create the plot model
            var plotModel = new PlotModel { Title = "My Plot" };

            // Create the axes
            var xAxis = new LinearAxis { Position = AxisPosition.Bottom };
            var yAxis = new LinearAxis { Position = AxisPosition.Left };

            // Create the series and add data points
            var series1 = new LineSeries
            {
                Title = "Series 1",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.SkyBlue,
                Color = OxyColors.SkyBlue,
                StrokeThickness = 2
            };

            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(1, 1));
            series1.Points.Add(new DataPoint(2, 2));
            series1.Points.Add(new DataPoint(3, 3));

            var series2 = new LineSeries
            {
                Title = "Series 2",
                MarkerType = MarkerType.Square,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.PaleVioletRed,
                Color = OxyColors.PaleVioletRed,
                StrokeThickness = 2
            };

            series2.Points.Add(new DataPoint(0, 1));
            series2.Points.Add(new DataPoint(1, 2));
            series2.Points.Add(new DataPoint(2, 3));
            series2.Points.Add(new DataPoint(3, 4));

            // Add the series to the plot model
            plotModel.Series.Add(series1);
            plotModel.Series.Add(series2);

            // Set the axes on the plot model
            plotModel.Axes.Add(xAxis);
            plotModel.Axes.Add(yAxis);

            // Assign the plot model to the plot view
            var plotView = FindViewById<OxyPlot.Xamarin.Android.PlotView>(Resource.Id.graph_1);
            plotView.Model = plotModel;
        }
        private void LoadData(FB_Data fbd)
        {
            for(int i = 0; i<1; i++)
            {
                GetResultsAsync(fbd, i);
            }
        }
        private async void GetResultsAsync(FB_Data fbd, int questionId)
        {
            // Retrieve question results object from Firebase
            var tmp = await fbd.RetrieveResults(questionId);

        }
    }
}