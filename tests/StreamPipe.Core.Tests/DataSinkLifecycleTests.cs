using StreamPipe;
using StreamPipe.Core;

namespace StreamPipe.Core.Tests;

public class DataSinkLifecycleTests
{
    private static DataSchema Schema { get; } =
        DataSchema.Create([new DataField("v", LogicalType.Int32, isNullable: false)]);

    // REQ-API-014 — sink validates before accepting a write.
    [Fact]
    public async Task WriteAsync_REQ_API_014_Rejects_Invalid_Record()
    {
        var writes = 0;
        await using var sink = new ValidatingDataSink(
            Schema,
            (_, _) =>
            {
                writes++;
                return ValueTask.CompletedTask;
            });

        await Assert.ThrowsAsync<StreamPipeFormatException>(async () =>
            await sink.WriteAsync(Array.Empty<ColumnValue>()));
        Assert.Equal(0, writes);
    }

    // REQ-API-015 — reject writes after successful completion.
    [Fact]
    public async Task WriteAsync_REQ_API_015_Rejects_After_Complete()
    {
        await using var sink = new ValidatingDataSink(
            Schema,
            (_, _) => ValueTask.CompletedTask);

        await sink.CompleteAsync();
        await Assert.ThrowsAsync<StreamStateException>(async () =>
            await sink.WriteAsync(new[] { ColumnValue.FromInt32(1) }));
    }

    // REQ-API-015 — reject writes after disposal.
    [Fact]
    public async Task WriteAsync_REQ_API_015_Rejects_After_Dispose()
    {
        var sink = new ValidatingDataSink(
            Schema,
            (_, _) => ValueTask.CompletedTask);

        await sink.DisposeAsync();
        await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
            await sink.WriteAsync(new[] { ColumnValue.FromInt32(1) }));
    }

    // REQ-API-016 — CompleteAsync is idempotent after successful completion.
    [Fact]
    public async Task CompleteAsync_REQ_API_016_Is_Idempotent()
    {
        var completions = 0;
        await using var sink = new ValidatingDataSink(
            Schema,
            (_, _) => ValueTask.CompletedTask,
            _ =>
            {
                completions++;
                return ValueTask.CompletedTask;
            });

        await sink.CompleteAsync();
        await sink.CompleteAsync();
        Assert.Equal(1, completions);
    }

    // REQ-API-004 — cancellation can be supplied for blocking sink operations.
    [Fact]
    public async Task WriteAsync_REQ_API_004_Honors_CancellationToken()
    {
        await using var sink = new ValidatingDataSink(
            Schema,
            (_, _) => ValueTask.CompletedTask);

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await sink.WriteAsync(new[] { ColumnValue.FromInt32(1) }, cts.Token));
    }
}
