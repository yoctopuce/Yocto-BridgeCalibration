
using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
    partial class Form1
    {

        private Label ClearLoadMessage1 = null;
        
      
   
        public void ClearLoadPanelResize()
    {
      ClearLoadMessage1.Size = new System.Drawing.Size(600, 80);

      ClearLoadMessage1.Location = new System.Drawing.Point((ClientSize.Width- ClearLoadMessage1.Width)/2, (ClientSize.Height - ClearLoadMessage1.Height) / 2);
           
        }

        public void ClearLoadClearContents()
        {
            Controls.Remove(ClearLoadMessage1);
           
        }

        public void ClearLoadDrawPanel()
        {
            int tabIndex = 0;
            SuspendLayout();
            //
            // WarningResetMessage1
            // 
            if (ClearLoadMessage1 == null) ClearLoadMessage1 = new System.Windows.Forms.Label();
            ClearLoadMessage1.Name = "ClearLoadMessage1";
            ClearLoadMessage1.AutoSize = false;
            ClearLoadMessage1.TabIndex = tabIndex++;
            ClearLoadMessage1.Text = "Make sure the weightscale is installed on a horizontal and stable surface, remove any load from it, and click on Next";
            ClearLoadMessage1.Font= new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            ClearLoadMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Controls.Add(ClearLoadMessage1);

           

            ResumeLayout(false);
            NextButton.Visible = true;
            NextButton.Enabled = true;
            PrevButton.Visible = true;
            PrevButton.TabIndex = tabIndex++;
            NextButton.TabIndex = tabIndex++;
            ClearLoadPanelResize();

        }


        private PanelDesc.WizardSteps ClearLoadNextClicked()
        {
            ChoosedWeighScale.sensor.tare();
            return PanelDesc.WizardSteps.REFWEIGHT;
        }

    }
}