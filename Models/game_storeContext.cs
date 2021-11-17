using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace game_store_be.Models
{
    public partial class game_storeContext : DbContext
    {
        public game_storeContext()
        {
        }

        public game_storeContext(DbContextOptions<game_storeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<DetailGenre> DetailGenre { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<GameVersion> GameVersion { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<ImageGameDetail> ImageGameDetail { get; set; }
        public virtual DbSet<SlideGameHot> SlideGameHot { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WishList> WishList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=103.142.139.104;Database=game_store;user=sa;password=khai12345@");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => e.IdBill)
                    .HasName("pk_Bill");

                entity.Property(e => e.IdBill)
                    .HasColumnName("idBill")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");

                entity.Property(e => e.Actions)
                    .HasColumnName("actions")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.DatePay)
                    .HasColumnName("datePay")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");

                entity.Property(e => e.Discount)
                    .HasColumnName("discount")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnType("varchar(100)");


                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.IdGame)
                    .HasConstraintName("fk_Bill_Game");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Bill)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("fk_Bill_Users");
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.HasKey(e => new { e.IdGame, e.IdUser })
                    .HasName("pk_Collection");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.IsIntalled).HasColumnName("isIntalled");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.Collection)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Collection_Game");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Collection)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Collection_Users");
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(e => e.IdComment)
                    .HasName("pk_Comments");

                entity.Property(e => e.IdComment)
                    .HasColumnName("idComment")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("text");

                entity.Property(e => e.Dislike).HasColumnName("dislike");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.Likes).HasColumnName("likes");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdGame)
                    .HasConstraintName("fk_Comments_Game");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("fk_Comments_Users");
            });

            modelBuilder.Entity<DetailGenre>(entity =>
            {
                entity.HasKey(e => new { e.IdGenre, e.IdGame })
                    .HasName("pk_DetailGenre");

                entity.Property(e => e.IdGenre)
                    .HasColumnName("idGenre")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.DetailGenre)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_DetailGenre_Game");

                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany(p => p.DetailGenre)
                    .HasForeignKey(d => d.IdGenre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_DetailGenre_Genre");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(e => e.IdDiscount)
                    .HasName("pk_Discount");

                entity.Property(e => e.IdDiscount)
                    .HasColumnName("idDiscount")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.EndDate)
                    .HasColumnName("endDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.PercentDiscount).HasColumnName("percentDiscount");

                entity.Property(e => e.StartDate)
                    .HasColumnName("startDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.IdGame)
                    .HasName("pk_Game");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.AverageRate).HasColumnName("averageRate");

                entity.Property(e => e.Cost).HasColumnName("cost");

                entity.Property(e => e.Developer)
                    .HasColumnName("developer")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.IdDiscount)
                    .HasColumnName("idDiscount")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.LastestVersion)
                    .HasColumnName("lastestVersion")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NameGame)
                    .HasColumnName("nameGame")
                    .HasMaxLength(50);

                entity.Property(e => e.NumOfRate).HasColumnName("numOfRate");

                entity.Property(e => e.NumberOfBuyer).HasColumnName("numberOfBuyer");

                entity.Property(e => e.NumberOfDownloaders).HasColumnName("numberOfDownloaders");

                entity.Property(e => e.Plaform)
                    .HasColumnName("plaform")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Publisher)
                    .HasColumnName("publisher")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ReleaseDate)
                    .HasColumnName("releaseDate")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdDiscountNavigation)
                    .WithMany(p => p.Game)
                    .HasForeignKey(d => d.IdDiscount)
                    .HasConstraintName("fk_Game_Discount");

            });


            modelBuilder.Entity<GameVersion>(entity =>
            {
                entity.HasKey(e => e.IdGameVersion)
                    .HasName("pk_GameVersion");

                entity.Property(e => e.IdGameVersion)
                    .HasColumnName("idGameVersion")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.DateUpdate)
                    .HasColumnName("dateUpdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Descriptions)
                    .HasColumnName("descriptions")
                    .HasColumnType("text");

                entity.Property(e => e.DirectX)
                    .HasColumnName("directX")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Graphics)
                    .HasColumnName("graphics")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.Os)
                    .HasColumnName("os")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PrivacyPolicy)
                    .HasColumnName("privacyPolicy")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Processor)
                    .HasColumnName("processor")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Requires)
                    .HasColumnName("requires")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ShortDescription)
                    .HasColumnName("shortDescription")
                    .HasColumnType("text");

                entity.Property(e => e.Storage)
                    .HasColumnName("storage")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UrlDowload)
                    .HasColumnName("urlDowload")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VersionGame)
                    .HasColumnName("versionGame")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.GameVersion)
                    .HasForeignKey(d => d.IdGame)
                    .HasConstraintName("fk_GameVersion_Game");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.IdGenre)
                    .HasName("pk_Genre");

                entity.Property(e => e.IdGenre)
                    .HasColumnName("idGenre")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.NameGenre)
                    .HasColumnName("nameGenre")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ImageGameDetail>(entity =>
            {
                entity.HasKey(e => e.IdImage)
                    .HasName("pk_ImageGameDetail");

                entity.Property(e => e.IdImage)
                    .HasColumnName("idImage")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.ImageGameDetail)
                    .HasForeignKey(d => d.IdGame)
                    .HasConstraintName("fk_ImageDetail_Game");
            });

            modelBuilder.Entity<SlideGameHot>(entity =>
            {
                entity.HasKey(e => e.IdGame)
                    .HasName("pk_SlideGameHot");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.UrlVideo)
                    .HasColumnName("urlVideo")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdGameNavigation)
                    .WithOne(p => p.SlideGameHot)
                    .HasForeignKey<SlideGameHot>(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SlideGameHot_Game");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("pk_Users");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__Users__AB6E61644D336FD2")
                    .IsUnique();

                entity.HasIndex(e => e.NumberPhone)
                    .HasName("UQ__Users__4A5B65A81FE2B8C4")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__Users__66DCF95C36D1E809")
                    .IsUnique();

                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.Avatar)
                    .HasColumnName("avatar")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Background)
                    .HasColumnName("background")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumberPhone)
                    .HasColumnName("numberPhone")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RealName)
                    .HasColumnName("realName")
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WishList>(entity =>
            {
                entity.HasKey(e => new { e.IdGame, e.IdUser })
                    .HasName("pk_WishList");

                entity.Property(e => e.IdGame)
                    .HasColumnName("idGame")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.Property(e => e.IdUser)
                    .HasColumnName("idUser")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnType("varchar(50) unsigned");


                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_WishList_Game");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.WishList)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_WishList_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
