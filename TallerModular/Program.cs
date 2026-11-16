using System;

namespace TallerModular
{
    internal static class Program
    {
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
                        Parqueadero.Ejecutar();
                        break;
                    case 0:
                        Console.WriteLine("\nHasta pronto.");
                        break;
                }
            } while (opcion != 0);
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("=== TALLER ALGORITMOS MODULARES ===");
            Console.WriteLine("1. Sistema de turnos IPS");
            Console.WriteLine("2. Control académico (notas)");
            Console.WriteLine("3. Parqueadero de visitantes");
            Console.WriteLine("0. Salir");
            Console.WriteLine("-----------------------------------");
        }
    }
}