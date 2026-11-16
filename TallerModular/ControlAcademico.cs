using System;

namespace TallerModular
{
    // CASO 3: Control académico para seguimiento de notas.
    // Vectores paralelos para datos simples + MATRIZ notas[estudiante, parcial].
    internal static class ControlAcademico
    {
        const int MAX_ESTUDIANTES = 25;
        const int NUM_NOTAS = 3;
        const double NOTA_APROBAR = 3.0;
        const double NOTA_HABILITAR = 2.0;
        const double ASISTENCIA_MINIMA = 80.0;

        static string[] codigos = new string[MAX_ESTUDIANTES];
        static string[] nombres = new string[MAX_ESTUDIANTES];
        static double[,] notas = new double[MAX_ESTUDIANTES, NUM_NOTAS]; // MATRIZ 25 x 3
        static double[] asistencias = new double[MAX_ESTUDIANTES];
        static double[] definitivas = new double[MAX_ESTUDIANTES];
        static string[] estados = new string[MAX_ESTUDIANTES];
        static int cantidad = 0;

        // ---------- Procedimiento principal del módulo ----------
        public static void Ejecutar()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== CONTROL ACADÉMICO ===");
                Console.WriteLine($"Estudiantes registrados: {cantidad} de {MAX_ESTUDIANTES}");
                Console.WriteLine("1. Registrar estudiante");
                Console.WriteLine("2. Calcular definitivas y estados (ver listado)");
                Console.WriteLine("3. Promedio general del grupo");
                Console.WriteLine("4. Mejor y peor promedio");
                Console.WriteLine("0. Volver al menú principal");
                opcion = Utilidades.LeerEntero("Opción: ", 0, 4);

                switch (opcion)
                {
                    case 1: RegistrarEstudiante(); break;
                    case 2: CalcularYMostrarTodos(); break;
                    case 3: MostrarPromedioGeneral(); break;
                    case 4: MostrarMejorYPeor(); break;
                }
                if (opcion != 0) Utilidades.Pausa();
            } while (opcion != 0);
        }

        // ---------- FUNCIONES ----------

        static int BuscarPosicion(string codigo)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (codigos[i] == codigo)
                    return i;
            }
            return -1;
        }

        // Definitiva = promedio de las 3 notas del estudiante i (recorre una FILA de la matriz).
        static double CalcularDefinitiva(int i)
        {
            double suma = 0;
            for (int j = 0; j < NUM_NOTAS; j++)
                suma += notas[i, j];
            return suma / NUM_NOTAS;
        }

        // Regla del estado (ver README). La asistencia baja reprueba aunque la nota sea buena.
        static string DeterminarEstado(double definitiva, double asistencia)
        {
            if (asistencia < ASISTENCIA_MINIMA) return "Reprueba";
            if (definitiva >= NOTA_APROBAR) return "Aprobado";
            if (definitiva >= NOTA_HABILITAR) return "Habilita";
            return "Reprueba";
        }

        // Promedio de las definitivas de todo el grupo (0 si no hay estudiantes).
        static double CalcularPromedioGeneral()
        {
            if (cantidad == 0) return 0;

            double suma = 0;
            for (int i = 0; i < cantidad; i++)
                suma += definitivas[i];
            return suma / cantidad;
        }

        // Posición del mejor (true) o del peor (false) promedio.
        static int PosicionExtremo(bool buscarMejor)
        {
            if (cantidad == 0) return -1;

            int pos = 0;
            for (int i = 1; i < cantidad; i++)
            {
                if (buscarMejor && definitivas[i] > definitivas[pos]) pos = i;
                if (!buscarMejor && definitivas[i] < definitivas[pos]) pos = i;
            }
            return pos;
        }

        // ---------- PROCEDIMIENTOS ----------

        static void RegistrarEstudiante()
        {
            Console.WriteLine("\n--- Registrar estudiante ---");

            if (cantidad >= MAX_ESTUDIANTES)
            {
                Console.WriteLine("Se alcanzó el máximo de 25 estudiantes.");
                return;
            }

            string codigo = Utilidades.LeerNumeroComoTexto("Código: ");
            if (BuscarPosicion(codigo) != -1)
            {
                Console.WriteLine("Ya existe un estudiante con ese código. Registro cancelado.");
                return;
            }

            codigos[cantidad] = codigo;
            nombres[cantidad] = Utilidades.LeerTexto("Nombre: ");

            for (int j = 0; j < NUM_NOTAS; j++)
                notas[cantidad, j] = Utilidades.LeerDecimal($"Nota parcial {j + 1} (0.0 - 5.0): ", 0.0, 5.0);

            asistencias[cantidad] = Utilidades.LeerDecimal("Porcentaje de asistencia (0 - 100): ", 0, 100);

            // Se calculan de una vez para que el listado siempre esté al día.
            definitivas[cantidad] = CalcularDefinitiva(cantidad);
            estados[cantidad] = DeterminarEstado(definitivas[cantidad], asistencias[cantidad]);

            cantidad++;
            Console.WriteLine("Estudiante registrado correctamente.");
        }

        static void MostrarEstudiante(int i)
        {
            Console.WriteLine($"{codigos[i],-10} {nombres[i],-20} " +
                              $"{notas[i, 0],4:0.0} {notas[i, 1],4:0.0} {notas[i, 2],4:0.0}  " +
                              $"{definitivas[i],5:0.00} {asistencias[i],5:0}%  {estados[i]}");
        }

        static void CalcularYMostrarTodos()
        {
            Console.WriteLine("\n--- Definitivas y estados ---");
            if (cantidad == 0)
            {
                Console.WriteLine("No hay estudiantes registrados.");
                return;
            }

            // Se recalcula todo, por si cambian las reglas o los datos.
            for (int i = 0; i < cantidad; i++)
            {
                definitivas[i] = CalcularDefinitiva(i);
                estados[i] = DeterminarEstado(definitivas[i], asistencias[i]);
            }

            Console.WriteLine($"{"Código",-10} {"Nombre",-20} {"N1",4} {"N2",4} {"N3",4}  {"Def",5} {"Asis",6}  Estado");
            for (int i = 0; i < cantidad; i++)
                MostrarEstudiante(i);
        }

        static void MostrarPromedioGeneral()
        {
            Console.WriteLine("\n--- Promedio general del grupo ---");
            if (cantidad == 0)
            {
                Console.WriteLine("No hay estudiantes registrados.");
                return;
            }
            Console.WriteLine($"Promedio general: {CalcularPromedioGeneral():0.00}");
        }

        static void MostrarMejorYPeor()
        {
            Console.WriteLine("\n--- Mejor y peor promedio ---");
            if (cantidad == 0)
            {
                Console.WriteLine("No hay estudiantes registrados.");
                return;
            }

            Console.WriteLine("Mejor promedio:");
            MostrarEstudiante(PosicionExtremo(true));
            Console.WriteLine("Peor promedio:");
            MostrarEstudiante(PosicionExtremo(false));
        }
    }
}