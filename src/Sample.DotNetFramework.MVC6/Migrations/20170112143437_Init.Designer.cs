using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.DotNetFramework.MVC6.Migrations
{
    [DbContext(typeof(DbContext))]
    [Migration("20170112143437_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sample.DotNetFramework.Common.DTO.SampleModel", b =>
                {
                    b.Property<int>("SampleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SampleId");

                    b.Property<string>("SampleName")
                        .HasColumnName("SampleName");

                    b.Property<int>("Status")
                        .HasColumnName("Status");

                    b.HasKey("SampleId");

                    b.ToTable("Sample");
                });

            modelBuilder.Entity("Sample.DotNetFramework.Common.DTO.UserModel", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserId");

                    b.Property<string>("Account")
                        .HasColumnName("Account");

                    b.Property<string>("FirstName")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .HasColumnName("LastName");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });
        }
    }
}
