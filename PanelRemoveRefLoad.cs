
using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
    partial class Form1
    {
        private Label removeLoadMessage1 = null;
         
        public void RemoveRefLoadPanelResize()
        {
           
            removeLoadMessage1.Size = new System.Drawing.Size(600, 30);
            removeLoadMessage1.Location = new System.Drawing.Point((ClientSize.Width - removeLoadMessage1.Width)/2 , (ClientSize.Height - removeLoadMessage1.Height) / 2);
    }

        public void RemoveRefLoadClearContents()
        {
            Controls.Remove(removeLoadMessage1);
            ChoosedWeighScale.sensor.registerValueCallback(null);
                  
        }


        public void newWeightValue(YWeighScale source, string value)
        {
            double v = double.Parse(value);
            NextButton.Enabled = v < refWeight / 50;

        }

        public void RemoveRefLoadDrawPanel()
        {
            int tabIndex = 0;
            SuspendLayout();
            //
            // WarningResetMessage1
            // 
            if (removeLoadMessage1 == null) removeLoadMessage1 = new System.Windows.Forms.Label();
            removeLoadMessage1.Name = "removeLoadMessage1";
            removeLoadMessage1.AutoSize = false;
            removeLoadMessage1.TabIndex = tabIndex++;
            removeLoadMessage1.Text = "Remove the reference weight and click on Next";
      removeLoadMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      removeLoadMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Controls.Add(removeLoadMessage1);

            ResumeLayout(false);
            NextButton.Visible = true;
            NextButton.Enabled = false;
            PrevButton.Visible = true;
            PrevButton.TabIndex = tabIndex++;
            NextButton.TabIndex = tabIndex++;
            RemoveRefLoadPanelResize();

            ChoosedWeighScale.sensor.registerValueCallback(newWeightValue);
        }


        private PanelDesc.WizardSteps RemoveRefLoadNextClicked()
        {

            ChoosedWeighScale.sensor.tare();
            return PanelDesc.WizardSteps.READY4TEMPCOMPENS;
        }

    }
}