using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00011, TransactionBehavior.None)]

public class Add_Division_To_Couples : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        alter table "couples" add column "division" text null;
        """;

    protected override string? DownSqlCommand =>
        """
        alter table "couples" drop column "division";
        """;
}
