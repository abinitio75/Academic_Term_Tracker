using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

namespace Academic_Term_Tracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTermPage : ContentPage
    {
        private Term term;
        private readonly List<Term> termList;
        private readonly bool newTerm = false;

        public EditTermPage(List<Term> termList)
        {
            newTerm = true;
            this.termList = termList;
            InitializeComponent();
        }

        public EditTermPage(Term term, List<Term> termList)
        {
            this.termList = termList;
            this.term = term;
            InitializeComponent();
            SetLabels();
        }

        private void SetLabels()
        {
            title.Text = term.Title;
            start.Date = term.Start.Date;
            end.Date = term.End.Date;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (InputValid())
            {
                if (newTerm)
                {
                    term = new Term
                    {
                        Title = title.Text,
                        Start = start.Date,
                        End = end.Date
                    };
                    _ = await DB.InsertTerm(term);
                }
                else
                {
                    term.Title = title.Text;
                    term.Start = start.Date;
                    term.End = end.Date;
                    _ = await DB.UpdateTerm(term);
                }
                await Navigation.PopToRootAsync();
            }
        }

        private bool InputValid()
        {
            if (termList.Any(t => t.Title == title.Text) && newTerm)
                _ = DisplayAlert("Duplicate Term", $"A term with the title \'{title.Text}\' already exists", "OK");
            else if (string.IsNullOrWhiteSpace(title.Text))
                _ = DisplayAlert("", "Term title cannot be empty", "OK");
            else if (start.Date > end.Date)
                _ = DisplayAlert("Date Error", "Start date must be before end date", "OK");
            else
                return true;

            return false;
        }
    }
}