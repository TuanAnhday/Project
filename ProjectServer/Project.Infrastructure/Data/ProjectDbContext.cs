using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Project.Infrastructure.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Infrastructure.Data
{
    public class ProjectDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectDbContext(
            DbContextOptions<ProjectDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<UseDataModel> Users { get; set; }

        private void UpdateTimeStamp(IEnumerable<EntityEntry> entities)
        {
            var now = DateTime.Now;
            Guid? currentUserId = null;
            if(_httpContextAccessor.HttpContext != null)
            {
                try
                {
                    currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub));
                }
                catch 
                {
                    //
                }
            }

            foreach(var changeEntity in ChangeTracker.Entries())
            {
                if(changeEntity.Entity is DataModel entity)
                {
                    switch(changeEntity.State){
                        case EntityState.Added:
                            entity.CreatedDate = now;
                            entity.ModifiedDate = now;
                            entity.CreatedBy = currentUserId;
                            entity.ModifiedBy = currentUserId;
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                            Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                            entity.ModifiedDate = now;
                            entity.ModifiedBy= currentUserId;
                            break ;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureProject();
        }

        public override int SaveChanges()
        {
            UpdateTimeStamp(ChangeTracker.Entries());
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateTimeStamp(ChangeTracker.Entries());
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateTimeStamp(ChangeTracker.Entries());
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimeStamp(ChangeTracker.Entries());
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
