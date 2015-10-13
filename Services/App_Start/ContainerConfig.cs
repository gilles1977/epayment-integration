using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Common.Factories;
using DataAccess.Base;
using Microsoft.Practices.Unity;
using System.Configuration;
using DataAccess.Interface;
using DomainModel;
using Services.Controllers;

namespace Services
{
    public class ContainerConfig
    {
        public static void Initialize()
        {
            var container = new UnityContainer();

            var productDataPath = ConfigurationManager.AppSettings["ProductDataPath"] ?? @"~\App_Data\Products.json";
            container.RegisterInstance<IRepository<Product>>(new Repository<Product>(HttpContext.Current.Server.MapPath(productDataPath)));

            var userDataPath = ConfigurationManager.AppSettings["UserDataPath"] ?? @"~\App_Data\Users.json";
            container.RegisterInstance<IRepository<User>>(new Repository<User>(HttpContext.Current.Server.MapPath(userDataPath)));

            var orderDataPath = ConfigurationManager.AppSettings["OrderDataPath"] ?? @"~\App_Data\Orders.json";
            container.RegisterInstance<IRepository<Order>>(new Repository<Order>(HttpContext.Current.Server.MapPath(orderDataPath)));

            var walletDataPath = ConfigurationManager.AppSettings["WalletDataPath"] ?? @"~\App_Data\Wallet.json";
            container.RegisterInstance<IRepository<Wallet>>(new Repository<Wallet>(HttpContext.Current.Server.MapPath(walletDataPath)));

            container.RegisterType<ProductsController>();
            container.RegisterType<UserAuthController>();
            container.RegisterType<OrdersController>();
            container.RegisterType<WalletController>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityApiDependencyResolver(container);
        }
    }
}