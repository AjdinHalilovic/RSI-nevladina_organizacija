using System;

namespace Core.Entities.Base.DTO
{
    public class PersonUserDTO
    {
        #region Constructors
        public PersonUserDTO(int id, string firstName, string lastName, string gender, DateTime? dateOfBirth, string username, string email, string cultureName, bool changePassword)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Username = username;
            Email = email;
            CultureName = cultureName;
            ChangePassword = changePassword;
        }
        #endregion

        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FirstAndLastName => $"{FirstName} {LastName}";

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public bool Birthday => DateOfBirth?.Date == DateTime.Now.Date;

        public string Username { get; set; }

        public string Email { get; set; }

        public string CultureName { get; set; }

        public bool ChangePassword { get; set; }
    }
}
