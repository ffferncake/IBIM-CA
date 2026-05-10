using System.Collections.ObjectModel;
using System.Windows;

namespace IBIMCA.UI
{
    public partial class StandardEvaluationSettingsWindow : Window
    {
        public ObservableCollection<EvalItem> BfItems { get; } = new ObservableCollection<EvalItem>();
        public ObservableCollection<GSeedQualitativeItem> GSeedItems { get; } = new ObservableCollection<GSeedQualitativeItem>();
        public ObservableCollection<CptedQualitativeItem> CptedItems { get; } = new ObservableCollection<CptedQualitativeItem>();

        public StandardEvaluationSettingsWindow()
        {
            InitializeComponent();
            LoadBfItems();
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

        private void LoadGSeedItems()
        {
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "에너지 성능", "에너지효율등급", true));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "에너지 성능", "에너지성능지표", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "TAB 및 커미셔닝 실시 여부", "TAB", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "TAB 및 커미셔닝 실시 여부", "커미셔닝", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "에너지 모니터링 및 관리지원 장치", "(가) 에너지별 모니터링 및 데이터 분석 기능", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "에너지 모니터링 및 관리지원 장치", "(나) 에너지 사용 용도별 모니터링 장치 및 데이터 분석 기능", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "에너지 모니터링 및 관리지원 장치", "(다) 실 용도별 모니터링 및 데이터 분석 기능", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "에너지 모니터링 및 관리지원 장치", "(라) 건축물 에너지 절약을 위한 통합 에너지관리 시스템 구축", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "신·재생에너지 이용", "신·재생에너지 설치비율", true));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "저탄소 에너지원 기술의 적용", "저탄소형 열병합 발전 시스템 또는 고효율 열원 설계", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "저탄소 에너지원 기술의 적용", "자연냉방방식", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "저탄소 에너지원 기술의 적용", "지역냉난방시설", false));
            GSeedItems.Add(new GSeedQualitativeItem("에너지 및 환경오염", "오존층 보호 및 지구온난화 저감", "냉방기기 냉매의 오존층파괴지수 및 지구온난화지수 기준", false));

            GSeedItems.Add(new GSeedQualitativeItem("재료 및 자원", "녹색건축자재의 적용 비율", "녹색건축자재 적용 비율", true));
            GSeedItems.Add(new GSeedQualitativeItem("재료 및 자원", "재활용가능자원의 보관시설 설치", "생활 폐기물 보관시설 및 분리 선별 공간 확보", true));

            GSeedItems.Add(new GSeedQualitativeItem("물순환 관리", "빗물관리", "빗물저류조 설치 및 적용 용량 만족 여부", false));
            GSeedItems.Add(new GSeedQualitativeItem("물순환 관리", "빗물 및 유출지하수 이용", "옥상녹화 설치 및 적용 용량 만족 여부", false));
            GSeedItems.Add(new GSeedQualitativeItem("물순환 관리", "절수형 기기 사용", "절수형 수도꼭지 적용", false));
            GSeedItems.Add(new GSeedQualitativeItem("물순환 관리", "절수형 기기 사용", "절수형 대변기 또는 소변기 절수 부속", false));
            GSeedItems.Add(new GSeedQualitativeItem("물순환 관리", "물 사용량 모니터링", "물 사용량 계측기와 모니터링 시스템 적용", false));

            GSeedItems.Add(new GSeedQualitativeItem("유지관리", "운영 유지관리 문서 및 지침제공", "건축물 운영 유지관리 매뉴얼", false));
            GSeedItems.Add(new GSeedQualitativeItem("유지관리", "운영 유지관리 문서 및 지침제공", "건축설비 유지관리 매뉴얼", false));
            GSeedItems.Add(new GSeedQualitativeItem("유지관리", "운영 유지관리 문서 및 지침제공", "녹색건축 인증 관련 매뉴얼", false));
        }

        private void LoadCptedItems()
        {
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-01", "CPTED", "1. 건물배치", "공공가로 대응 배치 및 출입구 방향"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-02", "CPTED", "1. 건물배치", "저층부 공개형 상점 배치"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-03", "CPTED", "1. 건물배치", "건물 전면공간 보행장애 방지 및 활성화"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-04", "CPTED", "1. 건물배치", "협소가로 교차부 가시범위 확보"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-05", "CPTED", "1. 건물배치", "연속 상업시설 경관 통일감 적용"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-06", "CPTED", "2. 건물 외부", "1층 전면부 투시형 구조 및 개방형 셔터"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-07", "CPTED", "2. 건물 외부", "옥외 간판의 시야·방범·조명 방해 방지"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-08", "CPTED", "2. 건물 외부", "옥외 간판 정돈 이미지 규제"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-09", "CPTED", "2. 건물 외부", "캐노피·어닝의 야간 조명 방해 방지"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-10", "CPTED", "2. 건물 외부", "저층부 유리창 광고·선팅 금지"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-11", "CPTED", "2. 건물 외부", "출입구 조명 및 CCTV 감시범위 배치"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-12", "CPTED", "2. 건물 외부", "출입구 은신공간 방지 디자인"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-13", "CPTED", "2. 건물 외부", "건물 사이공간 활동방지 방지·조명·출입통제"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-14", "CPTED", "3. 건물내부", "출입문 투시형 및 안내표지판"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-15", "CPTED", "3. 건물내부", "출입구 CCTV 및 조명 설치"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-16", "CPTED", "3. 건물내부", "내부 홀·EV·계단 대기공간 외부 가시 및 조명"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-17", "CPTED", "3. 건물내부", "필로티 하부공간 시설·동선 연결 및 조명"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-18", "CPTED", "3. 건물내부", "EV 투시형 전면거울 및 CCTV·비상벨"));
            CptedItems.Add(new CptedQualitativeItem("CPTED-COM-19", "CPTED", "3. 건물내부", "복도/계단실 투시형 창문"));
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
