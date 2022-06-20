using Passion.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Passion.Controllers
{
    public class DonorController : Controller
    {
        private static readonly HttpClient client;
        
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonorController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44372/api/");
        }// GET: Donor
        [HttpGet]
        public ActionResult List()
        {
            string url = "donordata/listdonors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DonorDto> donors = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;
            return View(donors);
        }

        // GET: Donor/Details/5
        public ActionResult Details(int id)
        {

            string url = "Donordata/findDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonorDto Donors = response.Content.ReadAsAsync<DonorDto>().Result;

            return View(Donors);
        }

        // GET: Donor/Create
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        // POST: Donor/Create
        [HttpPost]
        public ActionResult Create(Donor donor)
        {
            
            
            //https://localhost:44324/api/donordata/adddonor 
            string url = "donordata/adddonor";


            string jsonpayload = jss.Serialize(donor);
           

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Donor/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Donordata/findDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonorDto Donors = response.Content.ReadAsAsync<DonorDto>().Result;

            return View(Donors);
        }

        // POST: Donor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DonorDto Donor)
        {
            string url = "Donordata/updateDonor/" + id;
            string jsonpayload = jss.Serialize(Donor);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Donor/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Donordata/findDonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto selecteddonor = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(selecteddonor);
        }

        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Donordata/deleteDonor/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
