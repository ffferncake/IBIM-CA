using Autodesk.Revit.UI;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace IBIMCA.UI
{
    public class AutoDesignCorrectionViewModel : INotifyPropertyChanged
    {
        string _evaluationItem = "";
        public string EvaluationItem
        {
            get => _evaluationItem;
            set { _evaluationItem = value; OnPropertyChanged(); }
        }

        string _suggestedFix = "";
        public string SuggestedFix
        {
            get => _suggestedFix;
            set { _suggestedFix = value; OnPropertyChanged(); }
        }

        public ICommand PickEvaluationItemCommand { get; }
        public ICommand GenerateSuggestionCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand FinishCommand { get; }
        public ICommand LoadOptionsCommand { get; }
        public ICommand RunRecommendationCommand { get; }

        public AutoDesignCorrectionViewModel()
        {
            LoadOptionsCommand = new RelayCommand(_ =>
            {
                Autodesk.Revit.UI.TaskDialog.Show("설계 대안 추천 실행 옵션", "저장된 설계 대안 추천 옵션을 불러왔습니다.");
            });

            RunRecommendationCommand = new RelayCommand(_ =>
            {
                Autodesk.Revit.UI.TaskDialog.Show(
                    "설계 대안 추천 실행 옵션",
                    "추천 실행을 시작합니다.\n\n인증 체계: BF\n목표 등급: 최우수 (90점 이상)\n평가 범위: 현재 활성 뷰\n적용 방식: 검토 후 적용\n추천 유형: 미달 / 감점 / 상위등급");
            });

            // 데모: 평가항목 검색 버튼(.....) 눌렀을 때 값 채우기
            PickEvaluationItemCommand = new RelayCommand(_ =>
            {
                // TODO: 실제 구현에서는 EvaluationPanelViewModel/DB에서 항목 검색 UI 띄우기
                EvaluationItem = "BF 1.3.3 주출입구 유효폭(문폭)";
                Autodesk.Revit.UI.TaskDialog.Show("설계 대안 수정 제안", "데모: 평가항목을 선택했습니다.\n\n" + EvaluationItem);
            });

            // 데모: 추천안 생성 버튼(.....)
            GenerateSuggestionCommand = new RelayCommand(_ =>
            {
                if (string.IsNullOrWhiteSpace(EvaluationItem))
                {
                    Autodesk.Revit.UI.TaskDialog.Show("설계 대안 수정 제안", "먼저 [평가항목 검색]에서 항목을 선택하세요.");
                    return;
                }

                // TODO: 실제 구현에서는 규정 DB + BIM 파라미터 기반으로 추천안 생성
                SuggestedFix =
                    "현재 문폭이 800mm로 추정됩니다.\n" +
                    "BF 기준 충족을 위해 900mm 이상으로 변경 권장.\n" +
                    "가능하면 미닫이문/자동문 적용도 검토하세요.";
            });

            // 내보내기: txt 저장(데모)
            ExportCommand = new RelayCommand(_ =>
            {
                var path = BrowseSaveTxt("설계 대안 수정 제안 내보내기");
                if (string.IsNullOrWhiteSpace(path)) return;

                var sb = new StringBuilder();
                sb.AppendLine("설계 대안 수정 제안");
                sb.AppendLine("====================");
                sb.AppendLine("평가항목:");
                sb.AppendLine(EvaluationItem);
                sb.AppendLine();
                sb.AppendLine("추천 설계 대안:");
                sb.AppendLine(SuggestedFix);

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                Autodesk.Revit.UI.TaskDialog.Show("내보내기", "저장 완료:\n" + path);
            });

            FinishCommand = new RelayCommand(_ =>
            {
                Autodesk.Revit.UI.TaskDialog.Show("완료", "데모: 완료 버튼을 눌렀습니다.");
                // 창 닫기는 Window code-behind에서 Command로 연결하거나
                // Cmd에서 DialogResult 처리를 추가할 수 있음
            });
        }

        private static string BrowseSaveTxt(string title)
        {
            using var dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.Title = title;
            dlg.Filter = "Text File (*.txt)|*.txt";
            dlg.FileName = "AutoDesignCorrection.txt";
            dlg.OverwritePrompt = true;

            var result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                return dlg.FileName;

            return "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
