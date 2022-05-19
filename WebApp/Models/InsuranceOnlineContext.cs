using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApp.Models
{
    public partial class InsuranceOnlineContext : DbContext
    {
        public InsuranceOnlineContext()
        {
        }

        public InsuranceOnlineContext(DbContextOptions<InsuranceOnlineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Faq> Faqs { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<InsuarancePolicy> InsuarancePolicies { get; set; } = null!;
        public virtual DbSet<Insurance> Insurances { get; set; } = null!;
        public virtual DbSet<InsurancePoly> InsurancePolies { get; set; } = null!;
        public virtual DbSet<Loan> Loans { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<PaymentDetail> PaymentDetails { get; set; } = null!;
        public virtual DbSet<PeriodicPaymentMethod> PeriodicPaymentMethods { get; set; } = null!;
        public virtual DbSet<Policy> Policies { get; set; } = null!;
        public virtual DbSet<Reply> Replies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("Context");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder,string _c)
        {
            optionsBuilder.UseSqlServer(_c);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_at");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Beneficiary).HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.ExpriredAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Exprired_at");

                entity.Property(e => e.InsuranceId).HasColumnName("InsuranceID");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Contracts_Customers");

                entity.HasOne(d => d.Insurance)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.InsuranceId)
                    .HasConstraintName("FK_Contracts_Insurances");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_at");

                entity.Property(e => e.Username).HasMaxLength(255);
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("FAQs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Question).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_at");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.FeedbackContent).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_at");
            });

            modelBuilder.Entity<InsuarancePolicy>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.InsuarenceId).HasColumnName("InsuarenceID");

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.HasOne(d => d.Insuarence)
                    .WithMany(p => p.InsuarancePolicies)
                    .HasForeignKey(d => d.InsuarenceId)
                    .HasConstraintName("FK_InsuarancePolicies_Insurances");

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.InsuarancePolicies)
                    .HasForeignKey(d => d.PolicyId)
                    .HasConstraintName("FK_InsuarancePolicies_Policies");
            });

            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.InsuredObject).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SumInsured).HasColumnType("money");

                entity.Property(e => e.TotalFee).HasColumnType("money");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_at");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Insurances)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Insurances_Categories");
            });

            modelBuilder.Entity<InsurancePoly>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("date")
                    .HasColumnName("Created_at");

                entity.Property(e => e.InsuranceId).HasColumnName("InsuranceID");

                entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

                entity.HasOne(d => d.Insurance)
                    .WithMany(p => p.InsurancePolies)
                    .HasForeignKey(d => d.InsuranceId)
                    .HasConstraintName("FK_InsurancePolies_Insurances");

                entity.HasOne(d => d.Policy)
                    .WithMany(p => p.InsurancePolies)
                    .HasForeignKey(d => d.PolicyId)
                    .HasConstraintName("FK_InsurancePolies_Policies");
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.ExpiredAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Expired_at");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Loans_Customers");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK_Payments_Contracts");
            });

            modelBuilder.Entity<PaymentDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmountOfChange).HasColumnType("money");

                entity.Property(e => e.ContentDetails)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.PaidAmount).HasColumnType("money");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.PeriodicId).HasColumnName("PeriodicID");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_at");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.PaymentDetails)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_PaymentDetails_Payments");

                entity.HasOne(d => d.Periodic)
                    .WithMany(p => p.PaymentDetails)
                    .HasForeignKey(d => d.PeriodicId)
                    .HasConstraintName("FK_PaymentDetails_PeriodicPaymentMethods");
            });

            modelBuilder.Entity<PeriodicPaymentMethod>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_at");
            });

            modelBuilder.Entity<Reply>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Answer)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_at");

                entity.Property(e => e.FaqsId).HasColumnName("FAQsID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("Updated_at");

                entity.HasOne(d => d.Faqs)
                    .WithMany(p => p.Replies)
                    .HasForeignKey(d => d.FaqsId)
                    .HasConstraintName("FK_Replies_FAQs");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
