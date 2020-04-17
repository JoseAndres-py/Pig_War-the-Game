using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class GameController : Controller
    {
        // GET: Espera/Create
        public ActionResult Esperar()
        {
            var Listado = Negocio.Usuario.Listar();
            var retorno = View(new Models.ListadoUsers() { Listado = Listado });
            return retorno;
        }

        // GET: Espera/Salir
        public ActionResult Salir(Negocio.Usuario usuario)
        {
            bool hecho = false;
            int turno = 0;
            string mensaje = "";
            try
            {
                turno = Negocio.Usuario.Cerrar(usuario);
                hecho = true;
                if (turno != 0) { Session["usuario"] = ""; Negocio.Usuario.actualizarTurno(turno); }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { hecho, mensaje });
        }
  
        // POST: Actualizar contacto
        [HttpPost]
        public JsonResult Esperar(Negocio.Usuario usuario)
        {
            var name = (string)Session["usuario"];
            bool hecho = false;
            bool game = false;
            string mensaje = "";
            string registros = "";
            try
            {
                var Listado = Negocio.Usuario.Listar();
                registros = RenderRazorViewToString("ListaUsuarios", new Models.ListadoUsers() { Listado = Listado });
                hecho = true;
                if(usuario.Nombre != null) { game = Negocio.Usuario.Jugar(usuario); }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { name, game, hecho, mensaje, registros });
        }

        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        // POST: Espera/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Espera/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Espera/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Espera/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Espera/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
