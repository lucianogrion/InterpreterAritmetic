# InterpreterAritmetic
InterpreterAritmetic
Explicación del Analizador Sintáctico (Parser)
El objetivo del parser es convertir una cadena como 3 + 4 * (2 - 1) en una estructura en memoria (árbol de expresiones) que podamos evaluar correctamente.

Para lograr esto, necesitamos respetar la jerarquía de operadores:

Los paréntesis tienen la máxima prioridad.
Multiplicación y división (*, /) tienen prioridad sobre suma y resta.
Suma y resta (+, -) tienen la menor prioridad y se evalúan de izquierda a derecha.
ParseExpression()
📌 Maneja suma (+) y resta (-) porque tienen la menor prioridad.

Primero llama a ParseTerm(), que se encarga de manejar multiplicación y división.
Luego, consume + o - y sigue procesando términos.
csharp
Copy
Edit
private IExpression ParseExpression()
{
    IExpression left = ParseTerm(); // Primero parseamos un término

    while (tokens.Count > 0 && (tokens.Peek() == "+" || tokens.Peek() == "-"))
    {
        string op = tokens.Dequeue(); // Tomamos el operador
        IExpression right = ParseTerm(); // Procesamos el siguiente término

        if (op == "+")
            left = new AddExpression(left, right);
        else
            left = new SubtractExpression(left, right);
    }

    return left; // Devuelve la expresión completa
}
💡 Ejemplo:
Entrada: "3 + 4 * 2"

ParseTerm() procesa 4 * 2 antes que 3 +
Se genera el árbol de expresiones correctamente.
ParseTerm()
📌 Maneja multiplicación (*) y división (/), que tienen mayor prioridad que + y -.

Primero llama a ParseFactor(), que obtiene un número o un paréntesis.
Luego, consume * o / y sigue procesando factores.
csharp
Copy
Edit
private IExpression ParseTerm()
{
    IExpression left = ParseFactor(); // Primero obtenemos un factor

    while (tokens.Count > 0 && (tokens.Peek() == "*" || tokens.Peek() == "/"))
    {
        string op = tokens.Dequeue(); // Tomamos el operador
        IExpression right = ParseFactor(); // Procesamos el siguiente factor

        if (op == "*")
            left = new MultiplyExpression(left, right);
        else
            left = new DivideExpression(left, right);
    }

    return left;
}
💡 Ejemplo:
Entrada: "4 * 2"

ParseFactor() obtiene 4
ParseFactor() obtiene 2
Se genera la operación 4 * 2
ParseFactor()
📌 Maneja números y paréntesis.

Si el token es un número (3, 4, 5...), lo convierte en una NumberExpression.
Si el token es (, significa que hay una subexpresión dentro de paréntesis, así que llama recursivamente a ParseExpression().
csharp
Copy
Edit
private IExpression ParseFactor()
{
    string token = tokens.Dequeue(); // Tomamos el siguiente token

    if (int.TryParse(token, out int number))
    {
        return new NumberExpression(number); // Es un número
    }
    else if (token == "(")
    {
        IExpression expression = ParseExpression(); // Procesamos lo que está dentro de los paréntesis
        tokens.Dequeue(); // Consumimos el ')' correspondiente
        return expression;
    }
    else
    {
        throw new InvalidOperationException($"Token inesperado: {token}");
    }
}
💡 Ejemplo:
Entrada: "(2 - 1)"

Consume (
Llama a ParseExpression() para procesar 2 - 1
Consume )
Devuelve la subexpresión 2 - 1
Ejemplo Completo Paso a Paso
Entrada: "3 + 4 * (2 - 1)"
plaintext
Copy
Edit
Tokens: ["3", "+", "4", "*", "(", "2", "-", "1", ")"]
ParseExpression()
Llama a ParseTerm() → (Busca términos antes de +)
ParseFactor() obtiene 3
Detecta +
Llama a ParseTerm() → (Procesa 4 * (2 - 1))
ParseFactor() obtiene 4
Detecta *
ParseFactor() encuentra ( y llama a ParseExpression()
ParseTerm() obtiene 2
Detecta -
ParseTerm() obtiene 1
ParseExpression() devuelve 2 - 1
ParseFactor() devuelve (2 - 1)
ParseTerm() devuelve 4 * (2 - 1)
ParseExpression() devuelve 3 + (4 * (2 - 1))
✔ Árbol de Expresiones Generado:

markdown
Copy
Edit
      +
     / \
    3   *
       / \
      4   -
         / \
        2   1
✔ Evaluación:

Copy
Edit
2 - 1 = 1
4 * 1 = 4
3 + 4 = 7
➡ Resultado: 7

Resumen
✅ ParseExpression(): Maneja + y - (menor prioridad)
✅ ParseTerm(): Maneja * y / (mayor prioridad)
✅ ParseFactor(): Maneja números y paréntesis (máxima prioridad)
