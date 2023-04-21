using ElyriaAlumniAssociation.Data;
using ElyriaAlumniAssociation.Models;
using Microsoft.EntityFrameworkCore;

namespace ElyriaAlumniAssociation.Utils
{
    public class AlumnusSeeder
    {
        public void SeedAlumni(ApplicationDbContext dbcontent, int count)
        {
            var random = new Random();
            var schools = new[] { "Elyria", "Elyria West"};
            var states = new[] { "California", "New York", "Texas", "Florida", "Illinois" };
            var activities = new[] { "Activity 1", "Activity 2", "Activity 3", "Activity 4", "Activity 5" };
            var statuses = new[] { "Employed","Retired" };
            var graduationYears = Enumerable.Range(1900, DateTime.Now.Year - 1900 + 1).ToList();

            for (int i = 1; i <= count; i++)
            {
               Alumnus alumnus = new Alumnus
                {
                    FirstName = RandomString(10),
                    LastName = RandomString(10),
                    MiddleInitial = RandomString(1),
                    LastNameAtGraduation = RandomString(10),
                    School = schools[random.Next(schools.Length)],
                    GraduationYear = graduationYears[random.Next(graduationYears.Count)],
                    StreetAddress = RandomString(10),
                    City = RandomString(10),
                    Country = "USA",
                    State = states[random.Next(states.Length)],
                    PostalCode = RandomString(5, true),
                    EmailAddress = RandomString(10) + "@" + RandomString(5) + ".com",
                    PhoneNumber = RandomString(10, true),
                    ScholasticAward = random.Next(2) == 1,
                    Athletics = random.Next(2) == 1,
                    Theatre = random.Next(2) == 1,
                    Band = random.Next(2) == 1,
                    Choir = random.Next(2) == 1,
                    Clubs = random.Next(2) == 1,
                    ClassOfficer = random.Next(2) == 1,
                    ROTC = random.Next(2) == 1,
                    OtherActivities = activities[random.Next(activities.Length)],
                    CurrentStatus = statuses[random.Next(statuses.Length)],
                    Selected = false
                };

                dbcontent.Add(alumnus);
            }

            dbcontent.SaveChanges();
        }

        private string RandomString(int length, bool isNumeric = false)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const string numbers = "0123456789";

            var allowedChars = isNumeric ? numbers : chars;
            return new string(Enumerable.Repeat(allowedChars, length)
              .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}

