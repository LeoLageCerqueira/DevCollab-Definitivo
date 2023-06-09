﻿// <auto-generated />
using System;
using DevCollab.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevCollab.Infra.Migrations
{
    [DbContext(typeof(DevCollabDbContext))]
    partial class DevCollabDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DevCollab.Domain.Entities.Comentario", b =>
                {
                    b.Property<int>("IdComentario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdComentario"), 1L, 1);

                    b.Property<Guid>("AutorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PublicacaoId")
                        .HasColumnType("int");

                    b.Property<string>("Texto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("IdComentario");

                    b.HasIndex("PublicacaoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("DevCollab.Domain.Entities.Publicacao", b =>
                {
                    b.Property<int>("IdPublicacao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPublicacao"), 1L, 1);

                    b.Property<Guid>("AutorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Texto")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdPublicacao");

                    b.HasIndex("AutorId");

                    b.ToTable("Publicacoes");
                });

            modelBuilder.Entity("DevCollab.Domain.Entities.SeguidorSeguido", b =>
                {
                    b.Property<Guid>("SeguidorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SeguidoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("SeguidorId", "SeguidoId");

                    b.HasIndex("SeguidoId");

                    b.ToTable("SeguidoresSeguidos");
                });

            modelBuilder.Entity("DevCollab.Domain.Entities.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CaminhoFotoPerfil")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("DevCollab.Domain.Entities.Comentario", b =>
                {
                    b.HasOne("DevCollab.Domain.Entities.Publicacao", "Publicacao")
                        .WithMany("Comentarios")
                        .HasForeignKey("PublicacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevCollab.Domain.Entities.Usuario", null)
                        .WithMany("Comentarios")
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Publicacao");
                });

            modelBuilder.Entity("DevCollab.Domain.Entities.Publicacao", b =>
                {
                    b.HasOne("DevCollab.Domain.Entities.Usuario", "Autor")
                        .WithMany("Publicacoes")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");
                });

            modelBuilder.Entity("DevCollab.Domain.Entities.SeguidorSeguido", b =>
                {
                    b.HasOne("DevCollab.Domain.Entities.Usuario", "Seguido")
                        .WithMany("Seguidores")
                        .HasForeignKey("SeguidoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DevCollab.Domain.Entities.Usuario", "Seguidor")
                        .WithMany("Seguindo")
                        .HasForeignKey("SeguidorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Seguido");

                    b.Navigation("Seguidor");
                });

            modelBuilder.Entity("DevCollab.Domain.Entities.Publicacao", b =>
                {
                    b.Navigation("Comentarios");
                });

            modelBuilder.Entity("DevCollab.Domain.Entities.Usuario", b =>
                {
                    b.Navigation("Comentarios");

                    b.Navigation("Publicacoes");

                    b.Navigation("Seguidores");

                    b.Navigation("Seguindo");
                });
#pragma warning restore 612, 618
        }
    }
}
