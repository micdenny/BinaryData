# BinaryData

When parsing or storing data in binary file formats, the functionality offered by the default .NET `BinaryReader` and
`BinaryWriter` classes is often insufficient. It lacks support for a different byte order than the system one, and
cannot parse specific string or date formats (most prominently, 0-terminated strings instead of the default
variable-length prefixed .NET strings).

Further, navigating in binary files is slightly tedious when it is required to skip to another chunk in the file and
then navigate back. Also, aligning to specific block sizes might be a common task.

This NuGet package adds all this and more functionality by offering a large set of extension methods for the `Stream`
class of .NET 4.5 and .NET Standard 2.0. Additionally, serializing and deserializing complete class hierarchies can be
done immediately.
Additionally, `BinaryDataReader` and `BinaryDataWriter` classes are provided, usable like the default .NET
`BinaryReader` and `BinaryWriter` so that new functionality can be added almost instantly to existing projects.

## Installation

The library is available in the following NuGet packages:

- [Syroot.IO.BinaryData](https://www.nuget.org/packages/Syroot.IO.BinaryData) (unsigned)
- [Syroot.IO.BinaryData.Signed](https://www.nuget.org/packages/Syroot.IO.BinaryData.Signed) (signed assembly)

### Updated from 2.x.x to 3.x.x?
Please read the [release notes](https://github.com/Syroot/BinaryData/releases/tag/3.0.0) to learn more about the many
new features and possible breaking changes.

## Documentation

- A code-oriented feature tour is available [on the wiki](https://github.com/Syroot/BinaryData/wiki).
- Complete MSDN-style API documentation is hosted on [docs.syroot.com](http://docs.syroot.com/binarydata).

## Support

You can ask questions and suggest features on Discord aswell. Feel free to [join the BinaryData channel on the Syroot server](https://discord.gg/KSaSWTV)!
