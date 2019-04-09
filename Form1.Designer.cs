

namespace Yocto_BridgeCalibration
{
  


   


    partial class Form1
    {



       

        

        

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.ExportFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.UILabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // ExportFileDialog1
      // 
      this.ExportFileDialog1.Filter = "CSV file (.csv)|*.csv|Vector Image (.svg)|*.svg|Raster Image (.png)|*.png";
      this.ExportFileDialog1.Title = "Export Data";
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "\"CSV files (*.csv)|*.csv|All files (*.*)|*.*\";";
      // 
      // UILabel
      // 
      this.UILabel.AutoSize = true;
      this.UILabel.Location = new System.Drawing.Point(117, 182);
      this.UILabel.Name = "UILabel";
      this.UILabel.Size = new System.Drawing.Size(345, 13);
      this.UILabel.TabIndex = 0;
      this.UILabel.Text = "Don\'t look for the UI here, it is generated dynamcally, see Panel*.cs files";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(900, 600);
      this.Controls.Add(this.UILabel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(900, 600);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Yocto-Bridge Calibration Utility";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Resize += new System.EventHandler(this.Form1_Resize);
      this.ResumeLayout(false);
      this.PerformLayout();

         }

       

        

        #endregion

        
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PrevButton;
       


       

        public void DrawPrevNextButtons()
        {
            this.SuspendLayout();
            this.NextButton = new System.Windows.Forms.Button();
            this.PrevButton = new System.Windows.Forms.Button();
           
            // 
            // NextButton
            // 
            
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 4;
            this.NextButton.Text = "Next >";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.NextButton.Click += NextButton_Click;
            // 
            // PrevButton
            // 
            this.PrevButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.PrevButton.Size = new System.Drawing.Size(75, 23);
            this.PrevButton.Name = "PrevButton";
            
            this.PrevButton.TabIndex = 5;
            this.PrevButton.Text = "< Prev";
            this.PrevButton.UseVisualStyleBackColor = true;
            this.PrevButton.Click += PrevButton_Click;

            this.Controls.Add(this.PrevButton);
            this.Controls.Add(this.NextButton);
            this.ResumeLayout(false);
        }

       

        private void DrawCurrentPanel()
        {
            PanelDesc.Panel(CurrentStep).drawContents();      
        }

        private void PrevButton_Click(object sender, System.EventArgs e)
        {
            ClearCurrentPanel();
            CurrentStep = history[history.Count - 1];
            history.RemoveAt(history.Count - 1);
 
            DrawCurrentPanel();
        }

        private void ClearCurrentPanel()
        {
            PanelDesc.Panel(CurrentStep).clearContents();

        }


        private void NextButton_Click(object sender, System.EventArgs e)
        {
            PanelDesc.WizardSteps next = PanelDesc.WizardSteps.ERROR;
            history.Add(CurrentStep);
            next = PanelDesc.Panel(CurrentStep).nextClicked(); 
            if (next == PanelDesc.WizardSteps.ERROR) return;
            ClearCurrentPanel();
            CurrentStep = next;
            DrawCurrentPanel();
        }

        public void GenericResize()
        {
            this.NextButton.Location = new System.Drawing.Point(ClientSize.Width - NextButton.Size.Width - 5, ClientSize.Height - NextButton.Height - 5);
            this.PrevButton.Location = new System.Drawing.Point(NextButton.Left - PrevButton.Width - 5, ClientSize.Height - PrevButton.Height - 5);         
        }

        private System.Windows.Forms.SaveFileDialog ExportFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label UILabel;
    }
}

