using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00004, TransactionBehavior.None)]
public class Add_Tournaments : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "tournaments" (
            "id" bigserial primary key,
            "version" int not null,
            "created_at" timestamp not null,
            "changed_at" timestamp not null,
            "user_id" bigint not null,
            "name" text not null,
            "description" text,
            "date" timestamp not null,
            "state" int not null,
            "registration_started_at" timestamp,
            "registration_finished_at" timestamp,
            "started_at" timestamp,
            "finished_at" timestamp
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "tournaments";
        """;
}