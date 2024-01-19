﻿// <auto-generated />
using DispoDataAssistant.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DispoDataAssistant.Migrations.ServiceNowAsset
{
    [DbContext(typeof(ServiceNowAssetContext))]
    partial class ServiceNowAssetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("DispoDataAssistant.Data.Models.ServiceNowAsset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AssetTag")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "asset_tag");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "subcategory");

                    b.Property<string>("InstallStatus")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "install_status");

                    b.Property<string>("LastUpdated")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "sys_updated_on");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "manufacturer.name");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "model_id.name");

                    b.Property<string>("OperationalStatus")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "operational_status");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "serial_number");

                    b.Property<string>("SysId")
                        .HasColumnType("TEXT")
                        .HasAnnotation("Relational:JsonPropertyName", "sys_id");

                    b.Property<int>("TabId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TabName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ServiceNowAssets");
                });
#pragma warning restore 612, 618
        }
    }
}
