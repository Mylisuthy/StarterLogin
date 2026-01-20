# StarterLogin - Sistema de Autenticación Empresarial

Este proyecto es una solución robusta de autenticación construida con tecnologías de vanguardia, diseñada bajo principios de **Clean Architecture** y modularidad. Proporciona una base sólida para la gestión de usuarios, seguridad JWT y una interfaz de usuario moderna.

## 🚀 Tecnologías Core

- **Backend**: .NET 9.0 (C#)
- **Base de Datos**: PostgreSQL
- **Arquitectura**: Clean Architecture
- **Patrón de Mensajería**: MediatR (CQRS Lite)
- **Frontend**: Vue 3 + Vite + TypeScript
- **Gestión de Estado**: Pinia
- **Contenedores**: Docker & Docker Compose

---

## 📊 Arquitectura y Flujo (UML)

### Diagrama de Secuencia (Login)
Este diagrama describe la interacción entre componentes durante el proceso de autenticación.

```mermaid
sequenceDiagram
    participant U as Usuario
    participant F as Frontend (Vue/Pinia)
    participant A as API (.NET Controller)
    participant B as Application (MediatR Handler)
    participant I as Infrastructure (DB/Security)
    participant D as Database (PostgreSQL)

    U->>F: Ingresa credenciales (Click Login)
    F->>A: POST /api/auth/login
    A->>B: Envia LoginUserQuery
    B->>I: Validar Usuario/Password
    I->>D: SELECT user FROM Users
    D-->>I: User Data
    I-->>B: Password Valid/Invalid
    B->>I: Generar JWT Token
    I-->>B: Token String
    B-->>A: AuthResponse (Token + UserData)
    A-->>F: 200 OK + AuthResponse
    F->>U: Redirige a Dashboard
```

### Arquitectura de Capas (Clean Architecture)
```mermaid
graph TD
    UI[Frontend / Clients] --> API[StarterLogin.Api]
    API --> APP[StarterLogin.Application]
    APP --> DOM[StarterLogin.Domain]
    INF[StarterLogin.Infrastructure] -.-> APP
    INF -.-> DOM
    subgraph "Core"
        APP
        DOM
    end
```

---

## 🗺️ Mapa del Proyecto

### 🟢 Backend (LogiBackend/src)

| Capapa | Responsabilidad |
| :--- | :--- |
| **StarterLogin.Domain** | Contiene las entidades de negocio (`User`), Value Objects y interfaces base. Es agnóstico a cualquier tecnología externa. |
| **StarterLogin.Application** | Orquesta la lógica de negocio mediante comandos (`Commands`) y consultas (`Queries`). Maneja casos de uso como Login y Registro. |
| **StarterLogin.Infrastructure** | Implementa la persistencia de datos con Entity Framework Core (PostgreSQL), seguridad (BCrypt, JWT) y otros servicios externos. |
| **StarterLogin.Api** | Capa de exposición. Define los controladores REST que sirven como puntos de entrada para el cliente. |

### 🔵 Frontend (LogiFrontend/src)

| Directorio | Responsabilidad |
| :--- | :--- |
| **`api/`** | Servicios de comunicación HTTP (Axios) configurados para interactuar con el backend. |
| **`stores/`** | Gestión del estado global (Autenticación, Notificaciones) mediante Pinia. |
| **`views/`** | Páginas principales de la aplicación (Login, Dashboard, Perfil). |
| **`components/`** | Elementos de UI reutilizables como la barra de navegación y contenedores de mensajes. |

---

## 🔄 Ciclo de Vida de una Petición (Ejemplo: Login)

Para entender cómo fluye la información a través del sistema, aquí se detalla el ciclo de vida de una solicitud de inicio de sesión:

1.  **Frontend (UI)**: El usuario introduce sus credenciales en `Login.vue`. Al hacer clic en "Entrar", se invoca la acción `login` en el `authStore`.
2.  **Frontend (API)**: El `authStore` envía una petición POST a `/api/auth/login` mediante Axios.
3.  **Backend (API)**: El `AuthController` recibe la solicitud y delega la ejecución al `Mediator` enviando un `LoginUserQuery`.
4.  **Backend (Application)**: El `LoginUserQueryHandler` toma el control.
    - Consulta al repositorio (`Infrastructure`) para encontrar al usuario en PostgreSQL.
    - Valida la contraseña usando el servicio de hashing.
    - Si es válido, solicita al generador de tokens un JWT firmado.
5.  **Backend (Infrastructure)**: El repositorio realiza la consulta SQL optimizada a la base de datos PostgreSQL.
6.  **Respuesta**: El `Handler` devuelve la información del usuario y el token al controlador, que responde con un `200 OK`.
7.  **Sincronización**: El Store de Vue guarda el token y redirige al usuario al **Dashboard**.

---

## 🛠️ Configuración y Ejecución

### Requisitos
- .NET 9.0 SDK
- PostgreSQL
- Node.js (v18+)

### Ejecución con Docker (Recomendado)
```bash
docker-compose up --build
```

### Ejecución Manual
1.  **Base de Datos**: Asegúrate de tener PostgreSQL corriendo y actualiza el `appsettings.json`.
2.  **Backend**:
    ```bash
    cd LogiBackend/src/StarterLogin.Api
    dotnet run
    ```
3.  **Frontend**:
    ```bash
    cd LogiFrontend
    npm install
    npm run dev
    ```

---

## 📄 Licencia
Este proyecto está bajo la licencia MIT.
