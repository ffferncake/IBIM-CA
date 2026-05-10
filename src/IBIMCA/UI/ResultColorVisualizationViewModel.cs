using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IBIMCA.UI
{
    public class ResultColorVisualizationViewModel : INotifyPropertyChanged
    {
        // 평가항목
        string _evaluationItem = "";
        public string EvaluationItem
        {
            get => _evaluationItem;
            set { _evaluationItem = value; OnPropertyChanged(); }
        }

        // 옵션들(데모)
        public ObservableCollection<string> PatternOptions { get; } =
            new ObservableCollection<string> { "<재지정 없음>", "Solid Fill", "Diagonal", "Dots" };

        public ObservableCollection<string> WeightOptions { get; } =
            new ObservableCollection<string> { "<재지정 없음>", "Thin", "Medium", "Thick" };

        // 최우수
        string _bestPattern = "<재지정 없음>";
        public string BestPattern { get => _bestPattern; set { _bestPattern = value; OnPropertyChanged(); } }

        string _bestWeight = "<재지정 없음>";
        public string BestWeight { get => _bestWeight; set { _bestWeight = value; OnPropertyChanged(); } }

        System.Windows.Media.Color _bestColor = System.Windows.Media.Color.FromRgb(0, 183, 0);
        public System.Windows.Media.Brush BestColorBrush =>
            new System.Windows.Media.SolidColorBrush(_bestColor);

        public string BestColorText => $"RGB {_bestColor.R:000}-{_bestColor.G:000}-{_bestColor.B:000}";

        // 우수
        string _goodPattern = "<재지정 없음>";
        public string GoodPattern { get => _goodPattern; set { _goodPattern = value; OnPropertyChanged(); } }

        string _goodWeight = "<재지정 없음>";
        public string GoodWeight { get => _goodWeight; set { _goodWeight = value; OnPropertyChanged(); } }

        System.Windows.Media.Color _goodColor = System.Windows.Media.Color.FromRgb(0, 0, 255);
        public System.Windows.Media.Brush GoodColorBrush =>
            new System.Windows.Media.SolidColorBrush(_goodColor);

        public string GoodColorText => "파란색";

        // Commands
        public ICommand PickEvaluationItemCommand { get; }
        public ICommand PickBestPatternCommand { get; }
        public ICommand PickGoodPatternCommand { get; }
        public ICommand RevertCommand { get; }
        public ICommand ApplyCommand { get; }
        public ICommand FinishCommand { get; }

        // 원복용 백업
        readonly System.Windows.Media.Color _bestColorBackup;
        readonly System.Windows.Media.Color _goodColorBackup;
        readonly string _bestPatternBackup;
        readonly string _goodPatternBackup;
        readonly string _bestWeightBackup;
        readonly string _goodWeightBackup;

        public ResultColorVisualizationViewModel()
        {
            // 백업
            _bestColorBackup = _bestColor;
            _goodColorBackup = _goodColor;
            _bestPatternBackup = _bestPattern;
            _goodPatternBackup = _goodPattern;
            _bestWeightBackup = _bestWeight;
            _goodWeightBackup = _goodWeight;

            PickEvaluationItemCommand = new RelayCommand(_ =>
            {
                // TODO: 실제로는 평가항목 선택 팝업/검색 연결
                EvaluationItem = "BF 1.3.3 주출입구 유효폭(문폭)";
                Autodesk.Revit.UI.TaskDialog.Show("평가항목 선택", "데모: 평가항목을 선택했습니다.\n\n" + EvaluationItem);
            });

            PickBestPatternCommand = new RelayCommand(_ =>
            {
                // TODO: 실제 Revit FillPatternElement 선택 UI 연결
                BestPattern = "Solid Fill";
            });

            PickGoodPatternCommand = new RelayCommand(_ =>
            {
                GoodPattern = "Diagonal";
            });

            RevertCommand = new RelayCommand(_ =>
            {
                _bestColor = _bestColorBackup;
                _goodColor = _goodColorBackup;
                BestPattern = _bestPatternBackup;
                GoodPattern = _goodPatternBackup;
                BestWeight = _bestWeightBackup;
                GoodWeight = _goodWeightBackup;

                // Brush/Text 갱신
                OnPropertyChanged(nameof(BestColorBrush));
                OnPropertyChanged(nameof(BestColorText));
                OnPropertyChanged(nameof(GoodColorBrush));
                OnPropertyChanged(nameof(GoodColorText));
            });

            ApplyCommand = new RelayCommand(_ =>
            {
                // TODO: 여기서 Revit OverrideGraphicSettings로 뷰/요소 색상 적용
                Autodesk.Revit.UI.TaskDialog.Show("적용(데모)",
                    $"평가항목: {EvaluationItem}\n\n" +
                    $"[최우수] 패턴={BestPattern}, 색상={BestColorText}, 두께={BestWeight}\n" +
                    $"[우수]   패턴={GoodPattern}, 색상={GoodColorText}, 두께={GoodWeight}\n\n" +
                    $"(TODO: 실제 Revit 뷰 오버라이드 적용)");
            });

            FinishCommand = new RelayCommand(_ =>
            {
                Autodesk.Revit.UI.TaskDialog.Show("완료", "데모: 완료 버튼을 눌렀습니다.");
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}