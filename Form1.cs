using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{

   


    public partial class Form1 : Form
    {

        Timer YoctoTimer;
        int YoctoTimerCounter = 0;
        List<PanelDesc.WizardSteps> history;
        private PanelDesc.WizardSteps CurrentStep = PanelDesc.WizardSteps.WELCOME;
        public Form1()
        {        
            history = new List<PanelDesc.WizardSteps>();
            YoctoTimer = new Timer();
            YoctoTimer.Interval = 100;
            YAPI.RegisterDeviceArrivalCallback(DeviceArrival);
            YAPI.RegisterDeviceRemovalCallback(DeviceRemoval);
            YoctoTimer.Tick += YoctoTimer_Tick;
            YoctoTimer.Enabled = true;
            InitializeComponent();
            Controls.Remove(UILabel);
           
            new PanelDesc(PanelDesc.WizardSteps.WELCOME, WelcomePanelResize, WelcomeDrawPanel, WelcomePanelClearContents, WelcomePanelNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.CHOOSESENSOR, ChooseSensorPanelResize, ChooseSensorDrawPanel, ChooseSensorClearContents, ChooseSensorPanelNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.RESETWARNNG, WarningResetPanelResize, WarningResetDrawPanel, WarningResetClearContents, WarningResetPanelNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.CHOOSEEXCITATION, ChooseExcitationPanelResize, ChooseExcitationDrawPanel, ChooseExcitationClearContents, ChooseExcitation_NextClicked);
            new PanelDesc(PanelDesc.WizardSteps.CLEARLOAD, ClearLoadPanelResize, ClearLoadDrawPanel, ClearLoadClearContents, ClearLoadNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.REFWEIGHT, PutRefWeightPanelResize, PutRefWeightnDrawPanel, PutRefWeightClearContents, PutRefWeightnDrawPanel_NextClicked);
            new PanelDesc(PanelDesc.WizardSteps.REMOVEREFLOAD, RemoveRefLoadPanelResize, RemoveRefLoadDrawPanel, RemoveRefLoadClearContents, RemoveRefLoadNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.READY4TEMPCOMPENS, ReadyForCompensationPanelResize, ReadyForCompensationDrawPanel, ReadyForCompensationClearContents, ReadyForCompensationNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.DEVICEDISCONNECTED, DeviceDisconnectedPanelResize, DeviceDisconnectedDrawPanel, DeviceDisconnectedClearContents, DeviceDisconnectedNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.MONITORDATA, MonitorPanelResize, MonitorPanelDrawPanel, MonitorPanelClearContents, MonitorPanelNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.DATASOURCE, ChooseDataSourcePanelResize, ChooseDataSourceDrawPanel, ChooseDataSourcePanelClearContents,  ChooseDataSourcePanelNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.DATALOADING, LoadingDataPanelResize, LoadingDataDrawPanel, LoadingDataPanelClearContents, LoadingDataPanelNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.DOYOURTHING, computePanelResize, computeDrawPanel, computePanelClearContents, ComputePanelNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.CONFIRMCOMPENSATION, ConfirmCompensatioPanelResize, ConfirmCompensationDrawPanel, ConfirmCompensationPanelClearContents, ConfirmCompensationNextClicked);
            new PanelDesc(PanelDesc.WizardSteps.COMPENSATIONDONE, CompensationDonePanelResize, CompensationDoneDrawPanel, CompensationDonePanelClearContents, CompensationDoneNextClicked);

            DrawPrevNextButtons();
            WelcomeDrawPanel();
            GenericResize();
            if (constants.OpenLogWindowAtStartUp) LogManager.Show();


    }

    private void YoctoTimer_Tick(object sender, EventArgs e)
        {   string errmsg = "" ;
            if (YoctoTimerCounter == 0) YAPI.UpdateDeviceList(ref errmsg);
            YAPI.HandleEvents(ref errmsg);
            YoctoTimerCounter = (YoctoTimerCounter + 1) % 15;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            GenericResize();
            PanelDesc.Panel(CurrentStep).resize();
        }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      constants.SaveConfig();
    }
  }

  public class PanelDesc
    {
        private WizardSteps _step;
        private PanelActionResize _resize;
        private PanelActionClearContents _clearContents;
        private PanelActionDrawContents _drawContents;
        private PanelActionNextClicked _nextClicked;

        public delegate void PanelActionResize();
        public delegate void PanelActionClearContents();
        public delegate void PanelActionDrawContents();
        public delegate WizardSteps PanelActionNextClicked();

        static Dictionary<WizardSteps, PanelDesc> _panels = new Dictionary<WizardSteps, PanelDesc>();


        public enum WizardSteps
        {
            ERROR, WELCOME, CHOOSESENSOR, RESETWARNNG, CHOOSEEXCITATION, CLEARLOAD, REFWEIGHT,
            REMOVEREFLOAD, READY4TEMPCOMPENS, DEVICEDISCONNECTED, MONITORDATA, DATASOURCE,
            DATALOADING, DOYOURTHING, CONFIRMCOMPENSATION , COMPENSATIONDONE

    }


        public PanelDesc(WizardSteps step, PanelActionResize resize,
                         PanelActionDrawContents drawContents,
                         PanelActionClearContents clear,
                         PanelActionNextClicked nextClicked)
        {
            _step = step;
            _resize = resize;
            _clearContents = clear;
            _drawContents = drawContents;
            _nextClicked = nextClicked;
            _panels[step] = this;

        }

        public WizardSteps step { get { return _step; } }
        public PanelActionResize resize { get { return _resize; } }
        public PanelActionClearContents clearContents { get { return _clearContents; } }
        public PanelActionDrawContents drawContents { get { return _drawContents; } }
        public PanelActionNextClicked nextClicked { get { return _nextClicked; } }
        public static PanelDesc Panel(WizardSteps step) { return _panels[step]; }



    }

}
