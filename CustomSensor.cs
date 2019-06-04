using System;
using System.Collections.Generic;
using System.ComponentModel;

using YDataRendering;

namespace Yocto_BridgeCalibration
{





  class CustomSensor
  {
    public delegate void DataLoadCompletedCallback(string Error, List<pointXY> tdata, List<pointXY> wdata);
    public delegate void SensorLoadCompletedCallback(string Error, List<pointXY> data);


    public delegate void DataLoadProgressCallback(int progress);

    YWeighScale _sourceSensor;
    String _name = "";
    String _serial = "";
   
    YDataLogger _datalogger;
    YTemperature _tempSensor;
    YGenericSensor _genSensor;
    dataLoader GenSensorDataloader  =null;
    dataLoader TempSensorDataloader  =null;
    DataLoadCompletedCallback _DataloadingCompletedCallback;
    DataLoadProgressCallback _DataloadingProgressCallback;
    bool _PlzNotifyWhenPreloapCompled = false;


    string _unit = "";


    public void reloadConfig(YModule Source)
    {
      _name = _sourceSensor.get_friendlyName(); ;
      _serial = _sourceSensor.get_module().get_module().get_serialNumber();
      _unit = _sourceSensor.get_unit();
      _datalogger = YDataLogger.FindDataLogger(_serial + ".dataLogger");
      _tempSensor = YTemperature.FindTemperature(_serial + ".temperature");
      string tt = _sourceSensor.get_hardwareId().Replace("weighScale", "genericSensor");
      _genSensor = YGenericSensor.FindGenericSensor(tt);


    }

    public void setCompensation(double tempChgRatio, double tempAvgRatio,
      List<double> deviceTempCompensationValues,
      List<double> deviceOffsetChgCompensationValues,
      List<double> deviceOffsetAvgCompensationValues)
    {
      _sourceSensor.set_tempChgAdaptRatio(tempChgRatio);
      _sourceSensor.set_tempAvgAdaptRatio(tempAvgRatio);
      _sourceSensor.set_offsetChgCompensationTable(deviceTempCompensationValues, deviceOffsetChgCompensationValues);
      _sourceSensor.set_offsetAvgCompensationTable(deviceTempCompensationValues,deviceOffsetAvgCompensationValues);
      _sourceSensor.get_module().saveToFlash();
    }

    public CustomSensor(YWeighScale sourceSensor)
    {
      _sourceSensor = sourceSensor;
      reloadConfig(null);
      _sourceSensor.get_module().registerConfigChangeCallback(reloadConfig);
    }

    public void saveconfig()
    {
      _sourceSensor.get_module().saveToFlash();
    }

    public override string ToString()
    {
      return _name;
    }

    public string serial { get { return _serial; } }

    public YWeighScale sensor { get { return _sourceSensor; } }
    public YTemperature tsensor { get { return _tempSensor; } }
    public YGenericSensor genericSensor { get { return _genSensor; } }

    public YDataLogger datalogger { get { return _datalogger; } }

    public string unit { get { return _unit; } }


    public int TempSensorType
    {
      get
      {
        int t = _tempSensor.get_sensorType();
        if (t == YTemperature.SENSORTYPE_RES_INTERNAL) return 0;

        if (t == YTemperature.SENSORTYPE_RES_NTC)
        {

          List<Double> NTCtableTemp = new List<Double>();
          List<Double> NTCtableValue = new List<Double>();
          _tempSensor.loadThermistorResponseTable(NTCtableTemp, NTCtableValue);
          if ((NTCtableTemp.Count == 2) && (NTCtableTemp[0] == 25))
          {
            double t0 = NTCtableTemp[0] + 275.15;
            double t1 = NTCtableTemp[1] + 275.15;
            double beta = Math.Log(NTCtableValue[0] / NTCtableValue[1]) * (t0 * t1) / (t1 - t0);
            if ((beta < 3390) && (beta > 3370) && (NTCtableValue[0] == 10000)) return 1;
          }
        }

        return 2;

      }
    }


    List<pointXY> _gendata;
    private void genPreloadcompleted(string error, List<pointXY> data)
    {
      if (error != "")
      {
        _DataloadingCompletedCallback(error, null, null);
        return;
      }
      _gendata = data;
      TempSensorDataloader.preload(tempPreloadcompleted);
    }

    private void tempPreloadcompleted(string error, List<pointXY> data)
    {
      if (error != "")
      {
        _DataloadingCompletedCallback(error, null, null);
        return;
      }

      if (_PlzNotifyWhenPreloapCompled) _DataloadingCompletedCallback(error, _gendata, data);


      GenSensorDataloader.load(genProgressCallback, genLoadcompleted);

    }

    private void genProgressCallback(int progress)
    {
      _DataloadingProgressCallback(progress / 2);
    }


    private void tempProgressCallback(int progress)
    {
      _DataloadingProgressCallback(50 + progress / 2);
    }

    private void genLoadcompleted(string error, List<pointXY> data)
    {
      if (error == "")
      {
        _gendata = data;
        TempSensorDataloader.load(tempProgressCallback, tempLoadcompleted);
      }
      else _DataloadingCompletedCallback(error, null, null);

    }

    private void tempLoadcompleted(string error, List<pointXY> data)
    {
      if (error == "")
      {
        _DataloadingCompletedCallback(error, _gendata, data);
      }
      else _DataloadingCompletedCallback(error, null, null);



    }


    public void KillDataloggerLoad()
    {
      if  (GenSensorDataloader!=null) GenSensorDataloader.killLoad();
      if (TempSensorDataloader != null)  TempSensorDataloader.killLoad();

    }

    public void LoadDataLogger(DataLoadCompletedCallback CompletedCallback,
                               DataLoadProgressCallback ProgressCallback,
                               bool PlzNotifyWhenPreloapCompled)
    {
      _PlzNotifyWhenPreloapCompled = PlzNotifyWhenPreloapCompled;
      _DataloadingCompletedCallback = CompletedCallback;
      _DataloadingProgressCallback = ProgressCallback;
     
      GenSensorDataloader = new dataLoader(_genSensor);
      TempSensorDataloader = new dataLoader(_tempSensor);

      GenSensorDataloader.preload(genPreloadcompleted);




    }

  }

  public class TimedSensorValue
  {
    public double DateTime { get; set; }
    public double Value { get; set; }
  }

  class dataLoader
  {
    YSensor _sensor = null;
    BackgroundWorker predloadProcess = null;
    BackgroundWorker loadProcess = null;
    YDataSet recordedData = null;
    int recordedDataLoadProgress = 0;
    int globalDataLoadProgress = 0;
    bool _mustStopNow = false;
    public List<pointXY> previewCurData;
    public List<pointXY> loadedData = new List<pointXY>();
    string _hwdName = "";


    CustomSensor.SensorLoadCompletedCallback _completedCallback = null;
    CustomSensor.DataLoadProgressCallback _progressCallback = null;

    protected void load_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (_progressCallback != null) _progressCallback(e.ProgressPercentage);
    }

    public void killLoad()
    {
      _mustStopNow = true;

    }


    public dataLoader(YSensor sensor)
    {
      _sensor = sensor;
      _hwdName = _sensor.get_hardwareId();
      _progressCallback = null;
      _mustStopNow = false;


    }

    public void preload(
                        CustomSensor.SensorLoadCompletedCallback preloadCompletedCallback)
    {

      if (_mustStopNow) return;

      _completedCallback = preloadCompletedCallback;

      predloadProcess = new BackgroundWorker();
      predloadProcess.WorkerReportsProgress = true;
      predloadProcess.DoWork += new DoWorkEventHandler(preload_DoWork);
      predloadProcess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(preload_Completed);
      predloadProcess.ProgressChanged += new ProgressChangedEventHandler(load_ProgressChanged);
      predloadProcess.RunWorkerAsync(null);


    }

    public void load(CustomSensor.DataLoadProgressCallback progressCallback,
                      CustomSensor.SensorLoadCompletedCallback completedCallback)
    {
      if (_mustStopNow) return;

      _completedCallback = completedCallback;
      _progressCallback = progressCallback;

      loadProcess = new BackgroundWorker();
      loadProcess.WorkerReportsProgress = true;
      loadProcess.DoWork += new DoWorkEventHandler(load_DoWork);
      loadProcess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(load_Completed);
      loadProcess.ProgressChanged += new ProgressChangedEventHandler(load_ProgressChanged);
      loadProcess.RunWorkerAsync(null);

    }






    protected void preload_DoWork(object sender, DoWorkEventArgs e)
    {
      if (_mustStopNow) return;

      recordedData = _sensor.get_recordedData(0, 0);

      try
      {
        recordedDataLoadProgress = recordedData.loadMore();
      }
      catch (Exception ex) { LogManager.Log(_hwdName + ": preload more caused an exception " + ex.ToString()); }


      globalDataLoadProgress = recordedDataLoadProgress;
      ((BackgroundWorker)sender).ReportProgress(recordedDataLoadProgress);
      List<YMeasure> measures = recordedData.get_preview();
      previewCurData = new List<pointXY>();


      int startIndex = 0;

      for (int i = startIndex; i < measures.Count; i++)
      {
        double t = measures[i].get_endTimeUTC();
        previewCurData.Add(new pointXY() { x = t, y = measures[i].get_averageValue() });


      }

    }

    protected void preload_Completed(object sender, RunWorkerCompletedEventArgs e)
    {
      if (_mustStopNow) return;

      if (e.Error != null)
      {
        _completedCallback(e.Error.Message, null);
        return;
      }

      if (previewCurData == null) return;
      if (previewCurData.Count <= 0) return;
      LogManager.Log(_hwdName + " : datalogger preloading completed (" + previewCurData.Count + " rows )");
      _completedCallback("", previewCurData);


    }


    protected void load_DoWork(object sender, DoWorkEventArgs e)
    {

      int currentProgress = 0;

      while ((recordedDataLoadProgress < 100) && (!_mustStopNow))
      {
        try
        {
          recordedDataLoadProgress = recordedData.loadMore();
        }
        catch (Exception ex) { LogManager.Log(_hwdName + ": load more caused an exception " + ex.ToString()); }

        if (currentProgress != (int)(recordedDataLoadProgress))
        {

          currentProgress = (int)(recordedDataLoadProgress);
          LogManager.Log(_hwdName + ": " + currentProgress.ToString() + "% loaded");
          ((BackgroundWorker)sender).ReportProgress(currentProgress);
        }
      }

      List<YMeasure> measures = recordedData.get_measures();
      loadedData = new List<pointXY>();

      for (int i = 0; i < measures.Count; i++)
      {
        double t = measures[i].get_endTimeUTC();
        loadedData.Add(new pointXY { x = t, y = measures[i].get_averageValue() });

      }


    }

    protected void load_Completed(object sender, RunWorkerCompletedEventArgs e)
    {
      if (_mustStopNow) return;
      if (e.Error != null)
      {
        _completedCallback(e.Error.Message, null);
        return;
      }



      if (loadedData == null) return;
      if (loadedData.Count <= 0) return;
      _completedCallback("", loadedData);

    }
  }
}

