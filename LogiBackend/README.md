# 🛠️ Backend - Guía Técnica

Este servidor .NET 9.0 orquesta la seguridad y lógica de negocio.

### 🌐 Ejecución (Docker)
- **Puerto Host**: `5901`
- **Puerto Interno**: `80`
- **Endpoint Base**: `http://localhost:5901/api`
- **Swagger UI**: `http://localhost:5901/swagger` (Documentación Interactiva)

---

## 🏗️ Conceptos Clave

1.  **Clean Architecture**: El código se organiza en capas concéntricas. La regla de oro es que las dependencias siempre apuntan hacia adentro (hacia el **Dominio**).
2.  **CQRS (MediatR)**: Separamos las "Consultas" (Queries) de las "Acciones" (Commands). Esto evita que los controladores tengan lógica compleja.
3.  **Inyección de Dependencias**: Usamos interfaces (`IUnitOfWork`, `IJwtTokenGenerator`) para que las clases sean fáciles de probar y cambiar.

---

## 🗺️ Tour Guiado del Código

Sigue este camino para entender cómo funciona una petición:

### 1. La Puerta de Entrada: `StarterLogin.Api`
Mira en `Controllers/AuthController.cs`.
- **Qué hace**: Recibe el JSON del frontend, crea una "Query" o "Command" y se lo pasa a MediatR.
- **Concepto**: Los controladores aquí son "delgados"; solo sirven de puente.

### 2. El Cerebro: `StarterLogin.Application`
Mira en `Auth/Queries/Login/LoginUserQuery.cs`.
- **El Handler**: Es el encargado de realizar la tarea. Busca al usuario, valida su password y genera el token.
- **Flexibilidad**: Si quieres añadir una validación de "Usuario Bloqueado", este es el lugar.

### 3. La Base de Datos: `StarterLogin.Infrastructure`
Mira en `Persistence/ApplicationDbContext.cs`.
- **EF Core**: Aquí definimos las tablas de PostgreSQL.
- **Seguridad**: En `Security/JwtTokenGenerator.cs` verás cómo se firman los tokens que dan acceso al frontend.

### 4. El Corazón: `StarterLogin.Domain`
Mira en `Entities/User.cs`.
- **Pureza**: Aquí no hay librerías externas. Solo la definición de lo que es un Usuario en tu negocio.

---

## 🚀 Cómo modificarlo a tu antojo

- **¿Quieres otra Base de Datos?**: Solo cambia `DependencyInjection.cs` en Infraestructura para usar, por ejemplo, SQLite.
- **¿Nueva funcionalidad?**: 
  1. Crea la Entidad en **Domain**.
  2. Crea el Command/Query en **Application**.
  3. Expón el endpoint en el Controller de **Api**.

---

## 🛠️ Comandos Útiles (Backend)
- `dotnet build`: Compila el proyecto.
- `dotnet watch run`: Inicia el servidor con recarga automática.
- `dotnet ef migrations add <Nombre>`: Crea una nueva migración (ejecutar desde Infrastructure).
