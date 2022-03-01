using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLite;

namespace Academic_Term_Tracker
{
    public static class DB
    {
        public static int lastInsertIDTerm = 0;
        public static int lastInsertIDCourse = 0;

        private const string dbFileName = "Academic_Term_Tracker.db3";
        private const SQLiteOpenFlags Flags = SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
        private static readonly SQLiteAsyncConnection connection = new SQLiteAsyncConnection(DBPath, Flags);

        public static string DBPath
        {
            get
            {
                string dbBasePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return System.IO.Path.Combine(dbBasePath, dbFileName);
            }
        }

        public static void DeleteDB() => System.IO.File.Delete(DBPath);

        public static List<Term> GetAllTerms() => connection.Table<Term>().ToListAsync().Result;

        public static async Task<int> InsertTerm(Term term) => await connection.InsertAsync(term);

        public static async Task<int> UpdateTerm(Term term) => await connection.UpdateAsync(term);

        public static async Task<int> DeleteTerm(int TermID) => await connection.Table<Term>().Where(i => i.ID == TermID).DeleteAsync();

        public static List<Course> GetCourses(int TermID) => connection.Table<Course>().Where(c => c.TermID == TermID).ToListAsync().Result;

        public static List<Course> GetCoursesNotify() => connection.Table<Course>().Where(c => c.Notify).ToListAsync().Result;

        public static async Task<int> InsertCourse(Course course) => await connection.InsertAsync(course);

        public static async Task<int> UpdateCourse(Course course) => await connection.UpdateAsync(course);

        public static async Task<int> DeleteCourse(int CourseID) => await connection.Table<Course>().Where(i => i.ID == CourseID).DeleteAsync();

        public static string GetNotes(int CourseID) => connection.FindAsync<Course>(CourseID).Result.Notes;

        public static void SaveNotes(int CourseID, string Notes) => connection.Table<Course>().Where(i => i.ID == CourseID).FirstAsync().Result.Notes = Notes;

        public static void DeleteNotes(int CourseID) => connection.Table<Course>().Where(i => i.ID == CourseID).FirstAsync().Result.Notes = null;

        public static List<Assessment> GetAssessments(int CourseID) => connection.Table<Assessment>().Where(i => i.CourseID == CourseID).ToListAsync().Result;

        public static List<Assessment> GetAssessmentsNotify() => connection.Table<Assessment>().Where(a => a.Notify).ToListAsync().Result;

        public static async Task<int> InsertAssessment(Assessment asmt) => await connection.InsertAsync(asmt);

        public static async Task<int> UpdateAssessment(Assessment asmt) => await connection.UpdateAsync(asmt);

        public static async Task<int> DeleteAssessment(int asmtID) => await connection.Table<Assessment>().Where(i => i.ID == asmtID).DeleteAsync();

        public static async Task<int> DeleteAssessments(int CourseID) => await connection.Table<Assessment>().Where(i => i.CourseID == CourseID).DeleteAsync();

        public static void CreateTables()
        {
            _ = connection.CreateTableAsync<Term>();
            _ = connection.CreateTableAsync<Course>();
            _ = connection.CreateTableAsync<Assessment>();
        }

        public static List<SQLiteConnection.ColumnInfo> GetTableExists(string table) => connection.GetTableInfoAsync(table).Result;

        public static bool CheckTableExists()
        {
            bool exists = true;

            try
            {
                List<SQLiteConnection.ColumnInfo> tableInfo = connection.GetTableInfoAsync("Assessment").Result;
                if (!(tableInfo.Count > 0))
                {
                    exists = false;
                }
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine(e.ToString());
                exists = false;
            }
            catch (AggregateException e)
            {
                Debug.WriteLine(e.ToString());
                exists = false;
            }
            return exists;
        }

        public static int GetTermID() => connection.Table<Term>().FirstOrDefaultAsync().Result.ID;
        public static int GetCourseID() => connection.Table<Course>().FirstOrDefaultAsync().Result.ID;
        public static async Task<int> GetLastTermID() => await connection.Table<Term>().CountAsync() + 1;

        public static async Task<int> GetLastCourseID() => await connection.Table<Course>().CountAsync() + 1;

    }
}