﻿// <auto-generated />
using System;
using APIProject.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace APIProject.Domain.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220817070935_add_bill")]
    partial class add_bill
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("APIProject.Domain.Models.Bill", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BillCode")
                        .HasColumnType("longtext");

                    b.Property<string>("BillImage")
                        .HasColumnType("longtext");

                    b.Property<int>("BillPrice")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EventParticipantID")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<int>("StallID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("EventParticipantID");

                    b.HasIndex("StallID");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Categorys");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Config", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Value")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeviceID")
                        .HasColumnType("longtext");

                    b.Property<int?>("DistrictID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("IdentityCard")
                        .HasColumnType("longtext");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Job")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int?>("ProvinceID")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<int?>("WardID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DistrictID");

                    b.HasIndex("ProvinceID");

                    b.HasIndex("WardID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("APIProject.Domain.Models.CustomerGift", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("GiftID")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("GiftID");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("APIProject.Domain.Models.District", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("ProvinceID")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Code");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("APIProject.Domain.Models.EventChannel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("EventChannels");
                });

            modelBuilder.Entity("APIProject.Domain.Models.EventParticipant", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("EventChannelID")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<int>("NewsID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("EventChannelID");

                    b.HasIndex("NewsID");

                    b.ToTable("EventParticipants");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Gift", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreateByID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UrlImage")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Gifts");
                });

            modelBuilder.Entity("APIProject.Domain.Models.GiftCodeQR", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("GiftID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("GiftID");

                    b.ToTable("GiftCodeQRs");
                });

            modelBuilder.Entity("APIProject.Domain.Models.MemberPointHistory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Balance")
                        .HasColumnType("longtext");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("EventParticipantID")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<int>("Point")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("EventParticipantID");

                    b.ToTable("PointHistories");
                });

            modelBuilder.Entity("APIProject.Domain.Models.News", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<int>("IsBanner")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("TypePost")
                        .HasColumnType("int");

                    b.Property<string>("UrlImage")
                        .HasColumnType("longtext");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("News");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Notification", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("Viewed")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Province", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Type")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Code");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("APIProject.Domain.Models.RelatedStall", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("NewsID")
                        .HasColumnType("int");

                    b.Property<int>("StallID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("NewsID");

                    b.HasIndex("StallID");

                    b.ToTable("RelatedStalls");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Stall", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("LinkFB")
                        .HasColumnType("longtext");

                    b.Property<string>("LinkWeb")
                        .HasColumnType("longtext");

                    b.Property<string>("Logo")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Stalls");
                });

            modelBuilder.Entity("APIProject.Domain.Models.StallImage", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("StallID")
                        .HasColumnType("int");

                    b.Property<string>("UrlImage")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("StallID");

                    b.ToTable("ImageStalls");
                });

            modelBuilder.Entity("APIProject.Domain.Models.SurveySheet", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("SurveySheets");
                });

            modelBuilder.Entity("APIProject.Domain.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Ward", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DistrictID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Type")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Code");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Bill", b =>
                {
                    b.HasOne("APIProject.Domain.Models.EventParticipant", "EventParticipant")
                        .WithMany("Bills")
                        .HasForeignKey("EventParticipantID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APIProject.Domain.Models.Stall", "Stall")
                        .WithMany("Bills")
                        .HasForeignKey("StallID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventParticipant");

                    b.Navigation("Stall");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Customer", b =>
                {
                    b.HasOne("APIProject.Domain.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictID");

                    b.HasOne("APIProject.Domain.Models.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceID");

                    b.HasOne("APIProject.Domain.Models.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("WardID");

                    b.Navigation("District");

                    b.Navigation("Province");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("APIProject.Domain.Models.CustomerGift", b =>
                {
                    b.HasOne("APIProject.Domain.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APIProject.Domain.Models.Gift", "Gift")
                        .WithMany()
                        .HasForeignKey("GiftID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Gift");
                });

            modelBuilder.Entity("APIProject.Domain.Models.EventParticipant", b =>
                {
                    b.HasOne("APIProject.Domain.Models.Customer", "Customer")
                        .WithMany("EventParticipants")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APIProject.Domain.Models.EventChannel", "EventChannel")
                        .WithMany()
                        .HasForeignKey("EventChannelID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APIProject.Domain.Models.News", "News")
                        .WithMany()
                        .HasForeignKey("NewsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("EventChannel");

                    b.Navigation("News");
                });

            modelBuilder.Entity("APIProject.Domain.Models.GiftCodeQR", b =>
                {
                    b.HasOne("APIProject.Domain.Models.Gift", "Gift")
                        .WithMany("GiftCodeQRs")
                        .HasForeignKey("GiftID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gift");
                });

            modelBuilder.Entity("APIProject.Domain.Models.MemberPointHistory", b =>
                {
                    b.HasOne("APIProject.Domain.Models.Customer", "Customer")
                        .WithMany("MemberPoints")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APIProject.Domain.Models.EventParticipant", "EventParticipant")
                        .WithMany("MemberPointHistories")
                        .HasForeignKey("EventParticipantID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("EventParticipant");
                });

            modelBuilder.Entity("APIProject.Domain.Models.News", b =>
                {
                    b.HasOne("APIProject.Domain.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Notification", b =>
                {
                    b.HasOne("APIProject.Domain.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("APIProject.Domain.Models.RelatedStall", b =>
                {
                    b.HasOne("APIProject.Domain.Models.News", "News")
                        .WithMany("RelatedStalls")
                        .HasForeignKey("NewsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APIProject.Domain.Models.Stall", "Stall")
                        .WithMany("RelatedStalls")
                        .HasForeignKey("StallID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("News");

                    b.Navigation("Stall");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Stall", b =>
                {
                    b.HasOne("APIProject.Domain.Models.Category", "Category")
                        .WithMany("Stalls")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("APIProject.Domain.Models.StallImage", b =>
                {
                    b.HasOne("APIProject.Domain.Models.Stall", "Stall")
                        .WithMany()
                        .HasForeignKey("StallID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stall");
                });

            modelBuilder.Entity("APIProject.Domain.Models.SurveySheet", b =>
                {
                    b.HasOne("APIProject.Domain.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Category", b =>
                {
                    b.Navigation("Stalls");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Customer", b =>
                {
                    b.Navigation("EventParticipants");

                    b.Navigation("MemberPoints");
                });

            modelBuilder.Entity("APIProject.Domain.Models.EventParticipant", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("MemberPointHistories");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Gift", b =>
                {
                    b.Navigation("GiftCodeQRs");
                });

            modelBuilder.Entity("APIProject.Domain.Models.News", b =>
                {
                    b.Navigation("RelatedStalls");
                });

            modelBuilder.Entity("APIProject.Domain.Models.Stall", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("RelatedStalls");
                });
#pragma warning restore 612, 618
        }
    }
}
