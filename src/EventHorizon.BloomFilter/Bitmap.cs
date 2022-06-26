namespace EventHorizon.BloomFilter;

internal class Bitmap
{
    private readonly byte[] _bytes;
    private readonly long _capacity;

    public Bitmap(long capacity)
    {
        _capacity = capacity;
        _bytes = new byte[_capacity / 8 + 1];
    }

    public long Capacity => _capacity;

    public void Set(long index)
    {
        if (index >= _capacity)
        {
            throw new IndexOutOfRangeException();
        }

        // 计算出数据存在第几个 byte 上
        long byteIndex = index / 8;
        // 计算出数据存在第几个 bit 上
        int bitIndex = (int)(index % 8);
        _bytes[byteIndex] |= (byte)(1 << bitIndex);
    }

    public void Remove(long index)
    {
        if (index >= _capacity)
        {
            throw new IndexOutOfRangeException();
        }

        long byteIndex = index / 8;
        int bitIndex = (int)(index % 8);
        _bytes[byteIndex] &= (byte)~(1 << bitIndex);
    }

    public bool Get(long index)
    {
        if (index >= _capacity)
        {
            throw new IndexOutOfRangeException();
        }

        long byteIndex = index / 8;
        int bitIndex = (int)(index % 8);

        return (_bytes[byteIndex] & (byte)(1 << bitIndex)) != 0;
    }
}