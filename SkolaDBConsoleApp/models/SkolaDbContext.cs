using System;
using Microsoft.EntityFrameworkCore;
using SkolaDBConsoleApp.Models;

namespace SkolaDBConsoleApp
{
    public partial class SkolaDbContext : DbContext
    {
        public SkolaDbContext()
        {
        }

        public SkolaDbContext(DbContextOptions<SkolaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Betyg> Betygs { get; set; }
        public virtual DbSet<Elever> Elevers { get; set; }
        public virtual DbSet<Klasser> Klassers { get; set; }
        public virtual DbSet<Personal> Personals { get; set; }
        public virtual DbSet<Ämnen> Ämnens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SkolaDB;Username=postgres;Password=Fotboll1;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Betyg>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("betyg_pkey");

                entity.ToTable("betyg");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Betyg1)
                    .HasMaxLength(1)
                    .HasColumnName("betyg");
                entity.Property(e => e.Datum).HasColumnName("datum");
                entity.Property(e => e.Elev).HasColumnName("elev");
                entity.Property(e => e.Lärare).HasColumnName("lärare");

                entity.HasOne(d => d.ElevNavigation).WithMany(p => p.Betygs)
                    .HasForeignKey(d => d.Elev)
                    .HasConstraintName("betyg_elev_fkey");

                entity.HasOne(d => d.LärareNavigation).WithMany(p => p.Betygs)
                    .HasForeignKey(d => d.Lärare)
                    .HasConstraintName("betyg_lärare_fkey");

                entity.HasOne(d => d.ÄmneNavigation).WithMany(p => p.Betygs)
                    .HasForeignKey(d => d.Ämne)
                    .HasConstraintName("betyg_Ämne_fkey");
            });

            modelBuilder.Entity<Elever>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("elever_pkey");

                entity.ToTable("elever");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Efternamn)
                    .HasMaxLength(50)
                    .HasColumnName("efternamn");
                entity.Property(e => e.Förnamn)
                    .HasMaxLength(50)
                    .HasColumnName("förnamn");
                entity.Property(e => e.Klass).HasColumnName("klass");
                entity.Property(e => e.Personnummer)
                    .HasMaxLength(15)
                    .HasColumnName("personnummer");

                entity.HasOne(d => d.KlassNavigation).WithMany(p => p.Elevers)
                    .HasForeignKey(d => d.Klass)
                    .HasConstraintName("elever_klass_fkey");
            });

            modelBuilder.Entity<Klasser>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("klasser_pkey");

                entity.ToTable("klasser");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Mentor).HasColumnName("mentor");
                entity.Property(e => e.Namn)
                    .HasMaxLength(50)
                    .HasColumnName("namn");

                entity.HasOne(d => d.MentorNavigation).WithMany(p => p.Klassers)
                    .HasForeignKey(d => d.Mentor)
                    .HasConstraintName("klasser_mentor_fkey");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("personal_pkey");

                entity.ToTable("personal");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Befattning)
                    .HasMaxLength(50)
                    .HasColumnName("befattning");
                entity.Property(e => e.Efternamn)
                    .HasMaxLength(50)
                    .HasColumnName("efternamn");
                entity.Property(e => e.Förnamn)
                    .HasMaxLength(50)
                    .HasColumnName("förnamn");
            });

            modelBuilder.Entity<Ämnen>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Ämnen_pkey");

                entity.ToTable("Ämnen");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Namn)
                    .HasMaxLength(50)
                    .HasColumnName("namn");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
