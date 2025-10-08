namespace CandidateManagement.Domain.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; init; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("PhoneNumber required", nameof(value));
        }
        value = value.Trim();
        if (!IsValidPhoneNumber(value))
        {
            throw new ArgumentException("Phone number must contain digits", nameof(value));
        }
        Value = value;
    }

    private static bool IsValidPhoneNumber(string number)
    {
        return number.Any(char.IsDigit);
    }

    public override string ToString() => Value;
}