// See https://aka.ms/new-console-template for more information
using System;

string strExpression = "66+99*2+36";
Console.WriteLine($"ResolveExpresion {strExpression}");
Console.WriteLine(ResolveExpresion(strExpression));

static int ResolveExpresion(string expression)
{
    int parsedNumber = 0;
    List<string> OperationList = new List<string>();
    //"66+99*2+34"
    string number = String.Empty;

    for (int i = 0; i < expression.Length; i++)
     {
        string theChar = expression[i].ToString();

        if (int.TryParse(theChar,  out parsedNumber))
        {
            number = number + theChar;
        }
        else
        {
            OperationList = ParseChars(theChar,number, OperationList);
            number = "";
        }

    }

    if (number!="")
    {
        OperationList.Add(number);
    }


    //foreach (string operation in OperationList) 
    //{
    //    Console.Write(operation);
    //    Console.Write(" ");
    //}
    //Console.WriteLine("");


    //First resolve the *
    OperationList = ResolveSign("*", OperationList);
    //Second resolve the +
    OperationList = ResolveSign("+", OperationList);

    
    return int.Parse(OperationList[0]);
}

static List<string> ParseChars(string theChar, string number, List<string> OperationList)
{
    int parsedNumber = 0;

    if (theChar == "+")
    {
        //add the number to a leaf of the tree
        //and add the + to the node 
        if (int.TryParse(number, out parsedNumber))
        {
            OperationList.Add(number);
            OperationList.Add(theChar);
            
        }

    }

    if (theChar == "*")
    {
        //add the number to a leaf of the tree
        //and add the + to the node 
        if (int.TryParse(number, out parsedNumber))
        {
            OperationList.Add(number);
            OperationList.Add(theChar);
            number = "";
        }

    }

    return (List<string>)OperationList.Clone();
}

static List<string> ResolveSign(string Sign, List<string> OperationList)
{
    int index = 0;
    int NextNumber = 0;
    int PreviousNumber = 0;
    int result = 0;

    List<string> Returned = new List<string>();

    while (OperationList.Any(w => w == Sign))
    {
        //I need the left and the right values and solve item
        index = OperationList.FindIndex(w => w == Sign);

        NextNumber = int.Parse(OperationList[index + 1]);
        PreviousNumber = int.Parse(OperationList[index - 1]);
        if (Sign == "*")
        {
            result = NextNumber * PreviousNumber;
        }
        if (Sign == "+")
        {
            result = NextNumber + PreviousNumber;
        }
        //Replace on the operation List
        OperationList[index - 1] = result.ToString();
        OperationList.RemoveAt(index);
        OperationList.RemoveAt(index);
    }
    return (List<string>)OperationList.Clone();
}


static class Extensions
{
    public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
    {
        return listToClone.Select(item => (T)item.Clone()).ToList();
    }

}