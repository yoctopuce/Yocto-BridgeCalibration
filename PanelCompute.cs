using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.IO;

using System.Windows.Forms;
using YDataRendering;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {

    // const int maxPoints = 500000;
    const double learningRate = 0.002;
    const int nIterations = 10000;
    int SpikyMsgCount = 0;

    int totalCount;
    int learnCount;
    int prevProgress;
    double aRatio, alpha, bRatio, beta, autoOffset;
    double[] stamp, temp, rawW, avgTemp, deltaTemp, pred;
    double avgSqError;
    bool disable_textChangeEvents;

    YGraph chart = null;
    DataSerie tempData = null;
    DataSerie avgTempData = null;
    DataSerie deltaTempData = null;
    DataSerie rawWData = null;
    DataSerie predData = null;
    DataSerie errorData = null;
    Zone LearnZone = null;
    Zone CheckZone = null;
    YAxis s0 = null;
    YAxis s1 = null;
    DataPanel learningData = null;
    DataPanel checkingData = null;
    DataPanel SpikyData = null;





    private PictureBox pictureBox1 = null;
    private CheckBox showRawW = null;
    private CheckBox showAvg = null;
    private CheckBox showDelta = null;
    private GroupBox groupBox1 = null;
    private GroupBox groupBox2 = null;
    private CheckBox showPrediction = null;
    private CheckBox showError = null;
    private CheckBox showTemp = null;
    private CheckBox lockYaxes = null;
    private GroupBox groupBox3 = null;
    private TextBox deltaRatio = null;
    private TextBox avgRatio = null;
    private TrackBar deltaRatioTrackBar = null;
    private TrackBar avgRatioTrackBar = null;
    private Label label4 = null;
    private TextBox logBox = null;
    private CheckBox deriv = null;
    private Label label3 = null;
    private Label label1 = null;
    private Label label5 = null;
    private Label label2 = null;
    private Button autoDelta = null;
    private Button autoAvg = null;
    private GroupBox groupBox5 = null;
    private Label rmsError = null;
    private Label rmsErrorChk = null;
    private Label label6 = null;
    private ProgressBar progressBar = null;
    private BackgroundWorker gradientDescentWorker = null;
    private Label label7 = null;
    private Label label8 = null;
    private GroupBox groupBox8 = null;
    private Label avgCompMax = null;
    private Label label16 = null;
    private Label deltaCompMax = null;
    private GroupBox groupBox7 = null;
    private Label avgCompZero = null;
    private Label label13 = null;
    private Label deltaCompZero = null;
    private GroupBox groupBox6 = null;
    private Label avgCompMin = null;
    private Label label11 = null;
    private Label deltaCompMin = null;
    private GroupBox groupBox4 = null;
    private Label label10 = null;
    private Label label9 = null;
    private BackgroundWorker tempChangeWorker = null;



    private Panel dialogPanel = null;
    private Label errorMessage = null;
    private Button CancelComputationButton = null;
    private Label dialogTitle = null;
    private Button FilterButton = null;
    private Button ExportResultsButton = null;


    public void computePanelResize()
    {
      groupBox3.Top = ClientSize.Height - NextButton.Height - groupBox3.Height - 10;
      groupBox1.Top = groupBox3.Top;
      groupBox1.Width = ClientSize.Width - groupBox3.Width - groupBox5.Width - 10;
      groupBox2.Top = groupBox1.Top + groupBox1.Height;
      groupBox2.Width = groupBox1.Width;
      groupBox5.Top = groupBox3.Top;
      groupBox5.Left = ClientSize.Width - groupBox5.Width - 5;
      autoDelta.Left = groupBox1.Width - autoDelta.Width - 3;
      autoAvg.Left = autoDelta.Left;
      deltaRatioTrackBar.Width = groupBox1.Width - 10;
      avgRatioTrackBar.Width = deltaRatioTrackBar.Width;
      label7.Left = autoDelta.Left - label7.Width - 5;
      label8.Left = label7.Left;
      deltaRatio.Left = label7.Left - deltaRatio.Width;
      avgRatio.Left = deltaRatio.Left;
      FilterButton.Location = new Point(0, 0);


      deltaRatioTrackBar.MouseWheel += TrackBar_MouseWheel;
      avgRatioTrackBar.MouseWheel += TrackBar_MouseWheel;

      this.MouseWheel += chart.mouseWheelEvent;


      pictureBox1.Size = new System.Drawing.Size(ClientSize.Width - 10, ClientSize.Height - groupBox3.Height - NextButton.Height - 15);
      dialogPanel.AutoScrollPosition = new Point((ClientSize.Width - dialogPanel.Width) / 2, pictureBox1.Top + (pictureBox1.Height - dialogPanel.Height) / 2);

      ExportResultsButton.Location = new Point(PrevButton.Left - ExportResultsButton.Width - 5, ClientSize.Height - NextButton.Height - 5);
      FilterButton.Location = new Point(ExportResultsButton.Left - FilterButton.Width - 5, ClientSize.Height - NextButton.Height - 5);
    }

    private void TrackBar_MouseWheel(object sender, MouseEventArgs e)
    {

      Point locationOnForm = ((Control)sender).FindForm().PointToClient(
           ((Control)sender).Parent.PointToScreen(((Control)sender).Location));

      chart.mouseWheelEvent(sender, new MouseEventArgs(e.Button, e.Clicks, e.X + locationOnForm.X, e.Y + locationOnForm.Y, e.Delta));
      ((HandledMouseEventArgs)e).Handled = true;

    }

    public void computePanelClearContents()
    {
      CancelComputation_Click(null, null);
      groupBox1.Controls.Remove(label7);
      groupBox1.Controls.Remove(autoDelta);
      groupBox1.Controls.Remove(label1);
      groupBox1.Controls.Remove(deltaRatioTrackBar);
      groupBox1.Controls.Remove(deltaRatio);
      groupBox2.Controls.Remove(label8);
      groupBox2.Controls.Remove(autoAvg);
      groupBox2.Controls.Remove(label2);
      groupBox2.Controls.Remove(label4);
      groupBox2.Controls.Remove(avgRatio);
      groupBox2.Controls.Remove(avgRatioTrackBar);
      groupBox3.Controls.Remove(deriv);
      groupBox3.Controls.Remove(showPrediction);
      groupBox3.Controls.Remove(showTemp);
      groupBox3.Controls.Remove(showRawW);
      groupBox3.Controls.Remove(lockYaxes);
      groupBox3.Controls.Remove(showDelta);
      groupBox3.Controls.Remove(showAvg);
      groupBox3.Controls.Remove(showError);

      groupBox5.Controls.Remove(groupBox8);
      groupBox5.Controls.Remove(groupBox7);
      groupBox5.Controls.Remove(groupBox6);
      groupBox5.Controls.Remove(groupBox4);
      groupBox5.Controls.Remove(rmsError);
      groupBox5.Controls.Remove(rmsErrorChk);
      groupBox5.Controls.Remove(label6);
      groupBox8.Controls.Remove(avgCompMax);
      groupBox8.Controls.Remove(label16);
      groupBox8.Controls.Remove(deltaCompMax);
      groupBox7.Controls.Remove(avgCompZero);
      groupBox7.Controls.Remove(label13);
      groupBox7.Controls.Remove(deltaCompZero);
      groupBox6.Controls.Remove(avgCompMin);
      groupBox6.Controls.Remove(label11);
      groupBox6.Controls.Remove(deltaCompMin);
      groupBox4.Controls.Remove(label10);
      groupBox4.Controls.Remove(label9);
      groupBox4.Controls.Remove(label5);
      groupBox4.Controls.Remove(label3);
      dialogPanel.Controls.Remove(errorMessage);
      dialogPanel.Controls.Remove(CancelComputationButton);
      dialogPanel.Controls.Remove(dialogTitle);
      dialogPanel.Controls.Remove(progressBar);
      Controls.Remove(dialogPanel);
      Controls.Remove(groupBox5);
      Controls.Remove(logBox);
      Controls.Remove(groupBox3);
      Controls.Remove(groupBox2);
      Controls.Remove(groupBox1);
      Controls.Remove(pictureBox1);
      Controls.Remove(FilterButton);
      Controls.Remove(ExportResultsButton);


    }

    public void computeDrawPanel()
    {
      history.RemoveAt(history.Count - 1);
      SuspendLayout();
      int tabIndex = 0;

      if (pictureBox1 == null) pictureBox1 = new PictureBox();

      if (showRawW == null) showRawW = new CheckBox();
      if (showAvg == null) showAvg = new CheckBox();
      if (showDelta == null) showDelta = new CheckBox();
      if (lockYaxes == null) lockYaxes = new CheckBox();
      if (groupBox1 == null) groupBox1 = new GroupBox();
      if (label7 == null) label7 = new Label();
      if (label1 == null) autoDelta = new Button();
      if (label1 == null) label1 = new Label();
      if (deltaRatioTrackBar == null) deltaRatioTrackBar = new TrackBar();
      if (deltaRatio == null) deltaRatio = new TextBox();
      if (label3 == null) label3 = new Label();
      if (groupBox2 == null) groupBox2 = new GroupBox();
      if (label8 == null) label8 = new Label();
      if (autoAvg == null) autoAvg = new Button();
      if (label2 == null) label2 = new Label();
      if (label4 == null) label4 = new Label();
      if (avgRatio == null) avgRatio = new TextBox();
      if (avgRatioTrackBar == null) avgRatioTrackBar = new TrackBar();
      if (label5 == null) label5 = new Label();
      if (showPrediction == null) showPrediction = new CheckBox();
      if (showError == null) showError = new CheckBox();
      if (showTemp == null) showTemp = new CheckBox();
      if (groupBox3 == null) groupBox3 = new GroupBox();
      if (deriv == null) deriv = new CheckBox();
      if (logBox == null) logBox = new TextBox();
      if (groupBox5 == null) groupBox5 = new GroupBox();

      if (groupBox8 == null) groupBox8 = new GroupBox();
      if (avgCompMax == null) avgCompMax = new Label();
      if (label16 == null) label16 = new Label();
      if (deltaCompMax == null) deltaCompMax = new Label();
      if (groupBox7 == null) groupBox7 = new GroupBox();
      if (avgCompZero == null) avgCompZero = new Label();
      if (label13 == null) label13 = new Label();
      if (deltaCompZero == null) deltaCompZero = new Label();
      if (groupBox6 == null) groupBox6 = new GroupBox();
      if (avgCompMin == null) avgCompMin = new Label();
      if (label11 == null) label11 = new Label();
      if (deltaCompMin == null) deltaCompMin = new Label();
      if (groupBox4 == null) groupBox4 = new GroupBox();
      if (label10 == null) label10 = new Label();
      if (label9 == null) label9 = new Label();
      if (rmsError == null) rmsError = new Label();
      if (rmsErrorChk == null) rmsErrorChk = new Label();
      if (label6 == null) label6 = new Label();
      if (progressBar == null) progressBar = new ProgressBar();
      if (gradientDescentWorker == null) gradientDescentWorker = new System.ComponentModel.BackgroundWorker();
      if (tempChangeWorker == null) tempChangeWorker = new System.ComponentModel.BackgroundWorker();
      if (FilterButton == null) FilterButton = new Button();
      if (ExportResultsButton == null) ExportResultsButton = new Button();

      if (dialogPanel == null) dialogPanel = new Panel();
      if (errorMessage == null) errorMessage = new Label();
      if (CancelComputationButton == null) CancelComputationButton = new Button();
      if (dialogTitle == null) dialogTitle = new Label();
      ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
      groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(deltaRatioTrackBar)).BeginInit();
      groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(avgRatioTrackBar)).BeginInit();
      groupBox3.SuspendLayout();
      groupBox5.SuspendLayout();
      groupBox8.SuspendLayout();
      groupBox7.SuspendLayout();
      groupBox6.SuspendLayout();
      groupBox4.SuspendLayout();
      dialogPanel.SuspendLayout();
      SuspendLayout();
      // 
      // pictureBox1
      // 
      pictureBox1.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
      | AnchorStyles.Left)
      | AnchorStyles.Right)));
      pictureBox1.BorderStyle = BorderStyle.FixedSingle;
      pictureBox1.Location = new System.Drawing.Point(3, 3);
      pictureBox1.Margin = new Padding(4);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new System.Drawing.Size(1223, 709);
      pictureBox1.TabIndex = tabIndex++;
      pictureBox1.TabStop = false;



      // 
      // groupBox3
      // 
      groupBox3.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));

      groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      groupBox3.Location = new System.Drawing.Point(3, 716);
      groupBox3.Name = "groupBox3";
      groupBox3.Size = new System.Drawing.Size(218, 177);
      groupBox3.TabIndex = tabIndex++;
      groupBox3.TabStop = false;
      groupBox3.Text = "Show graphs";



      // 
      // showTemp
      // 
      showTemp.AutoSize = true;
      showTemp.Checked = true;
      showTemp.CheckState = CheckState.Checked;
      showTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      showTemp.Location = new System.Drawing.Point(10, 22);
      showTemp.Margin = new Padding(4);
      showTemp.Name = "showTemp";
      showTemp.Size = new System.Drawing.Size(174, 21);
      showTemp.TabIndex = tabIndex++;
      showTemp.Text = "Measured temperature";
      showTemp.UseVisualStyleBackColor = true;
      showTemp.CheckedChanged += new System.EventHandler(showTemp_CheckedChanged);

      // 
      // showRawW
      // 
      showRawW.AutoSize = true;
      showRawW.Checked = true;
      showRawW.CheckState = CheckState.Checked;
      showRawW.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      showRawW.Location = new System.Drawing.Point(10, showTemp.Top + 20);
      showRawW.Margin = new Padding(4);
      showRawW.Name = "showRawW";
      showRawW.Size = new System.Drawing.Size(155, 21);
      showRawW.TabIndex = tabIndex++;
      showRawW.Text = "Measured Zero drift";
      showRawW.UseVisualStyleBackColor = true;
      showRawW.CheckedChanged += new System.EventHandler(showRawW_CheckedChanged);

      // 
      // showDelta
      // 
      showDelta.AutoSize = true;
      showDelta.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      showDelta.Location = new System.Drawing.Point(10, showRawW.Top + 20);
      showDelta.Margin = new Padding(4);
      showDelta.Name = "showDelta";
      showDelta.Size = new System.Drawing.Size(163, 21);
      showDelta.TabIndex = tabIndex++;
      showDelta.Text = "Temperature change";
      showDelta.UseVisualStyleBackColor = true;
      showDelta.CheckedChanged += new System.EventHandler(showDelta_CheckedChanged);

      // 
      // showAvg
      // 
      showAvg.AutoSize = true;
      showAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      showAvg.Location = new System.Drawing.Point(10, showDelta.Top + 20);
      showAvg.Margin = new Padding(4);
      showAvg.Name = "showAvg";
      showAvg.Size = new System.Drawing.Size(164, 21);
      showAvg.TabIndex = tabIndex++;
      showAvg.Text = "Average temperature";
      showAvg.UseVisualStyleBackColor = true;
      showAvg.CheckedChanged += new System.EventHandler(showAvg_CheckedChanged);


      // 
      // showPrediction
      // 
      showPrediction.AutoSize = true;
      showPrediction.Checked = true;
      showPrediction.CheckState = CheckState.Checked;
      showPrediction.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      showPrediction.Location = new System.Drawing.Point(10, showAvg.Top + 20);
      showPrediction.Margin = new Padding(4);
      showPrediction.Name = "showPrediction";
      showPrediction.Size = new System.Drawing.Size(118, 21);
      showPrediction.TabIndex = tabIndex++;
      showPrediction.Text = "Predicted drift";
      showPrediction.UseVisualStyleBackColor = true;
      showPrediction.CheckedChanged += new System.EventHandler(showPrediction_CheckedChanged);

      // 
      // deriv
      // 
      deriv.AutoSize = true;
      deriv.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      deriv.Location = new System.Drawing.Point(147, showPrediction.Top);
      deriv.Margin = new Padding(4);
      deriv.Name = "deriv";
      deriv.Size = new System.Drawing.Size(37, 21);
      deriv.TabIndex = tabIndex++;
      deriv.Text = "f\'";
      deriv.UseVisualStyleBackColor = true;
      deriv.CheckedChanged += new System.EventHandler(deriv_CheckedChanged);

      // 
      // showError
      // 
      showError.AutoSize = true;
      showError.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      showError.Location = new System.Drawing.Point(10, deriv.Top + 20);
      showError.Margin = new Padding(4);
      showError.Name = "showError";
      showError.Size = new System.Drawing.Size(120, 21);
      showError.TabIndex = tabIndex++;
      showError.Text = "Residual error";
      showError.UseVisualStyleBackColor = true;
      showError.CheckedChanged += new System.EventHandler(showError_CheckedChanged);

      // lock Y axes
      lockYaxes.AutoSize = true;
      lockYaxes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      lockYaxes.Location = new System.Drawing.Point(10, showError.Top + 25);
      lockYaxes.Margin = new Padding(4);
      lockYaxes.Name = "lockYaxes";
      lockYaxes.Size = new System.Drawing.Size(163, 21);
      lockYaxes.TabIndex = tabIndex++;
      lockYaxes.Text = "Lock Y axes";
      lockYaxes.UseVisualStyleBackColor = true;
      lockYaxes.CheckedChanged += LockYaxes_CheckedChanged;

      groupBox3.Controls.Add(deriv);
      groupBox3.Controls.Add(showPrediction);
      groupBox3.Controls.Add(showTemp);
      groupBox3.Controls.Add(showRawW);
      groupBox3.Controls.Add(showDelta);
      groupBox3.Controls.Add(showAvg);
      groupBox3.Controls.Add(showError);
      groupBox3.Controls.Add(lockYaxes);


      // 
      // groupBox1
      // 
      groupBox1.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));

      groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      groupBox1.Location = new System.Drawing.Point(227, 716);
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new System.Drawing.Size(410, 89);
      groupBox1.TabIndex = tabIndex++;
      groupBox1.TabStop = false;
      groupBox1.Text = "1. Temperature change compensation";

      // 
      // deltaRatio
      // 
      deltaRatio.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      deltaRatio.Location = new System.Drawing.Point(233, 22);
      deltaRatio.Name = "deltaRatio";
      deltaRatio.Size = new System.Drawing.Size(69, 22);
      deltaRatio.TabIndex = tabIndex++;
      deltaRatio.Text = "0.6";
      deltaRatio.TextChanged += new System.EventHandler(deltaRatio_TextChanged);

      // 
      // autoDelta
      // 
      autoDelta.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      autoDelta.Location = new System.Drawing.Point(334, 20);
      autoDelta.Name = "autoDelta";
      autoDelta.Size = new System.Drawing.Size(60, 28);
      autoDelta.TabIndex = tabIndex++;
      autoDelta.Text = "Reset";
      autoDelta.UseVisualStyleBackColor = true;
      autoDelta.Click += new System.EventHandler(autoDelta_Click);




      // 
      // deltaRatioTrackBar
      // 
      deltaRatioTrackBar.AutoSize = false;
      deltaRatioTrackBar.Location = new System.Drawing.Point(6, 44);
      deltaRatioTrackBar.Maximum = 100;
      deltaRatioTrackBar.Minimum = -100;
      deltaRatioTrackBar.Name = "deltaRatioTrackBar";
      deltaRatioTrackBar.Size = new System.Drawing.Size(398, 39);
      deltaRatioTrackBar.TabIndex = tabIndex++;
      deltaRatioTrackBar.TickFrequency = 5;
      deltaRatioTrackBar.Scroll += new System.EventHandler(deltaRatioTrackBar_Scroll);



      groupBox1.Controls.Add(label7);
      groupBox1.Controls.Add(autoDelta);
      groupBox1.Controls.Add(label1);
      groupBox1.Controls.Add(deltaRatioTrackBar);
      groupBox1.Controls.Add(deltaRatio);




      // groupBox2
      // 
      groupBox2.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));

      groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      groupBox2.Location = new System.Drawing.Point(227, 805);
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new System.Drawing.Size(410, 88);
      groupBox2.TabIndex = tabIndex++;
      groupBox2.TabStop = false;
      groupBox2.Text = "2. Average temperature compensation";


      // 
      // avgRatio
      // 
      avgRatio.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      avgRatio.Location = new System.Drawing.Point(233, 24);
      avgRatio.Name = "avgRatio";
      avgRatio.Size = new System.Drawing.Size(69, 22);
      avgRatio.TabIndex = tabIndex++;
      avgRatio.Text = "0.2";
      avgRatio.TextChanged += new System.EventHandler(avgRatio_TextChanged);

      // 
      // autoAvg
      // 
      autoAvg.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      autoAvg.Location = new System.Drawing.Point(334, 23);
      autoAvg.Name = "autoAvg";
      autoAvg.Size = new System.Drawing.Size(60, 25);
      autoAvg.TabIndex = tabIndex++;
      autoAvg.Text = "Reset";
      autoAvg.UseVisualStyleBackColor = true;
      autoAvg.Click += new System.EventHandler(autoAvg_Click);


      // 
      // avgRatioTrackBar
      // 
      avgRatioTrackBar.AutoSize = false;
      avgRatioTrackBar.Location = new System.Drawing.Point(6, 46);
      avgRatioTrackBar.Maximum = 100;
      avgRatioTrackBar.Minimum = -100;
      avgRatioTrackBar.Name = "avgRatioTrackBar";
      avgRatioTrackBar.Size = new System.Drawing.Size(398, 36);
      avgRatioTrackBar.TabIndex = tabIndex++;
      avgRatioTrackBar.TickFrequency = 5;
      avgRatioTrackBar.Scroll += new System.EventHandler(avgRatioTrackBar_Scroll);


      groupBox2.Controls.Add(label8);
      groupBox2.Controls.Add(autoAvg);
      groupBox2.Controls.Add(label2);
      groupBox2.Controls.Add(label4);
      groupBox2.Controls.Add(avgRatio);
      groupBox2.Controls.Add(avgRatioTrackBar);



      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label7.Location = new System.Drawing.Point(305, 23);
      label7.Margin = new Padding(0, 0, 3, 0);
      label7.Name = "label7";
      label7.Size = new System.Drawing.Size(22, 17);
      label7.TabIndex = tabIndex++;
      label7.Text = "‰";

      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label1.Location = new System.Drawing.Point(7, 24);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(220, 17);
      label1.TabIndex = tabIndex++;
      label1.Text = "Thermal inertia (adaptation ratio):";

      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label3.Location = new System.Drawing.Point(201, 18);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(141, 17);
      label3.TabIndex = tabIndex++;
      label3.Text = "Temp. change comp.";
      // 

      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label8.Location = new System.Drawing.Point(305, 27);
      label8.Margin = new Padding(0, 0, 3, 0);
      label8.Name = "label8";
      label8.Size = new System.Drawing.Size(22, 17);
      label8.TabIndex = tabIndex++;
      label8.Text = "‰";

      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label2.Location = new System.Drawing.Point(7, 27);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(220, 17);
      label2.TabIndex = tabIndex++;
      label2.Text = "Thermal inertia (adaptation ratio):";
      // 
      // label4
      // 
      label4.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point(298, 7);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(0, 17);
      label4.TabIndex = tabIndex++;

      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label5.Location = new System.Drawing.Point(385, 18);
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size(113, 17);
      label5.TabIndex = tabIndex++;
      label5.Text = "Avg. temp. comp";


      // 
      // logBox
      // 
      logBox.Location = new System.Drawing.Point(346, 12);
      logBox.Multiline = true;
      logBox.Name = "logBox";
      logBox.Size = new System.Drawing.Size(572, 130);
      logBox.TabIndex = tabIndex++;
      logBox.Visible = false;
      // 
      // groupBox5
      // 
      groupBox5.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));

      groupBox5.Controls.Add(groupBox8);
      groupBox5.Controls.Add(groupBox7);
      groupBox5.Controls.Add(groupBox6);
      groupBox5.Controls.Add(groupBox4);
      groupBox5.Controls.Add(rmsError);
      groupBox5.Controls.Add(rmsErrorChk);
      groupBox5.Controls.Add(label6);
      groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      groupBox5.Location = new System.Drawing.Point(643, 719);
      groupBox5.Name = "groupBox5";
      groupBox5.Size = new System.Drawing.Size(583, 177);
      groupBox5.TabIndex = tabIndex++;
      groupBox5.TabStop = false;
      groupBox5.Text = "Result";

      // 
      // groupBox8
      // 
      groupBox8.Controls.Add(avgCompMax);
      groupBox8.Controls.Add(label16);
      groupBox8.Controls.Add(deltaCompMax);
      groupBox8.Location = new System.Drawing.Point(9, 137);
      groupBox8.Margin = new Padding(0);
      groupBox8.Name = "groupBox8";
      groupBox8.Padding = new Padding(0);
      groupBox8.Size = new System.Drawing.Size(546, 32);
      groupBox8.TabIndex = tabIndex++;
      groupBox8.TabStop = false;
      // 
      // avgCompMax
      // 
      avgCompMax.AutoSize = true;
      avgCompMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      avgCompMax.Location = new System.Drawing.Point(430, 12);
      avgCompMax.Name = "avgCompMax";
      avgCompMax.Size = new System.Drawing.Size(16, 17);
      avgCompMax.TabIndex = tabIndex++;
      avgCompMax.Text = "0";
      avgCompMax.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label16
      // 
      label16.AutoSize = true;
      label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label16.Location = new System.Drawing.Point(30, 12);
      label16.Name = "label16";
      label16.Size = new System.Drawing.Size(51, 17);
      label16.TabIndex = tabIndex++;
      label16.Text = "100 °C";
      label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // deltaCompMax
      // 
      deltaCompMax.AutoSize = true;
      deltaCompMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      deltaCompMax.Location = new System.Drawing.Point(264, 12);
      deltaCompMax.Name = "deltaCompMax";
      deltaCompMax.Size = new System.Drawing.Size(16, 17);
      deltaCompMax.TabIndex = tabIndex++;
      deltaCompMax.Text = "0";
      deltaCompMax.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // groupBox7
      // 
      groupBox7.Controls.Add(avgCompZero);
      groupBox7.Controls.Add(label13);
      groupBox7.Controls.Add(deltaCompZero);
      groupBox7.Location = new System.Drawing.Point(9, 111);
      groupBox7.Margin = new Padding(0);
      groupBox7.Name = "groupBox7";
      groupBox7.Padding = new Padding(0);
      groupBox7.Size = new System.Drawing.Size(546, 30);
      groupBox7.TabIndex = tabIndex++;
      groupBox7.TabStop = false;
      // 
      // avgCompZero
      // 
      avgCompZero.AutoSize = true;
      avgCompZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      avgCompZero.Location = new System.Drawing.Point(430, 12);
      avgCompZero.Name = "avgCompZero";
      avgCompZero.Size = new System.Drawing.Size(16, 17);
      avgCompZero.TabIndex = tabIndex++;
      avgCompZero.Text = "0";
      avgCompZero.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label13
      // 
      label13.AutoSize = true;
      label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label13.Location = new System.Drawing.Point(46, 12);
      label13.Name = "label13";
      label13.Size = new System.Drawing.Size(35, 17);
      label13.TabIndex = tabIndex++;
      label13.Text = "0 °C";
      label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // deltaCompZero
      // 
      deltaCompZero.AutoSize = true;
      deltaCompZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      deltaCompZero.Location = new System.Drawing.Point(264, 12);
      deltaCompZero.Name = "deltaCompZero";
      deltaCompZero.Size = new System.Drawing.Size(16, 17);
      deltaCompZero.TabIndex = tabIndex++;
      deltaCompZero.Text = "0";
      deltaCompZero.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // groupBox6
      // 
      groupBox6.Controls.Add(avgCompMin);
      groupBox6.Controls.Add(label11);
      groupBox6.Controls.Add(deltaCompMin);
      groupBox6.Location = new System.Drawing.Point(9, 85);
      groupBox6.Margin = new Padding(0);
      groupBox6.Name = "groupBox6";
      groupBox6.Padding = new Padding(0);
      groupBox6.Size = new System.Drawing.Size(546, 30);
      groupBox6.TabIndex = tabIndex++;
      groupBox6.TabStop = false;
      // 
      // avgCompMin
      // 
      avgCompMin.AutoSize = true;
      avgCompMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      avgCompMin.Location = new System.Drawing.Point(430, 12);
      avgCompMin.Name = "avgCompMin";
      avgCompMin.Size = new System.Drawing.Size(16, 17);
      avgCompMin.TabIndex = tabIndex++;
      avgCompMin.Text = "0";
      avgCompMin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // label11
      // 
      label11.AutoSize = true;
      label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label11.Location = new System.Drawing.Point(33, 12);
      label11.Name = "label11";
      label11.Size = new System.Drawing.Size(48, 17);
      label11.TabIndex = tabIndex++;
      label11.Text = "-30 °C";
      label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // deltaCompMin
      // 
      deltaCompMin.AutoSize = true;
      deltaCompMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      deltaCompMin.Location = new System.Drawing.Point(264, 12);
      deltaCompMin.Name = "deltaCompMin";
      deltaCompMin.Size = new System.Drawing.Size(16, 17);
      deltaCompMin.TabIndex = tabIndex++;
      deltaCompMin.Text = "0";
      deltaCompMin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // groupBox4
      // 
      groupBox4.Controls.Add(label10);
      groupBox4.Controls.Add(label9);
      groupBox4.Controls.Add(label5);
      groupBox4.Controls.Add(label3);
      groupBox4.Location = new System.Drawing.Point(9, 48);
      groupBox4.Name = "groupBox4";
      groupBox4.Size = new System.Drawing.Size(546, 46);
      groupBox4.TabIndex = tabIndex++;
      groupBox4.TabStop = false;
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label10.Location = new System.Drawing.Point(6, 21);
      label10.Name = "label10";
      label10.Size = new System.Drawing.Size(90, 17);
      label10.TabIndex = tabIndex++;
      label10.Text = "Temperature";
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label9.Location = new System.Drawing.Point(264, -2);
      label9.Name = "label9";
      label9.Size = new System.Drawing.Size(182, 17);
      label9.TabIndex = tabIndex++;
      label9.Text = "Temperature compensation";
      // 
      // rmsError
      // 
      rmsError.AutoSize = true;
      rmsError.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      rmsError.Location = new System.Drawing.Point(200, 20);
      rmsError.Name = "rmsError";
      rmsError.Size = new System.Drawing.Size(170, 17);
      rmsError.TabIndex = tabIndex++;
      rmsError.Text = "";
      // 
      // rmsErrorChk
      // 
      rmsErrorChk.AutoSize = true;
      rmsErrorChk.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      rmsErrorChk.Location = new System.Drawing.Point(400, 20);
      rmsErrorChk.Name = "rmsErrorChk";
      rmsErrorChk.Size = new System.Drawing.Size(170, 17);
      rmsErrorChk.TabIndex = tabIndex++;
      rmsErrorChk.Text = "";
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      label6.Location = new System.Drawing.Point(20, 20);
      label6.Name = "label6";
      label6.Size = new System.Drawing.Size(80, 17);
      label6.TabIndex = tabIndex++;
      label6.Text = "RMS error:";
      // 
      // progressBar
      // 
      progressBar.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
      | AnchorStyles.Right)));
      progressBar.Location = new System.Drawing.Point(43, 104);
      progressBar.Name = "progressBar";
      progressBar.Size = new System.Drawing.Size(696, 45);
      progressBar.TabIndex = tabIndex++;
      progressBar.Visible = false;
      // 
      // gradientDescentWorker
      // 
      gradientDescentWorker.WorkerReportsProgress = true;
      gradientDescentWorker.WorkerSupportsCancellation = true;
      gradientDescentWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(gradientDescent_DoWork);
      gradientDescentWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(gradientDescent_ProgressChanged);
      gradientDescentWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(gradientDescent_RunWorkerCompleted);
      // 
      // tempChangeWorker
      // 
      tempChangeWorker.WorkerReportsProgress = true;
      tempChangeWorker.WorkerSupportsCancellation = true;
      tempChangeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(tempChangeWorker_DoWork);
      tempChangeWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(tempChangeWorker_ProgressChanged);
      tempChangeWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(tempChangeWorker_RunWorkerCompleted);
      // 
      // 
      // dialogPanel
      // 
      dialogPanel.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
      | AnchorStyles.Left)
      | AnchorStyles.Right)));
      dialogPanel.Controls.Add(errorMessage);
      dialogPanel.Controls.Add(CancelComputationButton);
      dialogPanel.Controls.Add(dialogTitle);
      dialogPanel.Controls.Add(progressBar);
      dialogPanel.Location = new System.Drawing.Point(237, 209);
      dialogPanel.BorderStyle = BorderStyle.FixedSingle;
      dialogPanel.Name = "dialogPanel";
      dialogPanel.Size = new System.Drawing.Size(776, 250);
      dialogPanel.TabIndex = tabIndex++;
      dialogPanel.Visible = false;
      // 
      // errorMessage
      // 
      errorMessage.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
      | AnchorStyles.Right)));
      errorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      errorMessage.Location = new System.Drawing.Point(89, 101);
      errorMessage.Name = "errorMessage";
      errorMessage.Size = new System.Drawing.Size(598, 49);
      errorMessage.TabIndex = tabIndex++;
      errorMessage.Text = "errorMessage";
      errorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // dialogButton
      // 
      CancelComputationButton.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
      CancelComputationButton.AutoSize = true;
      CancelComputationButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      CancelComputationButton.Location = new System.Drawing.Point(679, 186);
      CancelComputationButton.Name = "dialogButton";
      CancelComputationButton.Size = new System.Drawing.Size(69, 35);
      CancelComputationButton.TabIndex = tabIndex++;
      CancelComputationButton.Text = "Abort";
      CancelComputationButton.UseVisualStyleBackColor = true;
      CancelComputationButton.Click += new System.EventHandler(CancelComputation_Click);
      // 
      // dialogTitle
      // 
      dialogTitle.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
      | AnchorStyles.Right)));
      dialogTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dialogTitle.Location = new System.Drawing.Point(89, 22);
      dialogTitle.Name = "dialogTitle";
      dialogTitle.Size = new System.Drawing.Size(598, 49);
      dialogTitle.TabIndex = tabIndex++;
      dialogTitle.Text = "DialogTitle";
      dialogTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;


      FilterButton.Name = "FilterButton";
      FilterButton.Text = "Filter";
      FilterButton.Click += FilterButton_Click;
      Controls.Add(FilterButton);

      ExportResultsButton.Name = "ExportResultsButton";
      ExportResultsButton.Text = "Export..";
      ExportResultsButton.Click += ExportResultsButton_Click;
      Controls.Add(ExportResultsButton);


      // 
      // Form1
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      AutoScaleMode = AutoScaleMode.Font;

      Controls.Add(dialogPanel);
      Controls.Add(groupBox5);
      Controls.Add(logBox);
      Controls.Add(groupBox3);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Controls.Add(pictureBox1);
      Margin = new Padding(4);

      ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(deltaRatioTrackBar)).EndInit();
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(avgRatioTrackBar)).EndInit();
      groupBox3.ResumeLayout(false);
      groupBox3.PerformLayout();
      groupBox5.ResumeLayout(false);
      groupBox5.PerformLayout();
      groupBox8.ResumeLayout(false);
      groupBox8.PerformLayout();
      groupBox7.ResumeLayout(false);
      groupBox7.PerformLayout();
      groupBox6.ResumeLayout(false);
      groupBox6.PerformLayout();
      groupBox4.ResumeLayout(false);
      groupBox4.PerformLayout();
      dialogPanel.ResumeLayout(false);
      dialogPanel.PerformLayout();
      ResumeLayout(false);
      PerformLayout();


      if (chart == null) chart = new YGraph(pictureBox1, LogManager.Log);
      stamp = Loadedstamp.ToArray();
      temp = Loadedtemp.ToArray();
      rawW = LoadedrawW.ToArray();

      avgTemp = new double[Loadedstamp.Count];
      deltaTemp = new double[Loadedstamp.Count];
      pred = new double[Loadedstamp.Count];

      if (s0 == null) s0 = chart.addYAxis();
      s0.legend.title = "Temperature (°C)";
      s0.position = YAxis.HrzPosition.RIGHT;

      if (s1 == null) s1 = chart.addYAxis();
      s1.showGrid = true;
      s1.legend.title = "Weight";
      s1.highlightZero = true;

      chart.navigator.enabled = true;
      chart.navigator.relativeheight = 10;
      chart.xAxis.showGrid = true;
      chart.legendPanel.enabled = true;
      chart.dataTracker.showSerieName = true;
      chart.dataTracker.dataPrecision = DataTracker.DataPrecision.PRECISION_0001;
      chart.dataTracker.enabled = true;

      chart.navigator.yAxisHandling = Navigator.YAxisHandling.INHERIT;
      computePanelResize();

      // add graph series
      totalCount = temp.Length - 24;
      learnCount = 3 * totalCount / 4;
      disable_textChangeEvents = false;

      chart.DisableRedraw();
      if (tempData == null) tempData = chart.addSerie();
      if (avgTempData == null) avgTempData = chart.addSerie();
      if (deltaTempData == null) deltaTempData = chart.addSerie();
      if (rawWData == null) rawWData = chart.addSerie();
      if (predData == null) predData = chart.addSerie();
      if (errorData == null) errorData = chart.addSerie();
      for (int i = 0; i < totalCount; i++)
      {
        tempData.AddPoint(new pointXY() { x = stamp[i], y = temp[i] });
        rawWData.AddPoint(new pointXY() { x = stamp[i], y = rawW[i] });
      }
      chart.xAxis.set_minMax(stamp[learnCount], stamp[totalCount - 1]);

      if (LearnZone == null) LearnZone = chart.xAxis.AddZone();
      if (CheckZone == null) CheckZone = chart.xAxis.AddZone();
      LearnZone.set_minMax(stamp[0], stamp[learnCount]);
      LearnZone.color = Color.FromArgb(25, 0, 0, 255);
      LearnZone.visible = true;


      CheckZone.set_minMax(stamp[learnCount], stamp[totalCount - 1]);
      CheckZone.color = Color.FromArgb(25, 0, 255, 0);
      CheckZone.visible = true;


      if (learningData == null) learningData = chart.addDataPanel();
      learningData.AbsoluteXposition = stamp[learnCount];
      learningData.horizontalMargin = 5;
      learningData.verticalMargin = 5;
      learningData.horizontalPosition = DataPanel.HorizontalPosition.ABSOLUTEX;
      learningData.verticalPosition = DataPanel.VerticalPosition.TOPBORDER;
      learningData.panelHrzAlign = DataPanel.HorizontalAlign.LEFTOF;
      learningData.panelVrtAlign = DataPanel.VerticalAlign.BELOW;
      learningData.padding = 3;
      learningData.borderthickness = 1;
      learningData.borderColor = Color.FromArgb(192, 0, 0, 255);
      learningData.bgColor = LearnZone.color;
      learningData.font.color = learningData.borderColor;
      learningData.text = "Learning set";
      learningData.enabled = true;


      if (checkingData == null) checkingData = chart.addDataPanel();
      checkingData.AbsoluteXposition = stamp[learnCount];
      checkingData.horizontalMargin = 5;
      checkingData.verticalMargin = 5;
      checkingData.horizontalPosition = DataPanel.HorizontalPosition.ABSOLUTEX;
      checkingData.verticalPosition = DataPanel.VerticalPosition.TOPBORDER;
      checkingData.panelHrzAlign = DataPanel.HorizontalAlign.RIGHTOF;
      checkingData.panelVrtAlign = DataPanel.VerticalAlign.BELOW;
      checkingData.padding = 3;
      checkingData.borderthickness = 1;
      checkingData.borderColor = Color.FromArgb(192, 0, 128, 0);
      checkingData.bgColor = CheckZone.color;
      checkingData.font.color = checkingData.borderColor;
      checkingData.text = "Testing set";
      checkingData.enabled = true;

      if (SpikyData == null) SpikyData = chart.addDataPanel();
      SpikyData.horizontalMargin = 5;
      SpikyData.verticalMargin = 5;
      SpikyData.horizontalPosition = DataPanel.HorizontalPosition.RIGHTBORDER;
      SpikyData.verticalPosition = DataPanel.VerticalPosition.BOTTOMBORDER;
      SpikyData.panelHrzAlign = DataPanel.HorizontalAlign.LEFTOF;
      SpikyData.panelVrtAlign = DataPanel.VerticalAlign.ABOVE;
      SpikyData.padding = 3;
      SpikyData.borderthickness = 1;
      SpikyData.borderColor = Color.Black;
      SpikyData.bgColor = Color.FromArgb(192, 255, 163, 0);
      SpikyData.font.color = Color.Black;
      SpikyData.text = "Data look a bit spiky,\nyou should try to filter them";
      SpikyMsgCount = 0;
      SpikyData.enabled = false;


      tempData.legend = "Temperature";
      tempData.color = Color.DarkRed;
      tempData.disabled = !showTemp.Checked;
      avgTempData.legend = "Avg.Temp.";
      avgTempData.color = Color.Red;
      avgTempData.disabled = !showAvg.Checked;
      deltaTempData.legend = "Delta.Temp.";
      deltaTempData.color = Color.DarkOrange;
      deltaTempData.disabled = !showDelta.Checked;
      rawWData.legend = "Zero Drift";
      rawWData.color = Color.DarkBlue;
      rawWData.yAxisIndex = 1;
      rawWData.disabled = !showRawW.Checked;
      predData.legend = "Predicted Drift";
      predData.color = Color.Green;
      predData.yAxisIndex = 1;
      predData.disabled = !showPrediction.Checked;
      errorData.legend = "Residual Error";
      errorData.color = Color.Black;
      errorData.yAxisIndex = 1;
      errorData.disabled = !showError.Checked;
      chart.AllowRedraw();

      FilterButton.TabIndex = tabIndex++;
      ExportResultsButton.TabIndex = tabIndex++;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;

      autoDelta_Click(null, null);
      enableUI(false);

    }

    private void LockYaxes_CheckedChanged(object sender, EventArgs e)
    {
      if (lockYaxes.Checked) { s0.lockMinMax(); s1.lockMinMax(); }
      else { s0.unlockMinMax(); s1.unlockMinMax(); }
    }

    private void ExportResultsButton_Click(object sender, EventArgs e)
    {
      if (ExportFileDialog1.ShowDialog() == DialogResult.OK)
      {
        string filename = ExportFileDialog1.FileName;
        string ext = Path.GetExtension(filename).ToUpper();
        switch (ext)
        {
          case ".CSV": resultsCSVExport(filename); break;
          case ".PNG": resultsPNGExport(filename); break;
          case ".SVG": resultsSVGExport(filename); break;

        }
      }
    }
    public void resultsCSVExport(string filename)
    {

      List<String> lines = new List<String>();
      lines.Add("Timestamp;Datetime;"
                + ChoosedWeighScale.tsensor.get_hardwareId() + ";"
                + ChoosedWeighScale.genericSensor.get_hardwareId() + ";"
                + "Average Temp;"
                + "Delta Temp;"
                + "Predicted Drift ;"
                + "Error");




      for (int i = 0; i < rawW.Length; i++)
      {
        string line = stamp[i].ToString() + ";" + constants.UnixTimeStampToDateTime(stamp[i]).ToString("yyyy/MM/dd HH:mm:ss.ff") + ";";
        line = line + temp[i].ToString() + ";"
                    + rawW[i].ToString() + ";"
                    + avgTemp[i].ToString() + ";"
                    + deltaTemp[i].ToString() + ";"
                    + pred[i].ToString() + ";"
                    + (rawW[i] - pred[i]).ToString();

        lines.Add(line);
      }

      StreamWriter sw = File.CreateText(filename);

      int n = 0;
      foreach (string line in lines)
      {
        n++;
        sw.WriteLine(line);

      }
      sw.Close();


    }
    public void resultsPNGExport(string filename)
    {
      chart.capture(YDataRenderer.CaptureType.PNG,
          YDataRenderer.CaptureTargets.ToFile,
          filename, YDataRenderer.CaptureFormats.Keep,
          90, 1200, 800);

    }
    public void resultsSVGExport(string filename)
    {
      chart.capture(YDataRenderer.CaptureType.SVG,
          YDataRenderer.CaptureTargets.ToFile,
          filename, YDataRenderer.CaptureFormats.Keep,
          90, 1200, 800);
    }

    private void FilterButton_Click(object sender, EventArgs e)
    {
      SpikyMsgCount++;
      rawW = FilterSpikes(rawW);
      chart.DisableRedraw();

      rawWData.clear();
      for (int i = 0; i < totalCount; i++)
      {
        rawWData.AddPoint(new pointXY() { x = stamp[i], y = rawW[i] });
      }
      chart.AllowRedraw();
      autoDelta_Click(null, null);
      enableUI(false);
    }

    private PanelDesc.WizardSteps ComputePanelNextClicked()
    {
      return PanelDesc.WizardSteps.CONFIRMCOMPENSATION;
    }


    /*****************************************************************************************
         * 
         *         Estimation of temperature change compensation parameters
         * 
         *****************************************************************************************/

    private void optimizeDeltaRate()
    {
      if (tempChangeWorker.IsBusy)
      {
        tempChangeWorker.CancelAsync();
      }
      else
      {
        if (learnCount == 0) return;
        prevProgress = -1;
        progressBar.Value = 0;
        progressBar.Visible = true;
        errorMessage.Visible = false;
        dialogTitle.Text = "Analyzing data...";
        CancelComputationButton.Text = "Abort";
        dialogPanel.Visible = true;
        disable_textChangeEvents = true;
        tempChangeWorker.RunWorkerAsync();
        autoDelta.Text = "Stop";
      }
    }

    private void tempChangeWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      double step, bestRatio, bestSqError;

      // Split the space in ten slices to find the initial guess (important for convergence)
      bRatio = 10.01;
      step = 1.0;
      this.recomputeDeltaTemp(false);
      bestRatio = bRatio;
      bestSqError = avgSqError;
      for (int depth = 0; depth <= 4; depth++)
      {
        for (int i = 0; i <= 10; i++)
        {
          worker.ReportProgress(20 * depth + 2 * i);
          this.recomputeDeltaTemp(false);
          if (avgSqError < bestSqError)
          {
            bestRatio = bRatio;
            bestSqError = avgSqError;
          }
          if (bRatio - step >= 0.001) bRatio -= step;
        }
        if (worker.CancellationPending)
        {
          bRatio = bestRatio;
          this.recomputeDeltaTemp(false);
          e.Cancel = true;
          return;
        }
        bRatio = bestRatio + step;
        step *= 0.2;
      }
      bRatio = bestRatio;
      this.recomputeDeltaTemp(false);
    }

    private void tempChangeWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      progressBar.Value = e.ProgressPercentage;
      deltaRatio.Text = (Math.Round(bRatio * 100000) / 100000).ToString();
      avgRatio.Text = (Math.Round(aRatio * 100000) / 100000).ToString();
      rmsError.Text = "Learning set: " + (Math.Round(Math.Sqrt(avgSqError) * 10000) / 10000).ToString();
      int val = (int)(Math.Round(-29.0 * Math.Log(Math.Abs(bRatio))));
      if (val >= 0 && val <= 200) deltaRatioTrackBar.Value = 100 - val;
      val = (int)(Math.Round(-29.0 * Math.Log(Math.Abs(aRatio))));
      if (val >= 0 && val <= 200) avgRatioTrackBar.Value = 100 - val;
    }

    private void tempChangeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {

      disable_textChangeEvents = false;
      dialogPanel.Visible = false;
      deltaRatio.Text = (Math.Round(bRatio * 100000) / 100000).ToString();
      int val = (int)(Math.Round(-29.0 * Math.Log(Math.Abs(bRatio))));
      if (val >= 0 && val <= 200) deltaRatioTrackBar.Value = 100 - val;
      autoDelta.Text = "Reset";
      enableUI(true);
    }

    public void recomputeDeltaTemp(bool refresh)
    {
      if (refresh) chart.DisableRedraw();
      if (refresh) deltaTempData.clear();

      double ratio = bRatio * 0.001;
      double compt = temp[0];
      for (int i = 1; i < totalCount; i++)
      {
        double dt = temp[i] - compt;
        compt += ratio * dt;
        deltaTemp[i] = dt;
        if (refresh) deltaTempData.AddPoint(new pointXY() { x = stamp[i], y = deltaTemp[i] });
      }
      this.estimateBeta();
      this.estimateAvgRate(refresh);
      this.recomputeAvgTemp(refresh);

      if (refresh) chart.AllowRedraw();
    }

    private void estimateBeta()
    {
      // Estimate deltaScale by assuming that deltaTemp derivative equals Zero Drift derivative
      // (so their sum of squares should be the same).
      // Derivatîve is estimated by comparing with 4 min. before
      double drawSq = 0, dtempSq = 0;
      int sameSign = 0;
      for (int i = 25; i < learnCount; i++)
      {
        double draw = rawW[i] - rawW[i - 24];
        double dtemp = deltaTemp[i] - deltaTemp[i - 24];
        drawSq += draw * draw;
        dtempSq += dtemp * dtemp;
        sameSign += ((draw < 0) == (dtemp < 0) ? 1 : -1);
      }
      double stdDev = Math.Sqrt(drawSq / dtempSq);
      beta = (sameSign < 0 ? -1 : 1) * stdDev;
    }


    /*****************************************************************************************
     * 
     *           Estimation of average temperature compensation parameters
     * 
     *****************************************************************************************/

    private void estimateAvgRate(bool refresh)
    {
      aRatio = bRatio;
      this.recomputeAvgTemp(false);
      double prevError = avgSqError;
      double step = Math.Pow(10, Math.Ceiling(Math.Log10(bRatio / 10)));
      while (step >= 0.0002)
      {
        for (int i = 0; i < 10 && step < aRatio; i++)
        {
          if (aRatio - step >= 0.001) aRatio -= step;
          this.recomputeAvgTemp(false);
          if (avgSqError > prevError)
          {
            aRatio += 2 * step;
            this.recomputeAvgTemp(false);
            prevError = avgSqError;
            break;
          }
          prevError = avgSqError;
        }
        step = step * 0.2;
      }
      if (refresh)
      {
        avgRatio.Text = (Math.Round(aRatio * 100000) / 100000).ToString();
        int val = (int)(Math.Round(-29.0 * Math.Log(Math.Abs(aRatio))));
        if (val >= 0 && val <= 200) avgRatioTrackBar.Value = 100 - val;
      }
      else
      {
        this.recomputeAvgTemp(false);
      }
    }

    public void recomputeAvgTemp(bool refresh)
    {
      if (refresh) chart.DisableRedraw();
      if (refresh) avgTempData.clear();

      double ratio = aRatio * 0.001;
      double compt = temp[0];
      for (int i = 1; i < totalCount; i++)
      {
        double dt = temp[i] - compt;
        compt += ratio * dt;
        avgTemp[i] = compt;
        if (refresh) avgTempData.AddPoint(new pointXY() { x = stamp[i], y = avgTemp[i] });
      }
      this.estimateAlpha();
      this.estimateOffset(refresh);

      if (refresh)
      {
        chart.AllowRedraw();
        enableUI(true);
      }
    }

    private void estimateAlpha()
    {
      // Estimate avgScale by assuming that non-constant part of the residual error is due to it
      // (so their variance should be the same).
      double ratio = aRatio * 0.001;
      double errSumSq = 0, errSum = 0;
      double tempSumSq = 0, tempSum = 0;
      int sameSign = 0;
      double compt = temp[0];
      for (int i = 0; i < 25; i++)
      {
        double dt = temp[i] - compt;
        compt += ratio * dt;
      }
      for (int i = 25; i < learnCount; i++)
      {
        double dt = temp[i] - compt;
        compt += ratio * dt;
        double err = rawW[i] - beta * deltaTemp[i];
        errSumSq += err * err;
        errSum += err;
        tempSumSq += compt * compt;
        tempSum += compt;
        sameSign += ((err < 0) == (compt < 0) ? 1 : -1);
      }
      double errSq = errSumSq / (learnCount - 25);
      double errAvg = errSum / (learnCount - 25);
      double tempSq = tempSumSq / (learnCount - 25);
      double tempAvg = tempSum / (learnCount - 25);
      double errStdDev = Math.Sqrt(errSq - errAvg * errAvg);
      double tempStdDev = Math.Sqrt(tempSq - tempAvg * tempAvg);
      alpha = (sameSign < 0 ? -1 : 1) * errStdDev / tempStdDev;
    }

    /*****************************************************************************************
     * 
     *          Computation of tare offset, RMS error and compensation table parameters
     * 
     *****************************************************************************************/

    private void estimateOffset(bool refresh)
    {
      // Estimate tare by minimizing RMS error for current parameters
      autoOffset = 0;

      this.recomputeError(false);
      double dOffset = 0;
      for (int i = 1; i < learnCount; i++)
      {
        dOffset += rawW[i] - pred[i];
      }
      autoOffset = dOffset / (learnCount - 1);
      if (refresh) updateCompensationTable();
      this.recomputeError(refresh);
    }

    public void recomputeError(bool refresh)
    {
      double sumSq = 0;

      for (int i = 1; i < learnCount; i++)
      {
        pred[i] = alpha * avgTemp[i] + beta * deltaTemp[i] + autoOffset;
        sumSq += (rawW[i] - pred[i]) * (rawW[i] - pred[i]);
      }
      avgSqError = sumSq / (learnCount - 1);

      if (refresh)
      {
        sumSq = 0;
        for (int i = learnCount; i < totalCount; i++)
        {
          pred[i] = alpha * avgTemp[i] + beta * deltaTemp[i] + autoOffset;
          sumSq += (rawW[i] - pred[i]) * (rawW[i] - pred[i]);
        }
        double chkSqError = sumSq / (totalCount - learnCount);

        rmsError.Text = "Learning set: " + (Math.Round(Math.Sqrt(avgSqError) * 10000) / 10000).ToString();
        rmsErrorChk.Text = "Testing set: " + (Math.Round(Math.Sqrt(chkSqError) * 10000) / 10000).ToString();
        chart.DisableRedraw();
        predData.clear();
        errorData.clear();
        if (deriv.Checked)
        {
          for (int i = 25; i < totalCount; i++)
          {
            predData.AddPoint(new pointXY() { x = stamp[i], y = pred[i] - pred[i - 24] });
          }
        }
        else
        {
          for (int i = 1; i < totalCount; i++)
          {
            predData.AddPoint(new pointXY() { x = stamp[i], y = pred[i] });
          }
        }
        for (int i = 1; i < totalCount; i++)
        {
          errorData.AddPoint(new pointXY() { x = stamp[i], y = rawW[i] - pred[i] });
        }
        chart.AllowRedraw();
      }
    }

    List<double> deviceTempCompensationValues = null;
    List<double> deviceOffsetAvgCompensationValues = null;
    List<double> deviceOffsetChgCompensationValues = null;

    private void updateCompensationTable()
    {
      deviceTempCompensationValues = new List<double>();
      deviceOffsetChgCompensationValues = new List<double>();
      deviceOffsetAvgCompensationValues = new List<double>();

      deviceTempCompensationValues.Add(-30);
      deviceTempCompensationValues.Add(0);
      deviceTempCompensationValues.Add(100);

      deviceOffsetChgCompensationValues.Add(Math.Round((0 + 30 * beta) * 1000) / 1000);
      deviceOffsetChgCompensationValues.Add(0);
      deviceOffsetChgCompensationValues.Add(Math.Round((0 - 100 * beta) * 1000) / 1000);

      deviceOffsetAvgCompensationValues.Add(Math.Round((-autoOffset + 30 * alpha) * 1000) / 1000);
      deviceOffsetAvgCompensationValues.Add(Math.Round(-autoOffset * 1000) / 1000);
      deviceOffsetAvgCompensationValues.Add(Math.Round((-autoOffset - 100 * alpha) * 1000) / 1000);

      label11.Text = deviceTempCompensationValues[0].ToString() + "°C";
      label13.Text = deviceTempCompensationValues[1].ToString() + "°C";
      label16.Text = deviceTempCompensationValues[2].ToString() + "°C";

      avgCompMin.Text = deviceOffsetAvgCompensationValues[0].ToString("0.000");
      avgCompZero.Text = deviceOffsetAvgCompensationValues[1].ToString("0.000");
      avgCompMax.Text = deviceOffsetAvgCompensationValues[2].ToString("0.000");
      deltaCompMin.Text = deviceOffsetChgCompensationValues[0].ToString("0.000");
      deltaCompZero.Text = deviceOffsetChgCompensationValues[1].ToString("0.000");
      deltaCompMax.Text = deviceOffsetChgCompensationValues[2].ToString("0.000");
    }

    /*****************************************************************************************
     * 
     *          Misc UI eventhandlers
     * 
     *****************************************************************************************/

    private void showTemp_CheckedChanged(object sender, EventArgs e)
    {
      tempData.disabled = !showTemp.Checked;
    }

    private void showRawW_CheckedChanged(object sender, EventArgs e)
    {
      rawWData.disabled = !showRawW.Checked;
    }

    private void showDelta_CheckedChanged(object sender, EventArgs e)
    {
      deltaTempData.disabled = !showDelta.Checked;
    }

    private void showAvg_CheckedChanged(object sender, EventArgs e)
    {
      avgTempData.disabled = !showAvg.Checked;
    }

    private void showPrediction_CheckedChanged(object sender, EventArgs e)
    {
      predData.disabled = !showPrediction.Checked;
    }

    private void showError_CheckedChanged(object sender, EventArgs e)
    {
      errorData.disabled = !showError.Checked;
    }

    private void deltaRatio_TextChanged(object sender, EventArgs e)
    {
      double newVal;
      if (disable_textChangeEvents) return;
      Double.TryParse(deltaRatio.Text, out newVal);
      if (newVal == 0) return;
      bRatio = newVal;
      int val = (int)(Math.Round(-29.0 * Math.Log(Math.Abs(bRatio))));
      if (val >= 0 && val <= 200) deltaRatioTrackBar.Value = 100 - val;
      this.recomputeDeltaTemp(true);
    }

    private void deltaRatioTrackBar_Scroll(object sender, EventArgs e)
    {
      int trackVal = deltaRatioTrackBar.Value;
      double absVal = 100 - trackVal;
      bRatio = Math.Round(Math.Exp(absVal / -29.0) * 100000) / 100000;
      deltaRatio.Text = bRatio.ToString();
    }

    private void avgRatio_TextChanged(object sender, EventArgs e)
    {
      double newVal;
      if (disable_textChangeEvents) return;
      Double.TryParse(avgRatio.Text, out newVal);
      if (newVal == 0) return;
      aRatio = newVal;
      int val = (int)(Math.Round(-29.0 * Math.Log(Math.Abs(aRatio))));
      if (val >= 0 && val <= 200) avgRatioTrackBar.Value = 100 - val;
      this.recomputeAvgTemp(true);
    }

    private void avgRatioTrackBar_Scroll(object sender, EventArgs e)
    {
      int trackVal = avgRatioTrackBar.Value;
      double absVal = 100 - trackVal;
      aRatio = Math.Round(Math.Exp(absVal / -29.0) * 100000) / 100000;
      avgRatio.Text = aRatio.ToString();
    }

    private void deriv_CheckedChanged(object sender, EventArgs e)
    {
      chart.DisableRedraw();
      rawWData.clear();
      if (deriv.Checked)
      {
        for (int i = 24; i < totalCount; i++)
        {
          rawWData.AddPoint(new pointXY() { x = stamp[i], y = rawW[i] - rawW[i - 24] });
        }
      }
      else
      {
        for (int i = 0; i < totalCount; i++)
        {
          rawWData.AddPoint(new pointXY() { x = stamp[i], y = rawW[i] });
        }

      }
      this.recomputeError(true);
      chart.AllowRedraw();
    }

    private void enableUI(bool state)
    {
      PrevButton.Enabled = state;
      autoDelta.Enabled = state;
      autoAvg.Enabled = state;
      deltaRatio.Enabled = state;
      avgRatio.Enabled = state;
      deltaRatioTrackBar.Enabled = state;
      avgRatioTrackBar.Enabled = state;
      showTemp.Enabled = state;
      showRawW.Enabled = state;
      showDelta.Enabled = state;
      showAvg.Enabled = state;
      deriv.Enabled = state;
      showError.Enabled = state;
      showPrediction.Enabled = state;
      lockYaxes.Enabled = state;

      if (state)
      {
        SpikyData.enabled = containsSpikes(rawW);
        if (SpikyMsgCount == 1) SpikyData.text = "Data are still spiky,\nyou should filter them again";
        else if (SpikyMsgCount == 2) SpikyData.text = "Still spiky,\nonce more?";
        else if (SpikyMsgCount > 2) SpikyData.text = "Ok, looks filter doesn't\nwork well on these data. :-(";
      }

    }

    private void autoDelta_Click(object sender, EventArgs e)
    {
      enableUI(false);
      this.optimizeDeltaRate();
    }

    private void autoAvg_Click(object sender, EventArgs e)
    {
      enableUI(false);
      chart.DisableRedraw();
      this.estimateAvgRate(true);
      this.recomputeAvgTemp(true);
      chart.AllowRedraw();
    }

    private void fileName_Click(object sender, EventArgs e)
    {
      openFileDialog1.ShowDialog();
    }



    private void CancelComputation_Click(object sender, EventArgs e)
    {
      if (tempChangeWorker.IsBusy)
      {
        tempChangeWorker.CancelAsync();
      }
      dialogPanel.Visible = false;
    }

    /*****************************************************************************************
    * 
    * Spikes filtering
    * 
    *****************************************************************************************/



    private double[] FilterSpikes(double[] data)
    {
      double sigma;
      double[] delta = Delta(data, out sigma);
      double limit = 20 * sigma;
      double[] newdata = new double[data.Length];
      for (int i = 0; i < data.Length; i++) newdata[i] = data[i];
      for (int i = 1; i < data.Length - 1; i++)
      {
        if (delta[i] > limit)
        {
          newdata[i] = (data[i - 1] + data[i + 1]) / 2;
        }
      }
      return newdata;
    }


    private bool containsSpikes(double[] data)
    {
      double sigma;
      double[] delta = Delta(data, out sigma);
      double limit = 20 * sigma;

      for (int i = 1; i < data.Length - 1; i++) if (delta[i] > limit)
          return true;


      return false;
    }




    /*****************************************************************************************
    * 
    * Gradient descent method (not used anymore, does not significantly improve quality)
    * 
    *****************************************************************************************/

    private void gradientDescent_StartStop()
    {
      if (gradientDescentWorker.IsBusy)
      {
        gradientDescentWorker.CancelAsync();
      }
      else
      {
        autoOffset = 0;

        // Setup initial tare to minimize RMS error for current parameters
        this.recomputeError(false);
        double dOffset = 0;
        for (int i = 1; i < learnCount; i++)
        {
          dOffset += rawW[i] - pred[i];
        }
        autoOffset = dOffset / (learnCount - 1);
        updateCompensationTable();
        this.recomputeError(true);

        prevProgress = -1;
        progressBar.Value = 0;
        progressBar.Visible = true;
        errorMessage.Visible = false;
        dialogTitle.Text = "Analyzing data...";
        CancelComputationButton.Text = "Abort";
        dialogPanel.Visible = true;
        gradientDescentWorker.RunWorkerAsync();
      }
    }

    private void gradientDescent_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      BackgroundWorker worker = sender as BackgroundWorker;
      double dOffset, dAvgScale, dDeltaScale;
      double descentFactor = learningRate / (learnCount - 1);
      int reportPeriod = nIterations / 256;

      // perform gradient descent on RMS error for deltaScale, avgScale and dOffset
      for (int j = 0; j < nIterations; j++)
      {
        if (worker.CancellationPending)
        {
          e.Cancel = true;
          break;
        }
        if (j % reportPeriod == 0)
        {
          worker.ReportProgress(j * 100 / nIterations);
        }
        this.recomputeError(false);
        dOffset = 0;
        dAvgScale = 0;
        dDeltaScale = 0;
        for (int i = 1; i < learnCount; i++)
        {
          dOffset += pred[i] - rawW[i];
          dAvgScale += (pred[i] - rawW[i]) * avgTemp[i];
          dDeltaScale += (pred[i] - rawW[i]) * deltaTemp[i];
        }
        autoOffset -= descentFactor * dOffset;
        alpha -= descentFactor * dAvgScale;
        beta -= descentFactor * dDeltaScale;
      }
    }

    private void gradientDescent_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (prevProgress != e.ProgressPercentage)
      {
        prevProgress = e.ProgressPercentage;
        progressBar.Value = e.ProgressPercentage;
        rmsError.Text = (Math.Round(Math.Sqrt(avgSqError) * 10000) / 10000).ToString();
        if ((e.ProgressPercentage % 10) == 0)
        {
          this.updateCompensationTable();
        }
      }
    }

    private void gradientDescent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      dialogPanel.Visible = false;
      this.updateCompensationTable();
      this.recomputeError(true);

      enableUI(true);
    }


    private double[] Delta(double[] data, out double sigma)
    {
      double[] delta = new double[data.Length];
      delta[0] = 0;
      delta[data.Length - 1] = 0;
      double sumSq = 0, sum = 0;

      for (int i = 1; i < data.Length - 1; i++)
      {
        double d = Math.Min(Math.Abs(data[i] - data[i - 1]), Math.Abs(data[i] - data[i + 1]));
        delta[i] = d;
        sumSq += d * d;
        sum += d;
      }
      double avg = sum / data.Length;
      double sigmaSq = sumSq / data.Length - avg * avg;
      sigma = Math.Sqrt(sigmaSq);
      return delta;
    }



  }
}