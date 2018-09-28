using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Mono;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Firebase;
using Firebase.Auth;
using Android.Gms.Tasks;


namespace App1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity,IOnCompleteListener
    {
        EditText Emails,Passwords;
        Button Logins, Register;
        FirebaseAuth auth;
        public static FirebaseApp app;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            InitFirebaseAuth();
            InitView();
            EventHandler();
            if (auth.CurrentUser != null)
            {
                Intent Login = new Intent(this, typeof(Menu));
                StartActivity(Login);
            }
        }
        public void Login(object sender, EventArgs e)
        {
            try
            {
                auth.SignInWithEmailAndPassword(Emails.Text, Passwords.Text).AddOnCompleteListener(this);
            }
            catch
            {
                Toast.MakeText(ApplicationContext, "Please Fill Email and Password Field", ToastLength.Long).Show();
            }

        }
        public void Registers(object sender, EventArgs e)
        {
            Intent Register = new Intent(this, typeof(RegisterBackEnd));
            StartActivity(Register);
        }
        public void InitFirebaseAuth()
        {
            var option = new FirebaseOptions.Builder()
                .SetApplicationId("1:947050354240:android:b742b23f08832e87")
                .SetApiKey("AIzaSyDZuLFOazSCYfI2q3oEGyb8qWl53DgpSWk").Build();
            if (app == null)
            {
                app = FirebaseApp.InitializeApp(this, option);
            }
            auth = FirebaseAuth.GetInstance(app);
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                Intent Login = new Intent(this, typeof(Menu));
                StartActivity(Login);
            }
            else 
            {
                Toast.MakeText(ApplicationContext,"Error to login",ToastLength.Long).Show();
            }
        }
        public void InitView()
        {
            Emails = FindViewById<EditText>(Resource.Id.Email);
            Passwords = FindViewById<EditText>(Resource.Id.Password);
            Logins = FindViewById<Button>(Resource.Id.Login);
            Register = FindViewById<Button>(Resource.Id.Register);
        }
        public void EventHandler()
        {
            Logins.Click += Login;
            Register.Click += Registers;
        }
    }
}

