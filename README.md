# BinaryData
.NET library extending binary reading and writing functionality.

## Updated from 2.x.x to 3.x.x?
- Please read the [release notes](https://github.com/Syroot/BinaryData/releases/tag/3.0.0) for breaking changes.

## Introduction

When parsing or storing data in binary file formats, the functionality offered by the default .NET `BinaryReader` and
`BinaryWriter` classes is often insufficient. It lacks support for a different byte order than the system one, and
cannot parse specific string or date formats (most prominently, 0-terminated strings instead of the default
variable-length prefixed .NET strings).

Further, navigating in binary files is slightly tedious when it becomes required to skip to another chunk in the file
and then navigate back. Also, aligning to specific block sizes might be a common task.

This NuGet package adds all this functionality by offering a large set of extension methods for the `Stream` class of
.NET 4.5 and .NET Standard 2.0.
Additionally, `BinaryDataReader` and `BinaryDataWriter` are provided, usable like the default .NET `BinaryReader`
and `BinaryWriter` classes so that new functionality is easy to add to existing projects - in fact, they can be used
directly without requiring any changes to existing code.

All features are described in detail [on the wiki](https://github.com/Syroot/BinaryData/wiki).

The library is available as an [unsigned](https://www.nuget.org/packages/Syroot.IO.BinaryData) or
[signed](https://www.nuget.org/packages/Syroot.IO.BinaryData.Signed) NuGet package.
