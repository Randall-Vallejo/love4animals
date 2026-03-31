# SafeWildlife Love4Animals Application Backend

Este proyecto representa el backend para la red social de conservación animal Love4Animals, desarrollada para la ONG SafeWildLife. El objetivo de la plataforma es permitir a los misioneros publicar historias y gestionar la recaudación de fondos para distintas causas.

## Tecnologías Utilizadas
- Lenguaje: C#
- Framework: .NET 10 (ASP.NET Core Web API)
- Persistencia: En memoria mediante el uso de listas dinámicas compartidas
- Arquitectura: Diseño por capas (Controladores, Servicios y Repositorios)

---

## Funcionalidades Implementadas (Práctica 1)

### 1. Gestión de Usuarios (L4A-01)
Se completó el ciclo de desarrollo para la administración de usuarios en la ruta /v1/users:
- Listar Usuarios (GET): Recupera la lista completa de usuarios registrados en el sistema.
- Crear Usuario (POST): Registra una cuenta nueva calculando un ID dinámico y aplicando validación de nulos.
- Actualizar Perfil (PUT): Modifica los datos de un usuario en particular proporcionando su ID específico.
- Eliminar Usuario (DELETE): Remueve permanentemente una cuenta del sistema localizando el registro por su ID específico.

### 2. Gestión de Campañas (L4A-02)
Se estructuró el motor para la recaudación de fondos dentro de la ruta /v1/campaigns:
- Listar Campañas (GET): Muestra todos los objetivos de recolección de dinero activos en el sistema.
- Crear Campaña (POST): Permite dar de alta una nueva meta financiera vinculada a un misionero.
- Actualizar Campaña (PUT): Modifica la información o el estado de una campaña existente localizándola mediante su ID específico.
- Eliminar Campaña (DELETE): Da de baja una causa archivada del sistema usando su ID específico.

### 3. Gestión de Posts (L4A-03)
Se implementó el CRUD completo para posts en la ruta /v1/users/posts, permitiendo publicar contenido que puede asociarse opcionalmente a campañas:
- Obtener Post por ID (GET): Recupera un post específico mediante su ID.
- Crear Post (POST): Publica un nuevo post vinculado a un usuario existente, con opción de asociarlo a una campaña.
- Actualizar Post (PUT): Modifica el contenido de un post existente.
- Eliminar Post (DELETE): Remueve un post del sistema.

#### Características adicionales:
- **Validaciones**: Verifica que el usuario y la campaña (si se especifica) existan antes de crear o actualizar.
- **Asociación opcional**: Los posts pueden estar ligados a campañas para promocionar causas.
- **Respuestas JSON estructuradas**: Errores y respuestas siguen un formato consistente con códigos de estado y mensajes descriptivos.
- **Arquitectura**: Incluye modelo Post, DTOs (CreatePostDto, GetPostDto, UpdatePostDto), interfaces, servicios, repositorios y controlador.

---

## Cómo ejecutar el proyecto localmente

1. Clona el repositorio en tu máquina local:
   ```bash
   git clone [https://github.com/Randall-Vallejo/love4animals.git](https://github.com/Randall-Vallejo/love4animals.git)