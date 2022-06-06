// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using X.Common.Helper;

DirectoryInfo directory = new(Environment.CurrentDirectory);

List<Stream> GetStreams(string fileName)
{
    string raw = CommandHelper.ExecuteCommand(directory.FullName, $"ffprobe -i  \"{fileName}\" -select_streams v  -show_streams  -print_format json");
    Regex jsonRegex = new(@"{[\w\W]+}");

    string json = jsonRegex.Match(raw).Value;
    using JsonDocument doc = JsonDocument.Parse(json);
    var streams = doc.RootElement.GetProperty("streams").Deserialize<List<Stream>>();
    return streams;
}
var files = directory.GetFiles().Where(x => x.Extension.ToLower() == ".mp4");
int count = files.Count();
var fileStreamInfos = files.Select(x => GetStreams(x.FullName)).ToList();
var kinds = fileStreamInfos.SelectMany(x => x.ToList()).GroupBy(x => x.Res).Select(x => x.Key).OrderByDescending(x => x.Split('x').Select(i => Convert.ToInt32(i)).Sum()).ToList();

var seq = fileStreamInfos.Select(x => x.Select(y => y.Res).ToList()).Select(x => x.IndexOf(kinds[0])).ToList();

string fileList = string.Join(" -i ", files.Select(x => x.Name));
StringBuilder sb = new($"ffmpeg -i {fileList} -vcodec h264_nvenc  -filter_complex \"");

for (int i = 0; i < count; i++)
{
    sb.Append($"[{i}:v:{seq[i]}][{i}:a:0]");
}
sb.Append($"concat=n={count}:v=1:a=1[outv][outa]\" -map \"[outv]\" -map \"[outa]\" output.mp4");
string command = sb.ToString();
Console.WriteLine(command);
//Console.WriteLine("Hello, World!");
Console.ReadLine();