using Autodesk.Revit.UI;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IBIMCA.UI
{
    public class UpdateExternalDBViewModel : INotifyPropertyChanged
    {
        // Tab
        int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set { _selectedTabIndex = value; OnPropertyChanged(); }
        }

        // Paths (left)
        string _bfPath = "";
        public string BfPath { get => _bfPath; set { _bfPath = value; OnPropertyChanged(); } }

        string _gbPath = "";
        public string GbPath { get => _gbPath; set { _gbPath = value; OnPropertyChanged(); } }

        string _cptedPath = "";
        public string CptedPath { get => _cptedPath; set { _cptedPath = value; OnPropertyChanged(); } }

        // Paths (right)
        string _eco1Path = "";
        public string Eco1Path { get => _eco1Path; set { _eco1Path = value; OnPropertyChanged(); } }

        string _eco2Path = "";
        public string Eco2Path { get => _eco2Path; set { _eco2Path = value; OnPropertyChanged(); } }

        string _eco3Path = "";
        public string Eco3Path { get => _eco3Path; set { _eco3Path = value; OnPropertyChanged(); } }

        string _statusText = "파일을 선택한 뒤 [완료]를 누르면 갱신을 시작합니다.";
        public string StatusText { get => _statusText; set { _statusText = value; OnPropertyChanged(); } }

        // Commands
        public ICommand BrowseBfCommand { get; }
        public ICommand BrowseGbCommand { get; }
        public ICommand BrowseCptedCommand { get; }
        public ICommand BrowseEco1Command { get; }
        public ICommand BrowseEco2Command { get; }
        public ICommand BrowseEco3Command { get; }

        public ICommand LoadAllCommand { get; }
        public ICommand ExportAllCommand { get; }
        public ICommand FinishCommand { get; }

        public UpdateExternalDBViewModel()
        {
            BrowseBfCommand = new RelayCommand(_ => BfPath = BrowseAnyFile("BF인증 파일 선택"));
            BrowseGbCommand = new RelayCommand(_ => GbPath = BrowseAnyFile("GB인증 파일 선택"));
            BrowseCptedCommand = new RelayCommand(_ => CptedPath = BrowseAnyFile("CPTED인증 파일 선택"));

            BrowseEco1Command = new RelayCommand(_ => Eco1Path = BrowseAnyFile("친환경 정보 파일 선택"));
            BrowseEco2Command = new RelayCommand(_ => Eco2Path = BrowseAnyFile("친환경 정보 파일 선택"));
            BrowseEco3Command = new RelayCommand(_ => Eco3Path = BrowseAnyFile("친환경 정보 파일 선택"));

            LoadAllCommand = new RelayCommand(_ =>
            {
                Autodesk.Revit.UI.TaskDialog.Show("외부 DB 갱신", "데모: 파일 로드(일괄) 버튼을 눌렀습니다.\n\n실제 구현에서는 각 파일을 파싱해 임시 메모리에 적재합니다.");
            });

            ExportAllCommand = new RelayCommand(_ =>
            {
                Autodesk.Revit.UI.TaskDialog.Show("외부 DB 갱신", "데모: 내보내기 버튼을 눌렀습니다.\n\n실제 구현에서는 현재 DB 상태를 파일로 저장합니다.");
            });

            FinishCommand = new RelayCommand(_ =>
            {
                // 여기서 실제 DB 갱신 로직 연결
                // TODO: Parse files and update external DB storage

                var summary =
                    $"BF: {BfPath}\n" +
                    $"GB: {GbPath}\n" +
                    $"CPTED: {CptedPath}\n\n" +
                    $"Eco1: {Eco1Path}\n" +
                    $"Eco2: {Eco2Path}\n" +
                    $"Eco3: {Eco3Path}\n";

                Autodesk.Revit.UI.TaskDialog.Show("외부 DB 갱신", "데모: 아래 파일 경로로 갱신을 실행합니다.\n\n" + summary);
                StatusText = "갱신 완료(데모).";
            });
        }

        private static string BrowseAnyFile(string title)
        {
            using var dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Title = title;
            dlg.Filter = "All Files (*.*)|*.*";
            dlg.Multiselect = false;

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