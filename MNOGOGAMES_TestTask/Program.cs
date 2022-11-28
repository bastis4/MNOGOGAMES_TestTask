using MNOGOGAMES_TestTask;

var longTermMessageStore = new MemoryMessageStore();
var queue = new MessageQueue(longTermMessageStore);

var systemA = new SystemA();
var systemB = new SystemB(queue);

systemB.StartMonitoring();

var totalMessagesCount = 0;
while (true)
{
    if (totalMessagesCount < 20)
    {
        var message = systemA.GenerateMessage();
        queue.Add(message);

        totalMessagesCount++;
    }
}

