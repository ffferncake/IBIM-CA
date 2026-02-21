// Revit API
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

// IBIMCA
using IBIMCA.Extensions;
using gAva = IBIMCA.Availability.AvailabilityNames;
using IBIMCA.Utilities; // IconLoader

namespace IBIMCA
{
    public class Application : IExternalApplication
    {
        private static UIControlledApplication _uiCtlApp;

        private static PushButton AddPulldownItem (
            PulldownButton parent,
            string internalName,
            string text,
            Type commandType,
            string icon16,
            string icon32
         )

        {
            var pbd = new PushButtonData(
                internalName,
                text,
                Globals.AssemblyPath,
                commandType.FullName
            );

            var btn = parent.AddPushButton(pbd) as PushButton;

            btn.Image = IconLoader.LoadPng("Icons16", icon16);
            btn.LargeImage = IconLoader.LoadPng("Icons32", icon32);

            return btn;
        }

        public Result OnStartup(UIControlledApplication uiCtlApp)
        {
            _uiCtlApp = uiCtlApp;

            try { _uiCtlApp.Idling += OnIdling; }
            catch { Globals.UiApp = null; }

            Globals.RegisterVariables(uiCtlApp);
            Globals.RegisterTooltips($"{Globals.AddinName}.Resources.Files.Tooltips");

            Warden.Register(uiCtlApp);
            SyncTimer.Register(uiCtlApp.ControlledApplication);

            // TAB
            uiCtlApp.Ext_AddRibbonTab(Globals.AddinName);

            // =========================
            // Panel 1: 1. 프로젝트 설정
            // =========================
            var p1 = uiCtlApp.Ext_AddRibbonPanelToTab(Globals.AddinName, "1. 프로젝트 설정");

            // Instead of AddButton(...) for CertificationToolset:
            var pdData = new PulldownButtonData("CertificationToolset", "설계인증\n평가 로드");
            var pd = p1.AddItem(pdData) as PulldownButton;

            // set icon for the dropdown button itself
            pd.Image = IconLoader.LoadPng("Icons16", "CertificationToolset16.png");
            pd.LargeImage = IconLoader.LoadPng("Icons32", "CertificationToolset32.png");

            // add 3 items inside dropdown
            NewMethod(pd);

            AddPulldownItem(pd,
                internalName: "LoadGSEED",
                text: "녹색건축 인증제도\n(G-SEED)",
                commandType: typeof(IBIMCA.Commands.General.Cmd_LoadCertification_GSEED),
                icon16: "GSEED16.png",
                icon32: "GSEED32.png"
            );

            AddPulldownItem(pd,
                internalName: "LoadCPTED",
                text: "범죄예방환경설계\n(CPTED)",
                commandType: typeof(IBIMCA.Commands.General.Cmd_LoadCertification_CPTED),
                icon16: "CPTED16.png",
                icon32: "CPTED32.png"
            );

            var b2 = AddButton(p1,
                internalName: "StandardEvaluation",
                text: "정성적 평가\n설정",
                commandType: typeof(IBIMCA.Commands.General.Cmd_StandardEvaluationSettings),
                availability: gAva.Document,
                iconBaseName: "StandardEvaluation");

            // =========================
            // Panel 2: 2. 자동화 평가
            // =========================
            var p2 = uiCtlApp.Ext_AddRibbonPanelToTab(Globals.AddinName, "2. 자동화 평가");

            var b3 = AddButton(p2,
                internalName: "OpenEvaluationPanel",
                text: "평가 패널\n열기",
                commandType: typeof(IBIMCA.Commands.Tools.Cmd_OpenEvaluationPanel),
                availability: gAva.Document,
                iconBaseName: "OpenEvaluationPanel");

            var b4 = AddButton(p2,
                internalName: "RunFullEvaluation",
                text: "전체 평가\n실행",
                commandType: typeof(IBIMCA.Commands.Tools.Cmd_RunFullEvaluation),
                availability: gAva.Document,
                iconBaseName: "RunFullEvaluation");

            // =========================
            // Panel 3: 3. 지능형 지원
            // =========================
            var p3 = uiCtlApp.Ext_AddRibbonPanelToTab(Globals.AddinName, "3. 지능형 지원");

            var b5 = AddButton(p3,
                internalName: "AIAssistant",
                text: "AI 챗봇\n어시스턴트",
                commandType: typeof(IBIMCA.Commands.Tools.Cmd_AIAssistant),
                availability: gAva.Document,
                iconBaseName: "AIAssistant");

            var b6 = AddButton(p3,
                internalName: "UpdateExternalDB",
                text: "외부 DB\n자동 갱신",
                commandType: typeof(IBIMCA.Commands.Tools.Cmd_UpdateExternalDB),
                availability: gAva.Document,
                iconBaseName: "UpdateExternalDB");

            var b7 = AddButton(p3,
                internalName: "AutoDesignCorrection",
                text: "설계 대안\n자동수정",
                commandType: typeof(IBIMCA.Commands.Tools.Cmd_AutoDesignCorrection),
                availability: gAva.Document,
                iconBaseName: "AutoDesignCorrection");

            // =========================
            // Panel 4: 4. 시각화 및 리포팅
            // =========================
            var p4 = uiCtlApp.Ext_AddRibbonPanelToTab(Globals.AddinName, "4. 시각화 및 리포팅");

            var b8 = AddButton(p4,
                internalName: "ResultColorVisualization",
                text: "결과 색상\n가시화",
                commandType: typeof(IBIMCA.Commands.Tools.Cmd_ResultColorVisualization),
                availability: gAva.Document,
                iconBaseName: "ResultColorVisualization");

            var b9 = AddButton(p4,
                internalName: "ExportExcelReport",
                text: "엑셀 보고서\n출력",
                commandType: typeof(IBIMCA.Commands.Tools.Cmd_ExportExcelReport),
                availability: gAva.Document,
                iconBaseName: "ExportExcelReport");

            return Result.Succeeded;
        }

        private static void NewMethod(PulldownButton pd)
        {
            AddPulldownItem(pd,
                            internalName: "LoadBF",
                            text: "장애물 없는 생활환경 인증제도\n(BF)",
                            commandType: typeof(IBIMCA.Commands.General.Cmd_LoadCertification_BF),
                            icon16: "BF16.png",
                            icon32: "BF32.png"
                        );
        }

        private static PushButton AddButton(
            RibbonPanel panel,
            string internalName,
            string text,
            System.Type commandType,
            string availability,
            string iconBaseName
        )
        {
            // PushButtonData 생성
            var pbd = new PushButtonData(
                internalName,
                text,
                Globals.AssemblyPath,               // Globals에 assembly path 있어야 함 (기존 코드에 보통 있음)
                commandType.FullName
            );

            var btn = panel.AddItem(pbd) as PushButton;

            // Availability class 설정 (너희 Ext 메서드로 하고 있으면 그 방식에 맞추면 됨)
            // 여기서는 단순 예시. 기존 Ext_AddPushButton 방식 유지하고 싶으면 그걸로 바꿔도 돼.
            btn.AvailabilityClassName = availability;

            // 아이콘 적용
            btn.Image = IconLoader.LoadPng("Icons16", $"{iconBaseName}16.png");
            btn.LargeImage = IconLoader.LoadPng("Icons32", $"{iconBaseName}32.png");

            return btn;
        }

        public Result OnShutdown(UIControlledApplication uiCtlApp)
        {
            ColouredTabs.DeRegister(uiCtlApp.ControlledApplication, Globals.UiApp);
            Warden.DeRegister(uiCtlApp);
            SyncTimer.DeRegister(uiCtlApp.ControlledApplication);
            return Result.Succeeded;
        }

        private void OnIdling(object sender, IdlingEventArgs e)
        {
            _uiCtlApp.Idling -= OnIdling;

            if (sender is UIApplication uiApp)
            {
                Globals.UiApp = uiApp;
                Globals.UsernameRevit = uiApp.Application.Username;
            }
        }
    }
}