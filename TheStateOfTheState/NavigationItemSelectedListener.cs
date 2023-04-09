using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheStateOfTheState
{
    internal class NavigationItemSelectedListener : Java.Lang.Object, NavigationView.IOnNavigationItemSelectedListener
    {
        private readonly Context context;

        public NavigationItemSelectedListener(Context context)
        {
            this.context = context;
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            int id = menuItem.ItemId;

            switch (id)
            {
                case Resource.Id.menu_results:
                    // Handle the Results option
                    Intent resultsIntent = new Intent(context, typeof(Results_Activity));
                    context.StartActivity(resultsIntent);
                    break;

                case Resource.Id.menu_surveys:
                    // Handle the Questions option
                    Intent surveysIntent = new Intent(context, typeof(Home_Activity));
                    context.StartActivity(surveysIntent);
                    break;
            }

            // Close the drawer after handling the selection
            DrawerLayout drawer = ((Activity)context).FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);

            return true;
        }
    }
}