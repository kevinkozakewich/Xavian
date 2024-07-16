using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xavian.DataContext.Models;

namespace Xavian.DataContext
{
    public partial class XavianDbContext : DbContext
    {
        public XavianDbContext(DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        public XavianDbContext() : base(new DbContextOptionsBuilder<XavianDbContext>().Options)
        {
        }

        public virtual DbSet<AppVersion> AppVersions { get; set; }

        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<Site> Sites { get; set; }

        public virtual DbSet<SitePermission> SitePermissions { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<TeamUser> TeamUsers { get; set; }

        public virtual DbSet<Territory> Territories { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.LastUpdatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(100);

                entity.HasOne<Territory>()
                    .WithMany()
                    .HasForeignKey(d => d.TerritoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.LastUpdatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SitePermission>(entity =>
            {
                entity.HasIndex(e => new { e.TeamId, e.SiteId }).IsUnique();

                entity.HasOne<Site>()
                    .WithMany()
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Team>()
                    .WithMany()
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.LastUpdatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasIndex(e => new { e.OwnerUserId, e.Name, }).IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(100);
                
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.LastUpdatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TeamUser>(entity =>
            {
                entity.HasIndex(e => new { e.TeamId, e.UserId, }).IsUnique();

                entity.HasOne<Team>()
                    .WithMany()
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.LastUpdatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Email)
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .HasMaxLength(100);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.LastUpdatedUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(d => d.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Territory>().HasData(
                new Territory 
                {
                    Id = 1,
                    Abbreviation = "CCA", 
                    FullName = "Central Canada",
                    Deleted = false
                },
                new Territory 
                {
                    Id = 2,
                    Abbreviation = "ECA", 
                    FullName = "Eastern Canada",
                    Deleted = false
                },
                new Territory 
                {
                    Id = 3,
                    Abbreviation = "WCA", 
                    FullName = "Western Canada",
                    Deleted = false
                },
                new Territory 
                {
                    Id = 4,
                    Abbreviation = "USA", 
                    FullName = "USA",
                    Deleted = false
                },
                new Territory 
                {
                    Id = 5,
                    Abbreviation = "OTHER", 
                    FullName = "Other",
                    Deleted = false
                }
            );

            modelBuilder.Entity<AppVersion>().HasData(
                new AppVersion
                {
                    Id = 1,
                    Version = 1
                }
            );
        }

        public class XavianDbContextFactory : IDesignTimeDbContextFactory<XavianDbContext>
        {
            public XavianDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (connectionString == null)
                {
                    throw new InvalidOperationException("Failed to load the connection string.");
                }

                var optionsBuilder = new DbContextOptionsBuilder<XavianDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                return new XavianDbContext(optionsBuilder.Options);
            }

        }
    }
}
