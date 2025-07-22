using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CleanArchitecture.Infrastructure.Authentication
{
    public class PermissionAutorizacionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //aqui es donde evaluamos el token y los permisos
            string? userId = context.User.Claims.FirstOrDefault(
                    x => x.Type == ClaimTypes.NameIdentifier
                    )?.Value;

            if (userId is null)
            {
                return Task.CompletedTask;
            }
            //obtenemos los permisos del usuario desde el token
            HashSet<string> permissions = context.User.Claims
                 .Where(x => x.Type == CustomClaims.Permissions)
                 .Select(x => x.Value).ToHashSet();

            if (permissions.Any())
            {
                //verificamos si el usuario tiene el permiso requerido
                if (permissions.Contains(requirement.Permission.ToString()))
                {
                    context.Succeed(requirement);
                }
            }
           
            return Task.CompletedTask;
        }
    }
}
