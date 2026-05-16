using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfBrush = System.Windows.Media.Brush;
using WpfColor = System.Windows.Media.Color;
using WpfColors = System.Windows.Media.Colors;
using WpfHorizontalAlignment = System.Windows.HorizontalAlignment;
using WpfSolidColorBrush = System.Windows.Media.SolidColorBrush;
using WpfVisibility = System.Windows.Visibility;

namespace IBIMCA.UI
{
    public class ChatMessage : INotifyPropertyChanged
    {
        public bool IsUser { get; set; }
        public string Text { get; set; } = "";
        public DateTime Time { get; set; } = DateTime.Now;

        public WpfHorizontalAlignment BubbleAlign => IsUser ? WpfHorizontalAlignment.Right : WpfHorizontalAlignment.Left;

        public WpfBrush BubbleBackground => IsUser
            ? new WpfSolidColorBrush(WpfColor.FromRgb(222, 238, 255))
            : new WpfSolidColorBrush(WpfColors.White);

        public WpfBrush BubbleBorderBrush => IsUser
            ? new WpfSolidColorBrush(WpfColor.FromRgb(178, 211, 247))
            : new WpfSolidColorBrush(WpfColor.FromRgb(214, 226, 240));

        public string SenderLabel => IsUser ? "사용자" : "AIBIM Chat  AI 어시스턴트";
        public WpfVisibility HeaderVisibility => IsUser ? WpfVisibility.Collapsed : WpfVisibility.Visible;
        public WpfVisibility BotIconVisibility => IsUser ? WpfVisibility.Hidden : WpfVisibility.Visible;
        public WpfVisibility UserIconVisibility => IsUser ? WpfVisibility.Visible : WpfVisibility.Hidden;
        public WpfVisibility UserTimeVisibility => IsUser ? WpfVisibility.Visible : WpfVisibility.Collapsed;
        public string TimeText => Time.ToString("HH:mm");

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class AIAssistantViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ChatMessage> Messages { get; } = new();

        private string _inputText = "";
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand SendCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AskPresetCommand { get; }
        public ICommand SelectDomainCommand { get; }

        public AIAssistantViewModel()
        {
            SendCommand = new RelayCommand(_ => Send());
            ClearCommand = new RelayCommand(_ => Clear());
            AskPresetCommand = new RelayCommand(text => AskPreset(text?.ToString() ?? ""));
            SelectDomainCommand = new RelayCommand(domain => SelectDomain(domain?.ToString() ?? "BF"));

            SeedWelcome();
        }

        private void Send()
        {
            var userText = (InputText ?? "").Trim();
            if (userText.Length == 0)
                return;

            AddUser(userText);
            InputText = "";
            AddBot(BuildResponse(userText));
        }

        private void AskPreset(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            AddUser(text);
            AddBot(BuildResponse(text));
        }

        private void SelectDomain(string domain)
        {
            if (domain.Equals("BF", StringComparison.OrdinalIgnoreCase))
                AddBot("BF 인증 기준 모드로 전환했습니다.\n주출입구, 보행로, 복도, 승강기, 장애인 화장실 기준을 바로 확인할 수 있습니다.");
            else if (domain.Equals("G-SEED", StringComparison.OrdinalIgnoreCase))
                AddBot("G-SEED 인증 기준 모드로 전환했습니다.\n에너지 성능, 재료 및 자원, 물순환관리, 실내환경 항목을 중심으로 안내합니다.");
            else
                AddBot("CPTED 인증 기준 모드로 전환했습니다.\n자연적 감시, 접근 통제, 조명, CCTV, 주차장 안전 기준을 중심으로 안내합니다.");
        }

        private void Clear()
        {
            Messages.Clear();
            SeedWelcome();
        }

        private void SeedWelcome()
        {
            Messages.Clear();
            AddBot("안녕하세요! 설계인증, 가이드 검색, 단차, 안전 분석 등 무엇을 도와드릴까요?");
        }

        private void AddUser(string text)
            => Messages.Add(new ChatMessage { IsUser = true, Text = text, Time = DateTime.Now });

        private void AddBot(string text)
            => Messages.Add(new ChatMessage { IsUser = false, Text = text, Time = DateTime.Now });

        private string BuildResponse(string userText)
        {
            var q = userText.ToLowerInvariant();

            if (q.Contains("bf") && (q.Contains("주출입구") || q.Contains("보행로") || q.Contains("유효폭") || q.Contains("단차")))
                return BuildBfEntranceResponse();

            if (q.Contains("bf") && (q.Contains("승강기") || q.Contains("엘리베이터")))
                return "BF 승강기 기준 요약입니다.\n\n• 출입구 유효폭: 0.9m 이상 권장\n• 내부 치수: 1.4m x 1.4m 이상 권장\n• 조작반: 휠체어 이용자가 접근 가능한 높이로 계획\n\n전체 결과: 일반\n검토 포인트: ElevatorType, ClearOpening, AccessibleToilet 파라미터와 함께 확인하세요.";

            if (q.Contains("bf") && (q.Contains("화장실") || q.Contains("장애인")))
                return "BF 장애인 화장실 기준 요약입니다.\n\n• 회전반경: 1.4m 이상 확보\n• 출입문: 유효 개구폭과 여닫이 방향 확인\n• 접근 동선: 복도 폭과 단차 조건 연계 검토\n\n전체 결과: 우수\n모델 파라미터: AccessibleToilet, ClearOpening, CorridorWidth";

            if ((q.Contains("g-seed") || q.Contains("gseed") || q.Contains("녹색") || q.Contains("gb")) && (q.Contains("에너지") || q.Contains("성능")))
                return "G-SEED 에너지 성능 기준 요약입니다.\n\n• 에너지효율등급: 1+등급 입력 예시\n• 에너지성능지표: 72점 예시\n• TAB 및 커미셔닝 실시 여부 확인\n• 에너지 모니터링 및 관리지원 장치 반영\n\n전체 결과: PASS\n연계 파라미터: SHGC, WindowUValue, LightingDensity, HVACType";

            if (q.Contains("g-seed") || q.Contains("gseed") || q.Contains("녹색") || q.Contains("gb"))
                return "G-SEED 주요 평가영역입니다.\n\n• 토지이용 및 교통\n• 에너지 및 환경오염\n• 재료 및 자원\n• 물순환관리\n• 유지관리\n• 생태환경\n• 실내환경\n\n현재 예시 결과: 54 / 54 항목, 점수 0 / 136, 등급 부적합";

            if (q.Contains("cpted") && (q.Contains("출입구") || q.Contains("가시성") || q.Contains("감시")))
                return "CPTED 출입구 가시성 기준 요약입니다.\n\n• 주 출입구와 시야 확보 구간을 모델링\n• 사각지대 발생 구간은 조명 또는 CCTV 계획 반영\n• 저층부는 내부 공개성과 접근 통제를 함께 검토\n\n전체 결과: PASS\n검토 파라미터: VisibilityRange, LightingLevel, CCTVCoverage, EntranceControl";

            if (q.Contains("cpted") || q.Contains("범죄") || q.Contains("방범"))
                return "CPTED 평가 요약입니다.\n\n• 상업시설 총점: 115 / 175\n• 결과: 최우수\n• PASS 영역: 상업시설 배치, 주차 공간\n• FAIL 영역: 건물 외부, 건물 내부 일부 항목\n\n우선 보완 추천: CCTV 범위, 후면공간 조명, 비상벨 설치 위치";

            if (q.Contains("결과") || q.Contains("요약") || q.Contains("등급"))
                return "현재 하드코딩 평가 결과 요약입니다.\n\n• BF: 182.65 / 288, 백분율 63.42%, 등급 부적합\n• G-SEED: 0 / 136, 등급 부적합\n• CPTED: 115 / 175, 등급 최우수\n\n필요하면 인증별 미달 항목만 따로 정리해드릴 수 있습니다.";

            if (q.Contains("보고서") || q.Contains("엑셀"))
                return "보고서 출력 안내입니다.\n\n• 평가 패널에서 항목별 점수와 결과 확인\n• 엑셀 보고서 출력 버튼으로 현재 결과 내보내기\n• BF/G-SEED/CPTED별 시트 분리 구성이 가능합니다.";

            return "확인했습니다. 해당 질문은 인증 기준, 모델 파라미터, 평가 결과를 함께 보고 답변할 수 있습니다.\n\n예시 질문:\n• BF 주출입구 보행로 기준 알려줘\n• G-SEED 에너지 성능 기준 알려줘\n• CPTED 출입구 가시성 기준 알려줘\n• 현재 평가 결과 요약해줘";
        }

        private static string BuildBfEntranceResponse()
        {
            return "BF 주출입구 보행로 기준은 다음과 같습니다.\n\n• 유효폭: 1.2m 이상\n• 단차: 20mm 이하(예시)\n• 기울기: 1.5m당 1.2m 이하\n\n전체 결과: 적합\n모델 검토: Width, ClearOpening, RampSlope 파라미터와 연결 가능합니다.";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

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
