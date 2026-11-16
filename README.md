# Taller Algoritmos Modulares (C#)

Aplicación de consola en C# con un menú principal (`Main` solo orquesta) y tres programas independientes. Usa variables, condicionales, ciclos, vectores, matrices, funciones y procedimientos, con validación de datos y sin POO.

**Integrantes:** Juan Pablo Villegas, Mario Aguilar

## Estructura

| Archivo | Contenido |
|---|---|
| `Program.cs` | `Main`: menú y orquestación |
| `Utilidades.cs` | Lectura de datos con validación |
| `SistemaIPS.cs` | Caso 1: turnos de una IPS |
| `ControlAcademico.cs` | Caso 3: control de notas |
| `Parqueadero.cs` | Caso 4: parqueadero de visitantes |

--- 

## Caso 1: Turnos IPS

**Análisis**
- Entradas: documento, nombre, edad, tipo de atención, prioridad (1 a 5; **5 = más alta**).
- Estructuras: 5 vectores paralelos de tamaño 30 + contador `cantidad`.
- Validaciones: cupos (`cantidad < 30`), documento numérico y no repetido, edad 0-120, tipo 1-3, prioridad 1-5.
- Salidas: listado, búsqueda por documento, conteo por tipo, paciente de mayor prioridad (en empate, el primero registrado).

**Prueba de escritorio: mayor prioridad** (prioridades: 2, 5, 3, 5)

| i | prioridades[i] | prioridades[mejor] | ¿Mayor? | mejor |
|---|---|---|---|---|
| inicio | - | - | - | 0 |
| 1 | 5 | 2 | Sí | 1 |
| 2 | 3 | 5 | No | 1 |
| 3 | 5 | 5 | No (empate) | 1 |

Resultado: posición 1. Con `cantidad = 30` el intento 31 se rechaza por falta de cupos.

---

## Caso 3: Control académico

**Análisis**
- Entradas: código, nombre, 3 notas (0.0-5.0), asistencia (0-100).
- Estructuras: matriz `notas[25,3]` (fila = estudiante, columna = parcial) + vectores de código, nombre, asistencia, definitiva y estado.
- Definitiva = promedio de las 3 notas. Estado:
  - Asistencia < 80 % → **Reprueba**
  - Definitiva ≥ 3.0 → **Aprobado**
  - 2.0 ≤ definitiva < 3.0 → **Habilita**
  - Definitiva < 2.0 → **Reprueba**
- Salidas: promedio general, mejor y peor promedio.

**Prueba de escritorio**

| Estudiante | N1 | N2 | N3 | Definitiva | Asistencia | Estado |
|---|---|---|---|---|---|---|
| Ana | 4.0 | 3.5 | 4.5 | 4.00 | 90 % | Aprobado |
| Luis | 2.0 | 2.5 | 3.0 | 2.50 | 85 % | Habilita |
| Pedro | 4.0 | 4.0 | 4.0 | 4.00 | 60 % | Reprueba (asistencia) |

Promedio general = (4.00 + 2.50 + 4.00) / 3 = **3.50**. Mejor: Ana (empata con Pedro, gana el primero). Peor: Luis.

---

## Caso 4: Parqueadero de visitantes

**Análisis**
- Entradas: placa, torre (1-10), apartamento, hora de ingreso, tipo (Carro, Moto, Bicicleta) y hora de salida.
- Estructuras: 6 vectores paralelos de tamaño 40. Las horas se guardan como minutos desde las 00:00 (08:30 = 510); `-1` en la salida significa "sigue dentro".
- Validaciones: máximo 40 ingresos, placa de 3 a 7 letras o números, salida no anterior al ingreso y **una placa no puede ingresar dos veces sin haber salido**.
- Supuestos: una placa puede reingresar tras salir; la mayor permanencia solo considera vehículos que ya salieron.

**Prueba de escritorio**

| Placa | Tipo | Ingreso | Salida | Permanencia |
|---|---|---|---|---|
| ABC123 | Carro | 08:00 (480) | 10:30 (630) | 630 - 480 = 150 min = 2 h 30 min |
| XYZ98A | Moto | 09:15 (555) | 09:45 (585) | 30 min |
| BIC001 | Bicicleta | 10:00 (600) | sigue dentro | - |

- Placa repetida: `ABC123` mientras su salida es `-1` → rechazada; después de salir (salida ≠ -1) → permitida.
- Mayor permanencia: ABC123 (150 > 30); BIC001 se omite por no haber salido.
- Ingresos por tipo: Carro 1, Moto 1, Bicicleta 1.