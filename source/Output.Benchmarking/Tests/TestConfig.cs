using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace Output.Benchmarking.Tests
{
    public class TestConfig : ManualConfig
    {
        public TestConfig()
        {
            Add(RankColumn.Roman);
            Add(MemoryDiagnoser.Default);
            //Add(CsvMeasurementsExporter.Default);
            //Add(RPlotExporter.Default);
            Add(Job.ShortRun);

            Set(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
        }
    }
}