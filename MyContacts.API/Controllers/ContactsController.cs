using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyContacts.API.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using MyContacts.Core.Services.Contacts;
using MyContacts.SqlClient.Tables;

namespace MyContacts.API.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private IContactsRepository _contactsRepository;
        private static ContactViewModel _contact;

        public ContactsController(IContactsRepository contactsRepository)
        {
            _contactsRepository = contactsRepository;
            _contact = new ContactViewModel();
        }

        [Route(""), HttpGet, Authorize]
        public async Task<ApiResult<IEnumerable<ContactViewModel>>> GetAllAsync()
        {
            List<ContactViewModel> contacts = new List<ContactViewModel>();

            try
            {
                var contactsList = await _contactsRepository.GetAllAsync();

                foreach (var contactDetails in contactsList.ToList())
                {                    
                    contacts.Add(new ContactViewModel()
                    {
                        Id = contactDetails.Id,
                        Email = contactDetails.Email,
                        PhoneNumber = contactDetails.PhoneNumber,
                        Username = await _contactsRepository.GetUsernameWithEmailAsync(contactDetails.Email)
                    });
                }

                return new ApiResult<IEnumerable<ContactViewModel>>(contacts);
            }
            catch (Exception ex)
            {
                return new ApiResult<IEnumerable<ContactViewModel>>()
                {
                    Error = ex.Message
                };
            }
        }

        [Route("{email}"), HttpGet, Authorize]
        public async Task<ApiResult<ContactViewModel>> GetUser(string email)
        {
           try
            {
                var contact = await _contactsRepository.GetUserWithEmail(email);

                if (contact == null)
                {
                    return new ApiResult<ContactViewModel>()
                    {
                        Error = $"{email} doesn't exist"
                    };
                }

                return new ApiResult<ContactViewModel>(new ContactViewModel()
                {
                    Id = contact.Id,
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber,
                    Username = await _contactsRepository.GetUsernameWithEmailAsync(email)
                });
            }
            catch (Exception ex)
            {
                return new ApiResult<ContactViewModel>()
                {
                    Error = ex.Message
                };
            }
        }

        [Route("create"), HttpPost, Authorize]
        public async Task<ApiResult<ContactViewModel>> CreateContact([FromBody]ContactViewModel contact)
        {
            try
            {
                var exists = _contactsRepository.GetUserWithEmail(contact.Email);

                if (exists.Result.Equals(null))
                {
                    return new ApiResult<ContactViewModel>(null) { Error = $"Contact with the email address {contact.Email}" };
                }

                var contactToCreate = new ContactDto() {
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber, 
                    User = new MyContactsUser {
                        PhoneNumber = contact.PhoneNumber,
                        Email = contact.Email,
                        UserName =  contact.Username
                    },
                    Role = Role.User
                };

                var contactCreated = await _contactsRepository.CreateAsync(contactToCreate);
                return new ApiResult<ContactViewModel>(Mapper.Map<ContactViewModel>(contactCreated));
            }
            catch (Exception ex)
            {
                return new ApiResult<ContactViewModel>()
                {
                    Error = ex.Message
                };
            }
        }

        [Route("update/{email}"), HttpPut, Authorize]
        public async Task<ApiResult<ContactViewModel>> UpdateUser([FromBody]ContactViewModel contact)
        {
            try
            {
                var contactToUpdate = new ContactDto()
                {
                    Id = contact.Id,
                    Email = contact.Email,
                    PhoneNumber = contact.PhoneNumber,
                    User = new MyContactsUser
                    {
                        PhoneNumber = contact.PhoneNumber,
                        Email = contact.Email,
                        UserName = contact.Username
                    },
                    Role = Role.User
                };

                var contactUpdate = await _contactsRepository.UpdateContactAsync(contactToUpdate);
                return new ApiResult<ContactViewModel>(Mapper.Map<ContactViewModel>(contactUpdate));
            }
            catch (Exception ex)
            {
                return new ApiResult<ContactViewModel>()
                {
                    Error = ex.Message
                };
            }
        }

        [Route("delete/{email}"), HttpDelete, Authorize]
        public async Task<ApiResult<string>> DeleteUser(string email)
        {
            try
            {
                var contactToDelete = _contactsRepository.GetUserWithEmail(email);

                if (contactToDelete.Result == null)
                {
                    return new ApiResult<string>()
                    {
                        Error = $"{email} doesn't exist"
                    };
                }

                var contactDeleted = await _contactsRepository.DeleteAsync(contactToDelete.Result);

                return new ApiResult<string>()
                {
                     Result = $"{contactDeleted.Email} has been successfully deleted"
                };
            }
            catch (Exception ex)
            {
                return new ApiResult<string>()
                {
                    Error = ex.Message
                };
            }
        }
    }
}