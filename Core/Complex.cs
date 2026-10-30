namespace Core;

public class Complex(double real, double imaginary)
{
    public double Real { get; } = real;
    public double Imaginary { get; } = imaginary;
    public double Magnitude { get; } = double.Hypot(real, imaginary);

    public static Complex operator +(Complex a, Complex b)
    {
        var real = a.Real + b.Real;
        var imaginary = a.Imaginary + b.Imaginary;
        return new Complex(real, imaginary);
    }

    public static Complex operator -(Complex a, Complex b)
    {
        var real = a.Real - b.Real;
        var imaginary = a.Imaginary - b.Imaginary;
        return new Complex(real, imaginary);
    }

    public static Complex operator *(Complex a, Complex b)
    {
        var real = a.Real * b.Real - a.Imaginary * b.Imaginary;
        var imaginary = a.Real * b.Imaginary + a.Imaginary * b.Real;
        return new Complex(real, imaginary);
    }

    public static Complex operator /(Complex a, Complex b)
    {
        // standard division
        // var denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
        // if (denominator == 0) throw new DivideByZeroException();
        // var real = (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator;
        // var imaginary = (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator;
        // return new Complex(real, imaginary);
        
        // Smith's division algorithm
        if (b.Real == 0 && b.Imaginary == 0) throw new DivideByZeroException();

        var real = 0.0;
        var imaginary = 0.0;
        if (double.Abs(b.Real) >= double.Abs(b.Imaginary))
        {
            var ratio = b.Imaginary / b.Real;
            var denominator = b.Real + b.Imaginary * ratio;

            real = (a.Real + a.Imaginary * ratio) / denominator;
            imaginary = (a.Imaginary - a.Real * ratio) / denominator;
        }
        else
        {
            var ratio = b.Real / b.Imaginary;
            var denominator = b.Imaginary + b.Real * ratio;

            real = (a.Real * ratio + a.Imaginary) / denominator;
            imaginary = (a.Imaginary * ratio - a.Real) / denominator;
        }

        return new Complex(real, imaginary);
    }

    public double DistanceTo(Complex other)
    {
        return double.Hypot(this.Real - other.Real, this.Imaginary - other.Imaginary);
    }

    public bool IsSimilarTo(Complex other, double tolerance)
    {
        return this.DistanceTo(other) <= tolerance;
    }

    public static Complex[] GenerateRandom(int number, double minMagnitude = 0, double maxMagnitude = double.PositiveInfinity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(number);
        if (minMagnitude < 0 || minMagnitude > maxMagnitude) throw new ArgumentException("Invalid magnitude range!");
        
        var random = new Random();
        var array = new Complex[number];
        for (var index = 0; index < number; index++)
        {
            var magnitude = random.NextDouble() * (maxMagnitude - minMagnitude) + minMagnitude;
            var angle = random.NextDouble() * 2 * Math.PI;
            var real = magnitude * double.Cos(angle);
            var imaginary = magnitude * double.Sin(angle);
            array[index] = new Complex(real, imaginary);
        }
        return array;
    }
    
    public static Complex Zero { get; } = new Complex(0, 0);
    public static Complex One { get; } = new Complex(1, 0);
}