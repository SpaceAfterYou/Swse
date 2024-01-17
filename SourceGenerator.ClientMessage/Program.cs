// See https://aka.ms/new-console-template for more information
using SourceGenerator.ClientRequestMessage;
using System.Reflection.PortableExecutable;

var path = "A:\\SteamLibrary\\steamapps\\common\\Soulworker_KR\\SoulWorker64.dll";

using var stream = File.OpenRead(path);
var headers = new PEHeaders(stream);

var magic = new PacketMagick();
var responses = new ClientRequestMessageSourceGenerator(magic, headers, stream);
responses.Run();
