using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class HomeController : Controller
    {
        static List<long> time = new List<long>();
        static List<int> usersConected = new List<int>();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Administrador()
        {
            return View();
        }

        // Get: Actualizar contacto
        [HttpPost]
        public JsonResult Grafica()
        {
            time.Add(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
            List<string> users = Negocio.Usuario.getUserGame();
            usersConected.Add(users.Count);
            return Json(new { time,usersConected });
        }

        // Get: Mostrar Uusarios Registrados
        [HttpPost]
        public JsonResult Users()
        {
            List<string> users = Negocio.Usuario.getUsersName();
            var nombre = Session["usuario"];
            return Json(new { nombre, users });
        }

        // Get: Mostrar Usuarios conectados
        [HttpPost]
        public JsonResult usersWait()
        {
            List<string> users = new List<string>();
            List<string> usersConnected = Negocio.Usuario.getUserGame();
            var Listado = Negocio.Usuario.Listar();
            foreach (var user in Listado) { users.Add(user.Nombre); }
            return Json(new { users, usersConnected });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // DELETE: Ingresar
        // POST: Actualizar contacto
        [HttpPost]
        public JsonResult Ingresar(Negocio.Usuario usuario)
        {
            bool hecho = false;
            bool admin = false;
            string mensaje = "";
            try
            {
                hecho = Negocio.Usuario.Validar(usuario);
                admin = Negocio.Usuario.ValidarAdmin(usuario);
                if (hecho || admin) {Session["usuario"] = usuario.Nombre; }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { admin, hecho, mensaje});
        }
    }
}