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
    public class QuestionClass
    {
        List<AnswerClass> answers;
        /*
         * 1: "answer_1":"..."
         * 2: "answer_2":"..."
         * ...
         */

        public QuestionClass(List<Dictionary<string, string>> answers)
        {
            this.answers = answers;
        }

        public QuestionClass()
        {
            answers = new List<Dictionary<string, string>>();
        }

        public List<AnswerClass> Answers { get => answers; set => answers = value; }

    }
}