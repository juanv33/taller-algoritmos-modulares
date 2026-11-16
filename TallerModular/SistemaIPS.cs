using System;

namespace TallerModular
{
    internal static class SistemaIPS
    {
        const int MAX_PACIENTES = 30;

        static string[] documentos = new string[MAX_PACIENTES];
        static string[] nombres = new string[MAX_PACIENTES];
        static int[] edades = new int[MAX_PACIENTES];
        static int[] tipos = new int[MAX_PACIENTES];        
        static int[] prioridades = new int[MAX_PACIENTES];  
        static int cantidad = 0;                            

        static readonly string[] TIPOS_ATENCION = { "Urgencias", "Consulta General", "Prioritaria" };

        
        public static void Ejecutar()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA DE TURNOS IPS ===");
                Console.WriteLine($"Cupos disponibles: {MAX_PACIENTES - cantidad} de {MAX_PACIENTES}");
                Console.WriteLine("1. Registrar paciente");
                Console.WriteLine("2. Mostrar listado general");
                Console.WriteLine("3. Buscar paciente por documento");
                Console.WriteLine("4. Pacientes por tipo de atención");
                Console.WriteLine("5. Paciente con mayor prioridad");
                Console.WriteLine("0. Volver al menú principal");
                opcion = Utilidades.LeerEntero("Opción: ", 0, 5);

                switch (opcion)
                {
                    case 1: RegistrarPaciente(); break;
                    case 2: MostrarListado(); break;
                    case 3: BuscarPaciente(); break;
                    case 4: MostrarConteoPorTipo(); break;
                    case 5: MostrarMayorPrioridad(); break;
                }
                if (opcion != 0) Utilidades.Pausa();
            } while (opcion != 0);
        }

        static int BuscarPosicion(string documento)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (documentos[i] == documento)
                    return i;
            }
            return -1;
        }

        static bool HayCupos()
        {
            return cantidad < MAX_PACIENTES;
        }

        static int PosicionMayorPrioridad()
        {
            if (cantidad == 0) return -1;

            int mejor = 0;
            for (int i = 1; i < cantidad; i++)
            {
                if (prioridades[i] > prioridades[mejor])
                    mejor = i;
            }
            return mejor;
        }

        static int[] ContarPorTipo()
        {
            int[] conteo = new int[TIPOS_ATENCION.Length];
            for (int i = 0; i < cantidad; i++)
            {
                conteo[tipos[i]]++;
            }
            return conteo;
        }


        static void RegistrarPaciente()
        {
            Console.WriteLine("\n--- Registrar paciente ---");

            if (!HayCupos())
            {
                Console.WriteLine("No hay cupos disponibles. Se alcanzó el máximo de 30 pacientes.");
                return;
            }

            string documento = Utilidades.LeerNumeroComoTexto("Número de documento: ");
            if (BuscarPosicion(documento) != -1)
            {
                Console.WriteLine("Ya existe un paciente con ese documento. Registro cancelado.");
                return;
            }

            documentos[cantidad] = documento;
            nombres[cantidad] = Utilidades.LeerTexto("Nombre completo: ");
            edades[cantidad] = Utilidades.LeerEntero("Edad (0-120): ", 0, 120);

            Console.WriteLine("Tipo de atención:");
            for (int i = 0; i < TIPOS_ATENCION.Length; i++)
                Console.WriteLine($"  {i + 1}. {TIPOS_ATENCION[i]}");
            tipos[cantidad] = Utilidades.LeerEntero("Seleccione tipo: ", 1, 3) - 1;

            prioridades[cantidad] = Utilidades.LeerEntero("Nivel de prioridad (1 a 5, siendo 5 la más alta): ", 1, 5);

            cantidad++; 
            Console.WriteLine($"Paciente registrado. Cupos restantes: {MAX_PACIENTES - cantidad}");
        }

        static void MostrarPaciente(int i)
        {
            Console.WriteLine($"{documentos[i],-12} {nombres[i],-25} {edades[i],4}  {TIPOS_ATENCION[tipos[i]],-18} {prioridades[i],3}");
        }

        static void MostrarListado()
        {
            Console.WriteLine("\n--- Listado general ---");
            if (cantidad == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                return;
            }

            Console.WriteLine($"{"Documento",-12} {"Nombre",-25} {"Edad",4}  {"Atención",-18} {"Prio",3}");
            for (int i = 0; i < cantidad; i++)
                MostrarPaciente(i);
        }

        static void BuscarPaciente()
        {
            Console.WriteLine("\n--- Buscar por documento ---");
            string documento = Utilidades.LeerNumeroComoTexto("Documento a buscar: ");
            int pos = BuscarPosicion(documento);

            if (pos == -1)
                Console.WriteLine("No se encontró ningún paciente con ese documento.");
            else
                MostrarPaciente(pos);
        }

        static void MostrarConteoPorTipo()
        {
            Console.WriteLine("\n--- Pacientes por tipo de atención ---");
            int[] conteo = ContarPorTipo();
            for (int i = 0; i < TIPOS_ATENCION.Length; i++)
                Console.WriteLine($"{TIPOS_ATENCION[i],-18}: {conteo[i]}");
        }

        static void MostrarMayorPrioridad()
        {
            Console.WriteLine("\n--- Paciente con mayor prioridad ---");
            int pos = PosicionMayorPrioridad();
            if (pos == -1)
                Console.WriteLine("No hay pacientes registrados.");
            else
                MostrarPaciente(pos);
        }
    }
}