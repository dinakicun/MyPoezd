using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyPoezd.Models;
using Route = MyPoezd.Models.Route;
using Type = MyPoezd.Models.Type;

namespace MyPoezd;

public partial class MyTrainContext : DbContext
{
    public MyTrainContext()
    {
    }

    public MyTrainContext(DbContextOptions<MyTrainContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Train> Trains { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wagon> Wagons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyTrain;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Places)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Places_Users1");

            entity.HasOne(d => d.Wagon).WithMany(p => p.Places)
                .HasForeignKey(d => d.WagonId)
                .HasConstraintName("FK_Places_Wagons");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.Property(e => e.PriceCoupe).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PriceEconom).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ArrivalCity).WithMany(p => p.RouteArrivalCities)
                .HasForeignKey(d => d.ArrivalCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_Cities");

            entity.HasOne(d => d.DepartureCity).WithMany(p => p.RouteDepartureCities)
                .HasForeignKey(d => d.DepartureCityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_DepartureCities");

            entity.HasOne(d => d.Trains).WithMany(p => p.Routes)
                .HasForeignKey(d => d.TrainsId)
                .HasConstraintName("FK_Routes_Trains1");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("UpdatePlaces"));

            entity.HasOne(d => d.Place).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.PlaceId)
                .HasConstraintName("FK_Tickets_Places");

            entity.HasOne(d => d.Route).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets_Routes");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Users");

            entity.HasOne(d => d.Wagon).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.WagonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Wagons");
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.NumberPhone).HasMaxLength(11);
            entity.Property(e => e.PassportData).HasMaxLength(10);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Role1");
        });

        modelBuilder.Entity<Wagon>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("InsertPlaces"));

            entity.Property(e => e.Count).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Trains).WithMany(p => p.Wagons)
                .HasForeignKey(d => d.TrainsId)
                .HasConstraintName("FK_Wagons_Trains");

            entity.HasOne(d => d.Type).WithMany(p => p.Wagons)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Wagons_Types1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
