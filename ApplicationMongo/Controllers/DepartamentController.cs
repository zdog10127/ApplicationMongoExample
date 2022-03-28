using ApplicationMongo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ApplicationMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartamentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dbList = dbClient.GetDatabase("testedb").GetCollection<Departament>("Departament").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Departament dep)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            int LastDepartamentId = dbClient.GetDatabase("testedb").GetCollection<Departament>("Departament").AsQueryable().Count();
            dep.DepartamentId = LastDepartamentId + 1;

            dbClient.GetDatabase("testedb").GetCollection<Departament>("Departament").InsertOne(dep);

            return new JsonResult("Adicionado com Sucesso");
        }

        [HttpPut]
        public JsonResult Put(Departament dep)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Departament>.Filter.Eq("DepartamentId", dep.DepartamentId);

            var update = Builders<Departament>.Update.Set("DepartamentName", dep.DepartamentName);

            dbClient.GetDatabase("testedb").GetCollection<Departament>("Departament").UpdateOne(filter, update);

            return new JsonResult("Atualizado com Sucesso");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var filter = Builders<Departament>.Filter.Eq("DepartamentId", id);

            dbClient.GetDatabase("testedb").GetCollection<Departament>("Departament").DeleteOne(filter);

            return new JsonResult("Deletado com Sucesso");
        }
    }
}
