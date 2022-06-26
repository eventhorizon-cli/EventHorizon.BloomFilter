using System.Text;

namespace EventHorizon.BloomFilter;

public interface ISink
{
    ISink PutByte(byte b);
    
    ISink PutBytes(byte[] bytes);

    ISink PutBool(bool b);
    
    ISink PutShort(short s);

    ISink PutInt(int i);

    ISink PutString(string s, Encoding encoding);

    ISink PutObject<T>(T obj, Funnel<T> funnel);

    /// ... 其他 built-in 类型，读者可自行补充
}