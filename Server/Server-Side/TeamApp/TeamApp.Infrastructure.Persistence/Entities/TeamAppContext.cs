using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace TeamApp.Infrastructure.Persistence.Entities
{
    public partial class TeamAppContext : IdentityDbContext<User>
    {
        public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder => { builder.AddConsole(); });
        public TeamAppContext()
        {

        }

        public TeamAppContext(DbContextOptions<TeamAppContext> options)
            : base(options)
        {

        }


        public virtual DbSet<GroupChat> GroupChat { get; set; }
        public virtual DbSet<GroupChatUser> GroupChatUser { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserConnection> UserConnection { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("refresh_token");
            });

            modelBuilder.Entity<UserConnection>(entity =>
            {
                entity.ToTable("user_connection");
                entity.HasKey(e => new { e.UserId, e.ConnectionId });
                entity.Property(e => e.ConnectionId)
                    .HasColumnName("user_connection_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");
            });            

            modelBuilder.Entity<GroupChat>(entity =>
            {
                entity.ToTable("group_chat");

                entity.Property(e => e.GroupChatId)
                    .HasColumnName("group_chat_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.GroupChatImageUrl)
                    .HasColumnName("group_chat_imageurl")
                    .HasColumnType("varchar(500)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.GroupChatName)
                    .HasColumnName("group_chat_name")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");
            });

            modelBuilder.Entity<GroupChatUser>(entity =>
            {
                entity.ToTable("group_chat_user");

                entity.HasIndex(e => e.GroupChatUserGroupChatId)
                    .HasName("group_chat_user_group_chat_id");

                entity.HasIndex(e => e.GroupChatUserUserId)
                    .HasName("group_chat_user_user_id");

                entity.Property(e => e.GroupChatUserId)
                    .HasColumnName("group_chat_user_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.GroupChatUserGroupChatId)
                    .HasColumnName("group_chat_user_group_chat_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                 entity.Property(e => e.GroupChatUserUserId)
                    .HasColumnName("group_chat_user_user_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.HasOne(d => d.GroupChatUserGroupChat)
                    .WithMany(p => p.GroupChatUser)
                    .HasForeignKey(d => d.GroupChatUserGroupChatId)
                    .HasConstraintName("group_chat_user_ibfk_2");

                entity.HasOne(d => d.GroupChatUserUser)
                    .WithMany(p => p.GroupChatUser)
                    .HasForeignKey(d => d.GroupChatUserUserId)
                    .HasConstraintName("group_chat_user_ibfk_1");
            });

               modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("message");

                entity.HasIndex(e => e.MessageGroupChatId)
                    .HasName("message_group_chat_id");

                entity.HasIndex(e => e.MessageUserId)
                    .HasName("message_user_id");

                entity.Property(e => e.MessageId)
                    .HasColumnName("message_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.MessageContent)
                    .HasColumnName("message_content")
                    .HasColumnType("text")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.MessageCreatedAt)
                    .HasColumnName("message_created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.MessageGroupChatId)
                    .HasColumnName("message_group_chat_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.MessageUserId)
                    .HasColumnName("message_user_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.HasOne(d => d.MessageGroupChat)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.MessageGroupChatId)
                    .HasConstraintName("message_ibfk_2");

                entity.HasOne(d => d.MessageUser)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.MessageUserId)
                    .HasConstraintName("message_ibfk_1");
            });            

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.PhoneNumber)
                            .HasColumnName("use_phone_number")
                            .HasColumnType("varchar(20)")
                            .HasCollation("utf8mb4_0900_ai_ci")
                            .HasCharSet("utf8mb4");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("user_created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.Email)
                    .HasColumnName("user_email")
                    .HasColumnType("varchar(50)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.FullName)
                    .HasColumnName("user_fullname")
                    .HasColumnType("varchar(100)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("user_image_url")
                    .HasColumnType("varchar(500)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");


                entity.Property(e => e.PasswordHash)
                    .HasColumnName("user_password")
                    .HasColumnType("varchar(500)")
                    .HasCollation("utf8mb4_0900_ai_ci")
                    .HasCharSet("utf8mb4");
            });

            
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("role");
                entity.Property(m => m.Id).HasMaxLength(85);
                entity.Property(m => m.NormalizedName).HasMaxLength(85);
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("user_roles");
                entity.Property(m => m.RoleId).HasMaxLength(50);
                entity.Property(m => m.UserId).HasMaxLength(50);
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("user_claims");
                entity.Property(m => m.Id).HasMaxLength(50);
                entity.Property(m => m.UserId).HasMaxLength(50);
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("user_logins");
                entity.HasKey(e => e.UserId);
                entity.Property(m => m.LoginProvider).HasMaxLength(50);
                entity.Property(m => m.ProviderKey).HasMaxLength(50);
                entity.Property(m => m.UserId).HasMaxLength(50);
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("role_claims");
                entity.Property(e => e.Id).HasMaxLength(50);
                entity.Property(m => m.RoleId).HasMaxLength(50);
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("user_tokens");
                entity.Property(m => m.UserId).HasMaxLength(50);
                entity.Property(m => m.LoginProvider).HasMaxLength(50);
                entity.Property(m => m.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
