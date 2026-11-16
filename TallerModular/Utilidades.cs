using System;
using System.Globalization;
using System.Text;

namespace TallerModular
{
    // Funciones de apoyo para leer datos del usuario VALIDÁNDOLOS.
    // Cada una repite la pregunta (ciclo) hasta que el dato sea correcto.
    internal static class Utilidades
    {
        public static void ConfigurarConsola()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        // FUNCIÓN: devuelve un entero entre min y max.
        public static int LeerEntero(string mensaje, int min, int max)
        {
            int valor;
            while (true)
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine() ?? "";
                if (int.TryParse(entrada, out valor) && valor >= min && valor <= max)
                    return valor;
                Console.WriteLine($"  Valor inválido. Ingrese un número entero entre {min} y {max}.");
            }
        }

        // FUNCIÓN: devuelve un decimal entre min y max (acepta coma o punto).
        public static double LeerDecimal(string mensaje, double min, double max)
        {
            double valor;
            while (true)
            {
                Console.Write(mensaje);
                string entrada = (Console.ReadLine() ?? "").Replace(',', '.');
                if (double.TryParse(entrada, NumberStyles.Float, CultureInfo.InvariantCulture, out valor)
                    && valor >= min && valor <= max)
                    return valor;
                Console.WriteLine($"  Valor inválido. Ingrese un número entre {min} y {max}.");
            }
        }

        // FUNCIÓN: devuelve un texto que no esté vacío.
        public static string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string entrada = (Console.ReadLine() ?? "").Trim();
                if (entrada.Length > 0)
                    return entrada;
                Console.WriteLine("  No puede estar vacío.");
            }
        }

        // FUNCIÓN: devuelve un texto compuesto solo por dígitos (para documentos/códigos numéricos).
        public static string LeerNumeroComoTexto(string mensaje)
        {
            while (true)
            {
                string entrada = LeerTexto(mensaje);
                bool soloDigitos = true;
                foreach (char c in entrada)
                {
                    if (!char.IsDigit(c)) { soloDigitos = false; break; }
                }
                if (soloDigitos)
                    return entrada;
                Console.WriteLine("  Solo se permiten números.");
            }
        }

        // PROCEDIMIENTO: espera una tecla para que el usuario alcance a leer.
        public static void Pausa()
        {
            Console.WriteLine("\nPresione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}