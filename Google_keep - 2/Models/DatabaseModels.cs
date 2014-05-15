using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Web.Helpers;
using Google_keep___2.Models;
using System.ComponentModel;

namespace Google_keep___2.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Notities> Notities { get; set; }
        public DbSet<NotitieGroups> NotitieGroups { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("Notities")]
    public class Notities
    {
        [Key]
        public int NotitieId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Note Group is required")]
        public int NotitieGroupId { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [Required (ErrorMessage = "Title is required")]
        public string NotitieTitle { get; set; }
        [Required (ErrorMessage = "Description is required")]
        public string NotitieDescription { get; set; }
        [Required(ErrorMessage = "DateTime is required")]
        public System.DateTime DateTime { get; set; }

        public virtual NotitieGroups NGroups { get; set; }
        public virtual UserProfile UserProfiles { get; set; }
    }

    [Table("NotitieGroups")]
    public class NotitieGroups
    {
        [Key]
        public int NotitieGroupId { get; set; }
        [Required(ErrorMessage = "Note Group is required")]
        public string NotitieGroup { get; set; }

        public virtual List<Notities> Notities { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
