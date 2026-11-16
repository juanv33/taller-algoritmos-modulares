# Taller Algoritmos Modulares (C#)

**Integrantes:** _(nombre 1)_, _(nombre 2)_, _(nombre 3)_

**Casos desarrollados:** 1 (IPS), 3 (Control académico) y _(caso del compañero)_.

## Estructura

| Archivo | Contenido |
|---|---|
| `Program.cs` | `Main`: solo orquesta el menú |
| `Utilidades.cs` | Funciones de lectura con validación |
| `SistemaIPS.cs` | Caso 1 |
| `ControlAcademico.cs` | Caso 3 |

Se usan clases `static` solo como contenedores de funciones (C# lo exige); no hay objetos ni POO.

---

## Caso 1: Sistema de turnos IPS

### Análisis
- **Entradas:** documento, nombre, edad, tipo de atención, prioridad.
- **Salidas:** listado, paciente encontrado, conteo por tipo, paciente de mayor prioridad.
- **Estructuras:** 5 vectores de tamaño 30 + contador `cantidad`.
- **Validaciones:** cupos (`cantidad < 30`), documento solo números y no repetido, edad 0-120, tipo 1-3, prioridad 1-5.
- **Supuesto:** prioridad 5 = más alta. En empate gana el primero registrado.

### Prueba de escritorio: mayor prioridad

Datos: P0 (prioridad 2), P1 (prioridad 5), P2 (prioridad 3), P3 (prioridad 5). `cantidad = 4`.

| Paso | i | prioridades[i] | prioridades[mejor] | ¿Mayor? | mejor |
|---|---|---|---|---|---|
| inicio | - | - | - | - | 0 |
| 1 | 1 | 5 | 2 | Sí | 1 |
| 2 | 2 | 3 | 5 | No | 1 |
| 3 | 3 | 5 | 5 | No (empate) | 1 |

Resultado: posición 1 (P1).

### Prueba de escritorio: cupos

| Intento | cantidad antes | ¿cantidad < 30? | Resultado |
|---|---|---|---|
| 1 | 0 | Sí | Registra, cantidad = 1 |
| 30 | 29 | Sí | Registra, cantidad = 30 |
| 31 | 30 | No | "No hay cupos disponibles" |

---

## Caso 3: Control académico

### Análisis
- **Entradas:** código, nombre, 3 notas (0.0-5.0), asistencia (0-100).
- **Salidas:** definitiva, estado, promedio general, mejor y peor promedio.
- **Estructuras:** matriz `notas[25,3]` + vectores para código, nombre, asistencia, definitiva y estado.
- **Reglas de estado:**
  - Asistencia < 80 % → **Reprueba**
  - Definitiva ≥ 3.0 → **Aprobado**
  - 2.0 ≤ definitiva < 3.0 → **Habilita**
  - Definitiva < 2.0 → **Reprueba**

### Prueba de escritorio

| Estudiante | N1 | N2 | N3 | Suma | Definitiva (suma/3) | Asistencia | Estado |
|---|---|---|---|---|---|---|---|
| Ana | 4.0 | 3.5 | 4.5 | 12.0 | 4.00 | 90 % | Aprobado |
| Luis | 2.0 | 2.5 | 3.0 | 7.5 | 2.50 | 85 % | Habilita |
| Pedro | 4.0 | 4.0 | 4.0 | 12.0 | 4.00 | 60 % | Reprueba (asistencia) |

**Promedio general** = (4.00 + 2.50 + 4.00) / 3 = **3.50**

**Mejor / peor promedio** (`pos` guarda la posición):

| i | definitivas[i] | definitivas[pos] mejor | mejor pos | peor pos |
|---|---|---|---|---|
| inicio | - | - | 0 | 0 |
| 1 | 2.50 | 4.00 | 0 (no supera) | 1 (menor) |
| 2 | 4.00 | 4.00 | 0 (empate) | 1 |

Resultado: mejor = Ana, peor = Luis.