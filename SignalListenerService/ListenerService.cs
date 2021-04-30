using CSDI.Data.Services.Interfaces;
using LupenM.SignalListener;
using RPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SignalListenerService
{
  public partial class ListenerService : ServiceBase
    {
        private readonly ISensorsService _sensorsService;
        private Dictionary<string, Sensor> dictSensors = new Dictionary<string, Sensor>();
        private SignalListener listener;
        public ListenerService(ISensorsService sensorsService)
        {
            InitializeComponent();
            this.ServiceName = "ListenerService";
        }

        protected override void OnStart(string[] args)
        {
            LogService("Service is Started");
            StartListener();
        }

        protected override void OnStop()
        {
            LogService("Service Stoped");
            try
            {
                if (listener != null)
                {
                    listener.Disconnect();
                }
            }
            catch (Exception e)
            {
                LogService(e.Message);
            }
        }

        private void StartListener()
        {
            try
            {
                listener = new SignalListener();
                string brockerHostName = @"test.mosquitto.org";

                List<Sensor> sensors = _sensorsService.GetAll();
                dictSensors = sensors.ToDictionary(k => k.Topic, k => k);
                string[] topics = sensors.Select(s => s.Topic).ToArray();

                listener.Initialize(brockerHostName, topics);
                listener.PublishReceived += Listener_PublishReceived;
            }
            catch(Exception e)
            {
                LogService(e.Message);
            }
            
        }

        private void Listener_PublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                var sensor = dictSensors[e.Topic];
                if (sensor != null)
                {
                    SensorIndication indication = new SensorIndication();
                    indication.Date = DateTime.Now;
                    indication.Value = Encoding.UTF8.GetString(e.Message);
                    indication.SensorId = sensor.SensorId;

                    IndicationsData.Add(indication);
                }
            }
            catch (Exception ex)
            {
                LogService(ex.Message);
            }

        }

        private void LogService(string content)
        {
            /*FileStream fs = new FileStream(@"c:\TestServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();*/
        }

    }
}
