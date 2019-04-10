using System;
using System.Drawing;
using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
    partial class Form1
    {

        private Label PutRefWeightMessage = null;
        private Label CalibrationMessage = null;
    private Label   RefWeightlabel = null;
        private TextBox RefWeightInput = null;
        private Label   RefWeightUnit = null;
        private Label   MaxWeightlabel = null;
        private TextBox MaxWeightInput = null;
        private Label   MaxWeightUnit = null;

        private double refWeight=0;
        private double maxWeight=0;



        public void PutRefWeightPanelResize()
        {

         CalibrationMessage.Size = new System.Drawing.Size(600, 40);
      CalibrationMessage.Location = new System.Drawing.Point((ClientSize.Width - CalibrationMessage.Width) / 2, ClientSize.Height / 2 - 200);



          PutRefWeightMessage.Size = new System.Drawing.Size(600, 80);
          PutRefWeightMessage.Location = new System.Drawing.Point((ClientSize.Width - PutRefWeightMessage.Width)/2, CalibrationMessage.Bottom+50);
           

            
            int w1 = RefWeightlabel.Width;
           
            if (MaxWeightlabel.Width > w1) w1 = MaxWeightlabel.Width;
            int tw = w1 + RefWeightInput.Width + RefWeightUnit.Width;


            RefWeightlabel.Location = new System.Drawing.Point((ClientSize.Width - tw)/2, PutRefWeightMessage.Bottom +50);
            RefWeightInput.Location = new System.Drawing.Point(RefWeightlabel.Left+5+ RefWeightInput.Width, RefWeightlabel.Top );
            RefWeightUnit.Location = new System.Drawing.Point(RefWeightInput.Left + 5 + RefWeightInput.Width, RefWeightlabel.Top);

            MaxWeightlabel.Location = new System.Drawing.Point((ClientSize.Width - tw) / 2, RefWeightlabel.Bottom + 30);
            MaxWeightInput.Location = new System.Drawing.Point(MaxWeightlabel.Left + 5 + MaxWeightInput.Width, MaxWeightlabel.Top);
            MaxWeightUnit.Location = new System.Drawing.Point(MaxWeightInput.Left + 5 + MaxWeightInput.Width, MaxWeightlabel.Top);


        }

        public void PutRefWeightClearContents()
        {
            Controls.Remove(PutRefWeightMessage);
            Controls.Remove(RefWeightlabel);
            Controls.Remove(RefWeightInput);
            Controls.Remove(RefWeightUnit);
            Controls.Remove(MaxWeightlabel);
            Controls.Remove(MaxWeightInput);
            Controls.Remove(MaxWeightUnit);
      Controls.Remove(CalibrationMessage);

        }

        public void PutRefWeightnDrawPanel()
        {
            int tabIndex = 0;
            SuspendLayout();

      //
      // PutRefWeightMessage
      // 
      if (CalibrationMessage == null) CalibrationMessage = new System.Windows.Forms.Label();
      CalibrationMessage.Name = "CalibrationMessage";
      CalibrationMessage.AutoSize = false;
      CalibrationMessage.TabIndex = tabIndex++;
      CalibrationMessage.Text = "Calibration";
      CalibrationMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      CalibrationMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(CalibrationMessage);

      //
      // PutRefWeightMessage
      // 
      if (PutRefWeightMessage == null) PutRefWeightMessage = new System.Windows.Forms.Label();
            PutRefWeightMessage.Name = "PutRefWeightMessage";
            PutRefWeightMessage.AutoSize = false;
            PutRefWeightMessage.TabIndex = tabIndex++;
            PutRefWeightMessage.Text = "Put a reference weight on the weighScale, enter its weight below as well as the maximum weight allowed by the load cell then click on next. \n\nThe heavier the reference weight, the better, but stay within the loadcell capabilities.";
            PutRefWeightMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Controls.Add(PutRefWeightMessage);


            // 
            // RefWeightlabel
            // 
            if (RefWeightlabel == null) RefWeightlabel = new System.Windows.Forms.Label();
            RefWeightlabel.Name = "PutRefWeightMessage";
            RefWeightlabel.AutoSize = true;
            RefWeightlabel.TabIndex = tabIndex++;
            RefWeightlabel.Text = "Reference weight:";
            Controls.Add(RefWeightlabel);

            //
            // RefWeightInput
            //
            if (RefWeightInput == null)
            {
                RefWeightInput = new System.Windows.Forms.TextBox();
                if (constants.MonoRunning) RefWeightInput.BackColor = Color.White;
                RefWeightInput.Text = "";
            }
            RefWeightInput.Name = "RefWeightInput";
            RefWeightInput.TextChanged+= RefWeightInput_textchanged;
            RefWeightInput.Size = new System.Drawing.Size(100, 20);
            RefWeightInput.TabIndex = tabIndex++;
            Controls.Add(RefWeightInput);
            RefWeightInput.Focus();

            //
            // RefWeightUnit
            // 
            if (RefWeightUnit == null) RefWeightUnit = new System.Windows.Forms.Label();
            RefWeightUnit.Name = "RefWeightUnit";
            RefWeightUnit.AutoSize = false;
            RefWeightUnit.TabIndex = tabIndex++;
            RefWeightUnit.Text = ChoosedWeighScale.sensor.get_unit(); 
            Controls.Add(RefWeightUnit);

            // 
            // MaxWeightlabel
            // 
            if (MaxWeightlabel == null) MaxWeightlabel = new System.Windows.Forms.Label();
            MaxWeightlabel.Name = "PutRefWeightMessage";
            MaxWeightlabel.AutoSize = true;
            MaxWeightlabel.TabIndex = tabIndex++;
            MaxWeightlabel.Text = "Maximum weight:";
            Controls.Add(MaxWeightlabel);

            //
            // MaxWeightInput
            //
            if (MaxWeightInput == null)
            {
                MaxWeightInput = new System.Windows.Forms.TextBox();
                if (constants.MonoRunning) MaxWeightInput.BackColor = Color.White;
                MaxWeightInput.Text = "";
            }
            MaxWeightInput.Name = "RefWeightInput";
            MaxWeightInput.TextChanged += RefWeightInput_textchanged;
            MaxWeightInput.Size = new System.Drawing.Size(100, 20);
            MaxWeightInput.TabIndex = tabIndex++;
            Controls.Add(MaxWeightInput);

            //
            // MaxWeightUnit
            // 
            if (MaxWeightUnit == null) MaxWeightUnit = new System.Windows.Forms.Label();
            MaxWeightUnit.Name = "MaxWeightUnit";
            MaxWeightUnit.AutoSize = false;
            MaxWeightUnit.TabIndex = tabIndex++;
            MaxWeightUnit.Text = ChoosedWeighScale.sensor.get_unit();
            Controls.Add(MaxWeightUnit);

            PutRefWeightPanelResize();

            ResumeLayout(false);

            RefWeightInput.Text = "200";
            MaxWeightInput.Text = "250";


            


            NextButton.Visible = true;
          
            PrevButton.Visible = true;
            PrevButton.TabIndex = tabIndex++;
            NextButton.TabIndex = tabIndex++;

            RefWeightInput_textchanged(null, null);


        }

        private void RefWeightInput_textchanged(object sender, EventArgs e)
        {
            refWeight = 0;
            maxWeight = 0;
            try { refWeight = Double.Parse(RefWeightInput.Text); }   catch (Exception ) { }
            try { maxWeight = Double.Parse(MaxWeightInput.Text); } catch (Exception ) { }

            NextButton.Enabled= (refWeight>0) && (maxWeight>0);

        }

        private PanelDesc.WizardSteps PutRefWeightnDrawPanel_NextClicked()
        {
            if (refWeight> maxWeight)
             {
                DialogResult dialogResult = MessageBox.Show("Reference weight is not supposed to be higher than\nload cell maximum load capability\n\n Are you sure about these numbers?", "humm.. This is suspicious", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No) return PanelDesc.WizardSteps.ERROR;
             }
            ChoosedWeighScale.sensor.setupSpan(refWeight, maxWeight);
            return PanelDesc.WizardSteps.REMOVEREFLOAD;
        }


    }
}