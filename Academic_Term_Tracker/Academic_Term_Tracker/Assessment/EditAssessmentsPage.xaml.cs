using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Academic_Term_Tracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAssessmentsPage : ContentPage
    {
        private List<Assessment> asmts;
        private readonly int courseID;
        private readonly bool newAsmt = false;
        private bool insertNew = false;

        public EditAssessmentsPage(int courseID)
        {
            this.courseID = courseID;
            newAsmt = true;
            InitializeComponent();
        }

        public EditAssessmentsPage(int courseID, List<Assessment> asmts)
        {
            this.asmts = asmts;
            this.courseID = courseID;
            InitializeComponent();
            SetLabels();
        }

        private void SetLabels()
        {
            if (!newAsmt)
            {
                if (asmts.Count == 1)
                {
                    if (asmts[0].AssessmentType == "Objective Assessment")
                    {
                        asmts.Add(new Assessment { Title = " ", AssessmentType = "Performance Assessment"});
                    }
                    else
                    {
                        asmts.Add(new Assessment { Title = " ", AssessmentType = "Objective Assessment" });
                    }
                    insertNew = true;
                }

                asmt1Title.Text = asmts[0].Title;
                notify1.IsChecked = asmts[0].Notify;
                type1.SelectedItem = asmts[0].AssessmentType;
                start1.Date = asmts[0].Start.Date;
                end1.Date = asmts[0].End.Date;

                asmt2Title.Text = asmts[1].Title;
                notify2.IsChecked = asmts[1].Notify;
                type2.SelectedItem = asmts[1].AssessmentType;
                start2.Date = asmts[1].Start.Date;
                end2.Date = asmts[1].End.Date;
            }
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (InputValid())
            {
                if (newAsmt)
                {
                    Assessment asmt = new Assessment
                    {
                        Title = asmt1Title.Text,
                        Notify = notify1.IsChecked,
                        AssessmentType = type1.SelectedItem.ToString(),
                        Start = start1.Date,
                        End = end1.Date,
                        CourseID = courseID
                    };
                    _ = await DB.InsertAssessment(asmt);

                    asmt = new Assessment()
                    {
                        Title = asmt2Title.Text,
                        Notify = notify2.IsChecked,
                        AssessmentType = type2.SelectedItem.ToString(),
                        Start = start2.Date,
                        End = end2.Date,
                        CourseID = courseID
                    };
                    _ = await DB.InsertAssessment(asmt);
                }
                else
                {
                    asmts[0].Title = asmt1Title.Text;
                    asmts[0].Notify = notify1.IsChecked;
                    asmts[0].AssessmentType = type1.SelectedItem.ToString();
                    asmts[0].Start = start1.Date;
                    asmts[0].End = end1.Date;
                    asmts[0].CourseID = courseID;

                    asmts[1].Title = asmt2Title.Text;
                    asmts[1].Notify = notify2.IsChecked;
                    asmts[1].AssessmentType = type2.SelectedItem.ToString();
                    asmts[1].Start = start2.Date;
                    asmts[1].End = end2.Date;
                    asmts[1].CourseID = courseID;
                    
                    if (insertNew)
                    {
                        _ = await DB.UpdateAssessment(asmts[0]);
                        _ = await DB.InsertAssessment(asmts[1]);
                    }
                    else
                    {
                        _ = await DB.UpdateAssessment(asmts[0]);
                        _ = await DB.UpdateAssessment(asmts[1]);
                    }
                }
                _ = await Navigation.PopAsync();
            }
        }

        private bool InputValid()
        {
            if (string.IsNullOrWhiteSpace(asmt1Title.Text) || string.IsNullOrWhiteSpace(asmt2Title.Text))
            {
                _ = DisplayAlert("", "Assessment Title cannot be empty", "OK");
            }
            else if (Equals(type1.SelectedItem, null) || Equals(type2.SelectedItem, null))
            {
                _ = DisplayAlert("", "Please select an assessment type", "OK");
            }
            else if (start1.Date > end1.Date || start2.Date > end2.Date)
            {
                _ = DisplayAlert("Date Error", "Start date should be before the end date", "OK");
            }
            else if (!Equals(type1.SelectedItem, null) && !Equals(type2.SelectedItem, null))
            {
                if (Equals(type1.SelectedItem, type2.SelectedItem))
                {
                    _ = DisplayAlert("", "Only 1 of each assessment type is permitted", "OK");
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}