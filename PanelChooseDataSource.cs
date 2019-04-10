using System;
using System.Drawing;
using System.IO;

using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {
    private Label ChooseDataSourceMessage = null;
    private RadioButton FromDeviceRadioButton = null;
    private RadioButton FromFileRadioButton = null;
    private TextBox FilenameInput = null;
    private Button FilenameInputButton = null;
    private Label ChooseDataSourceTitleLabel = null;
    private Label  LoadDataTitleLabel = null;





    public void ChooseDataSourcePanelResize()
    {
      LoadDataTitleLabel.Size = new System.Drawing.Size(600, 75);
      LoadDataTitleLabel.Location = new System.Drawing.Point((ClientSize.Width - LoadDataTitleLabel.Size.Width) / 2, ClientSize.Height / 2 - 200);


      ChooseDataSourceMessage.Location = new System.Drawing.Point(0, LoadDataTitleLabel.Bottom+30);
      ChooseDataSourceMessage.Size = new System.Drawing.Size(ClientSize.Width, 30);
      FromDeviceRadioButton.Size = new System.Drawing.Size(200, 30);
      FromDeviceRadioButton.Location = new System.Drawing.Point((ClientSize.Width  / 2) -150, ChooseDataSourceMessage.Top + ChooseDataSourceMessage.Height + 10);
      // FromFileRadioButton.Size = FromDeviceRadioButton.Size;
      FromFileRadioButton.Location = new System.Drawing.Point(FromDeviceRadioButton.Left, FromDeviceRadioButton.Top + FromDeviceRadioButton.Height);
      FilenameInput.Width = 200;
      FilenameInput.Location = new System.Drawing.Point(FromFileRadioButton.Left + FromFileRadioButton.Width + 5, FromFileRadioButton.Top);
      FilenameInputButton.Width = 40;
      FilenameInputButton.Location = new System.Drawing.Point(FilenameInput.Left + FilenameInput.Width + 5, FilenameInput.Top);
    }

    public void ChooseDataSourcePanelClearContents()
    {
      Controls.Remove(ChooseDataSourceMessage);
      Controls.Remove(FromDeviceRadioButton);
      Controls.Remove(FromFileRadioButton);
      Controls.Remove(FilenameInput);
      Controls.Remove(FilenameInputButton);
      Controls.Remove(LoadDataTitleLabel);
      FilenameInputButton.Click -= FilenameInputButton_Click;


    }

    public void ChooseDataSourceDrawPanel()
    {
      int tabIndex = 0;
      SuspendLayout();



      if (LoadDataTitleLabel == null) LoadDataTitleLabel = new System.Windows.Forms.Label();

      LoadDataTitleLabel.Name = "LoadDataTitleLabel";
      LoadDataTitleLabel.AutoSize = false;
      LoadDataTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      LoadDataTitleLabel.TabIndex = tabIndex++;
      LoadDataTitleLabel.Text =  "Now the application needs to retrieve the accumulated data do compute the temperature compensation.  You can choose to retreive the data directly from the device datalogger or from a previously exported CSV file.  ";
      Controls.Add(LoadDataTitleLabel);


      //
      // ChooseDataSourceMessage
      // 
      if (ChooseDataSourceMessage == null) ChooseDataSourceMessage = new System.Windows.Forms.Label();
      ChooseDataSourceMessage.Name = "ChooseDataSourceMessage";
      ChooseDataSourceMessage.AutoSize = false;
      ChooseDataSourceMessage.TabIndex = tabIndex++;
      ChooseDataSourceMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      ChooseDataSourceMessage.Text = "Where are the accumulated data stored?";
      ChooseDataSourceMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(ChooseDataSourceMessage);

      // 
      // dataloger radio button
      // 
      if (FromDeviceRadioButton == null) FromDeviceRadioButton = new System.Windows.Forms.RadioButton();
      FromDeviceRadioButton.AutoSize = true;
      FromDeviceRadioButton.Name = "RadioBtUSB";
      FromDeviceRadioButton.TabIndex = tabIndex++;
      FromDeviceRadioButton.TabStop = true;
      FromDeviceRadioButton.Text = "In the device data logger";
      FromDeviceRadioButton.UseVisualStyleBackColor = true;    
      Controls.Add(FromDeviceRadioButton);

      // 
      // dataloger radio button
      // 
      if (FromFileRadioButton == null) FromFileRadioButton = new System.Windows.Forms.RadioButton();
      FromFileRadioButton.AutoSize = true;
      FromFileRadioButton.Name = "RadioBtUSB";
      FromFileRadioButton.TabIndex = tabIndex++;
      FromFileRadioButton.TabStop = true;
      FromFileRadioButton.Text = "In a CSV file: ";
      FromFileRadioButton.CheckedChanged += FromFileRadioButton_CheckedChanged;
      FromFileRadioButton.UseVisualStyleBackColor = true;     
      Controls.Add(FromFileRadioButton);

      //
      // file name
      //
      if (FilenameInput == null) FilenameInput = new System.Windows.Forms.TextBox();
      if (constants.MonoRunning) FilenameInput.BackColor = Color.White;
      FilenameInput.Name = "FilenameInput";
      FilenameInput.Size = new System.Drawing.Size(300, 20);
     // FilenameInput.Text = "C:\\tmp\\6days.csv";

      FilenameInput.TabIndex = tabIndex++;
      Controls.Add(FilenameInput);

      // filename button
      if (FilenameInputButton == null) FilenameInputButton = new Button();
      FilenameInputButton.Name = "FilenameInputButton";
      FilenameInputButton.Width = 30;
      FilenameInputButton.Text = "...";
      FilenameInputButton.TabIndex = tabIndex++;
      Controls.Add(FilenameInputButton);
      FilenameInputButton.Click += FilenameInputButton_Click;

      if ((!FromDeviceRadioButton.Checked) && (!FromFileRadioButton.Checked)) FromDeviceRadioButton.Checked = true;
      FromFileRadioButton_CheckedChanged(null, null);

      ChooseDataSourcePanelResize();

      ResumeLayout(false);
      NextButton.Visible = true;
      NextButton.Enabled = true;
      PrevButton.Visible = true;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;
     
    }

    private void FromFileRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      FilenameInputButton.Enabled = FromFileRadioButton.Checked;
      FilenameInput.Enabled = FromFileRadioButton.Checked;

    }

    private void FilenameInputButton_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        FilenameInput.Text = "";
        FilenameInput.AppendText(openFileDialog1.FileName);

      }
    }

    private PanelDesc.WizardSteps ChooseDataSourcePanelNextClicked()
    {
      if (FromFileRadioButton.Checked)
      {
        if (FilenameInput.Text == "")
        {
          MessageBox.Show("Enter a valid filename", "Invalid filename", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return PanelDesc.WizardSteps.ERROR;
        }
        if (!File.Exists(FilenameInput.Text))
        {
          MessageBox.Show("File does not exists", "Invalid filename", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return PanelDesc.WizardSteps.ERROR;
        }
      }
      return PanelDesc.WizardSteps.DATALOADING;
    }


  }
}