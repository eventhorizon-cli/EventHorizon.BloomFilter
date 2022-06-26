using System.Buffers.Binary;
using System.Security.Cryptography;
using Murmur;

namespace EventHorizon.BloomFilter;

public class BloomFilter<T>
{
    private readonly int _hashFunctions;
    private readonly Funnel<T> _funnel;
    private readonly int _expectedInputSize;
    private readonly Bitmap _bitmap;
    private readonly HashAlgorithm _murmur128;

    public BloomFilter(Funnel<T> funnel, int buckets, int hashFunctions = 2, int expectedInputSize = 128)
    {
        _hashFunctions = hashFunctions;
        _funnel = funnel;
        _expectedInputSize = expectedInputSize;

        _bitmap = new Bitmap(buckets);
        _murmur128 = MurmurHash.Create128(managed: false);
    }

    public void Add(T item)
    {
        long bitSize = _bitmap.Capacity;

        var (hash1, hash2) = Hash(item);

        long combinedHash = hash1;
        for (int i = 0; i < _hashFunctions; i++)
        {
            _bitmap.Set((combinedHash & long.MaxValue) % bitSize);
            combinedHash += hash2;
        }
    }


    public bool MightContains(T item)
    {
        long bitSize = _bitmap.Capacity;

        var (hash1, hash2) = Hash(item);

        long combinedHash = hash1;
        for (int i = 0; i < _hashFunctions; i++)
        {
            if (!_bitmap.Get((combinedHash & long.MaxValue) % bitSize))
            {
                return false;
            }

            combinedHash += hash2;
        }

        return true;
    }


    private (long Hash1, long Hash2) Hash(T item)
    {
        byte[] inputBytes;
        using (var sink = new Sink(_expectedInputSize))
        {
            sink.PutObject(item, _funnel);
            inputBytes = sink.GetBytes();
        }

        var hash = _murmur128.ComputeHash(inputBytes);

        long hash1 = LowerEight(hash);
        long hash2 = UpperEight(hash);
        return (hash1, hash2);
    }

    private long LowerEight(byte[] bytes) =>
        BinaryPrimitives.ReadInt64BigEndian(
            new[] { bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2], bytes[1], bytes[0] });

    private long UpperEight(byte[] bytes) =>
        BinaryPrimitives.ReadInt64BigEndian(
            new[] { bytes[15], bytes[14], bytes[13], bytes[12], bytes[11], bytes[10], bytes[9], bytes[8] });
}