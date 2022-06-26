using System.Text;

namespace EventHorizon.BloomFilter;

internal class Sink : ISink, IDisposable
{
    private readonly ByteBuffer _byteBuffer;

    /// <summary>
    /// 创建一个新的 <see cref="Sink"/> 实例
    /// </summary>
    /// <param name="expectedInputSize">预计输入的单个元素的最大大小</param>
    public Sink(int expectedInputSize)
    {
        _byteBuffer = new ByteBuffer(expectedInputSize);
    }

    public ISink PutByte(byte b)
    {
        _byteBuffer.Put(b);
        return this;
    }

    public ISink PutBytes(byte[] bytes)
    {
        _byteBuffer.Put(bytes);
        return this;
    }

    public ISink PutBool(bool b)
    {
        _byteBuffer.Put((byte)(b ? 1 : 0));
        return this;
    }

    public ISink PutShort(short s)
    {
        _byteBuffer.PutShort(s);
        return this;
    }

    public ISink PutInt(int i)
    {
        _byteBuffer.PutInt(i);
        return this;
    }

    public ISink PutString(string s, Encoding encoding)
    {
        _byteBuffer.Put(encoding.GetBytes(s));
        return this;
    }

    public ISink PutObject<T>(T obj, Funnel<T> funnel)
    {
        funnel(obj, this);
        return this;
    }

    public byte[] GetBytes() => _byteBuffer.GetBuffer().ToArray();

    public void Dispose()
    {
        _byteBuffer.Dispose();
    }
}