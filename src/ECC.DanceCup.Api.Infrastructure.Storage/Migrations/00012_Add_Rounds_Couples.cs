using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00012, TransactionBehavior.None)]

public class Add_Rounds_Couples : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "rounds_couples" (
            "id" bigserial primary key,
            "round_id" bigint not null,
            "couple_id" bigint not null
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "rounds_couples";
        """;
}
