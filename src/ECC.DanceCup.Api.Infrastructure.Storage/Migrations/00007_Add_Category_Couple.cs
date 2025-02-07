using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(0007, TransactionBehavior.None)]

public class Add_Category_Couple : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "categories_couples" (
            "id" bigserial primary key,
            "category_id" bigint not null,
            "couple_id" bigint not null
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "categories_couples";
        """;
}