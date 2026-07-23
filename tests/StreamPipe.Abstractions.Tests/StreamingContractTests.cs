using StreamPipe;

namespace StreamPipe.Abstractions.Tests;

public class StreamingContractTests
{
    // REQ-API-009 / REQ-API-014 — contracts expose schema and streaming operations without full-result collections.
    [Fact]
    public void Contracts_REQ_API_001_Expose_Pull_And_Push_Surfaces()
    {
        Assert.True(typeof(IDataStream).IsInterface);
        Assert.True(typeof(IDataSink).IsInterface);
        Assert.True(typeof(IAsyncDisposable).IsAssignableFrom(typeof(IDataStream)));
        Assert.True(typeof(IAsyncDisposable).IsAssignableFrom(typeof(IDataSink)));

        Assert.NotNull(typeof(IDataStream).GetProperty(nameof(IDataStream.Schema)));
        Assert.NotNull(typeof(IDataStream).GetMethod(nameof(IDataStream.ReadAsync)));
        Assert.NotNull(typeof(IDataStream).GetProperty(nameof(IDataStream.Current)));

        Assert.NotNull(typeof(IDataSink).GetProperty(nameof(IDataSink.Schema)));
        Assert.NotNull(typeof(IDataSink).GetMethod(nameof(IDataSink.WriteAsync)));
        Assert.NotNull(typeof(IDataSink).GetMethod(nameof(IDataSink.CompleteAsync)));
    }

    // REQ-API-017 — public errors expose category and message.
    [Fact]
    public void StreamPipeException_REQ_API_017_Exposes_Category_And_Message()
    {
        var ex = new StreamPipeFormatException("invalid");
        Assert.Equal(StreamPipeErrorCategory.Format, ex.Category);
        Assert.Equal("invalid", ex.Message);
    }
}
