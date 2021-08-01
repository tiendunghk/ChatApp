﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeamApp.Infrastructure.Persistence.Entities;

namespace TeamApp.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(TeamAppContext))]
    [Migration("20210730182313_newdatabase")]
    partial class newdatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(85) CHARACTER SET utf8mb4")
                        .HasMaxLength(85);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(85) CHARACTER SET utf8mb4")
                        .HasMaxLength(85);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("role");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasMaxLength(50);

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("role_claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasMaxLength(50);

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("user_claims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.HasKey("UserId");

                    b.ToTable("user_logins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("user_roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("user_tokens");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.GroupChat", b =>
                {
                    b.Property<string>("GroupChatId")
                        .HasColumnName("group_chat_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<DateTime?>("GroupChatCreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GroupChatImageUrl")
                        .HasColumnName("group_chat_imageurl")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("GroupChatName")
                        .HasColumnName("group_chat_name")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.HasKey("GroupChatId");

                    b.ToTable("group_chat");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.GroupChatUser", b =>
                {
                    b.Property<string>("GroupChatUserId")
                        .HasColumnName("group_chat_user_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("GroupChatUserGroupChatId")
                        .HasColumnName("group_chat_user_group_chat_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("GroupChatUserUserId")
                        .HasColumnName("group_chat_user_user_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.HasKey("GroupChatUserId");

                    b.HasIndex("GroupChatUserGroupChatId")
                        .HasName("group_chat_user_group_chat_id");

                    b.HasIndex("GroupChatUserUserId")
                        .HasName("group_chat_user_user_id");

                    b.ToTable("group_chat_user");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.Message", b =>
                {
                    b.Property<string>("MessageId")
                        .HasColumnName("message_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("MessageContent")
                        .HasColumnName("message_content")
                        .HasColumnType("text")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<DateTime?>("MessageCreatedAt")
                        .HasColumnName("message_created_at")
                        .HasColumnType("timestamp");

                    b.Property<string>("MessageGroupChatId")
                        .HasColumnName("message_group_chat_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("MessageUserId")
                        .HasColumnName("message_user_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.HasKey("MessageId");

                    b.HasIndex("MessageGroupChatId")
                        .HasName("message_group_chat_id");

                    b.HasIndex("MessageUserId")
                        .HasName("message_user_id");

                    b.ToTable("message");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.RefreshToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("refresh_token");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("user_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnName("user_created_at")
                        .HasColumnType("timestamp");

                    b.Property<string>("Email")
                        .HasColumnName("user_email")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(256)
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FullName")
                        .HasColumnName("user_fullname")
                        .HasColumnType("varchar(100)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("ImageUrl")
                        .HasColumnName("user_image_url")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnName("user_password")
                        .HasColumnType("varchar(500)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnName("use_phone_number")
                        .HasColumnType("varchar(20)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("user");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.UserConnection", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ConnectionId")
                        .HasColumnName("user_connection_id")
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci")
                        .HasAnnotation("MySql:CharSet", "utf8mb4");

                    b.HasKey("UserId", "ConnectionId");

                    b.ToTable("user_connection");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.GroupChatUser", b =>
                {
                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.GroupChat", "GroupChatUserGroupChat")
                        .WithMany("GroupChatUser")
                        .HasForeignKey("GroupChatUserGroupChatId")
                        .HasConstraintName("group_chat_user_ibfk_2");

                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.User", "GroupChatUserUser")
                        .WithMany("GroupChatUser")
                        .HasForeignKey("GroupChatUserUserId")
                        .HasConstraintName("group_chat_user_ibfk_1");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.Message", b =>
                {
                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.GroupChat", "MessageGroupChat")
                        .WithMany("Message")
                        .HasForeignKey("MessageGroupChatId")
                        .HasConstraintName("message_ibfk_2");

                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.User", "MessageUser")
                        .WithMany("Message")
                        .HasForeignKey("MessageUserId")
                        .HasConstraintName("message_ibfk_1");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.RefreshToken", b =>
                {
                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TeamApp.Infrastructure.Persistence.Entities.UserConnection", b =>
                {
                    b.HasOne("TeamApp.Infrastructure.Persistence.Entities.User", null)
                        .WithMany("UserConnections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}