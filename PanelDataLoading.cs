using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

using System.Windows.Forms;
using YDataRendering;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {
    private Label DataloadingMessage = null;
    private ProgressBar DataLoadProgressBar1 = null;
    private Label DataloadingErrorMessage = null;

    List<double> Loadedstamp = null;
    List<double> Loadedtemp = null;
    List<double> LoadedrawW = null;

    BackgroundWorker CSVDataloadProcess = null;




    public void LoadingDataPanelResize()
    {
      DataloadingMessage.Location = new System.Drawing.Point(0, ClientSize.Height / 2 - 60);
      DataloadingMessage.Size = new System.Drawing.Size(ClientSize.Width, 30);
      DataLoadProgressBar1.Location = new System.Drawing.Point((ClientSize.Width - DataLoadProgressBar1.Width) / 2, ClientSize.Height / 2);

      DataloadingErrorMessage.Location = new System.Drawing.Point(0, ClientSize.Height / 2 + 30);
      DataloadingErrorMessage.Size = new System.Drawing.Size(ClientSize.Width, 30);

    }

    public void LoadingDataPanelClearContents()
    {
      Controls.Remove(DataloadingMessage);
      Controls.Remove(DataLoadProgressBar1);
      Controls.Remove(DataloadingErrorMessage);
      ChoosedWeighScale.KillDataloggerLoad();


    }

    public void LoadingDataDrawPanel()
    {
      int tabIndex = 0;
      SuspendLayout();

      //
      // ChooseDataSourceMessage
      // 
      if (DataloadingMessage == null) DataloadingMessage = new System.Windows.Forms.Label();
      DataloadingMessage.Name = "DataloadingMessage";
      DataloadingMessage.AutoSize = false;
      DataloadingMessage.TabIndex = tabIndex++;
      DataloadingMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      DataloadingMessage.Text = "Loading data, please wait...";
      DataloadingMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(DataloadingMessage);


      //
      // DataLoadProgressBar1
      // 
      if (DataLoadProgressBar1 == null) DataLoadProgressBar1 = new ProgressBar();
      DataLoadProgressBar1.Location = new System.Drawing.Point(192, 180);
      DataLoadProgressBar1.Name = "DataLoadProgressBar1";
      DataLoadProgressBar1.Size = new System.Drawing.Size(235, 23);
      DataLoadProgressBar1.TabIndex = tabIndex++;
      DataLoadProgressBar1.Value = 0;
      Controls.Add(DataLoadProgressBar1);

      //
      // DataLoadProgressBar1
      //
      if (DataloadingErrorMessage == null) DataloadingErrorMessage = new System.Windows.Forms.Label();
      DataloadingErrorMessage.Name = "DataloadingErrorMessage";
      DataloadingErrorMessage.AutoSize = false;
      DataloadingErrorMessage.TabIndex = tabIndex++;
      DataloadingErrorMessage.Text = "";
      DataloadingErrorMessage.Visible = false;

      DataloadingErrorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(DataloadingErrorMessage);
      LoadingDataPanelResize();

      ResumeLayout(false);
      NextButton.Visible = true;
      NextButton.Enabled = false;
      PrevButton.Visible = true;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;


      if (FromFileRadioButton.Checked)
      {
        DataloadingMessage.Text = "Loading data from file, please wait...";
        CSVDataloadProcess = new BackgroundWorker();
        CSVDataloadProcess.WorkerReportsProgress = true;
        CSVDataloadProcess.DoWork += new DoWorkEventHandler(CSVDataloadProcess_DoWork);
        CSVDataloadProcess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CSVDataloadProcess_completed);
        CSVDataloadProcess.ProgressChanged += new ProgressChangedEventHandler(CSVDataloadProcess_ProgressChanged);
        CSVDataloadProcess.RunWorkerAsync(null);
      }
      else
      if (FromDeviceRadioButton.Checked)
      {
       
        DataloadingMessage.Text = "Loading data from device, please wait...";
        ChoosedWeighScale.LoadDataLogger(DataloggerLoadCompleted,

                                         DataloggerLoadProgress, false);

      }


    }

    public void DataloggerLoadProgress(int progress)
    {

      DataLoadProgressBar1.Value = progress;
    }


    void DataloggerError(string error)
    {
      DataloadingMessage.Text = "Data loading error";
      DataloadingErrorMessage.Text = error;
      DataloadingErrorMessage.Visible = true;
    }

    public void DataloggerLoadCompleted(string error, List<pointXY> wdata, List<pointXY> tdata)
    {

      if (error != "") { DataloggerError(error); return; }


      if (tdata.Count == 0) { DataloggerError("No temperature data"); return; }
      if (wdata.Count == 0) { DataloggerError("No weight data"); return; }



      if (Math.Abs(wdata.Count - tdata.Count)>10)
        { DataloggerError("Temperature and Weight record count difference is more than 10 records"); return; }
      else
        { if (wdata.Count != tdata.Count)
           { while ((wdata.Count > 0) && (tdata.Count > 0) && (wdata[wdata.Count - 1].x > tdata[tdata.Count - 1].x)) wdata.RemoveAt(wdata.Count - 1);
             while ((wdata.Count > 0) && (tdata.Count > 0) && (tdata[tdata.Count - 1].x > wdata[wdata.Count - 1].x)) tdata.RemoveAt(tdata.Count - 1);
        }

      }

      if (wdata[0].x != tdata[0].x) { DataloggerError("Temperature and Weight records don't start at the same time"); return; }


      DataloadingMessage.Text = "Data loading completed";


      Loadedstamp = new List<double>();
      Loadedtemp = new List<double>();
      LoadedrawW = new List<double>();
      for (int i = 0; i < wdata.Count; i++)
      {
        Loadedstamp.Add(wdata[i].x);
        LoadedrawW.Add(wdata[i].y);
        Loadedtemp.Add(tdata[i].y);
      }


      checkdata();


    }





    protected void CSVDataloadProcess_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      DataLoadProgressBar1.Value = e.ProgressPercentage;
    }

    public void CSVDataloadProcess_DoWork(object sender, DoWorkEventArgs e)
    {
      string Filename = FilenameInput.Text;
      FileInfo fileInfo = new FileInfo(Filename);
      DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      CultureInfo culture = CultureInfo.InvariantCulture;
      int progress = 0;

      Loadedstamp = new List<double>();
      Loadedtemp = new List<double>();
      LoadedrawW = new List<double>();

      StreamReader reader = new StreamReader(Filename);
      long filesize = fileInfo.Length;
      long totalread = 0;

      string line = reader.ReadLine();
      if (line == null) throw new FileLoadException("CSV file is empty");
      int LineNumber = 1;
      string[] values = line.Split(';');
      if (values.Length < 4) throw new FileLoadException("CSV file is supposed to have at least 4 columns");

      if ((values[0]) != "Timestamp") throw new FileLoadException("1rst column name is supposed to be \"Timestamp\"");
      if (!values[2].Contains("temperature")) throw new FileLoadException("3rdt column name is supposed to contain \"Temperature\" ");
      if (!values[3].Contains("genericSensor")) throw new FileLoadException("4th column name is supposed to contain \"GenericSensor\" ");

      while (!reader.EndOfStream)
      {
        totalread += line.Length + 1;
        int currentProgress = (int)(100 * totalread / filesize);
        if (currentProgress != progress)
        {
          ((BackgroundWorker)sender).ReportProgress(currentProgress);
          progress = currentProgress;
        }

        line = reader.ReadLine();
        LineNumber++;
        values = line.Split(';');
        if ((values.Length >= 4) && (values[0] != "") && (values[2] != "") && (values[3] != ""))
          try
          {
            Loadedstamp.Add(double.Parse(values[0], culture));
            Loadedtemp.Add(float.Parse(values[2], culture));
            LoadedrawW.Add(float.Parse(values[3], culture));
          }
          catch (Exception ex)
          {
            throw new FileLoadException("Parse error at line " + LineNumber + "(" + ex.Message + ")");
          }
      }




    }


    public void CSVDataloadProcess_completed(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Error != null)
      {
        DataloadingMessage.Text = "Data loading error";
        DataloadingErrorMessage.Text = e.Error.Message;
        DataloadingErrorMessage.Visible = true;
        return;
      }
      DataLoadProgressBar1.Value = 100;
      DataloadingMessage.Text = "Data loading completed";
      checkdata();
    }

    public void checkdata()
    {


      if (Loadedstamp.Count < 100)
      {
        DataloadingErrorMessage.Text = "Data cannot be used: Not enough data points (" + Loadedstamp.Count + "), you need at least 100";
        DataloadingErrorMessage.Visible = true;
        return;
      }

      double period = (Loadedstamp[Loadedstamp.Count - 1] - Loadedstamp[0]) / (Loadedstamp.Count - 1);



      if ((period < 9.9) || (period > 10.1))
      {
        DataloadingErrorMessage.Text = "Data cannot be used: Invalid period (" + period.ToString("0.0") + "s), should be 10 seconds.";
        DataloadingErrorMessage.Visible = true;
        return;
      }



      NextButton.Enabled = true;

      NextButton_Click(null, null);



    }



    private PanelDesc.WizardSteps LoadingDataPanelNextClicked()
    {


      return PanelDesc.WizardSteps.DOYOURTHING;
    }


  }
}