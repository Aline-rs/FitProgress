using FitProgress.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitProgress.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PhysicalRecord> PhysicalRecords { get; set; }
        public DbSet<ProgressPhoto> ProgressPhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.HasIndex(x => x.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<PhysicalRecord>(entity =>
            {
                entity.ToTable("PhysicalRecords");

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.RecordDate)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(x => x.Weight)
                    .HasPrecision(5, 2)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.HasOne(x => x.User)
                    .WithMany(x => x.PhysicalRecords)
                    .HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<ProgressPhoto>(entity =>
            {
                entity.ToTable("ProgressPhotos");

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.ImageUrl)
                    .IsRequired();

                entity.Property(x => x.PublicId)
                    .HasMaxLength(200);

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.HasOne(x => x.User)
                    .WithMany(x => x.ProgressPhotos)
                    .HasForeignKey(x => x.UserId);

                entity.HasOne(x => x.PhysicalRecord)
                    .WithMany(x => x.ProgressPhotos)
                    .HasForeignKey(x => x.PhysicalRecordId)
                    .IsRequired(false);
            });
        }
    }
}