using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContentEngine.Models;

namespace ContentEngine.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public CollectionApiResult<string> Get()
        {
            return new CollectionApiResult<string>()
            {
                data = new string[] { "value1", "value2" },
                total = 2,
                page = 1,
                pageSize = 10
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
