using System;
using System.Xml.Linq;

class Segment
{
    public char? Type { get; set; }
    public int? StartAddress { get; set; }
    public int? Length { get; set; }
    public string? Name { get; set; } = "";
    public Segment? Next { get; set; } = null;
    public Segment? Previous { get; set; } = null;

}

class MemoryManager
{
    public static Segment head = new Segment
    {
        Name = "head",
        Next = new Segment
        {
            Type = 'H',
            StartAddress = 0,
            Length = 1000,
            Previous = head
        },
    };
    private static Segment lastAllocated = head;

    public void CreateProcess(string name, int length)
    {
        while (true)
        {
            if(lastAllocated.Next == null)
            {
                Console.WriteLine("End of the List. Process is not created!");
                lastAllocated = head;
                break;
            }
            //else if (lastAllocated.Next == head)
            //{
            //    head.Next = new Segment
            //    {
            //        Type = 'H',
            //        StartAddress = head.StartAddress + length,
            //        Length = head.Length - length,
            //        Next = null
            //    };
            //    head.Type = 'P';
            //    head.Name = name;
            //    head.Length = length;
            //    lastAllocated = head;
            //    break;
            //}
            else if (lastAllocated.Next.Type == 'H' && lastAllocated.Next.Length > length)
            {
                Segment newSegment = new Segment
                {
                    Next = new Segment
                    {
                        Type = 'H',
                        StartAddress = lastAllocated.Next.StartAddress + length,
                        Length = lastAllocated.Next.Length - length
                    },
                    Type = 'P',
                    StartAddress = lastAllocated.Next.StartAddress,
                    Length = length,
                    Name = name,
                    Previous = lastAllocated,
                };
                newSegment.Next.Previous = newSegment;
                lastAllocated.Next = newSegment;
                lastAllocated = newSegment;
                break; 
            }
            else if(lastAllocated.Next.Type == 'H' && lastAllocated.Next.Length == length)
            {
                lastAllocated.Next.Type = 'P';
                lastAllocated.Next.Name = name;
                break;
            }
            else
            {
                lastAllocated = lastAllocated.Next;
                continue;
            }
        }
    }

    public void StopProcess(string name)
    {
        Segment current = head.Next;

        while (current != null)
        {
            if(current.Name == name)
            {
                try
                {
                    if (current.Next.Type == 'H')
                    {
                        current.Type = 'H';
                        current.Name += current.Next.Name;
                        current.Length += current.Next.Length;
                        current.Next = current.Next.Next;
                    }
                }
                catch
                {
                    current.Type = 'H';
                }
                try
                {
                    if (current.Previous.Type == 'H')
                    {
                        current.Previous.Name += current.Name;
                        current.Previous.Length += current.Length;
                        current.Previous.Next = current.Next;
                    }
                }
                catch 
                {
                    current.Type = 'H';
                }
                current.Type = 'H';
            }
            current = current.Next;
        }
    }

    public void DisplayMemory()
    {
        Segment current = head.Next;

        Console.WriteLine("Memory Layout:[Type:process(P) or hole(H)]  [Start address]  [Length]  [Process name]");

        while (true)
        {
            //var pre = current.Previous != null ? current.Previous.Name : "null";[{pre}]
            Console.Write($"[{current.Type}][{current.StartAddress}][{current.Length}][{current.Name}]");
            if(current.Next == null)
            {
                Console.WriteLine();
                break;
            }
            Console.Write("<-->");
            current = current.Next;
        }
    }
}

class Program
{
    static void Main()
    {
        MemoryManager memoryManager = new MemoryManager();
        Console.WriteLine(
                "Menu:\n" +
                "1-Create a new process(process name, length)\n" +
                "2-Stop process by name(process name)\n" +
                "3-Display Memory\n" +
                "4-Exit");
        while (true)
        {
            string[] command = Console.ReadLine().Split(" ");
            if (command[0] == "4")
            {
                break;
            }
            switch (command[0])
            {
                case "1":
                    memoryManager.CreateProcess(command[1], Int32.Parse(command[2]));
                    break;
                case "2":
                    memoryManager.StopProcess(command[1]);
                    break;
                case "3":
                    memoryManager.DisplayMemory();
                    break;
            }
        }
    }
}
