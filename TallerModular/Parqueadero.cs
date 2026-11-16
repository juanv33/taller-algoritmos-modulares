using System;

namespace TallerModular
{
    internal static class Parqueadero
    {
        const int MAX_INGRESOS = 40;
        const int MAX_TORRES = 10;
        const int MAX_APARTAMENTO = 9999;
        const int SIN_SALIDA = -1;  

        static readonly string[] TIPOS_VEHICULO = { "Carro", "Moto", "Bicicleta" };

        static string[] placas = new string[MAX_INGRESOS];
        static int[] torres = new int[MAX_INGRESOS];
        static int[] apartamentos = new int[MAX_INGRESOS];
        static int[] horasIngreso = new int[MAX_INGRESOS];   
        static int[] horasSalida = new int[MAX_INGRESOS];    
        static int[] tipos = new int[MAX_INGRESOS];          
        static int cantidad = 0;                             

        public static void Ejecutar()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== PARQUEADERO DE VISITANTES ===");
                Console.WriteLine($"Ingresos del día: {cantidad} de {MAX_INGRESOS}");
                Console.WriteLine("1. Registrar ingreso");
                Console.WriteLine("2. Registrar salida de vehículo");
                Console.WriteLine("3. Calcular tiempo de permanencia por placa");
                Console.WriteLine("4. Cantidad de ingresos por tipo");
                Console.WriteLine("5. Vehículo con mayor permanencia");
                Console.WriteLine("6. Listado de ingresos del día");
                Console.WriteLine("0. Volver al menú principal");
                opcion = Utilidades.LeerEntero("Opción: ", 0, 6);

                switch (opcion)
                {
                    case 1: RegistrarIngreso(); break;
                    case 2: RegistrarSalida(); break;
                    case 3: ConsultarPermanencia(); break;
                    case 4: MostrarIngresosPorTipo(); break;
                    case 5: MostrarMayorPermanencia(); break;
                    case 6: MostrarListado(); break;
                }
                if (opcion != 0) Utilidades.Pausa();
            } while (opcion != 0);
        }

        static bool HayCupos()
        {
            return cantidad < MAX_INGRESOS;
        }

        static int BuscarActivo(string placa)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (placas[i] == placa && horasSalida[i] == SIN_SALIDA)
                    return i;
            }
            return -1;
        }

        static int BuscarUltimoRegistro(string placa)
        {
            for (int i = cantidad - 1; i >= 0; i--)
            {
                if (placas[i] == placa)
                    return i;
            }
            return -1;
        }

        static int CalcularPermanencia(int i)
        {
            return horasSalida[i] - horasIngreso[i];
        }

        static int[] ContarPorTipo()
        {
            int[] conteo = new int[TIPOS_VEHICULO.Length];
            for (int i = 0; i < cantidad; i++)
            {
                conteo[tipos[i]]++;
            }
            return conteo;
        }

        static int PosicionMayorPermanencia()
        {
            int mejor = -1;
            for (int i = 0; i < cantidad; i++)
            {
                if (horasSalida[i] == SIN_SALIDA) continue; 

                if (mejor == -1 || CalcularPermanencia(i) > CalcularPermanencia(mejor))
                    mejor = i;
            }
            return mejor;
        }

        static string FormatearHora(int minutos)
        {
            return $"{minutos / 60:00}:{minutos % 60:00}";
        }

        static string FormatearDuracion(int minutos)
        {
            return $"{minutos / 60} h {minutos % 60:00} min";
        }

        // Pide una placa válida: de 3 a 7 caracteres, solo letras y números. Se guarda en mayúscula.
        static string LeerPlaca(string mensaje)
        {
            while (true)
            {
                string placa = Utilidades.LeerTexto(mensaje).ToUpperInvariant();
                bool valida = placa.Length >= 3 && placa.Length <= 7;

                foreach (char c in placa)
                {
                    if (!char.IsLetterOrDigit(c))
                    {
                        valida = false;
                        break;
                    }
                }

                if (valida)
                    return placa;
                Console.WriteLine("  Placa inválida. Use de 3 a 7 letras o números, sin espacios.");
            }
        }

        // Pide hora y minutos; devuelve minutos desde 00:00. No acepta una hora anterior a 'minimo'.
        static int LeerHora(string mensaje, int minimo)
        {
            while (true)
            {
                Console.WriteLine(mensaje);
                int horas = Utilidades.LeerEntero("  Hora (0-23): ", 0, 23);
                int minutos = Utilidades.LeerEntero("  Minutos (0-59): ", 0, 59);
                int total = horas * 60 + minutos;

                if (total >= minimo)
                    return total;
                Console.WriteLine($"  La hora no puede ser anterior a {FormatearHora(minimo)}.");
            }
        }

        // ---------- PROCEDIMIENTOS (no devuelven valor) ----------

        static void RegistrarIngreso()
        {
            Console.WriteLine("\n--- Registrar ingreso ---");

            if (!HayCupos())
            {
                Console.WriteLine("Se alcanzó el máximo de 40 ingresos diarios.");
                return;
            }

            string placa = LeerPlaca("Placa: ");
            if (BuscarActivo(placa) != -1)
            {
                Console.WriteLine("Esa placa ya está dentro del parqueadero. Debe registrar su salida antes de un nuevo ingreso.");
                return;
            }

            placas[cantidad] = placa;
            torres[cantidad] = Utilidades.LeerEntero($"Torre visitada (1-{MAX_TORRES}): ", 1, MAX_TORRES);
            apartamentos[cantidad] = Utilidades.LeerEntero("Apartamento: ", 1, MAX_APARTAMENTO);
            horasIngreso[cantidad] = LeerHora("Hora de ingreso:", 0);

            Console.WriteLine("Tipo de vehículo:");
            for (int i = 0; i < TIPOS_VEHICULO.Length; i++)
                Console.WriteLine($"  {i + 1}. {TIPOS_VEHICULO[i]}");
            tipos[cantidad] = Utilidades.LeerEntero("Seleccione tipo: ", 1, 3) - 1;

            horasSalida[cantidad] = SIN_SALIDA;

            cantidad++; 
            Console.WriteLine($"Ingreso registrado. Ingresos restantes del día: {MAX_INGRESOS - cantidad}");
        }

        static void RegistrarSalida()
        {
            Console.WriteLine("\n--- Registrar salida de vehículo ---");

            string placa = LeerPlaca("Placa: ");
            int pos = BuscarActivo(placa);
            if (pos == -1)
            {
                Console.WriteLine("Ese vehículo no se encuentra dentro del parqueadero.");
                return;
            }

            horasSalida[pos] = LeerHora("Hora de salida:", horasIngreso[pos]);

            Console.WriteLine($"Salida registrada. {TIPOS_VEHICULO[tipos[pos]]} {placas[pos]} " +
                              $"(Torre {torres[pos]}, Apto {apartamentos[pos]})");
            Console.WriteLine($"Ingresó {FormatearHora(horasIngreso[pos])} - Salió {FormatearHora(horasSalida[pos])}");
            Console.WriteLine($"Tiempo de permanencia: {FormatearDuracion(CalcularPermanencia(pos))}");
        }

        static void ConsultarPermanencia()
        {
            Console.WriteLine("\n--- Tiempo de permanencia ---");

            string placa = LeerPlaca("Placa: ");
            int pos = BuscarUltimoRegistro(placa);
            if (pos == -1)
            {
                Console.WriteLine("No hay ingresos registrados con esa placa.");
                return;
            }

            if (horasSalida[pos] != SIN_SALIDA)
            {
                Console.WriteLine($"Ingresó {FormatearHora(horasIngreso[pos])} - Salió {FormatearHora(horasSalida[pos])}");
                Console.WriteLine($"Tiempo de permanencia: {FormatearDuracion(CalcularPermanencia(pos))}");
            }
            else
            {
                Console.WriteLine($"El vehículo sigue dentro desde las {FormatearHora(horasIngreso[pos])}.");
                int ahora = LeerHora("Indique la hora actual:", horasIngreso[pos]);
                Console.WriteLine($"Lleva {FormatearDuracion(ahora - horasIngreso[pos])} en el parqueadero.");
            }
        }

        static void MostrarIngresosPorTipo()
        {
            Console.WriteLine("\n--- Ingresos por tipo de vehículo ---");
            int[] conteo = ContarPorTipo();
            for (int i = 0; i < TIPOS_VEHICULO.Length; i++)
                Console.WriteLine($"{TIPOS_VEHICULO[i],-10}: {conteo[i]}");
        }

        static void MostrarMayorPermanencia()
        {
            Console.WriteLine("\n--- Vehículo con mayor permanencia ---");
            int pos = PosicionMayorPermanencia();
            if (pos == -1)
            {
                Console.WriteLine("Aún no hay vehículos que hayan registrado su salida.");
                return;
            }

            Console.WriteLine($"{TIPOS_VEHICULO[tipos[pos]]} {placas[pos]} (Torre {torres[pos]}, Apto {apartamentos[pos]})");
            Console.WriteLine($"Permanencia: {FormatearDuracion(CalcularPermanencia(pos))}");
        }

        static void MostrarListado()
        {
            Console.WriteLine("\n--- Ingresos del día ---");
            if (cantidad == 0)
            {
                Console.WriteLine("No hay ingresos registrados.");
                return;
            }

            Console.WriteLine($"{"Placa",-8} {"Tipo",-10} {"Torre",5} {"Apto",5} {"Ingreso",8} {"Salida",8}  Permanencia");
            for (int i = 0; i < cantidad; i++)
            {
                string salida = horasSalida[i] == SIN_SALIDA ? "Dentro" : FormatearHora(horasSalida[i]);
                string permanencia = horasSalida[i] == SIN_SALIDA ? "-" : FormatearDuracion(CalcularPermanencia(i));

                Console.WriteLine($"{placas[i],-8} {TIPOS_VEHICULO[tipos[i]],-10} {torres[i],5} {apartamentos[i],5} " +
                                  $"{FormatearHora(horasIngreso[i]),8} {salida,8}  {permanencia}");
            }
        }
    }
}