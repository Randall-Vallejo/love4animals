# 📖 **Entity Framework Core 10 en Love4Animals — Guía de Implementación**

---

## 📌 **Resumen de lo Implementado**

Se ha integrado **Entity Framework Core 10** en la API de Love4Animals manteniendo la **arquitectura de capas existente** (Controllers → Services → Repositories). Ahora el proyecto funciona en **dos modos simultáneamente**:

1. **Modo En Memoria** (Listas C#) — Desarrollo rápido, pruebas unitarias
2. **Modo Entity Framework Core** (PostgreSQL) — Producción, base de datos real

---

## 🧠 **¿Por Qué Entity Framework Core?**

### Ventajas de EF Core
- **LINQ → SQL**: Las consultas C# se convierten automáticamente a SQL optimizado
- **Change Tracking**: EF Core detecta cambios en entidades automáticamente
- **Migraciones**: Versiona el esquema de BD de forma controlada
- **Multi-Proveedor**: Soporta SQL Server, PostgreSQL, SQLite, Cosmos DB, etc.

### Arquitectura
```
Tu Código C# (LINQ)
    ↓ (traduce a)
Entity Framework Core (DbContext)
    ↓ (usa)
Proveedor PostgreSQL (Npgsql)
    ↓ (ejecuta)
Base de Datos PostgreSQL
```

---

## 📦 **Paquetes NuGet Instalados**

```bash
✓ Npgsql.EntityFrameworkCore.PostgreSQL  # Proveedor para PostgreSQL
✓ Microsoft.EntityFrameworkCore.Tools     # CLI para migraciones
✓ Microsoft.EntityFrameworkCore.Design    # Herramientas de diseño
```

**Versión**: EF Core 10.0.7 (compatible con .NET 10)

---

## 🗂️ **Estructura del Proyecto con EF Core**

```
Love4AnimalsApi/
├── Data/
│   └── AppDbContext.cs                 ← **NUEVO** DbContext de EF Core
├── Models/                             ← **MODIFICADOS** con propiedades de navegación
│   ├── User.cs
│   ├── Campaign.cs
│   ├── Post.cs
│   ├── Comment.cs
│   └── Donation.cs
├── Repositories/
│   ├── (antiguas implementaciones en memoria)
│   │   ├── UserRepository.cs
│   │   ├── CampaignRepository.cs
│   │   └── ...
│   └── EF/                             ← **NUEVO** Implementaciones con EF Core
│       ├── EFUserRepository.cs
│       ├── EFCampaignRepository.cs
│       ├── EFPostRepository.cs
│       ├── EFCommentRepository.cs
│       └── EFDonationRepository.cs
├── Services/
│   ├── UserService.cs
│   ├── CampaignService.cs
│   └── (sin cambios, funcionan con ambas implementaciones)
├── Controllers/
│   └── (sin cambios, funcionan con ambas implementaciones)
├── Migrations/                         ← **NUEVO** Carpeta de migraciones
│   ├── 20260430000000_Inicial.cs
│   ├── 20260430000000_Inicial.Designer.cs
│   └── Love4AnimalsApiContextModelSnapshot.cs
├── Program.cs                          ← **MODIFICADO** Registra DbContext y repositorios EF
├── appsettings.json                    ← **MODIFICADO** Agregada cadena de conexión
└── Love4AnimalsApi.csproj
```

---

## 🔌 **AppDbContext — Centro de EF Core**

**Archivo**: `Data/AppDbContext.cs`

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Donation> Donations => Set<Donation>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define relaciones, precisiones, restricciones
        // Ejemplo: HasMany(usuario).WithMany(campañas)
    }
}
```

**Responsabilidades**:
- Mapear entidades C# a tablas PostgreSQL
- Definir relaciones (1-a-muchos, muchos-a-muchos)
- Especificar restricciones (longitud, precisión decimal, etc.)

---

## 🔄 **Models — Propiedades de Navegación**

**ANTES (sin EF Core)**:
```csharp
public class Campaign
{
    public int IdCampania { get; set; }
    public string Titulo { get; set; }
    public int UsuarioId { get; set; }  // Solo FK
}
```

**AHORA (con EF Core)**:
```csharp
public class Campaign
{
    public int IdCampania { get; set; }
    public string Titulo { get; set; }
    public int UsuarioId { get; set; }  // FK
    
    // Propiedades de navegación ← NUEVAS
    public User? Usuario { get; set; }                    // Referencia al Usuario
    public ICollection<Post> Posts { get; set; } = [];   // Colección de Posts
    public ICollection<Donation> Donations { get; set; } = [];
}
```

**¿Qué son las propiedades de navegación?**
- Permiten acceder a entidades relacionadas sin escribir SQL
- Ejemplo: `campaign.Usuario.Nombre` → accede al usuario que creó la campaña
- EF Core las gestiona automáticamente mediante claves foráneas

---

## 📂 **Repositories EF Core — Acceso a Datos Real**

**Archivo Ejemplo**: `Repositories/EF/EFUserRepository.cs`

```csharp
public class EFUserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public EFUserRepository(AppDbContext context)
    {
        this._context = context;
    }

    public User? GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public User CreateUser(User user)
    {
        _context.Users.Add(user);       // Agrega a la sesión
        _context.SaveChanges();         // Ejecuta INSERT en BD
        return user;
    }
}
```

**Diferencias vs en memoria**:
- ✅ Usa `_context.Users` (DbSet) en lugar de `List<User>`
- ✅ `SaveChanges()` persiste cambios a PostgreSQL
- ✅ `Include()` para cargar entidades relacionadas

---

## ⚙️ **Configuración en Program.cs**

**NUEVO**: Registración de DbContext

```csharp
// Cadena de conexión desde appsettings.json
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

// Registra AppDbContext con PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
```

**OPCIÓN 1: Usar EF Core** (Actual)
```csharp
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<ICampaignRepository, EFCampaignRepository>();
// ... resto de repositorios
```

**OPCIÓN 2: Usar En Memoria** (Comentada)
```csharp
// builder.Services.AddSingleton<IUserRepository, UserRepository>();
// builder.Services.AddSingleton<ICampaignRepository, CampaignRepository>();
```

---

## 🔐 **Cadena de Conexión — appsettings.json**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=Love4AnimalsDb;Username=postgres;Password=postgres"
  }
}
```

**Parámetros**:
- `Host`: Servidor PostgreSQL (localhost en desarrollo)
- `Port`: 5432 (puerto predeterminado de PostgreSQL)
- `Database`: Nombre de la base de datos (se crea automáticamente)
- `Username`: Usuario PostgreSQL
- `Password`: Contraseña

---

## 🔄 **Migraciones — Control de Esquema**

### ¿Qué es una Migración?

Es un archivo C# que describe cambios en el esquema de BD de forma versionada.

### Crear la Primera Migración

```bash
dotnet ef migrations add Inicial
```

**Resultado**: Se crean dos archivos en `Migrations/`
- `20260430000000_Inicial.cs` — Código C# de cambios
- `Love4AnimalsApiContextModelSnapshot.cs` — Snapshot actual

### Aplicar la Migración a PostgreSQL

```bash
dotnet ef database update
```

**Qué hace**:
1. Se conecta a PostgreSQL
2. Crea la base de datos si no existe
3. Ejecuta las migraciones pendientes (CREATE TABLE, etc.)

### Revertir Cambios

```bash
dotnet ef database update NombreDeMigracionAnterior
```

---

## 🔍 **Ejemplos de Consultas LINQ con EF Core**

### Obtener un Usuario por ID (con include de relacionados)

```csharp
var user = await _context.Users
    .Include(u => u.Campaigns)  // Carga campañas del usuario
    .Include(u => u.Posts)
    .FirstOrDefaultAsync(u => u.Id == 1);
```

### Filtrar Donaciones por Campaña

```csharp
var donations = await _context.Donations
    .Where(d => d.IdCampania == 1)
    .Include(d => d.Usuario)        // Carga datos del usuario
    .OrderByDescending(d => d.Fecha)
    .ToListAsync();
```

### Actualización Masiva (EF Core 10)

```csharp
await _context.Campaigns
    .Where(c => c.IdCampania == 1)
    .ExecuteUpdateAsync(s => 
        s.SetProperty(c => c.MontoRecaudado, c => c.MontoRecaudado * 1.10m)
    );
```

**Ventaja**: No carga entidades en memoria, directamente en BD.

---

## 🚀 **Novedades de EF Core 10**

| Característica | Descripción |
|---|---|
| **LeftJoin/RightJoin** | Joins nativos en LINQ |
| **ExecuteUpdateAsync** | Actualizaciones masivas sin cargar datos |
| **Soporte JSON** | Mapeo de tipos JSON en BD |
| **Vector Types** | Soporte para búsquedas vectoriales (IA) |
| **Complex Types** | Tipos complejos anidados |
| **Rendimiento +20%** | Optimizaciones significativas |

---

## ✅ **Checklist de Implementación**

- ✅ Paquetes NuGet instalados
- ✅ `AppDbContext` creado en `Data/AppDbContext.cs`
- ✅ Models actualizados con propiedades de navegación
- ✅ Repositorios EF Core creados en `Repositories/EF/`
- ✅ Program.cs registra DbContext y repositorios
- ✅ `appsettings.json` contiene cadena de conexión
- ✅ Primera migración creada (`dotnet ef migrations add Inicial`)
- ✅ Proyecto compila sin errores

---

## 🔧 **Próximos Pasos para Usar en Producción**

### 1. Instalar PostgreSQL localmente o en servidor

```bash
# Windows (usando Chocolatey o descarga oficial)
# macOS
brew install postgresql@16

# Linux (Ubuntu/Debian)
sudo apt install postgresql postgresql-contrib
```

### 2. Crear base de datos

```bash
createdb Love4AnimalsDb
```

### 3. Aplicar migraciones

```bash
dotnet ef database update
```

### 4. Cambiar de En Memoria a EF Core en Program.cs

Descomenta la sección "OPCIÓN 1" y comenta la "OPCIÓN 2"

### 5. Ejecutar la API

```bash
dotnet run
```

---

## 📊 **Comparación: En Memoria vs EF Core**

| Aspecto | En Memoria | EF Core |
|---|---|---|
| **Almacenamiento** | Listas C# en RAM | Base de datos PostgreSQL |
| **Persistencia** | Se pierde al reiniciar | Permanente |
| **Rendimiento** | Muy rápido (desarrollo) | Optimizado (producción) |
| **Escalabilidad** | Limitado a RAM | Ilimitado (BD) |
| **Costos** | Libre | Licencia BD (PostgreSQL gratuito) |
| **Ideal para** | Desarrollo, testing | Producción |

---

## 🎯 **Arquitectura Final de Love4Animals**

```
┌─────────────────────────────────────────┐
│         HTTP Client (Postman/Web)       │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│          Controllers (v1/*)             │
│   DonationController, etc.              │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│          Services (Lógica)              │
│   DonationService (validaciones)        │
└────────────────┬────────────────────────┘
                 │
┌────────────────▼────────────────────────┐
│   IRepository (Interfaz)                │
│   ├─ En Memoria (desarrollo)            │
│   └─ EF Core (producción)     ← ELEGIR  │
└────────────────┬────────────────────────┘
                 │
      ┌──────────┴──────────┐
      │                     │
  ┌───▼──────┐      ┌──────▼────────┐
  │ List<T>  │      │ AppDbContext  │
  │(En RAM)  │      │(EF Core)      │
  └──────────┘      └──────┬────────┘
                           │
                    ┌──────▼────────┐
                    │  PostgreSQL   │
                    │  (BD Real)    │
                    └───────────────┘
```

---

## 📚 **Documentación Oficial**

- [Microsoft Learn — EF Core](https://learn.microsoft.com/ef/core)
- [Npgsql Documentation](https://www.npgsql.org/)
- [PostgreSQL Official Docs](https://www.postgresql.org/docs/)

---

## 🎓 **Conclusión**

Love4Animals ahora tiene una arquitectura **profesional y escalable**:

✅ Controllers sin cambios
✅ Services sin cambios  
✅ Interfaces estables
✅ Dos implementaciones de Repositories (elegibles)
✅ Base de datos real (PostgreSQL)
✅ Migraciones versionadas

**¡Lista para producción!** 🚀
