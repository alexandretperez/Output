using BenchmarkDotNet.Attributes;
using Output.Benchmarking.Mappers;

namespace Output.Benchmarking.Tests
{
    [Config(typeof(TestConfig))]
    public abstract class TestBase<TSource, TTarget> where TTarget : class
    {
        protected TestBase()
        {
            Output = new OutputMapper<TSource, TTarget>(GetName());
            Mapster = new MapsterMapper<TSource, TTarget>(GetName());
            Handwritten = new HandwrittenMapper<TSource, TTarget>(GetName());
            AutoMapper = new AutoMapMapper<TSource, TTarget>(GetName());

            _source = InitializeSource();
        }

        protected OutputMapper<TSource, TTarget> Output { get; }
        protected MapsterMapper<TSource, TTarget> Mapster { get; }
        protected HandwrittenMapper<TSource, TTarget> Handwritten { get; }
        protected AutoMapMapper<TSource, TTarget> AutoMapper { get; }

        private readonly TSource _source;

        protected abstract TSource InitializeSource();

        protected abstract string GetName();

        [Benchmark(Baseline = true)] // remove the Baseline = true to run all tests at once.
        //[Benchmark]
        public TTarget HandwrittenMap()
        {
            return Handwritten.Map(_source);
        }

        [Benchmark]
        public TTarget OutputMap()
        {
            return Output.Map(_source);
        }

        [Benchmark]
        public TTarget MapsterMap()
        {
            return Mapster.Map(_source);
        }

        [Benchmark]
        public TTarget AutoMapperMap()
        {
            return AutoMapper.Map(_source);
        }
    }
}