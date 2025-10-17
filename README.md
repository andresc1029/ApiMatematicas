# 🧮 ApiMatematicas

API REST desarrollada en **ASP.NET Core (C#)** que sirve como backend para la web **La Cupula**.

---

## 🚀 Funcionalidades principales

### 👤 Gestión de usuarios
- Registro con validación de duplicados  
- Login con **JWT (Json Web Tokens)**  
- Roles de usuario (**Administrador / Usuario**)  
- Activación y desactivación de cuentas
- Recuperación de contraseña mediante MailKit (SMTP)

### 🔐 Seguridad
- Autenticación con **JWT**  
- Validación de tokens en cada petición protegida
- Implementación de CORS para permitir comunicación con la web cliente

### 🗄️ Base de datos
- **Entity Framework Core** (Code First)  
- Migraciones  
- Modelos: **Usuario**, **Rachas**

### 🧩 Tecnologias
- ASP.NET Core 8 / C#
- Entity Framework Core (Code First)
- SQL Server
- MailKit (para envío de correos de recuperación)  
- JSON (almacenamiento y carga de ejercicios)
- Patrones de diseño: DTO + Strategy

📦 ApiMatematicas
 ┣ 📂 Controllers
 ┣ 📂 DTOs
 ┣ 📂 Models
 ┣ 📂 Strategies
 ┣ 📂 Services
 ┣ 📂 JSON
 ┣ 📜 Program.cs
 ┣ 📜 appsettings.json
 ┗ 📜 ApiMatematicas.csproj


