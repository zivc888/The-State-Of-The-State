using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Newtonsoft.Json;

namespace TheStateOfTheState
{
    internal class FB_Data
    {
        private readonly FirebaseApp app;
        private readonly FirebaseAuth auth;
        private readonly FirebaseClient firebase_client = new FirebaseClient("https://the-state-of-the-state-default-rtdb.firebaseio.com");

        public FB_Data()
        {
            app = FirebaseApp.InitializeApp(Application.Context);

            if (app is null)
            {
                Firebase.FirebaseOptions options = GetMyOptions();
                app = FirebaseApp.InitializeApp(Application.Context, options);
            }

            auth = FirebaseAuth.Instance;
        }

        private Firebase.FirebaseOptions GetMyOptions()
        {
            return new Firebase.FirebaseOptions.Builder()
                .SetProjectId("the-state-of-the-state")
                .SetApplicationId("the-state-of-the-state")
                .SetApiKey("AIzaSyDQFpQpc70lnIzRo3DGnfp9cb91xwUqroI")
                .SetStorageBucket("the-state-of-the-state.appspot.com")
                .Build();
        }

        public Android.Gms.Tasks.Task CreateUser(string email, string password)
        {
            return auth.CreateUserWithEmailAndPassword(email, password);
        }
        public Android.Gms.Tasks.Task SignIn(string email, string password)
        {
            return auth.SignInWithEmailAndPassword(email, password);
        }
        public Android.Gms.Tasks.Task ResetPassword(string email)
        {
            return auth.SendPasswordResetEmail(email);
        }

        public async System.Threading.Tasks.Task<User> RetrieveUser(string userId)
        {
            var tmp = firebase_client.Child("users/" + userId).OnceSingleAsync<User>();
            return await tmp;
        }
        public async System.Threading.Tasks.Task<Results_Structure> RetrieveResults(int questionId)
        {
            Results_Structure res = new Results_Structure();
            res.General_Matrix = await firebase_client.Child("Results/Q" + questionId + "/general").OnceSingleAsync<Dictionary<string, int>>();
            res.Ori_Matrix = await firebase_client.Child("Results/Q" + questionId + "/orientation").OnceSingleAsync<Dictionary<string, Dictionary<string, int>>>();
            res.Rel_Matrix = await firebase_client.Child("Results/Q" + questionId + "/religion").OnceSingleAsync<Dictionary<string, Dictionary<string, int>>>();
            return res;
        }
        public async System.Threading.Tasks.Task<List<QuestionClass>> RetrieveQuestions()
        {
            List<QuestionClass> questions = new List<QuestionClass>();
            var tmp = await firebase_client.Child("Questions").OnceSingleAsync<Dictionary<string, Dictionary<string, string>>>();
            foreach(var q in tmp.Keys)
            {
                QuestionClass question = new QuestionClass();
                var question_info = tmp[q];

                for (int i = 0; i < question_info.Count-1; i++)
                {
                    AnswerClass answer = new AnswerClass();
                    answer.Name = "answer_" + (i + 1);
                    answer.Content = question_info["answer_" + (i + 1)];
                    question.Answers.Add(answer);
                }
                question.Content = question_info["question"];
                question.Name = q;

                questions.Add(question);
            }

            return questions;
        }

    }
}