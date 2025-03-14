﻿// <auto-generated />
using System;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250308110140_FixColumnImgPathDb")]
    partial class FixColumnImgPathDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataIscrizione")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroTelefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Clienti");
                });

            modelBuilder.Entity("API.Models.Entities.DettaglioOrdine", b =>
                {
                    b.Property<int>("IdDettaglio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDettaglio"));

                    b.Property<int>("OrdineId")
                        .HasColumnType("int");

                    b.Property<int>("ProdottoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantita")
                        .HasColumnType("int");

                    b.HasKey("IdDettaglio");

                    b.HasIndex("OrdineId");

                    b.HasIndex("ProdottoId");

                    b.ToTable("DettagliOrdini");
                });

            modelBuilder.Entity("API.Models.Entities.Ordine", b =>
                {
                    b.Property<int>("IdOrdine")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdOrdine"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataOrdine")
                        .HasColumnType("datetime2");

                    b.Property<string>("MetodoPagamento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stato")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotaleOrdine")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdOrdine");

                    b.HasIndex("ClienteId");

                    b.ToTable("Ordini");
                });

            modelBuilder.Entity("API.Models.Entities.Prodotto", b =>
                {
                    b.Property<int>("IdPrdotto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPrdotto"));

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DataInserimento")
                        .HasColumnType("datetime2");

                    b.Property<int>("Giacenza")
                        .HasColumnType("int");

                    b.Property<string>("ImgPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeProdotto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Prezzo")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdPrdotto");

                    b.ToTable("Prodotti");
                });

            modelBuilder.Entity("API.Models.Cliente", b =>
                {
                    b.OwnsOne("API.Models.ValueObjects.Indirizzo", "Indirizzo", b1 =>
                        {
                            b1.Property<int>("ClienteId")
                                .HasColumnType("int");

                            b1.Property<string>("CAP")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Indirizzo_CAP");

                            b1.Property<string>("Citta")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Indirizzo_Citta");

                            b1.Property<string>("Via")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Indirizzo_Via");

                            b1.HasKey("ClienteId");

                            b1.ToTable("Clienti");

                            b1.WithOwner()
                                .HasForeignKey("ClienteId");
                        });

                    b.Navigation("Indirizzo")
                        .IsRequired();
                });

            modelBuilder.Entity("API.Models.Entities.DettaglioOrdine", b =>
                {
                    b.HasOne("API.Models.Entities.Ordine", "Ordine")
                        .WithMany("DettagliOrdini")
                        .HasForeignKey("OrdineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Models.Entities.Prodotto", "Prodotto")
                        .WithMany("DettagliOrdini")
                        .HasForeignKey("ProdottoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ordine");

                    b.Navigation("Prodotto");
                });

            modelBuilder.Entity("API.Models.Entities.Ordine", b =>
                {
                    b.HasOne("API.Models.Cliente", "Cliente")
                        .WithMany("Ordini")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("API.Models.Cliente", b =>
                {
                    b.Navigation("Ordini");
                });

            modelBuilder.Entity("API.Models.Entities.Ordine", b =>
                {
                    b.Navigation("DettagliOrdini");
                });

            modelBuilder.Entity("API.Models.Entities.Prodotto", b =>
                {
                    b.Navigation("DettagliOrdini");
                });
#pragma warning restore 612, 618
        }
    }
}
