using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Domain.Entities;
using UserManagement.Domain.ValueObject;

namespace UserManagement.Infrastructure.Data
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options):base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("Users");

                builder.HasKey(u => u.Id);

                // Map Email value object as owned/complex type
                builder.OwnsOne(u => u.Email, emailBuilder =>
                {
                    // Use a distinct column name to avoid name collisions between navigation and column
                    
                    emailBuilder.Property(e => e.Value)
                        .HasColumnName("EmailValue")
                        .IsRequired();

                    // Add unique index on the owned property's Value (will be created on owner table)
                    emailBuilder.HasIndex(e => e.Value)
                                .IsUnique()
                                .HasDatabaseName("IX_Users_Email");

                    // If you want the value object properties stored in same table (default), no further config needed
                });

                builder.Property(u => u.PasswordHash).HasColumnName("PasswordHash").IsRequired();
                builder.Property(u => u.FullName).HasMaxLength(200);
                builder.Property(u => u.Role).HasMaxLength(100);
                builder.Property(u => u.CreatedAt).IsRequired();
                builder.Property(u => u.IsActive).IsRequired();
            });
        }

    }
}
