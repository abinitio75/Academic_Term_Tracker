using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace Academic_Term_Tracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursePage : ContentPage
    {
        private readonly List<Course> courseList;
        List<Assessment> asmts;
        private readonly Course course;

        public CoursePage(Course course, List<Course> courseList)
        {
            this.course = course;
            this.courseList = courseList;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            courseStart.Text = "Start: " + course.Start.ToString("M/d/yyyy");
            courseEnd.Text = "End: " + course.End.ToString("M/d/yyyy");
            course_detail.ItemsSource = PopulateList();
            base.OnAppearing();
        }

        private List<object> PopulateList()
        {
            asmts = DB.GetAssessments(course.ID);

            object courseDetail;

            if (asmts.Count > 0)
            {
                Assessment asmt1 = asmts.FirstOrDefault(a => a.AssessmentType == "Objective Assessment");
                Assessment asmt2 = asmts.FirstOrDefault(a => a.AssessmentType == "Performance Assessment");

                if (Equals(asmt1, null))
                {
                    asmt1 = new Assessment() { Title = "N/A", AssessmentType = "Objective Assessment" };
                }
                else if (Equals(asmt2, null))
                {
                    asmt2 = new Assessment() { Title = "N/A", AssessmentType = "Performance Assessment" };
                }

                courseDetail = new
                {
                    CourseTitle = "Course Title: " + course.Title.ToUpper(),
                    CourseStart = "Start Date:" + course.Start.ToString("M/d/yyyy"),
                    CourseEnd = "End Date: " + course.End.ToString("M/d/yyyy"),
                    CourseStatus = "Course Status: " + course.Status.ToUpper(),
                    CourseNotify = "Date Notification: " + (course.Notify ? "Yes" : "No"),
                    CIName = "Instructor Name: " + course.InstructorName.ToUpper(),
                    CINumber = "Instructor Number: " + course.InstructorNumber.ToUpper(),
                    CIEmail = "Instructor Email: " + course.InstructorEmail.ToUpper(),

                    OATitle = "Assessment Title: " + asmt1.Title.ToUpper(),
                    OAType = "Assessment Type: " + asmt1.AssessmentType.ToUpper(),
                    OANotify = "Date Notification: " + (asmt1.Notify ? "Yes" : "No"),
                    OAStart = "Start Date: " + asmt1.Start.ToString("M/d/yyyy"),
                    OAEnd = "End Date: " + asmt1.End.ToString("M/d/yyyy"),
                    PATitle = "Assessment Title: " + asmt2.Title.ToUpper(),
                    PAType = "Assessment Type: " + asmt2.AssessmentType.ToUpper(),
                    PANotify = "Date Notification: " + (asmt2.Notify ? "Yes" : "No"),
                    PAStart = "Start Date: " + asmt2.Start.ToString("M/d/yyyy"),
                    PAEnd = "End Date: " + asmt2.End.ToString("M/d/yyyy")
                };
                btnAsmt.Text = "Edit Assessments";
            }
            else
            {
                courseDetail = new
                {
                    CourseTitle = "Course Title: " + course.Title.ToUpper(),
                    CourseStart = "Start Date: " + course.Start.ToString("M/d/yyyy"),
                    CourseEnd = "End Date: " + course.End.ToString("M/d/yyyy"),
                    CourseStatus = "Course Status: " + course.Status.ToUpper(),
                    CourseNotify = "Date Notification: " + (course.Notify ? "Yes" : "No"),
                    CIName = "Instructor Name: " + course.InstructorName.ToUpper(),
                    CINumber = "Instructor Number: " + course.InstructorNumber.ToUpper(),
                    CIEmail = "Instructor Email: " + course.InstructorEmail.ToUpper(),

                    OATitle = "Assessment Title: ",
                    OAType = "Assessment Type: ",
                    OANotify = "Date Notification: ",
                    OAStart = "Start Date: ",
                    OAEnd = "End Date: ",
                    PATitle = "Assessment Title: ",
                    PAType = "Assessment Type: ",
                    PANotify = "Date Notification: ",
                    PAStart = "Start Date: ",
                    PAEnd = "End Date: "
                };

                btnAsmt.Text = "Add Assessment";
            }
            return new List<object>() { courseDetail };
        }

        private async void EditAsmts_Clicked(object sender, EventArgs e)
        {
            asmts = DB.GetAssessments(course.ID);

            if(asmts.Count == 0)
            {
                await Navigation.PushAsync(new EditAssessmentsPage(course.ID) { Title = "Add Assessments" });
            }
            else
            {
                await Navigation.PushAsync(new EditAssessmentsPage(course.ID, asmts) { Title = "Edit Assessments" });
            }
        }

        private async void DeleteAsmt_Clicked(object sender, EventArgs e)
        {
            if(asmts.Count == 0)
            {
                await DisplayAlert("", "Nothing to Delete", "OK");
            }
            else if(asmts.Count == 1)
            {
                bool confirm = await DisplayAlert("Confirm", "Really delete the assessment?", "Yes", "No");
                
                if (confirm)
                {
                    _ = await DB.DeleteAssessments(course.ID);
                    course_detail.ItemsSource = PopulateList();
                }
            }
            else
            {
                string choice = await DisplayActionSheet("Which Assessment to Delete?", "Cancel", null, $"{asmts[0].Title}", $"{asmts[1].Title}", "Both");
                
                if(choice != "Cancel")
                {
                    if (choice == "Both")
                    {
                        _ = await DB.DeleteAssessments(course.ID);
                    }
                    else
                    {
                        _ = await DB.DeleteAssessment(asmts.FirstOrDefault(a => a.Title == choice).ID);
                    }
                    course_detail.ItemsSource = PopulateList();
                }
            }
        }

        private async void Notes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotesPage(course.ID) { Title = $"{course.Title} Notes" });
        }

        private async void EditCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditCoursePage(course, courseList) { Title = "Edit Course" });
        }

        private async void DeleteCourse_Clicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Confirm", "Really delete course and associated data?", "Yes", "Cancel");

            if (confirmed)
            {
                _ = await DB.DeleteAssessments(course.ID);
                _ = await DB.DeleteCourse(course.ID);
                _ = await Navigation.PopAsync();
            }
        }
    }
}