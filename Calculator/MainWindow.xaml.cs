using System.Collections.Generic;
using System.Windows.Controls;
using ComboBox = HandyControl.Controls.ComboBox;

namespace Сalculator
{
    public partial class MainWindow
    {
        private const int MaxBase = 10;
        private const int MinBase = 2;
        private const int DefaultBase = 9;
        private readonly List<Button> _buttonsList = new List<Button>(10);

        public MainWindow()
        {
            InitializeComponent();
            SetupButtonList();
            SetupBaseComboBox();
        }

        private void SetupBaseComboBox()
        {
            for (int i = MinBase; i <= MaxBase; i++)
            {
                BaseComboBox.Items.Add(i);
            }

            BaseComboBox.SelectedItem = DefaultBase;
        }

        private void SetupButtonList()
        {
            _buttonsList.Add(Button0);
            _buttonsList.Add(Button1);
            _buttonsList.Add(Button2);
            _buttonsList.Add(Button3);
            _buttonsList.Add(Button4);
            _buttonsList.Add(Button5);
            _buttonsList.Add(Button6);
            _buttonsList.Add(Button7);
            _buttonsList.Add(Button8);
            _buttonsList.Add(Button9);
        }

        private void BaseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            int selectedBase = int.Parse(comboBox.SelectedItem.ToString());
            for (int i = MinBase; i < MaxBase; i++)
            {
                _buttonsList[i].IsEnabled = i < selectedBase;
            }
        }
    }
}