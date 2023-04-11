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
using Firebase.Database;

namespace TheStateOfTheState
{
    [Activity(Label = "Results_Activity")]
    public class Results_Activity : AppCompatActivity
    {
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private AndroidX.AppCompat.Widget.Toolbar toolbar;

        private Button by_rel, by_ori;
        private TextView name, score;
        private FB_Data fbd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Results_Screen);
            Title = "";

            name = FindViewById<TextView>(Resource.Id.text_user_name);
            score = FindViewById<TextView>(Resource.Id.text_user_score);
            by_rel = FindViewById<Button>(Resource.Id.button_rel);
            by_ori = FindViewById<Button>(Resource.Id.button_ori);

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

            LoadData(fbd, General.KEY_ORI);

            by_rel.Click += By_rel_Click;
            by_ori.Click += By_ori_Click;
        }

        private void By_ori_Click(object sender, EventArgs e)
        {
            LoadData(fbd, General.KEY_ORI);
        }

        private void By_rel_Click(object sender, EventArgs e)
        {
            LoadData(fbd, General.KEY_REL);
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

            name.Text = "Name: " + user.Name;
            score.Text = "Score: " + user.Score;
        }

        private async void LoadData(FB_Data fbd, string type)
        {
            for (int i = 1; i <= await fbd.RetrieveQNUM(); i++)
            {
                GetResultsAsync(fbd, i, type);
            }
        }
        private async void GetResultsAsync(FB_Data fbd, int questionId, string type)
        {
            // Retrieve question results object from Firebase
            var tmp = await fbd.RetrieveResults(questionId);
            LoadGraph(tmp, questionId, type);

        }
        public void LoadGraph(Results_Structure data, int questionId, string type)
        {
            OxyColor[] colors = { OxyColors.Blue, OxyColors.Red, OxyColors.Green, OxyColors.Orange, OxyColors.Purple, OxyColors.Brown, OxyColors.Yellow, OxyColors.Gray, OxyColors.Pink, OxyColors.Teal };

            var plotModel = new PlotModel
            {
                Title = "Answers to Question " + questionId,
                Subtitle = "By " + type,
                LegendPosition = LegendPosition.TopRight,
                LegendOrientation = LegendOrientation.Vertical,
                LegendPlacement = LegendPlacement.Outside,
                LegendItemSpacing = 8,
                PlotMargins = new OxyThickness(60, 20, 20, 40)
            };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Category",
                IsZoomEnabled = false,
                IsPanEnabled = false
            };

            // Answers - X axis
            var tmp = new Dictionary<string, Dictionary<string, int>>();
            int len = 0;
            if (type == General.KEY_ORI)
            {
                tmp = data.Ori_Matrix;
                len = (int)General.OrientationTypes.Length;
            }
            else //if(type == General.KEY_REL)
            {
                tmp = data.Rel_Matrix;
                len = (int)General.ReligionTypes.Length;
            }

            foreach (var i in tmp.Keys)
            {
                categoryAxis.Labels.Add(i);
            }
            plotModel.Axes.Add(categoryAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Value",
                IsZoomEnabled = false,
                IsPanEnabled = false
            };
            plotModel.Axes.Add(valueAxis);

            // Options - sub X axis
            for (int i = 0; i < len; i++)
            {
                string title = "";
                if (type == General.KEY_ORI)
                {
                    title = ((General.OrientationTypes)i).ToString();
                }
                else //if(type == General.KEY_REL)
                {
                    title = ((General.ReligionTypes)i).ToString();
                }

                var series = new ColumnSeries
                {
                    Title = title,
                    FillColor = colors[i],
                    StrokeColor = OxyColors.Black,
                    StrokeThickness = 1
                };

                foreach(var answer in tmp.Keys)
                {
                    series.Items.Add(new ColumnItem(tmp[answer]["option_" + i]));
                }
                plotModel.Series.Add(series);

            }

            // Assign the plot model to the plot view
            var resourceId = Resources.GetIdentifier("graph_" + questionId, "id", PackageName);
            var plotView = FindViewById<OxyPlot.Xamarin.Android.PlotView>(resourceId);
            plotView.Model = plotModel;
        }

    }
}