using System.Windows;

namespace IBIMCA.UI
{
    public partial class AIAssistantWindow : Window
    {
        public AIAssistantWindow()
        {
            InitializeComponent();
        }

        // ✅ WPF KeyEventHandler 정확한 시그니처
        private void OnInputKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && DataContext is AIAssistantViewModel vm)
            {
                if (vm.SendCommand.CanExecute(null))
                    vm.SendCommand.Execute(null);

                e.Handled = true;
            }
        }
    }
}