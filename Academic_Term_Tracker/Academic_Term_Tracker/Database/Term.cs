using System;
using SQLite;

namespace Academic_Term_Tracker
{
    [Table("Term")]
    public class Term
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int ID { get; set; }
        [MaxLength(25)]
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
