using System;

namespace KrishnaRajamannar.NEA.Models.Dto
{
    [Serializable]
    public class ServerResponse
    {
        public string SessionId { get; set; }

        public string DataType { get; set; }

        public string Data { get; set; }
    }


}
