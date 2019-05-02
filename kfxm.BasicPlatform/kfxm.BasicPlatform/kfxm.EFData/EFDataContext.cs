using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.Composition;
using kfxms.Entity;

namespace kfxms.EFData
{
    [Export("DefaultData")]
    public partial class EFDataContext : DbContext,IDisposable
    {
        
        public EFDataContext()
            : base("default")
        {

        }
        public EFDataContext(String connectionString)
            : base(connectionString)
        {

        }

        public DbSet< kfxms.Entity.SysBasic.Sys_User> Users { get; set; }

        public DbSet<kfxms.Entity.SysBasic.Sys_Role> Roles { get; set; }

        public DbSet<kfxms.Entity.SysBasic.Sys_UserAndRole> UserAndRoles { get; set; }
        
        public DbSet<kfxms.Entity.SysBasic.Sys_Menu> Menus { get; set; }

        public DbSet<kfxms.Entity.SysBasic.Sys_MenuAuthority> MenuAuthoritys { get; set; }

        public DbSet<kfxms.Entity.SysBasic.Sys_AuthorityRequest> AuthorityRequests { get; set; }

        public DbSet<kfxms.Entity.SysBasic.Sys_RoleAndAuthority> RoleAndAuthoritys { get; set; }

        public DbSet<kfxms.Entity.SysBasic.Sys_Department> Departments { get; set; }

        public DbSet<kfxms.Entity.SysBasic.Sys_Dictionary> Dictionarys { get; set; }

        public DbSet<kfxms.Entity.Supplier.S_Supplier> Supplier { get; set; }

        public DbSet<kfxms.Entity.SupplierType.S_SupplierType> SupplierType { get; set; }

        public DbSet<kfxms.Entity.Supplier.S_SupplierHasTypeName> SupplierHasTypeName { get; set; }

        public DbSet<kfxms.Entity.Client.S_Client> Client { get; set; }

        public DbSet<kfxms.Entity.Project.S_Project> Project { get; set; }

        public DbSet<kfxms.Entity.Project.S_ProjectInfo> ProjectInfo { get; set; }

        public DbSet<kfxms.Entity.Revenue.S_Revenue> Revenue { get; set; }

        public DbSet<kfxms.Entity.Payment.S_PublicRelations> PublicRelations { get; set; }

        public DbSet<kfxms.Entity.Payment.S_InternalPayment> InternalPayment { get; set; }

        public DbSet<kfxms.Entity.Project.S_ProjectAndSupplier> ProjectAndSupplier { get; set; }

        public DbSet<kfxms.Entity.Invoice.S_Invoice> Invoice { get; set; }
    }
}
