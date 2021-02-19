using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ShopBot
{
    public partial class RST_DBContext : DbContext
    {
        public RST_DBContext()
        {
        }

        public RST_DBContext(DbContextOptions<RST_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminInfo> AdminInfos { get; set; }
        public virtual DbSet<FailedLogin> FailedLogins { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<UserLoginInfo> UserLoginInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.1;user id=root;password=root;port=3306;database=RST_DB", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.23-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminInfo>(entity =>
            {
                entity.HasKey(e => e.AdminEmail)
                    .HasName("PRIMARY");

                entity.ToTable("admin_info");

                entity.Property(e => e.AdminEmail)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("Admin_email")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.AdminPosition)
                    .IsRequired()
                    .HasColumnType("varchar(25)")
                    .HasColumnName("Admin_position")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<FailedLogin>(entity =>
            {
                entity.HasKey(e => e.UserEmail)
                    .HasName("PRIMARY");

                entity.ToTable("failed_login");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("user_email")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.FailedNum)
                    .HasColumnName("failed_num")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TimeOfTry)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("Time_of_try");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleIdn)
                    .HasName("PRIMARY");

                entity.ToTable("schedules");

                entity.Property(e => e.ScheduleIdn)
                    .ValueGeneratedNever()
                    .IsRequired()
                    .HasColumnName("schedule_IDN")
                    .HasColumnType("integer");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("user_email")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Archived)
                    .HasColumnType("bit(1)")
                    .HasColumnName("archived")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasColumnName("url")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.IsRecurring)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_recurring")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Frequency)
                    .HasColumnType("integer")
                    .HasColumnName("frequency")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.WantOption)
                    .HasColumnType("integer")
                    .HasColumnName("want_option")
                    .HasDefaultValueSql("'-1'");

                entity.Property(e => e.PriceLimit)
                    .HasColumnType("integer")
                    .HasColumnName("price_limit");
                    
                entity.Property(e => e.ExpireDate)
                    .HasColumnType("date")
                    .HasColumnName("expire_date");

                entity.Property(e => e.CurrentPrice)
                    .IsRequired()
                    .HasColumnType("float")
                    .HasColumnName("current_price");

                entity.Property(e => e.Item)
                    .IsRequired()
                    .HasColumnType("varchar(200)")
                    .HasColumnName("item")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<UserLoginInfo>(entity =>
            {
                entity.HasKey(e => e.UserEmail)
                    .HasName("PRIMARY");

                entity.ToTable("user_login_info");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasColumnName("user_email")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Verify)
                    .IsRequired()
                    .HasColumnType("varchar(12)")
                    .HasColumnName("verify")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasColumnType("varchar(65)")
                    .HasColumnName("salt")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
                
                entity.Property(e => e.EmailVerified)
                    .HasColumnType("bit(1)")
                    .HasColumnName("emailVerified")
                    .HasDefaultValueSql("b'0'");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
