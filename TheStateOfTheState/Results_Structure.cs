using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheStateOfTheState
{
    public class Results_Structure
    {
        public Dictionary<string, int> General_Matrix { get; set; }
        /*
         * "answer_1":#
         * "answer_2":#
         * ...
         */

        public Dictionary<string, Dictionary<string, int>> Ori_Matrix { get; set; }
        /*
         * "answer_1":
         *   "option_0":#
         *   "option_1":#
         *   ...
         * "answer_2":
         *   ...
         * ...
         */

        public Dictionary<string, Dictionary<string, int>> Rel_Matrix { get; set; }
        /*
         * "answer_1":
         *   "option_0":#
         *   "option_1":#
         *   ...
         * "answer_2":
         *   ...
         * ...
         */

        public Results_Structure()
        {
            General_Matrix = new Dictionary<string, int>();
            Ori_Matrix = new Dictionary<string, Dictionary<string, int>>();
            Rel_Matrix = new Dictionary<string, Dictionary<string, int>>();
        }
    }
}