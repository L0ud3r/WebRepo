﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebRepo.App.Data;

#nullable disable

namespace WebRepo.App.Migrations
{
    [DbContext(typeof(WebRepoAppContext))]
    [Migration("20230310235656_UserProfilePhoto")]
    partial class UserProfilePhoto
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebRepo.DAL.Entities.FileBlob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<long>("ContentLength")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PathAPI")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("VirtualDirectoryId")
                        .HasColumnType("int");

                    b.Property<bool>("isFavourite")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VirtualDirectoryId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("WebRepo.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("photoURL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebRepo.DAL.Entities.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expire")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("WebRepo.DAL.Entities.VirtualDirectory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentDirectory")
                        .HasColumnType("int");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("VirtualDirectories");
                });

            modelBuilder.Entity("WebRepo.DAL.Entities.FileBlob", b =>
                {
                    b.HasOne("WebRepo.DAL.Entities.User", "User")
                        .WithMany("Files")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebRepo.DAL.Entities.VirtualDirectory", "VirtualDirectory")
                        .WithMany("FileBlobs")
                        .HasForeignKey("VirtualDirectoryId");

                    b.Navigation("User");

                    b.Navigation("VirtualDirectory");
                });

            modelBuilder.Entity("WebRepo.DAL.Entities.UserToken", b =>
                {
                    b.HasOne("WebRepo.DAL.Entities.User", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebRepo.DAL.Entities.VirtualDirectory", b =>
                {
                    b.HasOne("WebRepo.DAL.Entities.User", "User")
                        .WithMany("VirtualDirectories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebRepo.DAL.Entities.User", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("UserTokens");

                    b.Navigation("VirtualDirectories");
                });

            modelBuilder.Entity("WebRepo.DAL.Entities.VirtualDirectory", b =>
                {
                    b.Navigation("FileBlobs");
                });
#pragma warning restore 612, 618
        }
    }
}
