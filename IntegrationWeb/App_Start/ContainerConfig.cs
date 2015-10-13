using System;
using System.Web.Http;
using System.Web.Mvc;
using Common.Factories;
using DomainModel;
using Microsoft.Practices.Unity;
using IntegrationWeb.Controllers;

namespace IntegrationWeb
{
    public class ContainerConfig
    {
        public static void Initialize()
        {
            try
            {
                var container = new UnityContainer();

                container.RegisterType<IController, ProductsController>("Products");
                container.RegisterType<IController, AdminController>("Admin");

                DependencyResolver.SetResolver(new UnityMvcDependencyResolver(container));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}