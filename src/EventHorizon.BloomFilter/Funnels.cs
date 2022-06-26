using System.Text;

namespace EventHorizon.BloomFilter;

public delegate void Funnel<in T>(T from, ISink sink);

public class Funnels
{
    public static Funnel<string> StringFunnel = (from, sink) =>
        sink.PutString(from, Encoding.UTF8);
    
    public static Funnel<int> IntFunnel = (from, sink) =>
        sink.PutInt(from);
}