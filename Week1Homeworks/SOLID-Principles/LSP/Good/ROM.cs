using Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.LSP.Good;
internal class ROM : StorageDeviceX64
{
    internal override byte[] Read(ulong address, ulong length)
    {
        Console.WriteLine($"Read ROM addresses {AddressString(address, length)}");
        return [];
    }

    internal override byte ReadByte(ulong address)
    {
        Console.WriteLine($"Read ROM address {AddressString(address)}");
        return 0;
    }
}