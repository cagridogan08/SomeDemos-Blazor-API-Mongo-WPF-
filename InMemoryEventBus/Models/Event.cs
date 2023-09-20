namespace InMemoryEventBus.Models
{
    public record EventMetadata(string CorrelationId);

    public record Event<T>(T? Data, EventMetadata? Metadata = default);
}
