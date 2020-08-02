using System;
using System.Collections.Generic;
using System.Linq;
using AddBook.Utilities;
using Realms;

namespace AddBook.Models
{
    public class Contact : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }

        public IList<PhoneNumber> PhoneNumbers { get; }
        public IList<EmailAddress> EmailAddresses { get; }

        public bool Validate(out string errorMessage)
        {
            errorMessage = null;

            if (!Validator.IsNotEmpty(FullName))
            {
                errorMessage = "The name cannot be empty";
                return false;
            }

            if (PhoneNumbers.Any(p => !Validator.IsPhoneNumber(p.Number)))
            {
                errorMessage = "At least one of the phone numbers is not valid";
                return false;
            }

            if (EmailAddresses.Any(p => !Validator.IsEmail(p.Address)))
            {
                errorMessage = "At least one of the email addresses is not valid";
                return false;
            }

            return true;
        }
    }
}
