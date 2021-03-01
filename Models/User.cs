using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UsersManager.Models
{
    public enum PermissionsType { Client, Employé, Administrateur }

    public class User
    {
        public int Id { get; set; }

        [Display(Name = "Nom")]
        [Required(ErrorMessage = "Obligatoire")]
        public string FullName { get; set; }

        [Display(Name = "Courriel")]
        [Required(ErrorMessage = "Obligatoire")]
        [EmailAddress(ErrorMessage = "Adresse invalide")]
        public string Email { get; set; }

        [NotMapped]
        [JsonIgnore]
        [Display(Name = "Confirmation du courriel")]
        [Compare("Email", ErrorMessage = "Le courriel et sa confirmation ne correspondent pas.")]
        public string EmailConfirm { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        [Required(ErrorMessage = "Obligatoire")]
        [StringLength(50, ErrorMessage = "Doit comporter au moins {2} caractères.", MinimumLength = 6)]
        public string Password { get; set; }

        [NotMapped]
        [JsonIgnore]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation du mot de passe")]
        [Compare("Password", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas.")]
        public string PasswordConfirm { get; set; }
        
        [Display(Name = "Téléphone")]
        [Required(ErrorMessage = "Obligatoire")]
        public string Phone { get; set; }

        [Display(Name = "Accès")]
        public PermissionsType Permissions { get; set; }

        [Display(Name = "Création")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Pays")]
        [Required(ErrorMessage = "Choix de pays obligatoire")]
        public int CountryId { get; set; }

        [JsonIgnore]
        [Display(Name = "Pays")]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public User()
        {
            CreationDate = DateTime.Now;
        }

        public void Copy(User source)
        {
            this.Id = source.Id;
            this.Email = source.Email;
            this.EmailConfirm = source.EmailConfirm;
            this.Password = source.Password;
            this.PasswordConfirm = source.PasswordConfirm;
            this.FullName = source.FullName;
            this.Phone = source.Phone;
            this.Permissions = source.Permissions;
            this.CreationDate = source.CreationDate;
            this.CountryId = source.CountryId;
        }


        public void CopyExceptPassword(User source)
        {
            string originalPassword = this.Password;
            this.Copy(source);
            this.Password = originalPassword;
            this.PasswordConfirm = originalPassword;
        }
    }
}