# API de Gestión de Adquisiciones ADRES

API RESTful desarrollada con .NET para la gestión de adquisiciones de bienes y servicios, implementando operaciones CRUD completas y seguimiento de historial de cambios.

![Logo de ADRES](https://github.com/jhongo20/adquisiciones-app/blob/main/src/assets/images/logo-adres.png)

## Tecnologías utilizadas

- **.NET 8.0**
- **Entity Framework Core**
- **SQLite** (configurable para SQL Server)
- **Swagger** para documentación de API

## Estructura del proyecto

El proyecto está organizado en una arquitectura de capas:

- **AdquisicionesApp.API**: Controladores REST y configuración de la aplicación.
- **AdquisicionesApp.Services**: Lógica de negocio y validaciones.
- **AdquisicionesApp.Data**: Acceso a datos, repositorios y modelos.

## Requisitos previos

- **.NET SDK 9.0** o superior
- **Visual Studio 2022** o **Visual Studio Code**
- **Git** (para clonar el repositorio)

## Guía de instalación y configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/jhongo20/backend-Adquisiciones.git
cd backend-Adquisiciones

API de Gestión de Adquisiciones ADRES
API RESTful desarrollada con .NET para la gestión de adquisiciones de bienes y servicios, implementando operaciones CRUD completas y seguimiento de historial de cambios.
Mostrar imagen
Tecnologías utilizadas

.NET 8.0
Entity Framework Core
SQLite (configurable para SQL Server)
Swagger para documentación de API

Estructura del proyecto
El proyecto está organizado en una arquitectura de capas:

AdquisicionesApp.API: Controladores REST y configuración de la aplicación
AdquisicionesApp.Services: Lógica de negocio y validaciones
AdquisicionesApp.Data: Acceso a datos, repositorios y modelos

Requisitos previos

.NET SDK 8.0 o superior
Visual Studio 2022 
Git (para clonar el repositorio)

Guía de instalación y configuración
1. Clonar el repositorio
Copiar
git clone https://github.com/jhongo20/backend-Adquisiciones.git
cd backend-Adquisiciones
2. Restaurar paquetes NuGet
Copiar
dotnet restore
3. Instalar paquetes necesarios
Asegúrate de tener todos los paquetes de Entity Framework Core instalados:
Copiar # En el proyecto AdquisicionesApp.Data
cd AdquisicionesApp.Data
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools

# En el proyecto AdquisicionesApp.API
cd ../AdquisicionesApp.API
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore
4. Configurar la base de datos
La aplicación utiliza SQLite por defecto para facilitar la configuración inicial. La cadena de conexión ya está configurada en appsettings.json:
Copiar
"ConnectionStrings": {
  "DefaultConnection": "Data Source=adquisiciones.db"
}
5. Compilar el proyecto


Navega a la carpeta del proyecto API:

Copiar
cd AdquisicionesApp.API

Ejecuta la aplicación:

Copiar
dotnet run

La API estará disponible en:

http://localhost:5138 (HTTP)


Accede a la documentación Swagger:

http://localhost:5138/swagger/index.html



Uso de la API con Swagger
Una vez que la aplicación esté en ejecución, puedes probar todas las funcionalidades a través de la interfaz de Swagger:

Abre en tu navegador: http://localhost:5138/swagger/index.html
Aquí verás todos los endpoints disponibles agrupados por controlador.
Probar diferentes endpoints:

Obtener todas las adquisiciones

Haz clic en el endpoint GET /api/Adquisiciones
Haz clic en "Try it out"
Haz clic en "Execute"
Verás la respuesta con el código de estado y los datos de adquisiciones

Crear una nueva adquisición

Haz clic en el endpoint POST /api/Adquisiciones
Haz clic en "Try it out"
Modifica el ejemplo de payload JSON que se muestra:

jsonCopiar{
  "presupuesto": 5000000,
  "unidad": "Dirección Administrativa",
  "tipoBienServicio": "Mobiliario",
  "cantidad": 20,
  "valorUnitario": 250000,
  "fechaAdquisicion": "2023-08-15T00:00:00",
  "proveedor": "Tecnologías Médicas S.A.",
  "documentacion": "Orden de compra No. 2023-08-15-002",
  "activo": true
}

Haz clic en "Execute"
Verifica el código de respuesta 201 (Created) y anota el ID generado

Ver detalles de una adquisición

Haz clic en el endpoint GET /api/Adquisiciones/{id}
Haz clic en "Try it out"
Ingresa el ID de la adquisición que acabas de crear
Haz clic en "Execute"
Verifica los detalles completos de la adquisición

Modificar una adquisición

Haz clic en el endpoint PUT /api/Adquisiciones/{id}
Haz clic en "Try it out"
Ingresa el ID de la adquisición
Modifica algunos campos en el payload JSON
Haz clic en "Execute"
Verifica el código de respuesta 204 (No Content)

Ver historial de cambios

Después de modificar una adquisición, puedes ver el historial de cambios
Haz clic en el endpoint GET /api/Adquisiciones/{id}/historial
Haz clic en "Try it out"
Ingresa el ID de la adquisición
Haz clic en "Execute"
Verás todos los cambios realizados, con los valores anteriores y nuevos

Desactivar una adquisición

Haz clic en el endpoint PUT /api/Adquisiciones/{id}/desactivar
Haz clic en "Try it out"
Ingresa el ID de la adquisición
Haz clic en "Execute"
Verifica el código de respuesta 204 (No Content)

Filtrar adquisiciones

Haz clic en el endpoint GET /api/Adquisiciones/filtrar
Haz clic en "Try it out"
Ingresa parámetros de filtro como tipoBienServicio, unidad, etc.
Haz clic en "Execute"
Verás las adquisiciones que coinciden con tus criterios de filtro

Resolución de problemas comunes
Error "Could not resolve @angular/animations/browser"
Si al ejecutar la aplicación recibes este error, necesitas instalar el paquete correspondiente:
bashCopiardotnet add package Swashbuckle.AspNetCore
Error de serialización circular
Si experimentas errores 500 al obtener adquisiciones con sus historiales, modifica la configuración JSON en Program.cs:
csharpCopiarbuilder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
Base de datos no creada
Si la base de datos no se crea automáticamente, verifica que el código en Program.cs contenga:
csharpCopiar// En el método Configure del ciclo de vida de la aplicación
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AdquisicionesDbContext>();
    dbContext.Database.EnsureCreated();
}
Estructura de la base de datos
La base de datos contiene dos tablas principales:

Adquisiciones: Almacena la información de cada adquisición

Id (PK)
Presupuesto
Unidad
TipoBienServicio
Cantidad
ValorUnitario
FechaAdquisicion
Proveedor
Documentacion
Activo
FechaCreacion
FechaModificacion


HistorialAdquisiciones: Almacena el historial de cambios

Id (PK)
AdquisicionId (FK)
CampoModificado
ValorAnterior
ValorNuevo
FechaModificacion
UsuarioModificacion



Endpoints de la API
MétodoRutaDescripciónGET/api/AdquisicionesObtener todas las adquisicionesGET/api/Adquisiciones/{id}Obtener una adquisición por IDGET/api/Adquisiciones/filtrarFiltrar adquisicionesPOST/api/AdquisicionesCrear una nueva adquisiciónPUT/api/Adquisiciones/{id}Actualizar una adquisición existentePUT/api/Adquisiciones/{id}/desactivarDesactivar una adquisiciónGET/api/Adquisiciones/{id}/historialObtener historial de cambios
Características principales

Gestión completa de adquisiciones: Consultar, crear, modificar y desactivar adquisiciones.
Seguimiento de historial: Registro automático de todos los cambios realizados a las adquisiciones.
Filtros avanzados: Búsqueda por unidad, tipo, proveedor, fechas, etc.
Validaciones: Reglas de negocio aplicadas antes de realizar cambios.
Serialización JSON configurable: Manejo de referencias circulares para evitar errores.

Contacto
Para más información o soporte, contactar a:

Desarrollador: Jhon Jairo Perez
GitHub: @jhongo20


