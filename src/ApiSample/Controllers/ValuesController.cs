using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Remote.Cocohub.Http;

namespace ApiSample.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string[] result = new string[2];
            result[0] = "111";
            result[1] = "222";

            Foo(123, null);

            RequestAgent.Send(HttpMethod.Get, new Uri("http://localhost:6000/api/values/1"), null, (code, content) =>
            {
                if (code == System.Net.HttpStatusCode.OK)
                {
                    result[1] = "333";
                }

            });


            Bar(123, new Uri("http://www.lifepre.com"));
            return result;
        }

        private bool Foo(int num, Uri uri)
        {
            Bar(1, new Uri("http://www.lifevc.com"));
            if (num > 100)
                return true;
            else
                return false;
        }
        private bool Bar(int num, Uri uri)
        {
            if (num < 100)
                return true;
            else
                return false;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]dynamic value)
        {
        }

        // PUT api/values
        [HttpPut]
        public void Password([FromBody]dynamic json)
        {
        }

        // DELETE api/values/5
        [HttpDelete]
        public void Delete([FromBody]dynamic json)
        {
        }
    }
}
