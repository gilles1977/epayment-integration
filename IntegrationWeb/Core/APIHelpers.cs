using DomainModel.RequestModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IntegrationWeb.Core
{
    public class ApiHelpers
    {
        public static async Task<IList<T>> LoadAsync<T>(string ressource)
        {
            IList<T> list = new List<T>();
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationUtil.ApiUrl) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync(ressource);
                    if (!response.IsSuccessStatusCode)
                        return list;
                    list = await response.Content.ReadAsAsync<IList<T>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            return list;
        }

        public static async Task<bool> SaveAsync<T>(T item, string ressource)
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationUtil.ApiUrl) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsJsonAsync(ressource, item);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static T Get<T>(string query) where T : new()
        {
            var item = new T();
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationUtil.ApiUrl) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var task = client.GetAsync(query)
                                     .ContinueWith(responseTask =>
                                     {
                                         var response = responseTask.Result;
                                         if (!response.IsSuccessStatusCode) return;
                                         var jsonTask = response.Content.ReadAsAsync<T>();
                                         jsonTask.Wait();
                                         item = jsonTask.Result;
                                     });
                    task.Wait();
                    return item;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static OrderQueryResponse GetOrderQuery(string requestUri, OrderQueryRequest request)
        {
            var item = new OrderQueryResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                        {
                            {"PSPID", request.PspId},
                            {"USERID", request.UserId},
                            {"PSWD", request.Pswd},
                            {"PAYID", request.PayId}
                        });

                    var task = client.PostAsync(requestUri, content)
                                     .ContinueWith(responseTask =>
                                     {
                                         var response = responseTask.Result;
                                         if (!response.IsSuccessStatusCode) return;
                                         var xmlTask = response.Content.ReadAsStringAsync();
                                         xmlTask.Wait();
                                         var str = xmlTask.Result;
                                         var buffer = Encoding.UTF8.GetBytes(str);
                                         using (var stream = new MemoryStream(buffer))
                                         {
                                             var ser = new XmlSerializer(typeof (OrderQueryResponse));
                                             item = (OrderQueryResponse) ser.Deserialize(stream);
                                         }
                                     });
                    task.Wait();
                    return item;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static async Task<OrderQueryResponse> PostOrderDirectAsync(string requestUri, OrderDirectRequest request)
        {
            var item = new OrderQueryResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                        {
                            {"PSPID", request.PspId},
                            {"USERID", request.UserId},
                            {"PSWD", request.Pswd},
                            {"ORDERID", request.OrderId},
                            {"AMOUNT", request.Amount},
                            {"CURRENCY", request.Currency},
                            {"CARDNO", request.CardNo},
                            {"ED", request.Ed},
                            {"CVC", request.Cvc},
                            {"OPERATION", request.Operation},
                            {"CN", request.Cn},
                            {"PM", request.Pm},
                            {"BRAND", request.Brand},
                            {"ALIAS", request.Alias}
                        });

                    var response = await client.PostAsync(requestUri, content);
                    if (!response.IsSuccessStatusCode)
                        throw new ApplicationException(
                            string.Format("Error while requesting direct order: status {0}", 
                            response.StatusCode, await response.Content.ReadAsStringAsync()));

                    var str = await response.Content.ReadAsStringAsync();
                    var buffer = Encoding.UTF8.GetBytes(str);
                    using (var stream = new MemoryStream(buffer))
                    {
                        var ser = new XmlSerializer(typeof(OrderQueryResponse));
                        item = (OrderQueryResponse)ser.Deserialize(stream);
                    }
                    
                    return item;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static async Task<OrderQueryResponse> PostMaintenanceDirectAsync(string requestUri, MaintenanceDirectRequest request)
        {
            var item = new OrderQueryResponse();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                        {
                            {"PSPID", request.PspId},
                            {"USERID", request.UserId},
                            {"PSWD", request.Pswd},
                            {"PAYID", request.PayId},
                            {"AMOUNT", request.Amount},
                            {"OPERATION", request.Operation}
                        });

                    var response = await client.PostAsync(requestUri, content);
                    if (!response.IsSuccessStatusCode)
                        throw new ApplicationException(
                            string.Format("Error while requesting direct maintenance: status {0}",
                            response.StatusCode, await response.Content.ReadAsStringAsync()));

                    var str = await response.Content.ReadAsStringAsync();
                    
                    var buffer = Encoding.UTF8.GetBytes(str);
                    using (var stream = new MemoryStream(buffer))
                    {
                        var ser = new XmlSerializer(typeof(OrderQueryResponse));
                        item = (OrderQueryResponse)ser.Deserialize(stream);
                    }
                    return item;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static OrderQueryResponse PostMaintenanceDirectCompleted(Task<HttpResponseMessage> responseTask)
        {
            var item = new OrderQueryResponse();
            var response = responseTask.Result;
            if (!response.IsSuccessStatusCode) return item;
            var xmlTask = response.Content.ReadAsStringAsync();
            xmlTask.Wait();
            var str = xmlTask.Result;
            var buffer = Encoding.UTF8.GetBytes(str);
            using (var stream = new MemoryStream(buffer))
            {
                var ser = new XmlSerializer(typeof(OrderQueryResponse));
                item = (OrderQueryResponse)ser.Deserialize(stream);
            }

            return item;
        }
    }
}