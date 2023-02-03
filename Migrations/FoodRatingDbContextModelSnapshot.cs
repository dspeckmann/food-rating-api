﻿// <auto-generated />
using System;
using FoodRatingApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoodRatingApi.Migrations
{
    [DbContext(typeof(FoodRatingDbContext))]
    partial class FoodRatingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FoodRatingApi.Entities.Food", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("PictureId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PictureId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.FoodRating", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("FoodId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PetId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PictureId")
                        .HasColumnType("uuid");

                    b.Property<int?>("Taste")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Wellbeing")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FoodId");

                    b.HasIndex("PetId");

                    b.HasIndex("PictureId");

                    b.ToTable("FoodRatings");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Invitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("PetId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PetId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string[]>("OwnerIds")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<Guid?>("PictureId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PictureId");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ObjectName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OriginalFileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Food", b =>
                {
                    b.HasOne("FoodRatingApi.Entities.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.FoodRating", b =>
                {
                    b.HasOne("FoodRatingApi.Entities.Food", "Food")
                        .WithMany("FoodRatings")
                        .HasForeignKey("FoodId");

                    b.HasOne("FoodRatingApi.Entities.Pet", "Pet")
                        .WithMany("FoodRatings")
                        .HasForeignKey("PetId");

                    b.HasOne("FoodRatingApi.Entities.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.Navigation("Food");

                    b.Navigation("Pet");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Invitation", b =>
                {
                    b.HasOne("FoodRatingApi.Entities.Pet", "Pet")
                        .WithMany()
                        .HasForeignKey("PetId");

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Pet", b =>
                {
                    b.HasOne("FoodRatingApi.Entities.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Food", b =>
                {
                    b.Navigation("FoodRatings");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Pet", b =>
                {
                    b.Navigation("FoodRatings");
                });
#pragma warning restore 612, 618
        }
    }
}
