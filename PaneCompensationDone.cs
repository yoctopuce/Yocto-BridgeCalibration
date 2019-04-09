
using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {

    private Label CompensationDoneMessage1 = null;


    public void CompensationDonePanelResize()
    {
      CompensationDoneMessage1.Size = new System.Drawing.Size(600, 50);
      CompensationDoneMessage1.Location = new System.Drawing.Point((ClientSize.Width- CompensationDoneMessage1.Width) /2, (ClientSize.Height - CompensationDoneMessage1.Height) / 2);

    }

    public void CompensationDonePanelClearContents()
    {
      Controls.Remove(CompensationDoneMessage1);
     
    }

    public void CompensationDoneDrawPanel()
    {
      int tabIndex = 0;
      SuspendLayout();
      //
      // CompensationDoneMessage1
      // 
      if (CompensationDoneMessage1 == null) CompensationDoneMessage1 = new System.Windows.Forms.Label();
      CompensationDoneMessage1.Name = "ConfirmCompensationMessage1";
      CompensationDoneMessage1.AutoSize = false;
      CompensationDoneMessage1.TabIndex = tabIndex++;
      CompensationDoneMessage1.TextAlign= System.Drawing.ContentAlignment.MiddleCenter;
      CompensationDoneMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      CompensationDoneMessage1.Text = ChoosedWeighScale.ToString() + "'s compensation is now defined, you  can click on \"Next\" to go to the main menu";
      Controls.Add(CompensationDoneMessage1);


   
      ResumeLayout(false);
      NextButton.Visible = true;
      NextButton.Enabled = true;
      PrevButton.Visible = false;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;
      CompensationDonePanelResize();

    }

 

    private PanelDesc.WizardSteps CompensationDoneNextClicked()
    {

      history.Clear();
      return PanelDesc.WizardSteps.WELCOME;
    }

  }
}