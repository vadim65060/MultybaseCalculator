using System;
using Prism.Mvvm;
using org.mariuszgromada.math.mxparser;
using Сalculator.Utilities;

namespace Сalculator.Models
{
    public class CalculatorModel : BindableBase
    {
        public string Equation => _equation;
        public int CountOpenBrackets => _countOpenBrackets;

        public int Base
        {
            get => _base;
            set
            {
                ClearEquation();

                _base = value;
                RaisePropertyChanged();
            }
        }

        private string _equation;
        private int _base;
        private int _countOpenBrackets;

        public CalculatorModel()
        {
            License.iConfirmNonCommercialUse("vadim65060");
        }

        public void Calculate()
        {
            _equation = _equation.Replace("NaN", "");
            _equation = ConvertNumbersToDecimal(_equation, ',');
            _equation = _equation.Replace(',', '.');
            _equation += new string(')', _countOpenBrackets);
            _countOpenBrackets = 0;
            RaisePropertyChanged(nameof(CountOpenBrackets));
            Expression expression = new Expression(_equation);
            _equation = NumeralSystemConvertor.ConvertFromDecimal(expression.calculate(), _base);
            Console.WriteLine(expression.calculate());
            RaisePropertyChanged(nameof(Equation));
        }

        public void AddToEquation(string str)
        {
            switch (str)
            {
                case "(":
                    _countOpenBrackets++;
                    RaisePropertyChanged(nameof(CountOpenBrackets));
                    break;
                case ")":
                    _countOpenBrackets--;
                    RaisePropertyChanged(nameof(CountOpenBrackets));
                    break;
            }

            if (_equation.Contains("NaN"))
            {
                _equation = string.Empty;
            }

            _equation += str;
            RaisePropertyChanged(nameof(Equation));
        }

        public void ClearEquation()
        {
            _countOpenBrackets = 0;
            RaisePropertyChanged(nameof(CountOpenBrackets));

            _equation = string.Empty;
            RaisePropertyChanged(nameof(Equation));
        }

        public void EraseEquation()
        {
            if (_equation.Length == 0)
            {
                return;
            }

            switch (_equation[_equation.Length - 1])
            {
                case '(':
                    _countOpenBrackets--;
                    RaisePropertyChanged(nameof(CountOpenBrackets));
                    break;
                case ')':
                    _countOpenBrackets++;
                    RaisePropertyChanged(nameof(CountOpenBrackets));
                    break;
            }

            _equation = _equation.Substring(0, _equation.Length - 1);
            RaisePropertyChanged(nameof(Equation));
        }

        private string ConvertNumbersToDecimal(string equation, char fractionalSeparator)
        {
            char[] delimiterChars = {' ', '+', '-', '*', '/', '(', ')'};
            string[] numbers = _equation.Split(delimiterChars);
            string basedEquation = equation;
            int lastNumberEnd = 0;

            foreach (var number in numbers)
            {
                if (string.IsNullOrEmpty(number))
                {
                    continue;
                }

                int numberStart = basedEquation.IndexOf(number, lastNumberEnd, StringComparison.Ordinal);
                lastNumberEnd = numberStart + number.Length;
                basedEquation = basedEquation.Substring(0, numberStart) +
                                NumeralSystemConvertor.ConvertToDecimal(number, _base, fractionalSeparator) +
                                basedEquation.Substring(lastNumberEnd);
            }

            Console.WriteLine(basedEquation);
            return basedEquation;
        }
    }
}