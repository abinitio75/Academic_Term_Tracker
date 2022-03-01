using System;
using SQLite;

namespace Academic_Term_Tracker
{
    [Table("Course")]
    public class Course
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }
        [MaxLength(25)]
        public string Title { get; set; }
        public string Status { get; set; }
        [MaxLength(500)]
        public string Notes { get; set; }
        [MaxLength(50)]
        public string InstructorName { get; set; }
        [MaxLength(50)]
        public string InstructorEmail { get; set; }
        [MaxLength(14)]
        public string InstructorNumber { get; set; }
        public bool Notify { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int TermID { get; set; }
    
    }
}
