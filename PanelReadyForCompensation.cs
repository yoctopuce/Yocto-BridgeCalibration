
using System;
using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {
    private Label readyForCompensationMessage1 = null;
    private Label readyForCompensationMessage2 = null;
    private Label currentWeightValueLabel = null;
    private Label ClickOnNextLabel = null;


    public void ReadyForCompensationPanelResize()
    {
      readyForCompensationMessage1.Size = new System.Drawing.Size(600, 50);

      readyForCompensationMessage1.Location = new System.Drawing.Point((ClientSize.Width - readyForCompensationMessage1.Width) / 2, ClientSize.Height/2 -200);
      readyForCompensationMessage2.Size = new System.Drawing.Size(600, 150);

      readyForCompensationMessage2.Location = new System.Drawing.Point((ClientSize.Width - readyForCompensationMessage2.Width) / 2, readyForCompensationMessage1.Bottom+10);
     

      currentWeightValueLabel.Location = new System.Drawing.Point((ClientSize.Width - currentWeightValueLabel.Width) / 2, readyForCompensationMessage2.Bottom+50);
      ClickOnNextLabel.Size = new System.Drawing.Size(600, 100);
      ClickOnNextLabel.Location = new System.Drawing.Point((ClientSize.Width - ClickOnNextLabel.Width) / 2, currentWeightValueLabel.Bottom + 50);
      
    }

    public void ReadyForCompensationClearContents()
    {

      Controls.Remove(readyForCompensationMessage1);
      Controls.Remove(readyForCompensationMessage2);
      Controls.Remove(currentWeightValueLabel);
      Controls.Remove(ClickOnNextLabel);
    }

    public void newWeightValue2(YWeighScale source, string value)
    {
      currentWeightValueLabel.Text = (Double.Parse(value)).ToString("0.000") + ChoosedWeighScale.unit;
      double v = double.Parse(value);
      NextButton.Enabled = v < refWeight / 50;

    }

    public void ReadyForCompensationDrawPanel()
    {
      int tabIndex = 0;
      SuspendLayout();
      //
      // WarningResetMessage1
      // 
      if (readyForCompensationMessage1 == null) readyForCompensationMessage1 = new System.Windows.Forms.Label();
      readyForCompensationMessage1.Name = "readyForCompensationMessage1";
      readyForCompensationMessage1.AutoSize = false;
      readyForCompensationMessage1.TabIndex = tabIndex++;
      readyForCompensationMessage1.Text = "Your weighscale is now ready for gathering the data required to compute temperature compensation.";
      readyForCompensationMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      readyForCompensationMessage1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(readyForCompensationMessage1);

      if (readyForCompensationMessage2 == null) readyForCompensationMessage2 = new System.Windows.Forms.Label();
      readyForCompensationMessage2.Name = "readyForCompensationMessage2";
      readyForCompensationMessage2.AutoSize = false;
      readyForCompensationMessage2.TabIndex = tabIndex++;
      readyForCompensationMessage2.Text = "You can either click on Next and start monitoring data gathering it unplug your device, install it at its planned location and power it again, data gathering will start automatically.\n\nIt is recommended to let the data gathering process run for several days so several day / night temperature cycles can be recorded.";
      readyForCompensationMessage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      Controls.Add(readyForCompensationMessage2);



     

            //
            // curentWeightValueLabel
            // 
        if (currentWeightValueLabel == null) currentWeightValueLabel = new System.Windows.Forms.Label();
      currentWeightValueLabel.Name = "readyForCompensationMessage1";
      currentWeightValueLabel.AutoSize = true;
      currentWeightValueLabel.TabIndex = tabIndex++;
      currentWeightValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      currentWeightValueLabel.Text = ChoosedWeighScale.sensor.get_currentValue().ToString() + ChoosedWeighScale.sensor.get_unit();
     currentWeightValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(currentWeightValueLabel);
      //
      // ClickOnNextLabel
      //
      if (ClickOnNextLabel == null) ClickOnNextLabel = new System.Windows.Forms.Label();
      ClickOnNextLabel.Name = "ClickOnNextLabel";
      ClickOnNextLabel.AutoSize = false;
      ClickOnNextLabel.TabIndex = tabIndex++;
      ClickOnNextLabel.Text = "You can test the weighscale calibration, if you are not satisfied of the current calibration you can use the previous button to go back to the reference Weight step. If calibration seems good enough make sure the weightscale is free of any load and Click on Next, or disconnect the device, install it where it is supposed to be and power it again, data collection will start automatically.";
      ClickOnNextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

    //  ClickOnNextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(ClickOnNextLabel);

      ResumeLayout(false);
      NextButton.Visible = true;
      NextButton.Enabled = false;
      PrevButton.Visible = true;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;
      ReadyForCompensationPanelResize();

      ChoosedWeighScale.sensor.registerValueCallback(newWeightValue2);


      ChoosedWeighScale.datalogger.set_autoStart(YDataLogger.AUTOSTART_ON);
      ChoosedWeighScale.datalogger.forgetAllDataStreams();
      ChoosedWeighScale.genericSensor.set_logFrequency("6/m");
      ChoosedWeighScale.genericSensor.set_reportFrequency("6/m");
      ChoosedWeighScale.tsensor.set_logFrequency("6/m");
      ChoosedWeighScale.tsensor.set_reportFrequency("6/m");
      ChoosedWeighScale.saveconfig();


    }


    private PanelDesc.WizardSteps ReadyForCompensationNextClicked()
    {
      
      

      ChoosedWeighScale.datalogger.set_recording(YDataLogger.RECORDING_ON);
      return PanelDesc.WizardSteps.MONITORDATA;
    }

  }
}