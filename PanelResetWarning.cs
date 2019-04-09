using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {

    private Label WarningResetMessage1 = null;
    private Label WarningResetMessage2 = null;
    private CheckBox WarningResetconfirm = null;


    public void WarningResetPanelResize()
    {
      WarningResetMessage1.Size = new System.Drawing.Size(600, 25);
      WarningResetMessage1.Location = new System.Drawing.Point((ClientSize.Width- WarningResetMessage1.Width) /2, (ClientSize.Height / 2)- WarningResetMessage1.Height -50);

      WarningResetMessage2.Size = new System.Drawing.Size(600, 100);
      WarningResetMessage2.Location = new System.Drawing.Point((ClientSize.Width - WarningResetMessage2.Width) / 2, WarningResetMessage1.Bottom  + 60);


      WarningResetconfirm.Location = new System.Drawing.Point((ClientSize.Width - WarningResetconfirm.Size.Width) / 2, WarningResetMessage2.Bottom+30);
     
    }

    public void WarningResetClearContents()
    {
      Controls.Remove(WarningResetMessage1);
      Controls.Remove(WarningResetMessage2);
      Controls.Remove(WarningResetconfirm);
    }

    public void WarningResetDrawPanel()
    {
      int tabIndex = 0;
      SuspendLayout();
      //
      // WarningResetMessage1
      // 
      if (WarningResetMessage1 == null) WarningResetMessage1 = new System.Windows.Forms.Label();
      WarningResetMessage1.Name = "WarningResetMessage1";
      WarningResetMessage1.AutoSize = false;
      WarningResetMessage1.TabIndex = tabIndex++;
      WarningResetMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      WarningResetMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      WarningResetMessage1.Text = "You chose to calibrate " + ChoosedWeighScale.ToString();
      Controls.Add(WarningResetMessage1);

      if (WarningResetMessage2 == null) WarningResetMessage2 = new System.Windows.Forms.Label();
      WarningResetMessage2.Name = "WarningResetMessage2";
      WarningResetMessage2.AutoSize = false;
      WarningResetMessage2.TabIndex = tabIndex++;
      
      WarningResetMessage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      WarningResetMessage2.Text = "Be aware that the calibration process will erase all calibration settings and datalogger contents from this sensor. Please confirm  that you understand, then Click on Next to Continue";
      Controls.Add(WarningResetMessage2);

      if (WarningResetconfirm == null) WarningResetconfirm = new CheckBox();
      WarningResetconfirm.Name = "WarningResetconfirm";
      WarningResetconfirm.AutoSize = true;
      WarningResetconfirm.TabIndex = tabIndex++;
      WarningResetconfirm.Text = "Ok, I understand.";
      WarningResetconfirm.Checked  = false;
      WarningResetconfirm.CheckedChanged += WarningResetconfirm_CheckedChanged;
      Controls.Add(WarningResetconfirm);

      ResumeLayout(false);
      NextButton.Visible = true;
      NextButton.Enabled = false;
      PrevButton.Visible = true;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;
      WarningResetPanelResize();

    }

    private void WarningResetconfirm_CheckedChanged(object sender, EventArgs e)
    {
      NextButton.Enabled = WarningResetconfirm.Checked;
    }

    private PanelDesc.WizardSteps WarningResetPanelNextClicked()
    {
      ChoosedWeighScale = (CustomSensor)SensorChooser.SelectedItem;
      ChoosedWeighScale.sensor.set_offsetAvgCompensationTable(new List<Double>(), new List<Double>());
      ChoosedWeighScale.sensor.set_offsetChgCompensationTable(new List<Double>(), new List<Double>());
      ChoosedWeighScale.sensor.set_spanAvgCompensationTable(new List<Double>(), new List<Double>());
      ChoosedWeighScale.sensor.set_spanChgCompensationTable(new List<Double>(), new List<Double>());
      ChoosedWeighScale.sensor.set_tempAvgAdaptRatio(0.0002);
      ChoosedWeighScale.sensor.set_tempChgAdaptRatio(0.0006);
      ChoosedWeighScale.sensor.set_zeroTracking(0);
      ChoosedWeighScale.sensor.stopDataLogger();

      return PanelDesc.WizardSteps.CHOOSEEXCITATION;
    }

  }
}