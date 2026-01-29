# LogiBackend: Arquitectura y Guia de Servidor

Este directorio contiene el núcleo de servicios y lógica de negocio de la plataforma StarterLogin. El servidor está construido sobre .NET 9.0 y sigue los principios de Clean Architecture y CQRS.

## Especificaciones de Ejecucion

Para la interoperabilidad con el frontend y otros servicios, el backend se expone bajo los siguientes parámetros:

*   **Puerto del Host**: 5901
*   **Puerto del Contenedor**: 80
*   **Endpoint de API**: http://localhost:5901/api
*   **Documentacion Swagger**: http://localhost:5901/swagger

## Arquitectura del Proyecto (Clean Architecture)

El backend está organizado en cuatro proyectos principales que garantizan el desacoplamiento y la mantenibilidad.

### 1. StarterLogin.Domain
Es la capa más interna y contiene las reglas de negocio esenciales.
*   **Entidades**: Definición de objetos de dominio (User, MediaContent, Genre).
*   **Excepciones**: Definición de errores específicos de negocio para evitar fugas de abstracción de infraestructura.
*   **Interfaces**: Definición de contratos para repositorios y servicios externos que serán implementados por la infraestructura.

### 2. StarterLogin.Application
Implementa los casos de uso de la aplicación utilizando el patrón CQRS mediante MediatR.
*   **Commands & Queries**: Separación de operaciones que modifican el estado de aquellas que solo recuperan información.
*   **Handlers**: Contienen la lógica de ejecución para cada comando o consulta.
*   **DTOs**: Objetos de transferencia de datos para asegurar que las entidades internas no se expongan directamente a la API.
*   **Pipeline Behaviors**: Funcionalidades transversales como validación automática y logging de peticiones.

### 3. StarterLogin.Infrastructure
Proporciona las implementaciones tecnológicas para los contratos del dominio.
*   **Persistencia**: Configuración de Entity Framework Core para PostgreSQL, incluyendo migraciones y mapeos de datos.
*   **Seguridad**: Implementación de la generación y validación de tokens JWT y servicios de hashing de contraseñas.
*   **Servicios Externos**: Integración con Cloudinary para la gestión de activos multimedia.

### 4. StarterLogin.Api
Capa de interfaz web que gestiona las peticiones externas.
*   **Controladores**: Puntos de entrada HTTP que delegan el procesamiento a MediatR.
*   **Middleware**: Manejo global de excepciones y auditoría de peticiones.
*   **Configuracion**: Inyección de dependencias y orquestación del inicio del servidor.

---

## Gestion de Base de Datos y Migraciones (Expert Mode)

El manejo correcto de Entity Framework Core es vital para la integridad del sistema.

### Comandos de Migracion Frecuentes
Todos los comandos de migraciones deben ejecutarse teniendo como referencia el proyecto de Infraestructura y el de inicio (Api).

*   **Añadir Migracion**:
    ```bash
    dotnet ef migrations add <NombreMigracion> --project src/StarterLogin.Infrastructure --startup-project src/StarterLogin.Api
    ```
*   **Remover Ultima Migracion (No aplicada)**:
    ```bash
    dotnet ef migrations remove --project src/StarterLogin.Infrastructure --startup-project src/StarterLogin.Api
    ```
*   **Generar Script SQL de Migraciones**: Útil para auditoría o despliegue manual.
    ```bash
    dotnet ef migrations script --project src/StarterLogin.Infrastructure --startup-project src/StarterLogin.Api
    ```
*   **Actualizar Base de Datos**:
    ```bash
    dotnet ef database update --project src/StarterLogin.Infrastructure --startup-project src/StarterLogin.Api
    ```

### Tips de Persistencia
*   **Database Reset**: Si necesitas limpiar la base de datos por completo y volver a ejecutar el Seeding, borra todas las tablas o ejecuta `docker-compose down -v`.
*   **Logs SQL**: Para ver las consultas generadas por EF Core, asegúrate de que el nivel de log en `appsettings.json` para `Microsoft.EntityFrameworkCore.Database.Command` esté en `Information`.

---

## Diagnostico y Resolucion de Problemas (Troubleshooting)

### Errores de Certificado / HTTPS
En entornos locales de Windows, si obtienes errores de confianza de certificado:
```bash
dotnet dev-certs https --trust
```

### Problemas de JWT y Autorización
Si recibes errores 401 constantes:
1.  **Verifica la Expiración**: Revisa el campo `exp` del token en [jwt.io](https://jwt.io).
2.  **Secret Key**: Asegúrate de que la clave en `appsettings.json` tenga al menos 32 caracteres.
3.  **Audience/Issuer**: Verifica que coincidan exactamente entre el generador y el validador.

### Limpieza Completa de Proyecto
A veces, los archivos temporales de compilación causan errores "fantasma".
```bash
# Ejecutar en la raíz del proyecto para limpiar binarios
find . -type d -name "bin" -prune -exec rm -rf {} \;
find . -type d -name "obj" -prune -exec rm -rf {} \;
```

---

## Comandos Utiles por Casos de Uso

### Desarrollo Activo
*   **Auto-recarga**: `dotnet watch run --project src/StarterLogin.Api` (Actualiza el servidor al guardar cambios).
*   **Depuracion de Logs**: `dotnet run --project src/StarterLogin.Api --verbosity detailed`.

### Docker & Contenedores
*   **Logs en tiempo real**: `docker logs -f starterlogin-backend-1`.
*   **Acceder a la Shell del Contenedor**: `docker exec -it starterlogin-backend-1 /bin/bash`.
*   **Ver Uso de Recursos**: `docker stats`.

### Seguridad
*   **Generar nueva clave secreta segura (C# Interactive)**:
    ```csharp
    var key = new byte[32]; 
    System.Security.Cryptography.RandomNumberGenerator.Fill(key); 
    Console.WriteLine(Convert.ToBase64String(key));
    ```

---

## Seguridad y Cumplimiento

La plataforma utiliza JWT para la autorización. Cada petición a endpoints protegidos debe incluir el header `Authorization: Bearer <token>`. La validez del token y los roles del usuario son verificados en cada ciclo de vida de la petición por el middleware de autenticación de ASP.NET Core.
