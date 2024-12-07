namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.LSP.Good;
internal abstract class StorageDeviceX64
{
    internal abstract byte ReadByte(ulong address);
    internal abstract byte[] Read(ulong address, ulong length);

    protected static string AddressString(ulong address)
    {
        return address.ToString("X");
    }

    protected static string AddressString(ulong address, ulong length)
    {
        return address.ToString("X") + "-" + (address + length).ToString("X");
    }
}