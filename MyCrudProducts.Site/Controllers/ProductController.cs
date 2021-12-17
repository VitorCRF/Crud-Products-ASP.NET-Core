using Microsoft.AspNetCore.Mvc;
using MyCrudProducts.Site.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyCrudProducts.Site.Controllers
{
    public class ProductController : Controller
    {
        private readonly string apiUrl = "https://localhost:44335/api/product";
        public async Task<IActionResult> Index()
        {
            List<Product> productList = new List<Product>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse);
                }
            }
            return View(productList);

        }

        public ViewResult GetProduct() => View();

        [HttpPost]
        public async Task<IActionResult> GetProduct(int id)
        {
            Product product = new Product();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if(response.StatusCode.ToString() != "OK")
                    {
                        return View();
                    }
                    else
                    {
                        product = JsonConvert.DeserializeObject<Product>(apiResponse);
                    }
                }
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(string name, decimal price, int stock)
        {
            if (!string.IsNullOrEmpty(name) && price != 0)
            {
                Product product = new Product();
                product.Name = name;
                product.Price = price;
                product.Stock = stock;

                try
                {
                    Product ProductReceived = new Product();
                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(JsonConvert.SerializeObject(product),
                                                          System.Text.Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(apiUrl, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            ProductReceived = JsonConvert.DeserializeObject<Product>(apiResponse);
                        }
                    }
                    TempData["success-message"] = "Produto cadastrado com sucesso.";
                    return RedirectToAction("Index");
                }
                catch (System.Exception exception)
                {
                    TempData["error-message"] = "Ocorreu um erro, mensagem: " + exception.Message;
                }
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            Product product = new Product();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, [Bind("ProductId,Name,Price,Stock")] Product product)
        {
            try
            {
                Product productRecebido = new Product();
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(product),
                                                      System.Text.Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PutAsync(apiUrl, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                    }
                }
                TempData["success-message"] = "Produto alterado com sucesso.";
                return RedirectToAction("Index");
            }
            catch (System.Exception exception)
            {
                TempData["error-message"] = "Ocorreu um erro, mensagem: " + exception.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
