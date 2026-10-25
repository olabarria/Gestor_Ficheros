using System;
using System.Collections.Generic;
using System.IO;

namespace GestorFicheros
{
    class Gestor
    {
        
        private string? ficheroActual = null;

        
        private string? PedirFichero()
        {
            if (ficheroActual != null && File.Exists(ficheroActual))
            {
                return ficheroActual;
            }

            Console.Write("No hay fichero activo. Nombre del fichero a usar: ");
            string nombre = (Console.ReadLine() ?? "").Trim();

            if (nombre == "")
            {
                Console.WriteLine("El nombre no puede estar vacío.");
                return null;
            }
            if (!File.Exists(nombre))
            {
                Console.WriteLine($"El fichero '{nombre}' no existe. Créalo con la opción 1.");
                return null;
            }

            ficheroActual = nombre;
            Console.WriteLine($"Fichero '{nombre}' abierto.");
            return nombre;
        }

        
        public void CrearFichero()
        {
            Console.Write("Nombre del nuevo fichero: ");
            string nombre = (Console.ReadLine() ?? "").Trim();

            if (nombre == "")
            {
                Console.WriteLine("El nombre no puede estar vacío.");
                return;
            }

            if (File.Exists(nombre))
            {
                
                ficheroActual = nombre;
                Console.WriteLine($"El fichero '{nombre}' ya existe. Se ha abierto para trabajar con él.");
                return;
            }

            try
            {
                using (File.Create(nombre)) { }
                ficheroActual = nombre;
                Console.WriteLine($"Fichero '{nombre}' creado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo crear el fichero: {ex.Message}");
            }
        }

        public void AnadirRegistros()
        {
            string? nombre = PedirFichero();
            if (nombre == null) return;

            Console.WriteLine("Escribe los registros (una línea por registro).");
            Console.WriteLine("Deja una línea vacía para terminar.");

            List<string> nuevos = new List<string>();
            while (true)
            {
                Console.Write("> ");
                string linea = (Console.ReadLine() ?? "").Trim();
                if (linea == "") break;
                nuevos.Add(linea);
            }

            if (nuevos.Count == 0)
            {
                Console.WriteLine("No se añadió ningún registro.");
                return;
            }

            try
            {
                File.AppendAllLines(nombre, nuevos);
                Console.WriteLine($"Se añadieron {nuevos.Count} registro(s) a '{nombre}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo escribir en el fichero: {ex.Message}");
            }
        }

        public void EliminarRegistros()
        {
            string? nombre = PedirFichero();
            if (nombre == null) return;

            Console.Write($"¿Seguro que quieres borrar TODOS los registros de '{nombre}'? (s/n): ");
            string confirmar = (Console.ReadLine() ?? "").Trim().ToLower();
            if (confirmar != "s")
            {
                Console.WriteLine("Operación cancelada.");
                return;
            }

            try
            {
                
                File.WriteAllText(nombre, "");
                Console.WriteLine("Todos los registros han sido eliminados.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo vaciar el fichero: {ex.Message}");
            }
        }

        public void EliminarFichero()
        {
            string? nombre = PedirFichero();
            if (nombre == null) return;

            Console.Write($"¿Seguro que quieres eliminar el fichero '{nombre}'? (s/n): ");
            string confirmar = (Console.ReadLine() ?? "").Trim().ToLower();
            if (confirmar != "s")
            {
                Console.WriteLine("Operación cancelada.");
                return;
            }

            try
            {
                File.Delete(nombre);
                Console.WriteLine($"Fichero '{nombre}' eliminado.");
                ficheroActual = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo eliminar el fichero: {ex.Message}");
            }
        }

        public void VisualizarRegistros()
        {
            string? nombre = PedirFichero();
            if (nombre == null) return;

            try
            {
                string[] registros = File.ReadAllLines(nombre);

                if (registros.Length == 0)
                {
                    Console.WriteLine("El fichero no tiene registros.");
                    return;
                }

                Console.WriteLine($"\n--- Registros de '{nombre}' ({registros.Length}) ---");
                for (int i = 0; i < registros.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {registros[i]}");
                }
                Console.WriteLine(new string('-', 30));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No se pudo leer el fichero: {ex.Message}");
            }
        }

        
        private void MostrarMenu()
        {
            string activo = ficheroActual ?? "ninguno";
            Console.WriteLine("\n========== GESTOR DE FICHEROS ==========");
            Console.WriteLine($"Fichero activo: {activo}");
            Console.WriteLine("1. Crear un nuevo fichero");
            Console.WriteLine("2. Añadir registros en el fichero");
            Console.WriteLine("3. Eliminar todos los registros del fichero");
            Console.WriteLine("4. Eliminar el fichero");
            Console.WriteLine("5. Visualizar los registros");
            Console.WriteLine("6. Cerrar la aplicación");
        }

        public void Ejecutar()
        {
            bool salir = false;

            while (!salir)
            {
                MostrarMenu();
                Console.Write("Elige una opción: ");
                string opcion = (Console.ReadLine() ?? "").Trim();

                switch (opcion)
                {
                    case "1": CrearFichero(); break;
                    case "2": AnadirRegistros(); break;
                    case "3": EliminarRegistros(); break;
                    case "4": EliminarFichero(); break;
                    case "5": VisualizarRegistros(); break;
                    case "6":
                        Console.WriteLine("¡Hasta pronto!");
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Introduce un número del 1 al 6.");
                        break;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new Gestor().Ejecutar();
        }
    }
}