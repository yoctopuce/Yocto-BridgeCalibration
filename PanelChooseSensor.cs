using System;

using System.Windows.Forms;

namespace Yocto_BridgeCalibration
{
  partial class Form1
  {

    private Label ChooseSensorMainMenuTitle = null;
    private Label ChooseSensorErrorLabel = null;
    private RadioButton RadioBtUSB = null;
    private RadioButton RadioBtVirtualHub = null;
    private RadioButton RadioBtRemote = null;
    private TextBox DeviceAddr = null;
    private Label ChooseSensorTitle = null;
    private ComboBox SensorChooser = null;

    private CustomSensor ChoosedWeighScale = null;



    enum ConnectionType { USB, LOCALVIRTUALHUB, IP, NONE };

    ConnectionType currentConnectionType = ConnectionType.NONE;
    String CurrentConnectionAddr = "";
    Timer AddrTimer;



    public void ChooseSensorPanelResize()
    {
      ChooseDataSourceTitleLabel.Size = new System.Drawing.Size(600, 75);
      ChooseDataSourceTitleLabel.Location = new System.Drawing.Point((ClientSize.Width - ChooseDataSourceTitleLabel.Size.Width) / 2, ClientSize.Height / 2 - 200);

      ChooseSensorMainMenuTitle.Location = new System.Drawing.Point(0, ChooseDataSourceTitleLabel.Location.Y + ChooseDataSourceTitleLabel.Size.Height+30);
      ChooseSensorMainMenuTitle.Size = new System.Drawing.Size(ClientSize.Width, 25);

      int w = RadioBtRemote.Width + DeviceAddr.Width + 5;
      if (RadioBtVirtualHub.Width > w) w = RadioBtVirtualHub.Width;
      if (RadioBtUSB.Width > w) w = RadioBtUSB.Width;



      RadioBtUSB.Location = new System.Drawing.Point((ClientSize.Width - w) / 2, ChooseSensorMainMenuTitle.Top + 45);
      RadioBtVirtualHub.Location = new System.Drawing.Point((ClientSize.Width - w) / 2, RadioBtUSB.Top + 30);
      RadioBtRemote.Location = new System.Drawing.Point((ClientSize.Width - w) / 2, RadioBtVirtualHub.Top + 30);
      DeviceAddr.Location = new System.Drawing.Point(RadioBtRemote.Left + RadioBtRemote.Width + 5, RadioBtRemote.Top);
      ChooseSensorErrorLabel.Location = new System.Drawing.Point(0, DeviceAddr.Top + 30);
      ChooseSensorErrorLabel.Size = new System.Drawing.Size(ClientSize.Width, 40);
      ChooseSensorTitle.Location = new System.Drawing.Point(0, ChooseSensorErrorLabel.Top + 40);
      ChooseSensorTitle.Size = new System.Drawing.Size(ClientSize.Width, 17);
      SensorChooser.Location = new System.Drawing.Point((ClientSize.Width - SensorChooser.Width) / 2, ChooseSensorTitle.Top + 40);
      AddrTimer = new Timer();

      AddrTimer.Tick += AddrTimer_Tick;

      AddrTimer.Enabled = false;
      AddrTimer.Interval = 1500;

    }



    public void ChooseSensorClearContents()
    {
      Controls.Remove(ChooseSensorMainMenuTitle);
      Controls.Remove(RadioBtUSB);
      Controls.Remove(RadioBtVirtualHub);
      Controls.Remove(RadioBtRemote);
      Controls.Remove(DeviceAddr);
      Controls.Remove(ChooseSensorTitle);
      Controls.Remove(SensorChooser);
      Controls.Remove(ChooseSensorErrorLabel);
      Controls.Remove(ChooseDataSourceTitleLabel);

    }

    public void ChooseSensorDrawPanel()
    {
      int tabIndex = 0;
      SuspendLayout();

      string intro = "";
      if  (RadioBtAccumulate.Checked)  intro = "You chose to calibrate a sensor and accumulate data for temperature compensation ";
      if (RadioBtMonitor.Checked) intro = "You chose to Monitor data accumulation ";
      if (RadioBtCalibrate.Checked) intro = "You chose to compute temperature compensation from accumulated data ";

      // 
      // ChooseDataSourceTitleLabel
      // 
      if (ChooseDataSourceTitleLabel == null) ChooseDataSourceTitleLabel = new System.Windows.Forms.Label();
     
      ChooseDataSourceTitleLabel.Name = "ChooseDataSourceTitleLabel";
      ChooseDataSourceTitleLabel.AutoSize = false;
      ChooseDataSourceTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      ChooseDataSourceTitleLabel.TabIndex = tabIndex++;
      ChooseDataSourceTitleLabel.Text = intro+"but first the application needs to connect to your device. This can be done through USB, a Local VirtualHub, or even a remote connection to a remote VirtualHub or a YoctoHub. ";
      Controls.Add(ChooseDataSourceTitleLabel);


      // MainMenuTitle
      // 
      if (ChooseSensorMainMenuTitle == null) ChooseSensorMainMenuTitle = new System.Windows.Forms.Label();
      ChooseSensorMainMenuTitle.Name = "MainMenuTitle";
      ChooseSensorMainMenuTitle.AutoSize = false;
      ChooseSensorMainMenuTitle.TabIndex = tabIndex++;
      ChooseSensorMainMenuTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

      ChooseSensorMainMenuTitle.Text = "Please indicate how the Yocto-Bridge device can be reached";
      ChooseSensorMainMenuTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(ChooseSensorMainMenuTitle);
      // 
      // RadioBtUSB
      // 
      if (RadioBtUSB == null) RadioBtUSB = new System.Windows.Forms.RadioButton();
      RadioBtUSB.AutoSize = true;
      RadioBtUSB.Name = "RadioBtUSB";
      RadioBtUSB.Size = new System.Drawing.Size(171, 17);
      RadioBtUSB.TabIndex = tabIndex++;
      RadioBtUSB.TabStop = true;
      RadioBtUSB.Text = "Through Local USB";
      RadioBtUSB.UseVisualStyleBackColor = true;
      RadioBtUSB.Click += RadioBtUSB_Click;
      Controls.Add(RadioBtUSB);
      // 
      // RadioBtVirtualHub
      // 
      if (RadioBtVirtualHub == null) RadioBtVirtualHub = new System.Windows.Forms.RadioButton();
      RadioBtVirtualHub.AutoSize = true;
      // RadioBtVirtualHub.Checked = currentConnectionType == ConnectionType.LOCALVIRTUALHUB;
      RadioBtVirtualHub.Name = "RadioBtVirtualHub";
      RadioBtVirtualHub.Size = new System.Drawing.Size(251, 17);
      RadioBtVirtualHub.TabIndex = tabIndex++;
      RadioBtVirtualHub.TabStop = true;
      RadioBtVirtualHub.Text = "Through Local VirtualHub (127.0.0.1)";
      RadioBtVirtualHub.UseVisualStyleBackColor = true;
      RadioBtVirtualHub.Click += RadioBtVirtualHub_Click;
      Controls.Add(RadioBtVirtualHub);
      // 
      // RadioBtRemote
      // 
      if (RadioBtRemote == null) RadioBtRemote = new System.Windows.Forms.RadioButton();
      RadioBtRemote.AutoSize = true;
      //RadioBtRemote.Checked = currentConnectionType == ConnectionType.IP;
      RadioBtRemote.Name = "RadioBtRemote";
      RadioBtRemote.Size = new System.Drawing.Size(220, 17);
      RadioBtRemote.TabIndex = tabIndex++;
      RadioBtRemote.TabStop = true;
      RadioBtRemote.Text = "Through remote connection, ip address is ";
      RadioBtRemote.AutoSize = true;
      RadioBtRemote.UseVisualStyleBackColor = true;
      RadioBtRemote.Click += RadioBtRemote_Click;
      RadioBtRemote.CheckedChanged += RadioBtRemote_CheckedChanged;
      Controls.Add(RadioBtRemote);
      // 
      // DeviceAddr
      // 
      if (DeviceAddr == null) DeviceAddr = new System.Windows.Forms.TextBox();
      DeviceAddr.Name = "DeviceAddr";
      DeviceAddr.Size = new System.Drawing.Size(100, 20);
      DeviceAddr.Text = CurrentConnectionAddr;
      DeviceAddr.TabIndex = tabIndex++;
      DeviceAddr.TextChanged += DeviceAddr_TextChanged;
      DeviceAddr.Enabled = currentConnectionType == ConnectionType.IP;
      Controls.Add(DeviceAddr);
      //
      // ChooseSensorErrorLabel
      // 
      if (ChooseSensorErrorLabel == null) ChooseSensorErrorLabel = new System.Windows.Forms.Label();
      ChooseSensorErrorLabel.Name = "ChooseSensorErrorLabel";
      ChooseSensorErrorLabel.AutoSize = false;
      ChooseSensorErrorLabel.TabIndex = tabIndex++;
      ChooseSensorErrorLabel.Text = "";
      ChooseSensorErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(ChooseSensorErrorLabel);
      //
      // ChooseSensorTitle
      // 
      if (ChooseSensorTitle == null)
      {
        ChooseSensorTitle = new System.Windows.Forms.Label();
        ChooseSensorTitle.Text = "No WeighScale sensor found so far";
      }
      ChooseSensorTitle.Name = "ChooseSensorTitle";
      ChooseSensorTitle.AutoSize = false;
      ChooseSensorTitle.TabIndex = tabIndex++;
      
      ChooseSensorTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      Controls.Add(ChooseSensorTitle);
      //
      // SensorChooser
      //
      if (SensorChooser == null) SensorChooser = new System.Windows.Forms.ComboBox();
      SensorChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      SensorChooser.FormattingEnabled = true;
      SensorChooser.Name = "SensorChooser";
      SensorChooser.Size = new System.Drawing.Size(200, 21);
      SensorChooser.TabIndex = tabIndex++;
      SensorChooser.Visible = SensorChooser.Items.Count > 0;
      Controls.Add(SensorChooser);
      ChooseSensorPanelResize();


      if (currentConnectionType == ConnectionType.NONE) RadioBtUSB.Checked = true;
      ResumeLayout(false);

      NextButton.Visible = true;
      NextButton.Enabled = ((SensorChooser.Items.Count > 0) && (SensorChooser.SelectedIndex >= 0));
      PrevButton.Visible = true;
      PrevButton.TabIndex = tabIndex++;
      NextButton.TabIndex = tabIndex++;

    }

    private void RadioBtRemote_CheckedChanged(object sender, EventArgs e)
    {
      DeviceAddr.Enabled = RadioBtRemote.Checked;
    }

    private void DeviceAddr_TextChanged(object sender, EventArgs e)
    {
      AddrTimer.Enabled = false;
      AddrTimer.Enabled = true;
      ChooseSensorTitle.Text = "Please wait...";

    }

    private void AddrTimer_Tick(object sender, EventArgs e)
    {
      string errmsg = "";
      AddrTimer.Enabled = false;
      ClearPreviousRegister();
      CurrentConnectionAddr = DeviceAddr.Text;
      if (CurrentConnectionAddr != "")
      {
        LogManager.Log("registering " + CurrentConnectionAddr);
        if (YAPI.PreregisterHub(CurrentConnectionAddr, ref errmsg) != YAPI.SUCCESS)
        {
          ChooseSensorErrorLabel.Text = "cannot connect to " + CurrentConnectionAddr + " : " + errmsg;
          LogManager.Log(ChooseSensorErrorLabel.Text);
          return;
        }
      }

    }

    private void ClearPreviousRegister()
    {
      switch (currentConnectionType)
      {
        case ConnectionType.USB:
          LogManager.Log("unregistering usb");
          YAPI.UnregisterHub("usb"); break;
        case ConnectionType.LOCALVIRTUALHUB:
          LogManager.Log("unregistering usb");
          YAPI.UnregisterHub("127.0.0.1"); break;
        case ConnectionType.IP:
          if (CurrentConnectionAddr != "")
          {
            LogManager.Log("unregistering usb");
            YAPI.UnregisterHub(CurrentConnectionAddr);

          }
          break;
      }
    }

    private void RadioBtUSB_Click(object sender, EventArgs e)
    {
      string errmsg = "";
      if (currentConnectionType == ConnectionType.USB) return;
      NextButton.Enabled = false;
      ChooseSensorErrorLabel.Text = "";
      ClearPreviousRegister();
      currentConnectionType = ConnectionType.USB;
      LogManager.Log("registering usb");
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        ChooseSensorErrorLabel.Text = "Cannot use USB: " + errmsg;
        LogManager.Log(ChooseSensorErrorLabel.Text);
        return;
      }
      ChooseSensorTitle.Text = "Please wait...";
    }

    private void RadioBtVirtualHub_Click(object sender, EventArgs e)
    {
      string errmsg = "";

      if (currentConnectionType == ConnectionType.LOCALVIRTUALHUB) return;
      NextButton.Enabled = false;
      ChooseSensorErrorLabel.Text = "";
      ClearPreviousRegister();
      currentConnectionType = ConnectionType.LOCALVIRTUALHUB;
      LogManager.Log("registering 127.0.0.1");
      if (YAPI.PreregisterHub("127.0.0.1", ref errmsg) != YAPI.SUCCESS)
      {
        ChooseSensorErrorLabel.Text = "Cannot use Local VirtualHub: " + errmsg;
        LogManager.Log(ChooseSensorErrorLabel.Text);
        return;
      }
      ChooseSensorTitle.Text = "Please wait...";
    }

    private void RadioBtRemote_Click(object sender, EventArgs e)
    {
      NextButton.Enabled = false;
      string errmsg = "";
      if (currentConnectionType == ConnectionType.IP) return;
      NextButton.Enabled = false;
      ChooseSensorErrorLabel.Text = "";
      ClearPreviousRegister();
      currentConnectionType = ConnectionType.IP;
      if (CurrentConnectionAddr != "")
      {
        if (YAPI.PreregisterHub(DeviceAddr.Text, ref errmsg) != YAPI.SUCCESS)
        {
          CurrentConnectionAddr = "";
          ChooseSensorErrorLabel.Text = "Cannot use this IP address: " + errmsg; return;
        }
        ChooseSensorTitle.Text = "Please wait...";
      }
    }

    private void DeviceArrival(YModule m)
    {
      string serial = m.get_serialNumber();
      int count = m.functionCount();
      for (int i = 0; i < count; i++)
      {

        string t = m.functionType(i);
        if (m.functionType(i) == "WeighScale")
        {
          string id = m.functionId(i);
          CustomSensor ws = new CustomSensor(YWeighScale.FindWeighScale(serial + "." + id));

          SensorChooser.Items.Add(ws);
          SensorChooser.Visible = SensorChooser.Items.Count > 0;
          if (SensorChooser.Items.Count == 1) ChooseSensorTitle.Text = "One WeighScale sensor found, Click on Next if this is the one you want to use";
          else ChooseSensorTitle.Text = SensorChooser.Items.Count.ToString() + " WeighScale sensors found, Choose the one you want to use and click on Next";
          if (SensorChooser.SelectedIndex < 0) SensorChooser.SelectedIndex = 0;
          NextButton.Enabled = true;

        }
      }
    }

    private void DeviceRemoval(YModule m)
    {
      bool oneItemRemoved = false;
      string serial = m.get_serialNumber();
      for (int i = SensorChooser.Items.Count - 1; i >= 0; i--)
      {
        if (((CustomSensor)SensorChooser.Items[i]).serial == serial)
        {
          if (SensorChooser.SelectedIndex == i) SensorChooser.SelectedIndex = -1;
          SensorChooser.Items.RemoveAt(i);
          oneItemRemoved = true;
        }
      }

      if (oneItemRemoved)
      {
        if (SensorChooser.Items.Count == 0) ChooseSensorTitle.Text = "No more sensor available";
        else if (SensorChooser.Items.Count == 1) ChooseSensorTitle.Text = "One WeighScale sensor left, Click on Next if this is the one you want use";
        else ChooseSensorTitle.Text = SensorChooser.Items.Count.ToString() + " WeighScale sensors found. Choose the one you want to use and click on Next";
        NextButton.Enabled = (SensorChooser.Items.Count) > 0 && (SensorChooser.SelectedIndex > 0);
        SensorChooser.Visible = SensorChooser.Items.Count > 0;


      }

      if (ChoosedWeighScale != null)
      {
        if (ChoosedWeighScale.serial == serial)
        {
          ClearCurrentPanel();
          CurrentStep = PanelDesc.WizardSteps.DEVICEDISCONNECTED;
          DrawCurrentPanel();
        }

      }
    }

    private PanelDesc.WizardSteps ChooseSensorPanelNextClicked()
    {
      ChoosedWeighScale = (CustomSensor)SensorChooser.SelectedItem;

      //  if (RadioBtCalibrate.Checked)  return PanelDesc.WizardSteps.RESETWARNNG;
      if (RadioBtAccumulate.Checked) return PanelDesc.WizardSteps.RESETWARNNG;
      if (RadioBtMonitor.Checked) return PanelDesc.WizardSteps.MONITORDATA;
      if (RadioBtCalibrate.Checked) return PanelDesc.WizardSteps.DATASOURCE;
      return PanelDesc.WizardSteps.ERROR;

    }


  }
}