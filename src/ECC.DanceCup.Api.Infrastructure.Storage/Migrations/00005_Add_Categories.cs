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
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "categories";
        """;
}