using Autodesk.Revit.UI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfBrush = System.Windows.Media.Brush;
using WpfColor = System.Windows.Media.Color;
using WpfSolidColorBrush = System.Windows.Media.SolidColorBrush;
using WpfVisibility = System.Windows.Visibility;

namespace IBIMCA.UI
{
    public class EcoProductRow
    {
        public string RevitId { get; set; } = "";
        public string Category { get; set; } = "";
        public string MaterialName { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string CertifiedProduct { get; set; } = "";
        public string CertificationNumber { get; set; } = "";
        public string MatchStatus { get; set; } = "";
        public string Quantity { get; set; } = "";
        public string Unit { get; set; } = "";
        public string CarbonFootprint { get; set; } = "";
        public bool UseGb { get; set; }
        public bool UseCarbon { get; set; }
        public bool UseVisualization { get; set; }
        public bool UseReport { get; set; }
        public WpfVisibility UseGbVisibility => UseGb ? WpfVisibility.Visible : WpfVisibility.Collapsed;
        public WpfVisibility UseCarbonVisibility => UseCarbon ? WpfVisibility.Visible : WpfVisibility.Collapsed;
        public WpfVisibility UseVisualizationVisibility => UseVisualization ? WpfVisibility.Visible : WpfVisibility.Collapsed;
        public WpfVisibility UseReportVisibility => UseReport ? WpfVisibility.Visible : WpfVisibility.Collapsed;
        public WpfVisibility EmptyUsageVisibility => !UseGb && !UseCarbon && !UseVisualization && !UseReport ? WpfVisibility.Visible : WpfVisibility.Collapsed;

        public WpfBrush MatchStatusBackground
        {
            get
            {
                if (MatchStatus == "자동 매칭")
                    return new WpfSolidColorBrush(WpfColor.FromRgb(224, 247, 224));
                if (MatchStatus == "검토 필요")
                    return new WpfSolidColorBrush(WpfColor.FromRgb(255, 243, 210));
                return new WpfSolidColorBrush(WpfColor.FromRgb(255, 229, 229));
            }
        }

        public WpfBrush MatchStatusForeground
        {
            get
            {
                if (MatchStatus == "자동 매칭")
                    return new WpfSolidColorBrush(WpfColor.FromRgb(32, 132, 54));
                if (MatchStatus == "검토 필요")
                    return new WpfSolidColorBrush(WpfColor.FromRgb(239, 126, 0));
                return new WpfSolidColorBrush(WpfColor.FromRgb(214, 37, 37));
            }
        }
    }

    public class UpdateExternalDBViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EcoProductRow> ProductRows { get; } = new();

        public string TotalCount => "1,266";
        public string AutoMatchedCount => "1,156";
        public string ReviewNeededCount => "87";
        public string UnregisteredCount => "23";
        public string CertifiedProductCount => "987";
        public string CarbonTotal => "12,845 kgCO2e";
        public string CarbonAverage => "10.1 kgCO2e/m²";
        public string LastMatchDate => "2026-05-10";
        public string StatusText => "완료";

        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFilterCommand { get; }
        public ICommand ReviewMatchCommand { get; }
        public ICommand VisualizeCommand { get; }
        public ICommand ApplyGbCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand SummaryReportCommand { get; }
        public ICommand DetailReportCommand { get; }

        public UpdateExternalDBViewModel()
        {
            SeedRows();

            ApplyFilterCommand = new RelayCommand(_ => ShowDemo("필터 적용", "검색 조건을 적용해 친환경 제품 매칭 목록을 갱신했습니다."));
            ResetFilterCommand = new RelayCommand(_ => ShowDemo("초기화", "검색 및 필터 조건을 기본값으로 초기화했습니다."));
            ReviewMatchCommand = new RelayCommand(_ => ShowDemo("매칭 검토", "검토 필요 및 미등록 항목을 우선순위로 정렬했습니다."));
            VisualizeCommand = new RelayCommand(_ => ShowDemo("결과 색상 가시화", "BIM 모델에 친환경 제품 연계 상태 색상 규칙을 적용합니다."));
            ApplyGbCommand = new RelayCommand(_ => ShowDemo("GB 평가 반영", "자동 매칭된 인증 제품 정보를 녹색건축 평가 데이터에 반영했습니다."));
            ExportExcelCommand = new RelayCommand(_ => ShowDemo("엑셀 보고서 출력", "BIM 친환경 정보 연계 결과 보고서를 엑셀 형식으로 출력합니다."));
            SummaryReportCommand = new RelayCommand(_ => ShowDemo("요약 보고서", "자재 사용 현황과 탄소발자국 요약 보고서를 생성합니다."));
            DetailReportCommand = new RelayCommand(_ => ShowDemo("상세 보고서", "Revit ID별 매칭 상세 내역과 검토 필요 항목을 포함한 보고서를 생성합니다."));
        }

        private void SeedRows()
        {
            ProductRows.Add(NewRow("234567", "벽", "외벽 마감패널", "KCC", "KCC 세라믹 패널", "EL1234-001", "자동 매칭", "152.40", "m²", "1,582", true, true, true, true));
            ProductRows.Add(NewRow("234568", "창", "로이 복층유리", "유리코리아", "Low-E 복층유리 24T", "EL1234-002", "자동 매칭", "78.30", "m²", "734", true, true, true, true));
            ProductRows.Add(NewRow("234569", "바닥", "강마루 바닥재", "동화자연마루", "나투스 강마루", "EL1234-003", "자동 매칭", "215.60", "m²", "1,294", true, true, true, true));
            ProductRows.Add(NewRow("234570", "단열재", "경질 우레탄폼", "코니코", "코니폼 PF Board", "EL1234-004", "자동 매칭", "95.20", "m²", "612", true, true, true, true));
            ProductRows.Add(NewRow("234571", "구조재", "레미콘", "유진레미콘", "유진 그린 콘크리트", "EL1234-005", "자동 매칭", "45.80", "m³", "5,842", true, true, true, true));
            ProductRows.Add(NewRow("234572", "마감재", "수성페인트", "삼화페인트", "아이생각 수성페인트", "EL1234-006", "자동 매칭", "186.70", "L", "395", true, true, true, true));
            ProductRows.Add(NewRow("234573", "천장", "석고보드", "한국석고보드", "그린보드 9.5T", "EL1234-007", "검토 필요", "124.00", "m²", "842", true, true, true, true));
            ProductRows.Add(NewRow("234574", "바닥", "타일", "삼영타일", "세라믹 타일 300각", "-", "검토 필요", "32.50", "m²", "278", false, true, true, false));
            ProductRows.Add(NewRow("234575", "설비", "PVC 배관", "LG하우시스", "PVC PIPE KS M 3507", "-", "미등록", "86.40", "m", "-", false, false, false, false));
            ProductRows.Add(NewRow("234576", "마감재", "접착제", "쌍곰", "세라픽스 AC-7000", "-", "미등록", "15.80", "kg", "-", false, false, false, false));
        }

        private static EcoProductRow NewRow(
            string revitId,
            string category,
            string materialName,
            string manufacturer,
            string certifiedProduct,
            string certificationNumber,
            string matchStatus,
            string quantity,
            string unit,
            string carbonFootprint,
            bool useGb,
            bool useCarbon,
            bool useVisualization,
            bool useReport)
        {
            return new EcoProductRow
            {
                RevitId = revitId,
                Category = category,
                MaterialName = materialName,
                Manufacturer = manufacturer,
                CertifiedProduct = certifiedProduct,
                CertificationNumber = certificationNumber,
                MatchStatus = matchStatus,
                Quantity = quantity,
                Unit = unit,
                CarbonFootprint = carbonFootprint,
                UseGb = useGb,
                UseCarbon = useCarbon,
                UseVisualization = useVisualization,
                UseReport = useReport
            };
        }

        private static void ShowDemo(string title, string message)
        {
            Autodesk.Revit.UI.TaskDialog.Show("BIM 친환경 정보 연계", $"{title}\n\n{message}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
