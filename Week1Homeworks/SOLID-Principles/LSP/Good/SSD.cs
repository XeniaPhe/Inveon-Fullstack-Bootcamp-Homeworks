﻿namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.LSP.Good;
internal class SSD : StorageDeviceX64, IWritableStorageX64
{
    internal override byte[] Read(ulong address, ulong length)
    {
        Console.WriteLine($"Read SSD addresses {AddressString(address, length)}");
        return [];
    }

    internal override byte ReadByte(ulong address)
    {
        Console.WriteLine($"Read SSD address {AddressString(address)}");
        return 0;
    }

    public void Write(ulong address, byte[] bytes)
    {
        Console.WriteLine($"Written to SSD addresses {AddressString(address, (ulong)bytes.Length)}");
    }

    public void WriteByte(ulong address, byte value)
    {
        Console.WriteLine($"Written to SSD address {AddressString(address)}");
    }
}