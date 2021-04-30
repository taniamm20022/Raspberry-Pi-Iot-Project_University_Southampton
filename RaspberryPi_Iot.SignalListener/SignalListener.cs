
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace LupenM.SignalListener
{
  public class SignalListener
  {
    MqttClient client;

    public event MqttMsgPublishEventHandler PublishReceived;
    public delegate void MqttMsgPublishEventHandler(object sender, MqttMsgPublishEventArgs e);

    public void Initialize(string brockerHostName, string[] topics)
    {
      client = new MqttClient(brockerHostName); //new MqttClient(IPAddress.Parse("10.132.132.247"));
      client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

      client.Connect(Guid.NewGuid().ToString());
      byte[] qosLevels = InitializeQOSLevels(topics.Length, MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE);
      client.Subscribe(topics, qosLevels);
    }

    public void Disconnect()
    {
      if (client != null && client.IsConnected)
      {
        client.Disconnect();
      }
    }

    private byte[] InitializeQOSLevels(int items, byte qosLevel)
    {
      byte[] qos = new byte[items];

      for (int i = 0; i < qos.Length; i++)
      {
        qos[i] = qosLevel;
      }

      return qos;
    }

    protected virtual void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
    {
      MqttMsgPublishEventHandler handler = PublishReceived;
      if (handler != null)
      {
        handler(sender, e);
      }
    }

    public void Publish(string topic, byte[] message)
    {
      if (client != null && client.IsConnected)
      {
        client.Publish(topic, message);
      }
    }
  }
}