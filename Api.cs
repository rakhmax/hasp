using Aladdin.HASP;

namespace Rakhmax.HASP
{
    class Api
    {
        

        private static string getHaspStatusMessage(HaspStatus status)
        {
            string statusText;

            switch (status)
            {
                case HaspStatus.ContainerNotFound:
                    statusText = "Evaluation is expired! Plug in your HASP HL key and restart the application.";
                    break;
                case HaspStatus.StatusOk:
                    statusText = "OK";
                    break;
                default:
                    statusText = status.ToString();
                    break;
            }

            return statusText;
        }
    }
}
