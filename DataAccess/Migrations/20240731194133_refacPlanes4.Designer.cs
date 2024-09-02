﻿// <auto-generated />
using System;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(PilatesContext))]
    [Migration("20240731194133_refacPlanes4")]
    partial class refacPlanes4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("ActividadLocal", b =>
                {
                    b.Property<int>("ActividadesId")
                        .HasColumnType("int");

                    b.Property<int>("LocalesId")
                        .HasColumnType("int");

                    b.HasKey("ActividadesId", "LocalesId");

                    b.HasIndex("LocalesId");

                    b.ToTable("ActividadLocal");
                });

            modelBuilder.Entity("ActividadPlan", b =>
                {
                    b.Property<int>("ActividadesId")
                        .HasColumnType("int");

                    b.Property<int>("PlanesId")
                        .HasColumnType("int");

                    b.HasKey("ActividadesId", "PlanesId");

                    b.HasIndex("PlanesId");

                    b.ToTable("ActividadPlan");
                });

            modelBuilder.Entity("ActividadProfesor", b =>
                {
                    b.Property<int>("ActividadesId")
                        .HasColumnType("int");

                    b.Property<int>("ProfesoresId")
                        .HasColumnType("int");

                    b.HasKey("ActividadesId", "ProfesoresId");

                    b.HasIndex("ProfesoresId");

                    b.ToTable("ActividadProfesor");
                });

            modelBuilder.Entity("AlumnoPatologia", b =>
                {
                    b.Property<int>("AlumnosId")
                        .HasColumnType("int");

                    b.Property<int>("PatologíasQuePresentaId")
                        .HasColumnType("int");

                    b.HasKey("AlumnosId", "PatologíasQuePresentaId");

                    b.HasIndex("PatologíasQuePresentaId");

                    b.ToTable("AlumnoPatologia");
                });

            modelBuilder.Entity("Domain.Actividad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Color")
                        .HasColumnType("longtext");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<string>("DescripcionCorta")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Actividad");
                });

            modelBuilder.Entity("Domain.Agenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ClaseId")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .HasColumnType("longtext");

                    b.Property<string>("Dia")
                        .HasColumnType("longtext");

                    b.Property<string>("Hora")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("HorarioFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("HorarioInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("LocalId")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ClaseId")
                        .IsUnique();

                    b.HasIndex("LocalId");

                    b.ToTable("Agendas");
                });

            modelBuilder.Entity("Domain.Alumnos.AlumnoClase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AlumnoId")
                        .HasColumnType("int");

                    b.Property<int>("ClaseId")
                        .HasColumnType("int");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AlumnoId");

                    b.HasIndex("ClaseId");

                    b.ToTable("AlumnoClase");
                });

            modelBuilder.Entity("Domain.Clase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActividadId")
                        .HasColumnType("int");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("CuposOtorgados")
                        .HasColumnType("int");

                    b.Property<int>("CuposTotales")
                        .HasColumnType("int");

                    b.Property<DateTime>("HorarioFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("HorarioInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("LocalId")
                        .HasColumnType("int");

                    b.Property<int?>("ProfesorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActividadId");

                    b.HasIndex("LocalId");

                    b.HasIndex("ProfesorId");

                    b.ToTable("Clases");
                });

            modelBuilder.Entity("Domain.ClaseFija", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActividadId")
                        .HasColumnType("int");

                    b.Property<int>("AlumnoId")
                        .HasColumnType("int");

                    b.Property<string>("Dia")
                        .HasColumnType("longtext");

                    b.Property<string>("Hora")
                        .HasColumnType("longtext");

                    b.Property<int>("LocalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActividadId");

                    b.HasIndex("AlumnoId");

                    b.ToTable("ClasesFijas");
                });

            modelBuilder.Entity("Domain.Local", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Celular")
                        .HasColumnType("longtext");

                    b.Property<string>("Ciudad")
                        .HasColumnType("longtext");

                    b.Property<string>("Direccion")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext");

                    b.Property<string>("Pais")
                        .HasColumnType("longtext");

                    b.Property<string>("Telefono")
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Local");
                });

            modelBuilder.Entity("Domain.Patologia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Patologias");
                });

            modelBuilder.Entity("Domain.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActividadLibreId")
                        .HasColumnType("int");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext");

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext");

                    b.Property<int>("Precio")
                        .HasColumnType("int");

                    b.Property<int>("VecesxSemana")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Planes");
                });

            modelBuilder.Entity("Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("ChangePassword")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<int>("Rol")
                        .HasColumnType("int");

                    b.Property<Guid>("Token")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("LocalProfesor", b =>
                {
                    b.Property<int>("LocalesId")
                        .HasColumnType("int");

                    b.Property<int>("ProfesoresId")
                        .HasColumnType("int");

                    b.HasKey("LocalesId", "ProfesoresId");

                    b.HasIndex("ProfesoresId");

                    b.ToTable("LocalProfesor");
                });

            modelBuilder.Entity("PatologiaProfesor", b =>
                {
                    b.Property<int>("PatologíasQuePresentaId")
                        .HasColumnType("int");

                    b.Property<int>("ProfesoresId")
                        .HasColumnType("int");

                    b.HasKey("PatologíasQuePresentaId", "ProfesoresId");

                    b.HasIndex("ProfesoresId");

                    b.ToTable("PatologiaProfesor");
                });

            modelBuilder.Entity("Domain.Alumno", b =>
                {
                    b.HasBaseType("Domain.User");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("Alumno_Activo");

                    b.Property<string>("Apellido")
                        .HasColumnType("longtext")
                        .HasColumnName("Alumno_Apellido");

                    b.Property<string>("Cedula")
                        .HasColumnType("longtext")
                        .HasColumnName("Alumno_Cedula");

                    b.Property<string>("Celular")
                        .HasColumnType("longtext")
                        .HasColumnName("Alumno_Celular");

                    b.Property<string>("Ciudad")
                        .HasColumnType("longtext")
                        .HasColumnName("Alumno_Ciudad");

                    b.Property<string>("ContactoEmergencia")
                        .HasColumnType("longtext")
                        .HasColumnName("Alumno_ContactoEmergencia");

                    b.Property<string>("Direccion")
                        .HasColumnType("longtext")
                        .HasColumnName("Alumno_Direccion");

                    b.Property<string>("EmeregenciaMovil")
                        .HasColumnType("longtext")
                        .HasColumnName("Alumno_EmeregenciaMovil");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("Alumno_FechaAlta");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("Alumno_FechaNacimiento");

                    b.Property<string>("Observaciones")
                        .HasColumnType("longtext");

                    b.Property<int?>("PlanId")
                        .HasColumnType("int");

                    b.Property<string>("Telefono")
                        .HasColumnType("longtext");

                    b.Property<string>("TelefonoContacto")
                        .HasColumnType("longtext")
                        .HasColumnName("Alumno_TelefonoContacto");

                    b.HasIndex("PlanId");

                    b.HasDiscriminator().HasValue("Alumno");
                });

            modelBuilder.Entity("Domain.Profesor", b =>
                {
                    b.HasBaseType("Domain.User");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Apellido")
                        .HasColumnType("longtext");

                    b.Property<string>("Cedula")
                        .HasColumnType("longtext");

                    b.Property<string>("Celular")
                        .HasColumnType("longtext");

                    b.Property<string>("Ciudad")
                        .HasColumnType("longtext");

                    b.Property<string>("ContactoEmergencia")
                        .HasColumnType("longtext");

                    b.Property<string>("Direccion")
                        .HasColumnType("longtext");

                    b.Property<string>("EmeregenciaMovil")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TelefonoContacto")
                        .HasColumnType("longtext");

                    b.HasDiscriminator().HasValue("Profesor");
                });

            modelBuilder.Entity("ActividadLocal", b =>
                {
                    b.HasOne("Domain.Actividad", null)
                        .WithMany()
                        .HasForeignKey("ActividadesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Local", null)
                        .WithMany()
                        .HasForeignKey("LocalesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ActividadPlan", b =>
                {
                    b.HasOne("Domain.Actividad", null)
                        .WithMany()
                        .HasForeignKey("ActividadesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Plan", null)
                        .WithMany()
                        .HasForeignKey("PlanesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ActividadProfesor", b =>
                {
                    b.HasOne("Domain.Actividad", null)
                        .WithMany()
                        .HasForeignKey("ActividadesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Profesor", null)
                        .WithMany()
                        .HasForeignKey("ProfesoresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AlumnoPatologia", b =>
                {
                    b.HasOne("Domain.Alumno", null)
                        .WithMany()
                        .HasForeignKey("AlumnosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Patologia", null)
                        .WithMany()
                        .HasForeignKey("PatologíasQuePresentaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Agenda", b =>
                {
                    b.HasOne("Domain.Clase", "Clase")
                        .WithOne("Agenda")
                        .HasForeignKey("Domain.Agenda", "ClaseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Domain.Local", "Local")
                        .WithMany("Agendas")
                        .HasForeignKey("LocalId");

                    b.Navigation("Clase");

                    b.Navigation("Local");
                });

            modelBuilder.Entity("Domain.Alumnos.AlumnoClase", b =>
                {
                    b.HasOne("Domain.Alumno", "Alumno")
                        .WithMany("ClasesAlumno")
                        .HasForeignKey("AlumnoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Clase", "Clase")
                        .WithMany("ClasesAlumno")
                        .HasForeignKey("ClaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Alumno");

                    b.Navigation("Clase");
                });

            modelBuilder.Entity("Domain.Clase", b =>
                {
                    b.HasOne("Domain.Actividad", "Actividad")
                        .WithMany("Clases")
                        .HasForeignKey("ActividadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Local", "Local")
                        .WithMany()
                        .HasForeignKey("LocalId");

                    b.HasOne("Domain.Profesor", "Profesor")
                        .WithMany()
                        .HasForeignKey("ProfesorId");

                    b.Navigation("Actividad");

                    b.Navigation("Local");

                    b.Navigation("Profesor");
                });

            modelBuilder.Entity("Domain.ClaseFija", b =>
                {
                    b.HasOne("Domain.Actividad", "Actividad")
                        .WithMany()
                        .HasForeignKey("ActividadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Alumno", "Alumno")
                        .WithMany("ClasesFijas")
                        .HasForeignKey("AlumnoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actividad");

                    b.Navigation("Alumno");
                });

            modelBuilder.Entity("LocalProfesor", b =>
                {
                    b.HasOne("Domain.Local", null)
                        .WithMany()
                        .HasForeignKey("LocalesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Profesor", null)
                        .WithMany()
                        .HasForeignKey("ProfesoresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PatologiaProfesor", b =>
                {
                    b.HasOne("Domain.Patologia", null)
                        .WithMany()
                        .HasForeignKey("PatologíasQuePresentaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Profesor", null)
                        .WithMany()
                        .HasForeignKey("ProfesoresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Alumno", b =>
                {
                    b.HasOne("Domain.Plan", "Plan")
                        .WithMany("Alumnos")
                        .HasForeignKey("PlanId");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("Domain.Actividad", b =>
                {
                    b.Navigation("Clases");
                });

            modelBuilder.Entity("Domain.Clase", b =>
                {
                    b.Navigation("Agenda");

                    b.Navigation("ClasesAlumno");
                });

            modelBuilder.Entity("Domain.Local", b =>
                {
                    b.Navigation("Agendas");
                });

            modelBuilder.Entity("Domain.Plan", b =>
                {
                    b.Navigation("Alumnos");
                });

            modelBuilder.Entity("Domain.Alumno", b =>
                {
                    b.Navigation("ClasesAlumno");

                    b.Navigation("ClasesFijas");
                });
#pragma warning restore 612, 618
        }
    }
}
