using Azure.Data.Tables;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MvcCoreSasAzureStorage.Helpers;
using MvcCoreSasAzureStorage.Models;
using System.Security.Principal;

namespace MvcCoreSasAzureStorage.Controllers
{
    public class MigracionController : Controller
    {
        private HelperXml helperXml;

        public MigracionController(HelperXml helperXml) {
            this.helperXml = helperXml;
        }
        
        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string accion) {
            string azureKeys = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

            //This line creates an instance of TableServiceClient using the azureKeys connection string.TableServiceClient is used to interact with the Azure Table Storage service.
            TableServiceClient serviceClient = new TableServiceClient(azureKeys);

            //This line creates an instance of TableClient for a table named "alumnos" using the TableServiceClient instance.TableClient is used to perform operations on a specific table.
            TableClient tableClient = serviceClient.GetTableClient("alumnos");

            //This line asynchronously creates the "alumnos" table if it does not already exist.
            await tableClient.CreateIfNotExistsAsync();

            List<Alumno> alumnos = this.helperXml.GetAlumnos();
            foreach (Alumno alumno in alumnos) {
                await tableClient.AddEntityAsync(alumno);
            }
            ViewBag.Mensaje = "Alumnos migrados a Azure Table Storage";
            return View();
        }
    }
}
