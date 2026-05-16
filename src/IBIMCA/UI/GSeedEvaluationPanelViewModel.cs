using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IBIMCA.UI
{
    public class GSeedEvaluationPanelViewModel : INotifyPropertyChanged
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

        public int TotalCriteria => FlattenLeaves(RootNodes).Count;
        public int CheckedCriteria => FlattenLeaves(RootNodes).Count(x => x.IsChecked);
        public int TotalMaxScore => FlattenLeaves(RootNodes).Sum(x => x.MaxScore);
        public double TotalScore => FlattenLeaves(RootNodes).Sum(x => x.Score);

        public string StatusItems => $"항목: {CheckedCriteria} / {TotalCriteria}";
        public string StatusScore => $"점수: {EvaluationPanelViewModel.FormatScore(TotalScore)} / {TotalMaxScore}";
        public string StatusGrade => TotalScore >= TotalMaxScore * 0.8 ? "등급: 우수" : "등급: 부적합";

        public GSeedEvaluationPanelViewModel()
        {
            ToggleExpandCommand = new RelayCommand<EvaluationNode>(node =>
            {
                if (node == null || !node.HasChildren) return;
                node.IsExpanded = !node.IsExpanded;
                RebuildVisibleRows();
            });

            BuildGSeedStructure();
            ApplySampleResults();
            RebuildVisibleRows();
        }

        private void BuildGSeedStructure()
        {
            AddCategory("1", "1. 토지이용 및 교통", 14, true,
                ("1.1", "기존대지의 생태학적 가치", 2),
                ("1.2", "과도한 지하개발 지양", 3),
                ("1.3", "토공사 절성토량 최소화", 2),
                ("1.4", "일조권 간섭방지 대책의 타당성", 2),
                ("1.5", "적정 일조권 확보를 위한 배치계획", 1),
                ("1.6", "대중교통의 근접성", 2),
                ("1.7", "자전거주차장 설치", 2));

            AddCategory("2", "2. 에너지 및 환경오염", 29, true,
                ("2.1", "에너지 성능", 12),
                ("2.2", "시험.조정.평가(TAB) 및 커미셔닝 실시", 2),
                ("2.3", "에너지 모니터링 및 관리지원 장치", 2),
                ("2.4", "조명에너지 절약", 4),
                ("2.5", "신.재생에너지 이용", 3),
                ("2.6", "저탄소 에너지원 기술의 적용", 1),
                ("2.7", "오존층 보호를 위한 특정물질의 사용 금지", 3),
                ("2.8", "냉방에너지 절감을 위한 일사조절 계획 수립", 2));

            AddCategory("3", "3. 재료 및 자원", 15, false,
                ("3.1", "환경성 선언 제품의 사용", 3),
                ("3.2", "저탄소 자재의 사용", 2),
                ("3.3", "자원순환 자재의 사용", 2),
                ("3.4", "재활용 가능자원의 분리수거", 2),
                ("3.5", "재료의 유해물질 저감", 2),
                ("3.6", "생활 폐기물 보관시설 계획", 2),
                ("3.7", "건설폐기물 저감 계획", 2));

            AddCategory("4", "4. 물순환관리", 14, false,
                ("4.1", "빗물관리", 2),
                ("4.2", "물 절약", 2),
                ("4.3", "생활용 상수 절감 대책", 2),
                ("4.4", "중수도 및 우수 이용", 2),
                ("4.5", "절수형 기기 사용", 2),
                ("4.6", "우수유출 저감대책", 2),
                ("4.7", "수자원 모니터링 계획", 2));

            AddCategory("5", "5. 유지관리", 8, false,
                ("5.1", "운영 유지관리 문서 및 지침 제공", 2),
                ("5.2", "건물관리 시스템 구축", 1),
                ("5.3", "녹색건축 교육 및 운영 계획", 1),
                ("5.4", "사용자 매뉴얼 제공", 1),
                ("5.5", "에너지 사용량 관리 계획", 1),
                ("5.6", "시설물 성능 유지계획", 2));

            AddCategory("6", "6. 생태환경", 17, false,
                ("6.1", "생태면적률 확보", 3),
                ("6.2", "자연지반 녹지율", 3),
                ("6.3", "생물서식공간 조성", 2),
                ("6.4", "옥상 및 벽면 녹화", 2),
                ("6.5", "표토재활용", 2),
                ("6.6", "수생비오톱 조성", 2),
                ("6.7", "녹지 네트워크 연계", 3));

            AddCategory("7", "7. 실내환경", 39, false,
                ("7.1", "실내공기 오염물질 저방출 제품 적용", 4),
                ("7.2", "자연환기성능 확보", 4),
                ("7.3", "외기 급배기구의 설계", 4),
                ("7.4", "실내 소음환경", 4),
                ("7.5", "채광 및 조망 확보", 3),
                ("7.6", "자동온도조절장치 채택", 3),
                ("7.7", "쾌적한 실내 열환경 조성", 4),
                ("7.8", "휴식 및 커뮤니티 공간 계획", 3),
                ("7.9", "거주자 건강을 고려한 마감계획", 3),
                ("7.10", "시각적 쾌적성 확보", 3),
                ("7.11", "실내 유해물질 관리", 2),
                ("7.12", "사용자 건강지원 공간 계획", 2));
        }

        private void AddCategory(string id, string title, int maxScore, bool expanded, params (string Id, string Title, int Score)[] items)
        {
            var category = NewNode(id, title, maxScore, 0, expanded);

            foreach (var item in items)
                category.Children.Add(NewLeaf(item.Id, $"{item.Id} {item.Title}", 1, item.Score));

            RootNodes.Add(category);
        }

        private static EvaluationNode NewNode(string id, string title, int maxScore, int level, bool expanded)
            => new EvaluationNode
            {
                Id = id,
                Title = title,
                MaxScore = maxScore,
                Level = level,
                IsExpanded = expanded,
                IsChecked = true
            };

        private static EvaluationNode NewLeaf(string id, string title, int level, int maxScore)
            => new EvaluationNode
            {
                Id = id,
                Title = title,
                MaxScore = maxScore,
                Level = level,
                IsChecked = true
            };

        private void ApplySampleResults()
        {
            var leaves = FlattenLeaves(RootNodes);

            for (int i = 0; i < leaves.Count; i++)
            {
                var leaf = leaves[i];

                if ((i + 1) % 13 == 0)
                {
                    leaf.Score = 0;
                    leaf.Result = "N/A";
                }
                else if ((i + 1) % 4 == 0)
                {
                    leaf.Score = 0;
                    leaf.Result = "FAIL";
                }
                else
                {
                    leaf.Score = leaf.MaxScore;
                    leaf.Result = "PASS";
                }
            }

            foreach (var root in RootNodes)
                Rollup(root);
        }

        private static void Rollup(EvaluationNode node)
        {
            if (!node.HasChildren) return;

            foreach (var child in node.Children)
                Rollup(child);

            node.Score = node.Children.Sum(x => x.Score);

            var children = node.Children.ToList();
            if (children.All(x => x.Result == "N/A"))
                node.Result = "N/A";
            else
                node.Result = node.Score >= node.MaxScore * 0.8 ? "PASS" : "FAIL";
        }

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

        private static List<EvaluationNode> FlattenLeaves(ObservableCollection<EvaluationNode> nodes)
        {
            var leaves = new List<EvaluationNode>();

            foreach (var node in nodes)
            {
                if (!node.HasChildren)
                    leaves.Add(node);
                else
                    leaves.AddRange(FlattenLeaves(node.Children));
            }

            return leaves;
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
