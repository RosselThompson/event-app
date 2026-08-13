# Event App

Sistema web para administrar eventos, registrar asistentes y controlar su entrada mediante un proceso de check-in. El proyecto se construirá inicialmente como un **monolito regular**, compuesto por una aplicación React, una API ASP.NET Core y una única base de datos PostgreSQL.

El propósito es mantener una arquitectura sencilla de desarrollar y desplegar, sin renunciar a una separación clara de responsabilidades dentro del código.

## Objetivos funcionales

La aplicación permitirá:

- Registrar usuarios y autenticar sesiones mediante JWT.
- Autorizar operaciones de acuerdo con el rol del usuario.
- Crear, consultar, actualizar y eliminar eventos.
- Registrar personas como invitadas o como asistentes que pagaron entrada.
- Asociar asistentes con eventos y generar un código único por registro.
- Buscar registros por código, nombre o correo electrónico.
- Realizar el check-in y guardar quién lo realizó y en qué momento.
- Evitar que un mismo registro sea utilizado para entrar más de una vez.

## Arquitectura general

```text
┌──────────────────────────────────────────────────────────────┐
│                         React SPA                            │
│  React Router · TanStack Query · Zustand · TypeScript       │
└─────────────────────────────┬────────────────────────────────┘
                              │ HTTPS / JSON
                              ▼
┌──────────────────────────────────────────────────────────────┐
│                    ASP.NET Core Web API                      │
│                                                              │
│  Controllers → Application Services → Domain → Persistence  │
│                                                              │
│  Authentication · Events · Attendees · Check-in             │
└─────────────────────────────┬────────────────────────────────┘
                              │ Entity Framework Core
                              ▼
┌──────────────────────────────────────────────────────────────┐
│                         PostgreSQL                           │
│               Única base de datos del sistema               │
└──────────────────────────────────────────────────────────────┘
```

### Frontend

El frontend será una SPA desarrollada con React y TypeScript. Toda operación de negocio se realizará a través de la API; el navegador no accederá directamente a la base de datos.

Responsabilidades principales:

- Renderizar las pantallas y manejar la navegación.
- Consumir y almacenar en caché datos remotos con TanStack Query.
- Mantener únicamente estado global del cliente con Zustand, por ejemplo la sesión y preferencias de interfaz.
- Validar formularios para mejorar la experiencia del usuario.
- Aplicar protección visual de rutas según el rol, sin considerar esta protección un reemplazo de la autorización del backend.

### Backend

El backend será una única aplicación ASP.NET Core Web API. Contendrá todas las capacidades del sistema y se desplegará como una sola unidad.

Se organizará en capas lógicas:

- **API:** endpoints, autenticación, autorización, serialización y manejo global de errores.
- **Aplicación:** casos de uso y coordinación de operaciones.
- **Dominio:** entidades, reglas de negocio y estados válidos.
- **Infraestructura:** Entity Framework Core, PostgreSQL, generación de tokens y servicios externos.

La separación en capas busca evitar que los controladores contengan reglas de negocio o accedan directamente a la base de datos.

### Base de datos

El sistema utilizará una sola instancia y una sola base de datos PostgreSQL. Entity Framework Core administrará el mapeo y las migraciones.

Las relaciones y restricciones importantes también deberán protegerse en la base de datos mediante claves foráneas, índices únicos y restricciones apropiadas. Las migraciones formarán parte del repositorio.

## Estructura prevista del repositorio

```text
event-app/
├── backend/
│   ├── src/
│   │   ├── EventApp.Api/
│   │   ├── EventApp.Application/
│   │   ├── EventApp.Domain/
│   │   └── EventApp.Infrastructure/
│   ├── tests/
│   │   ├── EventApp.UnitTests/
│   │   └── EventApp.IntegrationTests/
│   └── EventApp.sln
├── frontend/
│   ├── src/
│   │   ├── api/
│   │   ├── components/
│   │   ├── features/
│   │   ├── pages/
│   │   ├── routes/
│   │   └── stores/
│   └── tests/
├── infrastructure/
│   └── docker/
├── docs/
├── docker-compose.yml
├── .env.example
└── README.md
```

Esta es la estructura objetivo. Las carpetas se crearán a medida que se implemente cada componente.

## Áreas funcionales

Aunque todo se ejecutará en una misma API, el código se separará por las siguientes áreas:

### Identidad y acceso

- Registro e inicio de sesión.
- Hash seguro de contraseñas; nunca se almacenarán contraseñas en texto plano.
- Emisión y validación de JWT.
- Administración de roles.
- Políticas de autorización para cada operación.

Roles iniciales sugeridos:

| Rol | Responsabilidades |
| --- | --- |
| `Admin` | Administrar eventos, usuarios, asistentes y consultar toda la información. |
| `Staff` | Consultar asistentes y realizar check-in. |

### Eventos

- Creación y edición de eventos.
- Configuración de fecha, lugar y capacidad.
- Consulta del listado y detalle de eventos.
- Eliminación controlada cuando no existan datos que deban conservarse.

### Asistentes y registros

Una persona y su registro representan conceptos diferentes:

- **Persona:** identidad básica del asistente, como nombre y correo.
- **Registro:** asociación de una persona con un evento específico.

Cada registro tendrá un código público único, apto para representarse como QR. El código identificará el registro, pero no incluirá datos personales sensibles.

### Check-in

- Búsqueda por código único, nombre o correo.
- Validación de que el registro corresponde al evento seleccionado.
- Confirmación de que el registro se encuentra habilitado para entrar.
- Prevención de check-ins duplicados.
- Registro de la fecha, hora y usuario de staff responsable.

## Modelo de dominio inicial

### User

| Campo | Tipo sugerido | Descripción |
| --- | --- | --- |
| `Id` | UUID | Identificador interno. |
| `Name` | string | Nombre visible. |
| `Email` | string | Correo normalizado y único. |
| `PasswordHash` | string | Hash seguro de la contraseña. |
| `Role` | enum/string | `Admin` o `Staff`. |
| `CreatedAt` | timestamp UTC | Fecha de creación. |

### Event

| Campo | Tipo sugerido | Descripción |
| --- | --- | --- |
| `Id` | UUID | Identificador del evento. |
| `Name` | string | Nombre del evento. |
| `StartsAt` | timestamp UTC | Fecha y hora de inicio. |
| `Venue` | string | Lugar. |
| `Capacity` | integer | Capacidad máxima positiva. |
| `CreatedAt` | timestamp UTC | Fecha de creación. |

### Person

| Campo | Tipo sugerido | Descripción |
| --- | --- | --- |
| `Id` | UUID | Identificador de la persona. |
| `Name` | string | Nombre completo. |
| `Email` | string | Correo de contacto normalizado. |

### Registration

| Campo | Tipo sugerido | Descripción |
| --- | --- | --- |
| `Id` | UUID | Identificador del registro. |
| `PersonId` | UUID | Persona registrada. |
| `EventId` | UUID | Evento asociado. |
| `Type` | enum | `Guest` o `Paid`. |
| `Status` | enum | `Pending`, `Confirmed`, `Paid` o `Cancelled`. |
| `UniqueCode` | UUID/string | Código público único. |
| `CreatedAt` | timestamp UTC | Fecha del registro. |

Se deberá crear una restricción única para impedir que una misma persona tenga registros duplicados para el mismo evento.

### CheckIn

| Campo | Tipo sugerido | Descripción |
| --- | --- | --- |
| `Id` | UUID | Identificador del check-in. |
| `RegistrationId` | UUID | Registro validado. |
| `PerformedByUserId` | UUID | Usuario que autorizó la entrada. |
| `CheckedInAt` | timestamp UTC | Momento del ingreso. |

`CheckIn` será una entidad independiente y no solamente un booleano dentro de `Registration`. Esto permite conservar auditoría y aplicar una restricción única sobre `RegistrationId` para evitar entradas duplicadas.

## Flujo principal

1. Un usuario inicia sesión con correo y contraseña.
2. La API valida sus credenciales y devuelve un JWT con su identificador y rol.
3. Un administrador crea un evento.
4. El administrador registra una persona y la asocia con el evento.
5. La API crea el registro y genera su código único.
6. El día del evento, un usuario de staff busca o escanea el registro.
7. La API valida el evento, estado del registro y check-ins anteriores.
8. La API crea el check-in y devuelve la confirmación.

## API inicial

Los endpoints se versionarán bajo `/api/v1`.

```text
POST   /api/v1/auth/register
POST   /api/v1/auth/login
GET    /api/v1/auth/me

GET    /api/v1/events
POST   /api/v1/events
GET    /api/v1/events/{eventId}
PUT    /api/v1/events/{eventId}
DELETE /api/v1/events/{eventId}

GET    /api/v1/attendees
POST   /api/v1/attendees
GET    /api/v1/attendees/{attendeeId}
PUT    /api/v1/attendees/{attendeeId}
GET    /api/v1/events/{eventId}/attendees

GET    /api/v1/registrations/code/{uniqueCode}
POST   /api/v1/events/{eventId}/registrations/{registrationId}/check-ins
GET    /api/v1/events/{eventId}/check-ins
```

La lista es un contrato inicial y podrá ajustarse durante el diseño detallado. La especificación definitiva se expondrá mediante OpenAPI/Swagger.

## Seguridad

- Las contraseñas se almacenarán mediante un algoritmo de hash apropiado proporcionado por ASP.NET Core Identity o un componente equivalente.
- Los JWT tendrán una vida útil limitada y se firmarán con secretos que no se guardarán en Git.
- La autorización se aplicará en la API mediante roles y políticas.
- Todos los datos recibidos se validarán en el backend, incluso si ya fueron validados por React.
- Los códigos QR no contendrán nombres, correos ni credenciales.
- Los mensajes de error no expondrán hashes, secretos ni detalles internos.
- Las fechas se almacenarán en UTC y se convertirán para su presentación en el frontend.

## Estado del frontend

Se asignará una responsabilidad distinta a cada herramienta:

- **TanStack Query:** datos provenientes de la API, caché, reintentos e invalidación después de mutaciones.
- **Zustand:** estado estrictamente local y global de la interfaz, como información mínima de la sesión.
- **Estado de componentes:** formularios y datos efímeros que solo pertenecen a una pantalla.

No se duplicarán en Zustand los datos administrados por TanStack Query.

## Pruebas

La estrategia inicial incluirá:

- Pruebas unitarias para reglas de dominio y servicios de aplicación.
- Pruebas de integración de endpoints y persistencia con PostgreSQL.
- Pruebas del frontend para componentes y flujos críticos.
- Una prueba end-to-end del flujo: autenticación, registro y check-in.

Los casos críticos incluyen capacidad agotada, registro cancelado, código inexistente, evento incorrecto y check-in duplicado.

## Desarrollo y despliegue

Docker Compose permitirá ejecutar localmente:

- PostgreSQL.
- ASP.NET Core Web API.
- React SPA.

La configuración sensible se proporcionará mediante variables de entorno. El archivo `.env.example` documentará las variables necesarias sin incluir valores secretos.

En la primera versión, frontend, API y base de datos podrán desplegarse como tres componentes, pero la lógica del backend continuará siendo una sola aplicación monolítica.

## Decisiones arquitectónicas

1. **Monolito regular:** reduce la complejidad operacional y permite concentrarse en autenticación, CRUD, reglas de negocio y experiencia de usuario.
2. **Una sola API:** React dispone de un único punto de entrada y no requiere gateway.
3. **Una sola base de datos:** las operaciones relacionadas pueden utilizar transacciones locales y claves foráneas.
4. **Capas lógicas:** mantienen las reglas de negocio separadas de HTTP y de Entity Framework Core.
5. **Check-in como entidad:** conserva auditoría y evita depender de un indicador booleano sin historial.
6. **UUID para identificadores:** evita identificadores secuenciales expuestos y facilita una posible distribución futura.

## Evolución futura

No se diseñará el sistema como microservicios prematuros. Si el producto creciera, las áreas de identidad, eventos, asistentes o check-in podrían convertirse en servicios independientes, pero dicha extracción requeriría revisar transacciones, propiedad de datos y comunicación entre procesos.

Esta posibilidad no debe añadir complejidad innecesaria a la primera versión del proyecto.

## Estado del proyecto

El proyecto se encuentra en la fase de diseño. La estructura, código fuente, migraciones y configuración de ejecución se incorporarán en las siguientes etapas.
