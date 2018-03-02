using Output.Providers;

namespace Output.UnitTests
{
    public abstract class TestBase<T> where T : IMappingProvider, new()
    {
        protected TestBase()
        {
            Mapper = new Mapper(new T());
        }

        protected IMapper Mapper { get; }
    }
}