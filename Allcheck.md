# Allcheck - Enterprise Authentication System

## 🎯 Resumen del Proyecto
Este documento actúa como la fuente de verdad sobre el cumplimiento de los requerimientos técnicos y las decisiones de diseño tomadas durante el desarrollo del sistema de autenticación.

## 🏛️ Análisis de Arquitectura (Punto de Vista Senior)

### 1. Clean Architecture & DDD
- **Dominio al Centro**: La lógica de negocio reside en `StarterLogin.Domain`, libre de dependencias externas.
- **Inversión de Dependencias**: La infraestructura depende de las interfaces definidas en la capa de aplicación/dominio.
- **Entidades Ricas**: Evitamos el "Modelo de Dominio Anémico". Las entidades validan su propio estado.

### 2. Seguridad de Grado Empresarial
- **GUIDs vs Ints**: Usamos `Guid` para IDs para prevenir ataques de enumeración y facilitar la sincronización en sistemas distribuidos.
- **Encapsulamiento**: Propiedades con `protected set` para asegurar que el estado solo cambie a través de métodos de negocio.

---

## ✅ Cumplimiento de Tareas

| Phase | Tarea | Estado | Decisión de Diseño / Justificación |
| :--- | :--- | :--- | :--- |
| **1-Nucleo** | `BaseEntity` | ✅ Completado | Estandarización de auditoría (CreatedAt, UpdatedAt). |
| **1-Nucleo** | `User` & `Role` | ✅ Completado | Encapsulamiento total y lógica descentralizada. |
| **1-Nucleo** | Value Objects | ✅ Completado | `Email` y `Password` no son strings, son objetos con reglas. |
| **2-Infra** | EF Core / Mapping| ✅ Completado | Fluent API para blindar el esquema SQL sin ensuciar el Dominio. |
| **2-Infra** | Seeding / Repos | ✅ Completado | Repositorios genéricos y seeding automático del usuario admin. |
| **3-App** | CQRS / JWT | ✅ Completado | MediatR para desacoplar comandos de controladores. JWT para stateless auth. |
| **4-Api** | Controllers | ✅ Completado | RFC 7807 (Problem Details) para errores estandarizados. |
| **5-Front** | Vue 3 + TS Setup| ✅ Completado | Estructura modular y tipado estricto para escalabilidad. |
| **6-Front** | UI Enterprise | ✅ Completado | Diseño premium con Glassmorphism y micro-animaciones. |
| **7-Elite** | UX & Profile | ✅ Completado | Perfil de usuario, Toasts, Dark Mode y estándar 2026. |
| **8-Infra** | Docker & Docs | ✅ Completado | Orquestación completa con Docker Compose para despliegue inmutable. |

---

## 🛠️ Bitácora de Decisiones

### [2026-01-16] Inicialización del Proyecto
- **Decisión**: Estructurar el backend en 4 capas (Api, Application, Domain, Infrastructure).
- **Razón**: Es el estándar de oro para aplicaciones escalables, permitiendo cambiar la base de datos o el framework de UI sin tocar la lógica central.

### [2026-01-16] Arquitectura Frontend Enterprise
- **Decisión**: Usar Pinia para el estado y Axios con interceptores.
- **Razón**: Pinia es el estándar moderno para Vue 3. Los interceptores permiten un manejo centralizado de la seguridad (JWT) y errores (401 Redirect).

### [2026-01-16] UI de Grado Empresarial
- **Decisión**: Implementar una estética de "Glassmorphism" con gradientes profundos.
- **Razón**: Transmite una sensación de modernidad y robustez tecnológica ("Senior Look"), diferenciándose de aplicaciones genéricas.

### [2026-01-16] Estándar Elite 2026 (Dark Mode & Toasts)
- **Decisión**: Soporte nativo para Dark Mode y sistema de notificaciones no intrusivas.
- **Razón**: El estándar 2026 exige interfaces que se adapten al entorno del usuario y proporcionen feedback inmediato de alta calidad (Elite UX).

### [2026-01-16] Dockerización e Infraestructura
- **Decisión**: Usar Docker Compose con SQL Server y Nginx.
- **Razón**: Asegura que el entorno de desarrollo sea idéntico al de producción, eliminando el clásico "en mi máquina funciona".
