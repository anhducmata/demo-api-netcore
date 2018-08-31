using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using test5.Models;
using test5.Services;

namespace test5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<string> Post()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                try
                {
                    var posts = new PostModel();
                    var body = await reader.ReadToEndAsync();
                    var jObj = JObject.Parse(body);

                    // Get the object that return from the function. Make sure the the jObj["data"] as data is a closest parent of the list.
                    var objReturn = ParseJsonServices.GetDataFromJson(typeof(PostModelData), jObj["data"]);

                    // New a list model
                    var listData = new List<PostModelData>();
                    var listObject = (List<object>) objReturn;
                    foreach (PostModelData i in listObject)
                    {
                        // parse list object to model
                        listData.Add(i);
                    }

                    // Add posts data is listData
                    posts.data = listData;

                    /* Some business logic here to get a lasted response for user.
                     ...
                     Code here
                     ...
                     */
                }
                catch(Exception exception) // Throw ex mes
                {
                    return exception.Message;
                }
            }

            return "ok ok ok";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
