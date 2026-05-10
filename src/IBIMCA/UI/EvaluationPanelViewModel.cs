// UI/EvaluationPanelViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace IBIMCA.UI
{
    public class EvaluationNode : INotifyPropertyChanged
    {
        bool _isChecked;
        bool _isExpanded;
        int _score;
        int _level;

        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public int MaxScore { get; set; }

        public int Score
        {
            get => _score;
            set
            {
                int clamped = value;
                if (clamped < 0) clamped = 0;
                if (MaxScore > 0 && clamped > MaxScore) clamped = MaxScore;

                if (_score == clamped) return;
                _score = clamped;
                OnPropertyChanged();
            }
        }

        string _result = "";   // ✅ 처음에는 빈칸
        public string Result
        {
            get => _result;
            set
            {
                if (_result == value) return;
                _result = value;
                OnPropertyChanged();
            }
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ExpandGlyph)); // 컨버터 대체
            }
        }

        public ObservableCollection<EvaluationNode> Children { get; } = new();

        public int Level
        {
            get => _level;
            set
            {
                if (_level == value) return;
                _level = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IndentMargin)); // 컨버터 대체
            }
        }

        public bool HasChildren => Children.Count > 0;

        // ===== 컨버터 대체 속성 =====
        public Thickness IndentMargin => new Thickness(Level * 16, 0, 0, 0);
        public string ExpandGlyph => IsExpanded ? "▼" : "▶";
        // ===========================

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string n = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    public class EvaluationPanelViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<EvaluationNode> RootNodes { get; } = new();
        public ObservableCollection<EvaluationNode> VisibleRows { get; } = new();

        public ICommand ToggleExpandCommand { get; }

        string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); RebuildVisibleRows(); }
        }

        // counts include all nodes including sub rows
        public int TotalCount => FlattenAll(RootNodes).Count;
        public int CheckedCount => FlattenAll(RootNodes).Count(x => x.IsChecked);

        public int TotalMaxScore => FlattenAll(RootNodes).Sum(x => x.MaxScore);
        public int TotalScore => FlattenAll(RootNodes).Sum(x => x.Score);

        public string StatusItems => $"항목: {CheckedCount} / {TotalCount}";
        public string StatusScore => $"점수: {TotalScore} / {TotalMaxScore}";
        public string StatusPercent =>
            TotalMaxScore == 0
                ? "백분율: 0.00%"
                : $"백분율: {(TotalScore * 100.0 / TotalMaxScore):0.00}%";

        public string StatusGrade =>
            TotalMaxScore == 0
                ? "등급: -"
                : (TotalScore >= TotalMaxScore * 0.8 ? "등급: 우수" : "등급: 부적합");

        public EvaluationPanelViewModel()
        {
            ToggleExpandCommand = new RelayCommand<EvaluationNode>(node =>
            {
                if (node == null || !node.HasChildren) return;
                node.IsExpanded = !node.IsExpanded;
                RebuildVisibleRows();
            });

            BuildBFStructure();
            RebuildVisibleRows();
        }

        private void BuildBFStructure()
        {
            // 1. 매개시설
            var n1 = NewNode("1", "1. 매개시설", maxScore: 64, level: 0, expanded: true);

            var n11 = NewNode("1.1", "1.1 접근로", level: 1, expanded: true);
            n11.Children.Add(NewLeaf("1.1.1", "1.1.1 보도에서 주출입구까지 보행로", maxScore: 2, level: 2));
            n11.Children.Add(NewLeaf("1.1.2", "1.1.2 유효폭", maxScore: 2, level: 2));
            n11.Children.Add(NewLeaf("1.1.3", "1.1.3 단차", maxScore: 2, level: 2));
            n11.Children.Add(NewLeaf("1.1.4", "1.1.4 기울기", maxScore: 2, level: 2));
            n11.Children.Add(NewLeaf("1.1.5", "1.1.5 바닥마감", maxScore: 2, level: 2));
            n11.Children.Add(NewLeaf("1.1.6", "1.1.6 보행장애물", maxScore: 2, level: 2));
            n11.Children.Add(NewLeaf("1.1.7", "1.1.7 덮개", maxScore: 2, level: 2));

            var n12 = NewNode("1.2", "1.2 장애인 전용주차 구역", level: 1, expanded: false);
            n12.Children.Add(NewLeaf("1.2.1", "1.2.1 주차장에서 출입구까지의 경로", maxScore: 2, level: 2));
            n12.Children.Add(NewLeaf("1.2.2", "1.2.2 주차면수 확보", maxScore: 2, level: 2));
            n12.Children.Add(NewLeaf("1.2.3", "1.2.3 주차구역 크기", maxScore: 2, level: 2));
            n12.Children.Add(NewLeaf("1.2.4", "1.2.4 보행 안전통로", maxScore: 2, level: 2));
            n12.Children.Add(NewLeaf("1.2.5", "1.2.5 안내 및 유도표시", maxScore: 2, level: 2));

            var n13 = NewNode("1.3", "1.3 주출입구 (문)", level: 1, expanded: false);
            n13.Children.Add(NewLeaf("1.3.1", "1.3.1 주출입구의 높이 차이", maxScore: 2, level: 2));
            n13.Children.Add(NewLeaf("1.3.2", "1.3.2 주출입문의 형태", maxScore: 2, level: 2));
            n13.Children.Add(NewLeaf("1.3.3", "1.3.3 유효폭", maxScore: 2, level: 2));
            n13.Children.Add(NewLeaf("1.3.4", "1.3.4 단차", maxScore: 2, level: 2));
            n13.Children.Add(NewLeaf("1.3.5", "1.3.5 전면유효거리", maxScore: 2, level: 2));
            n13.Children.Add(NewLeaf("1.3.6", "1.3.6 손잡이", maxScore: 2, level: 2));
            n13.Children.Add(NewLeaf("1.3.7", "1.3.7 경고블록", maxScore: 2, level: 2));

            n1.Children.Add(n11);
            n1.Children.Add(n12);
            n1.Children.Add(n13);

            // 2. 내부시설 (샘플 일부)
            var n2 = NewNode("2", "2. 내부시설", maxScore: 63, level: 0, expanded: false);

            var n21 = NewNode("2.1", "2.1 일반출입문", level: 1, expanded: true);
            n21.Children.Add(NewLeaf("2.1.1", "2.1.1 단차", maxScore: 2, level: 2));
            n21.Children.Add(NewLeaf("2.1.2", "2.1.2 유효폭", maxScore: 2, level: 2));
            n21.Children.Add(NewLeaf("2.1.3", "2.1.3 전후면 유효거리", maxScore: 2, level: 2));
            n21.Children.Add(NewLeaf("2.1.4", "2.1.4 손잡이 및 점자표지판", maxScore: 2, level: 2));

            n2.Children.Add(n21);

            // TODO: add 2.2~6.1 same way

            RootNodes.Add(n1);
            RootNodes.Add(n2);
        }

        private EvaluationNode NewNode(string id, string title, int maxScore = 0, int level = 0, bool expanded = false)
            => new EvaluationNode
            {
                Id = id,
                Title = title,
                MaxScore = maxScore,
                Level = level,
                IsExpanded = expanded,
                IsChecked = true
            };

        private EvaluationNode NewLeaf(string id, string title, int level, int maxScore)
            => new EvaluationNode
            {
                Id = id,
                Title = title,
                MaxScore = maxScore, 
                Level = level,
                IsChecked = true
            };

        private void RebuildVisibleRows()
        {
            VisibleRows.Clear();

            foreach (var root in RootNodes)
                AddVisibleRecursive(root);

            OnPropertyChanged(nameof(StatusItems));
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(CheckedCount));
        }

        private void AddVisibleRecursive(EvaluationNode node)
        {
            if (!PassSearch(node)) return;

            VisibleRows.Add(node);

            if (node.HasChildren && node.IsExpanded)
            {
                foreach (var ch in node.Children)
                    AddVisibleRecursive(ch);
            }
        }

        private bool PassSearch(EvaluationNode node)
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return true;
            return node.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        private static System.Collections.Generic.List<EvaluationNode> FlattenAll(ObservableCollection<EvaluationNode> nodes)
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

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string n = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public void NotifyStatusChanged()
        {
            OnPropertyChanged(nameof(StatusItems));
            OnPropertyChanged(nameof(StatusScore));
            OnPropertyChanged(nameof(StatusPercent));
            OnPropertyChanged(nameof(StatusGrade));
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        public RelayCommand(Action<T> execute) => _execute = execute;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute((T)parameter);

        public event EventHandler CanExecuteChanged;
    }
}