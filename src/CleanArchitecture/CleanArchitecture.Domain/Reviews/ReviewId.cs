namespace CleanArchitecture.Domain
{
    public record ReviewId
    {
        public Guid Value { get; init; }
        public ReviewId(Guid value)
        {
            Value = value;
        }
        public static ReviewId New() => new(Guid.NewGuid());
    }
}