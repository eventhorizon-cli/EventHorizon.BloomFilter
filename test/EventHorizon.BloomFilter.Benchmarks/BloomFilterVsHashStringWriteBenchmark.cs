using BenchmarkDotNet.Attributes;
using EventHorizon.BloomFilter;

public class BloomFilterVsHashStringWriteBenchmark
{
    private string[] _data;

    [Params(1_000, 10_000, 100_000)] public int N;

    [GlobalSetup]
    public void Setup()
    {
        _data = new string[N];

        for (int i = 0; i < N; i++)
        {
            _data[i] = Guid.NewGuid().ToString();
        }
    }

    [Benchmark]
    public void BloomFilter()
    {
        var bf = new BloomFilter<string>(Funnels.StringFunnel, N / 10, 2);
        foreach (var item in _data)
        {
            bf.Add(item);
        }
    }

    [Benchmark]
    public void HashSet()
    {
        var hashSet = new HashSet<string>();
        foreach (var item in _data)
        {
            hashSet.Add(item);
        }
    }
}