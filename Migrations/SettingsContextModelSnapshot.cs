﻿// <auto-generated />
using DispoDataAssistant.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DispoDataAssistant.Migrations
{
    [DbContext(typeof(SettingsContext))]
    partial class SettingsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("DispoDataAssistant.Data.Models.Settings.General", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("SettingsId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SettingsId");

                    b.ToTable("General");
                });

            modelBuilder.Entity("DispoDataAssistant.Data.Models.Settings.Integration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SettingsId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SettingsId");

                    b.ToTable("Integrations");
                });

            modelBuilder.Entity("DispoDataAssistant.Data.Models.Settings.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("DispoDataAssistant.Data.Models.Settings.General", b =>
                {
                    b.HasOne("DispoDataAssistant.Data.Models.Settings.Settings", "Settings")
                        .WithMany("General")
                        .HasForeignKey("SettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("DispoDataAssistant.Data.Models.Settings.Integration", b =>
                {
                    b.HasOne("DispoDataAssistant.Data.Models.Settings.Settings", "Settings")
                        .WithMany("Integrations")
                        .HasForeignKey("SettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("DispoDataAssistant.Data.Models.Settings.Settings", b =>
                {
                    b.Navigation("General");

                    b.Navigation("Integrations");
                });
#pragma warning restore 612, 618
        }
    }
}
