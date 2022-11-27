using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNOGOGAMES_TestTask
{
    public interface IMessageLongTermStore
    {
        public void Add(Message msg);
        public IEnumerable<Message> ExtractMessages(MessagePriority priority, int limit);
    }

    public class MemoryMessageStore : IMessageLongTermStore
    {
        private readonly Dictionary<MessagePriority, Queue<Message>> _priority2queue = new();

        public void Add(Message message)
        {
            if (!_priority2queue.ContainsKey(message.Priority))
            {
                _priority2queue[message.Priority] = new Queue<Message>();
            }

            _priority2queue[message.Priority].Enqueue(message);
        }

        public IEnumerable<Message> ExtractMessages(MessagePriority priority, int limit)
        {
            var messages = new List<Message>();

            if (_priority2queue.TryGetValue(priority, out var queue))
            {
                while (messages.Count < limit && queue.Count > 0)
                {
                    messages.Add(queue.Dequeue());
                }
            }

            return messages;
        }
    }
}
