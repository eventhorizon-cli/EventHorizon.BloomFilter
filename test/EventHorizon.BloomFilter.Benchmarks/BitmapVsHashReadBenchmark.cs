using BenchmarkDotNet.Attributes;
using EventHorizon.BloomFilter;

public class BitmapVsHashReadBenchmark
{
    private bool[] _data;
    private Bitmap _bitmap;
    private HashSet<int> _hashSet;

    [Params(1_000, 10_000, 100_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {
        _data = new bool[N];
        _bitmap = new Bitmap(N);
        _hashSet = new HashSet<int>();

        var bytes = new byte[N];
        new Random().NextBytes(bytes);
        for (int i = 0; i < N; i++)
        {
            bool isTure = bytes[i] % 2 == 0;
            _data[i] = isTure;
            if (isTure)
            {
                _bitmap.Set(i);
                _hashSet.Add(i);
            }
        }
    }

    [Benchmark]
    public void Bitmap()
    {
        for (int i = 0; i < N; i++)
        {
            var result = _bitmap.Get(i);
            if (result != _data[i])
            {
                throw new Exception("Bitmap mismatch");
            }
        }
    }

    [Benchmark]
    public void HashSet()
    {
        for (int i = 0; i < N; i++)
        {
            var result = _hashSet.Contains(i);
            if (result != _data[i])
            {
                throw new Exception("HashSet mismatch");
            }
        }
    }
}