using System;

namespace Academic_Term_Tracker
{
    public class PlaceHolderData
    {
        public static void CreateData()
        {
            Term term = new Term();
            Course course = new Course();
            Assessment asmtPA = new Assessment();
            Assessment asmtOA = new Assessment();

            term.Title = "Term " + term.ID.ToString();
            term.Start = new DateTime(2021, 10, 1);
            term.End = new DateTime(2022, 4, 1);
            _ = DB.InsertTerm(term);

            course.Title = "Mobile Application Development Using C# – Academic_Term_Tracker";
            course.Status = "In Progress";
            course.Notes = "This is a sample text for the notes section. Lorem ipsum dolor sit amet, " +
                "consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna " +
                "aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip " +
                "ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum " +
                "dolore eu fugiat nulla pariatur.";
            course.InstructorName = "John Doe";
            course.InstructorNumber = "(555)555-5555";
            course.InstructorEmail = "jdoe@gmail.com";
            course.Notify = true;
            course.Start = new DateTime(2022, 1, 26);
            course.End = new DateTime(2022, 1, 31);
            course.TermID = DB.GetTermID();
            _ = DB.InsertCourse(course);

            int courseID = DB.GetCourseID();
            
            asmtPA.Title = "LAP1";
            asmtPA.AssessmentType = "Performance Assessment";
            asmtPA.Start = new DateTime(2022, 1, 26);
            asmtPA.End = new DateTime(2022, 1, 29);
            asmtPA.Notify = true;
            asmtPA.CourseID = courseID;

            asmtOA.Title = "LAO1";
            asmtOA.AssessmentType = "Objective Assessment";
            asmtOA.Start = new DateTime(2022, 1, 26);
            asmtOA.End = new DateTime(2022, 1, 29);
            asmtOA.Notify = true;
            asmtOA.CourseID = courseID;

            _ = DB.InsertAssessment(asmtOA);
            _ = DB.InsertAssessment(asmtPA);
        }
    }
}
