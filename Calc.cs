using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GB_CSharp_ConsoleCalc_Exception
{
    internal class Calc
    {
        private Stack<double> operationStack = new Stack<double>();

        public delegate void Operation(string msg);

        public event Operation CalcOperation;

        private double result { get; set; }

        private bool isRunning = true;
        public Calc()
        {
            this.result = 0;
        }

        void ConsoleMessage(string msg) => Console.WriteLine(msg);
        public double SetValue()
        {
            CalcOperation?.Invoke("Введите число: ");
            string value = Console.ReadLine()!;
            if (value.Contains("."))
            {
                return Convert.ToDouble(value);
            }
            else
            {

                return Convert.ToDouble(value.Replace(".", ","));
            }
        }
        public void Add()
        {
            result += SetValue();
            operationStack.Push(result);
            CalcOperation?.Invoke($"Результат: {result}");
        }

        public void Diff()
        {
            result -= SetValue();
            operationStack.Push(result);
            CalcOperation?.Invoke($"Результат: {result}");
        }

        public void Multiply()
        {
            result *= SetValue();
            operationStack.Push(result);
            CalcOperation?.Invoke($"Результат: {result}");
        }

        public void Divide()
        {
            try
            {
                double x = SetValue();
                if (x != 0)
                {
                    result /= x;
                    operationStack.Push(result);
                    CalcOperation?.Invoke($"Результат: {result}");
                }
                else
                {
                    throw new DivideByZeroException("Деление на ноль !!!!!");
                }
            }
            catch (DivideByZeroException ex) {
                CalcOperation?.Invoke(ex.Message);            
            }
        }

        public void Undo()
        {            
            try
            {
                operationStack.Pop();
                if (operationStack.Count == 0)
                {
                    result = 0;
                }
                else
                {
                    result = operationStack.Peek();
                }

                CalcOperation?.Invoke($"Последнее значение отменено. Текущее значение равно {result}");
            }
            catch (InvalidOperationException) {
                CalcOperation?.Invoke("Нет значений для отмены - нет выполненных операций!");
            }            
        }

        public bool Esc()
        {
            CalcOperation?.Invoke("Конец работы.");
            return false;
        }
        public void DefaultCase()
        {
            CalcOperation?.Invoke("Введена неверная операция.");
        }
        public void Welcome()
        {
            CalcOperation?.Invoke("Добро пожаловать, выберите одну из операций и следуйте интрукциям.");
        }

        public void Run()
        {
            CalcOperation += ConsoleMessage;
            Welcome();
            while (isRunning)
            {
                Console.WriteLine("Выберите операцию: +, -, *, /, Back, Esc");
                string? operation = Console.ReadLine()?.ToLower();
                switch (operation)
                {
                    case "+":
                        Add();
                        break;
                    case "-":
                        Diff();
                        break;
                    case "*":
                        Multiply();
                        break;
                    case "/":
                        Divide();
                        break;
                    case "back":
                        Undo();
                        break;
                    case "esc":
                        isRunning = Esc();
                        break;
                    case "":
                        isRunning = Esc();
                        break;
                    default:
                        DefaultCase();
                        break;
                }
            }
        }


    }
}

