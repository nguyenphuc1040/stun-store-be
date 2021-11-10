using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace game_store_be.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bills> Bills { get; set; }
        public virtual DbSet<Collections> Collections { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Demo> Demo { get; set; }
        public virtual DbSet<Discounts> Discounts { get; set; }
        public virtual DbSet<GameVersions> GameVersions { get; set; }
        public virtual DbSet<Games> Games { get; set; }
        public virtual DbSet<Genres> Genres { get; set; }
        public virtual DbSet<Migrations> Migrations { get; set; }
        public virtual DbSet<PersonalAccessTokens> PersonalAccessTokens { get; set; }
        public virtual DbSet<SystemRequirements> SystemRequirements { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WishLists> WishLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=103.142.139.104;port=3307;user=root;password=my-secret-pw;database=maria", x => x.ServerVersion("10.6.4-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bills>(entity =>
            {
                entity.ToTable("bills");

                entity.HasIndex(e => e.IdGame)
                    .HasName("bills_idgame_foreign");

                entity.HasIndex(e => e.IdUser)
                    .HasName("bills_iduser_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.ActionPayment)
                    .IsRequired()
                    .HasColumnName("actionPayment")
                    .HasColumnType("enum('pay','refund')")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.AtDay)
                    .HasColumnName("at_day")
                    .HasColumnType("timestamp");

                entity.Property(e => e.CostBill)
                    .HasColumnName("costBill")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bills_idgame_foreign");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bills_iduser_foreign");
            });

            modelBuilder.Entity<Collections>(entity =>
            {
                entity.ToTable("collections");

                entity.HasIndex(e => e.IdGame)
                    .HasName("collections_idgame_foreign");

                entity.HasIndex(e => e.IdUser)
                    .HasName("collections_iduser_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.Collections)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("collections_idgame_foreign");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Collections)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("collections_iduser_foreign");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.ToTable("comments");

                entity.HasIndex(e => e.IdGame)
                    .HasName("comments_idgame_foreign");

                entity.HasIndex(e => e.IdUser)
                    .HasName("comments_iduser_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.AtDay)
                    .HasColumnName("at_day")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Dislike)
                    .HasColumnName("dislike")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Like)
                    .HasColumnName("like")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Rate)
                    .HasColumnName("rate")
                    .HasColumnType("double(8,2)");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comments_idgame_foreign");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comments_iduser_foreign");
            });

            modelBuilder.Entity<Demo>(entity =>
            {
                entity.ToTable("demo");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20)");

                entity.Property(e => e.Gamedata)
                    .HasColumnName("gamedata")
                    .HasColumnType("longtext")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_bin");

                entity.Property(e => e.Uid)
                    .HasColumnName("uid")
                    .HasColumnType("bigint(20)");
            });

            modelBuilder.Entity<Discounts>(entity =>
            {
                entity.ToTable("discounts");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.EndDate)
                    .HasColumnName("endDate")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Percent)
                    .HasColumnName("percent")
                    .HasColumnType("double(8,2)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("startDate")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<GameVersions>(entity =>
            {
                entity.ToTable("game_versions");

                entity.HasIndex(e => e.IdGame)
                    .HasName("game_versions_idgame_foreign");

                entity.HasIndex(e => e.IdGenre)
                    .HasName("game_versions_idgenre_foreign");

                entity.HasIndex(e => e.IdSystemRequirement)
                    .HasName("game_versions_idsystemrequirement_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DateUpdate)
                    .HasColumnName("dateUpdate")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.IdGenre)
                    .HasColumnName("idGenre")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.IdSystemRequirement)
                    .HasColumnName("idSystemRequirement")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.MainImage)
                    .HasColumnName("mainImage")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PrivacyPolicy)
                    .HasColumnName("privacyPolicy")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ShortDescription)
                    .HasColumnName("shortDescription")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Thumbnails)
                    .HasColumnName("thumbnails")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UrlDownload)
                    .HasColumnName("urlDownload")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.GameVersions)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("game_versions_idgame_foreign");

                //entity.HasOne(d => d.IdGenreNavigation)
                //    .WithMany(p => p.GameVersions)
                //    .HasForeignKey(d => d.IdGenre)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("game_versions_idgenre_foreign");

                entity.HasOne(d => d.IdSystemRequirementNavigation)
                    .WithMany(p => p.GameVersions)
                    .HasForeignKey(d => d.IdSystemRequirement)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("game_versions_idsystemrequirement_foreign");
            });

            modelBuilder.Entity<Games>(entity =>
            {
                entity.ToTable("games");

                entity.HasIndex(e => e.IdDiscount)
                    .HasName("games_iddiscount_foreign");

                entity.HasIndex(e => e.IdGenre)
                    .HasName("games_idgenre_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.AverageRate)
                    .HasColumnName("averageRate")
                    .HasColumnType("double(8,2)");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Developer)
                    .HasColumnName("developer")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IdDiscount)
                    .HasColumnName("idDiscount")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.IdGenre)
                    .HasColumnName("idGenre")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.LastestVersion)
                    .HasColumnName("lastestVersion")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.NumOfRate)
                    .HasColumnName("numOfRate")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Platform)
                    .HasColumnName("platform")
                    .HasColumnType("enum('OS','Window')")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Publisher)
                    .HasColumnName("publisher")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("releaseDate")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.IdDiscountNavigation)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.IdDiscount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("games_iddiscount_foreign");

                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.IdGenre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("games_idgenre_foreign");
            });

            modelBuilder.Entity<Genres>(entity =>
            {
                entity.ToTable("genres");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<Migrations>(entity =>
            {
                entity.ToTable("migrations");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Batch)
                    .HasColumnName("batch")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Migration)
                    .IsRequired()
                    .HasColumnName("migration")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");
            });

            modelBuilder.Entity<PersonalAccessTokens>(entity =>
            {
                entity.ToTable("personal_access_tokens");

                entity.HasIndex(e => e.Token)
                    .HasName("personal_access_tokens_token_unique")
                    .IsUnique();

                entity.HasIndex(e => new { e.TokenableType, e.TokenableId })
                    .HasName("personal_access_tokens_tokenable_type_tokenable_id_index");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.Abilities)
                    .HasColumnName("abilities")
                    .HasColumnType("text")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.LastUsedAt)
                    .HasColumnName("last_used_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnName("token")
                    .HasColumnType("varchar(64)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.TokenableId)
                    .HasColumnName("tokenable_id")
                    .HasColumnType("bigint(20) unsigned");

                entity.Property(e => e.TokenableType)
                    .IsRequired()
                    .HasColumnName("tokenable_type")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<SystemRequirements>(entity =>
            {
                entity.ToTable("system_requirements");

                entity.HasIndex(e => e.IdGame)
                    .HasName("system_requirements_idgame_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Graphics)
                    .HasColumnName("graphics")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Memory)
                    .HasColumnName("memory")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Processor)
                    .HasColumnName("processor")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Storage)
                    .HasColumnName("storage")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.SystemRequirements)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("system_requirements_idgame_foreign");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email)
                    .HasName("users_email_unique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.DisplayName)
                    .HasColumnName("displayName")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<WishLists>(entity =>
            {
                entity.ToTable("wish_lists");

                entity.HasIndex(e => e.IdGame)
                    .HasName("wish_lists_idgame_foreign");

                entity.HasIndex(e => e.IdUser)
                    .HasName("wish_lists_iduser_foreign");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasColumnType("int(10) unsigned");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.WishLists)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("wish_lists_idgame_foreign");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.WishLists)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("wish_lists_iduser_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
