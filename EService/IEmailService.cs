using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;

namespace EService
{
    [ServiceContract]
    public interface IEmailService
    {
        [OperationContract]
        void SendMessage(string userName, string password, string sendTo, string title, string body, List<string> attachment);

        [OperationContract]
        ObservableCollection<string> ViewMail(string userName, string password);

        [OperationContract]
        void DeleteMail(string userName, string password, int selectedMessage);

        [OperationContract]
        string ViewMessageBody(string userName, string password, int selectedMessage);
    }
}
