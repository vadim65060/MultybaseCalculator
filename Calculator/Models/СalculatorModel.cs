using Prism.Mvvm;
using org.mariuszgromada.math.mxparser;

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
            _equation = ConvertNumbersToDecimal(_equation, ',');
            _equation = _equation.Replace(',', '.');
            _equation += new string(')', _countOpenBrackets);
            Expression expression = new Expression(_equation);
            _equation = NumeralSystemConvertor.ConvertFromDecimal(expression.calculate(), _base);
            System.Console.WriteLine(expression.calculate());
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
            char[] delimiterChars = { ' ', '+', '-', '*', '/', '(', ')' };
            string[] numbers = _equation.Split(delimiterChars);
            string basedEquation = equation;

            foreach (var number in numbers)
            {
                if (string.IsNullOrEmpty(number))
                {
                    continue;
                }

                basedEquation = basedEquation.Replace(number,
                    NumeralSystemConvertor.ConvertToDecimal(number, _base, fractionalSeparator));
            }

            System.Console.WriteLine(basedEquation);
            return basedEquation;
        }
    }
}