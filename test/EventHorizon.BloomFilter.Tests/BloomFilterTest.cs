namespace EventHorizon.BloomFilter.Tests;

public class BloomFilterTest
{
    [Fact]
    public void Custom_type_bloom_filter_can_be_added_successfully()
    {
        // Arrange
        var bf = new BloomFilter<Foo>(
            (foo, sink) =>
            {
                sink.PutString(foo.A, Encoding.UTF8);
                sink.PutInt(foo.B);
                sink.PutObject(foo.Bar, (bar, barSink) => barSink.PutBool(bar.C));
            }, 1000, 4, 50);

        var foo1 = new Foo
        {
            A = "AAA",
            B = 123,
            Bar = new Bar
            {
                C = true
            }
        };

        var foo2 = new Foo
        {
            A = "AAA",
            B = 123,
            Bar = new Bar
            {
                C = true
            }
        };

        var foo3 = new Foo
        {
            A = "BBB",
            B = 123,
            Bar = new Bar
            {
                C = true
            }
        };
        // Act
        bf.Add(foo1);

        // Assert
        Assert.True(bf.MightContains(foo1));
        Assert.True(bf.MightContains(foo2));
        Assert.False(bf.MightContains(foo3));
    }

    class Foo
    {
        public string A { get; set; }

        public int B { get; set; }

        public Bar Bar { get; set; }
    }

    class Bar
    {
        public bool C { get; set; }
    }
}