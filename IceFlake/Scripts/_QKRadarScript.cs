using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IceFlake.Client;
using IceFlake.Client.Objects;
using IceFlake.Client.Scripts;
using IceFlake.Client.Patchables;
using System.Threading;
//using WhiteMagic.Internals;
//using GreyMagic.Internals;



namespace IceFlake.Scripts
{
    public class QKRadarScript : Script
    {
        public QKRadarScript()
            : base("QK", "_QKRadarScript")
        { }

        // Fields

        //public bool SpeedHackActive = false;
        //public WhiteMagic.Magic _magic = new WhiteMagic.Magic();
        //internal static MainMenu MainMenu;

        // Methods

        //public static class DllLoader
        //{
        //    [System.Runtime.InteropServices.DllImport("RadarDll.dll")]
        //    public static extern int Main();
        //}

        public override void OnStart()
        {
            if (!Manager.ObjectManager.IsInGame)
                return;

            //DllLoader.Main();

            //Assembly assembly = Assembly.LoadFile(@"C:\dyn.dll");
            //Type type = assembly.GetType("TestRunner");
            //var obj = (TestRunner)Activator.CreateInstance(type);
            //obj.Run();

            //Assembly assembly = Assembly.LoadFile("RadarDll.dll");
            //Type type = assembly.GetType("TestRunner");
            //Print("" + type.Assembly.EntryPoint);

            //return;

            //IRunnable runnable = Activator.CreateInstance(type) as IRunnable;
            //if (runnable == null) throw new Exception("broke");
            //runnable.Run();





            /*/
            List<string> assemblyNames = new List<string>();
            Assembly[] oAssemblies = new Assembly[args.Length];

            for(int assemblyCount = 0;assemblyCount < args.Length;assemblyCount++)
            {
                oAssemblies[assemblyCount] = Assembly.LoadFile(args[assemblyCount]);

                try
                {
                    foreach(Type oType in oAssemblies[assemblyCount].GetTypes())
                    {
                        // Check whether class is inheriting from IMFServicePlugin.
                        if(oType.GetInterface("IMFDBAnalyserPlugin") == typeof(IMFDBAnalyserPlugin))
                        {
                            assemblyNames.Add(args[assemblyCount].Substring(args[assemblyCount].LastIndexOf("\\") + 1));
                        }
                    }
                }
                catch(Exception ex)
                {
                    EventLog log = new EventLog("Application");
                    log.Source = "MFPluggerService";
                    log.WriteEntry(ex.Message);
                }
            }

            // Passing data one application domain to another.
            AppDomain.CurrentDomain.SetData("AssemblyNames", assemblyNames.ToArray());
            /*/







        }





        
    }
}
