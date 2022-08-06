# VideoConcat

[Concatenating media files](https://trac.ffmpeg.org/wiki/Concatenate#filter)

### Publish

```bash
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true
dotnet publish -c Release --no-self-contained -r linux-x64 -p:PublishSingleFile=true
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishReadyToRun=true -p:PublishReadyToRunComposite=true
dotnet publish -c Release -r osx.11.0-arm64 --no-self-contained -p:PublishSingleFile=true
dotnet publish -c Release -r osx.11.0-arm64 --self-contained -p:PublishSingleFile=true -p:PublishTrimmed=true -p:PublishReadyToRun=true -p:PublishReadyToRunComposite=true
```

### Encoders
    - Nvidia : h264_nvenc
    - Intel : h264_qsv
    $ ffmpeg -encoders

### Bug

ffprobe version