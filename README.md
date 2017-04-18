# BinaryData
.NET library extending binary reading and writing functionality.

## Introduction

When parsing or storing data in binary file formats, the functionality offered by the default .NET `BinaryReader` and `BinaryWriter` classes is often not sufficient. It totally lacks support for a different byte order than the one of the system and specific string or date formats (most prominently, 0-terminated strings instead of the default Int32 prefixed .NET strings).

Further, navigating in binary files is slightly tedious when it becomes required to skip to another chunk in the file and then navigate back. Also, aligning to specific block sizes might be a common task.

This NuGet package adds all this functionality by offering two new .NET 4.5 and .NET Standard 1.1 compatible classes, `BinaryDataReader` and  `BinaryDataWriter`, which extend the aforementioned .NET reader and writer, usable in a similar way so that they are easy to implement into existing projects - in fact, they can be used directly without requiring any changes to existing code.

The usage is described in detail [on the wiki](https://github.com/Syroot/BinaryData/wiki).

The library is available as a [NuGet package](https://www.nuget.org/packages/Syroot.IO.BinaryData).
