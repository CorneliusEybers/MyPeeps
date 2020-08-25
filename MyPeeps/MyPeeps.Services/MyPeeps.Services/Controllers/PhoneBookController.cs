// - Required Assemblies
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// - Aplication Assemblies

namespace MyPeeps.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneBookController : ControllerBase
    {
        // GET: api/PhoneBook
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PhoneBook/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PhoneBook
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/PhoneBook/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
