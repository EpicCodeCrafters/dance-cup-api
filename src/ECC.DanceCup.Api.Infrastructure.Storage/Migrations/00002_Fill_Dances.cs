using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

[Migration(00002, TransactionBehavior.None)]
public class Fill_Dances : SqlMigration
{
    protected override string? UpSqlCommand =>
        """
        insert into "dances" ("short_name", "name")
        values ('W', 'Медленный вальс'),
               ('T', 'Танго'),
               ('V', 'Венский вальс'),
               ('F', 'Фокстрот'),
               ('Q', 'Квикстеп'),
               ('S', 'Самба'),
               ('Ch', 'Ча-ча-ча'),
               ('R', 'Румба'),
               ('P', 'Пасодобль'),
               ('J', 'Джайв');
        """;
}