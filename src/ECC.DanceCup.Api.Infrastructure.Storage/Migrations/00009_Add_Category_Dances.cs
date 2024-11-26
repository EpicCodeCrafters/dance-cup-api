using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00009, TransactionBehavior.None)]

public class Add_Category_Dances : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "categories_dances" (
            "id" bigserial primary key,
            "category_id" bigint not null,
            "dance_id" bigint not null
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "categories_dances";
        """;
}