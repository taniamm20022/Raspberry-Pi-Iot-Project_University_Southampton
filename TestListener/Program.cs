
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Configuration;
using RPI.Core.Entities;
using CSDI.Data;
using LupenM.SignalListener;

namespace TestListener
{
    class Program
    {
        private static Dictionary<string, Sensor> dictSensors = new Dictionary<string, Sensor>();
        private static SignalListener listener;

        static void Main(string[] args)
        {
            listener = new SignalListener();
            string brockerHostName = @"test.mosquitto.org";

            List<Sensor> sensors = SensorRepository.GetAll();
            dictSensors = sensors.ToDictionary(k => k.Topic, k => k);
            string[] topics = sensors.Select(s => s.Topic).ToArray();

            listener.Initialize(brockerHostName, topics);
            listener.PublishReceived += Listener_PublishReceived;

            //Publish
            if(false)
            { 
                foreach (var sensor in sensors)
                {
                    //SensorTimer_Elapsed(null, null, sensor);
                    System.Timers.Timer timer = new System.Timers.Timer();

                    timer.Interval = sensor.IndicationInterval.TotalMilliseconds >= 1 ? sensor.IndicationInterval.TotalMilliseconds : 1000;
                    timer.Elapsed += (sender, e) => SensorTimer_Elapsed(sender, e, sensor);
                    timer.Enabled = true;
                }
            }

            /*for (int i = 0; i < 5; i++)
            {
                foreach(var topic in topics)
                {

                    listener.Publish(topic, Encoding.UTF8.GetBytes((rand.Next(19, 25) + rand.NextDouble()).ToString()));
                    Thread.Sleep(300);
                }
            }*/
        }

        private static void SensorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e, Sensor sensor)
        {
            #region ReadSettings
            var sAttr = ConfigurationManager.AppSettings.Get("Type1Min");
            int Type1Min = ParseConfig(sAttr, 12);

            sAttr = ConfigurationManager.AppSettings.Get("Type1Max");
            int Type1Max = ParseConfig(sAttr, 15);

            sAttr = ConfigurationManager.AppSettings.Get("Type2Min");
            int Type2Min = ParseConfig(sAttr, 890);

            sAttr = ConfigurationManager.AppSettings.Get("Type2Max");
            int Type2Max = ParseConfig(sAttr, 965);

            sAttr = ConfigurationManager.AppSettings.Get("Type3Min");
            int Type3Min = ParseConfig(sAttr, 28);

            sAttr = ConfigurationManager.AppSettings.Get("Type3Max");
            int Type3Max = ParseConfig(sAttr, 62);

            sAttr = ConfigurationManager.AppSettings.Get("Type16Min");
            int Type16Min = ParseConfig(sAttr, 350);

            sAttr = ConfigurationManager.AppSettings.Get("Type16Max");
            int Type16Max = ParseConfig(sAttr, 550);

            sAttr = ConfigurationManager.AppSettings.Get("Type17Min");
            int Type17Min = ParseConfig(sAttr, 30);

            sAttr = ConfigurationManager.AppSettings.Get("Type17Max");
            int Type17Max = ParseConfig(sAttr, 65);

            sAttr = ConfigurationManager.AppSettings.Get("Type18Min");
            int Type18Min = ParseConfig(sAttr, 0);

            sAttr = ConfigurationManager.AppSettings.Get("Type18Max");
            int Type18Max = ParseConfig(sAttr, 0);

            sAttr = ConfigurationManager.AppSettings.Get("Type19Min");
            int Type19Min = ParseConfig(sAttr, 0);

            sAttr = ConfigurationManager.AppSettings.Get("Type19Max");
            int Type19Max = ParseConfig(sAttr, 1);

            sAttr = ConfigurationManager.AppSettings.Get("Type20Min");
            int Type20Min = ParseConfig(sAttr, 0);

            sAttr = ConfigurationManager.AppSettings.Get("Type20Max");
            int Type20Max = ParseConfig(sAttr, 0);
            #endregion

            Random rand = new Random();
            byte[] value = { 0 };

            switch (sensor.UnitId)
            {
                case 1:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type1Min, Type1Max) + rand.NextDouble()), 2).ToString());
                    break;
                case 2:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type2Min, Type2Max) + rand.NextDouble()), 2).ToString());
                    break;
                case 3:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type3Min, Type3Max) + rand.NextDouble()), 2).ToString());
                    break;
                case 23:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type16Min, Type16Max) + rand.NextDouble()), 2).ToString());
                    break;
                case 24:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type17Min, Type17Max) + rand.NextDouble()), 2).ToString());
                    break;
                case 25:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type18Min, Type18Max) + rand.NextDouble()), 2).ToString());
                    break;
                case 26:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type19Min, Type19Max) + rand.NextDouble()), 2).ToString());
                    break;
                case 27:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type20Min, Type20Max) + rand.NextDouble()), 2).ToString());
                    break;
                default:
                    value = Encoding.UTF8.GetBytes(Math.Round((rand.Next(Type1Min, Type1Max) + rand.NextDouble()), 2).ToString());
                    break;
            }

            listener.Publish(sensor.Topic, value);


        }

        private static int ParseConfig(string sAttr, int defaultValue)
        {
            int value;

            if (!Int32.TryParse(sAttr, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        private static void Listener_PublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(String.Format("{0}:{1}:{2}", DateTime.Now, e.Topic, Encoding.UTF8.GetString(e.Message)));

            var sensor = dictSensors[e.Topic];

            if (sensor != null)
            {
                SensorIndication indication = new SensorIndication();
                indication.Date = DateTime.Now;
                indication.Value = Encoding.UTF8.GetString(e.Message);
                indication.SensorId = sensor.SensorId;

                //IndicationsData.Add(indication);
            }

            //Console.ReadLine();
        }
    }

    /*public class SignalListener
    {
        MqttClient client;
        public void Initialize()
        {
            client = new MqttClient(@"test.mosquitto.org"); //new MqttClient(IPAddress.Parse("10.132.132.247"));
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            client.MqttMsgUnsubscribed += Client_MqttMsgUnsubscribed;

            client.Connect(Guid.NewGuid().ToString());
            string[] topic = { "#" };//{ "room/temp", "room/humidity" };//select topics from sensors
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE };//{ MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE };
            client.Subscribe(topic, qosLevels);
        }

        private void Client_MqttMsgUnsubscribed(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgUnsubscribedEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }

        private void Client_MqttMsgSubscribed(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }

        private void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            //var a = e.Message;
            //var b = e.Topic;
            Console.WriteLine(String.Format("{0}:{1}", e.Topic, Encoding.UTF8.GetString(e.Message)));
            Console.ReadLine();
        }
    }*/
}