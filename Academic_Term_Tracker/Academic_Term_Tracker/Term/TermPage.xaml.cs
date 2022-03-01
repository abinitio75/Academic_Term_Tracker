using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Academic_Term_Tracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermPage : ContentPage
    {
        private readonly Term term;
        private List<Course> courseList;
        private readonly List<Term> termList;

        public TermPage(Term term, List<Term> termList)
        {
            this.termList = termList;
            this.term = term;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            courseList = DB.GetCourses(term.ID);

            course_view.ItemsSource = courseList;
            termStart.Text = "Start: " + term.Start.ToString("M/d/yyyy");
            termEnd.Text = "End: " + term.End.ToString("M/d/yyyy");
            base.OnAppearing();
        }

        private async void CourseTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new CoursePage((Course)e.Item, courseList) { Title = $"{((Course)e.Item).Title}" });
        }

        private async void BtnAddCourse_Clicked(object sender, EventArgs e)
        {
            if (courseList.Where(c => c.TermID == term.ID).Count() == 6)
                await DisplayAlert("", "The maximum number of courses has been reached for this term", "OK");
            else
                await Navigation.PushAsync(new EditCoursePage(term.ID, courseList) { Title = "Add Course" });
        }

        private async void BtnEditTerm_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditTermPage(term, termList) { Title = "Edit Term" });
        }

        private async void BtnDeleteTerm_Clicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Confirm", "Really delete term and associated data?", "Yes", "Cancel");
            
            if (confirmed)
            {
                List<int> coursesToDelete = courseList.Where(i => i.TermID == term.ID).Select(x => x.ID).ToList();

                foreach (int id in coursesToDelete)
                {
                    _ = await DB.DeleteAssessments(id);
                    _ = await DB.DeleteCourse(id);
                }
                _ = await DB.DeleteTerm(term.ID);
                _ = await Navigation.PopAsync();
            }
        }
    }
}