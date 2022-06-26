using BenchmarkDotNet.Attributes;

namespace EventHorizon.BloomFilter.Benchmarks;

public class BloomFilterVsHashStringReadBenchmark
{
    private string[] _data;
    private BloomFilter<string> _bloomFilter;
    private HashSet<string> _hashSet;

    [Params(1_000, 10_000, 100_000)] public int N;

    [GlobalSetup]
    public void Setup()
    {
        _data = new string[N];
        _bloomFilter = new BloomFilter<string>(Funnels.StringFunnel, N / 10, 2);
        _hashSet = new HashSet<string>();

        for (int i = 0; i < N; i++)
        {
            var guid = Guid.NewGuid().ToString();
            _data[i] = guid;
            _bloomFilter.Add(guid);
            _hashSet.Add(guid);
        }
    }

    [Benchmark]
    public void BloomFilter()
    {
        foreach (var guid in _data)
        {
            var result = _bloomFilter.MightContains(guid);
        }
    }

    [Benchmark]
    public void HashSet()
    {
        foreach (var guid in _data)
        {
            var result = _hashSet.Contains(guid);
        }
    }
}