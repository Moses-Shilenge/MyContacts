using System;
using System.Linq;
using System.Collections.Generic;
using FluentValidation.Validators;
using System.Text.RegularExpressions;
using MyContacts.SqlClient;

namespace MyContacts.Core.Validators
{
    public class UniqueEmailValidator<T> : PropertyValidator
    {
        private MyContactsContext _context;

        private const string IS_EMAIL_REGULAR_EXPRESSION = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9][\-a-zA-Z0-9]{0,22}[a-zA-Z0-9]))*$";

        public UniqueEmailValidator(MyContactsContext context) : base("The {PropertyName} field must be unique. Duplicates are not allowed")
        {
            _context = context;
        }

        /**
        * Validate the uniquess of the Name property that is passed to the DTO.
        * Duplicate names are not allowed. 
        */
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var fieldValue = _context.Contacts.Where(v => v.Email == (string)context.PropertyValue).FirstOrDefault();
        
            if (fieldValue == null) return true; // Nothing to check

            Regex pattern = new Regex(IS_EMAIL_REGULAR_EXPRESSION);
            return pattern.IsMatch(fieldValue.ToString().ToLower());
        }
    }
}