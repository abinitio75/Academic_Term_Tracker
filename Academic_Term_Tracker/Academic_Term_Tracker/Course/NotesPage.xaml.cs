using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Academic_Term_Tracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPage : ContentPage
    {
        private readonly int courseID;
        private readonly string notes;
        private bool saved = false;

        public NotesPage(int courseID)
        {
            this.courseID = courseID;
            InitializeComponent();
            notes = DB.GetNotes(courseID);
            courseNotes.Text = notes;
        }

        private async void BtnShare_Clicked(object sender, EventArgs e)
        {
            if (NotesValid())
                await Xamarin.Essentials.Share.RequestAsync(notes);
            else
                await DisplayAlert("", "Nothing to share", "OK");
        }

        private bool NotesValid() => !string.IsNullOrWhiteSpace(courseNotes.Text);

        private void CourseNotes_Unfocused(object sender, FocusEventArgs e)
        {
            if (saved && !NotesValid())
            {
                DB.DeleteNotes(courseID);
                saved = false;
            }
            else if (NotesValid())
            {
                DB.SaveNotes(courseID, notes);
                saved = true;
            }
        }
    }
}