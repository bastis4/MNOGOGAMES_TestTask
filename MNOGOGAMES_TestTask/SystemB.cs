namespace MNOGOGAMES_TestTask
{
    public class SystemB
    {
        
        private bool _isMonitoringStarted;


        private readonly IMessageQueue _messageQueue;
        private Thread _monitorThread;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public SystemB(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public void StartMonitoring()
        {
            if(!_isMonitoringStarted)
            {
                _isMonitoringStarted = true;

                _monitorThread = new Thread(() => MonitorMessageQueue());
                _monitorThread.Start();
            }
        }

        private void MonitorMessageQueue()
        {
            while (_isMonitoringStarted)
            {
                if (_messageQueue.TryGetMessage(out var message))
                {
                    //Имитируем длительную работу
                    Thread.Sleep(1000);

                    ProcessMessageFromQueue(message);
                }
                else
                {
                    //Ожидаем поступления сообщений в очередь
                    Thread.Sleep(500);
                }
            }
        }

        private void ProcessMessageFromQueue(Message message)
        {
            Console.WriteLine($"{message.Priority} | {message.Payload}");
        }

        public void StopMonitoring()
        {
            if(_monitorThread != null)
            {
                _isMonitoringStarted = false;
                _cts.Cancel();
            }
        }
    }
}