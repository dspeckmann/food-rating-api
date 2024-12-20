﻿// <auto-generated />
using System;
using FoodRatingApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FoodRatingApi.Migrations
{
    [DbContext(typeof(FoodRatingDbContext))]
    [Migration("20230127220805_AddMoreDetails")]
    partial class AddMoreDetails
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasIndex("PictureId");

                    b.ToTable("FoodRatings");
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

                    b.Property<string>("DataString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Picture");
                });

            modelBuilder.Entity("FoodRatingPet", b =>
                {
                    b.Property<Guid>("FoodRatingsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PetsId")
                        .HasColumnType("uuid");

                    b.HasKey("FoodRatingsId", "PetsId");

                    b.HasIndex("PetsId");

                    b.ToTable("FoodRatingPet");
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

                    b.HasOne("FoodRatingApi.Entities.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.Navigation("Food");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Pet", b =>
                {
                    b.HasOne("FoodRatingApi.Entities.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("FoodRatingPet", b =>
                {
                    b.HasOne("FoodRatingApi.Entities.FoodRating", null)
                        .WithMany()
                        .HasForeignKey("FoodRatingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodRatingApi.Entities.Pet", null)
                        .WithMany()
                        .HasForeignKey("PetsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FoodRatingApi.Entities.Food", b =>
                {
                    b.Navigation("FoodRatings");
                });
#pragma warning restore 612, 618
        }
    }
}
