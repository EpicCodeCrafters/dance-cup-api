using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00005, TransactionBehavior.None)]

public class Add_Categories : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
         create table "categories" (
             "id" bigserial primary key,
             "tournament_id" bigint not null,
             "name" text not null
         );
         
         create table "categories_dances" (
             "id" bigserial primary key,
             "category_id" bigint not null,
             "dance_id" bigint not null
         );

        create table "categories_referees_ids" (
          "id" bigserial primary key,
          "category_id" bigint not null,
          "referee_id" bigint not null  
        );

        create table "categories_couples_ids" (
          "id" bigserial primary key,
          "category_id" bigint not null,
          "couple_id" bigint not null
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "categories";
        drop table "categories_dances";
        drop table "categories_referees_ids";
        drop table "categories_couples_ids";
        """;
}