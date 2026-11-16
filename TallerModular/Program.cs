using System;

namespace TallerModular
{
    internal static class Program
    {
        // Main SOLO orquesta: muestra el menú, lee la opción y llama al módulo elegido.
        static void Main()
        {
            Utilidades.ConfigurarConsola();
            int opcion;

            do
            {
                MostrarMenu();
                opcion = Utilidades.LeerEntero("Seleccione una opción: ", 0, 3);

                switch (opcion)
                {
                    case 1:
                        SistemaIPS.Ejecutar();
                        break;
                    case 2:
                        ControlAcademico.Ejecutar();
                        break;
                    case 3:
                        ProduccionFabrica.Ejecutar();
                        break;
                    case 0:
                        Console.WriteLine("\nHasta pronto.");
                        break;
                }
            } while (opcion != 0);
        }

        // PROCEDIMIENTO: no devuelve nada, solo imprime.
        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== TALLER ALGORITMOS MODULARES ===");
            Console.WriteLine("1. Sistema de turnos IPS");
            Console.WriteLine("2. Control académico (notas)");
            Console.WriteLine("3. Producción de la fábrica");
            Console.WriteLine("0. Salir");
            Console.WriteLine("-----------------------------------");
        }
    }
}