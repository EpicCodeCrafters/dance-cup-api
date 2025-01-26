using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00006, TransactionBehavior.None)]

public class Add_Couples : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        create table "couples"(
            "id" bigserial primary key,
            "tournament_id" bigint not null,
            "first_participant_full_name" text not null,
            "second_participant_full_name" text null,
            "dance_organization_name" text null,
            "first_trainer_full_name" text null,
            "second_trainer_full_name" text null
        );
        
        create sequence "couples_ids_seq";
        """;

    protected override string? DownSqlCommand =>
        """
        drop table "couples";
        """;
}