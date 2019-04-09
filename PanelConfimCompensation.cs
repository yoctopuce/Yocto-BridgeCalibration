
using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {

    private Label ConfirmCompensationMessage1 = null;

    private Label confirmTableTemp = null;
    private Label confirmTableTempMinus30 = null;
    private Label confirmTableTempZero = null;
    private Label confirmTableTemp100 = null;

    private Label confirmTableTCC = null;
    private Label confirmTableTCCMinus30 = null;
    private Label confirmTableTCCZero = null;
    private Label confirmTableTCC100 = null;

    private Label confirmTableATC = null;
    private Label confirmTableATCMinus30 = null;
    private Label confirmTableATCZero = null;
    private Label confirmTableATC100 = null;

    private Label confirmTableClickNext = null;

    public void ConfirmCompensatioPanelResize()
    { 
      ConfirmCompensationMessage1.Size = new System.Drawing.Size(600, 100);
      ConfirmCompensationMessage1.Location = new System.Drawing.Point((ClientSize.Width- ConfirmCompensationMessage1.Width) /2, (ClientSize.Height / 2)- ConfirmCompensationMessage1.Height-100);

    
      int col1 = ClientSize.Width / 2 - 250;
      int col2 = ClientSize.Width / 2 - 50;
      int col3 = ClientSize.Width / 2 +150;

      int row1 = ClientSize.Height / 2 -50;
      int row2 = row1 + 20; 
      int row3 = row2 + 20;
      int row4 = row3 + 20;

      confirmTableTemp.Location = new System.Drawing.Point(col1+25, row1); 
     confirmTableTempMinus30.Location = new System.Drawing.Point(col1, row2);
     confirmTableTempZero.Location = new System.Drawing.Point(col1, row3);
     confirmTableTemp100.Location = new System.Drawing.Point(col1, row4);

     confirmTableTCC.Location = new System.Drawing.Point(col2, row1);
     confirmTableTCCMinus30.Location = new System.Drawing.Point(col2, row2);
     confirmTableTCCZero.Location = new System.Drawing.Point(col2, row3);
     confirmTableTCC100.Location = new System.Drawing.Point(col2, row4);

     confirmTableATC.Location = new System.Drawing.Point(col3, row1);
     confirmTableATCMinus30.Location = new System.Drawing.Point(col3, row2);
     confirmTableATCZero.Location = new System.Drawing.Point(col3, row3);
     confirmTableATC100.Location = new System.Drawing.Point(col3, row4);

      confirmTableClickNext.Size = new System.Drawing.Size(600, 30);
      confirmTableClickNext.Location = new System.Drawing.Point((ClientSize.Width - confirmTableClickNext.Width) / 2, confirmTableATC100.Bottom+50);



    }

    public void ConfirmCompensationPanelClearContents()
    {
      Controls.Remove(ConfirmCompensationMessage1);
      Controls.Remove(confirmTableTemp);
      Controls.Remove(confirmTableTempMinus30);
      Controls.Remove(confirmTableTempZero);
      Controls.Remove(confirmTableTemp100);
      Controls.Remove(confirmTableTCC);
      Controls.Remove(confirmTableTCCMinus30);
      Controls.Remove(confirmTableTCCZero);
      Controls.Remove(confirmTableTCC100);
      Controls.Remove(confirmTableATC);
      Controls.Remove(confirmTableATCMinus30);
      Controls.Remove(confirmTableATCZero);
      Controls.Remove(confirmTableATC100);
      Controls.Remove(confirmTableClickNext);
    }

    public void ConfirmCompensationDrawPanel()
    {
      int tabIndex = 0;
      SuspendLayout();
      //
      // WarningResetMessage1
      // 
      if (ConfirmCompensationMessage1 == null) ConfirmCompensationMessage1 = new System.Windows.Forms.Label();
      ConfirmCompensationMessage1.Name = "ConfirmCompensationMessage1";
      ConfirmCompensationMessage1.AutoSize = false;
      ConfirmCompensationMessage1.TabIndex = tabIndex++;
      ConfirmCompensationMessage1.TextAlign= System.Drawing.ContentAlignment.MiddleCenter;
      ConfirmCompensationMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      ConfirmCompensationMessage1.Text = "You are about to inject the following compensation table in the sensor "+ChoosedWeighScale.ToString();
    
      Controls.Add(ConfirmCompensationMessage1);

  

      if (confirmTableTemp == null) confirmTableTemp = new System.Windows.Forms.Label();
      confirmTableTemp.Name = "confirmTableTemp";
      confirmTableTemp.TabIndex = tabIndex++;

      confirmTableTemp.AutoSize = true;
      confirmTableTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableTemp.Text = label10.Text;
      Controls.Add(confirmTableTemp);

      if (confirmTableTempMinus30 == null)  confirmTableTempMinus30 = new System.Windows.Forms.Label();
      confirmTableTempMinus30.Name = "confirmTableTempMinus30";
      confirmTableTempMinus30.AutoSize = false;
      confirmTableTempMinus30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableTempMinus30.Width = 100;
      confirmTableTempMinus30.TabIndex = tabIndex++;
      confirmTableTempMinus30.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableTempMinus30.Text = label11.Text;
      Controls.Add(confirmTableTempMinus30);

      if (confirmTableTempZero == null) confirmTableTempZero = new System.Windows.Forms.Label();
      confirmTableTempZero.Name = "confirmTableTempZero";
      confirmTableTempZero.AutoSize = false;
      confirmTableTempZero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableTempZero.Width = 100;
      confirmTableTempZero.TabIndex = tabIndex++;
      confirmTableTempZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableTempZero.Text = label13.Text;
      Controls.Add(confirmTableTempZero);

      if (confirmTableTemp100 == null) confirmTableTemp100 = new System.Windows.Forms.Label();
      confirmTableTemp100.Name = "confirmTableTemp100";
      confirmTableTempZero.AutoSize = false;
      confirmTableTemp100.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableTemp100.TabIndex = tabIndex++;
      confirmTableTemp100.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableTemp100.Text = label16.Text;
      Controls.Add(confirmTableTemp100);

      if (confirmTableTCC == null) confirmTableTCC = new System.Windows.Forms.Label();
      confirmTableTCC.Name = "confirmTableTCC";
      confirmTableTCC.AutoSize = true;
      confirmTableTCC.TabIndex = tabIndex++;
      confirmTableTCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableTCC.Text = label3.Text;
      Controls.Add(confirmTableTCC);

      if (confirmTableTCCMinus30  == null) confirmTableTCCMinus30 = new System.Windows.Forms.Label();
      confirmTableTCCMinus30.Name = "confirmTableTCCMinus30";
      confirmTableTCCMinus30.AutoSize = false;
      confirmTableTCCMinus30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableTCCMinus30.Width = 100;
      confirmTableTCCMinus30.TabIndex = tabIndex++;
      confirmTableTCCMinus30.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableTCCMinus30.Text = deltaCompMin.Text;
      Controls.Add(confirmTableTCCMinus30);

      if (confirmTableTCCZero == null) confirmTableTCCZero = new System.Windows.Forms.Label();
      confirmTableTCCZero.Name = "confirmTableTCCZero";
      confirmTableTCCZero.AutoSize = false;
      confirmTableTCCZero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableTCCZero.Width = 100;
      confirmTableTCCZero.TabIndex = tabIndex++;
      confirmTableTCCZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableTCCZero.Text = deltaCompZero.Text;
      Controls.Add(confirmTableTCCZero);

      if (confirmTableTCC100 == null) confirmTableTCC100 = new System.Windows.Forms.Label();
      confirmTableTCC100.Name = "confirmTableTCC100";
      confirmTableTCC100.AutoSize = false;
      confirmTableTCC100.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableTCC100.Width = 100;
      confirmTableTCC100.TabIndex = tabIndex++;
      confirmTableTCC100.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableTCC100.Text = deltaCompMax.Text;
      Controls.Add(confirmTableTCC100);

      if (confirmTableATC == null) confirmTableATC = new System.Windows.Forms.Label();
      confirmTableATC.Name = "confirmTableATC";
      confirmTableATC.AutoSize = true;
      confirmTableATC.TabIndex = tabIndex++;
      confirmTableATC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableATC.Text = label5.Text;
      Controls.Add(confirmTableATC);

      if (confirmTableATCMinus30 == null) confirmTableATCMinus30 = new System.Windows.Forms.Label();
      confirmTableATCMinus30.Name = "confirmTableATCMinus30";
      confirmTableATCMinus30.AutoSize = false;
      confirmTableATCMinus30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableATCMinus30.Width = 100;
      confirmTableATCMinus30.TabIndex = tabIndex++;
      confirmTableATCMinus30.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableATCMinus30.Text = avgCompMin.Text;
      Controls.Add(confirmTableATCMinus30);

      if (confirmTableATCZero  == null) confirmTableATCZero = new System.Windows.Forms.Label();
      confirmTableATCZero.Name = "confirmTableATCZero";
      confirmTableATCZero.AutoSize = false;
      confirmTableATCZero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableATCZero.Width = 100;
      confirmTableATCZero.TabIndex = tabIndex++;
      confirmTableATCZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableATCZero.Text = avgCompZero.Text;
      Controls.Add(confirmTableATCZero);

      if (confirmTableATC100 == null) confirmTableATC100 = new System.Windows.Forms.Label();
      confirmTableATC100.Name = "confirmTableATC100";
      confirmTableATC100.AutoSize = false;
      confirmTableATC100.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      confirmTableATC100.Width = 100;
      confirmTableATC100.TabIndex = tabIndex++;
      confirmTableATC100.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableATC100.Text = avgCompMax.Text;
      Controls.Add(confirmTableATC100);


      if (confirmTableClickNext==null) confirmTableClickNext = new System.Windows.Forms.Label();
      confirmTableClickNext.Name = "confirmTableATC100";
      confirmTableClickNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;   
      confirmTableClickNext.TabIndex = tabIndex++;
      confirmTableClickNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      confirmTableClickNext.Text = "Click on \"Next\" to continue";
      Controls.Add(confirmTableClickNext);
      ResumeLayout(false);
      NextButton.Visible = true;
      NextButton.Enabled = true;
      PrevButton.Visible = false;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;
      ConfirmCompensatioPanelResize();

    }

 

    private PanelDesc.WizardSteps ConfirmCompensationNextClicked()
    {
      ChoosedWeighScale.setCompensation(deviceTempCompensationValues, deviceOffsetChgCompensationValues, deviceOffsetAvgCompensationValues);

      return PanelDesc.WizardSteps.COMPENSATIONDONE;
    }

  }
}