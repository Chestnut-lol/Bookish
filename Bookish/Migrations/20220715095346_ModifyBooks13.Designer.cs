﻿// <auto-generated />
using Bookish;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bookish.Migrations
{
    [DbContext(typeof(EFCore))]
    [Migration("20220715095346_ModifyBooks13")]
    partial class ModifyBooks13
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bookish.Models.Book", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumOfAvailableCopies")
                        .HasColumnType("int");

                    b.Property<int>("NumOfCopies")
                        .HasColumnType("int");

                    b.Property<int>("Searches")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Bookish.Models.BookCopy", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BookId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MemberId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("MemberId");

                    b.ToTable("BookCopies");
                });

            modelBuilder.Entity("Bookish.Models.Member", b =>
                {
                    b.Property<string>("MemberId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MemberId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Bookish.Models.BookCopy", b =>
                {
                    b.HasOne("Bookish.Models.Book", "Book")
                        .WithMany("Copies")
                        .HasForeignKey("BookId");

                    b.HasOne("Bookish.Models.Member", "Member")
                        .WithMany("Books")
                        .HasForeignKey("MemberId");

                    b.Navigation("Book");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Bookish.Models.Book", b =>
                {
                    b.Navigation("Copies");
                });

            modelBuilder.Entity("Bookish.Models.Member", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
