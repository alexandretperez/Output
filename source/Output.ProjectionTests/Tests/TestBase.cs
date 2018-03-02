using Output.ProjectionTests.Database;
using Output.ProjectionTests.Tests;
using Output.Providers;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Output.ProjectionTests
{
    public abstract class TestBase : IClassFixture<DbFixture>
    {
        protected IMapper Mapper { get; }
        private readonly ITestOutputHelper _xunitLog;
        protected TestContext Db { get; }

        protected TestBase(DbFixture fixture, ITestOutputHelper xunitLog)
        {
            Mapper = new Mapper(new MappingProvider());

            Db = fixture.Db;
            _xunitLog = xunitLog;
            Run();
        }

        public void CaptureSql(Action action)
        {
            Db.Database.Log = _xunitLog.WriteLine;
            action();
            Db.Database.Log = null;
        }

        protected abstract void Run();
    }
}