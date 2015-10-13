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
    public class OrdersController : ApiController
    {
        private readonly IRepository<Order> _repository;

        public OrdersController(IRepository<Order> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _repository.GetAll();
        }

        public void SaveOrder(Order order)
        {
            _repository.Save(order);
        }
    }
}
