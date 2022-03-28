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
        public JsonResult Post(Employee emp)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int LastEmployeeId = dbClient.GetDatabase("testedb").GetCollection<Employee>("Employee").AsQueryable().Count();
            emp.EmployeeId = LastEmployeeId + 1;

            dbClient.GetDatabase("testedb").GetCollection<Employee>("Employee").InsertOne(emp);

            return new JsonResult("Adicionado com Sucesso");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", emp.EmployeeId);

            var update = Builders<Employee>.Update.Set("EmployeeName", emp.EmployeeName)
                                                    .Set("Departament", emp.Departament)
                                                      .Set("DateOfJoining", emp.DateOfJoining)
                                                        .Set("PhotoFileName", emp.PhotoFileName);

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
