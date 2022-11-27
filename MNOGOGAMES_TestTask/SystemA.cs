using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNOGOGAMES_TestTask
{
    public class SystemA
    {
        private Random _r = new Random();

        public Message GenerateMessage()
        {
            return new Message
            {
                Payload = Guid.NewGuid().ToString(),
                Priority = (MessagePriority)_r.Next(0, 3)
            };
        }
    }
}
