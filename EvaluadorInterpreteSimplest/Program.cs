using System;
using System.Collections.Generic;

interface IExpression
{
    int Interpret();
}

// Clases concretas para números y operaciones
class Number : IExpression
{
    private int _value;
    public Number(int value) => _value = value;
    public int Interpret() => _value;
}

class Add : IExpression
{
    private IExpression _left, _right;
    public Add(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    public int Interpret() => _left.Interpret() + _right.Interpret();
}
class Sub : IExpression
{
    private IExpression _left, _right;
    public Sub(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    public int Interpret() => _left.Interpret() - _right.Interpret();
}


class Multiply : IExpression
{
    private IExpression _left, _right;
    public Multiply(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    public int Interpret() => _left.Interpret() * _right.Interpret();
}


class Divide : IExpression
{
    private IExpression _left, _right;
    public Divide(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }
    public int Interpret() => _left.Interpret() / _right.Interpret();
}

// Parser que convierte una cadena en un árbol de expresión
class ExpressionParser
{
    private Queue<string> tokens;

    public ExpressionParser(string expression)
    {
        tokens = new Queue<string>(Tokenize(expression));
    }

    private List<string> Tokenize(string expression)
    {
        List<string> tokens = new List<string>();
        string number = "";

        foreach (char c in expression)
        {
            if (char.IsDigit(c))
            {
                number += c; // Acumulamos dígitos para formar números
            }
            else if ("+-*/".Contains(c))
            {
                if (number != "")
                {
                    tokens.Add(number);
                    number = "";
                }
                tokens.Add(c.ToString());
            }
        }

        if (number != "") tokens.Add(number); // Agregar último número
        return tokens;
    }

    public IExpression Parse()
    {
        return ParseAddSubtract();
    }

    private IExpression ParseAddSubtract()
    {
        IExpression left = ParseMultiplyDivide();

        while (tokens.Count > 0 && (tokens.Peek() == "+" || tokens.Peek() == "-"))
        {
            string op = tokens.Dequeue();
            IExpression right = ParseMultiplyDivide();
            left = (op == "+") ? new Add(left, right) : new Sub(left, right);
        }

        return left;
    }

    private IExpression ParseMultiplyDivide()
    {
        IExpression left = ParseNumber();

        while (tokens.Count > 0 && (tokens.Peek() == "*" || tokens.Peek() == "/"))
        {
            string op = tokens.Dequeue();
            IExpression right = ParseNumber();
            left = (op == "*") ? new Multiply(left, right) : new Divide(left, right);
        }

        return left;
    }

    private IExpression ParseNumber()
    {
        if (tokens.Count == 0) throw new Exception("Expresión inválida");

        return new Number(int.Parse(tokens.Dequeue()));
    }
}

class Program
{
    static void Main()
    {
        string input = "2 * 3 * 4 + 2 * 4";
        ExpressionParser parser = new ExpressionParser(input);
        IExpression expression = parser.Parse();
        Console.WriteLine($"Resultado: {expression.Interpret()}"); // Output: 11
    }
}