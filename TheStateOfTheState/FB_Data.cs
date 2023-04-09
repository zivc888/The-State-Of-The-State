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
            var a = await firebase_client.Child("Results/Q" + questionId + "/matrix").OnceSingleAsync<Dictionary<string, Dictionary<string, int>>>();
            var a1 = await firebase_client.Child("Results/Q" + questionId + "/matrix/answer_1").OnceSingleAsync<Dictionary<string, int>>();
            var a2 = await firebase_client.Child("Results/Q" + questionId + "/matrix/answer_2").OnceSingleAsync<Dictionary<string, int>>();
            var a3 = await firebase_client.Child("Results/Q" + questionId + "/matrix/answer_3").OnceSingleAsync<Dictionary<string, int>>();
            var a4 = await firebase_client.Child("Results/Q" + questionId + "/matrix/answer_4").OnceSingleAsync<Dictionary<string, int>>();
            var a5 = await firebase_client.Child("Results/Q" + questionId + "/matrix/answer_5").OnceSingleAsync<Dictionary<string, int>>();
            Results_Structure result = new Results_Structure(a1, a2, a3, a4, a5);


            if (result != null)
            {
                if (result.A1 != null)
                {
                    Console.WriteLine(result.A1["age"]);
                }
                else
                {
                    Console.WriteLine("oof");
                }
            }
            else
            {
                Console.WriteLine("bad");
            }
            return result;
        }
    }
}