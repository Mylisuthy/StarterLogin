### 📝 Epic User Story: Sistema de Autenticación Seguro

> **"Como** Administrador del sistema,
> **Quiero** una plataforma segura y escalable con roles definidos (Admin/User),
> **Para** gestionar el acceso a recursos protegidos mediante autenticación moderna."

#### Criterios de Aceptación Técnicos (Definition of Done)

1. **Arquitectura:** Backend y Frontend totalmente desacoplados.
2. **Seguridad:** IDs inpredecibles (GUIDs), Passwords hasheados (BCrypt), JWT con expiración.
3. **Código:** Tipado estricto (C# Nullable / TypeScript), Clean Code, Patrón Repositorio.
4. **UX:** Feedback visual de errores y carga en el Frontend (Bootstrap).

---

### 🗺️ Roadmap de Desarrollo (Paso a Paso)

Vamos a dividir esto en **6 Fases Tácticas**. No pasaremos a la siguiente hasta completar la anterior.

#### 🟢 Fase 1: El Núcleo del Dominio (Backend - DDD Puro)

*El corazón de la lógica, sin depender de bases de datos ni frameworks.*

1. **Base Entity Abstracta:** Crear la clase `BaseEntity` (con `Guid Id`, `CreatedAt`, `UpdatedAt`, `IsDeleted`) para herencia.
2. **Entidades Ricas:** Crear `User` y `Role` heredando de BaseEntity. Nada de sets públicos (encapsulamiento).
3. **Value Objects:** Definir objetos de valor para `Email` y `PasswordHash` (validación en el dominio).
4. **Interfaces de Repositorio:** Definir `IUserRepository` y `IUnitOfWork`.

#### 🔵 Fase 2: Infraestructura y Persistencia (EF Core Code First)

*Conectar el dominio con el mundo real (SQL Server).*

1. **DbContext:** Configurar `ApplicationDbContext` heredando de `IdentityDbContext` o custom.
2. **Entity Configuration:** Usar `IEntityTypeConfiguration` para definir límites de SQL (Fluent API) sin ensuciar las entidades.
3. **Inyección de Dependencias:** Configurar los servicios en el contenedor IoC.
4. **Migraciones:** Ejecutar la primera migración y crear el **Seeding** (datos semilla) para crear un Admin por defecto automáticamente.

#### 🟡 Fase 3: Lógica de Aplicación (CQRS + DTOs)

*El cerebro que coordina las peticiones.*

1. **DTOs (Data Transfer Objects):** Crear `LoginRequestDto`, `RegisterRequestDto`, `UserResponseDto`.
2. **Mappers:** Configurar el mapeo (manual o con Mapster/AutoMapper).
3. **Features (CQRS):** Implementar con MediatR:
* `RegisterUserCommand`
* `LoginUserQuery`


4. **Servicio de Token:** Implementar la lógica para generar y firmar el JWT.

#### 🟠 Fase 4: Exposición de la API (Controllers)

*La puerta de entrada.*

1. **AuthController:** Crear endpoints `POST /login` y `POST /register`.
2. **Manejo de Errores Global:** Middleware para capturar excepciones y devolver respuestas estandarizadas (RFC 7807).
3. **Swagger:** Configurar OpenApi para probar la seguridad (botón de candado "Authorize").

#### 🟣 Fase 5: Arquitectura Frontend (Vue 3 + TS)

*Los cimientos del cliente.*

1. **Setup del Proyecto:** Crear proyecto con Vite + Vue 3 + TypeScript.
2. **Estructura de Carpetas:** Organizar por módulos (`/auth`, `/dashboard`).
3. **Instalación UI:** Configurar **Bootstrap 5** (Nota: Usaremos la integración nativa o `bootstrap-vue-next` ya que la librería clásica no soporta bien Vue 3).
4. **Gestión de Estado:** Configurar **Pinia** para guardar el usuario y el token.
5. **Cliente HTTP:** Configurar **Axios** con interceptores (para inyectar el token en cada petición automáticamente).

#### 🔴 Fase 6: Integración y UI (La conexión)

*Donde todo cobra vida.*

1. **Vistas de Auth:** Maquetar Login y Registro con validaciones de formulario.
2. **Lógica de Login:** Conectar el formulario con el Store de Pinia y la API.
3. **Protección de Rutas:** Crear "Guards" en Vue Router para que si no hay token, te mande al login.
4. **Manejo de Roles:** Ocultar botones si el usuario no es Admin.