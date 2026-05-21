using System.Globalization;

namespace EasyShop.Domain.Primitives;

public enum CurrencyCode
{
    CUP,  // Cuban Peso
    USD,  // US Dollar
}

public class MoneyDto
{
    public int Amount { get; set; }      // en céntimos (unidades mínimas)
    public int Currency { get; set; }    // valor del enum, ej. 0 = USD
}

public sealed class Money : IEquatable<Money>, IComparable<Money>
{
    private readonly decimal _amount;
    private readonly CurrencyCode _currency;

    public Money(decimal amount, CurrencyCode currency)
    {
        if (amount < 0) throw new ArgumentException("El monto no puede ser negativo", nameof(amount));

        _amount = Math.Round(amount, GetDecimalPlaces(currency), MidpointRounding.AwayFromZero);
        _currency = currency;
    }

    public Money(int cents, CurrencyCode currency)
        : this(cents / 100m, currency) { }

    public decimal Amount => _amount;
    public CurrencyCode Currency => _currency;
    public int Cents => (int)(_amount * 100);

    // Obtener número de decimales según la moneda (JPY no tiene centavos)
    private static int GetDecimalPlaces(CurrencyCode currency) => currency switch
    {
        _ => 2
    };

    // Operaciones aritméticas básicas
    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(_amount + other._amount, _currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        if (_amount < other._amount)
            throw new InvalidOperationException("No se puede restar un monto mayor al actual (evita valores negativos)");
        return new Money(_amount - other._amount, _currency);
    }

    public Money Multiply(decimal factor)
    {
        if (factor < 0) throw new ArgumentException("Factor no puede ser negativo", nameof(factor));
        return new Money(_amount * factor, _currency);
    }

    public Money Multiply(int factor) => Multiply((decimal)factor);

    public Money Transform(Money changeValue)
    {
        return new Money(_amount + changeValue._amount, changeValue.Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));
        if (_currency != other._currency)
            throw new InvalidOperationException($"No se puede operar con monedas distintas: {_currency} vs {other._currency}");
    }

    // Métodos de comparación
    public int CompareTo(Money other)
    {
        if (other is null) return 1;
        EnsureSameCurrency(other);
        return _amount.CompareTo(other._amount);
    }

    public bool Equals(Money other)
    {
        if (other is null) return false;
        return _amount == other._amount && _currency == other._currency;
    }

    public override bool Equals(object obj) => Equals(obj as Money);

    public override int GetHashCode() => HashCode.Combine(_amount, _currency);

    public static bool operator ==(Money left, Money right) => Equals(left, right);
    public static bool operator !=(Money left, Money right) => !Equals(left, right);
    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
    public static Money operator +(Money a, Money b) => a.Add(b);
    public static Money operator -(Money a, Money b) => a.Subtract(b);
    public static Money operator *(Money a, decimal factor) => a.Multiply(factor);

    // Serialización eficiente: guardas int (código enum) + int cents
    public MoneyDto Serialize() => new MoneyDto
    {
        Amount = Cents,
        Currency = (int)_currency   // Guardar como entero
    };

    public static Money Deserialize(MoneyDto dto)
    {
        var currency = (CurrencyCode)dto.Currency;
        return new Money(dto.Amount, currency);
    }

    // ToString con símbolo cultural (opcional)
    public override string ToString()
    {
        var symbol = _currency switch
        {
            CurrencyCode.CUP => "$",
            CurrencyCode.USD => "$",
            _ => "?"
        };
        return $"{symbol}{_amount:F2}";
    }

    public string ToUI()
    {
        var symbol = _currency switch
        {
            CurrencyCode.CUP => "$",
            CurrencyCode.USD => "$",
            _ => "?"
        };
        return $"{symbol}{_amount:F2} {_currency}";
    }

    // Formato según cultura (útil solo para UI)
    //public string ToString(CultureInfo culture) => _amount.ToString("C", culture);
    //public override string ToString() => ToString(CultureInfo.CurrentCulture);
}