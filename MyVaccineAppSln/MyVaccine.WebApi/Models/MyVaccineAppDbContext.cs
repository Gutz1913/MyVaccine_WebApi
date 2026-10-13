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

            entity.Property(u => u.CreatedAt)
                  .IsRequired();

            entity.Property(u => u.UpdatedAt)
                  .IsRequired(false);

            entity.HasOne(u => u.AspNetUser)
                  .WithMany()
                  .HasForeignKey(u => u.AspNetUserId);

            entity.HasOne(u => u.FamilyGroup)
                  .WithMany(fg => fg.Users)
                  .HasForeignKey(u => u.FamilyGroupId)
                  .OnDelete(DeleteBehavior.Cascade);
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

            entity.Property(d => d.CreatedAt)
                  .IsRequired();

            entity.Property(d => d.UpdatedAt)
                  .IsRequired(false);

            entity.HasOne(d => d.User)
                  .WithMany(u => u.Dependents)
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.FamilyGroup)
                  .WithMany(fg => fg.Dependents)
                  .HasForeignKey(d => d.FamilyGroupId)
                  .OnDelete(DeleteBehavior.Cascade);
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

            entity.Property(v => v.CreatedAt)
                  .IsRequired();

            entity.Property(v => v.UpdatedAt)
                  .IsRequired(false);

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

            entity.Property(vc => vc.CreatedAt)
                  .IsRequired();

            entity.Property(vc => vc.UpdatedAt)
                  .IsRequired(false);

            entity.HasIndex(vc => vc.Name)
                  .IsUnique();
        });

        // === VaccineCategory_Vaccine Configuration ===
        builder.Entity<VaccineCategory_Vaccine>(entity =>
        {
            entity.HasIndex(vcv => vcv.Id);

            entity.Property(vcv => vcv.RegisteredAt)
                  .IsRequired();

            entity.Property(vcv => vcv.CreatedAt)
                  .IsRequired();

            entity.Property(vcv => vcv.UpdatedAt)
                  .IsRequired(false);

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

            entity.HasIndex(vc => new { vc.VaccineId, vc.VaccineCategoryId })
                  .IsUnique();
        });

        // === Allergy Configurations ===
        builder.Entity<Allergy>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(a => a.CreatedAt)
                  .IsRequired();

            entity.Property(a => a.UpdatedAt)
                  .IsRequired(false);

            entity.HasIndex(a => a.Name)
                  .IsUnique();
        });

        // === UserAllergy Configurations ===
        builder.Entity<UserAllergy>(entity =>
        {
            entity.HasKey(ua => ua.Id);

            entity.Property(ua => ua.RegisteredAt)
                  .IsRequired();

            entity.Property(a => a.CreatedAt)
                  .IsRequired();

            entity.Property(a => a.UpdatedAt)
                  .IsRequired(false);

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

            entity.HasIndex(ua => new { ua.UserId, ua.AllergyId })
                  .IsUnique();
        });

        // === DependentAllergy Configurations ===
        builder.Entity<DependentAllergy>(entity =>
        {
            entity.HasKey(da => da.Id);


            entity.Property(da => da.RegisteredAt)
                  .IsRequired();

            entity.Property(da => da.CreatedAt)
                  .IsRequired();

            entity.Property(da => da.UpdatedAt)
                  .IsRequired(false);

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

            entity.HasIndex(da => new { da.DependentId, da.AllergyId })
                  .IsUnique();
        });

        // === FamilyGroup Configurations ===
        builder.Entity<FamilyGroup>(entity =>
        {
            entity.HasKey(fg => fg.Id);

            entity.Property(fg => fg.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(fg => fg.CreatedAt)
                  .IsRequired();

            entity.Property(fg => fg.UpdatedAt)
                  .IsRequired(false);

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

            entity.Property(vr => vr.CreatedAt)
                  .IsRequired();

            entity.Property(vr => vr.UpdatedAt)
                  .IsRequired(false);

            // === Relationship with User ===
            entity.HasOne(vr => vr.User)
                  .WithMany(u => u.VaccineRecords)
                  .HasForeignKey(vr => vr.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // === Relationship with Dependent ===
            entity.HasOne(vr => vr.Dependent)
                  .WithMany(d => d.VaccineRecords)
                  .HasForeignKey(vr => vr.DependentId)
                  .OnDelete(DeleteBehavior.Cascade);

            // === Relationship with Vaccine ===
            entity.HasOne(vr => vr.Vaccine)
                  .WithMany(d => d.VaccineRecords)
                  .HasForeignKey(vr => vr.VaccineId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

