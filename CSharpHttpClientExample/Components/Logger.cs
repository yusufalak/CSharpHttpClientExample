using log4net;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore.Extensions;
using System.Reflection;
using System.Xml;

namespace Commons.Log
{
    public class Logger
    {
        private static readonly string LOG_CONFIG_FILE = @"Properties{0}{1}{2}log4net.config";
        private ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Logger GetInstance(System.Type type)
        {
            Logger _logger = new Logger();
            _logger.SetLog4NetConfiguration();
            _logger._log = LogManager.GetLogger(type);
            return _logger;
        }

        public void Info(object message)
        {
            _log.Info(message);
        }
        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }
        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }
        public void Error(object message)
        {
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }
        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void Critical(object message, Exception exception)
        {
            _log.Critical(message, exception);
        }

        private void SetLog4NetConfiguration()
        {
            string applicationLevel = Environment.GetEnvironmentVariable("ApplicationLevel");

            char seperator = Path.AltDirectorySeparatorChar;

            string logConfigFile = String.Format(LOG_CONFIG_FILE, seperator, applicationLevel, seperator);

            XmlDocument log4netConfig = new XmlDocument();

            log4netConfig.Load(File.OpenRead(logConfigFile));

            var repo = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            string path = Directory.GetCurrentDirectory() + seperator + "Logs" + seperator;
            Directory.CreateDirectory(path);
            string fileName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            log4net.GlobalContext.Properties["LogFileName"] = path + fileName;

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}
