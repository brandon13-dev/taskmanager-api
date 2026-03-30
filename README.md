# API de Gestión de Tareas (TaskManager) 🚀

Una Web API RESTful de nivel intermedio construida con **.NET**, diseñada para demostrar principios de arquitectura limpia, autenticación segura y gestión de bases de datos relacionales. 

Este proyecto fue desarrollado para exhibir habilidades sólidas en el backend, enfocándose específicamente en la seguridad del flujo de usuarios y la integración con Entity Framework Core.

## ✨ Características Principales

* **Autenticación y Autorización JWT**: Endpoints seguros protegidos mediante JSON Web Tokens.
* **Control de Acceso Basado en Roles (RBAC)**: Permisos estrictos y diferenciados para los roles de `User` (Usuario) y `Admin` (Administrador).
* **Cifrado de Contraseñas Seguro**: Implementación de Hashing con BCrypt para proteger las credenciales de los usuarios contra filtraciones.
* **Base de Datos Relacional**: Gestionada a través de Entity Framework Core utilizando el enfoque *Code-First* con SQLite.
* **Validación de Datos y DTOs**: Separación de los modelos internos de base de datos de las respuestas de la API para prevenir vulnerabilidades de *over-posting* y exponer únicamente la información necesaria.
* **Borrado en Cascada (Cascade Deletion)**: Reglas estrictas a nivel de base de datos para mantener la integridad referencial de la información.

## 🛠️ Stack Tecnológico

* **Framework**: .NET Web API (C#)
* **ORM**: Entity Framework Core
* **Base de Datos**: SQLite (Diseñado para ser fácilmente escalable a SQL Server o PostgreSQL)
* **Seguridad**: JWT Bearer Tokens, BCrypt.Net-Next
* **Documentación**: Swagger / OpenAPI

## 🚦 Primeros Pasos

### Requisitos Previos
* Tener instalado el [.NET SDK](https://dotnet.microsoft.com/download) en tu equipo.
* Una herramienta para probar la API (La interfaz de Swagger viene incluida por defecto, o puedes usar Postman).

### Instalación y Configuración

1. **Clonar el repositorio:**
   `git clone https://github.com/brandon13-dev/taskmanager-api.git`  
   `cd taskmanager-api`

2. **Aplicar las Migraciones de la Base de Datos:**
   El proyecto utiliza SQLite, por lo que no necesitas configurar un servidor de base de datos externo. Solo ejecuta las migraciones para generar el archivo `.db` local:
   `dotnet ef database update`

3. **Ejecutar la Aplicación:**
   `dotnet run`

4. **Acceder a la Interfaz de Swagger:**
   Abre tu navegador y dirígete a `http://localhost:<puerto>/swagger` para interactuar visualmente con la API.

## 📡 Endpoints de la API

### Autenticación (Auth)
| Método | Endpoint | Descripción | Acceso |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/Auth/register` | Registra un nuevo usuario | Público |
| `POST` | `/api/Auth/login` | Autentica al usuario y devuelve el JWT | Público |

### Tareas (Tasks)
| Método | Endpoint | Descripción | Acceso |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Task` | Obtiene la lista de tareas del usuario logueado | `User` |
| `POST` | `/api/Task` | Crea una nueva tarea | `User` |
| `PUT` | `/api/Task/{id}/status` | Actualiza el estado de una tarea | `User` (Solo el creador) |
| `DELETE` | `/api/Task/{id}` | Elimina una tarea | `User` (Solo el creador) |
| `GET` | `/api/Task/all` | Obtiene TODAS las tareas de todos los usuarios | `Admin` |

---
*Desarrollado por Brandon*

    
