using GiftCardManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
               .SelectMany(t => t.GetForeignKeys())
               .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<GiftCard> GiftCard { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PromoCode> PromoCode { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TypeOfBuying> TypeOfBuying { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRefreshToken> UserRefreshToken { get; set; }
    }
}
