using ElyriaAlumniAssociation.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ElyriaAlumniAssociation.ViewModels
{
    public class AlumnusViewModel
    {
        public int Id { get; set; }
    
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleInitial { get; set; }
        public string? LastNameAtGraduation { get; set; }
        public string? School { get; set; }
        public int GraduationYear { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool ScholasticAward { get; set; }
        public bool Athletics { get; set; }
        public bool Theatre { get; set; }
        public bool Band { get; set; }
        public bool Choir { get; set; }
        public bool Clubs { get; set; }
        public bool ClassOfficer { get; set; }
        public bool ROTC { get; set; }
        public string? OtherActivities { get; set; }
        public string? CurrentStatus { get; set; }
        public bool Selected { get; set; }
        public SelectListItem Alumni { get; set; }
    }
}
