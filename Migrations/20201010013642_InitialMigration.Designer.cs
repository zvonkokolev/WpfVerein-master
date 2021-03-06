﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WpfVerein.Model;

namespace WpfVerein.Migrations
{
    [DbContext(typeof(ClubMemberContext))]
    [Migration("20201010013642_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("WpfVerein.Core.Entities.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ActualDateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BirthDay")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("Firstname")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("Lastname")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
