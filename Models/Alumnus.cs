
using ElyriaAlumniAssociation.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElyriaAlumniAssociation.Models
{
    public class Alumnus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z\s]+")]
        [StringLength(25)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z\s]+")]
        [StringLength(25)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [RegularExpression(@"[a-zA-Z\s]+")]
        [StringLength(1)]
        public string? MiddleInitial { get; set; }
        [RegularExpression(@"[a-zA-Z\s]+")]
        [StringLength(25)]
        [Display(Name ="Maiden/Name at Graduation")]
        public string? LastNameAtGraduation { get; set; }
        [Required]
        public string? School { get; set; }
        [Required]
        [RangeUntilCurrentYear(1900)]
        [Display(Name ="Graduation Year")]
        public int GraduationYear { get; set; }
        [Required]
        [RegularExpression(@"[\w\s]+")]
        [Display(Name ="Street Address")]
        public string? StreetAddress { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z\s]+")]
        [StringLength(25)]
        public string? City { get; set; }
        [Required]
        [RegularExpression(@"[a-zA-Z\s]+")]
        [StringLength(25)]
        public string? Country { get; set; }
        [Required]
        [RegularExpression(@"[\w\s\-]+")]
        [StringLength(25)]
        [Display(Name ="Postal Code")]
        public string? PostalCode { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(55)]
        [Display(Name ="Email Address")]
        public string? EmailAddress { get; set; }
        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }
        public bool ScholasticAward { get; set; }
        public bool Athletics { get; set; }
        public bool Theatre { get; set; }
        public bool Band { get; set; }
        public bool Choir { get; set; }
        public bool Clubs { get; set; }
        [Display(Name ="Class Officer")]
        public bool ClassOfficer { get; set; }
        public bool ROTC { get; set; }
        [Display(Name ="Other Activities")]
        public string? OtherActivities { get; set; }
        [Display(Name ="Current Status")]
        public string? CurrentStatus { get; set; }
        [NotMapped]
        public bool? Selected { get; set; }


    }
}
