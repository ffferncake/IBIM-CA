// ExportExcelReportViewModel.cs — FinishCommand 변경/대체용 전체 구현
using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace IBIMCA.UI
{
    public class ExcelReportPreviewRow
    {
        public string Certification { get; set; } = "";
        public string Item { get; set; } = "";
        public string Result { get; set; } = "";
        public string Grade { get; set; } = "";
    }

    public class ExportExcelReportViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ExcelReportPreviewRow> PreviewRows { get; } = new();

        string _selectedItemsPath = "";
        public string SelectedItemsPath
        {
            get => _selectedItemsPath;
            set { _selectedItemsPath = value; OnPropertyChanged(); }
        }

        string _selfEvaluationPath = "";
        public string SelfEvaluationPath
        {
            get => _selfEvaluationPath;
            set { _selfEvaluationPath = value; OnPropertyChanged(); }
        }

        string _fileName = "프로젝트명_BF_GB_평가보고서_20260510.xlsx";
        public string FileName
        {
            get => _fileName;
            set { _fileName = value; OnPropertyChanged(); }
        }

        string _saveLocation = @"C:\Reports\";
        public string SaveLocation
        {
            get => _saveLocation;
            set { _saveLocation = value; OnPropertyChanged(); }
        }

        public ICommand BrowseSelectedItemsCommand { get; }
        public ICommand BrowseSelfEvaluationCommand { get; }
        public ICommand BrowseSaveLocationCommand { get; }
        public ICommand PreviewCommand { get; }
        public ICommand FinishCommand { get; }

        public ExportExcelReportViewModel()
        {
            SeedPreviewRows();

            BrowseSelectedItemsCommand = new RelayCommand(_ =>
            {
                var path = BrowseExcelPath("평가항목 선택 내보내기");
                if (!string.IsNullOrWhiteSpace(path))
                    SelectedItemsPath = path;
            });

            BrowseSelfEvaluationCommand = new RelayCommand(_ =>
            {
                var path = BrowseExcelPath("자체평가 내보내기");
                if (!string.IsNullOrWhiteSpace(path))
                    SelfEvaluationPath = path;
            });

            BrowseSaveLocationCommand = new RelayCommand(_ =>
            {
                using var dlg = new System.Windows.Forms.FolderBrowserDialog();
                dlg.Description = "엑셀 보고서 저장 위치 선택";
                dlg.SelectedPath = SaveLocation;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    SaveLocation = EnsureTrailingSlash(dlg.SelectedPath);
            });

            PreviewCommand = new RelayCommand(_ =>
            {
                Autodesk.Revit.UI.TaskDialog.Show("엑셀 보고서 출력", "보고서 요약 미리보기를 갱신했습니다.\n\n선택 인증: BF, GB\n출력 범위: 전체 평가 결과\n예상 시트 수: 5");
            });

            // ← 여기만 수정: 실제로 CSV 파일 두 개를 생성
            FinishCommand = new RelayCommand(_ =>
            {
                try
                {
                    var reportPath = BuildReportPath();
                    SelectedItemsPath = reportPath;

                    if (string.IsNullOrWhiteSpace(SelectedItemsPath) && string.IsNullOrWhiteSpace(SelfEvaluationPath))
                    {
                        Autodesk.Revit.UI.TaskDialog.Show("Export", "저장할 파일 경로가 없습니다. 하나 이상 선택하세요.");
                        return;
                    }

                    // EvaluationPanel 의 ViewModel에 접근 (있을 때만)
                    var provider = EvaluationPanelProvider.Instance;
                    if (provider == null || provider.ViewModel == null)
                    {
                        Autodesk.Revit.UI.TaskDialog.Show("Export", "평가 패널이 초기화되지 않았습니다. 먼저 평가 패널을 열어주세요.");
                        return;
                    }

                    var vm = provider.ViewModel;

                    // 1) SelectedItemsPath -> VisibleRows (현재 보여지는 행)
                    if (!string.IsNullOrWhiteSpace(SelectedItemsPath))
                    {
                        ExportCsv(SelectedItemsPath, vm.VisibleRows);
                    }

                    // 2) SelfEvaluationPath -> 모든 노드(루트부터 flatten)
                    if (!string.IsNullOrWhiteSpace(SelfEvaluationPath))
                    {
                        var all = FlattenAll(vm.RootNodes);
                        ExportCsv(SelfEvaluationPath, all);
                    }

                    Autodesk.Revit.UI.TaskDialog.Show("Export", "파일 내보내기를 완료했습니다.");
                }
                catch (Exception ex)
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Export Error", ex.ToString());
                }
            });
        }

        private void SeedPreviewRows()
        {
            PreviewRows.Add(new ExcelReportPreviewRow { Certification = "BF", Item = "출입문 유효폭", Result = "적합", Grade = "● 우수" });
            PreviewRows.Add(new ExcelReportPreviewRow { Certification = "GB", Item = "에너지 성능", Result = "검토 필요", Grade = "● 일반" });
            PreviewRows.Add(new ExcelReportPreviewRow { Certification = "BF", Item = "주출입구", Result = "부적합", Grade = "● 미흡" });
            PreviewRows.Add(new ExcelReportPreviewRow { Certification = "GB", Item = "주차공간 비율", Result = "적합", Grade = "● 우수" });
        }

        private string BuildReportPath()
        {
            var directory = string.IsNullOrWhiteSpace(SaveLocation) ? @"C:\Reports\" : SaveLocation;
            var fileName = string.IsNullOrWhiteSpace(FileName) ? "평가보고서.csv" : FileName;

            if (!Path.HasExtension(fileName))
                fileName += ".xlsx";

            return Path.Combine(directory, fileName);
        }

        private static string EnsureTrailingSlash(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "";

            return path.EndsWith("\\") || path.EndsWith("/")
                ? path
                : path + "\\";
        }

        private static string BrowseExcelPath(string title)
        {
            using var dlg = new System.Windows.Forms.SaveFileDialog();
            dlg.Title = title;
            dlg.Filter = "CSV 파일 (*.csv)|*.csv|Excel Workbook (*.xlsx)|*.xlsx";
            dlg.FileName = "report.csv";
            dlg.OverwritePrompt = true;

            var result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                return dlg.FileName;

            return "";
        }

        // CSV 출력 유틸리티
        private static void ExportCsv(string path, System.Collections.Generic.IEnumerable<EvaluationNode> rows)
        {
            // 강제 .csv 확장자 처리 (선택사항)
            if (!Path.HasExtension(path) || Path.GetExtension(path).ToLower() != ".csv")
            {
                // 사용자가 .xlsx를 선택했더라도 csv로 저장할지 물어보고 바꿀 수 있음.
                // 여기서는 그냥 확장자를 csv로 바꿔 저장합니다.
                path = Path.ChangeExtension(path, ".csv");
            }

            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            // UTF8 BOM 으로 저장(한글 Excel에서 깨짐 방지)
            var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);

            using var sw = new StreamWriter(path, false, encoding);

            // 헤더
            sw.WriteLine("Id,Title,Level,MaxScore,Score,Result,IsChecked");

            // 내용 (CSV로 안전하게 출력: 쉼표에 대한 간단한 처리 - 큰따옴표로 감싸기)
            foreach (var r in rows)
            {
                var safeTitle = CsvEscape(r.Title);
                sw.WriteLine($"{r.Id},{safeTitle},{r.Level},{r.MaxScore},{r.Score},{r.Result},{r.IsChecked}");
            }

            sw.Flush();
        }

        private static string CsvEscape(string s)
        {
            if (s == null) return "";
            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n") || s.Contains("\r"))
            {
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            }
            return s;
        }

        // FlattenAll 재사용 (같은 로직)
        private static System.Collections.Generic.List<EvaluationNode> FlattenAll(System.Collections.ObjectModel.ObservableCollection<EvaluationNode> nodes)
        {
            var list = new System.Collections.Generic.List<EvaluationNode>();
            foreach (var n in nodes)
            {
                list.Add(n);
                if (n.Children.Count > 0)
                    list.AddRange(FlattenAll(n.Children));
            }
            return list;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
