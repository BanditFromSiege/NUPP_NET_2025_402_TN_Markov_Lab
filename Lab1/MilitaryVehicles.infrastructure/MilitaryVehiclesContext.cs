using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.infrastructure.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MilitaryVehicles.infrastructure
{
    public class MilitaryVehiclesContext : IdentityDbContext<ApplicationUser>
    {
        public MilitaryVehiclesContext() { }

        public MilitaryVehiclesContext(DbContextOptions<MilitaryVehiclesContext> options)
            : base(options) { }

        public DbSet<ArmyModel> Armies { get; set; }
        public DbSet<CrewMemberModel> CrewMembers { get; set; }
        public DbSet<MilitaryVehicleModel> MilitaryVehicles { get; set; }

        public DbSet<GroundVehicleModel> GroundVehicles { get; set; }
        public DbSet<AirVehicleModel> AirVehicles { get; set; }
        public DbSet<SeaVehicleModel> SeaVehicles { get; set; }

        public DbSet<TankModel> Tanks { get; set; }
        public DbSet<HelicopterModel> Helicopters { get; set; }
        public DbSet<DestroyerModel> Destroyers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MilitaryVehicleModel>().ToTable("MilitaryVehicles");
            modelBuilder.Entity<GroundVehicleModel>().ToTable("GroundVehicles");
            modelBuilder.Entity<AirVehicleModel>().ToTable("AirVehicles");
            modelBuilder.Entity<SeaVehicleModel>().ToTable("SeaVehicles");

            modelBuilder.Entity<TankModel>().ToTable("Tanks");
            modelBuilder.Entity<HelicopterModel>().ToTable("Helicopters");
            modelBuilder.Entity<DestroyerModel>().ToTable("Destroyers");

            modelBuilder.Entity<ArmyModel>()
                .HasMany(a => a.Vehicles)
                .WithOne(v => v.Army)
                .HasForeignKey(v => v.ArmyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MilitaryVehicleModel>()
                .HasMany(v => v.CrewMembers)
                .WithMany(c => c.Vehicles)
                .UsingEntity<Dictionary<string, object>>(
                    "VehicleCrewMember",
                    join => join
                        .HasOne<CrewMemberModel>()
                        .WithMany()
                        .HasForeignKey("CrewMemberId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join => join
                        .HasOne<MilitaryVehicleModel>()
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.HasKey("VehicleId", "CrewMemberId");
                        join.ToTable("VehicleCrewMembers");
                    });

            modelBuilder.Entity<ArmyModel>(entity =>
            {
                entity.Property(a => a.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<CrewMemberModel>(entity =>
            {
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.Rank)
                      .IsRequired()
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<MilitaryVehicleModel>(entity =>
            {
                entity.Property(v => v.Model)
                      .IsRequired()
                      .HasMaxLength(100);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=MilitaryVehiclesDB.sqlite");
            }
        }
    }
}