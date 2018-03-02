namespace Output.ProjectionTests.Migrations
{
    using Output.ProjectionTests.Database;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            Database.SetInitializer(new DropCreateDatabaseAlways<TestContext>());
        }

        protected override void Seed(TestContext context)
        {
        }
    }
}