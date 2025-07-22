
using CleanArchitecture.Domain.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace CleanArchitecture.Infrastructure.Authentication
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        /*
         En este constructor se transforma el enum PermissionEnum a string, lo quue hacemos es registrar este string en nuestra
        aplicacion .net como un policy de autorizacion(esto lo hace la clase ) , luego en el handler de autorizacion PermissionAutorizacionHandler.cs 

         */
        public HasPermissionAttribute(PermissionEnum permission) : base(policy: permission.ToString())
        {

        }
    }
}