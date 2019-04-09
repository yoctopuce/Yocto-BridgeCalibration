using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using System.Windows.Forms;
using YDataRendering;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {

    private Label MonitorMessage1 = null;
    private PictureBox MonitorGraphContainer = null;
    private YGraph MonitorGraph = null;
    DataSerie monitor_wData = null;
    DataSerie monitor_tData = null;
    List<pointXY> WeightData = null;
    List<pointXY> TempData = null;
    DataPanel progressPanel = null;
    Button ExportButton;

    public void newGenericSensorData(YGenericSensor source, YMeasure data)
    {
      pointXY p = new pointXY() { x = data.get_endTimeUTC(), y = data.get_averageValue() };
      WeightData.Add(p);
      monitor_wData.AddPoint(p);
    }

    public void newTempSensorData(YTemperature source, YMeasure data)
    {
      pointXY p = new pointXY() { x = data.get_endTimeUTC(), y = data.get_averageValue() };
      TempData.Add(p);
      monitor_tData.AddPoint(p);
    }


    public void MonitorPanelResize()
    {
      MonitorMessage1.Size = new Size(ClientSize.Width - 80, 85);
      MonitorMessage1.Location = new Point((ClientSize.Width - MonitorMessage1.Width) / 2, 5);
      MonitorGraphContainer.Location = new Point(0, MonitorMessage1.Height + 5);
      MonitorGraphContainer.Size = new Size(ClientSize.Width, ClientSize.Height - MonitorMessage1.Height - 10 - NextButton.Height);
      ExportButton.Location = new Point(NextButton.Left - ExportButton.Width - 5, ClientSize.Height - NextButton.Height - 5);
    }


    public void MonitorPanelClearContents()
    {
      Controls.Remove(MonitorMessage1);
      Controls.Remove(MonitorGraphContainer);
      Controls.Remove(ExportButton);
      ChoosedWeighScale.genericSensor.registerTimedReportCallback(null);
      ChoosedWeighScale.tsensor.registerTimedReportCallback(null);
      MonitorGraph = null;
      ChoosedWeighScale.KillDataloggerLoad();
    }


    public void CSVExport(string filename)
    {
      double Currenttime = Double.MaxValue;
      if ((TempData.Count > 0) && (TempData[0].x < Currenttime)) Currenttime = TempData[0].x;
      if ((WeightData.Count > 0) && (WeightData[0].x < Currenttime)) Currenttime = WeightData[0].x;
      int tIndex = 0;
      int wIndex = 0;
      List<String> lines = new List<String>();
      lines.Add("Timestamp; Datetime; " + ChoosedWeighScale.tsensor.get_hardwareId() + ";" + ChoosedWeighScale.genericSensor.get_hardwareId());
      while ((tIndex < TempData.Count) && (wIndex < WeightData.Count))
      {
        string line = Currenttime.ToString() + ";" + constants.UnixTimeStampToDateTime(Currenttime).ToString("yyyy/MM/dd HH:mm:ss.ff") + ";";
        if ((tIndex < TempData.Count) && (Currenttime == TempData[tIndex].x)) { line += TempData[tIndex].y.ToString(); tIndex++; }
        line += ";";
        if ((wIndex < WeightData.Count) && (Currenttime == WeightData[wIndex].x)) { line += WeightData[wIndex].y.ToString(); wIndex++; }
        lines.Add(line);
        if (tIndex < TempData.Count) Currenttime = TempData[tIndex].x; else if (wIndex < WeightData.Count) Currenttime = WeightData[wIndex].x;
        if ((wIndex < WeightData.Count) && (Currenttime > WeightData[wIndex].x)) Currenttime = WeightData[wIndex].x;
      }
      StreamWriter sw = File.CreateText(filename);

      int n = 0;
      foreach (string line in lines)
      {
        n++;
        sw.WriteLine(line);
        // ((BackgroundWorker)sender).ReportProgress(((int)(50 + 50.0 * n / data.Count)));
      }
      sw.Close();


    }
    public void PNGExport(string filename)
    {
      MonitorGraph.capture(YDataRenderer.CaptureType.PNG,
          YDataRenderer.CaptureTargets.ToFile,
          filename, YDataRenderer.CaptureFormats.Keep,
          90, 1200, 800);

    }
    public void SVGExport(string filename)
    {
      MonitorGraph.capture(YDataRenderer.CaptureType.SVG,
          YDataRenderer.CaptureTargets.ToFile,
          filename, YDataRenderer.CaptureFormats.Keep,
          90, 1200, 800);
    }

    public void MonitorPanelDrawPanel()
    {
      int tabIndex = 0;

      SuspendLayout();
      //
      // WarningResetMessage1
      // 
      if (MonitorMessage1 == null) MonitorMessage1 = new System.Windows.Forms.Label();
      MonitorMessage1.Name = "MonitorMessage1";
      MonitorMessage1.AutoSize = false;
      MonitorMessage1.TabIndex = tabIndex++;
      MonitorMessage1.Font= new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      MonitorMessage1.Text = "Your device is gathering data at 0.1Hz, let the process run for several days to make sure that day/night temperature cycles are captured, then use this utility to analyze it and compute Temperature compensation. "
                       + (RadioBtMonitor.Checked? " Once datalogger is loaded, you can export data at any time. ":"") 
                       +"Click on next to go back to the welcome page. You can also close this application, the capture process will still run in background.";
                 
      

      MonitorMessage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      Controls.Add(MonitorMessage1);

      if (MonitorGraphContainer == null) MonitorGraphContainer = new PictureBox();
      MonitorGraphContainer.Name = "MonitorGraphContainer";
      MonitorGraphContainer.TabIndex = tabIndex++;
      Controls.Add(MonitorGraphContainer);

      // 
      // Export button
      // 
      if (ExportButton == null) ExportButton = new Button();
      this.ExportButton.Name = "ExportButton";
      this.ExportButton.AutoSize = false;
      this.ExportButton.Size = new Size(75, 23);
      this.ExportButton.TabIndex = tabIndex++;
      this.ExportButton.Text = "Export..";
      this.ExportButton.UseVisualStyleBackColor = true;

      this.ExportButton.Click += ExportButton_Click;
      ExportButton.Enabled = false;
      Controls.Add(ExportButton);


      ResumeLayout(false);
      NextButton.Visible = true;
      NextButton.Enabled = true;
      PrevButton.Visible = false;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;
      MonitorPanelResize();

      MonitorGraph = new YGraph(MonitorGraphContainer, null);

      MonitorGraph.DisableRedraw();

      YAxis wAxis = MonitorGraph.addYAxis();
      wAxis.showGrid = true;
      wAxis.visible = true;
      wAxis.legend.title = "Weight (" + ChoosedWeighScale.sensor.get_unit() + ")";
      wAxis.highlightZero = true;


      YAxis tAxis = MonitorGraph.addYAxis();
      tAxis.visible = true;
      tAxis.legend.title = "Temperature (" + ChoosedWeighScale.tsensor.get_unit() + ")";
      tAxis.position = YAxis.HrzPosition.RIGHT;




      MonitorGraph.navigator.enabled = true;
      MonitorGraph.navigator.relativeheight = 12;
      MonitorGraph.xAxis.showGrid = true;

      monitor_wData = MonitorGraph.addSerie();
      monitor_wData.yAxisIndex = wAxis.index;
      monitor_wData.color = Color.DarkBlue;
      wAxis.legend.font.color = monitor_wData.color;

      monitor_tData = MonitorGraph.addSerie();
      monitor_tData.yAxisIndex = tAxis.index;
      monitor_tData.color = Color.DarkRed;
      tAxis.legend.font.color = monitor_tData.color;


      progressPanel = MonitorGraph.addDataPanel();
      progressPanel.horizontalPosition = DataPanel.HorizontalPosition.RIGHTBORDER;
      progressPanel.verticalPosition = DataPanel.VerticalPosition.TOPBORDER;
      progressPanel.horizontalMargin = 5;
      progressPanel.verticalMargin = 5;
      progressPanel.panelHrzAlign = DataPanel.HorizontalAlign.LEFTOF;
      progressPanel.panelVrtAlign = DataPanel.VerticalAlign.BELOW;
      progressPanel.borderColor = Color.Black;
      progressPanel.borderthickness = 1;

      progressPanel.enabled = true;
  



      MonitorGraph.AllowRedraw();
      WeightData = new List<pointXY>();
      TempData = new List<pointXY>();

      ChoosedWeighScale.genericSensor.registerTimedReportCallback(newGenericSensorData);
      ChoosedWeighScale.tsensor.registerTimedReportCallback(newTempSensorData);

      //MonitorLoadProgress(0);
      ChoosedWeighScale.LoadDataLogger(MonitorLoadCompleted, MonitorLoadProgress, true);
    }

    private void ExportButton_Click(object sender, EventArgs e)
    {
      if (ExportFileDialog1.ShowDialog() == DialogResult.OK)
      {
        string filename = ExportFileDialog1.FileName;
        string ext = Path.GetExtension(filename).ToUpper();
        switch (ext)
        {
          case ".CSV": CSVExport(filename); break;
          case ".PNG": PNGExport(filename); break;
          case ".SVG": SVGExport(filename); break;

        }
      }

    }

    public void MonitorLoadProgress(int p)
    {
      progressPanel.text = "Loading Datalogger " + p.ToString() + "%";
      if (p == 100)
      {
        progressPanel.enabled = false;
        ExportButton.Enabled = true;

      }
      else
      {
        progressPanel.enabled = true;
        ExportButton.Enabled = false;

      }
    }



    public void MonitorLoadCompleted(string error, List<pointXY> wdata, List<pointXY> tdata)
    {

      if (error != "")
      {
        progressPanel.text = "Data load error: " + error;
        return;
      }

      LoadCompleted(monitor_tData, ref TempData, tdata);
      LoadCompleted(monitor_wData, ref WeightData, wdata);
    }


    public void LoadCompleted(DataSerie serie, ref List<pointXY> liveData, List<pointXY> dataLoggerData)
    {
      if (dataLoggerData.Count <= 0) return;

      if (liveData.Count > 0)
      {
        double lastTimeStamp = dataLoggerData[dataLoggerData.Count - 1].x;
        int n = 0;
        while ((n < liveData.Count) && (lastTimeStamp >= liveData[n].x)) n++;
        if (n > 0) liveData.RemoveRange(0, n);
      }

      liveData = (List<pointXY>)dataLoggerData.Concat(liveData).ToList();


      MonitorGraph.DisableRedraw();
      serie.clear();
      serie.InsertPoints(liveData.ToArray());
      MonitorGraph.AllowRedraw();
    }


    private PanelDesc.WizardSteps MonitorPanelNextClicked()
    {

      history.Clear();
      return PanelDesc.WizardSteps.WELCOME;
    }

  }
}