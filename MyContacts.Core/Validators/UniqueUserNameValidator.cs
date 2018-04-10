using System;
using System.Linq;
using System.Collections.Generic;
using FluentValidation.Validators;
using MyContacts.SqlClient;

namespace MyContacts.Core.Validators
{
    public class UniqueUserNameValidator<T> : PropertyValidator
    {
        private MyContactsContext _context;
        
        public UniqueUserNameValidator(MyContactsContext context) : base("The {PropertyName} field must be unique. Duplicates are not allowed")
        {
            _context = context;
        }

        /**
        * Validate the uniquess of the Name property that is passed to the DTO.
        * Duplicate names are not allowed. 
        */
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var fieldValue = _context.Contacts.Where(v => v.User.UserName == (string)context.PropertyValue).FirstOrDefault();
        
            if (fieldValue == null)
            {
                return true;
            }

            return false;
        }
    }
}