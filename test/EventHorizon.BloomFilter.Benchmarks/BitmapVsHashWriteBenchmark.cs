using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EventHorizon.BloomFilter;

public class BitmapVsHashWriteBenchmark
{
    private bool[] _data;

    [Params(1_000, 10_000, 100_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _data = new bool[N];

        var bytes = new byte[N];
        new Random().NextBytes(bytes);
        for (int i = 0; i < N; i++)
        {
            _data[i] = bytes[i] % 2 == 0;
        }
    }

    [Benchmark]
    public void Bitmap()
    {
        var bitmap = new Bitmap(N);
        for (int i = 0; i < N; i++)
        {
            if (_data[i])
            {
                bitmap.Set(i);
            }
        }
    }

    [Benchmark]
    public void HashSet()
    {
        var hashSet = new HashSet<int>();
        for (int i = 0; i < N; i++)
        {
            if (_data[i])
            {
                hashSet.Add(i);
            }
        }
    }
}