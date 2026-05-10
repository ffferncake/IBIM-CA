using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace IBIMCA.UI
{
    public class ChatMessage : INotifyPropertyChanged
    {
        public bool IsUser { get; set; }
        public string Text { get; set; } = "";
        public DateTime Time { get; set; } = DateTime.Now;

        // UI helper props
        public System.Windows.HorizontalAlignment BubbleAlign => IsUser ? System.Windows.HorizontalAlignment.Right : System.Windows.HorizontalAlignment.Left;

        public System.Windows.Media.Brush BubbleBackground => IsUser
            ? new System.Windows.Media.SolidColorBrush(
                  System.Windows.Media.Color.FromRgb(219, 234, 254))
            : new System.Windows.Media.SolidColorBrush(
                  System.Windows.Media.Color.FromRgb(255, 255, 255));

        public System.Windows.Media.Brush BubbleBorderBrush => IsUser
            ? new System.Windows.Media.SolidColorBrush(
                  System.Windows.Media.Color.FromRgb(219, 234, 254))
            : new System.Windows.Media.SolidColorBrush(
                  System.Windows.Media.Color.FromRgb(255, 255, 255));

        public string SenderLabel => IsUser ? "나" : "AI";
        public System.Windows.Visibility SenderLabelVisibility => System.Windows.Visibility.Visible;

        public string TimeText => Time.ToString("HH:mm");

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    public class AIAssistantViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ChatMessage> Messages { get; } = new();

        string _inputText = "";
        public string InputText
        {
            get => _inputText;
            set { _inputText = value; OnPropertyChanged(); }
        }

        public ICommand SendCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand SeedDemoCommand { get; }

        public AIAssistantViewModel()
        {
            SendCommand = new RelayCommand(_ => Send(), _ => !string.IsNullOrWhiteSpace(InputText));
            ClearCommand = new RelayCommand(_ => Clear());
            SeedDemoCommand = new RelayCommand(_ => SeedDemo());

            // ✅ 처음 열릴 때 예시 대화 자동 표시
            SeedDemo();
        }

        private void Send()
        {
            var userText = (InputText ?? "").Trim();
            if (userText.Length == 0) return;

            Messages.Add(new ChatMessage { IsUser = true, Text = userText, Time = DateTime.Now });
            InputText = "";

            // ✅ 하드코딩 응답(데모)
            Messages.Add(new ChatMessage
            {
                IsUser = false,
                Text =
                    "확인했습니다. 선택된 요소의 기준 충족 여부를 점검할게요.\n\n" +
                    "1) 기준값 확인\n" +
                    "2) 모델 속성(치수/폭/높이) 조회\n" +
                    "3) 미충족 시 수정 가이드 제안\n\n" +
                    "원하시면 '기준 변경 이유'도 같이 기록해둘까요?",
                Time = DateTime.Now
            });
        }

        private void Clear()
        {
            Messages.Clear();
        }

        private void SeedDemo()
        {
            Messages.Clear();

            Messages.Add(new ChatMessage
            {
                IsUser = false,
                Text = "평가 BF 항목에서 ‘보도→주출입구 보행로’ 기준을 요약해드릴까요?",
                Time = DateTime.Now.AddMinutes(-3)
            });

            Messages.Add(new ChatMessage
            {
                IsUser = true,
                Text = "응. 그리고 폭 기준이 1.2m인지 1.5m인지 헷갈려.",
                Time = DateTime.Now.AddMinutes(-3)
            });

            Messages.Add(new ChatMessage
            {
                IsUser = false,
                Text =
                    "현재 설정된 기준은 다음과 같습니다.\n\n" +
                    "• 보행로 유효폭: 1.2m 이상\n" +
                    "• 단차: 20mm 이하(예시)\n\n" +
                    "기준을 1.5m로 변경하면, 평가점 산정 로직도 함께 조정해야 합니다.\n" +
                    "‘기준을 1.5m로 변경’ 버튼을 누르시겠어요?",
                Time = DateTime.Now.AddMinutes(-2)
            });

            Messages.Add(new ChatMessage
            {
                IsUser = true,
                Text = "기준을 1.5m로 변경해줘.",
                Time = DateTime.Now.AddMinutes(-2)
            });

            Messages.Add(new ChatMessage
            {
                IsUser = false,
                Text =
                    "✅ 기준을 1.5m로 변경했습니다.\n" +
                    "다음 단계로 ‘선택 요소 평가’를 실행해, 현재 모델이 기준을 충족하는지 확인할게요.\n\n" +
                    "(데모) 선택 요소: ID 123456\n" +
                    "측정값: 1500mm → 기준 충족 → 결과: 우수",
                Time = DateTime.Now.AddMinutes(-1)
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    // 재사용 RelayCommand (이미 프로젝트에 있으면 이 부분은 제거하고 기존 거 쓰면 됨)
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object? parameter) => _execute(parameter);
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}