using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.LocalNotifications;

namespace Academic_Term_Tracker
{
    public partial class MainPage : ContentPage
    {
        public List<Term> termList;

        public MainPage()
        {
            //DB.DeleteDB();

            if (!DB.CheckTableExists())
            {
                DB.CreateTables();
            }
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            termList = DB.GetAllTerms();

            if (termList.Count == 0)
            {
                PlaceHolderData.CreateData();
                termList = DB.GetAllTerms();
            }
            term_view.ItemsSource = termList;
            base.OnAppearing();
            GetNotifications();
        }

        private void GetNotifications()
        {
            List<Course> courseNotifications = DB.GetCoursesNotify();
            List<Assessment> asmtNotifications = DB.GetAssessmentsNotify();
            
            double timeDiff;
            DateTime today = DateTime.Today;
            foreach (Course c in courseNotifications)
            {
                timeDiff = today.Subtract(c.End).TotalDays;
                if (timeDiff <= 0 && timeDiff >= -7)
                {
                    CrossLocalNotifications.Current.Show("Course Notification", $"{c.Title} will be ending on {c.End:M/d/yyyy}");
                    DisplayAlert("Course Notification", $"{c.Title} will be ending on {c.End:M/d/yyyy}", "OK");
                }
                timeDiff = today.Subtract(c.Start).TotalDays;
                if (timeDiff <= 0 && timeDiff >= -7)
                {
                    CrossLocalNotifications.Current.Show("Course Notification", $"{c.Title} will be starting on {c.Start:M/d/yyyy}");
                    DisplayAlert("Course Notification", $"{c.Title} will be starting on {c.Start:M/d/yyyy}", "OK");
                }
            }
            foreach (Assessment a in asmtNotifications)
            {
                timeDiff = today.Subtract(a.End).TotalDays;

                if (timeDiff <= 0 && timeDiff >= -7)
                {
                    CrossLocalNotifications.Current.Show("Assessment Notification", $"{a.Title} will be ending on {a.End:M/d/yyyy}");
                    DisplayAlert("Assessment Notification", $"{a.Title} will be ending on {a.End:M/d/yyyy}", "OK");
                }
                timeDiff = today.Subtract(a.Start).TotalDays;
                if (timeDiff <= 0 && timeDiff >= -7)
                {
                    CrossLocalNotifications.Current.Show("Assessment Notification", $"{a.Title} will be starting on {a.Start:M/d/yyyy}");
                    DisplayAlert("Assessment Notification", $"{a.Title} will be starting on {a.Start:M/d/yyyy}", "OK");
                }
            }
        }

        private async void BtnAddTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTermPage(termList) { Title = "Add Term"});
        }

        private async void TermTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new TermPage((Term)e.Item, termList) { Title = $"{((Term)e.Item).Title}" });
        }
    }
}
