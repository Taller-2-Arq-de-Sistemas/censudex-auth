
# Censudex — Auth Service (microservice)

Servicio micro (REST) que forma parte de la plataforma **Censudex**, consumido por la [**API Gateway**](https://github.com/Taller-2-Arq-de-Sistemas/censudex-api-gateway) y utilizado por otros servicios (como [Clients](https://github.com/Taller-2-Arq-de-Sistemas/censudex-clients), [Products](https://github.com/Taller-2-Arq-de-Sistemas/censudex-products), [Inventory](https://github.com/Taller-2-Arq-de-Sistemas/censudex-inventory) y [Orders](https://github.com/Taller-2-Arq-de-Sistemas/censudex-orders)).
Gestiona la **autenticación y validación de tokens JWT** de los usuarios (clientes) registrados, funcionando como **Identity Provider** interno.

Implementado en **C# (.NET 9)**, sigue una arquitectura simple tipo *Clean-ish* (Controllers → Services → JWT / HTTP Clients) y comunica con el **Clients Service** vía Gateway.

---

## Arquitectura y patrones

* **Arquitectura general:** microservicio REST independiente, responsable de emitir y validar tokens JWT.
* **Capas / organización:**

  * `Controllers` — endpoints de autenticación (`/login`, `/validate-token`, `/logout`).
  * `Services y Repositories` — lógica principal (`AuthService`, `JwtService`, `ClientsApiService`).
  * `Dtos` — request/response models (`LoginRequest`, `LoginResult`, `ClientResponse`, etc.).
  * `Extensions` — registro de servicios y carga de variables de entorno.
* **Patrones aplicados:**

  * **Service pattern:** separación clara entre controller y lógica de negocio.
  * **Delegación de autenticación:** el AuthService delega la validación de credenciales al *Clients Service*.
  * **JWT issuance:** los tokens se generan con `JwtService` y se firman con una clave secreta definida en `.env`.

---

## Requisitos previos

* .NET SDK 9 instalado.
* El [Clients Service](https://github.com/Taller-2-Arq-de-Sistemas/censudex-clients) debe estar corriendo y **accesible vía Gateway**.
* Paquetes NuGet principales:

  * `System.IdentityModel.Tokens.Jwt`
  * `Microsoft.AspNetCore.Authentication.JwtBearer`
  * `DotNetEnv`
  * `BCrypt.Net-Next`
* Archivo `.env` con variables sensibles:

  ```
  CLIENTS_SERVICE_URL=should_use_api_gateway_clients_url
  JWT_SECRET=your_jwt_secret
  JWT_EXPIRATION_MINUTES=your_jwt_expiration_time
  ```

---

## Levantamiento

> Asegúrate de que el API Gateway y el Clients Service estén en ejecución antes de iniciar Auth.

1. **Clonar / entrar al proyecto**

   ```bash
   git clone https://github.com/Taller-2-Arq-de-Sistemas/censudex-auth
   cd censudex-auth
   ```

2. **Copiar `.env.example`**

   ```bash
   cp .env.example .env
   ```

3. **Restaurar paquetes**

   ```bash
   dotnet restore
   ```

4. **Ejecutar**

   ```bash
   dotnet run
   ```

5. **Swagger (dev)**

   ```
   http://localhost:5002/swagger
   ```

---

## Endpoints principales (3)

### 1. `POST /login` — Autenticación de usuario

* URL: `POST http://localhost:5002/auth/login`

* Body (JSON):

  ```json
  {
    "email": "admin@censudex.cl",
    "password": "Password1234!"
  }
  ```

  O también:

  ```json
  {
    "username": "admin",
    "password": "Password1234!"
  }
  ```

* Proceso interno:

  1. AuthService envía la solicitud al **Clients Service** (vía Gateway) para verificar credenciales.
  2. Si el cliente existe y está activo, se genera un **JWT** firmado.
  3. Se devuelve el token.

* Respuesta exitosa:

  ```json
  {
    "success": true,
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
  ```

* Errores posibles:

  * `401 Unauthorized` — Usuario inactivo o no encontrado.
  * `401 Unauthorized` — Usuario no encontrado.
  * `400 Bad Request` 

---

### 2. `POST /validate-token` — Validar JWT

* URL: `POST http://localhost:5002/auth/validate-token`
* Acción: revisa si el token es válido y elimina tokens expirados de la blocklist.
* Headers:

  ```
  Authorization: Bearer <your_jwt_token>
  ```

* Respuesta:

  ```json
  {
    "isValid": true,
    "clientId": "8f5d1a3e-4f1c-490d-89a9-9aafdb612e63",
    "role": "Admin"
  }
  ```

* Errores:

  * `401 Unauthorized` — Token bloqueado o sesión cerrada.

---

### 3. `POST /logout` — Invalidar sesión (client-side)

* URL: `POST http://localhost:5002/auth/logout`
* Acción: no invalida el token en servidor (JWT es stateless), pero permite controlar logout desde el cliente agregándolo a una blocklist.
* Respuesta:

  ```json
  { "success": true, "message": "Sesión cerrada con éxito." }
  ```

---

## Qué contiene este repositorio

* `AuthController.cs` — define los endpoints `/login`, `/validate-token`, `/logout`.
* `AuthService.cs` — maneja lógica de autenticación (delegación al Clients Service + verificación de contraseñas).
* `JwtService.cs` — genera, firma y valida tokens JWT.
* `ClientsApiService.cs` — maneja la comunicación HTTP con el Clients Service.
* `Dtos/` — modelos de request/response.
* `.env` — configuración de entorno (JWT_SECRET, URLs, etc.).
* `Swagger` — para probar endpoints manualmente.

---

## Este repositorio debería usarse en conjunto con:

* [Censudex API Gateway](https://github.com/Taller-2-Arq-de-Sistemas/censudex-api-gateway)
* [Censudex Clients Service](https://github.com/Taller-2-Arq-de-Sistemas/censudex-clients)