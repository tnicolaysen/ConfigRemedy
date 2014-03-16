using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ConfigRemedy.Models;
using Raven.Client;

namespace ConfigRemedy.Controllers
{
    public class EnvironmentsController : RavenDbController
    {
        [HttpGet]
        public Task<IList<Environment>> GetDocs()
        {
            return Session.Query<Environment>().ToListAsync();
        }

        [HttpPost]
        public async Task<Environment> Post([FromUri] string name)
        {
            var environment = new Environment { EnvironmentName = name };
            await Session.StoreAsync(environment);

            return environment;
        }


        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}