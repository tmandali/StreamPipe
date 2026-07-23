using StreamPipe;
using StreamPipe.Core;

namespace StreamPipe.Core.Tests;

public class DataStreamLifecycleTests
{
    private static DataSchema Schema { get; } =
        DataSchema.Create([new DataField("v", LogicalType.Int32, isNullable: false)]);

    // REQ-DATA-016 / REQ-API-009 — schema available before first record.
    [Fact]
    public async Task FixedRecordDataReader_REQ_DATA_016_Exposes_Schema_Before_Read()
    {
        await using var stream = new FixedRecordDataReader(Schema, []);
        Assert.Same(Schema, stream.Schema);
    }

    // REQ-API-010 — Current valid only after successful ReadAsync.
    [Fact]
    public async Task Current_REQ_API_010_Invalid_Before_Successful_Read()
    {
        await using var stream = new FixedRecordDataReader(
            Schema,
            [[ColumnValue.FromInt32(1)]]);

        Assert.Throws<StreamStateException>(
            (Action)(() => _ = stream.Current.Length));
    }

    // REQ-API-011 — false only for successful end-of-stream.
    [Fact]
    public async Task ReadAsync_REQ_API_011_Returns_False_Only_At_Successful_End()
    {
        await using var stream = new FixedRecordDataReader(
            Schema,
            [[ColumnValue.FromInt32(1)]]);

        Assert.True(await stream.ReadAsync());
        Assert.Equal(1, stream.Current[0].GetInt32());
        Assert.False(await stream.ReadAsync());
    }

    // REQ-DATA-017 — must not deliver a record after completion.
    [Fact]
    public async Task ReadAsync_REQ_DATA_017_Does_Not_Deliver_After_Completion()
    {
        await using var stream = new FixedRecordDataReader(
            Schema,
            [[ColumnValue.FromInt32(1)]]);

        Assert.True(await stream.ReadAsync());
        Assert.False(await stream.ReadAsync());
        Assert.Throws<StreamStateException>(
            (Action)(() => _ = stream.Current.Length));
    }

    // REQ-DATA-018 — successful completion distinct from cancellation and failure.
    [Fact]
    public void Lifecycle_REQ_DATA_018_Distinguishes_Terminal_Kinds()
    {
        var completed = new DataStreamLifecycle();
        completed.CompleteSuccessfully();
        Assert.Equal(StreamTerminalKind.Completed, completed.Terminal);

        var cancelled = new DataStreamLifecycle();
        cancelled.Cancel();
        Assert.Equal(StreamTerminalKind.Cancelled, cancelled.Terminal);

        var failed = new DataStreamLifecycle();
        failed.Fail(new InvalidOperationException("boom"));
        Assert.Equal(StreamTerminalKind.Failed, failed.Terminal);
    }

    // REQ-API-012 — concurrent ReadAsync is not allowed.
    [Fact]
    public void BeginRead_REQ_API_012_Rejects_Concurrent_Reads()
    {
        var lifecycle = new DataStreamLifecycle();
        lifecycle.BeginRead(CancellationToken.None);
        Assert.Throws<StreamStateException>(() => lifecycle.BeginRead(CancellationToken.None));
        lifecycle.EndRead();
    }

    // REQ-API-011 — cancellation must not be reported as false.
    [Fact]
    public async Task ReadAsync_REQ_API_011_Cancellation_Throws_Not_False()
    {
        await using var stream = new FixedRecordDataReader(
            Schema,
            [[ColumnValue.FromInt32(1)]]);

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await stream.ReadAsync(cts.Token));
    }

    // REQ-API-013 — disposing releases and invalidates current.
    [Fact]
    public async Task Dispose_REQ_API_013_Invalidates_Current()
    {
        var stream = new FixedRecordDataReader(
            Schema,
            [[ColumnValue.FromInt32(1)]]);

        Assert.True(await stream.ReadAsync());
        await stream.DisposeAsync();
        Assert.Throws<StreamStateException>(
            (Action)(() => _ = stream.Current.Length));
    }

    // REQ-MEM-001 / REQ-MEM-013 — fixed stream is bounded by supplied records, not an open-ended List accumulation API.
    [Fact]
    public async Task FixedRecordDataReader_REQ_MEM_001_Is_Bounded_By_Construction()
    {
        await using var stream = new FixedRecordDataReader(
            Schema,
            [
                [ColumnValue.FromInt32(1)],
                [ColumnValue.FromInt32(2)],
            ]);

        var count = 0;
        while (await stream.ReadAsync())
        {
            count++;
        }

        Assert.Equal(2, count);
    }
}
