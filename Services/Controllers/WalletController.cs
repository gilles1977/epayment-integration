using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess.Interface;
using DomainModel;

namespace Services.Controllers
{
    public class WalletController : ApiController
    {
        private readonly IRepository<Wallet> _repository;

        public WalletController(IRepository<Wallet> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Wallet> GetAllWallets()
        {
            return _repository.GetAll();
        }

        public void SaveWallet(Wallet order)
        {
            _repository.Save(order);
        }
    }
}
