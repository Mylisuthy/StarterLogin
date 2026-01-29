# StarterLogin: Guia Maestra de Composicion y Ambiente (WLC)

Esta es la guía definitiva para entender profundamente la arquitectura, el flujo de datos y la composición técnica del proyecto StarterLogin. Ha sido diseñada como un recurso de estudio para desarrolladores Senior que deseen dominar el entorno.

---

## 1. Arquitectura de Seguridad y Sesion

La seguridad es el pilar central. Aquí explicamos cómo se mantiene la sesión protegida y persistente.

### Flujo de Autenticacion (Silent Refresh)
No solo usamos JWT, sino que implementamos un flujo de **Silent Refresh** para mejorar la UX y la seguridad.
*   **[LogiFrontend/src/api/axios.ts](LogiFrontend/src/api/axios.ts)**: El interceptor de respuesta detecta errores 401 (autorización fallida) e intenta automáticamente renovar el token llamando a `silentRefresh` en el store. Si tiene éxito, reintenta la petición original sin que el usuario lo note.
*   **[LogiBackend/src/StarterLogin.Infrastructure/Security/JwtTokenGenerator.cs](LogiBackend/src/StarterLogin.Infrastructure/Security/JwtTokenGenerator.cs)**: Responsable de la creación de los Claims. Es vital observar cómo se inyectan los roles y la fecha de nacimiento para las validaciones posteriores.

---

## 2. El Pipeline de MediatR y el Manejo de Errores

El backend no procesa peticiones de forma lineal; usa un pipeline de "comportamientos" (Behaviors).

### La Cadena de Responsabilidad
1.  **Validacion**: Antes de llegar al Handler, se ejecutan las reglas de negocio.
2.  **Manejo Global de Excepciones**:
    *   **[LogiBackend/src/StarterLogin.Api/Middleware/ExceptionHandlingMiddleware.cs](LogiBackend/src/StarterLogin.Api/Middleware/ExceptionHandlingMiddleware.cs)**: Captura cualquier error en la cadena. Si es una excepción de negocio (`AppException`), la traduce a un formato `ProblemDetails` con un código 400. Si es un error desconocido, devuelve un 500 y oculta detalles sensibles.
    *   **Consumo en Frontend**: El interceptor en `axios.ts` extrae el campo `detail` de este JSON y lo convierte en un `friendlyMessage` que la UI muestra al usuario.

---

## 3. Persistencia y Modelado de Datos Avanzado

Usamos EF Core con PostgreSQL aplicando patrones de optimización.

### Herencia de Contenido (TPH)
Para evitar múltiples tablas similares, usamos **Table Per Hierarchy**.
*   **[LogiBackend/src/StarterLogin.Infrastructure/Persistence/ApplicationDbContext.cs](LogiBackend/src/StarterLogin.Infrastructure/Persistence/ApplicationDbContext.cs)**: Observa cómo las clases `Movie`, `Series` y `Documentary` comparten la tabla `MediaContents`.
*   **Configuraciones**: En `Configurations/MediaContentConfiguration.cs` se definen los índices y el discriminador que permite a la DB distinguir entre tipos de contenido.

### Ciclo de Vida (Dependency Injection)
*   **[LogiBackend/src/StarterLogin.Infrastructure/DependencyInjection.cs](LogiBackend/src/StarterLogin.Infrastructure/DependencyInjection.cs)**: Aquí se define la vida de los objetos (`AddScoped`, `AddTransient`). El `IUnitOfWork` es Scoped para asegurar que todos los repositorios compartan la misma transacción durante una petición HTTP.

---

## 4. Reactividad y Estado en el Frontend

El frontend no solo muestra datos; gestiona un estado complejo y reactivo.

### El Corazon Selectivo (Pinia)
*   **[LogiFrontend/src/stores/auth.ts](LogiFrontend/src/stores/auth.ts)**: Gestiona la persistencia selectiva. Observa cómo el store actúa como el único punto de verdad para saber si hay un usuario logueado.
*   **Sincronizacion**: El interceptor de Axios y el store de Pinia están íntimamente ligados; uno gestiona la red y el otro el estado en memoria.

---

## 5. Archivos Maestros para Estudio Detallado

| Archivo | Proposito en el Estudio |
| :--- | :--- |
| `Program.cs` | Orquestación total del despliegue del servidor (.NET). |
| `App.vue` | Punto de entrada del layout y carga de estilos globales. |
| `DbInitializer.cs` | Entender la conformación de los datos iniciales de prueba. |
| `RoleConfiguration.cs` | Cómo se estructuran los permisos a nivel de base de datos. |

---

## Conclusion Educativa

Este entorno ha sido diseñado para que el código sea **Auto-Documentado**. Al leer el nombre de un Command (`RegisterUserCommand`), ya sabes qué hace, dónde está su validación y quién lo procesa.

**Consejo para el estudio**: No intentes entender todo a la vez. Elige un flujo (ej: "Ver una Película") y sigue el rastro desde el componente Vue -> Service -> Controller -> Handler -> Repository.
