using System;
using System.Web.Http;
using Hyland.Unity;
using log4net;
using Newtonsoft.Json;

namespace OnBaseAuthentication.Controllers
{
    public class AuthController : ApiController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AuthController));

        // GET api/values
        public string Get()
        {
            return "It's alive! It's alive!";
        }

        [HttpPost]
        public Application OnBaseLogin([FromBody] AuthenticationProperty properties)
        {
            var values = JsonConvert.SerializeObject(properties);
            _logger.Info(values);

            Application app = null;

            AuthenticationProperties authentication = Application.CreateOnBaseAuthenticationProperties(properties.AppServer, properties.Username, properties.Password, properties.DataSource);
            authentication.LicenseType = LicenseType.Default;

            try
            {
                app = Application.Connect(authentication);
                _logger.Info("Session " + app.SessionID + " connected.");
            }
            catch (MaxLicensesException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (SystemLockedOutException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (InvalidLoginException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (AuthenticationFailedException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (MaxConcurrentLicensesException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (InvalidLicensingException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (SessionNotFoundException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (DllNotFoundException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (NullReferenceException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (OutOfMemoryException ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }
            catch (Exception ex) { _logger.Error(ex.StackTrace); throw new Exception(ex.Message); }

            return app;
        }

        [HttpPost]
        public void OnBaseLogout(Application app)
        {
            if (app != null)
            {
                app.Disconnect();
                _logger.Info("Session " + app.SessionID + " disconnected.");
            }
        }
    }
}
