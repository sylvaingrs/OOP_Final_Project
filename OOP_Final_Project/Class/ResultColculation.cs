//Roxane Seibel 74527 - Jon Gouspy 74538 - Sylvain Gross 74525 - Leith Chakroun 74529
using System;
using System.Collections.Generic;
using OOP_Final_Project.Models;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;

namespace OOP_Final_Project.Class
{
    public static class ResultCalulation
    {
        private static DBModels db = new DBModels();

        public static Dictionary<Course, double> CalculateAveragePerCourse(Cohort cohort, int studentId)
        {
            Dictionary<Course, double> courseAverages = new Dictionary<Course, double>();

            var courses = db.Course.Where(course => course.Cohort.Any(c => c.Id == cohort.Id));

            foreach (var course in courses)
            {
                var exams = db.Exam.Where(e => e.CourseId == course.Id);

                double sum = 0;
                double coef = 0;

                foreach (var exam in exams)
                {
                    var results = exam.Result.Where(r => r.AccountId == studentId);

                    foreach (var result in results)
                    {
                        sum += result.Grade * exam.Coefficient;
                        coef += exam.Coefficient;
                    }
                }

                double average = coef == 0 ? 0 : sum / coef;

                courseAverages.Add(course, average);
            }

            return courseAverages;
        }
    }
}