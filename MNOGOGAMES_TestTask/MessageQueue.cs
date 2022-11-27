using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNOGOGAMES_TestTask
{
    public interface IMessageQueue
    {
        public void Add(Message message);
        public bool TryGetMessage(out Message message);
    }

    public class MessageQueue : IMessageQueue
    {
        private const int QueueMessagesLimit = 1;
        private readonly IMessageLongTermStore _messageStore;
        private readonly Dictionary<MessagePriority, Queue<Message>> _priority2queue = new();

        public MessageQueue(IMessageLongTermStore messageStore)
        {
            _messageStore = messageStore;
        }

        public void Add(Message message)
        {
            if (!_priority2queue.ContainsKey(message.Priority))
            {
                _priority2queue[message.Priority] = new Queue<Message>();
            }

            if (_priority2queue[message.Priority].Count < QueueMessagesLimit)
            {
                _priority2queue[message.Priority].Enqueue(message);
            }
            else
            {
                SaveMessageToStore(message);
            }
        }

        public bool TryGetMessage(out Message message)
        {
            FillQueues();

            foreach (var (_, queue) in _priority2queue.OrderByDescending(x => (int) x.Key))
            {
                if(queue.Any())
                {
                    message = queue.Dequeue();
                    return true;
                }
            }

            message = null;
            return false;
        }

        private void FillQueues()
        {
            foreach (var (priority, queue) in _priority2queue)
            {
                if (queue.Count <= 0)
                {
                    foreach (var message in _messageStore.ExtractMessages(priority, QueueMessagesLimit))
                    {
                        queue.Enqueue(message);
                    }
                }
            }
        }

        private void SaveMessageToStore(Message message)
        {
            _messageStore.Add(message);
        }
    }
}
