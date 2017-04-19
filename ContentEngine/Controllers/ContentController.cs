using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContentEngine.Models;
using ContentEngine.Persistence;

namespace ContentEngine.Controllers
{
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class ContentController : Controller
    {
        private readonly IContentReader _contentReader;
        private readonly IContentWriter _contentWriter;

        public ContentController(IContentReader contentReader, IContentWriter contentWriter) {
            _contentReader = contentReader;
            _contentWriter = contentWriter;
        }
        
        // GET api/values
        [HttpGet]
        public CollectionApiResult<string> Get()
        {
            //_contentReader.ReadJson()
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
        public void Post([FromBody]dynamic data)
        {
            _contentWriter.WriteJson(Guid.Parse("96d93542-f684-4e17-85c1-51555a4ff281"), data.ToString());
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
