using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal static class CalculatorSupport 
    {
        public static string[] ByElementToStringArray(string Expression, char[] operators, bool IsNumber, ref int i)
        {
            int number;
            string[] Numbers = new string[Expression.Length];

            foreach (char Component in Expression)
            {
                IsNumber = Int32.TryParse(Component.ToString(), out number);

                if (Component == ' ' && !IsChange(Numbers[i], IsNumber))
                    continue;

                switch (IsNumber)
                {
                    case true:
                        if (IsChange(Numbers[i], IsNumber) == true)
                            i++;
                        Numbers[i] += Component;
                        break;

                    case false:
                        if (IsChange(Numbers[i], IsNumber) == true || Component == '√')
                        {
                            if (Numbers[i] != null && Component == '√')
                                i++;
                            if (Component != '√')
                                i++;
                            if (Component == ' ')
                                break;
                        }

                        if (operators.Contains(Component) && Numbers[i] == null)
                        {
                            Numbers[i] += Component;
                            break;
                        }
                        else
                        {
                            Console.WriteLine(" Ошибка ввода");
                            return null;
                        }
                }
            }

            return Numbers;
        }

        public static object[] ByElementToObjectArray(string[] Numbers, bool IsNumber, int i, int number)
        {
            int N = ++i, LastNumber;
            object[] Values = new object[N];
            int j = 0;

            foreach (string Component in Numbers)
            {
                if (Component == null) { continue; }
                IsNumber = Int32.TryParse(Component.ToString(), out number);

                switch (IsNumber)
                {
                    case false:

                        if ((j == N - 1) | (j == 0 && (Component != "-" && Component != "+" && Component != "√")))
                        { break; }

                        Values[j] = Component;
                        j++;
                        continue;

                    case true:
                        switch (j)
                        {
                            case 0:
                                Values[j] = number;
                                j++;
                                continue;
                            case > 0:
                                IsNumber = Int32.TryParse(Values[j - 1].ToString(), out LastNumber);
                                switch (IsNumber)
                                {
                                    case false:
                                        Values[j] = number;
                                        j++;
                                        continue;
                                    case true:
                                        break;
                                }
                                break;
                        }
                        break;
                }

                Console.WriteLine(" Ошибка ввода");
                return null;
            }

            return Values;

        }

        public static object[] SimplifyExpression(string Component1, string Component2, object[] Values)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                object j = 0;

                if (Values[i] == null) continue;
                if (Values[i].ToString() == Component1) 
                {
                    if (i == 0)
                        Calculation(ref j, ref Values[i], ref Values[i + 1], Values);
                    else
                        Calculation(ref Values[i - 1], ref Values[i], ref Values[i + 1], Values);

                    Values = ConnectElements(Values, Values[i], ref i);

                    continue;
                }
                if (Values[i].ToString() == Component2) 
                {
                    if (i == 0)
                        Calculation(ref j, ref Values[i], ref Values[i + 1], Values);
                    else
                        Calculation(ref Values[i - 1], ref Values[i], ref Values[i + 1], Values);

                    Values = ConnectElements(Values, Values[i], ref i);

                    continue;
                }
            }

            return Values;
        }
        
        private static bool IsChange(string Element, bool ActualValue)
        {
            if (Element == null) return false;

            int number;
            bool IsNumber = Int32.TryParse(Element, out number);

            if (IsNumber != ActualValue) return true;
            else return false;
        }

        private static void Calculation(ref object PreviousElement, ref object ActualElement,ref object NextElement, object[] Values)
        {
            switch (ActualElement)
            {
                case "√":
                    ActualElement = (int)Math.Sqrt((int)NextElement);
                    NextElement = null;
                    break;
                case "%":
                    int counter = 0;
                    for (int i = 0; i < Values.Length; i++)
                    {
                        if (Values[i] is int)
                            counter++;
                    }

                    if (counter != 2)
                    {
                        Console.WriteLine(" Ошибка ввода");
                        Environment.Exit(0);
                    }
                    else
                    {
                        ActualElement = (double)(int)PreviousElement / (double)(int)NextElement * 100;
                        Console.WriteLine($"\n\n\tОтвет: Процент {PreviousElement} от {NextElement} это {ActualElement} %.");
                        return;
                    }

                    break;
                case "*":
                    ActualElement = (int)PreviousElement * (int)NextElement;
                    PreviousElement = null;
                    NextElement = null;
                    break;
                case "/":
                    ActualElement = (int)PreviousElement / (int)NextElement;
                    PreviousElement = null;
                    NextElement = null;
                    break;
                case "+":
                    ActualElement = (int)PreviousElement + (int)NextElement;
                    PreviousElement = null;
                    NextElement = null;
                    break;
                case "-":
                    ActualElement = (int)PreviousElement - (int)NextElement;
                    PreviousElement = null;
                    NextElement = null;
                    break;
            }
        }

        private static object[] ConnectElements(object[] Values, object ActualValue, ref int IndexOfActualValue)
        {   
            for (int i = 0; i < Values.Length; i++)
            {
                if (Values[i] != null && i != 0 && Values[i-1] == null)
                {
                    if (Values[i] == ActualValue)
                        IndexOfActualValue--;

                    Values[i - 1] = Values[i];
                    Values[i] = null;
                    i = 0;
                }
            }

            return Values;
        }

    }
}
