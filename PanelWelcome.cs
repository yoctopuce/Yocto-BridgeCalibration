using System;

using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
    partial class Form1
    {
        private Label TitleLabel=null;
        private Label MainMenuTitle = null;
        private RadioButton RadioBtAccumulate = null;
        private RadioButton RadioBtCalibrate = null;
        private RadioButton RadioBtMonitor = null;

        public void WelcomePanelResize()
        {
            TitleLabel.Size = new System.Drawing.Size(600, 75);
            TitleLabel.Location = new System.Drawing.Point((ClientSize.Width- TitleLabel.Size.Width)/2, ClientSize.Height/2 -200);
           
            MainMenuTitle.Size = new System.Drawing.Size(ClientSize.Width, 23);
            MainMenuTitle.Location = new System.Drawing.Point(0, ClientSize.Height / 2 -75 );

            int w = RadioBtAccumulate.Width;
            if (RadioBtMonitor.Width > w) w = RadioBtMonitor.Width;
            if (RadioBtCalibrate.Width > w) w = RadioBtCalibrate.Width;


            RadioBtAccumulate.Location = new System.Drawing.Point((ClientSize.Width -w)/2, MainMenuTitle.Top + 75);
            RadioBtMonitor.Location = new System.Drawing.Point((ClientSize.Width - w) / 2, RadioBtAccumulate.Top + 30);
            RadioBtCalibrate.Location = new System.Drawing.Point((ClientSize.Width - w) / 2, RadioBtMonitor.Top + 30);

            NextButton.Visible = true;
            PrevButton.Visible = false;
       }

        public void WelcomePanelClearContents()
        {
            Controls.Remove(TitleLabel);
            Controls.Remove(MainMenuTitle);
            Controls.Remove(RadioBtAccumulate);
            Controls.Remove(RadioBtMonitor);
            Controls.Remove(RadioBtCalibrate);
        }

       

        public void WelcomeDrawPanel()
        {
            int tabIndex = 0;
            SuspendLayout();
            // 
            // TitleLabel
            // 
            if (TitleLabel == null) TitleLabel = new System.Windows.Forms.Label();          
            TitleLabel.Location = new System.Drawing.Point(0, 0);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.AutoSize = false;
            TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TitleLabel.TabIndex = tabIndex++;
            TitleLabel.Text = "This little Application will guide you into the steps required to make the best of your Yocto-Bridge and your load cell by calibrating its temperature compensation.";
            Controls.Add(TitleLabel);
            // 
            // MainMenuTitle
            // 
            if (MainMenuTitle == null)  MainMenuTitle = new System.Windows.Forms.Label();          
            MainMenuTitle.Name = "MainMenuTitle";
            MainMenuTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            MainMenuTitle.TabIndex = tabIndex++;
            MainMenuTitle.Text = "What would you like to do ?";
            MainMenuTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Controls.Add(MainMenuTitle);

            // 
            // RadioBtAccumulate
            // 

            if (RadioBtAccumulate == null) RadioBtAccumulate = new System.Windows.Forms.RadioButton();
            RadioBtAccumulate.AutoSize = true;
            RadioBtAccumulate.Name = "RadioBtAccumulate";
            RadioBtAccumulate.Size = new System.Drawing.Size(400, 17);
            RadioBtAccumulate.TabIndex = tabIndex++;
            RadioBtAccumulate.TabStop = true;
            RadioBtAccumulate.Text = "Calibrate the sensor and accumulate data for temperature compensation";
            RadioBtAccumulate.UseVisualStyleBackColor = true;
            RadioBtAccumulate.CheckedChanged += MainMenu_CheckedChanged;
            Controls.Add(RadioBtAccumulate);
            // 
            // RadioBtCalibrate
            // 
            if (RadioBtCalibrate == null) RadioBtCalibrate = new System.Windows.Forms.RadioButton();
            RadioBtCalibrate.AutoSize = true;
            RadioBtCalibrate.Name = "RadioBtCalibrate";
            RadioBtCalibrate.Size = new System.Drawing.Size(400, 17);
            RadioBtCalibrate.TabIndex = tabIndex++;
            RadioBtCalibrate.TabStop = true;
            RadioBtCalibrate.Text = "Compute  temperature compensation from accumulated data";
            RadioBtCalibrate.UseVisualStyleBackColor = true;
            RadioBtCalibrate.CheckedChanged += MainMenu_CheckedChanged;
            Controls.Add(RadioBtCalibrate);
            // 
            // RadioBtMonitor
            // 
            if (RadioBtMonitor == null)  RadioBtMonitor = new System.Windows.Forms.RadioButton();
            RadioBtMonitor.AutoSize = true;
            RadioBtMonitor.Name = "RadioBtMonitor";
            RadioBtMonitor.Size = new System.Drawing.Size(400, 17);
            RadioBtMonitor.TabIndex = tabIndex++;
            RadioBtMonitor.TabStop = true;
            RadioBtMonitor.Text = "Monitor data accumulation";
            RadioBtMonitor.UseVisualStyleBackColor = true;
            RadioBtMonitor.CheckedChanged += MainMenu_CheckedChanged;
            Controls.Add(RadioBtMonitor);



           


            PrevButton.TabIndex = tabIndex++;
            NextButton.TabIndex = tabIndex++;

            WelcomePanelResize();
            ResumeLayout(false);
        }

    private void MainMenu_CheckedChanged(object sender, EventArgs e)
    {
      NextButton.Enabled = true;
    }

    private PanelDesc.WizardSteps WelcomePanelNextClicked()
         {
            return PanelDesc.WizardSteps.CHOOSESENSOR;
         }


    }
}
