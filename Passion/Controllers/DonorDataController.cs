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
    public class DonorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DonorData/ListDonors
        [HttpGet]
        public IHttpActionResult ListDonors()
        {
            List<Donor> Donors = db.Donors.ToList();
            List<DonorDto> DonorDtos = new List<DonorDto>();

            Donors.ForEach(a => DonorDtos.Add(new DonorDto()
            {
                DonorID = a.DonorID,
                DonorName = a.DonorName,
                DonorBlood = a.DonorBlood,
                DonorEmail = a.DonorEmail,
                RegistrationDate = a.RegistrationDate
            }));

            return Ok(DonorDtos);
        }

        // GET: api/DonorData/FindDonor/1
        [ResponseType(typeof(Donor))]
        [HttpGet]
        public IHttpActionResult FindDonor(int id)
        {
            Donor donor = db.Donors.Find(id);
            DonorDto DonorDto = new DonorDto()
            {
                DonorID=donor.DonorID,
                DonorName=donor.DonorName,
                DonorEmail=donor.DonorEmail,
                DonorBlood=donor.DonorBlood,
                RegistrationDate=donor.RegistrationDate
            };
            if (donor == null)
            {
                return NotFound();
            }

            return Ok(DonorDto);
        }

        // PUT: api/DonorData/UpdateDonor/2
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDonor(int id, Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donor.DonorID)
            {
                return BadRequest();
            }

            db.Entry(donor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonorExists(id))
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

        // POST: api/DonorData/AddDonor
        [ResponseType(typeof(Donor))]
        [HttpPost]
        public IHttpActionResult AddDonor(Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donors.Add(donor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donor.DonorID }, donor);
        }

        // DELETE: api/DonorData/DeleteDonor/3
        [ResponseType(typeof(Donor))]
        [HttpPost]
        public IHttpActionResult DeleteDonor(int id)
        {
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }

            db.Donors.Remove(donor);
            db.SaveChanges();

            return Ok(donor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DonorExists(int id)
        {
            return db.Donors.Count(e => e.DonorID == id) > 0;
        }
    }
}