using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Framework.Core.DAL.Infrastructure;

namespace Sample.DotNetFramework.MVC6.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20161222132608_Init")]
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
