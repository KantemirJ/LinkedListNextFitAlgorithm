using System;
using System.Xml.Linq;

class Segment
{
    public char? Type { get; set; }
    public int? StartAddress { get; set; }
    public int? Length { get; set; }
    public string? Name { get; set; } = null;
    public Segment? Next { get; set; } = null;
    public Segment? Previous { get; set; } = null;

}

class MemoryManager
{
    private static Segment head = new Segment
    {
        Type = 'H',
        StartAddress = memoryStart,
        Length = 1000,
        Next = null, 
        Previous = null
    };
    private static Segment lastAllocated = new Segment
    {
        Next = head,
        Previous = head
    };
    private static readonly int memoryStart = 0;
    private static readonly int memoryEnd = 999;

    public void CreateProcess(string name, int length)
    {
        while (true)
        {
            if(lastAllocated.Next == null)
            {
                Console.WriteLine("End of the List. Process is not created!");
                break;
            }
            else if (lastAllocated.Next == head)
            {
                head.Next = new Segment
                {
                    Type = 'H',
                    StartAddress = head.StartAddress + length,
                    Length = head.Length - length,
                    Next = null
                };
                head.Type = 'P';
                head.Name = name;
                head.Length = length;
                lastAllocated = head;
                break;
            }
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
                    StartAddress = lastAllocated.StartAddress + lastAllocated.Length,
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
        Segment current = head;

        while (current != null)
        {
            if(current.Name == name)
            {
                //if (current.Previous == null && current.Next ==null)
                //{

                //}
                current.Type = 'H';
            }
            current = current.Next;
        }
    }

    public void DisplayMemory()
    {
        Segment current = head;

        Console.WriteLine("Memory Layout:");

        while (current != null)
        {
            var pre = current.Previous != null ? current.Previous.Name : "null";
            Console.Write($"[{pre}][{current.Type}][{current.StartAddress}][{current.Length}][{current.Name}]-->");
            current = current.Next;
        }
    }
}

class Program
{
    static void Main()
    {
        MemoryManager memoryManager = new MemoryManager();

        // Allocate memory using Next Fit
        memoryManager.CreateProcess("P1", 20);
        memoryManager.CreateProcess("P2", 50);
        memoryManager.CreateProcess("P3", 30);
        memoryManager.StopProcess("P2");

        // Display memory layout
        memoryManager.DisplayMemory();
    }
}
