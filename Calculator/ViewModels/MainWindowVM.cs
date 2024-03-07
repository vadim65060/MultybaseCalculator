using Prism.Commands;
using Prism.Mvvm;
using Сalculator.Models;

namespace Сalculator.ViewModels
{
    public class MainWindowVM : BindableBase
    {
        public DelegateCommand CalculateCommand { get; }
        public DelegateCommand<string> AddCommand { get; }
        public DelegateCommand ClearCommand { get; }
        public DelegateCommand EraseCommand { get; }
        public string Equation => _model.Equation;

        public int CountOpenBrackets => _model.CountOpenBrackets;


        public int Base
        {
            get => _model.Base;
            set => _model.Base = value;
        }

        private const string Operators = "+-*/,";
        private readonly CalculatorModel _model = new CalculatorModel();

        public MainWindowVM()
        {
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            CalculateCommand = new DelegateCommand(() => { _model.Calculate(); });
            AddCommand = new DelegateCommand<string>(input =>
            {
                if (WrongInputValidation(input))
                {
                    return;
                }

                _model.AddToEquation(input);
            });
            ClearCommand = new DelegateCommand(() => { _model.ClearEquation(); });
            EraseCommand = new DelegateCommand(() => { _model.EraseEquation(); });
        }

        private bool WrongInputValidation(string input)
        {
            switch (input)
            {
                case "+":
                case "*":
                case "/":
                    return Equation.Length == 0 ||
                           Equation[Equation.Length - 1] == '(' ||
                           Operators.Contains(Equation[Equation.Length - 1].ToString()) ||
                           Equation.Contains("NaN");
                case "-":
                    return Equation.Length != 0 && Operators.Contains(Equation[Equation.Length - 1].ToString());
                case ",":
                    if (Equation.Length == 0 ||
                        Equation[Equation.Length - 1] < '0' || Equation[Equation.Length - 1] > '9' ||
                        Equation.Contains("NaN"))
                    {
                        return true;
                    }

                    for (int i = Equation.Length - 1; i >= 0; i--)
                    {
                        if (Equation[i] == ',')
                        {
                            return true;
                        }

                        if (Equation[i] < '0' || Equation[i] > '9')
                        {
                            return false;
                        }
                    }

                    return false;
                case "(":
                    return Equation.Length != 0 && Equation[Equation.Length - 1] == ',';
                default:
                    return false;
            }
        }
    }
}