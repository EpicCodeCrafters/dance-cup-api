using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00001, TransactionBehavior.None)]
public class Add_Dances : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "dances" (
            "id" bigserial primary key,
            "short_name" text not null,
            "name" text not null
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "dances";
        """;
}