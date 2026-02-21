using System.Collections.ObjectModel;
using System.Windows;

namespace IBIMCA.UI
{
    public partial class StandardEvaluationSettingsWindow : Window
    {
        public ObservableCollection<EvalItem> Items { get; } = new ObservableCollection<EvalItem>();

        public StandardEvaluationSettingsWindow()
        {
            InitializeComponent();

            // Sample data (replace with real BF rules)
            Items.Add(new EvalItem("바닥", "미끄럼 방지", true));
            Items.Add(new EvalItem("바닥", "걸려넘어질 염려가 없음", true));
            Items.Add(new EvalItem("바닥", "충갹흠수", true));
            Items.Add(new EvalItem("바닥", "색상변화", true));
            Items.Add(new EvalItem("바닥", "울림이 적음", true));

            Items.Add(new EvalItem("손잡이", "미끄럼 방지", true));
            Items.Add(new EvalItem("손잡이", "차갑지 않은", false));
            Items.Add(new EvalItem("손잡이", "점자 표시", false));

            Items.Add(new EvalItem("시설물", "마감처리", true));
            Items.Add(new EvalItem("시설물", "색상대비", true));
            Items.Add(new EvalItem("시설물", "그림", true));

            DataContext = this;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            // TODO: Save Items to your settings store (JSON, extensible storage, etc.)
            System.Windows.MessageBox.Show("저장 (TODO)", "IBIMCA");
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class EvalItem
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }

        public EvalItem(string category, string name, bool isEnabled)
        {
            Category = category;
            Name = name;
            IsEnabled = isEnabled;
        }
    }
}