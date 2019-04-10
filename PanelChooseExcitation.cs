
using System.Drawing;
using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {
    private Label CellParamMessage = null;
    private Label ChooseExcitationMessage = null;
    private ComboBox ExcitationChooser = null;
    private Label ChooseUnitMessage = null;
    private TextBox ChooseUnitInput = null;
    private Label TemperatureSensorMessage = null;
    private ComboBox TemperatureSensorChooser = null;


    public void ChooseExcitationPanelResize()
    {
      CellParamMessage.Size = new System.Drawing.Size(ClientSize.Width, 30);
      CellParamMessage.Location = new System.Drawing.Point(0, ClientSize.Height / 2 - 200);

      ChooseExcitationMessage.Location = new System.Drawing.Point(0, CellParamMessage.Top+50);
      ChooseExcitationMessage.Size = new System.Drawing.Size(ClientSize.Width, 30);
      ExcitationChooser.Location = new System.Drawing.Point((ClientSize.Width - ExcitationChooser.Width) / 2, ChooseExcitationMessage.Bottom + 20);
      ChooseUnitMessage.Location = new System.Drawing.Point(0, ExcitationChooser.Bottom+30);
      ChooseUnitMessage.Size = new System.Drawing.Size(ClientSize.Width, 30);
      ChooseUnitInput.Location = new System.Drawing.Point(ClientSize.Width / 2, ChooseUnitMessage.Bottom + 20);
      ChooseUnitInput.Size = new System.Drawing.Size(50, 30);

      TemperatureSensorMessage.Location = new System.Drawing.Point(0, ChooseUnitInput.Bottom+30);
      TemperatureSensorMessage.Size = new System.Drawing.Size(ClientSize.Width, 30);
      TemperatureSensorChooser.Location = new System.Drawing.Point((ClientSize.Width - TemperatureSensorChooser.Width) / 2, TemperatureSensorMessage.Bottom + 20);




    }

    public void ChooseExcitationClearContents()
    {
      Controls.Remove(ChooseExcitationMessage);
      Controls.Remove(ExcitationChooser);
      Controls.Remove(ChooseUnitMessage);
      Controls.Remove(ChooseUnitInput);
      Controls.Remove(TemperatureSensorMessage);
      Controls.Remove(TemperatureSensorChooser);
      Controls.Remove(CellParamMessage);
    }

    public void ChooseExcitationDrawPanel()
    {
      int tabIndex = 0;
      SuspendLayout();


      //
      // CellParamMessage
      // 
      if (CellParamMessage == null) CellParamMessage = new System.Windows.Forms.Label();
      CellParamMessage.Name = "CellParamMessage";
      CellParamMessage.AutoSize = false;
      CellParamMessage.TabIndex = tabIndex++;
      CellParamMessage.Text = "Enter the load cell parameters";
      CellParamMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      CellParamMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(CellParamMessage);

      //
      // WarningResetMessage1
      // 
      if (ChooseExcitationMessage == null) ChooseExcitationMessage = new System.Windows.Forms.Label();
      ChooseExcitationMessage.Name = "ChooseExcitationMessage";
      ChooseExcitationMessage.AutoSize = false;
      ChooseExcitationMessage.TabIndex = tabIndex++;
      ChooseExcitationMessage.Text = "Choose the sensor excitation type, use AC excitation if you have no idea.";
      ChooseExcitationMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      ChooseExcitationMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(ChooseExcitationMessage);

      //
      // ExcitationChooser
      //
      if (ExcitationChooser == null)
      {
        ExcitationChooser = new System.Windows.Forms.ComboBox();
        ExcitationChooser.Items.Add("AC excitation");
        ExcitationChooser.Items.Add("DC excitation");
      }
      ExcitationChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      ExcitationChooser.FormattingEnabled = true;
      ExcitationChooser.Name = "ExcitationChooser";
      ExcitationChooser.Size = new System.Drawing.Size(200, 21);
      ExcitationChooser.TabIndex = tabIndex++;
      ExcitationChooser.Visible = SensorChooser.Items.Count > 0;
      if (ExcitationChooser.SelectedIndex < 0) ExcitationChooser.SelectedIndex = 0;
      Controls.Add(ExcitationChooser);

      //
      // ChooseUnitMessage
      // 
      if (ChooseUnitMessage == null) ChooseUnitMessage = new System.Windows.Forms.Label();
      ChooseUnitMessage.Name = "ChooseUnitMessage";
      ChooseUnitMessage.AutoSize = false;
      ChooseUnitMessage.TabIndex = tabIndex++;
      ChooseUnitMessage.Text = "Enter the unit your weighscale is working on";
      ChooseUnitMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      ChooseUnitMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      Controls.Add(ChooseUnitMessage);

      //
      // ChooseUnitInput
      //
      if (ChooseUnitInput == null) ChooseUnitInput = new System.Windows.Forms.TextBox();
      if (constants.MonoRunning) ChooseUnitInput.BackColor = Color.White;
      ChooseUnitInput.Name = "ChooseUnitInput";
      ChooseUnitInput.Size = new System.Drawing.Size(100, 20);
      ChooseUnitInput.Text = CurrentConnectionAddr;
      ChooseUnitInput.TabIndex = tabIndex++;
      if (ChooseUnitInput.Text == "") ChooseUnitInput.Text = ChoosedWeighScale.unit;
  
      Controls.Add(ChooseUnitInput);




      //
      // TemperatureSensorMessage
      // 
      if (TemperatureSensorMessage == null) TemperatureSensorMessage = new System.Windows.Forms.Label();
      TemperatureSensorMessage.Name = "ChooseExcitationMessage";
      TemperatureSensorMessage.AutoSize = false;
      TemperatureSensorMessage.TabIndex = tabIndex++;
      TemperatureSensorMessage.Text = "Choose which temperature sensor you want to use.";
      TemperatureSensorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      TemperatureSensorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(TemperatureSensorMessage);

      //
      // TemperatureSensorChooser
      //
      if (TemperatureSensorChooser == null)
      {
        TemperatureSensorChooser = new System.Windows.Forms.ComboBox();
        TemperatureSensorChooser.Items.Add("Internal sensor");
        TemperatureSensorChooser.Items.Add("External NTC 10KΩ β=3380 @ 25°C ");
        TemperatureSensorChooser.Items.Add("External, other sensor ");

      }


      TemperatureSensorChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      TemperatureSensorChooser.FormattingEnabled = true;
      TemperatureSensorChooser.Name = "TemperatureSensorChooser";
      TemperatureSensorChooser.Size = new System.Drawing.Size(200, 21);
      TemperatureSensorChooser.TabIndex = tabIndex++;
      TemperatureSensorChooser.Visible = SensorChooser.Items.Count > 0;
      if (TemperatureSensorChooser.SelectedIndex<0)TemperatureSensorChooser.SelectedIndex = ChoosedWeighScale.TempSensorType;
      Controls.Add(TemperatureSensorChooser);

      ChooseExcitationPanelResize();

      ResumeLayout(false);
      NextButton.Visible = true;
      NextButton.Enabled = true;
      PrevButton.Visible = true;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;






    }

    private PanelDesc.WizardSteps ChooseExcitation_NextClicked()
    {
      ChoosedWeighScale.sensor.set_excitation(ExcitationChooser.SelectedIndex == 0 ? YWeighScale.EXCITATION_AC : YWeighScale.EXCITATION_DC);
      ChoosedWeighScale.sensor.set_unit(ChooseUnitInput.Text);
      switch (TemperatureSensorChooser.SelectedIndex)
      {
        case 0: ChoosedWeighScale.tsensor.set_sensorType(YTemperature.SENSORTYPE_RES_INTERNAL); break;
        case 1:
          ChoosedWeighScale.tsensor.set_sensorType(YTemperature.SENSORTYPE_RES_NTC);
          ChoosedWeighScale.tsensor.set_ntcParameters(10000, 3385);
          break;
        case 2: break; // custom sensor, do nothing
      }


      return PanelDesc.WizardSteps.CLEARLOAD;
    }


  }
}