using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00008, TransactionBehavior.None)]

public class Add_Category_Referee : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "categories_referees" (
            "id" bigint primary key,
            "category_id" bigint not null,
            "referee_id" bigint not null
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "categories_referees";
        """;
}