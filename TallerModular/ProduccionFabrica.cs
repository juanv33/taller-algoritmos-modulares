using System;

namespace TallerModular
{
    // CASO 5: Control de producción para una fábrica de empaques.
    // MATRIZ produccion[operario, dia]: 10 filas (operarios) x 5 columnas (días).
    internal static class ProduccionFabrica
    {
        const int MAX_OPERARIOS = 10;
        const int DIAS = 5;
        const int UMBRAL_BAJA = 50;          // por debajo de esto, el día cuenta como "baja producción"
        const int MAX_PRODUCCION = 100000;   // tope razonable para detectar errores de digitación

        static readonly string[] NOMBRES_DIAS = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes" };

        static string[] codigos = new string[MAX_OPERARIOS];
        static string[] nombres = new string[MAX_OPERARIOS];
        static int[,] produccion = new int[MAX_OPERARIOS, DIAS];     // MATRIZ 10 x 5
        static bool[] tieneProduccion = new bool[MAX_OPERARIOS];     // ¿ya se ingresó su semana?
        static int cantidad = 0;                                     // operarios registrados

        // ---------- Procedimiento principal del módulo ----------
        public static void Ejecutar()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== PRODUCCIÓN DE LA FÁBRICA ===");
                Console.WriteLine($"Operarios registrados: {cantidad} de {MAX_OPERARIOS}");
                Console.WriteLine("1. Registrar operario");
                Console.WriteLine("2. Ingresar producción diaria");
                Console.WriteLine("3. Producción semanal por operario");
                Console.WriteLine("4. Mejor operario de la semana");
                Console.WriteLine("5. Promedio general de producción");
                Console.WriteLine("6. Detectar días con baja producción");
                Console.WriteLine("0. Volver al menú principal");
                opcion = Utilidades.LeerEntero("Opción: ", 0, 6);

                switch (opcion)
                {
                    case 1: RegistrarOperario(); break;
                    case 2: IngresarProduccion(); break;
                    case 3: MostrarProduccionSemanal(); break;
                    case 4: MostrarMejorOperario(); break;
                    case 5: MostrarPromedioGeneral(); break;
                    case 6: MostrarDiasBajaProduccion(); break;
                }
                if (opcion != 0) Utilidades.Pausa();
            } while (opcion != 0);
        }

        // ---------- FUNCIONES ----------

        // Posición del operario con ese código, o -1 si no existe.
        static int BuscarPosicion(string codigo)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (codigos[i] == codigo)
                    return i;
            }
            return -1;
        }

        // Producción semanal = suma de la FILA i de la matriz.
        static int CalcularProduccionSemanal(int i)
        {
            int suma = 0;
            for (int d = 0; d < DIAS; d++)
                suma += produccion[i, d];
            return suma;
        }

        // Cuántos operarios ya tienen su semana registrada.
        static int ContarConProduccion()
        {
            int total = 0;
            for (int i = 0; i < cantidad; i++)
            {
                if (tieneProduccion[i]) total++;
            }
            return total;
        }

        // Posición del operario con mayor producción semanal (-1 si nadie tiene producción).
        static int PosicionMejorOperario()
        {
            int mejor = -1;
            for (int i = 0; i < cantidad; i++)
            {
                if (!tieneProduccion[i]) continue;

                if (mejor == -1 || CalcularProduccionSemanal(i) > CalcularProduccionSemanal(mejor))
                    mejor = i;
            }
            return mejor;
        }

        // Suma de la producción semanal de todos los operarios que ya tienen datos.
        static int CalcularProduccionTotal()
        {
            int total = 0;
            for (int i = 0; i < cantidad; i++)
            {
                if (tieneProduccion[i])
                    total += CalcularProduccionSemanal(i);
            }
            return total;
        }

        // ---------- PROCEDIMIENTOS ----------

        static void RegistrarOperario()
        {
            Console.WriteLine("\n--- Registrar operario ---");

            if (cantidad >= MAX_OPERARIOS)
            {
                Console.WriteLine("Se alcanzó el máximo de 10 operarios.");
                return;
            }

            string codigo = Utilidades.LeerNumeroComoTexto("Código de operario: ");
            if (BuscarPosicion(codigo) != -1)
            {
                Console.WriteLine("Ya existe un operario con ese código. Registro cancelado.");
                return;
            }

            codigos[cantidad] = codigo;
            nombres[cantidad] = Utilidades.LeerTexto("Nombre: ");
            cantidad++;
            Console.WriteLine("Operario registrado correctamente.");
        }

        static void IngresarProduccion()
        {
            Console.WriteLine("\n--- Ingresar producción diaria ---");

            if (cantidad == 0)
            {
                Console.WriteLine("Primero debe registrar al menos un operario.");
                return;
            }

            string codigo = Utilidades.LeerNumeroComoTexto("Código del operario: ");
            int pos = BuscarPosicion(codigo);
            if (pos == -1)
            {
                Console.WriteLine("No existe un operario con ese código.");
                return;
            }

            Console.WriteLine($"Operario: {nombres[pos]}");
            for (int d = 0; d < DIAS; d++)
            {
                // El mínimo 0 garantiza que no se acepten valores negativos.
                produccion[pos, d] = Utilidades.LeerEntero($"Producción del {NOMBRES_DIAS[d]} (unidades): ", 0, MAX_PRODUCCION);
            }
            tieneProduccion[pos] = true;
            Console.WriteLine("Producción registrada correctamente.");
        }

        static void MostrarProduccionSemanal()
        {
            Console.WriteLine("\n--- Producción semanal por operario ---");
            if (cantidad == 0)
            {
                Console.WriteLine("No hay operarios registrados.");
                return;
            }

            Console.Write($"{"Código",-10} {"Nombre",-20}");
            for (int d = 0; d < DIAS; d++)
                Console.Write($" {NOMBRES_DIAS[d].Substring(0, 3),5}");
            Console.WriteLine($" {"Total",7}");

            for (int i = 0; i < cantidad; i++)
            {
                Console.Write($"{codigos[i],-10} {nombres[i],-20}");
                if (tieneProduccion[i])
                {
                    for (int d = 0; d < DIAS; d++)
                        Console.Write($" {produccion[i, d],5}");
                    Console.WriteLine($" {CalcularProduccionSemanal(i),7}");
                }
                else
                {
                    Console.WriteLine("  (sin producción ingresada)");
                }
            }
        }

        static void MostrarMejorOperario()
        {
            Console.WriteLine("\n--- Mejor operario de la semana ---");
            int pos = PosicionMejorOperario();
            if (pos == -1)
            {
                Console.WriteLine("Aún no hay producción ingresada.");
                return;
            }
            Console.WriteLine($"{nombres[pos]} (código {codigos[pos]}) con {CalcularProduccionSemanal(pos)} unidades.");
        }

        static void MostrarPromedioGeneral()
        {
            Console.WriteLine("\n--- Promedio general de producción ---");
            int operarios = ContarConProduccion();
            if (operarios == 0)
            {
                Console.WriteLine("Aún no hay producción ingresada.");
                return;
            }

            double total = CalcularProduccionTotal();
            Console.WriteLine($"Promedio semanal por operario: {total / operarios:0.00} unidades");
            Console.WriteLine($"Promedio diario por operario:  {total / (operarios * DIAS):0.00} unidades");
        }

        static void MostrarDiasBajaProduccion()
        {
            Console.WriteLine($"\n--- Días con baja producción (menos de {UMBRAL_BAJA} unidades) ---");
            bool hayAlguno = false;

            for (int i = 0; i < cantidad; i++)
            {
                if (!tieneProduccion[i]) continue;

                for (int d = 0; d < DIAS; d++)
                {
                    if (produccion[i, d] < UMBRAL_BAJA)
                    {
                        Console.WriteLine($"{nombres[i]} - {NOMBRES_DIAS[d]}: {produccion[i, d]} unidades");
                        hayAlguno = true;
                    }
                }
            }

            if (!hayAlguno)
                Console.WriteLine("No se detectaron días con baja producción.");
        }
    }
}