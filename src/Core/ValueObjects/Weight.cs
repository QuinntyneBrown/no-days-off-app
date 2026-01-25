using Core.Exceptions;

namespace Core.ValueObjects;

/// <summary>
/// Value object representing weight in kilograms
/// </summary>
public sealed class Weight : ValueObject
{
    public int Kilograms { get; }

    private Weight(int kilograms)
    {
        Kilograms = kilograms;
    }

    public static Weight FromKilograms(int kilograms)
    {
        if (kilograms < 0)
        {
            throw new ValidationException("Weight cannot be negative");
        }

        return new Weight(kilograms);
    }

    public static Weight Zero => new(0);

    public double ToPounds() => Kilograms * 2.20462;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Kilograms;
    }

    public override string ToString() => $"{Kilograms} kg";
}
