using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Students_Groups
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> estudiantes = new List<string>();
            List<Group> grupos = new List<Group>();
            List<string> temas = new List<string>();

            try
            {
                grupos = LeerArchivoGrupos();
            }
            catch(FileNotFoundException e)
            {
                System.Console.WriteLine($"Error: El archivo {e.FileName} no existe.");
                Console.ReadKey();
                return;
            }
            catch(Exception vacio)
            {
                System.Console.WriteLine($"Error: {vacio.Message}");
            }
            
            try
            {
                estudiantes = LeerArchivosEstudiantes();
            }
            catch(FileNotFoundException e)
            {
                System.Console.WriteLine($"Error: El archivo {e.FileName} no existe.");
                Console.ReadKey();
                return;
            }
            catch(Exception vacio)
            {
                System.Console.WriteLine($"Error: {vacio.Message}");
            }

            int cantGrupos = grupos.Count;
            int cantEstudiantes = estudiantes.Count;

            if(ValidarRepeticiones(grupos, estudiantes, out string Message))
            {
                System.Console.WriteLine(Message);
                Console.ReadKey();
                return;
            }
           
            if(cantGrupos > cantEstudiantes)
            {
                System.Console.WriteLine("Error: Hay mas grupos que estudiantes.");
                Console.ReadKey();
                return;
            }
            
            try
            {
                temas = LeerArchivosTemas();
            }
            catch(FileNotFoundException e)
            {
                System.Console.WriteLine($"Error: El archivo {e.FileName} no existe.");
                Console.ReadKey();
                return;
            }
            catch(Exception vacio)
            {
                System.Console.WriteLine($"Error: {vacio.Message}");
            }

            grupos=RepartirEstudiantesExacto(grupos, estudiantes);
            temas = RepartirTemasExacto(grupos, temas);
            MostrarResultado(grupos);
            MostrarResultadoTemas(temas, grupos);
            
            Console.ReadKey();
        }

        static bool ValidarRepeticiones(List<Group> grupos, List<string> estudiantes, out string Message)
        {
            bool repeticion = false;
            bool repeticionE = false;
            Message=String.Empty;
            foreach(var grupo in grupos)
            {
                foreach(var g in grupos.Where(x=>x.Name==grupo.Name && x.GetHashCode() != grupo.GetHashCode()))
                {
                    repeticion = true;
                    Message="Error: Se han encontrado nombres de grupos duplicados. Favor utilizar un identificador unico para cada Grupo.\n";
                    break;
                }
                
            }

            foreach(var estudiante in estudiantes)
            {
                
                for(int i=estudiantes.IndexOf(estudiante)+1; i<estudiantes.Count; i++)
                {
                    if(estudiante==estudiantes[i])
                    {
                        repeticion = true;
                        repeticionE = true;
                        break;
                    }
                }
                if(repeticionE)
                {
                    Message += "Error: Se han encontrado nombres de estudiantes duplicados. Favor utilizar un identificador unico para cada Estudiante.";
                    break;
                }
            }

            return repeticion;
        }

        static void MostrarResultado(List<Group> grupos)
        {
            foreach(var grupo in grupos)
            {
                Console.WriteLine($"{grupo.Name}:");
                foreach(var estudiante in grupo.estudiantes)
                {
                    System.Console.WriteLine(estudiante);
                }
                System.Console.WriteLine("");
            }
            
        }

        static void MostrarResultadoTemas(List<string> temas, List<Group> grupos)
        {
            foreach(var grupo in grupos)
            {
                Console.WriteLine($"{grupo.Name}:");
                foreach(var tema in temas)
                {
                    System.Console.WriteLine(tema);
                }
                System.Console.WriteLine("");
            }
            
        }
        static List<Group> RepartirEstudiantesExacto(List<Group> grupos, List<string> estudiantes)
        {
            int cociente=estudiantes.Count/grupos.Count;
            int max = cociente+1;
            int mod = estudiantes.Count%grupos.Count;
            foreach(var estudiante in estudiantes)
            {
                bool dec=false;
                while(!dec)
                {
                    int index = DameOtro(grupos.Count);

                    if(grupos[index].estudiantes.Count < cociente) 
                    {
                        grupos[index].estudiantes.Add(estudiante);
                        dec=true;
                    }
                    else if(((grupos[index].estudiantes.Count < max) && (mod!=0)))
                    {
                        grupos[index].estudiantes.Add(estudiante);
                        dec=true;
                        mod--;
                    }
                    
                }
            }

            return grupos;
        }

        static List<string> RepartirTemasExacto(List<Group> grupos, List<string> temas)
        {
            int cociente = temas.Count/grupos.Count;
            int max = cociente+1;
            int mod = temas.Count%grupos.Count;
            foreach(var tema in temas)
            {
                bool dec=false;
                while(!dec)
                {
                    int index = DameOtro(temas.Count);

                    if(temas.Count < cociente) 
                    {
                        temas.Add(tema);
                        dec=true;
                    }
                    else if(((temas.Count < max) && (mod!=0)))
                    {
                        temas.Add(tema);
                        dec=true;
                        mod--;
                    }
                }
            }

            return temas;
        }

        static int DameOtro(int cantG)
        {
            Random random = new Random();
            return random.Next(cantG);
        }
        static List<Group> LeerArchivoGrupos()
        {
            List<Group> grupos = new List<Group>();
            using(StreamReader reader = new StreamReader("groups.txt"))
            {
                while(reader.Peek()>-1)
                {
                    grupos.Add(new Group(reader.ReadLine()));
                }
            }

            if(grupos.Count==0)
            {
                throw new Exception("El archivo de grupos esta vacio");
            }

            return grupos;
        }

        static List<string> LeerArchivosEstudiantes()
        {
            List<string> estudiantes = new List<string>();
            using(StreamReader reader = new StreamReader("students.txt"))
            {
                while(reader.Peek()>-1)
                {
                    estudiantes.Add(reader.ReadLine());
                }
            }

            if(estudiantes.Count==0)
            {
                throw new Exception("El archivo de estudiantes esta vacio");
            }

            return estudiantes;
        }

        static List<string> LeerArchivosTemas()
        {
            List<string> temas = new List<string>();
            using(StreamReader reader = new StreamReader("temas.txt"))
            {
                while(reader.Peek()>-1)
                {
                    temas.Add(reader.ReadLine());
                }
            }

            if(temas.Count==0)
            {
                throw new Exception("El archivo de estudiantes esta vacio");
            }

            return temas;
        }
        
    }


}
