## ModernKeePassLib

[![Build status](https://dev.azure.com/geogeob/ModernKeePass/_apis/build/status/Builds/ModernKeePassLib)](https://dev.azure.com/geogeob/ModernKeePass/_build/latest?definitionId=6)

# What is this ?

ModernKeePassLib is a port of KeePassLib to .netstandard 1.2, distributed as a nuget package.
The aim was to change as little as possible the original library. However, some workarounds have to be made as .netstandard misses quite a few features of the full .net framework.
Main changes:
- Removed the dependency on the filesystem
- Added a dependency on Windows (I'm working on tring to find a way to remove it altogether)
- Some features are handled by external nuget packages (cryptography, colors, etc.), so it may introduce small differences

# Usage

1. Create a IOConnectionInfo from a byte array:
`var ioConnection = IOConnectionInfo.FromByteArray(file);`
2. Create a composite key and add credential information:
`var compositeKey = new CompositeKey();
// Password
compositeKey.AddUserKey(new KcpPassword("Password"));`
// Key file
compositeKey.AddUserKey(new KcpKeyFile(IOConnectionInfo.FromByteArray(KeyFileContents)));`
3. Use it to open the database:
// You may use whatever logger you choose (here, I use nullstatuslogger)
`PwDatabase.Open(ioConnection, compositeKey, new NullStatusLogger());`
4. Do stuff in you database (create entries, groups, ...)
5. Save your changes:
`PwDatabase.Save(new NullStatusLogger());`
6. At this point, nothing is commited to disk, so you need to retrieve the byte array:
`var contents = PwDatabase.IOConnectionInfo.Bytes;`
7. Write the byte array to a file, to a stream, whatever !