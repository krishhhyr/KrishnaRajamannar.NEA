using System;

namespace KrishnaRajamannar.NEA.Models.Dto
{
    // This means that 
    [Serializable]
    // This is the class which is used to store the data being sent 
    // from the server to the client 
    public class ServerResponse
    {
        public string SessionId { get; set; }
        // This is used so that the client can recognise the purpose for
        // which the data is being sent
        public string DataType { get; set; }

        public string Data { get; set; }
    }
}
