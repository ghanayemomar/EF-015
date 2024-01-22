using EF015.QueryData.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace C01.QueryData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
        }

        private static void QueryData()
        {
            using (var context = new AppDbContext())
            {
                var course = context.Courses.Single(x => x.Id == 1);
                Console.WriteLine($"course name: {course.CourseName}, {course.HoursToComplete} hrs., {course.Price.ToString("C")}");
            }
        }

        private static void ClientVsServerEvaluation()
        {
            using (var context = new AppDbContext())
            {
                var courseId = 1;
                var result = context.Sections.Where(x => x.CourseId == courseId).Select(x => new
                {
                    Id = x.Id,
                    Section = x.SectionName.Substring(4),
                    TotalDate = CalculateTotalDays(x.DateRange.StartDate, x.DateRange.EndDate)
                });
                Console.WriteLine(result.ToQueryString());
            }
            Console.ReadKey();
        }

        private static int CalculateTotalDays(DateOnly startDate, DateOnly endDate)
        {
            return endDate.DayNumber - startDate.DayNumber;
        }

        private static void TrackingvsNoTrackingQuery()
        {
            using (var context = new AppDbContext())
            {
                var section = context.Sections.FirstOrDefault(x => x.Id == 1);
                Console.WriteLine("before changing tracked object");
                Console.WriteLine(section.SectionName);

                section.SectionName = "BlaBla";
                var section02 = context.Sections.FirstOrDefault(x => x.Id == 2);
                context.SaveChanges();
                Console.WriteLine("after changing tracked object");
                Console.WriteLine(section.SectionName);
            }
            Console.ReadKey();
        }

        private static void RelatedData()
        {
            using (var context = new AppDbContext())
            {
                var sectionId = 1;
                var sectionQuery = context.Sections.Include(x => x.Participants).Where(x => x.Id == sectionId);
                Console.WriteLine(sectionQuery.ToQueryString());
                //
                var section = sectionQuery.FirstOrDefault();
                Console.WriteLine($"Section Name: {section.SectionName}");
                //
                foreach (var participant in section.Participants)
                    Console.WriteLine($"[{participant.Id}] {participant.FName} {participant.LName}");
                //
            }
            Console.ReadKey();
        }


        private static void RelatedData02()
        {
            using (var context = new AppDbContext())
            {
                var sectionId = 1;
                var sectionQuery = context.Sections.Include(x => x.Instructor).ThenInclude(x => x.Office).Where(x => x.Id == sectionId);
                Console.WriteLine(sectionQuery.ToQueryString());
                //
                var section = sectionQuery.FirstOrDefault();
                Console.WriteLine($"sectons {section.SectionName} , {section.Instructor.FName} , {section.Instructor.LName}");
                //

                //
            }
            Console.ReadKey();
        }
        private static void ExplicitLoading()
        {
            using (var context = new AppDbContext())
            {
                var sectionId = 1;
                var section = context.Sections.FirstOrDefault(x => x.Id == sectionId);
                //
                var query = context.Entry(section).Collection(x => x.Participants).Query();
                Console.WriteLine(query.ToQueryString());
                //

                //
            }
            Console.ReadKey();
        }
        private static void LazyLoading()
        {
            using (var context = new AppDbContext())
            {
                var sectionId = 1;
                var section = context.Sections.FirstOrDefault(x => x.Id == sectionId);
                //
                var query = context.Entry(section).Collection(x => x.Participants).Query();
                Console.WriteLine(query.ToQueryString());
                //
                // Need to sit all navigation prop virsual and public to work.
                //
            }
            Console.ReadKey();
        }

    }
}
