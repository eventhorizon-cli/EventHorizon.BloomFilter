using BenchmarkDotNet.Running;
using EventHorizon.BloomFilter.Benchmarks;

class Program
{
    static void Main(string[] args)
    {
        new BenchmarkSwitcher(new[]
        {
            typeof(BloomFilterVsHashStringReadBenchmark),
            typeof(BloomFilterVsHashStringWriteBenchmark),
            typeof(BitmapVsHashReadBenchmark),
            typeof(BitmapVsHashWriteBenchmark)
        }).Run(args, new BenchmarkConfig());
    }
}
