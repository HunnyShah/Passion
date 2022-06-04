using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Passion.Models;

namespace Passion.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PatientData/ListPatients
        [HttpGet]
        public IHttpActionResult ListPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(a => PatientDtos.Add(new PatientDto()
            {
                PatientID = a.PatientID,
                PatientName = a.PatientName,
                PatientBlood=a.PatientBlood,
                PatientEmail=a.PatientEmail,
                DiseaseName=a.DiseaseName,
                RegistrationDate=a.RegistrationDate,
            }));

            return Ok(PatientDtos);
        }

        // GET: api/PatientData/FindPatient/1
        [ResponseType(typeof(Patient))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            PatientDto PatientDto = new PatientDto()
            {
                PatientID = Patient.PatientID,
                PatientName= Patient.PatientName,
                PatientEmail= Patient.PatientEmail,
                PatientBlood= Patient.PatientBlood,
                RegistrationDate= Patient.RegistrationDate,
                DiseaseName = Patient.DiseaseName
            };
            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(PatientDto);
        }

        // PUT: api/PatientData/UpdatePatient/2
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PatientData/AddPatient
        [HttpPost]
        [ResponseType(typeof(Patient))]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientID }, patient);
        }

        // DELETE: api/PatientData/DeletePatient/3
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok(patient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientID == id) > 0;
        }
    }
}