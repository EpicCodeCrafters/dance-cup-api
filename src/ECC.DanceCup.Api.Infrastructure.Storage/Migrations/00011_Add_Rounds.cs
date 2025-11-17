using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00011, TransactionBehavior.None)]

public class Add_Rounds : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "rounds" (
            "id" bigserial primary key,
            "category_id" bigint not null,
            "order_number" int not null
        );
        
        create sequence "rounds_ids_seq";
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "rounds";
        drop sequence "rounds_ids_seq";
        """;
}
