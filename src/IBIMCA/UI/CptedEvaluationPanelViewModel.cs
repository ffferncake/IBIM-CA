using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IBIMCA.UI
{
    public class CptedEvaluationPanelViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EvaluationNode> RootNodes { get; } = new();
        public ObservableCollection<EvaluationNode> VisibleRows { get; } = new();

        public ICommand ToggleExpandCommand { get; }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged();
                RebuildVisibleRows();
            }
        }

        public int TotalCriteria => FlattenCriteria(RootNodes).Count;
        public int CheckedCriteria => FlattenCriteria(RootNodes).Count(x => x.IsChecked);
        public int TotalMaxScore => FlattenCriteria(RootNodes).Sum(x => x.MaxScore);
        public double TotalScore => FlattenCriteria(RootNodes).Sum(x => x.Score);

        public string StatusItems => $"항목: {CheckedCriteria} / {TotalCriteria}";
        public string StatusScore => $"점수: {EvaluationPanelViewModel.FormatScore(TotalScore)} / {TotalMaxScore}";
        public string StatusGrade => "등급: 최우수";
        public string FacilityCount => $"시설별 평가항목 개수: {CheckedCriteria}/{TotalCriteria}";

        public CptedEvaluationPanelViewModel()
        {
            ToggleExpandCommand = new RelayCommand<EvaluationNode>(node =>
            {
                if (node == null || !node.HasChildren) return;
                node.IsExpanded = !node.IsExpanded;
                RebuildVisibleRows();
            });

            BuildCptedStructure();
            RebuildVisibleRows();
        }

        private void BuildCptedStructure()
        {
            var root = NewNode("CPTED", "상업시설", 175, 115, "FAIL", 0, true);

            root.Children.Add(BuildCategory(
                "1",
                "1. 건물 배치",
                25,
                25,
                "PASS",
                true,
                "상업시설 배치 CPTED 가이드라인 : 건물배치 및 총별 입점기준",
                true,
                ("1", "상업건물(상점)은 공공가로(보행로)와 대응해서 배치하고 출입구는 보행로를 향하도록 배치한다(감시, 영역성강화).", 5, 5, "PASS"),
                ("2", "저층부는 내부를 공개할 수 있는 상점을 입점시킨다(감시, 접근통제).", 5, 5, "PASS"),
                ("3", "건물(상점), 보행로, 도로의 위계를 명확히 지키면서 건물전면 공간은 보행자의 이동에 장애가 되지 않도록 하며(불법 주정차 금지), 필요시 공간이 활성화될 수 있도록 디자인한다(영역성강화, 활용성증대).", 5, 5, "PASS"),
                ("4", "좁은 가로와 연계되는 ㄱ, ㄴ, T자형 교차점에 위치한 상점계획용지는 건축선 후퇴나 가각공제를 통해 보행공간의 가시범위를 확보하는 것이 필요하다. 다만, 이러한 필지 정리가 곤란한 경우 모서리 입면은 투시형 구조를 권장한다(감시).", 5, 5, "PASS"),
                ("5", "연속되는 상업시설의 경우 가로공간의 연속성, 건축적 동일감 등을 통해 정돈된 이미지를 제공할 수 있도록 재료, 디자인, 색채 등에 대한 공통된 가이드라인을 적용하는 것이 필요하다(영역성강화, 명료성강화).", 5, 5, "PASS")));

            root.Children.Add(BuildCategory(
                "2",
                "2. 건물 외부",
                40,
                30,
                "FAIL",
                true,
                "건물외부 CPTED 가이드라인 : 건물외부 디자인 및 시설설치 기준",
                false,
                ("1", "주요 출입구와 보행동선은 자연적 감시가 가능한 위치에 계획한다.", 5, 5, "PASS"),
                ("2", "외부 조명은 보행로와 출입구를 균일하게 비추도록 설치한다.", 5, 5, "PASS"),
                ("3", "CCTV는 사각지대가 발생하지 않도록 출입구와 공용공간을 중심으로 배치한다.", 5, 5, "PASS"),
                ("4", "외부 설비와 후면공간은 접근통제 계획을 포함한다.", 5, 5, "PASS"),
                ("5", "건물 외벽과 간판은 시야를 방해하지 않는 규모와 위치로 계획한다.", 5, 5, "PASS"),
                ("6", "야간 이용구간에는 비상벨 또는 긴급호출 장치를 설치한다.", 5, 5, "PASS"),
                ("7", "방범 취약공간에는 보완 조명과 안내표지를 설치한다.", 5, 0, "FAIL"),
                ("8", "외부 휴게공간은 은닉공간이 생기지 않도록 개방적으로 계획한다.", 5, 0, "FAIL")));

            root.Children.Add(BuildCategory(
                "3",
                "3. 건물 내부",
                55,
                40,
                "FAIL",
                true,
                "건물내부 CPTED 가이드라인 : 건물내부 공간구성 및 시설설치 기준",
                false,
                ("1", "내부 공용공간은 시야가 연속되도록 배치한다.", 5, 5, "PASS"),
                ("2", "계단실과 엘리베이터 홀은 투시형 구조와 조명을 확보한다.", 5, 5, "PASS"),
                ("3", "화장실 진입부는 공용공간에서 확인 가능한 위치에 둔다.", 5, 5, "PASS"),
                ("4", "복도와 대기공간은 사각지대가 생기지 않도록 계획한다.", 5, 5, "PASS"),
                ("5", "내부 CCTV는 주요 동선과 공용공간을 중심으로 설치한다.", 5, 5, "PASS"),
                ("6", "안내표지와 층별 동선표시는 명확하게 계획한다.", 5, 5, "PASS"),
                ("7", "비상벨 및 경보설비는 접근 가능한 위치에 설치한다.", 5, 5, "PASS"),
                ("8", "야간 운영공간은 관리자의 시야 확보가 가능해야 한다.", 5, 5, "PASS"),
                ("9", "출입통제구역은 이용자 동선과 분리한다.", 5, 0, "FAIL"),
                ("10", "창고 및 후면공간은 잠금장치와 조명계획을 포함한다.", 5, 0, "FAIL"),
                ("11", "공용부 마감은 훼손과 은닉행위를 줄이는 재료를 적용한다.", 5, 0, "FAIL")));

            root.Children.Add(BuildCategory(
                "4",
                "4. 주차 공간",
                30,
                20,
                "PASS",
                true,
                "주차공간 CPTED 가이드라인 : 주차공간 배치 및 시설설치 기준",
                false,
                ("1", "주차장 진입부는 보행자 동선과 충돌하지 않도록 계획한다.", 5, 5, "PASS"),
                ("2", "주차구역은 충분한 조도와 시야를 확보한다.", 5, 5, "PASS"),
                ("3", "보행통로와 차량동선은 명확히 구분한다.", 5, 5, "PASS"),
                ("4", "주차장 CCTV는 출입구와 주요 통로를 포함한다.", 5, 5, "PASS"),
                ("5", "장애인 주차구역은 출입구와 안전하게 연결한다.", 5, 0, "FAIL"),
                ("6", "주차장 사각지대에는 반사경 또는 보완설비를 설치한다.", 5, 0, "FAIL")));

            root.Children.Add(BuildCategory(
                "5",
                "5. 편의점",
                25,
                0,
                "N/A",
                false,
                "편의점 CPTED 가이드라인 : 점포 운영 및 야간 안전관리 기준",
                false,
                ("1", "점포 전면부는 외부에서 내부 활동을 확인할 수 있도록 계획한다.", 5, 0, "N/A"),
                ("2", "계산대와 출입구는 관리자의 시야가 확보되도록 배치한다.", 5, 0, "N/A"),
                ("3", "야간 운영 시 외부 조도와 비상연락체계를 확보한다.", 5, 0, "N/A"),
                ("4", "휴게 및 적치공간은 통행과 감시를 방해하지 않도록 관리한다.", 5, 0, "N/A"),
                ("5", "무인 운영공간은 출입통제와 원격감시 설비를 포함한다.", 5, 0, "N/A")));

            RootNodes.Add(root);
        }

        private EvaluationNode BuildCategory(
            string id,
            string title,
            int maxScore,
            int score,
            string result,
            bool expanded,
            string guidelineTitle,
            bool guidelineExpanded,
            params (string Id, string Title, int MaxScore, int Score, string Result)[] criteria)
        {
            var category = NewNode(id, title, maxScore, score, result, 1, expanded);
            var guideline = NewNode($"{id}.G", guidelineTitle, maxScore, score, result, 2, guidelineExpanded);

            foreach (var item in criteria)
            {
                guideline.Children.Add(NewNode(
                    $"{id}.{item.Id}",
                    $"{item.Id}. {item.Title}",
                    item.MaxScore,
                    item.Score,
                    item.Result,
                    3,
                    false));
            }

            category.Children.Add(guideline);
            return category;
        }

        private static EvaluationNode NewNode(string id, string title, int maxScore, int score, string result, int level, bool expanded)
            => new EvaluationNode
            {
                Id = id,
                Title = title,
                MaxScore = maxScore,
                Score = score,
                Result = result,
                Level = level,
                IsExpanded = expanded,
                IsChecked = true
            };

        private void RebuildVisibleRows()
        {
            VisibleRows.Clear();

            foreach (var root in RootNodes)
                AddVisibleRecursive(root);

            NotifyStatusChanged();
        }

        private void AddVisibleRecursive(EvaluationNode node)
        {
            if (!PassSearch(node)) return;

            VisibleRows.Add(node);

            if (node.HasChildren && node.IsExpanded)
            {
                foreach (var child in node.Children)
                    AddVisibleRecursive(child);
            }
        }

        private bool PassSearch(EvaluationNode node)
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return true;
            return node.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        private static List<EvaluationNode> FlattenCriteria(ObservableCollection<EvaluationNode> nodes)
        {
            var criteria = new List<EvaluationNode>();

            foreach (var node in nodes)
            {
                if (!node.HasChildren && node.Level == 3)
                    criteria.Add(node);
                else
                    criteria.AddRange(FlattenCriteria(node.Children));
            }

            return criteria;
        }

        public void NotifyStatusChanged()
        {
            OnPropertyChanged(nameof(TotalCriteria));
            OnPropertyChanged(nameof(CheckedCriteria));
            OnPropertyChanged(nameof(TotalMaxScore));
            OnPropertyChanged(nameof(TotalScore));
            OnPropertyChanged(nameof(StatusItems));
            OnPropertyChanged(nameof(StatusScore));
            OnPropertyChanged(nameof(StatusGrade));
            OnPropertyChanged(nameof(FacilityCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
