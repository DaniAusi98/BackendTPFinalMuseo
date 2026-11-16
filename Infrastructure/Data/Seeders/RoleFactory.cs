
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infrastructure.Data.Seeders
{
    public static class RolePermissionsSeeder
    {
        private const string PermissionClaimType = "permission";

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("RolePermissionsSeeder");

            try
            {
                logger.LogInformation(
                    "Verificando roles y permisos del sistema...");

                var rolePermissions = RolePermissionsConfig.RolePermissions;

                foreach (var rolePermission in rolePermissions)
                {
                    string roleName = rolePermission.Key;

                    // 1. Buscar el rol
                    var role = await roleManager.FindByNameAsync(roleName);

                    // 2. Crear el rol si no existe
                    if (role == null)
                    {
                        role = new IdentityRole(roleName);

                        var createResult = await roleManager.CreateAsync(role);

                        if (!createResult.Succeeded)
                        {
                            throw new InvalidOperationException(
                                $"Error al crear el rol {roleName}: " +
                                string.Join(", ", createResult.Errors.Select(e => e.Description)));
                        }

                        logger.LogInformation(
                            "Rol creado: {RoleName}", roleName);
                    }

                    // 3. Obtener los permisos configurados
                    var permisosConfigurados = rolePermission.Value
                        .Distinct()
                        .ToHashSet();

                    // 4. Obtener los claims actuales del rol
                    var claimsActuales = await roleManager.GetClaimsAsync(role);

                    var permisosActuales = claimsActuales
                        .Where(c => c.Type == PermissionClaimType)
                        .Select(c => c.Value)
                        .ToHashSet();

                    // 5. Agregar permisos que todavía no existen
                    foreach (var permission in permisosConfigurados)
                    {
                        if (permisosActuales.Contains(permission))
                            continue;

                        var addResult = await roleManager.AddClaimAsync(
                            role,
                            new Claim(PermissionClaimType, permission));

                        if (!addResult.Succeeded)
                        {
                            throw new InvalidOperationException(
                                $"Error al agregar el permiso {permission} " +
                                $"al rol {roleName}: " +
                                string.Join(", ", addResult.Errors.Select(e => e.Description)));
                        }

                        logger.LogInformation(
                            "Permiso agregado: {Permission} -> {RoleName}",
                            permission, roleName);
                    }

                    // 6. Eliminar permisos que ya no están configurados
                    var permisosObsoletos = claimsActuales
                        .Where(c =>
                            c.Type == PermissionClaimType &&
                            !permisosConfigurados.Contains(c.Value))
                        .ToList();

                    foreach (var claim in permisosObsoletos)
                    {
                        var removeResult = await roleManager.RemoveClaimAsync(
                            role,
                            claim);

                        if (!removeResult.Succeeded)
                        {
                            throw new InvalidOperationException(
                                $"Error al eliminar el permiso {claim.Value} " +
                                $"del rol {roleName}: " +
                                string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                        }

                        logger.LogInformation(
                            "Permiso eliminado: {Permission} -> {RoleName}",
                            claim.Value, roleName);
                    }
                }

                logger.LogInformation(
                    "Sincronización de roles y permisos completada. " +
                    "Se procesaron {Count} roles.",
                    rolePermissions.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error al sincronizar los roles y permisos del sistema.");

                throw;
            }
        }
    }
}
