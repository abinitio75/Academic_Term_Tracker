using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Academic_Term_Tracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCoursePage : ContentPage
    {
        private readonly List<Course> courseList;
        private Course course;
        private readonly int termID;
        private readonly bool newCourse = false;

        public EditCoursePage(int TermID, List<Course> courseList)
        {
            newCourse = true;
            this.courseList = courseList;
            termID = TermID;
            InitializeComponent();
        }

        public EditCoursePage(Course course, List<Course> courseList)
        {
            this.courseList = courseList;
            this.course = course;
            InitializeComponent();
            SetLabels();
        }

        private void SetLabels()
        {
            title.Text = course.Title;
            start.Date = course.Start.Date;
            end.Date = course.End.Date;
            status.SelectedItem = course.Status;
            notify.IsChecked = course.Notify;
            instName.Text = course.InstructorName;
            instNumber.Text = course.InstructorNumber;
            instEmail.Text = course.InstructorEmail;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (InputValid())
            {
                if (newCourse)
                {
                    course = new Course
                    {
                        Title = title.Text,
                        Status = status.SelectedItem.ToString(),
                        InstructorName = instName.Text,
                        InstructorEmail = instEmail.Text,
                        InstructorNumber = instNumber.Text,
                        Notify = notify.IsChecked,
                        Start = start.Date,
                        End = end.Date,
                        TermID = termID
                    };
                    _ = await DB.InsertCourse(course);
                }
                else
                {
                    course.Title = title.Text;
                    course.Status = status.SelectedItem.ToString();
                    course.InstructorName = instName.Text;
                    course.InstructorEmail = instEmail.Text;
                    course.InstructorNumber = instNumber.Text;
                    course.Notify = notify.IsChecked;
                    course.Start = start.Date;
                    course.End = end.Date;
                    _ = await DB.UpdateCourse(course);
                }
                
                _ = await Navigation.PopAsync();
            }
        }

        private bool InputValid()
        {
            if (string.IsNullOrWhiteSpace(title.Text))
                _ = DisplayAlert("", "Course title cannot be empty", "OK");
            else if (courseList.Any(c => c.Title == title.Text) && newCourse)
                _ = DisplayAlert("Duplicate Course", $"A course with the title \'{title.Text}\' already exists", "OK");
            else if (start.Date > end.Date)
                _ = DisplayAlert("Date Error", "Start date must be before end date", "OK");
            else if (status.SelectedItem == null)
                _ = DisplayAlert("", "Select a course status", "OK");
            else if (string.IsNullOrWhiteSpace(instName.Text))
                _ = DisplayAlert("", "Course must have an instructor assigned", "OK");
            else if (string.IsNullOrWhiteSpace(instEmail.Text))
                _ = DisplayAlert("", "Instructor email is missing", "OK");
            else if (string.IsNullOrWhiteSpace(instNumber.Text))
                _ = DisplayAlert("", "Instructor phone number is missing", "OK");
            else
                return true;

            return false;
        }
    }
}