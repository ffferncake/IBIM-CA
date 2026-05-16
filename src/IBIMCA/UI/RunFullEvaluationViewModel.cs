using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IBIMCA.UI
{
    public class CertificationOption : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string Description { get; set; } = "";

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RunFullEvaluationViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CertificationOption> Certifications { get; } = new();
        public ObservableCollection<EvaluationNode> RootNodes { get; } = new();
        public ObservableCollection<EvaluationNode> VisibleRows { get; } = new();

        public ICommand ToggleExpandCommand { get; }
        public ICommand SelectCertificationCommand { get; }
        public ICommand ResetSelectionCommand { get; }

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

        public CertificationOption SelectedCertification =>
            Certifications.FirstOrDefault(x => x.IsSelected) ?? Certifications.First();

        public string SelectedCertificationText => $"선택 인증: {SelectedCertification.Code}";
        public string SelectedItemCountText => $"선택 항목: {SelectedLeafCount}개";
        public string RunModeText => "실행 방식: 부분 실행";
        public string SelectionPath => "선택 결과: BF > 1. 매개시설 > 1.1 접근로 > 1.1.1 보도에서 주출입구까지 보행로";

        public int SelectedLeafCount => FlattenLeaves(RootNodes).Count(x => x.IsChecked);

        public RunFullEvaluationViewModel()
        {
            ToggleExpandCommand = new RelayCommand<EvaluationNode>(node =>
            {
                if (node == null || !node.HasChildren) return;
                node.IsExpanded = !node.IsExpanded;
                RebuildVisibleRows();
            });

            SelectCertificationCommand = new RelayCommand<CertificationOption>(option =>
            {
                if (option == null) return;

                foreach (var item in Certifications)
                    item.IsSelected = ReferenceEquals(item, option);

                NotifySelectionChanged();
            });

            ResetSelectionCommand = new RelayCommand<object>(_ =>
            {
                foreach (var node in FlattenAll(RootNodes))
                    node.IsChecked = false;

                var firstLeaf = FlattenLeaves(RootNodes).FirstOrDefault();
                if (firstLeaf != null)
                    firstLeaf.IsChecked = true;

                NotifySelectionChanged();
            });

            BuildCertifications();
            BuildEvaluationRange();
            RebuildVisibleRows();
        }

        private void BuildCertifications()
        {
            Certifications.Add(new CertificationOption
            {
                Name = "장애물 없는 생활환경 인증제도 (BF)",
                Code = "BF",
                Description = "장애물 없는 생활환경 인증 항목을 평가합니다.",
                IsSelected = true
            });
            Certifications.Add(new CertificationOption
            {
                Name = "녹색건축 인증제도 (G-SEED)",
                Code = "G-SEED",
                Description = "녹색건축 성능 및 환경 항목을 평가합니다."
            });
            Certifications.Add(new CertificationOption
            {
                Name = "범죄예방환경설계 (CPTED)",
                Code = "CPTED",
                Description = "범죄예방환경설계 항목을 평가합니다."
            });
        }

        private void BuildEvaluationRange()
        {
            var root = NewNode("BF", "BF 전체", 0, true);

            var n1 = NewNode("1", "1. 매개시설", 1, true);
            var n11 = NewNode("1.1", "1.1 접근로", 2, true);
            n11.Children.Add(NewLeaf("1.1.1", "1.1.1 보도에서 주출입구까지 보행로", 3, true));
            n11.Children.Add(NewLeaf("1.1.2", "1.1.2 유효폭", 3, true));
            n11.Children.Add(NewLeaf("1.1.3", "1.1.3 기울기", 3, false));

            var n12 = NewNode("1.2", "1.2 장애인 전용 주차구역", 2, true);
            n12.Children.Add(NewLeaf("1.2.1", "1.2.1 주차구역 위치", 3, true));
            n12.Children.Add(NewLeaf("1.2.2", "1.2.2 주차면 크기", 3, false));

            n1.Children.Add(n11);
            n1.Children.Add(n12);
            n1.Children.Add(NewLeaf("1.3", "1.3 주출입구", 2, false));

            var n2 = NewNode("2", "2. 내부시설", 1, true);
            n2.Children.Add(NewLeaf("2.1", "2.1 출입문", 2, true));
            n2.Children.Add(NewLeaf("2.2", "2.2 복도", 2, false));
            n2.Children.Add(NewLeaf("2.3", "2.3 계단", 2, false));
            n2.Children.Add(NewLeaf("2.4", "2.4 승강기", 2, true));

            root.Children.Add(n1);
            root.Children.Add(n2);
            RootNodes.Add(root);
        }

        private static EvaluationNode NewNode(string id, string title, int level, bool expanded)
            => new EvaluationNode
            {
                Id = id,
                Title = title,
                Level = level,
                IsExpanded = expanded,
                IsChecked = true
            };

        private static EvaluationNode NewLeaf(string id, string title, int level, bool isChecked)
            => new EvaluationNode
            {
                Id = id,
                Title = title,
                Level = level,
                IsChecked = isChecked
            };

        private void RebuildVisibleRows()
        {
            VisibleRows.Clear();

            foreach (var root in RootNodes)
                AddVisibleRecursive(root);

            NotifySelectionChanged();
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

        private static System.Collections.Generic.List<EvaluationNode> FlattenAll(ObservableCollection<EvaluationNode> nodes)
        {
            var list = new System.Collections.Generic.List<EvaluationNode>();
            foreach (var node in nodes)
            {
                list.Add(node);
                list.AddRange(FlattenAll(node.Children));
            }

            return list;
        }

        private static System.Collections.Generic.List<EvaluationNode> FlattenLeaves(ObservableCollection<EvaluationNode> nodes)
        {
            var list = new System.Collections.Generic.List<EvaluationNode>();
            foreach (var node in nodes)
            {
                if (!node.HasChildren)
                    list.Add(node);
                else
                    list.AddRange(FlattenLeaves(node.Children));
            }

            return list;
        }

        private void NotifySelectionChanged()
        {
            OnPropertyChanged(nameof(SelectedCertification));
            OnPropertyChanged(nameof(SelectedCertificationText));
            OnPropertyChanged(nameof(SelectedItemCountText));
            OnPropertyChanged(nameof(RunModeText));
            OnPropertyChanged(nameof(SelectionPath));
            OnPropertyChanged(nameof(SelectedLeafCount));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
