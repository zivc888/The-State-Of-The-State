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
    internal class AnswerClass
    {
        string name;
        string content;

        public AnswerClass(string name, string content)
        {
            this.name = name;
            this.content = content;
        }

        public string Name { get => name; set => name = value; }
        public string Content { get => content; set => content = value; }
    }
}