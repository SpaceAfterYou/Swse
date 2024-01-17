using System.Reflection.PortableExecutable;

namespace SourceGenerator.ClientRequestMessage;

public readonly record struct PacketMagick(byte Left = 0x2, byte Right = 0x0);
public readonly record struct PacketHeader(byte Group, byte Command)
{
    public PacketMagick Magic { get; }
}

public sealed class ClientRequestMessageSourceGenerator(PacketMagick magic, PEHeaders headers, Stream stream)
{
    private readonly byte[][] _patterns =
        [
            // push    rbx
            [0x40, 0x53],

            // sub     rsp, 20h
            [0x48, 0x83, 0xEC, 0x20],

            // mov     dword ptr [rcx], 7
            [0xC7, 0x01, magic.Left, 0x00, 0x00, 0x00],

            // mov     word ptr [rcx+4], 2
            [0x66, 0xC7, magic.Right, 0x04, 0x02, 0x00]
        ];

    public void Run()
    {
        var reader = new BinaryReader(stream);

        var sectionHeader = headers.SectionHeaders.FirstOrDefault(e => e.Name == ".text");
        reader.BaseStream.Seek(sectionHeader.PointerToRawData, SeekOrigin.Begin);

        var sectionContent = reader.ReadBytes(sectionHeader.SizeOfRawData);

        var pattern = _patterns.SelectMany(e => e).ToArray();

        var header = sectionContent
            .Select((_, i) => i)
            .FirstOrDefault(i => sectionContent.AsSpan(i, pattern.Length).SequenceEqual(pattern));

        reader.BaseStream.Seek(sectionHeader.PointerToRawData + header, SeekOrigin.Begin);
        var packetConstructor = reader.ReadBytes(pattern.Length + 16).Skip(pattern.Length).ToArray();
    }
}
