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
        private string content, name;
        private List<AnswerClass> answers;
        /*
         * 1: "answer_1":"..."
         * 2: "answer_2":"..."
         * ...
         */

        private QuestionClass(List<AnswerClass> answers, string name, string content)
        {
            this.answers = answers;
            this.name = name;
            this.content = content;
        }

        private QuestionClass(List<AnswerClass> answers, int questionId, string content)
        {
            this.answers = answers;
            this.name = "Q" + questionId;
            this.content = content;
        }

        public QuestionClass()
        {
            answers = new List<AnswerClass>();
            name = "Q";
            content = "Empty";
        }

        public List<AnswerClass> Answers { get => answers; set => answers = value; }
        public string Name { get => name; set => name = value; }
        public string Content { get => content; set => content = value; }

    }
}