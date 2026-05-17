using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using ClosedXML.Excel;

namespace IBIMCA.UI
{
    public partial class StandardEvaluationSettingsWindow : Window
    {
        public ObservableCollection<EvalItem> BfItems { get; } = new ObservableCollection<EvalItem>();
        public ObservableCollection<BfReferenceNote> BfNotes { get; } = new ObservableCollection<BfReferenceNote>();
        public ObservableCollection<QualitativeChecklistItem> GSeedItems { get; } = new ObservableCollection<QualitativeChecklistItem>();
        public ObservableCollection<QualitativeChecklistItem> CptedItems { get; } = new ObservableCollection<QualitativeChecklistItem>();

        public StandardEvaluationSettingsWindow()
        {
            InitializeComponent();
            LoadBfItems();
            LoadBfChecklistFromExcelSpec();
            LoadGSeedItems();
            LoadCptedItems();
            DataContext = this;
        }

        private void LoadBfItems()
        {
            BfItems.Add(new EvalItem("바닥", "미끄럼 방지", true));
            BfItems.Add(new EvalItem("바닥", "걸려넘어질 염려가 없음", true));
            BfItems.Add(new EvalItem("바닥", "충격흡수", true));
            BfItems.Add(new EvalItem("바닥", "색상변화", true));
            BfItems.Add(new EvalItem("바닥", "울림이 적음", true));

            BfItems.Add(new EvalItem("손잡이", "미끄럼 방지", true));
            BfItems.Add(new EvalItem("손잡이", "차갑지 않은", false));
            BfItems.Add(new EvalItem("손잡이", "점자 표시", false));

            BfItems.Add(new EvalItem("시설물", "마감처리", true));
            BfItems.Add(new EvalItem("시설물", "색상대비", true));
            BfItems.Add(new EvalItem("시설물", "그림", true));
        }

        private void LoadBfChecklistFromExcelSpec()
        {
            BfItems.Clear();
            BfNotes.Clear();

            BfItems.Add(new EvalItem("1", "바닥\n(Floor)", "미끄럼 방지\n(Anti-slip)", "✔ True", "Checkbox", "Boolean", "floor_antislip", "재료 또는 표면 처리 기준 확인", true));
            BfItems.Add(new EvalItem("2", "바닥\n(Floor)", "걸려넘어질 염려가 없음\n(No trip hazard)", "✔ True", "Checkbox", "Boolean", "floor_no_trip", "단차·요철 없음 여부 확인", true));
            BfItems.Add(new EvalItem("3", "바닥\n(Floor)", "충격흡수\n(Shock absorption)", "✔ True", "Checkbox", "Boolean", "floor_shock_absorb", "탄성 재료 사용 여부", true));
            BfItems.Add(new EvalItem("4", "바닥\n(Floor)", "색상변화\n(Color variation)", "✔ True", "Checkbox", "Boolean", "floor_color_change", "시각장애인 유도 색상 적용 여부", true));
            BfItems.Add(new EvalItem("5", "바닥\n(Floor)", "울림이 적음\n(Low echo)", "✔ True", "Checkbox", "Boolean", "floor_low_echo", "음향 반향 최소화 여부", true));
            BfItems.Add(new EvalItem("6", "손잡이\n(Handrail)", "미끄럼 방지\n(Anti-slip)", "✔ True", "Checkbox", "Boolean", "handrail_antislip", "손잡이 표면 처리 기준", true));
            BfItems.Add(new EvalItem("7", "손잡이\n(Handrail)", "차갑지 않은\n(Non-cold material)", "✔ True", "Checkbox", "Boolean", "handrail_warm", "금속 재료 단열 처리 여부", true));
            BfItems.Add(new EvalItem("8", "손잡이\n(Handrail)", "점자 표시\n(Braille marking)", "✔ True", "Checkbox", "Boolean", "handrail_braille", "점자 안내 부착 여부", true));
            BfItems.Add(new EvalItem("9", "시설물\n(Facility)", "촉지도식 안내판\n(Tactile map board)", "✔ True", "Checkbox", "Boolean", "fac_tactile_map", "촉지도 설치 여부·위치", true));
            BfItems.Add(new EvalItem("10", "시설물\n(Facility)", "마감처리\n(Surface finish)", "✔ True", "Checkbox", "Boolean", "fac_finish", "날카로운 모서리·돌출 방지", true));
            BfItems.Add(new EvalItem("11", "시설물\n(Facility)", "색상대비\n(Color contrast)", "✔ True", "Checkbox", "Boolean", "fac_color_contrast", "배경 대비 명도 기준 충족", true));
            BfItems.Add(new EvalItem("12", "시설물\n(Facility)", "그림(픽토그램)\n(Pictogram)", "✔ True", "Checkbox", "Boolean", "fac_pictogram", "픽토그램 병행 표기 여부", true));
            BfItems.Add(new EvalItem("13", "시설물\n(Facility)", "외국어\n(Foreign language)", "✔ True", "Checkbox", "Boolean", "fac_foreign_lang", "영문 등 외국어 병기 여부", true));
            BfItems.Add(new EvalItem("14", "시설물\n(Facility)", "음성안내\n(Voice guidance)", "✔ True", "Checkbox", "Boolean", "fac_voice_guide", "음성 안내 장치 설치 여부", true));
            BfItems.Add(new EvalItem("15", "시설물\n(Facility)", "문자안내\n(Text guidance)", "✔ True", "Checkbox", "Boolean", "fac_text_guide", "LED·디지털 문자 안내 여부", true));
            BfItems.Add(new EvalItem("16", "피난\n(Evacuation)", "공용공간 피난구 설치\n(Common area exit)", "☐ False", "Checkbox", "Boolean", "evac_common_exit", "공용공간 내 피난구 존재 여부", false));
            BfItems.Add(new EvalItem("17", "피난\n(Evacuation)", "각실 피난구 설치\n(Room exit)", "☐ False", "Checkbox", "Boolean", "evac_room_exit", "각 실별 피난구 설치 여부", false));
            BfItems.Add(new EvalItem("18", "피난\n(Evacuation)", "피난 매뉴얼 구비\n(Evacuation manual)", "☐ False", "Checkbox", "Boolean", "evac_manual", "피난 안내 매뉴얼 비치 여부", false));
            BfItems.Add(new EvalItem("19", "피난\n(Evacuation)", "외부로 피난가능\n(Escape to exterior)", "☐ False", "Checkbox", "Boolean", "evac_to_exterior", "외부 직접 탈출 가능 여부", false));
            BfItems.Add(new EvalItem("20", "피난\n(Evacuation)", "외부에서 지상으로 피난가능\n(Exterior to ground)", "☐ False", "Checkbox", "Boolean", "evac_to_ground", "외부 → 지상 레벨 도달 가능", false));

            BfNotes.Add(new BfReferenceNote("【범례】 ✔ True: 시스템 초기 로드 시 기본값 True로 설정됨 (BF 기준 충족 대상)"));
            BfNotes.Add(new BfReferenceNote("【범례】 ☐ False: 사용자 수동 입력 필요. 현장 조건에 따라 추가 확인 필요"));
            BfNotes.Add(new BfReferenceNote("Boolean: DB 저장 타입 True / False (1 / 0), Checkbox: UI 컨트롤은 체크박스 형태로 표시"));
            BfNotes.Add(new BfReferenceNote("개발 참고: 각 항목은 프로젝트별 독립 Boolean 값으로 DB에 저장하고, 유형(Type)은 UI 그룹핑 및 category 컬럼 매핑에 활용"));
            BfNotes.Add(new BfReferenceNote("피난(Evacuation) 항목은 기본값 False이며, 추후 BIM IFC 자동 매핑을 고려해 DB 필드명은 snake_case로 관리"));
        }

        private void LoadGSeedItems()
        {
            LoadChecklistWorkbook(
                GSeedItems,
                @"C:\Users\justi\Documents\카카오톡 받은 파일\GB_정성적평가_체크리스트_개발자용.xlsx");
        }

        private void LoadCptedItems()
        {
            LoadChecklistWorkbook(
                CptedItems,
                @"C:\Users\justi\Documents\카카오톡 받은 파일\CPTED_정성적평가_체크리스트_개발자용.xlsx");
        }

        private static void LoadChecklistWorkbook(ObservableCollection<QualitativeChecklistItem> target, string path)
        {
            target.Clear();
            if (!File.Exists(path))
                return;

            using var workbook = new XLWorkbook(path);
            var worksheet = workbook.Worksheets.Worksheet(1);
            var currentCategory = string.Empty;
            var currentPrimary = string.Empty;

            foreach (var row in worksheet.RowsUsed().Skip(3))
            {
                var no = row.Cell(1).GetString().Trim();
                if (!int.TryParse(no, out _))
                    continue;

                var category = row.Cell(2).GetString().Trim();
                var primary = row.Cell(3).GetString().Trim();
                if (!string.IsNullOrWhiteSpace(category))
                    currentCategory = category;
                if (!string.IsNullOrWhiteSpace(primary))
                    currentPrimary = primary;

                var defaultText = row.Cell(5).GetString().Trim();
                target.Add(new QualitativeChecklistItem
                {
                    No = no,
                    Category = NormalizeCell(currentCategory),
                    PrimaryItem = NormalizeCell(currentPrimary),
                    SecondaryItem = NormalizeCell(row.Cell(4).GetString()),
                    DefaultText = NormalizeCell(defaultText),
                    UiControl = NormalizeCell(row.Cell(6).GetString()),
                    DataType = NormalizeCell(row.Cell(7).GetString()),
                    Options = NormalizeCell(row.Cell(8).GetString()),
                    DbField = NormalizeCell(row.Cell(9).GetString()),
                    DevNotes = NormalizeCell(row.Cell(10).GetString()),
                    IsEnabled = defaultText.Contains("True")
                });
            }
        }

        private static string NormalizeCell(string value)
        {
            return value.Replace("\r", " ").Replace("\n", " / ").Trim();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("저장 (TODO)", "IBIMCA");
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class EvalItem
    {
        public string No { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string DefaultText { get; set; }
        public string UiControl { get; set; }
        public string DataType { get; set; }
        public string DbField { get; set; }
        public string DevNotes { get; set; }
        public bool IsEnabled { get; set; }

        public EvalItem(string category, string name, bool isEnabled)
        {
            No = string.Empty;
            Category = category;
            Name = name;
            DefaultText = isEnabled ? "✔ True" : "☐ False";
            UiControl = "Checkbox";
            DataType = "Boolean";
            DbField = string.Empty;
            DevNotes = string.Empty;
            IsEnabled = isEnabled;
        }

        public EvalItem(string no, string category, string name, string defaultText, string uiControl, string dataType, string dbField, string devNotes, bool isEnabled)
        {
            No = no;
            Category = category;
            Name = name;
            DefaultText = defaultText;
            UiControl = uiControl;
            DataType = dataType;
            DbField = dbField;
            DevNotes = devNotes;
            IsEnabled = isEnabled;
        }
    }

    public class BfReferenceNote
    {
        public string Text { get; set; }

        public BfReferenceNote(string text)
        {
            Text = text;
        }
    }

    public class QualitativeChecklistItem
    {
        public string No { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string PrimaryItem { get; set; } = string.Empty;
        public string SecondaryItem { get; set; } = string.Empty;
        public string DefaultText { get; set; } = string.Empty;
        public string UiControl { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public string Options { get; set; } = string.Empty;
        public string DbField { get; set; } = string.Empty;
        public string DevNotes { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }

    public class GSeedQualitativeItem
    {
        public string Type { get; set; }
        public string CertificationItem { get; set; }
        public string Info { get; set; }
        public bool IsEnabled { get; set; }

        public GSeedQualitativeItem(string type, string certificationItem, string info, bool isEnabled)
        {
            Type = type;
            CertificationItem = certificationItem;
            Info = info;
            IsEnabled = isEnabled;
        }
    }

    public class CptedQualitativeItem
    {
        public string Id { get; set; }
        public string Certification { get; set; }
        public string MajorCategory { get; set; }
        public string RepresentativeItem { get; set; }

        public CptedQualitativeItem(string id, string certification, string majorCategory, string representativeItem)
        {
            Id = id;
            Certification = certification;
            MajorCategory = majorCategory;
            RepresentativeItem = representativeItem;
        }
    }
}
