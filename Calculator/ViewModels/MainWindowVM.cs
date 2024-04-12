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

        public int Base
        {
            get => _model.Base;
            set
            {
                _model.Base = value;
                UpdateButtons();
            }
        }

        public bool CanPrintMinus =>
            Equation.Length == 0 || !Operators.Contains(Equation[Equation.Length - 1].ToString());

        public bool CanPrintDot => !CantPrintDot();

        public bool CanPrintOperator => !(Equation.Length == 0 ||
                                          Equation[Equation.Length - 1] == '(' ||
                                          Operators.Contains(Equation[Equation.Length - 1].ToString()) ||
                                          Equation.Contains("NaN"));

        public bool CanPrintCloseBracket =>
            _model.CountOpenBrackets > 0 && Equation[Equation.Length - 1] >= '0' &&
            Equation[Equation.Length - 1] <= '9';

        public bool CanPrintOpenBracket => Equation.Length == 0 || Equation[Equation.Length - 1] != ',';


        private const string Operators = "+-*/,";
        private readonly CalculatorModel _model = new CalculatorModel();

        public MainWindowVM()
        {
            _model.PropertyChanged += (s, e) => { RaisePropertyChanged(e.PropertyName); };
            CalculateCommand = new DelegateCommand(() =>
            {
                _model.Calculate();
                UpdateButtons();
            });
            AddCommand = new DelegateCommand<string>(input =>
            {
                _model.AddToEquation(input);
                UpdateButtons();
            });
            ClearCommand = new DelegateCommand(() =>
            {
                _model.ClearEquation();
                UpdateButtons();
            });
            EraseCommand = new DelegateCommand(() =>
            {
                _model.EraseEquation();
                UpdateButtons();
            });
        }

        private void UpdateButtons()
        {
            RaisePropertyChanged(nameof(CanPrintMinus));
            RaisePropertyChanged(nameof(CanPrintDot));
            RaisePropertyChanged(nameof(CanPrintOperator));
            RaisePropertyChanged(nameof(CanPrintCloseBracket));
            RaisePropertyChanged(nameof(CanPrintOpenBracket));
        }

        private bool CantPrintDot()
        {
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
        }
    }
}