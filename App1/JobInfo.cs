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
using Firebase.Database;
using Firebase.Database.Query;

namespace App1
{
    [Activity(Label = "JobInfo")]
    public class JobInfo : Activity
    {
        TextView JobTypeDisplay, JobDescriptionDisplay, DateFromTitle, DateFromDisplay, DateToTitle, DateToDisplay, DateTitle, DateDisplay, FromTitle, FromDisplay, ToTitle, ToDisplay, LocationTitle, LocationDisplay, PriceDisplay, Status;
        ListView Negotiations;
        EditText TextSendNegotiation;
        Button SentButtonNegotiation, JobCompleted;

        string FirebaseUrl = "https://macaimy-386c2.firebaseio.com";
        List<string> chatlist = new List<string>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.JobsInfo);
            InitView();
            LoadData();
            EventHandler();
            InitChatNegotiation();
        }
        public void InitView()
        {
            JobTypeDisplay = FindViewById<TextView>(Resource.Id.JobTypeDisplay);
            JobDescriptionDisplay = FindViewById<TextView>(Resource.Id.JobDescriptionDisplay);
            DateFromTitle = FindViewById<TextView>(Resource.Id.DateFromTitle);
            DateFromDisplay = FindViewById<TextView>(Resource.Id.DateFromDisplay);
            DateToTitle = FindViewById<TextView>(Resource.Id.DateToTitle);
            DateToDisplay = FindViewById<TextView>(Resource.Id.DateToDisplay);
            DateTitle = FindViewById<TextView>(Resource.Id.DateTitle);
            DateDisplay = FindViewById<TextView>(Resource.Id.DateDisplay);
            FromTitle = FindViewById<TextView>(Resource.Id.FromTitle);
            FromDisplay = FindViewById<TextView>(Resource.Id.FromDisplay);
            ToTitle = FindViewById<TextView>(Resource.Id.ToTitle);
            ToDisplay = FindViewById<TextView>(Resource.Id.ToDisplay);
            LocationTitle = FindViewById<TextView>(Resource.Id.LocationTitle);
            LocationDisplay = FindViewById<TextView>(Resource.Id.LocationDisplay);
            PriceDisplay = FindViewById<TextView>(Resource.Id.PriceDisplay);
            Status = FindViewById<TextView>(Resource.Id.Status);
            Negotiations = FindViewById<ListView>(Resource.Id.Negotiations);
            TextSendNegotiation = FindViewById<EditText>(Resource.Id.TextSendNegotiation);
            SentButtonNegotiation = FindViewById<Button>(Resource.Id.SentButtonNegotiation);
            JobCompleted = FindViewById<Button>(Resource.Id.JobCompleted);
        }
        public void LoadData()
        {
            if (this.Intent.Extras.Get("Job_Type").ToString()== "Sent Item" || this.Intent.Extras.Get("Job_Type").ToString()== "Pick up Item" || this.Intent.Extras.Get("Job_Type").ToString()== "Buy Item")
            {
                DateDisplay.Visibility = ViewStates.Visible;
                DateTitle.Visibility = ViewStates.Visible;
                LocationTitle.Visibility = ViewStates.Gone;
                LocationDisplay.Visibility = ViewStates.Gone;
                FromTitle.Visibility = ViewStates.Visible;
                FromDisplay.Visibility = ViewStates.Visible;
                ToTitle.Visibility = ViewStates.Visible;
                ToDisplay.Visibility = ViewStates.Visible;
                DateFromTitle.Visibility = ViewStates.Gone;
                DateFromDisplay.Visibility = ViewStates.Gone;
                DateToTitle.Visibility = ViewStates.Gone;
                DateToDisplay.Visibility = ViewStates.Gone;
            }
            else if (this.Intent.Extras.Get("Job_Type").ToString()== "Fix Item" || this.Intent.Extras.Get("Job_Type").ToString()== "Clean Item" || this.Intent.Extras.Get("Job_Type").ToString()== "Healthcare Services")
            {
                LocationTitle.Visibility = ViewStates.Visible;
                LocationDisplay.Visibility = ViewStates.Visible;
                FromTitle.Visibility = ViewStates.Gone;
                FromDisplay.Visibility = ViewStates.Gone;
                ToTitle.Visibility = ViewStates.Gone;
                ToDisplay.Visibility = ViewStates.Gone;
                DateFromTitle.Visibility = ViewStates.Gone;
                DateFromDisplay.Visibility = ViewStates.Gone;
                DateToTitle.Visibility = ViewStates.Gone;
                DateToDisplay.Visibility = ViewStates.Gone;
                DateTitle.Visibility = ViewStates.Visible;
                DateDisplay.Visibility = ViewStates.Visible;
            }
            else if (this.Intent.Extras.Get("Job_Type").ToString() == "Rental")
            {
                FromTitle.Visibility = ViewStates.Gone;
                FromDisplay.Visibility = ViewStates.Gone;
                ToTitle.Visibility = ViewStates.Gone;
                ToDisplay.Visibility = ViewStates.Gone;
                LocationTitle.Visibility = ViewStates.Visible;
                LocationDisplay.Visibility = ViewStates.Visible;
                DateTitle.Visibility = ViewStates.Gone;
                DateDisplay.Visibility = ViewStates.Gone;
                DateFromTitle.Visibility = ViewStates.Visible;
                DateFromDisplay.Visibility = ViewStates.Visible;
                DateToTitle.Visibility = ViewStates.Visible;
                DateToDisplay.Visibility = ViewStates.Visible;
            }
            else
            {
                FromTitle.Visibility = ViewStates.Gone;
                FromDisplay.Visibility = ViewStates.Gone;
                ToDisplay.Visibility = ViewStates.Gone;
                ToTitle.Visibility = ViewStates.Gone;
                LocationTitle.Visibility = ViewStates.Visible;
                LocationDisplay.Visibility = ViewStates.Visible;
                DateTitle.Visibility = ViewStates.Gone;
                DateDisplay.Visibility = ViewStates.Gone;
                DateToTitle.Visibility = ViewStates.Visible;
                DateToDisplay.Visibility = ViewStates.Visible;
                DateFromTitle.Visibility = ViewStates.Visible;
                DateFromDisplay.Visibility = ViewStates.Visible;
            }
            JobTypeDisplay.Text = this.Intent.Extras.Get("Job_Type").ToString();
            JobDescriptionDisplay.Text = this.Intent.Extras.Get("Job_Description").ToString();
            FromDisplay.Text = this.Intent.Extras.Get("From").ToString();
            ToDisplay.Text = this.Intent.Extras.Get("To").ToString();
            DateDisplay.Text = this.Intent.Extras.Get("Date").ToString();
            PriceDisplay.Text = this.Intent.Extras.Get("Price").ToString();
            LocationDisplay.Text = this.Intent.Extras.Get("Location").ToString();
            DateFromDisplay.Text = this.Intent.Extras.Get("Date_From").ToString();
            DateToDisplay.Text = this.Intent.Extras.Get("Date_To").ToString();
            Status.Text = "Status: "+this.Intent.Extras.Get("Status").ToString();
        }
        public void EventHandler()
        {
            SentButtonNegotiation.Click += SentNegotiation;
            JobCompleted.Click += MarkCompleted;
        }
        public void SentNegotiation(object sender,EventArgs e)
        {
            try
            {
                var firebase = new FirebaseClient(FirebaseUrl);
                var nego = firebase.Child("Negotiation").PostAsync<string>(TextSendNegotiation.Text);
            }
            catch
            {

            }
        }
        public void MarkCompleted(object sender, EventArgs e)
        {

        }
        public void InitChatNegotiation()
        {
            var firebase = new FirebaseClient(FirebaseUrl);
            var chat = firebase
                        .Child("Negotiation")
                        .AsObservable<Negotiation>()
                        .Subscribe(d =>ChatList(d.Object.Chat,d.EventType.ToString()));
        }
        public void ChatList(string chat,string stats)
        {
            if (stats == "InsertOrUpdate")
            {
                chatlist.Add(chat);
                var item = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, chatlist);
                Negotiations.Adapter = item;
            }
            else
            {

            }
        }
    }
}