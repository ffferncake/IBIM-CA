using System.Windows;

namespace IBIMCA.UI
{
    public partial class BFCertificationOverviewWindow : Window
    {
        public BFCertificationOverviewWindow()
        {
            InitializeComponent();
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnGuideLink(object sender, RoutedEventArgs e)
        {
            ShowPending("설계인증 가이드 링크");
        }

        private void OnBimModelGuide(object sender, RoutedEventArgs e)
        {
            ShowPending("BIM 모델링 가이드");
        }

        private void OnEvaluationItems(object sender, RoutedEventArgs e)
        {
            ShowPending("주요 심사항목");
        }

        private void OnLinkedData(object sender, RoutedEventArgs e)
        {
            ShowPending("개발 연계 데이터");
        }

        private void OnSettings(object sender, RoutedEventArgs e)
        {
            ShowPending("설정");
        }

        private void OnOpenWebsite(object sender, RoutedEventArgs e)
        {
            ShowPending("공식 안내");
        }

        private void OnOpenPdf(object sender, RoutedEventArgs e)
        {
            ShowPending("평가기준 PDF");
        }

        private void OnChecklist(object sender, RoutedEventArgs e)
        {
            ShowPending("체크리스트");
        }

        private void OnFaq(object sender, RoutedEventArgs e)
        {
            ShowPending("FAQ");
        }

        private static void ShowPending(string featureName)
        {
            System.Windows.MessageBox.Show($"{featureName} 기능은 다음 단계에서 연결할 수 있습니다.", "IBIMCA");
        }
    }
}
