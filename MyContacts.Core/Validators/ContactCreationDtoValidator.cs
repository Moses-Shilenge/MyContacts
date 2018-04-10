using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using MyContacts.SqlClient.Tables;
using MyContacts.SqlClient;

namespace MyContacts.Core.Validators
{
    public class ContactCreationDtoValidator : AbstractValidator<ContactDto>
    {
        public ContactCreationDtoValidator(MyContactsContext context)
        {
            RuleFor(contact => contact.User.UserName).NotEmpty().Length(1, 255);
            RuleFor(contact => contact.User.UserName).SetValidator(new UniqueUserNameValidator<ContactDto>(context));
            RuleFor(contact => contact.Email).SetValidator(new UniqueUserNameValidator<ContactDto>(context));

        }
    }
}