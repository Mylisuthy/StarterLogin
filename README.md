# StarterLogin: Enterprise Media Streaming Platform

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512bd4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Vue 3](https://img.shields.io/badge/Vue-3.0-42b883?logo=vuedotjs)](https://vuejs.org/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-latest-336791?logo=postgresql)](https://www.postgresql.org/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean-blue)](#)

StarterLogin es una plataforma de streaming multimedia de alto rendimiento diseñada bajo los estándares de arquitectura limpia (Clean Architecture), diseño guiado por el dominio (Domain-Driven Design) y segregación de responsabilidad en consultas y comandos (CQRS) mediante la librería MediatR. Esta solución proporciona una base robusta y altamente escalable para gestionar volúmenes masivos de usuarios y contenido multimedia de manera eficiente y segura.

---

## Especificaciones Tecnicas y Arquitectura

La plataforma utiliza un stack tecnológico de vanguardia para garantizar el máximo rendimiento y la estabilidad a largo plazo.

| Capa | Tecnologias | Proposito y Justificacion |
| :--- | :--- | :--- |
| **API Layer** | .NET 9.0 Web API | Implementación de controladores delgados que delegan la lógica de negocio a la capa de aplicación. |
| **Orquestacion** | MediatR (CQRS) | Implementación de comandos y consultas desacoplados, facilitando el mantenimiento y el escalado independiente de las operaciones de lectura y escritura. |
| **Persistencia** | PostgreSQL (EF Core) | Uso de un motor relacional de grado empresarial con soporte avanzado para consultas complejas e indexación eficiente. |
| **Capa de Dominio** | Logic Pura (POCO) | Entidades y lógica de negocio totalmente desacopladas de cualquier infraestructura o framework externo. |
| **Capa de Aplicacion** | Handlers de MediatR | Orquestación de casos de uso y transformación de datos entre entidades de dominio y DTOs de respuesta. |
| **Frontend Core** | Vue 3 + Vite | Framework reactivo de última generación que garantiza una experiencia de usuario fluida y tiempos de carga mínimos. |
| **Gestion de Estado** | Pinia | Implementación de almacenes (stores) globales para gestionar la autenticación y el flujo de datos de forma centralizada. |

---

## Arquitectura del Sistema y Flujo de Datos

### Ciclo de Vida de una Peticion

La arquitectura está diseñada para que cada solicitud pase por un proceso estandarizado que garantiza la integridad y el rendimiento antes de interactuar con la persistencia.

```mermaid
sequenceDiagram
    autonumber
    participant Client as Cliente (Vue 3)
    participant API as API Controller
    participant Pipe as MediatR Pipeline
    participant Handler as Logic Handler
    participant DB as Persistence (PostgreSQL)

    Client->>API: Solicitud HTTP con JWT
    API->>Pipe: Notificacion de Request (Command/Query)
    Note over Pipe: El pipeline permite aplicar decoradores de validacion, logging y auditoria.
    Pipe->>Handler: Ejecución de la lógica de negocio
    Handler->>DB: Interacción con el Repositorio (Unit of Work)
    DB-->>Handler: Resultados de la base de datos
    Handler-->>Pipe: Retorno de DTO optimizado
    Pipe-->>API: Resultado procesado por el orquestador
    API-->>Client: Respuesta JSON estandarizada
```

### Infraestructura de Red y Escalabilidad Fisica

El sistema contempla una distribución que permite el escalado horizontal y la descarga de tráfico pesado a servicios especializados.

```mermaid
graph TD
    User[Usuario Final] --> DNS[Gestion de DNS]
    DNS --> CDN[Cloudinary CDN - Streaming de Video/Assets]
    DNS --> LB[Balanceador de Carga]
    
    subgraph "Cluster de Aplicacion"
        LB --> API1[Nodo API Principal]
        LB --> API2[Nodo API Secundario]
    end

    subgraph "Capa de Cache y Persistencia"
        API1 & API2 --> Cache[L1: Memory Cache / L2: Redis]
        Cache --- DB_M[PostgreSQL Master - Escritura]
        DB_M --- DB_R1[PostgreSQL Replica - Lectura]
    end
```

### Modelo de Datos (Relacional)

```mermaid
erDiagram
    %% --- 1. USER & ROLES CLUSTER (Top/Left isolated) ---
    ROLES ||--o{ USER_ROLES : "assigned_to"
    USERS ||--o{ USER_ROLES : "has_roles"

    ROLES {
        uuid Id PK
        string Name
        string Description
        datetime CreatedAt
        datetime UpdatedAt
    }

    USER_ROLES {
        uuid UserId FK, PK
        uuid RoleId FK, PK
    }

    USERS {
        uuid Id PK
        string UserName
        string Email
        string PasswordHash
        boolean IsActive
        datetime BirthDate
        string Sex
        datetime CreatedAt
        datetime UpdatedAt
    }

    %% --- 2. MEDIA CLUSTER (Right side) ---
    GENRES ||--o{ MEDIA_CONTENTS : "categorizes"
    
    MEDIA_CONTENTS ||--o{ SEASONS : "has_seasons"
    SEASONS ||--o{ EPISODES : "has_episodes"

    GENRES {
        uuid Id PK
        string Name
        string Description
        datetime CreatedAt
        datetime UpdatedAt
    }

    MEDIA_CONTENTS {
        uuid Id PK
        string ContentType "Discriminator"
        string Title
        string Description
        string ImageUrl
        string VideoUrl
        interval Duration
        datetime ReleaseDate
        string Rating
        uuid GenreId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    SEASONS {
        uuid Id PK
        int Number
        string Title
        uuid SeriesId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    EPISODES {
        uuid Id PK
        int Number
        string Title
        string Description
        string VideoUrl
        interval Duration
        uuid SeasonId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    %% --- 3. THE BRIDGE (Pivot Tables) ---
    
    %% Bridge 1: Favorites
    USERS ||--o{ USER_FAVORITES : "saves"
    MEDIA_CONTENTS ||--o{ USER_FAVORITES : "favorited_by"

    USER_FAVORITES {
        uuid Id PK
        uuid UserId FK
        uuid MediaContentId FK
        datetime CreatedAt
        datetime UpdatedAt
    }

    %% Bridge 2: History
    USERS ||--o{ USER_MEDIA_HISTORY : "watches"
    MEDIA_CONTENTS ||--o{ USER_MEDIA_HISTORY : "watched_by"

    USER_MEDIA_HISTORY {
        uuid Id PK
        uuid UserId FK
        uuid MediaContentId FK
        interval WatchedTime
        boolean IsFinished
        datetime CreatedAt
        datetime UpdatedAt
    }
```

### Jerarquia de Dominio (Diagrama de Clases)

```mermaid
classDiagram
    direction LR
    
    %% Base Entity
    class BaseEntity {
        <<abstract>>
        +Guid Id
        +DateTime CreatedAt
        +DateTime? UpdatedAt
        #MarkAsUpdated()
    }

    %% Value Objects
    class Email {
        <<ValueObject>>
        +string Value
        -Email()
        +Create(string email)
    }

    class PasswordHash {
        <<ValueObject>>
        +string Value
        -PasswordHash()
        +Create(string hash)
    }

    %% User Aggregate
    class User {
        +string UserName
        +Email Email
        +PasswordHash PasswordHash
        +bool IsActive
        +DateTime? BirthDate
        +string? Sex
        +IReadOnlyCollection~Role~ Roles
        +Create(string userName, Email email, PasswordHash passwordHash)$
        +UpdateProfile(DateTime? birthDate, string? sex)
        +AddRole(Role role)
        +RemoveRole(Role role)
        +Deactivate()
        +Activate()
        +UpdatePassword(PasswordHash newPasswordHash)
    }

    class Role {
        +string Name
        +string Description
        +Create(string name, string description)$
        +UpdateDescription(string description)
    }

    %% Media Aggregate
    class MediaContent {
        <<abstract>>
        +string Title
        +string Description
        +string? ImageUrl
        +string? VideoUrl
        +TimeSpan? Duration
        +DateTime? ReleaseDate
        +string? Rating
        +Guid GenreId
        +Genre Genre
        #MediaContent(...)
    }

    class Genre {
        +string Name
        +string? Description
        +Create(string name, string? description)$
        +Update(string name, string? description)
    }

    class Movie {
        +Create(...)$
        +Update(...)
    }

    class Series {
        +IReadOnlyCollection~Season~ Seasons
        +Create(...)$
        +Update(...)
        +AddSeason(Season season)
    }

    class Documentary {
        +Create(...)$
        +Update(...)
    }

    class Season {
        +int Number
        +string? Title
        +Guid SeriesId
        +IReadOnlyCollection~Episode~ Episodes
        +Create(int number, Guid seriesId, string? title)$
        +Update(int number, string? title)
        +AddEpisode(Episode episode)
    }

    class Episode {
        +int Number
        +string Title
        +string Description
        +string? VideoUrl
        +TimeSpan? Duration
        +Guid SeasonId
        +Create(...)$
        +Update(...)
    }

    %% Pivot / Connection Entities
    class UserFavorite {
        +Guid UserId
        +Guid MediaContentId
        +UserFavorite(Guid userId, Guid mediaContentId)
    }

    class UserMediaHistory {
        +Guid UserId
        +Guid MediaContentId
        +TimeSpan WatchedTime
        +bool IsFinished
        +UserMediaHistory(Guid userId, Guid mediaContentId, ...)
        +UpdateProgress(TimeSpan watchedTime, bool isFinished)
    }

    %% Inheritance Relationships
    BaseEntity <|-- User
    BaseEntity <|-- Role
    BaseEntity <|-- Genre
    BaseEntity <|-- MediaContent
    BaseEntity <|-- Season
    BaseEntity <|-- Episode
    BaseEntity <|-- UserFavorite
    BaseEntity <|-- UserMediaHistory

    MediaContent <|-- Movie
    MediaContent <|-- Series
    MediaContent <|-- Documentary

    %% Composition & Aggregation
    User *-- Email : Owns
    User *-- PasswordHash : Owns
    
    User "1" o-- "many" Role : UserRoles
    
    MediaContent --> Genre : Has_One
    
    Series *-- Season : Contains_Many
    Season *-- Episode : Contains_Many

    %% Usage / Cross-Aggregate References
    UserFavorite --> User : References
    UserFavorite --> MediaContent : References
    
    UserMediaHistory --> User : References
    UserMediaHistory --> MediaContent : References
```

---

## Estructura de Directorios y Responsabilidades

Una separación clara de las capas asegura que cada componente tenga una única responsabilidad bien definida.

### Backend (LogiBackend)

*   **StarterLogin.Domain**: Contiene las definiciones de entidades, interfaces de repositorio y lógica de negocio pura. Es la capa de mayor nivel y no debe tener dependencias de otras capas.
*   **StarterLogin.Application**: Define los flujos de trabajo de la aplicación, DTOs, comandos, consultas y sus respectivos manejadores. Es donde se implementa la lógica de orquestación de MediatR.
*   **StarterLogin.Infrastructure**: Implementa las interfaces definidas en el dominio. Aquí se encuentran las configuraciones de Entity Framework, la implementación de repositorios, los servicios de seguridad (JWT) y la integración con proveedores externos como Cloudinary.
*   **StarterLogin.Api**: Contiene los puntos de entrada HTTP, la configuración de la inyección de dependencias general y la definición del pipeline de ejecución de ASP.NET Core.

### Frontend (LogiFrontend)

*   **src/api**: Centraliza la configuración de Axios, incluyendo interceptores para la gestión automática de tokens de autenticación en las cabeceras.
*   **src/services**: Proporciona una capa de abstracción para las llamadas a la API, agrupadas por dominios funcionales (autenticación, multimedia).
*   **src/stores**: Gestiona el estado reactivo global de la aplicación utilizando Pinia.
*   **src/views**: Agrupa los componentes de nivel de página que se asocian a las rutas del sistema.
*   **src/components**: Contiene elementos de interfaz de usuario reutilizables siguiendo un enfoque de diseño modular.

---

## Configuracion del Sistema

La correcta configuración de las variables de entorno y los archivos de parámetros es fundamental para el funcionamiento del ecosistema. A continuación se detallan las configuraciones requeridas para ambos componentes.

### Configuracion del Backend (appsettings.json)

El archivo `appsettings.json` en `LogiBackend/src/StarterLogin.Api` gestiona la conectividad y la seguridad del servidor.

| Seccion | Variable | Descripcion | Ejemplo / Valor Recomendado |
| :--- | :--- | :--- | :--- |
| **ConnectionStrings** | `DefaultConnection` | Cadena de conexión para PostgreSQL. | `Host=localhost;Port=5902;Database=StarterLoginDb;Username=postgres;Password=admin` |
| **JwtSettings** | `Secret` | Clave de firma para los tokens JWT. Debe ser una cadena larga y aleatoria. | `TU_CLAVE_SECRETA_DE_AL_MENOS_32_CARACTERES` |
| **JwtSettings** | `Issuer` | El emisor del token (usualmente el nombre de la API). | `StarterLoginApi` |
| **JwtSettings** | `Audience` | El receptor del token (usualmente el nombre del frontend). | `StarterLoginFront` |
| **JwtSettings** | `ExpirationInMinutes` | Tiempo de vida del Access Token. | `1440` (24 horas) |
| **CloudinarySettings** | `CloudName` | Nombre de la nube en el dashboard de Cloudinary. | `nombre_de_tu_nube` |
| **CloudinarySettings** | `ApiKey` | Clave de API proporcionada por Cloudinary. | `tu_api_key_publica` |
| **CloudinarySettings** | `ApiSecret` | Secreto de API para operaciones firmadas. | `tu_api_secret_privado` |

### Configuracion del Frontend (.env)

El frontend utiliza variables de entorno cargadas por Vite. Cree un archivo `.env` en la raíz de `LogiFrontend` basado en el siguiente esquema:

| Variable | Descripcion | Valor por Defecto / Local |
| :--- | :--- | :--- |
| `VITE_API_URL` | URL base de los endpoints de la API backend. | `http://localhost:5901/api` |
| `VITE_APP_TITLE` | Título de la aplicación que se muestra en el navegador. | `StarterLogin` |

> [!NOTE]
> En entornos Docker, estas variables se inyectan automáticamente a través del archivo `docker-compose.yml`. Si realiza cambios manuales en el entorno local, asegúrese de reiniciar los servicios para que los cambios surtan efecto.

---

## Guia de Instalacion y Despliegue

### Despliegue con Docker

El uso de Docker garantiza la consistencia entre los entornos de desarrollo, pruebas y producción.

1.  Asegúrese de tener instalado Docker Desktop en su sistema.
2.  Desde la raíz del proyecto, ejecute el siguiente comando:
    ```bash
    docker-compose up --build
    ```
3.  Una vez finalizado, los servicios estarán disponibles en:
    - Frontend: http://localhost:5900
    - Backend Swagger: http://localhost:5901/swagger

### Despliegue en Entorno de Desarrollo Local

Si prefiere ejecutar los servicios de forma nativa para un desarrollo más dinámico:

1.  **Requisitos**: Instale el SDK de .NET 9.0, Node.js v18+ y una instancia de PostgreSQL accesible.
2.  **Configuracion de Base de Datos**: Actualice la cadena de conexión en el archivo `appsettings.json` o utilice las variables de entorno correspondientes.
3.  **Ejecucion del Backend**:
    ```bash
    cd LogiBackend/src/StarterLogin.Api
    dotnet run
    ```
4.  **Ejecucion del Frontend**:
    ```bash
    cd LogiFrontend
    npm install
    npm run dev
    ```

---

## Estrategias de Desarrollo y Mejores Practicas

### Implementacion de Nuevas Funcionalidades

Para mantener la integridad arquitectónica, siga estos pasos al extender el sistema:

1.  **Definicion en el Dominio**: Si la funcionalidad requiere nuevas entidades o cambios en las existentes, empiece por la capa de Dominio.
2.  **Definicion de Contratos**: Crea las interfaces necesarias en el Dominio para que la Infraestructura las implemente después.
3.  **Casos de Uso**: Crea los Comandos o Consultas en la capa de Aplicación junto con sus manejadores.
4.  **Exposicion de API**: Añada o actualice los controladores en la capa Api para permitir el acceso a través de MediatR.
5.  **Integracion Frontend**: Implemente el servicio correspondiente en Vue y actualice los almacenes de Pinia según sea necesario.

### Recomendaciones Expertas de Rendimiento y Seguridad

> [!IMPORTANT]
> **Gestion de Caché**: Utilice estrategias de caché SlidingExpiration para datos de alta frecuencia de consulta pero baja volatilidad. Esto reduce drásticamente la carga sobre la base de datos y mejora los tiempos de respuesta.

> [!IMPORTANT]
> **Validacion en Pipeline**: Implemente validaciones automáticas utilizando FluentValidation dentro del pipeline de MediatR. Esto asegura que ningún comando con datos inválidos llegue siquiera a la capa de infraestructura.

> [!IMPORTANT]
> **Seguridad JWT**: No almacene información sensible en los claims del token. El backend debe validar siempre la identidad del usuario contra la base de datos o el sistema de caché para operaciones críticas.

---

## Solucion de Problemas y Diagnostico

*   **Conflictos de Puerto**: Si los puertos 5900 o 5901 están ocupados, modifique el archivo `docker-compose.yml` y ajuste la configuración de CORS en el backend y la variable de entorno de la API en el frontend.
*   **Errores de Migracion**: En caso de errores al iniciar la base de datos, limpie los volúmenes de Docker mediante `docker-compose down -v` para forzar un re-seed de los datos iniciales.
*   **Conexion de Red en Docker**: Recuerde que dentro de los contenedores, la instancia de base de datos se identifica por el nombre del servicio definido en el archivo compose, no por "localhost".

---

## Licencia

Este proyecto se distribuye bajo la licencia MIT.
