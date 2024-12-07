namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.LSP.Bad;

/*
 * Why does this class violate the Liskov Substitution Principle?
 * 
 * Because its design suggests that all subclasses must implement all methods (ReadByte, Read, Write, WriteByte).
 * However, not all storage devices can support both read and write operations.
 * A StorageDeviceX64 instance representing a ROM should not require Write methods.
 */

internal abstract class StorageDeviceX64
{
    internal abstract byte ReadByte(ulong address);
    internal abstract byte[] Read(ulong address, ulong length);
    internal abstract void Write(ulong address, byte[] bytes);
    internal abstract void WriteByte(ulong address, byte value);


    protected static string AddressString(ulong address)
    {
        return address.ToString("X");
    }

    protected static string AddressString(ulong address, ulong length)
    {
        return address.ToString("X") + "-" + (address + length).ToString("X");
    }
}