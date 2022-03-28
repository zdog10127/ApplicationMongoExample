using ApplicationMongo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApplicationMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dbList = dbClient.GetDatabase("testedb").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee em)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int LastEmployeeId = dbClient.GetDatabase("testedb").GetCollection<Employee>("Employee").AsQueryable().Count();
            em.EmployeeId = LastEmployeeId + 1;

            dbClient.GetDatabase("testedb").GetCollection<Employee>("Employee").InsertOne(em);

            return new JsonResult("Adicionado com Sucesso");
        }

        [HttpPut]
        public JsonResult Put(Employee em)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", em.EmployeeId);

            var update = Builders<Employee>.Update.Set("EmployeeName", em.EmployeeName)
                                                    .Set("Departament", em.Departament)
                                                      .Set("DateOfJoining", em.DateOfJoining)
                                                        .Set("PhotoFileName", em.PhotoFileName);

            dbClient.GetDatabase("testedb").GetCollection<Employee>("Employee").UpdateOne(filter, update);

            return new JsonResult("Atualizado com Sucesso");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", id);

            dbClient.GetDatabase("testedb").GetCollection<Employee>("Employee").DeleteOne(filter);

            return new JsonResult("Deletado com Sucesso");
        }
    }
}
