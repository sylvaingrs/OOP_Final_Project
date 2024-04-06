using System;
using System.Collections.Generic;
using OOP_Final_Project.Models;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;

namespace OOP_Final_Project.Class
{
    public class ResultCalulation
    {
        private DBModels db = new DBModels();

        public Dictionary<string, double> CalculateAveragePerCourse(Cohort cohort, int studentId)
        {
            Dictionary<string, double> courseAverages = new Dictionary<string, double>();

            List<Course> courses = db.Course.Where(course => course.Cohort.Any(c => c.Id == cohort.Id)).ToList();

            foreach (Course course in courses)
            {
                List<Exam> exams = db.Exam.Where(e => e.CourseId == course.Id).ToList();

                if (exams.Any())
                {
                    double sum = 0;
                    double coef = 0;

                    foreach (Exam exam in exams)
                    {
                        var results = exam.Result.Where(r => r.AccountId == studentId);

                        foreach(Result result in results)
                        {
                            sum += result.Grade * exam.Coefficient;
                            coef += exam.Coefficient;
                        }
                    }

                    double average = sum / coef;
                    courseAverages.Add(course.CourseName, average);
                }
                else
                {
                    courseAverages.Add(course.CourseName, 0);
                }
            }

            return courseAverages;
        }
    }
}