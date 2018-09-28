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
using System.Net;
using Newtonsoft.Json;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;


namespace App1
{
    [Activity(Label = "Menu")]
    public class Menu : Activity
    {
        //View Declaration
        Spinner picker;
        TextView JobsLabel, DescriptionLabel, FromLabel, ToLabel, LocationLabel, DateFromLabel, DateToLabel, PriceLabel, DateLabel;
        EditText Description, Price;
        AutoCompleteTextView To, Location, From;
        DatePicker DateFrom, DateTo, Date;
        Button Submit, Logout;
        FirebaseAuth auth;
        ListView joblist;
        TabHost host;

        //Google place prediction array
        string[] strPredictiveTextTo;
        string autoCompleteOptionsTo;
        string[] strPredictiveTextFrom;
        string autoCompleteOptionsFrom;
        string[] strPredictiveTextLocation;
        string autoCompleteOptionsLocation;

        //Google place ApiKey
        const string strGoogleApiKey = "AIzaSyAorXPfTH-Ztbr90RRuC8DSlTqI3yNiNHA";

        //Get places class object
        googlemapplaceclass.GoogleMapPlaceClass objMapClassTo;
        googlemapplaceclass.GoogleMapPlaceClass objMapClassFrom;
        googlemapplaceclass.GoogleMapPlaceClass objMapClassLocation;

        //other declaration including Firebase database url and Google place url
        int index = 0;
        string FirebaseUrl = "https://macaimy-386c2.firebaseio.com";
        const string strAutoCompleteGoogleApi = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=";
        string JobsTypes;

        //List declaration
        List<string> itemJobType = new List<string>();
        List<string> itemDate = new List<string>();
        List<string> itemDateFrom = new List<string>();
        List<string> itemDateTo = new List<string>();
        List<string> itemFrom = new List<string>();
        List<string> itemJobDescription = new List<string>();
        List<string> itemLocation = new List<string>();
        List<string> itemPrice = new List<string>();
        List<string> itemTo = new List<string>();
        List<string> itemStatus = new List<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);
            FirebaseInit();
            InitView();
            InitTab();
            InitStreamFirebase();
            InitPicker();
            SetEventHandler();
        }

        public void FirebaseInit()
        {
            auth = FirebaseAuth.GetInstance(MainActivity.app);
        }

        public void LogoutEvent(object sender, EventArgs e)
        {
            try
            {
                auth.SignOut();
                Intent Login = new Intent(this, typeof(MainActivity));
                StartActivity(Login);
            }
            catch
            {
                Toast.MakeText(this, "Failed to Signout", ToastLength.Short).Show();
            }
        }

        public void JobPicked(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (picker.GetItemAtPosition(e.Position).ToString() == "Sent Item" || picker.GetItemAtPosition(e.Position).ToString() == "Pick up Item" || picker.GetItemAtPosition(e.Position).ToString() == "Buy Item")
            {
                JobsTypes = picker.GetItemAtPosition(e.Position).ToString();
                JobsLabel.Visibility = ViewStates.Visible;
                picker.Visibility = ViewStates.Visible;
                DescriptionLabel.Visibility = ViewStates.Visible;
                Description.Visibility = ViewStates.Visible;
                FromLabel.Visibility = ViewStates.Visible;
                From.Visibility = ViewStates.Visible;
                ToLabel.Visibility = ViewStates.Visible;
                To.Visibility = ViewStates.Visible;
                LocationLabel.Visibility = ViewStates.Gone;
                Location.Visibility = ViewStates.Gone;
                DateFromLabel.Visibility = ViewStates.Gone;
                DateFrom.Visibility = ViewStates.Gone;
                DateToLabel.Visibility = ViewStates.Gone;
                DateTo.Visibility = ViewStates.Gone;
                DateLabel.Visibility = ViewStates.Visible;
                Date.Visibility = ViewStates.Visible;
                PriceLabel.Visibility = ViewStates.Visible;
                Price.Visibility = ViewStates.Visible;
            }
            else if (picker.GetItemAtPosition(e.Position).ToString() == "Fix Item" || picker.GetItemAtPosition(e.Position).ToString() == "Clean Item" || picker.GetItemAtPosition(e.Position).ToString() == "Healthcare Services")
            {
                JobsTypes = picker.GetItemAtPosition(e.Position).ToString();
                JobsLabel.Visibility = ViewStates.Visible;
                picker.Visibility = ViewStates.Visible;
                DescriptionLabel.Visibility = ViewStates.Visible;
                Description.Visibility = ViewStates.Visible;
                FromLabel.Visibility = ViewStates.Gone;
                From.Visibility = ViewStates.Gone;
                ToLabel.Visibility = ViewStates.Gone;
                To.Visibility = ViewStates.Gone;
                LocationLabel.Visibility = ViewStates.Visible;
                Location.Visibility = ViewStates.Visible;
                DateFromLabel.Visibility = ViewStates.Gone;
                DateFrom.Visibility = ViewStates.Gone;
                DateToLabel.Visibility = ViewStates.Gone;
                DateTo.Visibility = ViewStates.Gone;
                DateLabel.Visibility = ViewStates.Visible;
                Date.Visibility = ViewStates.Visible;
                PriceLabel.Visibility = ViewStates.Visible;
                Price.Visibility = ViewStates.Visible;
            }
            else if (picker.GetItemAtPosition(e.Position).ToString() == "Rental")
            {
                JobsTypes = picker.GetItemAtPosition(e.Position).ToString();
                JobsLabel.Visibility = ViewStates.Visible;
                picker.Visibility = ViewStates.Visible;
                DescriptionLabel.Visibility = ViewStates.Visible;
                Description.Visibility = ViewStates.Visible;
                FromLabel.Visibility = ViewStates.Gone;
                From.Visibility = ViewStates.Gone;
                ToLabel.Visibility = ViewStates.Gone;
                To.Visibility = ViewStates.Gone;
                LocationLabel.Visibility = ViewStates.Visible;
                Location.Visibility = ViewStates.Visible;
                DateFromLabel.Visibility = ViewStates.Visible;
                DateFrom.Visibility = ViewStates.Visible;
                DateToLabel.Visibility = ViewStates.Visible;
                DateTo.Visibility = ViewStates.Visible;
                DateLabel.Visibility = ViewStates.Gone;
                Date.Visibility = ViewStates.Gone;
                PriceLabel.Visibility = ViewStates.Visible;
                Price.Visibility = ViewStates.Visible;
            }
            else
            {
                JobsTypes = picker.GetItemAtPosition(e.Position).ToString();
                JobsLabel.Visibility = ViewStates.Visible;
                picker.Visibility = ViewStates.Visible;
                DescriptionLabel.Visibility = ViewStates.Visible;
                Description.Visibility = ViewStates.Visible;
                FromLabel.Visibility = ViewStates.Gone;
                From.Visibility = ViewStates.Gone;
                ToLabel.Visibility = ViewStates.Gone;
                To.Visibility = ViewStates.Gone;
                LocationLabel.Visibility = ViewStates.Visible;
                Location.Visibility = ViewStates.Visible;
                DateFromLabel.Visibility = ViewStates.Visible;
                DateFrom.Visibility = ViewStates.Visible;
                DateToLabel.Visibility = ViewStates.Visible;
                DateTo.Visibility = ViewStates.Visible;
                DateLabel.Visibility = ViewStates.Gone;
                Date.Visibility = ViewStates.Gone;
                PriceLabel.Visibility = ViewStates.Visible;
                Price.Visibility = ViewStates.Visible;
            }
        }

        public async void Submits(object sender, EventArgs e)
        {
            var firebase = new FirebaseClient(FirebaseUrl);
            if (JobsTypes=="Sent Item" ||JobsTypes=="Pick up Item"||JobsTypes=="Buy Item"||JobsTypes=="Fix Item"||JobsTypes=="Clean Item"||JobsTypes=="Healthcare Services")
            {
                try
                {
                    Jobs job = new Jobs();
                    job.JobsType = JobsTypes;
                    job.JobDescription = Description.Text;
                    job.Date = Date.DateTime.ToShortDateString();
                    job.DateFrom = "";
                    job.DateTo = "";
                    job.Location = Location.Text;
                    job.Price = Price.Text;
                    job.To = To.Text;
                    job.From = From.Text;
                    job.Status = "Published";
                    var item = await firebase.Child("Jobs").PostAsync<Jobs>(job);
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    Jobs job = new Jobs();
                    job.JobsType = JobsTypes;
                    job.JobDescription = Description.Text;
                    job.Date = "";
                    job.DateFrom = DateFrom.DateTime.ToShortDateString();
                    job.DateTo = DateTo.DateTime.ToShortDateString();
                    job.Location = Location.Text;
                    job.Price = Price.Text;
                    job.To = To.Text;
                    job.From = From.Text;
                    job.Status = "Published";
                    var item = await firebase.Child("Jobs").PostAsync<Jobs>(job);
                }
                catch
                {

                }
            }
        }

        public void UpdateData(string DataJobsType, string DataDate, string DataDateFrom, string DataDateTo, string DataFrom, string DataJobDescription, string DataLocation, string DataPrice, string DataTo,string Status, string stats)
        {
            if (stats == "InsertOrUpdate")
            {
                itemJobType.Add(DataJobsType);
                itemDate.Add(DataDate);
                itemDateFrom.Add(DataDateFrom);
                itemDateTo.Add(DataDateTo);
                itemFrom.Add(DataFrom);
                itemJobDescription.Add(DataJobDescription);
                itemLocation.Add(DataLocation);
                itemPrice.Add(DataPrice);
                itemTo.Add(DataTo);
                itemStatus.Add(Status);
                var list = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, itemJobType);
                joblist.Adapter = list;
            }
            else
            {
                itemJobType.Remove(DataJobsType);
                itemDate.Remove(DataDate);
                itemDateFrom.Remove(DataDateFrom);
                itemDateTo.Remove(DataDateTo);
                itemFrom.Remove(DataFrom);
                itemJobDescription.Remove(DataJobDescription);
                itemLocation.Remove(DataLocation);
                itemPrice.Remove(DataPrice);
                itemTo.Remove(DataTo);
                itemStatus.Remove(Status);
                var list = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, itemJobType);
                joblist.Adapter = list;
            }
        }

        public void InitView()
        {
            host = FindViewById<TabHost>(Resource.Id.tabHost1);
            joblist = FindViewById<ListView>(Resource.Id.Joblist);
            picker = FindViewById<Spinner>(Resource.Id.JobPicker);
            JobsLabel = FindViewById<TextView>(Resource.Id.JobsLabel);
            DescriptionLabel = FindViewById<TextView>(Resource.Id.DescriptionLabel);
            FromLabel = FindViewById<TextView>(Resource.Id.FromLabel);
            ToLabel = FindViewById<TextView>(Resource.Id.ToLabel);
            LocationLabel = FindViewById<TextView>(Resource.Id.LocationLabel);
            DateFromLabel = FindViewById<TextView>(Resource.Id.DateFromLabel);
            DateToLabel = FindViewById<TextView>(Resource.Id.DateToLabel);
            PriceLabel = FindViewById<TextView>(Resource.Id.PriceLabel);
            Description = FindViewById<EditText>(Resource.Id.Description);
            From = FindViewById<AutoCompleteTextView>(Resource.Id.From);
            To = FindViewById<AutoCompleteTextView>(Resource.Id.To);
            Location = FindViewById<AutoCompleteTextView>(Resource.Id.Location);
            Price = FindViewById<EditText>(Resource.Id.Price);
            DateFrom = FindViewById<DatePicker>(Resource.Id.DateFrom);
            DateTo = FindViewById<DatePicker>(Resource.Id.DateTo);
            Submit = FindViewById<Button>(Resource.Id.Submit);
            DateLabel = FindViewById<TextView>(Resource.Id.DateLabel);
            Date = FindViewById<DatePicker>(Resource.Id.Date);
            Logout = FindViewById<Button>(Resource.Id.Logout);
        }

        public void InitTab()
        {
            host.Setup();
            TabHost.TabSpec spec = host.NewTabSpec("Jobs");

            spec.SetContent(Resource.Id.TabOne);
            spec.SetIndicator("Jobs");
            host.AddTab(spec);

            spec = host.NewTabSpec("History");
            spec.SetContent(Resource.Id.TabTwo);
            spec.SetIndicator("History");
            host.AddTab(spec);

            spec = host.NewTabSpec("Settings");
            spec.SetContent(Resource.Id.TabThree);
            spec.SetIndicator("Settings");
            host.AddTab(spec);
        }

        public void SetEventHandler()
        {
            To.TextChanged += async delegate (object sender, Android.Text.TextChangedEventArgs e)
            {
                WebClient webclient = new WebClient();
                try
                {
                    try
                    {
                        autoCompleteOptionsTo = await webclient.DownloadStringTaskAsync(new Uri(strAutoCompleteGoogleApi + To.Text + "&key=" + strGoogleApiKey));
                    }
                    catch
                    {
                        autoCompleteOptionsTo = "Exceptions";
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Unable to Connect to Server!", ToastLength.Long).Show();
                        });
                    }
                    finally
                    {
                        webclient.Dispose();
                        webclient = null;
                    }
                    if (autoCompleteOptionsTo == "Exception")
                    {
                        Toast.MakeText(this, "Unable to connect to server!!!", ToastLength.Short).Show();
                        return;
                    }
                    objMapClassTo = JsonConvert.DeserializeObject<googlemapplaceclass.GoogleMapPlaceClass>(autoCompleteOptionsTo);
                    strPredictiveTextTo = new string[objMapClassTo.predictions.Count];
                    index = 0;
                    foreach (googlemapplaceclass.Prediction objPred in objMapClassTo.predictions)
                    {
                        strPredictiveTextTo[index] = objPred.description;
                        index++;
                    }
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, strPredictiveTextTo);
                    To.Adapter = adapter;
                }
                catch
                {
                    Toast.MakeText(this, "Unable to process at this moment!!!", ToastLength.Short).Show();
                }
            };
            From.TextChanged += async delegate (object sender, Android.Text.TextChangedEventArgs e)
            {
                WebClient webclient = new WebClient();
                try
                {
                    try
                    {
                        autoCompleteOptionsFrom = await webclient.DownloadStringTaskAsync(new Uri(strAutoCompleteGoogleApi + From.Text + "&key=" + strGoogleApiKey));
                    }
                    catch
                    {
                        autoCompleteOptionsFrom = "Exceptions";
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Unable to Connect to Server!", ToastLength.Long).Show();
                        });
                    }
                    finally
                    {
                        webclient.Dispose();
                        webclient = null;
                    }
                    if (autoCompleteOptionsFrom == "Exception")
                    {
                        Toast.MakeText(this, "Unable to connect to server!!!", ToastLength.Short).Show();
                        return;
                    }
                    objMapClassFrom = JsonConvert.DeserializeObject<googlemapplaceclass.GoogleMapPlaceClass>(autoCompleteOptionsFrom);
                    strPredictiveTextFrom = new string[objMapClassFrom.predictions.Count];
                    index = 0;
                    foreach (googlemapplaceclass.Prediction objPred in objMapClassFrom.predictions)
                    {
                        strPredictiveTextFrom[index] = objPred.description;
                        index++;
                    }
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, strPredictiveTextFrom);
                    From.Adapter = adapter;
                }
                catch
                {
                    Toast.MakeText(this, "Unable to process at this moment!!!", ToastLength.Short).Show();
                }
            };
            Location.TextChanged += async delegate (object sender, Android.Text.TextChangedEventArgs e)
            {
                WebClient webclient = new WebClient();
                try
                {
                    try
                    {
                        autoCompleteOptionsLocation = await webclient.DownloadStringTaskAsync(new Uri(strAutoCompleteGoogleApi + Location.Text + "&key=" + strGoogleApiKey));
                    }
                    catch
                    {
                        autoCompleteOptionsLocation = "Exceptions";
                        RunOnUiThread(() =>
                        {
                            Toast.MakeText(this, "Unable to Connect to Server!", ToastLength.Long).Show();
                        });
                    }
                    finally
                    {
                        webclient.Dispose();
                        webclient = null;
                    }
                    if (autoCompleteOptionsLocation == "Exception")
                    {
                        Toast.MakeText(this, "Unable to connect to server!!!", ToastLength.Short).Show();
                        return;
                    }
                    objMapClassLocation = JsonConvert.DeserializeObject<googlemapplaceclass.GoogleMapPlaceClass>(autoCompleteOptionsLocation);
                    strPredictiveTextLocation = new string[objMapClassLocation.predictions.Count];
                    index = 0;
                    foreach (googlemapplaceclass.Prediction objPred in objMapClassLocation.predictions)
                    {
                        strPredictiveTextLocation[index] = objPred.description;
                        index++;
                    }
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, strPredictiveTextLocation);
                    Location.Adapter = adapter;
                }
                catch
                {
                    Toast.MakeText(this, "Unable to process at this moment!!!", ToastLength.Short).Show();
                }
            };
            picker.ItemSelected += JobPicked;
            Logout.Click += LogoutEvent;
            Submit.Click += Submits;
            joblist.ItemClick += ListSelected;
        }

        public void InitStreamFirebase()
        {
            var firebase = new FirebaseClient(FirebaseUrl);
            var observable = firebase
                            .Child("Jobs")
                            .AsObservable<Jobs>()
                            .Subscribe(d => UpdateData(d.Object.JobsType, d.Object.Date, d.Object.DateFrom, d.Object.DateTo, d.Object.From, d.Object.JobDescription, d.Object.Location, d.Object.Price, d.Object.To,d.Object.Status, d.EventType.ToString()));
        }

        public void InitPicker()
        {
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.JobsType, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            picker.Adapter = adapter;
        }

        public void ListSelected(object sender,AdapterView.ItemClickEventArgs e)
        {
            int dataposition = e.Position;
            Intent JobInto = new Intent(this, typeof(JobInfo));
            JobInto.PutExtra("Job_Type",itemJobType.ElementAt(dataposition));
            JobInto.PutExtra("Job_Description", itemJobDescription.ElementAt(dataposition));
            JobInto.PutExtra("From", itemFrom.ElementAt(dataposition));
            JobInto.PutExtra("To", itemTo.ElementAt(dataposition));
            JobInto.PutExtra("Date", itemDate.ElementAt(dataposition));
            JobInto.PutExtra("Price", itemPrice.ElementAt(dataposition));
            JobInto.PutExtra("Location", itemLocation.ElementAt(dataposition));
            JobInto.PutExtra("Date_From", itemDateFrom.ElementAt(dataposition));
            JobInto.PutExtra("Date_To", itemDateTo.ElementAt(dataposition));
            JobInto.PutExtra("Status", itemStatus.ElementAt(dataposition));
            StartActivity(JobInto);
        }
    }
}