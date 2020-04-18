using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Handlers;
using System.Threading.Tasks;
using Consumo.Web.Cliente.Models;

namespace Consumo.Web.Cliente.Controllers
{
    public class UsuariosController : Controller
    {
        string BaseUrl = "http://www.nuevaintranet2.somee.com";
        // GET: Usuarios
        [HttpGet]
       public async Task<ActionResult> Index()
        {
            List<Usuarios> EmpInfo = new List<Usuarios>();
            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(BaseUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //llena todos los usuarios usando el el HttpClient
            HttpResponseMessage Res = await httpClient.GetAsync("api/usuarios/");
            if (Res.IsSuccessStatusCode)
            {
                //Si Res-True entra y asigna los datos
                var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                //Deserializa el api y almacena los datos
                EmpInfo = JsonConvert.DeserializeObject<List<Usuarios>>(EmpResponse);
            }

            return View(EmpInfo);

        }

        //Crear Un Nuevo Usuario
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Usuarios usuarios)
        {
            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("http://www.nuevaintranet2.somee.com/api/Usuarios");
            var PostTask = httpClient.PostAsJsonAsync<Usuarios>("usuarios", usuarios);
            PostTask.Wait();

            var result = PostTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Error, Contacta al Administrador ");
            return View(usuarios);
        }

       [HttpGet]
       public ActionResult Edit(int id)
        {
            Usuarios usuarios = null;
            HttpClient httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("http://www.nuevaintranet2.somee.com/");
            var responseTask = httpClient.GetAsync("api/usuarios/" + id.ToString());
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Usuarios>();
                readTask.Wait();
                usuarios = readTask.Result;

            }

            return View(usuarios);
        }

        [HttpPost]
        public ActionResult Edit(Usuarios usuarios)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://www.nuevaintranet2.somee.com/");
            var putTask = httpClient.PutAsJsonAsync($"api/usuarios/{usuarios.int_id}", usuarios);
            putTask.Wait();
            var result = putTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(usuarios);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Usuarios usuarios = null;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://www.nuevaintranet2.somee.com/");

            var responseTask = httpClient.GetAsync("api/usuarios/" + id.ToString());
            responseTask.Wait();
            var resulTask = responseTask.Result;

            if (resulTask.IsSuccessStatusCode)
            {
                var readTask = resulTask.Content.ReadAsAsync<Usuarios>();
                readTask.Wait();
                usuarios = readTask.Result;
            }

            return View(usuarios);
        }

        [HttpPost]
        public ActionResult Delete(Usuarios usuarios,int id)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://www.nuevaintranet2.somee.com/");
            var DeleteTask = httpClient.DeleteAsync($"api/usuarios/" + id.ToString());
            DeleteTask.Wait();
            var result = DeleteTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }

            return View(usuarios);
        }

    }
}