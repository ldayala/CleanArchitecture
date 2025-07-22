
using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Roles
{
    public sealed class Role:Enumeration<Role>
    {
        public static readonly Role Admin = new(1, "Admin");
        public static readonly Role Cliente = new(2, "User");
        

        private Role(int id, string name) : base(id, name)
        {
        }
       
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
    
}
