using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Directorio.Controllers
{
    public class DirectorioController : Controller
    {
        // GET: Directorio
        public ActionResult Index()
        {
            var Listado = Negocio.Persona.Listar(100, 1);
            var retorno = View(new Models.ListadoPersonas() { Listado = Listado });
            return retorno;
        }

        //// GET: Directorio/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Directorio/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Directorio/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Directorio/Edit/5
        public ActionResult Edit1(long id)
        { 
            var modelo = new Models.Persona();
            modelo.Datos = Negocio.Persona.Buscar(id);
            modelo.TiposContacto = Negocio.TipoContacto.Listar();
            return View(modelo);
        }

        // POST: Directorio/Edit/5
        [HttpPost]
        public ActionResult Edit1(Negocio.Persona persona)
        {
            try
            {
                Negocio.Persona.Actualizar(persona);
                return Edit1(persona.PersonaId);
            }
            catch(Exception ex)
            {
                return View("Error",ex);
            }
        }

        //// GET: Directorio/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Directorio/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
