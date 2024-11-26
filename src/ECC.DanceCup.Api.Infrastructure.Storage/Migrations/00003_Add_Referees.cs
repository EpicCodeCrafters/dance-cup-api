using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00003, TransactionBehavior.None)]
public class Add_Referees : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "referees" (
            "id" bigserial primary key,
            "version" int not null,
            "created_at" timestamp not null,
            "changed_at" timestamp not null,
            "full_name" text not null
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "referees";
        """;
}