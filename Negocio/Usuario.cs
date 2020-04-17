using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public int Puntaje { get; set; }
        public int Estado { get; set; }
        public bool Admin { get; set; }
        public int Turno { get; set; }

        public static Usuario FromDatos(Datos.Users usuario)
        {
            return new Usuario()
            {
                Nombre = usuario.UserGame,
                Contrasena = usuario.Password,
                Puntaje = (int)usuario.Points,
                Estado = (int)usuario.State,
                Admin = (usuario.TypeUser == "Admin"),
                Turno = (int)usuario.Ingreso
            };
        }
        private static Datos.Users ToDatos(Usuario usuario)
        {
            return new Datos.Users()
            {
                UserGame = usuario.Nombre,
                Password = usuario.Contrasena,
                Points = usuario.Puntaje,
                State = usuario.Estado,
                TypeUser = "Gamer",
                Ingreso = 0
            };
        }
        private static void MapToDatos(Usuario origen, ref Datos.Users destino)
        {
            destino.State = origen.Estado;
            destino.Ingreso = origen.Turno;
        }

        public static bool  Validar(Usuario usuario)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var registro = context.Users.Single(r => r.UserGame == usuario.Nombre);
                if (registro.Password.Replace(" ", "") == usuario.Contrasena) {
                    if (registro.State == 0)
                    {
                        usuario.Estado = 1;
                        usuario.Turno = context.Users.Where(r => r.State == 1).ToList().Count+1; 
                        MapToDatos(usuario, ref registro);
                        context.SaveChanges();
                        return true;
                    }else throw new Exception("Usuario ya loggeado");
                }else throw new Exception("Usuario o Contrasena Incorrectos");

            }
            return false;
        }

        public static bool ValidarAdmin(Usuario usuario)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var registro = context.Users.Single(r => r.UserGame == usuario.Nombre);
                if (registro.TypeUser.Replace(" ", "") == "Admin")
                {
                    usuario.Estado = 3;
                    MapToDatos(usuario, ref registro);
                    context.SaveChanges();
                    return true;
                }

            }
            return false;
        }

        public static List<Usuario> Listar()
        {
            var registros = new List<Usuario>();
            using (var context = new Datos.JuegoEntities1())
            {
                var dbr = context.Users.Where(r => r.State == 1);
                foreach (var reg in dbr)
                {
                    registros.Add(FromDatos(reg));
                }

            }
            var registrosOrdenado = registros.OrderBy(x => x.Turno);
            registros = new List<Usuario>();
            registros.AddRange(registrosOrdenado);
            return registros;
        }

        public static int Cerrar(Usuario usuario)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var registro = context.Users.Single(r => r.UserGame == usuario.Nombre);
                {
                    usuario.Estado = 0;
                    int turno = (int)registro.Ingreso;
                    MapToDatos(usuario, ref registro);
                    context.SaveChanges();
                    return turno;
                }

            }
        }
        public static void aumentarPuntuacion(Usuario usuario)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var registro = context.Users.Single(r => (r.UserGame != usuario.Nombre)&&(r.State == 2));
                {
                    registro.Points++;
                    context.SaveChanges();
                }

            }
        }

        public static bool Jugar(Usuario usuario)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var gamers = context.Users.Where(r => r.State == 2).ToList();
                if (gamers.Count < 2) {
                    var user = context.Users.Single(r => r.UserGame == usuario.Nombre);
                    usuario.Estado = 2;
                    var Gamers = context.Users.Where(r => r.State == 2).ToList();
                    if (Gamers.Count == 0)
                    {
                        usuario.Turno = 1;
                    }
                    else
                    {
                        usuario.Turno = 1;
                        if (Gamers[0].Ingreso == 1) { usuario.Turno = 2; }
                    }
                    MapToDatos(usuario, ref user);
                    context.SaveChanges();
                    return true;
                }

            }
            return false;
        }

        public static void actualizarTurno(int turno)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var dbr = context.Users.Where(r => r.State == 1).ToList();
                for (int i = 0; i < dbr.Count; i++) {
                    if (dbr[i].Ingreso > turno) { dbr[i].Ingreso--; }
                }
                context.SaveChanges();

            }
        }

        public static List<string> getUsersName()
        {
            var names = new List<string>();
            using (var context = new Datos.JuegoEntities1())
            {
                var dbr = context.Users.ToList();
                foreach(var user in dbr)
                {
                    names.Add(user.UserGame);
                }
            }
            return names;
        }

        public static List<string> getUserGame()
        {
            var names = new List<string>();
            using (var context = new Datos.JuegoEntities1())
            {
                var dbr = context.Users.Where(r => r.State == 2).ToList();
                foreach (var user in dbr)
                {
                    names.Add(user.UserGame);
                }
            }
            return names;
        }
        //private static void maptodatos(contacto origen, ref datos.contacto destino)
        //{
        //    destino.tipocontactoid = origen.tipocontactoid;
        //    destino.valor = origen.valor;
        //}
        //public static void adicionar(contacto contacto)
        //{
        //    using (var context = new Datos.DirectorioEntities1())
        //    {
        //        var newreg = ToDatos(contacto);
        //        newreg.Activo = true;
        //        context.Contacto.Add(newreg);
        //        context.SaveChanges();
        //    }
        //}
        //public static void Actualizar(Contacto contacto)
        //{
        //    using (var context = new Datos.DirectorioEntities1())
        //    {
        //        var registro = context.Contacto.Single(r => r.ContactoId == contacto.ContactoId);
        //        MapToDatos(contacto, ref registro);
        //        context.SaveChanges();
        //    }
        //}
        //public static List<Contacto> Listar()
        //{
        //    var registros = new List<Contacto>();
        //    using (var context = new Datos.DirectorioEntities1())
        //    {
        //        var dbr = context.Contacto.Where(r => r.Activo == false);
        //        foreach (var reg in dbr)
        //        {
        //            registros.Add(FromDatos(reg));
        //        }

        //    }
        //    return registros;
        //}
        //public static List<Contacto> Listar(long PersonaId)
        //{
        //    var registros = new List<Contacto>();
        //    using (var context = new Datos.DirectorioEntities1())
        //    {
        //        var dbr = context.Contacto.Where(r => r.PersonaId == PersonaId && r.Activo == true);
        //        foreach (var reg in dbr)
        //        {
        //            registros.Add(FromDatos(reg));
        //        }

        //    }
        //    return registros;
        //}
        //public static void Borrar(long RegistroId)
        //{
        //    using (var context = new Datos.DirectorioEntities1())
        //    {
        //        var registro = context.Contacto.Single(r => r.ContactoId == RegistroId);
        //        registro.Activo = false;
        //        context.SaveChanges();
        //    }

        //}
    }
}
