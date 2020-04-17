using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Directorio.Controllers
{
    public class ContactoController : Controller
    {
        // POST: Actualizar contacto
        [HttpPost]
        public JsonResult Set(Negocio.Contacto contacto)
        {
            bool hecho = false;
            string mensaje = "";
            string registros="";
            try
            {
                Negocio.Contacto.Actualizar(contacto);
                registros = RenderRazorViewToString("ListaContactos",Negocio.Contacto.Listar(contacto.PersonaId));
                hecho = true;
            }
            catch (Exception ex) {
                mensaje = ex.Message;
            }
            return Json(new { hecho, mensaje, registros});
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
        // POST: Actualizar Crear
        [HttpPost]
        public JsonResult Create(Negocio.Contacto contacto)
        {
            bool hecho = false;
            string mensaje = "";
            string registros = "";
            try
            {
                Negocio.Contacto.Adicionar(contacto);
                registros = RenderRazorViewToString("ListaContactos", Negocio.Contacto.Listar(contacto.PersonaId));
                hecho = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { hecho = hecho, mensaje = mensaje, registros=registros });
        }

        // DELETE: Actualizar Eliminar
        [HttpPost]
        //https://docs.microsoft.com/en-us/iis/extensions/introduction-to-iis-express/iis-express-faq
        public JsonResult Delete(long ContactoId,long PersonaId)
        {
            bool hecho = false;
            string mensaje = "";
            var registros = new List<Negocio.Contacto>();
            try
            {
                Negocio.Contacto.Borrar(ContactoId);
                registros = Negocio.Contacto.Listar(PersonaId);
                hecho = true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new { hecho = hecho, mensaje = mensaje, registros = registros });
        }
    }
}