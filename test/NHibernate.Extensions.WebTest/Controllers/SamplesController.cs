using System;
using Microsoft.AspNetCore.Mvc;
using NHibernate;

namespace NHibernate.Extensions.WebTest.Controllers {

    [Route("api/[controller]")]
    public class SamplesController : Controller {

        private ISession session;

        public SamplesController(ISession session) {
            this.session = session;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                session = null;
            }
        }

        [HttpGet("")]
        public ActionResult GetAll() {
            Console.WriteLine(session);
            return Ok("Hello, world!");
        }

    }

}
