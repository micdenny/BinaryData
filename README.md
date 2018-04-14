# BinaryData

When parsing or storing data in binary file formats, the functionality offered by the default .NET `BinaryReader` and
`BinaryWriter` classes is often insufficient. It lacks support for a different byte order than the system one, and
cannot parse specific string or date formats (most prominently, 0-terminated strings instead of the default
variable-length prefixed .NET strings).

Further, navigating in binary files is slightly tedious when it is required to skip to another chunk in the file and
then navigate back. Also, aligning to specific block sizes might be a common task.

The `Syroot.BinaryData` NuGet package adds all this and more functionality by offering a large set of extension methods
for the `Stream` class of .NET 4.5 and .NET Standard 2.0. `BinaryDataReader` and `BinaryDataWriter` classes are
provided, usable like the default .NET `BinaryReader` and `BinaryWriter` so that new functionality can be added almost
instantly to existing projects.

The `Syroot.BinaryData.Serialization` package can serialize and deserialize complete class hierarchies without any
further code required.

## Installation

The library is available in the following NuGet packages:

Core functionality:
- [Syroot.BinaryData](https://www.nuget.org/packages/Syroot.BinaryData) (unsigned, released as beta)
- [Syroot.BinaryData.Signed](https://www.nuget.org/packages/Syroot.BinaryData.Signed) (signed assembly, released as beta)

Serialization functionality (adds the core package dependency):
- [Syroot.BinaryData.Serialization](https://www.nuget.org/packages/Syroot.BinaryData.Serialization) (unsigned, to be released)
- [Syroot.BinaryData.Serialization.Signed](https://www.nuget.org/packages/Syroot.BinaryData.Serialization.Signed) (signed assembly, to be released)

### Updated from 4.x.x?

The previous [Syroot.IO.BinaryData](https://www.nuget.org/packages/Syroot.IO.BinaryData) package has been split into the
core and the serialization packages mentioned above. Beyond many new features, several breaking changes were made.
Please have a look at the most recent [release notes](https://github.com/Syroot/BinaryData/releases/) for more info.

## Documentation

- A code-oriented feature tour is available [on the wiki](https://github.com/Syroot/BinaryData/wiki).
- Complete MSDN-style API documentation is hosted on [docs.syroot.com](http://docs.syroot.com/binarydata).
