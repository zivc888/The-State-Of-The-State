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
        /*
        public Dictionary<string, Dictionary<string, int>> Matrix { get; set; }

        public Results_Structure(Dictionary<string, Dictionary<string, int>> matrix)
        {
            Matrix = matrix;
        }

        public Results_Structure()
        {
            Dictionary<string, Dictionary<string, int>> dict = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> innerDict = new Dictionary<string, int>();

            for (int i=0; i<5; i++)
            {
                innerDict.Add("age", 0);
                innerDict.Add("religion", 0);
                innerDict.Add("orientation", 0);
                dict.Add("answer_"+(i+1), innerDict);
            }
        }
        */

        Dictionary<string, int> answer1;
        Dictionary<string, int> answer2;
        Dictionary<string, int> answer3;
        Dictionary<string, int> answer4;
        Dictionary<string, int> answer5;


        public Results_Structure(Dictionary<string, int> answer1, Dictionary<string, int> answer2, Dictionary<string, int> answer3, Dictionary<string, int> answer4, Dictionary<string, int> answer5)
        {
            this.answer1 = answer1;
            this.answer2 = answer2;
            this.answer3 = answer3;
            this.answer4 = answer4;
            this.answer5 = answer5;
        }
        public Results_Structure()
        {
            answer1 = new Dictionary<string, int>();
            answer2 = new Dictionary<string, int>();
            answer3 = new Dictionary<string, int>();
            answer4 = new Dictionary<string, int>();
            answer5 = new Dictionary<string, int>();

        }

        public Dictionary<string, int> A1 { get => answer1; set => answer1 = value; }
        public Dictionary<string, int> A2 { get => answer2; set => answer2 = value; }
        public Dictionary<string, int> A3 { get => answer3; set => answer3 = value; }
        public Dictionary<string, int> A4 { get => answer4; set => answer4 = value; }
        public Dictionary<string, int> A5 { get => answer5; set => answer5 = value; }


        /*
        public override string ToString()
        {
            string s1 = "answer1: " + answer1[1] + " " + answer1[2] + " " + answer1[3];
            string s2 = "answer2: " + answer2[1] + " " + answer2[2] + " " + answer2[3];
            string s3 = "answer3: " + answer3[1] + " " + answer3[2] + " " + answer3[3];
            string s4 = "answer4: " + answer4[1] + " " + answer4[2] + " " + answer4[3];
            return s1 + "\n" + s2 + "\n" + s3 + "\n" + s4;
        }
        */

    }
}