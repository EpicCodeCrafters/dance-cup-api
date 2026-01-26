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
            "state" text not null,
            "registration_started_at" timestamp null,
            "registration_finished_at" timestamp null,
            "started_at" timestamp null,
            "finished_at" timestamp null,
            "attachments" jsonb not null default '[]'::jsonb
        );
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "tournaments";
        """;
}