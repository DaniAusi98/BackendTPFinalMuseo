using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infrastructure.Data.Seeders
{
    public static class RolePermissionsSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = scope.ServiceProvider.GetService<ILogger<RoleManager<IdentityRole>>>();

            try
            {
                logger?.LogInformation("⏳ Verificando la existencia de roles y permisos en el sistema...");

                // 1. Verificar si ya existen roles en la Base de Datos para evitar duplicación
                var rolesExistentes = await roleManager.Roles.ToListAsync();

                if (rolesExistentes != null && rolesExistentes.Any())
                {
                    logger?.LogInformation("✅ Los roles del sistema ya se encuentran poblados ({Count} encontrados). Seed omitido.", rolesExistentes.Count());
                    return;
                }

                logger?.LogInformation("🚀 No se encontraron roles. Iniciando el volcado de roles y permisos oficiales del sistema...");

                // 2. Obtener la configuración validada desde la Factory de Seguridad
                var rolePermissions = RolePermissionsConfig.RolePermissions;

                // 3. Persistir cada rol y sus permisos (claims) de forma asíncrona mediante el RoleManager
                foreach (var rolePermission in rolePermissions)
                {
                    string roleName = rolePermission.Key;
                    var role = new IdentityRole(roleName);

                    await roleManager.CreateAsync(role);
                    logger?.LogInformation("   -> Rol registrado: [{Nombre}]", roleName);

                    // Agregar claims (permisos) al rol
                    foreach (var permission in rolePermission.Value)
                    {
                        await roleManager.AddClaimAsync(role, new Claim("permission", permission));
                        logger?.LogInformation("      ✓ Permiso agregado: {Permiso}", permission);
                    }
                }

                logger?.LogInformation("🎉 Inyección de datos completada de manera exitosa. Se crearon {Count} roles con sus permisos respectivos.", rolePermissions.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "❌ Ocurrió un error crítico al intentar poblar los roles y permisos iniciales del sistema.");
                throw;
            }
        }
    }
}