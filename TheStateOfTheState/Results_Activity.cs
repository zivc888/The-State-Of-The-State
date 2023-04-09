﻿using Android.App;
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

        private Button exit;
        private TextView name, score;
        private FB_Data fbd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Results_Screen);
            Title = "";

            name = FindViewById<TextView>(Resource.Id.text_user_name);
            score = FindViewById<TextView>(Resource.Id.text_user_score);
            exit = FindViewById<Button>(Resource.Id.button_exit);

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
            exit.Click += Exit_Click;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
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

        private void LoadData(FB_Data fbd)
        {
            for (int i = 1; i <= General.Q_NUM; i++)
            {
                GetResultsAsync(fbd, i);
            }
        }
        private async void GetResultsAsync(FB_Data fbd, int questionId)
        {
            var type = General.KEY_ORI;

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
            if (type == General.KEY_ORI)
            {
                tmp = data.Ori_Matrix;
            }
            else //if(type == General.KEY_REL)
            {
                tmp = data.Rel_Matrix;
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
            for (int i = 0; i < (int)General.OrientationTypes.Length; i++)
            {
                var series = new ColumnSeries
                {
                    Title = ((General.OrientationTypes)i).ToString(),
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