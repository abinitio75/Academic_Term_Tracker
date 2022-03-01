using System;
using SQLite;

namespace Academic_Term_Tracker
{
    [Table("Assessment")]
    public class Assessment
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }
        [MaxLength(25)]
        public string Title { get; set; }
        public string AssessmentType { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool Notify { get; set; }
        public int CourseID { get; set; }
    }
}
