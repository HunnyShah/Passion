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
    public class PatientController : Controller
    {
        private static readonly HttpClient client;

        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PatientController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44372/api/");
        }// GET: Patient
        [HttpGet]
        public ActionResult List()
        {
            string url = "patientdata/listpatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PatientDto> patients = response.Content.ReadAsAsync<IEnumerable<PatientDto>>().Result;
            return View(patients);
        }

        // GET: Patient/Details/5
        public ActionResult Details(int id)
        {

            string url = "Patientdata/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto Patients = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(Patients);
        }

        // GET: Patient/Create
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(Patient patient)
        {


            //https://localhost:44324/api/patientdata/addpatient 
            string url = "patientdata/addpatient";


            string jsonpayload = jss.Serialize(patient);


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

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Patientdata/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PatientDto Patients = response.Content.ReadAsAsync<PatientDto>().Result;

            return View(Patients);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PatientDto patient)
        {
            string url = "patientdata/updatepatient/" + id;
            string jsonpayload = jss.Serialize(patient);
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

        // GET: Patient/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Patientdata/findPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PatientDto selectedpatient = response.Content.ReadAsAsync<PatientDto>().Result;
            return View(selectedpatient);
        }

        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Patientdata/deletePatient/" + id;
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
