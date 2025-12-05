using Audit.EntityFramework;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Persistence.Extensions;
using CleanArchitecture.Infrastructure.Shared.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : AuditIdentityDbContext<CustomIdentityUser>, IApplicationDbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public DbSet<Book> Books { get; set; }
        public DbSet<GeneralAudit> GeneralAudits { get; set; }


        public ApplicationDbContext()
            : base()
        { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        public void SoftRemove<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, IAuditable
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            entity.IsDeleted = true;
            Entry(entity).Property(p => p.IsDeleted).IsModified = true;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_dateTime != null && _authenticatedUser != null)
            {
                foreach (var entry in ChangeTracker.Entries<IAuditable>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedAt = _dateTime.NowUtc;
                            entry.Entity.CreatedBy = _authenticatedUser.UserId;
                            break;
                        case EntityState.Modified:
                            
                            if (entry.Property(nameof(IAuditable.IsDeleted)).IsModified &&
                                entry.Entity.IsDeleted)
                            {
                                // This is a soft delete - only update delete-related fields
                                entry.Entity.DeletedAt = _dateTime.NowUtc;
                                entry.Entity.DeletedBy = _authenticatedUser.UserId;

                                // Ensure delete properties are marked as modified
                                entry.Property(nameof(IAuditable.IsDeleted)).IsModified = true;
                                entry.Property(nameof(IAuditable.DeletedAt)).IsModified = true;
                                entry.Property(nameof(IAuditable.DeletedBy)).IsModified = true;
                            }
                            else
                            {
                                // This is a regular update - update modification fields
                                entry.Entity.ModifiedAt = _dateTime.NowUtc;
                                entry.Entity.ModifiedBy = _authenticatedUser.UserId;
                            }
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // apply global filters
            modelBuilder.ApplyGlobalFilters<IAuditable>(e => e.IsDeleted == false);

            //All Decimals will have 18,6 Range
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }



            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.ISBN).IsUnique();
            });
        }
    }
}
