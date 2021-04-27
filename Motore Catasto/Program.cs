using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Motore_Catasto
{
    class ProgramCatasto : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProgramCatasto));
        private int _runnings;
        private volatile bool _stopping;
        private volatile bool _stopped;

        #region Avvio/arresto del servizio
        /// <summary>
        /// Avvia i moduli per fare il debug.
        /// </summary>
        private void StartForDebug()
        {
            OnStart(null);
            Application.EnableVisualStyles();
            MessageBox.Show("Service running in DEBUG mode... Press OK to stop.", ServiceName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnStop();
        }

        /// <summary>
        /// Avvio del servizio.
        /// </summary>
        /// <param name="args">Parametri del servizio.</param>
        protected override void OnStart(string[] args)
        {
            try
            {
                string pathfileinfo = RouteConfig.PathConfLog4Net;
                FileInfo fileconfiglog4net = new FileInfo(pathfileinfo);
                XmlConfigurator.ConfigureAndWatch(fileconfiglog4net);
                new Thread(CheckFileToImport).Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Arresto del servizio.
        /// </summary>
        protected override void OnStop()
        {
            if (_stopping || _stopped)
                return;

            _stopping = true;

            while (Thread.VolatileRead(ref _runnings) > 0)
                Thread.Sleep(100);

            _stopped = true;
            _stopping = false;
        }

        /// <summary>
        /// Arresto del server.
        /// </summary>
        protected override void OnShutdown()
        {
            OnStop();
        }
        #endregion
        #region Avvio dell'applicazione
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        /// <summary>
        /// Avvio dell'applicazione.
        /// </summary>
        /// <param name="args">Eventuali parametri da riga di comando.</param>
        /// <returns>Codice di ritorno usato dall'installer.</returns>
        [MTAThread]
        private static int Main(string[] args)
        {
            try
            {
                string pathfileinfo = RouteConfig.PathConfLog4Net;
                FileInfo fileconfiglog4net = new FileInfo(pathfileinfo);
                XmlConfigurator.ConfigureAndWatch(fileconfiglog4net);

                AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
                {
                    string message = (e.ExceptionObject is Exception) ?
                                         (e.ExceptionObject as Exception).Message : "Unspecified error.";
                };

                using (ProgramCatasto service = new ProgramCatasto())
                {
                    service.ServiceName = "Motore Catasto";
#if DEBUG
                    service.StartForDebug();
#else
                    ServiceBase.Run(service);
#endif
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        private void CheckFileToImport()
        {
            try
            {
                Interlocked.Increment(ref _runnings);
                int millisecondsToSleep = 600000;//(int)(new DateTime(DateTime.Now.AddDays(1).Year, DateTime.Now.AddDays(1).Month, DateTime.Now.AddDays(1).Day, 1, 0, 0) - DateTime.Now).TotalMilliseconds;

                millisecondsToSleep = int.Parse(TimeSpan.FromMinutes(30).TotalMilliseconds.ToString());
                for (; !_stopping; Thread.Sleep(millisecondsToSleep))
                {
                    if (_stopping)
                        break;
                    new ImportFiles().CheckStart();
                    new ImportFiles().CheckRibalta();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Interlocked.Decrement(ref _runnings);
            }
        }
    }
}