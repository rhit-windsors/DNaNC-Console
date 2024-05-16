//Ask user for port

using DNaNC_Console.Services;
using FileShare = DNaNC_Console.Services.FileShare;

Console.Write("Enter the port to host this node from: ");
var port = int.Parse(Console.ReadLine()!);

//Ask which node to join
Console.Write("Enter the host of the node to join: ");
var host = Console.ReadLine()!;
Console.Write("Enter the port of the node to join: ");
var joinPort = Console.ReadLine();

//Join the network
if(host == "" || joinPort == "")
{
    //This is the first node in the network
    NodeManager.InitNetwork(port);
}
else
{
    NodeManager.Join(host, int.Parse(joinPort!), port);
}

//Start listening
_ = Task.Run(NodeServer.Listen);

while (true)
{
    Console.Write("-> ");
    var input = Console.ReadLine();
    if (input == "exit")
    {
        NodeManager.Leave();
        break;
    }
    if (input == "print")
    {
        Console.WriteLine($"Local Node: {NodeManager.LocalNode.Host}:{NodeManager.LocalNode.Port}");
        Console.WriteLine($"Successor: {NodeManager.Successor?.Host}:{NodeManager.Successor?.Port}");
        Console.WriteLine($"Predecessor: {NodeManager.Predecessor?.Host}:{NodeManager.Predecessor?.Port}");
        Console.WriteLine($"Successor's Successor: {NodeManager.SuccessorSuccessor?.Host}:{NodeManager.SuccessorSuccessor?.Port}");
        Console.WriteLine($"Predecessor's Predecessor: {NodeManager.PredecessorPredecessor?.Host}:{NodeManager.PredecessorPredecessor?.Port}");
    }

    if (input.Split(" ")[0] == "query")
    {
        var fileName = input.Split(" ")[1].Trim();
        NodeManager.Query(fileName, NodeManager.LocalNode);
    }

    if (input.Split(" ")[0] == "share")
    {
        //Share the file
        FileShare.ShareFile(input.Split(" ")[1]);
    }
    
    if(input == "list")
    {
        //List the files
        Console.WriteLine(FileShare.ListSharedFiles());
    }

    if (input == "found")
    {
        Console.WriteLine(FileShare.ListFoundFiles());
    }

    if (input.Split(" ")[0] == "get")
    {
        var index = int.Parse(input.Split(" ")[1]);
        NodeManager.RequestFile(0);
    }
}