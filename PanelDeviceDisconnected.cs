
using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
    partial class Form1
    {
        private Label DeviceDisconnectedMessage1 = null;
         
        public void DeviceDisconnectedPanelResize()
        {
            DeviceDisconnectedMessage1.Location = new System.Drawing.Point(20, ClientSize.Height /2);
            DeviceDisconnectedMessage1.Size = new System.Drawing.Size(ClientSize.Width-40, 20);      
        }

        public void DeviceDisconnectedClearContents()
        { 
            Controls.Remove(DeviceDisconnectedMessage1);         
        }
 

        public void DeviceDisconnectedDrawPanel()
        {
            int tabIndex = 0;
            SuspendLayout();
            //
            // WarningResetMessage1
            // 
            if (DeviceDisconnectedMessage1 == null) DeviceDisconnectedMessage1 = new System.Windows.Forms.Label();
            DeviceDisconnectedMessage1.Name = "DeviceDisconnectedMessage1";
            DeviceDisconnectedMessage1.AutoSize = false;
            DeviceDisconnectedMessage1.TabIndex = tabIndex++;
            DeviceDisconnectedMessage1.Text = "The device as been disconnected, click on Next to go back to the first step.";
            DeviceDisconnectedMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Controls.Add(DeviceDisconnectedMessage1);

            ResumeLayout(false);
            NextButton.Visible = true;
            NextButton.Enabled = true;
            PrevButton.Visible = true;
            PrevButton.Enabled = false ;

            PrevButton.TabIndex = tabIndex++;
            NextButton.TabIndex = tabIndex++;
            DeviceDisconnectedPanelResize();          
        }


        private PanelDesc.WizardSteps DeviceDisconnectedNextClicked()
        {
            ChoosedWeighScale = null;
            history.Clear();

            return PanelDesc.WizardSteps.WELCOME;
        }

    }
}