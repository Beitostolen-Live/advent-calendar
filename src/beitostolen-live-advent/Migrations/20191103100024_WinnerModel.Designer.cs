﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using beitostolen_live_api.Models;

namespace beitostolen_live_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191103100024_WinnerModel")]
    partial class WinnerModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:Enum:winner_status", "in_progress,completed")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("beitostolen_live_api.Models.Alternative", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Alternatives");
                });

            modelBuilder.Entity("beitostolen_live_api.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<DateTime>("QuestionForDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("beitostolen_live_api.Models.Response", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AnswearId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Registered")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("AnswearId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("beitostolen_live_api.Models.Winner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AlternativeId")
                        .HasColumnType("integer");

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AlternativeId");

                    b.ToTable("Winners");
                });

            modelBuilder.Entity("beitostolen_live_api.Models.Alternative", b =>
                {
                    b.HasOne("beitostolen_live_api.Models.Question", "Question")
                        .WithMany("Alternatives")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("beitostolen_live_api.Models.Response", b =>
                {
                    b.HasOne("beitostolen_live_api.Models.Alternative", "Answear")
                        .WithMany()
                        .HasForeignKey("AnswearId");

                    b.HasOne("beitostolen_live_api.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("beitostolen_live_api.Models.Winner", b =>
                {
                    b.HasOne("beitostolen_live_api.Models.Response", "Alternative")
                        .WithMany()
                        .HasForeignKey("AlternativeId");
                });
#pragma warning restore 612, 618
        }
    }
}
