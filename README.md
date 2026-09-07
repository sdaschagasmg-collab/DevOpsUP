# DevOpsUP — ProductsApi

API REST de gestión de productos desarrollada como Trabajo Práctico de la materia DevOps.

## Requisitos previos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) o superior (`dotnet --version` debe reportar `10.0.400` o compatible).
- Git.
- Visual Studio 2026 o VS Code con la extensión de C#.

## Estructura del proyecto

```
DevOpsUP/
├── DevOpsUP.slnx                          # Archivo de solución
├── src/
│   └── ProductsApi/                       # Proyecto principal (API)
│       ├── Controllers/
│       │   └── ProductsController.cs      # Endpoints CRUD de productos
│       ├── Models/
│       │   └── Product.cs                 # Modelo de dominio
│       ├── Services/
│       │   ├── IProductService.cs         # Contrato del servicio
│       │   └── InMemoryProductService.cs  # Implementación en memoria
│       ├── Program.cs                     # Configuración y arranque de la app
│       └── appsettings.json
└── tests/
    └── ProductsApi.Tests/                 # Tests unitarios (xUnit + Moq)
        └── ProductsControllerTests.cs
```

## Cómo ejecutar la API

Desde la raíz del repositorio:

```bash
dotnet run --project src/ProductsApi
```

La API queda disponible en la URL indicada en consola (por defecto sería algo como `https://localhost:xxxx`). También se puede explorar y probar todos los endpoints desde Swagger UI, disponible en:

```
https://localhost:xxxx/swagger
```

## Endpoints disponibles

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | `/api/products` | Lista todos los productos |
| GET | `/api/products/{id}` | Obtiene un producto por ID |
| POST | `/api/products` | Crea un nuevo producto |
| PUT | `/api/products/{id}` | Actualiza un producto existente |
| DELETE | `/api/products/{id}` | Elimina un producto |

## Cómo correr los tests

Desde la raíz del repositorio:

```bash
dotnet test tests/ProductsApi.Tests
```

El proyecto de tests utiliza xUnit como framework de testing y Moq para simular dependencias (`IProductService`), cubriendo los principales casos de uso del `ProductsController`.

## Flujo de trabajo (branching)

Este proyecto sigue un modelo simplificado de GitHub Flow:

- `main`: rama protegida, siempre estable y desplegable. No se permite push directo.
- `feature/*`: ramas de trabajo para cada nueva funcionalidad o corrección, integradas a `main` exclusivamente mediante **Pull Requests**.

Cada Pull Request requiere que el pipeline de integración continua (build + tests) finalice en verde antes de poder mergearse.

## Convención de commits

El historial de commits sigue la especificación de Conventional Commits:

| Prefijo | Uso |
|---------|-----|
| `feat:` | Nueva funcionalidad |
| `fix:` | Corrección de errores |
| `docs:` | Cambios en documentación |
| `test:` | Agregado o modificación de tests |
| `refactor:` | Cambios de código que no alteran el comportamiento |
| `chore:` | Tareas de mantenimiento (configuración, dependencias, etc.) |
| `ci:` | Cambios en la configuración de integración continua |

## Integración continua

El repositorio cuenta con un workflow de GitHub Actions que se ejecuta automáticamente en cada Pull Request hacia `main`, realizando:

1. Restauración de dependencias (`dotnet restore`).
2. Compilación del proyecto (`dotnet build`).
3. Ejecución de la suite de tests unitarios (`dotnet test`).

Un Pull Request solo puede mergearse si este pipeline finaliza exitosamente.

## Estrategia de versionado

El proyecto utiliza Semantic Versioning (SemVer) para el etiquetado de releases, con el formato MAJOR.MINOR.PATCH (ej: v1.0.0):
- MAJOR: cambios incompatibles en la API.
- MINOR: nuevas funcionalidades compatibles con versiones anteriores.
- PATCH: correcciones de errores compatibles con versiones anteriores.

## Autor Sebastián Das Chagas

Trabajo Práctico desarrollado para la materia DevOps — Universidad de Palermo.