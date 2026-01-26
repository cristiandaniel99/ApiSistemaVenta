# 🛒 ApiSistemaVenta

![.NET](https://img.shields.io/badge/.NET-8%2B-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-5C2D91)
![EF Core](https://img.shields.io/badge/EntityFramework-Core-green)
![License](https://img.shields.io/badge/license-MIT-blue)
![Status](https://img.shields.io/badge/status-Activo-success)

API RESTful para la gestión de un **Sistema de Ventas / Inventario (POS)** desarrollada con **ASP.NET Core + Entity Framework Core + Arquitectura por capas**.

> Backend ideal para sistemas de punto de venta, e-commerce o paneles administrativos.

---

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Tecnologías utilizadas](#-tecnologías-utilizadas)
- [Arquitectura](#-arquitectura)
- [Instalación y ejecución](#-instalación-y-ejecución)
- [Uso de la API](#-uso-de-la-api)
- [Endpoints principales](#-endpoints-principales)
- [Estructura del proyecto](#-estructura-del-proyecto)
- [Casos de uso](#-casos-de-uso)



---

## ✨ Características

- ✅ API REST con ASP.NET Core  
- ✅ CRUD de productos, categorías, clientes y ventas  
- ✅ Entity Framework Core  
- ✅ Arquitectura por capas (API, BLL, DAL)  
- ✅ Inyección de dependencias  
- ✅ Swagger / OpenAPI  
- ✅ Proyecto escalable y mantenible  

---

## 🛠 Tecnologías utilizadas

| Tecnología | Uso |
|-----------|------|
| .NET 7/8 | Framework principal |
| ASP.NET Core Web API | Endpoints REST |
| Entity Framework Core | ORM |
| SQL Server | Base de datos |
| Swagger | Documentación |

---

## 🧱 Arquitectura

El proyecto implementa separación de responsabilidades por capas:

API → BLL → DAL → Database


### Capas

- **SistemaVenta.API** → Controllers y configuración
- **SistemaVenta.BLL** → Lógica de negocio
- **SistemaVenta.DAL** → Acceso a datos
- **SistemaVenta.DTO** → Transferencia de datos
- **SistemaVenta.Model** → Entidades
- **SistemaVenta.IOC** → Inyección de dependencias
- **SistemaVenta.Utility** → Helpers

---

## ⚙️ Instalación y ejecución

### 1. Clonar repositorio

```bash

git clone https://github.com/cristiandaniel99/ApiSistemaVenta.git
cd ApiSistemaVenta 

```

### 2. Configurar base de datos

Editar appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SistemaVentaDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

### 3. Restaurar dependencias

dotnet restore

### 4. Aplicar migraciones (opcional)

dotnet ef database update

### 5. Ejecutar la API

dotnet run --project SistemaVenta.API

## 🚀 Uso de la API
Swagger (recomendado)

## Abrir en el navegador:

https://localhost:5001/swagger

Permite probar todos los endpoints sin herramientas externas.

# 📌 Endpoints principales
Método	Endpoint	Descripción
GET	/api/productos	Listar productos
GET	/api/productos/{id}	Obtener producto
POST	/api/productos	Crear producto
PUT	/api/productos/{id}	Actualizar producto
DELETE	/api/productos/{id}	Eliminar producto
POST	/api/ventas	Registrar venta
POST	/api/auth/login	Autenticación
🧪 Ejemplos
Crear producto

POST /api/productos
Content-Type: application/json

{
  "nombre": "Teclado Mecánico",
  "precio": 45.99,
  "stock": 10
}

Obtener productos

GET /api/productos

## 📂 Estructura del proyecto

```bash
ApiSistemaVenta/
│
├── SistemaVenta.API
├── SistemaVenta.BLL
├── SistemaVenta.DAL
├── SistemaVenta.DTO
├── SistemaVenta.Model
├── SistemaVenta.IOC
└── SistemaVenta.Utility
```



## 🎯 Casos de uso

Este backend puede utilizarse para:

 🛒 Punto de venta (POS)

 📦 Gestión de inventario

 🧾 Facturación

 📱 Backend móvil

 🌐 Panel administrativo
