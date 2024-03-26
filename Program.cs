using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Calculator;
using static Calculator.CalculatorSupport;



string Expression;
char[] operators = {'+', '-', '%', '/', '*', '√' };
bool IsNumber = false;
int number = 0, i = 0;



Console.WriteLine("Эта программа может выполнять операции '+', '-', '/', '*', '√' над N числами," + "\n а также нахождение процента одного числа от другого при вводе двух чисел.\n" +
    "Введите численное выражение: ");


Expression = Console.ReadLine();



string[] Numbers = ByElementToStringArray(Expression, operators, IsNumber, ref i);
if (Numbers == null) { return; }
object[] Values = ByElementToObjectArray(Numbers, IsNumber, i, number);
if (Values == null) { return; }

Console.WriteLine("\n\nВыражение в массиве:");

foreach (object Component in Values)
    Console.WriteLine($"{Component}");

while (Values[1] != null)
{
    Values = SimplifyExpression("√", null, Values);
    if (Values[1] == null) { break; }

    Values = SimplifyExpression("%", null, Values);
    if (Values[1] == null) { break; }

    Values = SimplifyExpression("*", "/", Values);
    if (Values[1] == null) { break; }

    Values = SimplifyExpression("+", "-", Values);
    if (Values[1] == null) { break; }
}


Console.WriteLine($"\n\n\tОтвет: {Values[0]}.");



