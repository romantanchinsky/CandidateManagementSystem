using System.Net.Mail;

namespace CandidateManagement.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; init; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email required", nameof(value));
        }
        value = value.ToLower().Trim();
        if (!IsValidEmail(value))
        {
            throw new ArgumentException("Email format is invalid", nameof(value));
        }
        Value = value;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var temp = new MailAddress(email);
            return temp.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString() => Value;
}