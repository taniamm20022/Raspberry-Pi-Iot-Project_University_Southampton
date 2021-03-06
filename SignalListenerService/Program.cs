
/*******************************************************************************
* Специализиран-софтуер-за-събиране-и-съхраняване-на-показания-от-прибори
* Произведен-от-УЕБСАЙТ-БГ-ЕООД-Сртф.№359-Д-р359/19.12.2017г.
* Продуктов-№-20171219-359
********************************************************************************/


using System;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Collections;

namespace SignalListenerService
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        ServiceBase[] ServicesToRun;
        ServicesToRun = new ServiceBase[]
        {
          new ListenerService()
        };
        ServiceBase.Run(ServicesToRun);
      }
      else if (args.Length == 1)
      {
        switch (args[0])
        {
          case "-install":
            InstallService();
            StartService();
            break;
          case "-uninstall":
            StopService();
            UninstallService();
            break;
          default:
            throw new NotImplementedException();
        }
      }
    }

    private static bool IsInstalled()
    {
      using (ServiceController controller = new ServiceController("ListenerService"))
      {
        try
        {
          ServiceControllerStatus status = controller.Status;
        }
        catch
        {
          return false;
        }
        return true;
      }
    }

    private static bool IsRunning()
    {
      using (ServiceController controller = new ServiceController("ListenerService"))
      {
        if (!IsInstalled()) return false;

        return (controller.Status == ServiceControllerStatus.Running);
      }
    }

    private static AssemblyInstaller GetInstaller()
    {
      AssemblyInstaller installer = new AssemblyInstaller(typeof(ListenerService).Assembly, null);
      installer.UseNewContext = true;

      return installer;
    }

    private static void InstallService()
    {
      if (IsInstalled()) return;

      try
      {
        using (AssemblyInstaller installer = GetInstaller())
        {
          IDictionary state = new Hashtable();
          try
          {
            installer.Install(state);
            installer.Commit(state);
          }
          catch
          {
            try
            {
              installer.Rollback(state);
            }
            catch { }
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
    }

    private static void UninstallService()
    {
      if (!IsInstalled()) return;

      try
      {
        using (AssemblyInstaller installer = GetInstaller())
        {
          IDictionary state = new Hashtable();
          try
          {
            installer.Uninstall(state);
          }
          catch
          {
            throw;
          }
        }
      }
      catch
      {
        throw;
      }
    }

    private static void StartService()
    {
      if (!IsInstalled()) return;

      using (ServiceController controller = new ServiceController("ListenerService"))
      {
        try
        {
          if (controller.Status != ServiceControllerStatus.Running)
          {
            controller.Start();
            controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
          }
        }
        catch
        {
          throw;
        }
      }
    }

    private static void StopService()
    {
      if (!IsInstalled()) return;

      using (ServiceController controller = new ServiceController("ListenerService"))
      {
        try
        {
          if (controller.Status != ServiceControllerStatus.Stopped)
          {
            controller.Stop();
            controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
          }
        }
        catch
        {
          throw;
        }
      }
    }

  }
}