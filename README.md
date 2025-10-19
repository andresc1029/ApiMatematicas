# 🧮 ApiMatematicas

API REST desarrollada en **ASP.NET Core (C#)** que sirve como backend para la web **La Cupula**.


## Estructura de proyecto
<img width="472" height="454" alt="image" src="https://github.com/user-attachments/assets/33d6f23e-8f23-4ac4-a9ef-eb7baf4f39af" />
## Registro
<img width="967" height="528" alt="image" src="https://github.com/user-attachments/assets/de85db09-f073-4eb0-a8e0-ddac70c7704b" />
## Auteticacion JWT
<img width="884" height="271" alt="image" src="https://github.com/user-attachments/assets/c929e227-f2c8-4a08-9b4b-9c2802a0e904" />
<img width="526" height="169" alt="image" src="https://github.com/user-attachments/assets/22635f64-77be-4c04-a408-c4bdc3068ab0" />
<img width="821" height="434" alt="image" src="https://github.com/user-attachments/assets/7991a7e6-edc7-4353-9400-6aa9eea201c9" />





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
┣ 📂 Clases
┣ 📂 Controllers
┣ 📂 Data
┣ 📂 DTOs
┣ 📂 JSON
┣ 📂 Models
┣ 📂 Strategies
┣ 📂 Services
┣ 📜 Program.cs
┣ 📜 appsettings.json
┗ 📜 ApiMatematicas.csproj
