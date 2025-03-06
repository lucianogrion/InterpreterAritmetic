// See https://aka.ms/new-console-template for more information


try
{
    
    ExpressionParser parser = new ExpressionParser();
    IExpression expression = parser.Parse("66+99*2+36");
    Console.WriteLine($"Resultado: {expression.Evaluate()}");

    expression = parser.Parse("66+99*((2+36)+1)");
    Console.WriteLine($"Resultado: {expression.Evaluate()}");

    expression = parser.Parse("66+99*2+36 + 8 * 2 - 4 / 2");
    Console.WriteLine($"Resultado: {expression.Evaluate()}");

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

public interface IExpression
{
    int Evaluate();
}

// Expresión de número
public class NumberExpression : IExpression
{
    private int _number;
    public NumberExpression(int number)
    {
        _number = number;
    }
    public int Evaluate()
    {
        return _number;
    }
}

// Expresión para suma
public class AddExpression : IExpression
{
    private IExpression _left, _right;
    public AddExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    public int Evaluate()
    {
        return _left.Evaluate() + _right.Evaluate();
    }
}

// Expresión para resta
public class SubtractExpression : IExpression
{
    private IExpression _left, _right;
    public SubtractExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    public int Evaluate()
    {
        return _left.Evaluate() - _right.Evaluate();
    }
}

// Expresión para multiplicación
public class MultiplyExpression : IExpression
{
    private IExpression _left, _right;
    public MultiplyExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Evaluate()
    {
        return _left.Evaluate() * _right.Evaluate();
    }
}

// Expresión para división
public class DivideExpression : IExpression
{
    private IExpression _left, _right;
    public DivideExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    public int Evaluate()
    {
        return _left.Evaluate() / _right.Evaluate();
    }
}


// Analizador de expresiones en notación infija
public class ExpressionParser
{
    private Queue<string> tokens;
    public IExpression Parse(string expression)
    {
        Console.Write("Tokenize(expression)=> ");
        foreach (string item in Tokenize(expression))
        {
            Console.Write($"|{item}| ");
        }
        Console.WriteLine("");

        tokens = new Queue<string>(Tokenize(expression));
        return ParseExpression();
    }

    private List<string> Tokenize(string expression)
    {
        List<string> tokens = new List<string>();
        string numberBuffer = "";

        foreach (char c in expression.Replace(" ", ""))
        {
            if (char.IsDigit(c))
            {
                numberBuffer += c;
            }
            else
            {
                if (!string.IsNullOrEmpty(numberBuffer))
                {
                    tokens.Add(numberBuffer);
                    numberBuffer = "";
                }
                tokens.Add(c.ToString());
            }
        }

        if (!string.IsNullOrEmpty(numberBuffer))
        {
            tokens.Add(numberBuffer);
        }

        return tokens;
    }

    private IExpression ParseExpression()
    {
        IExpression left = ParseTerm();

        while (tokens.Count > 0 && (tokens.Peek() == "+" || tokens.Peek() == "-"))
        {
            string op = tokens.Dequeue();
            IExpression right = ParseTerm();

            if (op == "+")
                left = new AddExpression(left, right);
            else
                left = new SubtractExpression(left, right);
        }

        return left;
    }

    private IExpression ParseTerm()
    {
        IExpression left = ParseFactor();

        while (tokens.Count > 0 && (tokens.Peek() == "*" || tokens.Peek() == "/"))
        {
            string op = tokens.Dequeue();
            IExpression right = ParseFactor();

            if (op == "*")
                left = new MultiplyExpression(left, right);
            else
                left = new DivideExpression(left, right);
        }

        return left;
    }

    private IExpression ParseFactor()
    {
        string token = tokens.Dequeue();

        if (int.TryParse(token, out int number))
        {
            return new NumberExpression(number);
        }
        else if (token == "(")
        {
            IExpression expression = ParseExpression();
            tokens.Dequeue(); // Consumir el ')'
            return expression;
        }
        else
        {
            throw new InvalidOperationException($"Token inesperado: {token}");
        }
    }
}


