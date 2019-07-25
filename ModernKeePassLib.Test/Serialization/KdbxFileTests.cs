﻿using System;
using System.Globalization;
using System.IO;
using System.Text;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Security;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Collections;
using Xunit;

namespace ModernKeePassLib.Test.Serialization
{
    public class KdbxFileTests
    {
        const string TestLocalizedAppName = "My Localized App Name";

        const string TestDatabaseName = "My Database Name";
        const string TestDatabaseDescription = "My Database Description";
        const string TestDefaultUserName = "My Default User Name";
        const string TestColor = "#FF0000"; // Red

        const string TestRootGroupName = "My Root Group Name";
        const string TestRootGroupNotes = "My Root Group Notes";
        const string TestRootGroupDefaultAutoTypeSequence = "My Root Group Default Auto Type Sequence";

        const string TestDatabase = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>\r\n" +
          "<KeePassFile>\r\n" +
          "\t<Meta>\r\n" +
          "\t\t<Generator>" + TestLocalizedAppName + "</Generator>\r\n" +
          "\t\t<DatabaseName>" + TestDatabaseName + "</DatabaseName>\r\n" +
          "\t\t<DatabaseNameChanged>2017-10-23T08:03:55Z</DatabaseNameChanged>\r\n" +
          "\t\t<DatabaseDescription>" + TestDatabaseDescription + "</DatabaseDescription>\r\n" +
          "\t\t<DatabaseDescriptionChanged>2017-10-23T08:03:55Z</DatabaseDescriptionChanged>\r\n" +
          "\t\t<DefaultUserName>" + TestDefaultUserName + "</DefaultUserName>\r\n" +
          "\t\t<DefaultUserNameChanged>2017-10-23T08:03:55Z</DefaultUserNameChanged>\r\n" +
          "\t\t<MaintenanceHistoryDays>365</MaintenanceHistoryDays>\r\n" +
          //"\t\t<Color>" + testColor + "</Color>\r\n" +
          "\t\t<Color></Color>\r\n" +
          "\t\t<MasterKeyChanged>2017-10-23T08:03:55Z</MasterKeyChanged>\r\n" +
          "\t\t<MasterKeyChangeRec>-1</MasterKeyChangeRec>\r\n" +
          "\t\t<MasterKeyChangeForce>-1</MasterKeyChangeForce>\r\n" +
          "\t\t<MemoryProtection>\r\n" +
          "\t\t\t<ProtectTitle>False</ProtectTitle>\r\n" +
          "\t\t\t<ProtectUserName>False</ProtectUserName>\r\n" +
          "\t\t\t<ProtectPassword>True</ProtectPassword>\r\n" +
          "\t\t\t<ProtectURL>False</ProtectURL>\r\n" +
          "\t\t\t<ProtectNotes>False</ProtectNotes>\r\n" +
          "\t\t</MemoryProtection>\r\n" +
          "\t\t<RecycleBinEnabled>True</RecycleBinEnabled>\r\n" +
          "\t\t<RecycleBinUUID>AAAAAAAAAAAAAAAAAAAAAA==</RecycleBinUUID>\r\n" +
          "\t\t<RecycleBinChanged>2017-10-23T08:03:55Z</RecycleBinChanged>\r\n" +
          "\t\t<EntryTemplatesGroup>AAAAAAAAAAAAAAAAAAAAAA==</EntryTemplatesGroup>\r\n" +
          "\t\t<EntryTemplatesGroupChanged>2017-10-23T08:03:55Z</EntryTemplatesGroupChanged>\r\n" +
          "\t\t<HistoryMaxItems>10</HistoryMaxItems>\r\n" +
          "\t\t<HistoryMaxSize>6291456</HistoryMaxSize>\r\n" +
          "\t\t<LastSelectedGroup>AAAAAAAAAAAAAAAAAAAAAA==</LastSelectedGroup>\r\n" +
          "\t\t<LastTopVisibleGroup>AAAAAAAAAAAAAAAAAAAAAA==</LastTopVisibleGroup>\r\n" +
          "\t\t<Binaries />\r\n" +
          "\t\t<CustomData />\r\n" +
          "\t</Meta>\r\n" +
          "\t<Root>\r\n" +
          "\t\t<Group>\r\n" +
          "\t\t\t<UUID>AAAAAAAAAAAAAAAAAAAAAA==</UUID>\r\n" +
          "\t\t\t<Name>" + TestRootGroupName + "</Name>\r\n" +
          "\t\t\t<Notes>" + TestRootGroupNotes + "</Notes>\r\n" +
          "\t\t\t<IconID>49</IconID>\r\n" +
          "\t\t\t<Times>\r\n" +
          "\t\t\t\t<CreationTime>2017-10-23T08:03:55Z</CreationTime>\r\n" +
          "\t\t\t\t<LastModificationTime>2017-10-23T08:03:55Z</LastModificationTime>\r\n" +
          "\t\t\t\t<LastAccessTime>2017-10-23T08:03:55Z</LastAccessTime>\r\n" +
          "\t\t\t\t<ExpiryTime>2017-10-23T08:03:55Z</ExpiryTime>\r\n" +
          "\t\t\t\t<Expires>False</Expires>\r\n" +
          "\t\t\t\t<UsageCount>0</UsageCount>\r\n" +
          "\t\t\t\t<LocationChanged>2017-10-23T08:03:55Z</LocationChanged>\r\n" +
          "\t\t\t</Times>\r\n" +
          "\t\t\t<IsExpanded>True</IsExpanded>\r\n" +
          "\t\t\t<DefaultAutoTypeSequence>" + TestRootGroupDefaultAutoTypeSequence + "</DefaultAutoTypeSequence>\r\n" +
          "\t\t\t<EnableAutoType>null</EnableAutoType>\r\n" +
          "\t\t\t<EnableSearching>null</EnableSearching>\r\n" +
          "\t\t\t<LastTopVisibleEntry>AAAAAAAAAAAAAAAAAAAAAA==</LastTopVisibleEntry>\r\n" +
          "\t\t</Group>\r\n" +
          "\t\t<DeletedObjects />\r\n" +
          "\t</Root>\r\n" +
          "</KeePassFile>";

        const string TestDate = "2017-10-23T08:03:55Z";

        [Fact]
        public void TestLoad()
        {
            var database = new PwDatabase();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(TestDatabase)))
            {
                var file = new KdbxFile(database);
                file.Load(ms, KdbxFormat.PlainXml, null);
            }
            //Assert.That(database.Color.ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
            Assert.Equal(PwCompressionAlgorithm.GZip, database.Compression);
            //Assert.That (database.CustomData, Is.EqualTo ());
            Assert.True(database.CustomIcons.Count == 0);
        }

        [Fact]
        public void TestSave()
        {
            var buffer = new byte[4096];
            using (var ms = new MemoryStream(buffer))
            {
                var database = new PwDatabase();
                database.New(new IOConnectionInfo(), new CompositeKey());
                var date = DateTime.Parse(TestDate, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);
                //var date = DateTime.UtcNow;
                PwDatabase.LocalizedAppName = TestLocalizedAppName;
                database.Name = TestDatabaseName;
                database.NameChanged = date;
                database.Description = TestDatabaseDescription;
                database.DescriptionChanged = date;
                database.DefaultUserName = TestDefaultUserName;
                database.DefaultUserNameChanged = date;
                //database.Color = Color.Red;
                database.MasterKeyChanged = date;
                database.RecycleBinChanged = date;
                database.EntryTemplatesGroupChanged = date;
                database.RootGroup.Uuid = PwUuid.Zero;
                database.RootGroup.Name = TestRootGroupName;
                database.RootGroup.Notes = TestRootGroupNotes;
                database.RootGroup.DefaultAutoTypeSequence = TestRootGroupDefaultAutoTypeSequence;
                database.RootGroup.CreationTime = date;
                database.RootGroup.LastModificationTime = date;
                database.RootGroup.LastAccessTime = date;
                database.RootGroup.ExpiryTime = date;
                database.RootGroup.LocationChanged = date;
                var file = new KdbxFile(database);
                file.Save(ms, null, KdbxFormat.PlainXml, null);
            }
            var fileContents = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Replace("\0", "");
            if (typeof(KdbxFile).Namespace.StartsWith("KeePassLib."))
            {
                // Upstream KeePassLib does not specify line endings for XmlTextWriter,
                // so it uses native line endings.
                fileContents = fileContents.Replace("\n", "\r\n");
            }
            Assert.Equal(fileContents, TestDatabase);
        }

        [Fact]
        public void TestSearch()
        {
            var database = new PwDatabase();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(TestDatabase)))
            {
                var file = new KdbxFile(database);
                file.Load(ms, KdbxFormat.PlainXml, null);
            }
            var sp = new SearchParameters()
            {
                SearchString = "sfsoiwsefsi"
            };
            var listStorage = new PwObjectList<PwEntry>();
            database.RootGroup.SearchEntries(sp, listStorage);
            Assert.Equal(0U, listStorage.UCount);
            var entry = new PwEntry(true, true);
            entry.Strings.Set("Title", new ProtectedString(false, "NaMe"));
            database.RootGroup.AddEntry(entry, true);
            sp.SearchString = "name";
            database.RootGroup.SearchEntries(sp, listStorage);
            Assert.Equal(1U, listStorage.UCount);
        }
    }
}
