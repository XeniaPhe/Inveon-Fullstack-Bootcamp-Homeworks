namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.LSP.Bad;
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

    internal override void Write(ulong address, byte[] bytes)
    {
        throw new NotImplementedException("You cannot write to Read-Only Memory");
    }

    internal override void WriteByte(ulong address, byte value)
    {
        throw new NotImplementedException("You cannot write to Read-Only Memory");
    }
}