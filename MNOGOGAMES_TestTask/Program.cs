using MNOGOGAMES_TestTask;

var longTermMessageStore = new MemoryMessageStore();
var queue = new MessageQueue(longTermMessageStore);

var systemA = new SystemA();
var systemB = new SystemB(queue);

systemB.StartMonitoring();

var totalMessagesCount = 0;

for (int i = 0; i < 20; i++)
{
    if (totalMessagesCount < 20)
    {
        var message = systemA.GenerateMessage();
        queue.Add(message);

        totalMessagesCount++;
    }
    i++;
}

Thread.Sleep(10000);

systemB.StopMonitoring();

