using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using DataAccess.Interface;
using DomainModel;
using Common.Utils;
using DomainModel.Models;

namespace Services.Controllers
{
    public class UserAuthController : ApiController
    {
        private readonly IRepository<User> _repository;

        public UserAuthController(IRepository<User> repository)
        {
            _repository = repository;
        }

        public HttpResponseMessage PostValidate(UserModel userModel)
        {
            var user = _repository.GetAll().FirstOrDefault(u => u.Name.ToLowerInvariant() == userModel.UserName.ToLowerInvariant());


            if (!IsValid(user, userModel.Password))
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User or password is incorrect");

            var response = Request.CreateResponse(HttpStatusCode.OK, user);
            return response;
        }

        public User GetUser(string userName)
        {
            return _repository.GetAll().FirstOrDefault(u => u.Name.ToLowerInvariant() == userName.ToLowerInvariant());
        }

        public IList<User> GetAllUsers()
        {
            return _repository.GetAll();
        }

        private bool IsValid(User user, string password)
        {
            if (user != null)
            {
                return password == user.PasswordHash;
            }
            return false;
        }

    }
}
