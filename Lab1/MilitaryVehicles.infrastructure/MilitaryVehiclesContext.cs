using Microsoft.EntityFrameworkCore;
using MilitaryVehicles.infrastructure.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MilitaryVehicles.infrastructure
{
    public class MilitaryVehiclesContext : DbContext
    {
        //Конструктор за замовчуванням
        public MilitaryVehiclesContext() { }
        //Конструктор — приймає параметри конфігурації (рядок підключення і т.д.)
        public MilitaryVehiclesContext(DbContextOptions<MilitaryVehiclesContext> options)
            : base(options) {}

        //DbSet — колекції сутностей (таблиці в базі даних)
        public DbSet<ArmyModel> Armies { get; set; }
        public DbSet<CrewMemberModel> CrewMembers { get; set; }
        public DbSet<MilitaryVehicleModel> MilitaryVehicles { get; set; }
        public DbSet<GroundVehicleModel> GroundVehicles { get; set; }
        public DbSet<AirVehicleModel> AirVehicles { get; set; }
        public DbSet<SeaVehicleModel> SeaVehicles { get; set; }
        public DbSet<TankModel> Tanks { get; set; }
        public DbSet<HelicopterModel> Helicopters { get; set; }
        public DbSet<DestroyerModel> Destroyers { get; set; }

        //Налаштування схеми бази даних через Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Table-per-Type (TPT) наслідування
            modelBuilder.Entity<MilitaryVehicleModel>().ToTable("MilitaryVehicles");
            modelBuilder.Entity<GroundVehicleModel>().ToTable("GroundVehicles");
            modelBuilder.Entity<AirVehicleModel>().ToTable("AirVehicles");
            modelBuilder.Entity<SeaVehicleModel>().ToTable("SeaVehicles");
            modelBuilder.Entity<TankModel>().ToTable("Tanks");
            modelBuilder.Entity<HelicopterModel>().ToTable("Helicopters");
            modelBuilder.Entity<DestroyerModel>().ToTable("Destroyers");

            //Один-до-багатьох: Army → Vehicles
            modelBuilder.Entity<ArmyModel>()
                .HasMany(a => a.Vehicles)
                .WithOne(v => v.Army)
                .HasForeignKey(v => v.ArmyId)
                .OnDelete(DeleteBehavior.Cascade);

            //Багато-до-багатьох: Vehicles ↔ CrewMembers
            modelBuilder.Entity<MilitaryVehicleModel>()
                .HasMany(v => v.CrewMembers)
                .WithMany(c => c.Vehicles)
                .UsingEntity<Dictionary<string, object>>(
                    "VehicleCrewMember", // назва таблиці зв’язку
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

            //Додаткові обмеження та властивості
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