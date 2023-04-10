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
    public class AnswerClass
    {
        private string name;
        private string content;

        public AnswerClass(string name, string content)
        {
            this.name = name;
            this.content = content;
        }

        public AnswerClass(int answerId, string content)
        {
            this.name = "A" + answerId;
            this.content = content;
        }

        public AnswerClass()
        {
            name = "";
            content = "";
        }

        public string Name { get => name; set => name = value; }
        public string Content { get => content; set => content = value; }
    }
}