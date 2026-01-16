# 🚀 Enterprise Authentication System 2026

¡Bienvenido al sistema de autenticación de grado empresarial definitivo! Este proyecto combina el poder de **.NET 8 (Clean Architecture)** con la elegancia de **Vue 3 (Vite + TypeScript)** para ofrecer una experiencia segura, escalable y visualmente impresionante.

## 🛠 Stack Tecnológico

- **Backend**: .NET 8, EF Core, MediatR (CQRS), JWT, BCrypt, SQL Server.
- **Frontend**: Vue 3, Pinia (State Management), Axios (Interceptors), Bootstrap 5, Glassmorphism UI.
- **Infraestructura**: Docker, Docker Compose, Nginx.

---

## 🏗 Arquitectura

El sistema sigue los principios de **Clean Architecture** y **DDD** (Domain-Driven Design):
1. **Domain**: Corazón del negocio (Entidades, Value Objects).
2. **Application**: Casos de uso y lógica de aplicación (CQRS).
3. **Infrastructure**: Implementaciones externas (Base de datos, Seguridad).
4. **Api**: Exposición de servicios mediante REST.

---

## 🚀 Cómo Empezar (Setup Rápido)

### Opción A: Con Docker (Recomendado 🐳)
Solo necesitas tener instalado Docker y Docker Compose. Ejecuta el siguiente comando en la raíz del proyecto:

```bash
docker-compose up --build
```

- **Frontend**: [http://localhost:8080](http://localhost:8080)
- **Backend API**: [http://localhost:5170](http://localhost:5170)
- **Swagger**: [http://localhost:5170/swagger](http://localhost:5170/swagger)

### Opción B: Ejecución Local (Desarrollo)

#### 1. Requisitos
- .NET 8 SDK
- Node.js 22+ (o usa el `node-env` incluido en `LogiFrontend`)
- SQL Server LocalDB o similar.

#### 2. Backend
```bash
cd LogiBackend
dotnet run --project src/StarterLogin.Api
```

#### 3. Frontend
```bash
cd LogiFrontend
# Si tienes node instalado:
npm install && npm run dev
# Si NO tienes node instalado:
export PATH=$PWD/node-env/bin:$PATH
npm install && npm run dev
```

---

## 🎨 Características de Élite
- **Diseño 2026**: Estética Glassmorphism con soporte nativo para **Dark Mode**.
- **Seguridad**: Hasheo de contraseñas con BCrypt y autenticación stateless por JWT.
- **UX Premium**: Sistema de notificaciones Toast y transiciones fluidas.
- **Seeding Automático**: Al iniciar por primera vez, el sistema crea roles y un usuario administrador por defecto.

---

## 📄 Documentación y Decisiones
Para un detalle exhaustivo de las decisiones técnicas y el progreso del proyecto, consulta:
- [Allcheck.md](./Allcheck.md): Bitácora de decisiones de diseño y cumplimiento de fases.
- [Walkthrough.md](./docs/walkthrough.md): Resumen de hitos alcanzados.

---
Creado con ❤️ para estándares de alta ingeniería por **Mylisuthy**.
