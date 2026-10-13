using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyVaccine.WebApi.Models;

public class MyVaccineAppDbContext : IdentityDbContext<IdentityUser>
{
    public MyVaccineAppDbContext(DbContextOptions<MyVaccineAppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserAllergy> UserAllergies => Set<UserAllergy>();
    public DbSet<Dependent> Dependents => Set<Dependent>();
    public DbSet<DependentAllergy> DependentAllergies => Set<DependentAllergy>();
    public DbSet<FamilyGroup> FamilyGroups => Set<FamilyGroup>();
    public DbSet<Allergy> Allergies => Set<Allergy>();
    public DbSet<Vaccine> Vaccines => Set<Vaccine>();
    public DbSet<VaccineCategory> VaccineCategories => Set<VaccineCategory>();
    public DbSet<VaccineCategory_Vaccine> VaccineCategory_Vaccines => Set<VaccineCategory_Vaccine>();
    public DbSet<VaccineRecord> VaccineRecords => Set<VaccineRecord>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // === User Configurations ===
        builder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.FirstName)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(u => u.LastName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasOne(u => u.AspNetUser)
                  .WithMany()
                  .HasForeignKey(u => u.AspNetUserId);
        });

        // === Dependent Configurations ===
        builder.Entity<Dependent>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(d => d.BirthDate)
                  .IsRequired();

            entity.HasOne(d => d.User)
                  .WithMany(u => u.Dependents)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.FamilyGroup)
                  .WithMany(fg => fg.Dependents)
                  .HasForeignKey(d => d.FamilyGroupId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // === Vaccine Configurations ===
        builder.Entity<Vaccine>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(v => v.RequiresBooster)
                  .IsRequired();

            entity.HasIndex(v => v.Name)
                  .IsUnique();
        });

        // === VaccineCategory Configurations ===
        builder.Entity<VaccineCategory>(entity =>
        {
            entity.HasKey(vc => vc.Id);

            entity.Property(vc => vc.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(vc => vc.Description)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.HasIndex(vc => vc.Name)
                  .IsUnique();
        });

        // === VaccineCategory_Vaccine Configuration ===
        builder.Entity<VaccineCategory_Vaccine>(entity =>
        {
            entity.HasIndex(vcv => vcv.Id);

            entity.Property(vcv => vcv.RegisteredAt)
                  .IsRequired();

            // === Relationship with Vaccine ===
            entity.HasOne(vcv => vcv.Vaccine)
                  .WithMany(v => v.VaccineCategory_Vaccines)
                  .HasForeignKey(vcv => vcv.VaccineId)
                  .OnDelete(DeleteBehavior.Cascade);

            // === Relationship with VaccineCategory ===
            entity.HasOne(vcv => vcv.VaccineCategory)
                  .WithMany(vc => vc.VaccineCategory_Vaccines)
                  .HasForeignKey(vcv => vcv.VaccineCategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // === Allergy Configurations ===
        builder.Entity<Allergy>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(a => a.Name)
                  .IsUnique();
        });

        // === UserAllergy Configurations ===
        builder.Entity<UserAllergy>(entity =>
        {
            entity.HasKey(ua => ua.Id);

            entity.Property(ua => ua.RegisteredAt)
                  .IsRequired();

            // === Relationship with User ===
            entity.HasOne(ua => ua.User)
                  .WithMany(u => u.UserAllergies)
                  .HasForeignKey(ua => ua.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // === Relaltionship with Allergy ===
            entity.HasOne(ua => ua.Allergy)
                  .WithMany(a => a.UserAllergies)
                  .HasForeignKey(ua => ua.AllergyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // === DependentAllergy Configurations ===
        builder.Entity<DependentAllergy>(entity =>
        {
            entity.HasKey(da => da.Id);


            entity.Property(da => da.RegisteredAt)
                  .IsRequired();

            // === Relationship with Dependent ===
            entity.HasOne(ua => ua.Dependent)
                  .WithMany(u => u.DependentAllergies)
                  .HasForeignKey(ua => ua.DependentId)
                  .OnDelete(DeleteBehavior.Cascade);

            // === Relaltionship with Allergy ===
            entity.HasOne(ua => ua.Allergy)
                  .WithMany(a => a.DependentAllergies)
                  .HasForeignKey(ua => ua.AllergyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // === FamilyGroup Configurations ===
        builder.Entity<FamilyGroup>(entity =>
        {
            entity.HasKey(fg => fg.Id);

            entity.Property(fg => fg.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasIndex(fg => fg.Name)
                  .IsUnique();
        });

        // === VaccineRecord Configuration ===
        builder.Entity<VaccineRecord>(entity =>
        {
            entity.HasKey(vr => vr.Id);

            entity.Property(vr => vr.AdministeredDate)
                  .IsRequired();

            entity.Property(vr => vr.AdministeredLocation)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(vr => vr.AdministeredLocation)
                  .IsRequired()
                  .HasMaxLength(200);

            // === Relationship with User ===
            entity.HasOne(vr => vr.User)
                  .WithMany(u => u.VaccineRecords)
                  .HasForeignKey(vr => vr.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            // === Relationship with Dependent ===
            entity.HasOne(vr => vr.Dependent)
                  .WithMany(d => d.VaccineRecords)
                  .HasForeignKey(vr => vr.DependentId)
                  .OnDelete(DeleteBehavior.NoAction);

            // === Relationship with Vaccine ===
            entity.HasOne(vr => vr.Vaccine)
                  .WithMany(d => d.VaccineRecords)
                  .HasForeignKey(vr => vr.VaccineId)
                  .OnDelete(DeleteBehavior.NoAction);
        });
    }
}

