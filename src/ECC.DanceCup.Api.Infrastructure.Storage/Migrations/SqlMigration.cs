using FluentMigrator;

namespace ECC.DanceCup.Api.Infrastructure.Storage.Migrations;

public abstract class SqlMigration : Migration
{
    protected virtual string? UpSqlCommand => null;

    protected virtual string? DownSqlCommand => null;

    public override void Up()
    {
        if (UpSqlCommand is not null)
        {
            Execute.Sql(UpSqlCommand);
        }
    }

    public override void Down()
    {
        if (DownSqlCommand is not null)
        {
            Execute.Sql(DownSqlCommand);
        }
    }
}