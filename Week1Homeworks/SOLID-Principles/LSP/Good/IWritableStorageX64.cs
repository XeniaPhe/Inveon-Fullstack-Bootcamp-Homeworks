namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.LSP.Good;
internal interface IWritableStorageX64
{
    void Write(ulong address, byte[] bytes);
    void WriteByte(ulong address, byte value);
}