using FluentMigrator;

namespace Signature.API.Infra.Data.Migrations
{
    [Migration(20250429)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Name").AsString(200).NotNullable()
                .WithColumn("Email").AsString(200).NotNullable();

            Create.Table("SubscriptionPlans")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("Title").AsString(200).NotNullable()
                .WithColumn("Price").AsDecimal().NotNullable();

            Create.Table("UserSubscriptions")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("UserId").AsGuid().NotNullable()
                .WithColumn("SubscriptionPlanId").AsGuid().NotNullable()
                .WithColumn("SubscribedAt").AsDateTime().NotNullable();

            Create.ForeignKey("FK_UserSubscriptions_Users")
                .FromTable("UserSubscriptions").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey("FK_UserSubscriptions_Plans")
                .FromTable("UserSubscriptions").ForeignColumn("SubscriptionPlanId")
                .ToTable("SubscriptionPlans").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("UserSubscriptions");
            Delete.Table("SubscriptionPlans");
            Delete.Table("Users");
        }
    }
}
