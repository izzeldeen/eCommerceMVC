using eCommerce.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data
{
    public class eCommerceContext : IdentityDbContext<eCommerceUser>
    {
        public eCommerceContext() : base("name=eCommerceConnectionString-OK")
        {
            Database.SetInitializer<eCommerceContext>(new eCommerceDBInitializer());
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
            
            modelBuilder.Entity<Promo>()
                .HasIndex(p => new { p.Code })
                .IsUnique(true);

            modelBuilder.Entity<eCommerceUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promo> Promos { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<ProductCost> ProductCosts { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<CountryCode> CountryCodes { get; set; }
        public DbSet<Payment> Payment { get; set; }

        public DbSet<IdentityUserRole> UserRoles { get; set; }


        public static eCommerceContext Create()
        {
            return new eCommerceContext();
        }
    }
}
