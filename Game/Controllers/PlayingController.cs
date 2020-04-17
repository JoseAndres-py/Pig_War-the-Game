using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class PlayingController : Controller
    {
        static long millisecondsold = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        static int direction = -1;
        static private Models.ListadoPositions ModelPos = new Models.ListadoPositions()
        {
            Barrera = Negocio.elementGame.listarBarrera(4),
            Enemigo = new Negocio.elementGame() { x = 3, y = 0 },
            Disparos = new List<Negocio.elementGame>(),
            Puntaje = 0
        };
        // GET: Playing/Game
        public ActionResult Game()
        {
            var retorno = View(ModelPos);
            Negocio.elementGame.updateJugador(new Negocio.elementGame() { Id = 1,y = 0});
            Negocio.elementGame.updateJugador(new Negocio.elementGame() { Id = 2, y = 0 });
            return retorno;
        }

        // POST: Actualizar Juego
        [HttpPost]
        public JsonResult Game(Negocio.elementGame position)
        {
            bool hecho = false;
            string mensaje = "";
            string registros = "";
            int idGame = 0;
            long millisecondsnow = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            try
            {
                // Identificar Jugador
                int id = 0;
                idGame = Negocio.elementGame.getId(new Negocio.Usuario() { Nombre = (string)Session["usuario"]});
                ModelPos.Puntaje = Negocio.elementGame.getPoints(new Negocio.Usuario() { Nombre = (string)Session["usuario"] });
                position.Id = idGame;
                hecho = Negocio.elementGame.updateJugador(position);

                //Validar disparo
                if (position.fire)
                {
                    int x = 0;
                    if (idGame == 2) { x = 10; }else { x = -2; }
                    var gamerFire = new Negocio.elementGame() { x = position.x+x, y = position.y + 6 ,Id=idGame };
                    ModelPos.Disparos.Add(gamerFire);
                }
                // Obtener Enemigo
                if (idGame == 1) { id = 2; } else { id = 1; }
                ModelPos.Enemigo = Negocio.elementGame.getEnemy(id);
                registros = RenderRazorViewToString("ListaBarrera", ModelPos);

                // Mover misiles
                if (millisecondsnow - millisecondsold > 1)
                {
                    Negocio.elementGame.moveMissiles(ref ModelPos.Disparos);
                    Negocio.elementGame.collisionDetection(ref ModelPos.Barrera, ref ModelPos.Disparos);
                    //Terminar Juego
                    if (Negocio.elementGame.collisionDetectionGamer(position, ref ModelPos.Disparos)) {
                        mensaje = "Loser";
                        var gammer = new Negocio.Usuario() { Nombre = (string)Session["usuario"] };
                        var turno = Negocio.Usuario.Cerrar(gammer);
                        ModelPos.Barrera = Negocio.elementGame.listarBarrera(4);
                        position.y = 0;
                        Negocio.elementGame.updateJugador(position);
                        if (turno != 0) {
                            Session["usuario"] = "";
                            Negocio.Usuario.actualizarTurno(turno);
                            Negocio.Usuario.aumentarPuntuacion(gammer);
                        }

                    }
                    direction = Negocio.elementGame.moveBarrera(ref ModelPos.Barrera, direction);
                    millisecondsold = millisecondsnow;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return Json(new {idGame ,hecho, mensaje, registros });
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

        // GET: Playing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Playing/Create
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

        // GET: Playing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Playing/Edit/5
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

        // GET: Playing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Playing/Delete/5
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
