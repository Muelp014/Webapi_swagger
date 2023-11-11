using Northwind.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace NorthWind.Controllers
{
public class HomeController : Controller
    {
//Hosted web API REST Service base url
    readonly string Baseurl = "http://localhost:5156/";

    public List<Employee>? ProductInfo { get; private set; }

    public async Task<ActionResult> Index()
        {
List<Employee> EmpInfo = new List<Employee>();  
using var client = new HttpClient();

//Passing service base url
client.BaseAddress = new Uri(Baseurl);
client.DefaultRequestHeaders.Clear();
//Define request data format
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//Sending request to find web api REST service resource GetProducts using HttpClient
HttpResponseMessage Res = await client.GetAsync("api/Products/GetProducts");
//Checking the response is successful or not which is sent using HttpClient
    if (Res.IsSuccessStatusCode)
        {
//Storing the response details recieved from web api
        var ProductResponse = Res.Content.ReadAsStringAsync().Result;
//Deserializing the response recieved from web api and storing into the Product list
        ProductInfo = JsonConvert.DeserializeObject<List<Employee>>(ProductResponse);
        }
//returning the employee list to view
    return View(ProductInfo);
        }
    }
}