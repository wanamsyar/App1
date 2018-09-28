using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Android.Gms.Tasks;
using Firebase.Database;
using Firebase.Database.Query;


namespace App1
{
    [Activity(Label = "RegisterBackEnd")]
    public class RegisterBackEnd : Activity, IOnCompleteListener
    {
        EditText Name,Password,ConfirmPassword,IC,PhoneNumber,Email,Adress1,Adress2,Adress3;
        FirebaseAuth auth;
        Button Submit;
        bool PasswordValidated=false;
        string FirebaseUrl = "https://macaimy-386c2.firebaseio.com";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.registerLayout);
            InitFirebaseAuth();
            InitView();
            EventHandler();
        }
        public void InitFirebaseAuth()
        {
            auth = FirebaseAuth.GetInstance(MainActivity.app);
        }
        public async void Submitted(object sender,EventArgs e)
        {
            if(string.IsNullOrEmpty(Name.Text)|| string.IsNullOrEmpty(Password.Text)|| string.IsNullOrEmpty(IC.Text)||string.IsNullOrEmpty(PhoneNumber.Text)|| string.IsNullOrEmpty(Email.Text)|| string.IsNullOrEmpty(Adress1.Text)|| string.IsNullOrEmpty(Adress2.Text)|| string.IsNullOrEmpty(Adress3.Text))
            {
                Toast.MakeText(ApplicationContext, "Please Enter All Field", ToastLength.Long).Show();
            }
            else
            {
                try
                {
                    Account user = new Account();
                    user.Name = Name.Text;
                    user.Password = Password.Text;
                    user.IC = IC.Text;
                    user.PhoneNumber = PhoneNumber.Text;
                    user.Email = Email.Text;
                    user.Adress1 = Adress1.Text;
                    user.Adress2 = Adress2.Text;
                    user.Adress3 = Adress3.Text;
                    var firebase = new FirebaseClient(FirebaseUrl);
                    var item = await firebase.Child("users").PostAsync<Account>(user);
                    auth.CreateUserWithEmailAndPassword(Email.Text, Password.Text).AddOnCompleteListener(this);
                }
                catch
                {
                    Toast.MakeText(ApplicationContext, "Failed", ToastLength.Long).Show();
                }
            }
        }
        public void ValidatePassword(object sender,EventArgs e)
        {
            if (Password.Text != ConfirmPassword.Text)
            {
                
                Password.SetTextColor(Android.Graphics.Color.Red);
                ConfirmPassword.SetTextColor(Android.Graphics.Color.Red);
                PasswordValidated = false;
            }
            else
            {
                Password.SetTextColor(Android.Graphics.Color.Black);
                ConfirmPassword.SetTextColor(Android.Graphics.Color.Black);
                PasswordValidated = true;
            }
        }
        public void OnComplete(Task task)
        {
            if (task.IsSuccessful&&PasswordValidated==true)
            {
                Intent Login = new Intent(this, typeof(MainActivity));
                StartActivity(Login);
            }
            else
            {
                Toast.MakeText(ApplicationContext, "Failed", ToastLength.Long).Show();
            }
        }
        public void InitView()
        {
            Name = FindViewById<EditText>(Resource.Id.NameEntry);
            Password = FindViewById<EditText>(Resource.Id.Password);
            ConfirmPassword = FindViewById<EditText>(Resource.Id.ConfirmPassword);
            IC = FindViewById<EditText>(Resource.Id.ICNumber);
            PhoneNumber = FindViewById<EditText>(Resource.Id.PhoneNumber);
            Email = FindViewById<EditText>(Resource.Id.EmailAdress);
            Adress1 = FindViewById<EditText>(Resource.Id.AdressOne);
            Adress2 = FindViewById<EditText>(Resource.Id.AdressTwo);
            Adress3 = FindViewById<EditText>(Resource.Id.AdressThree);
            Submit = FindViewById<Button>(Resource.Id.SubmitForm);
        }
        public void EventHandler()
        {
            Password.TextChanged += ValidatePassword;
            ConfirmPassword.TextChanged += ValidatePassword;
            Submit.Click += Submitted;
        }
    }
}