using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class elementGame
    {
        public int Id { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public bool fire { get; set; }

        public static elementGame FromDatos(Datos.Game gamer)
        {
            return new elementGame()
            {
                Id = gamer.Id,
                x = (float)gamer.positionX,
                y = (float)gamer.positionY

            };
        }
        private static Datos.Game ToDatos(elementGame enemy)
        {
            return new Datos.Game()
            {
                Id = enemy.Id,
                positionX = enemy.x,
                positionY = enemy.y
            };
        }
        private static void MapToDatos(elementGame origen, ref Datos.Game destino)
        {
            destino.Id = origen.Id;
            destino.positionY = origen.y;
        }
        public static bool updateJugador(elementGame usuario)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var registro = context.Game.Single(r => r.Id == usuario.Id);
                MapToDatos(usuario, ref registro);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public static int getId(Usuario usuario)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var registro = context.Users.Single(r => r.UserGame == usuario.Nombre);
                {
                    return (int)registro.Ingreso;
                }

            }
        }

        public static int getPoints(Usuario usuario)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var registro = context.Users.Single(r => r.UserGame == usuario.Nombre);
                {
                    return (int)registro.Points;
                }

            }
        }

        public static elementGame getEnemy(int idEnemy)
        {
            using (var context = new Datos.JuegoEntities1())
            {
                var registro = context.Game.Single(r => r.Id == idEnemy);
                {
                    return FromDatos(registro);
                }

            }
        }


        public static List<elementGame> listarBarrera(int number)
        {
            var Barreras = new List<Negocio.elementGame>();
            for (int i = 0; i < number; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Barreras.Add(new elementGame() { x = (42 - (number / 2) * 5) + (i * 5), y = j * 8+10 });
                }
            }
            return Barreras;
        }

        public static void moveMissiles(ref List<elementGame> misiles)
        {

            for (int i = 0; i < misiles.Count; i++)
            {
                if (misiles[i].x >= 90 || misiles[i].x <= -0) {
                    misiles.Remove(misiles[i]);
                }
                else { 
                    if (misiles[i].Id == 1)
                    {
                        misiles[i].x -= (float)0.4;
                    }
                    else { misiles[i].x += (float)0.4; }
                }
                
            }

        }

        public static int moveBarrera(ref List<elementGame> Barrera, int direction)
        {
            for (int i = 0; i < Barrera.Count; i++)
            {
                if ((Barrera[i].y <= 0) || (Barrera[i].y >= 80.0))
                {
                    direction *= -1;
                    break;
                }
            }
            for (int i = 0; i < Barrera.Count; i++)
            {
                Barrera[i].y = Barrera[i].y + (direction * (float)0.2);
            }
            return direction;
        }

        public static void collisionDetection(ref List<elementGame> Barrera,ref List<elementGame> Disparos)
        {
            for (var enemy = 0; enemy < Barrera.Count; enemy++)
            {
                for (var missile = 0; missile < Disparos.Count; missile++)
                {
                    if (Disparos[missile].x >= Barrera[enemy].x && Disparos[missile].x <= (Barrera[enemy].x + 5) &&Disparos[missile].y <= (Barrera[enemy].y + 8) &&Disparos[missile].y >= Barrera[enemy].y)
                    {
                        Barrera.Remove(Barrera[enemy]);
                        Disparos.Remove(Disparos[missile]);
                        break;
                    }
                }
            }

        }

        public static bool collisionDetectionGamer(elementGame Enemy, ref List<elementGame> Disparos)
        {
                for (var missile = 0; missile < Disparos.Count; missile++)
                {
                    if (Disparos[missile].x >= Enemy.x && Disparos[missile].x <= (Enemy.x + 5) && Disparos[missile].y <= (Enemy.y + 8) && Disparos[missile].y >= Enemy.y)
                    {
                        Disparos.Remove(Disparos[missile]);
                        return true;
                    }
                }
            return false;
        }
    }
}
