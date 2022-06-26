using System.Buffers;
using System.Buffers.Binary;

namespace EventHorizon.BloomFilter;

class ByteBuffer : IDisposable
{
    private readonly int _capacity;
    private readonly byte[] _buffer;
    private int _offset;
    private bool _disposed;

    public ByteBuffer(int capacity)
    {
        _capacity = capacity;
        _buffer = ArrayPool<byte>.Shared.Rent(capacity);
    }

    public void Put(byte b)
    {
        CheckInsertable();
        _buffer[_offset] = b;
        _offset++;
    }

    public void Put(byte[] bytes)
    {
        CheckInsertable();
        bytes.CopyTo(_buffer.AsSpan(_offset, bytes.Length));
        _offset += bytes.Length;
    }

    public void PutInt(int i)
    {
        CheckInsertable();
        BinaryPrimitives.WriteInt32BigEndian(GetRemainingAsSpan(), i);
        _offset += sizeof(int);
    }
    
    public void PutShort(short s)
    {
        CheckInsertable();
        BinaryPrimitives.WriteInt32BigEndian(GetRemainingAsSpan(), s);
        _offset += sizeof(short);
    }

    // ... 其他的 primitive type 的实现

    public Span<byte> GetBuffer() =>
        _buffer.AsSpan(.._offset);

    public bool HasRemaining() => _offset < _capacity;

    public void Dispose()
    {
        _disposed = true;
        ArrayPool<byte>.Shared.Return(_buffer);
    }

    private void CheckInsertable()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(typeof(ByteBuffer).FullName);
        }

        if (_offset >= _capacity)
        {
            throw new OverflowException("Byte buffer overflow");
        }
    }

    private Span<byte> GetRemainingAsSpan() => _buffer.AsSpan(_offset..);
}