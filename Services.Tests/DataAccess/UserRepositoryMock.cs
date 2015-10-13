using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Interface;
using DomainModel;

namespace Services.Tests.DataAccess
{
    public class UserRepositoryMock : IRepository<User>
    {
        public IList<User> GetAll()
        {
            var user = new User() { FirstName = "John", LastName = "Doe", Name = "jdo", PasswordHash = "FxjCSxCuuAmeP8RJYKtpSat2omc1JFnyA+oQNr7DgsI=" };

            return new List<User>(new[] { user });
        }

        public void Save(User item)
        {
            throw new NotImplementedException();
        }
    }
}
