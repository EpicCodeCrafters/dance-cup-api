using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00010, TransactionBehavior.None)]
public class Add_Users : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "users" (
            "id" bigserial primary key,
            "version" int not null,
            "created_at" timestamp not null,
            "changed_at" timestamp not null,
            "external_id" bigint not null,
            "username" text not null
        );
        """;
    
    protected override string? DownSqlCommand =>
        """
        drop table "users";
        """;
}