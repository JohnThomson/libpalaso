﻿using System.Linq;
using System.Xml;
using NUnit.Framework;
using SIL.WritingSystems;

namespace SIL.LexiconUtils.Tests
{
	[TestFixture]
	public class UserSettingsWritingSystemDataMapperTests
	{
		[Test]
		public void Read_ValidXml_SetsAllProperties()
		{
			const string userSettingsXml =
@"<LexiconUserSettings>
  <WritingSystems>
    <WritingSystem id=""en-US"">
      <LocalKeyboard>en-US_English-IPA</LocalKeyboard>
      <DefaultFontName>Times New Roman</DefaultFontName>
    </WritingSystem>
    <WritingSystem id=""fr-FR"">
      <DefaultFontSize>12</DefaultFontSize>
      <IsGraphiteEnabled>false</IsGraphiteEnabled>
    </WritingSystem>
  </WritingSystems>
</LexiconUserSettings>";

			var userSettingsDataMapper = new UserSettingsWritingSystemDataMapper(() => userSettingsXml, xml => { });

			var ws1 = new WritingSystemDefinition("en-US");
			userSettingsDataMapper.Read(ws1);

			Assert.That(ws1.LocalKeyboard.Id, Is.EqualTo("en-US_English-IPA"));
			Assert.That(ws1.DefaultFont.Name, Is.EqualTo("Times New Roman"));
			Assert.That(ws1.DefaultFontSize, Is.EqualTo(0));
			Assert.That(ws1.IsGraphiteEnabled, Is.True);

			var ws2 = new WritingSystemDefinition("fr-FR");
			userSettingsDataMapper.Read(ws2);

			Assert.That(ws2.LocalKeyboard, Is.EqualTo(Keyboard.Controller.DefaultForWritingSystem(ws2)));
			Assert.That(ws2.DefaultFont, Is.Null);
			Assert.That(ws2.DefaultFontSize, Is.EqualTo(12));
			Assert.That(ws2.IsGraphiteEnabled, Is.False);

			var ws3 = new WritingSystemDefinition("es");
			userSettingsDataMapper.Read(ws3);

			Assert.That(ws3.LocalKeyboard, Is.EqualTo(Keyboard.Controller.DefaultForWritingSystem(ws1)));
			Assert.That(ws3.DefaultFont, Is.Null);
			Assert.That(ws3.DefaultFontSize, Is.EqualTo(0));
			Assert.That(ws3.IsGraphiteEnabled, Is.True);
		}

		[Test]
		public void Read_InvalidXml_Throws()
		{
			var userSettingsDataMapper = new UserSettingsWritingSystemDataMapper(() => "Bad XML", xml => { });
			var ws1 = new WritingSystemDefinition("en-US");
			Assert.That(() => userSettingsDataMapper.Read(ws1), Throws.TypeOf<XmlException>());
		}

		[Test]
		public void Read_EmptyXml_NothingSet()
		{
			var userSettingsDataMapper = new UserSettingsWritingSystemDataMapper(() => "", xml => { });

			var ws1 = new WritingSystemDefinition("en-US");
			userSettingsDataMapper.Read(ws1);

			Assert.That(ws1.LocalKeyboard, Is.EqualTo(Keyboard.Controller.DefaultForWritingSystem(ws1)));
			Assert.That(ws1.DefaultFont, Is.Null);
			Assert.That(ws1.DefaultFontSize, Is.EqualTo(0));
			Assert.That(ws1.IsGraphiteEnabled, Is.True);
		}

		[Test]
		public void Write_EmptyXml_XmlUpdated()
		{
			string userSettingsXml = "";
			var userSettingsDataMapper = new UserSettingsWritingSystemDataMapper(() => userSettingsXml, xml => userSettingsXml = xml);

			var ws1 = new WritingSystemDefinition("en-US");
			ws1.LocalKeyboard = Keyboard.Controller.CreateKeyboardDefinition("en-US_English-IPA", KeyboardFormat.Unknown, Enumerable.Empty<string>());
			ws1.DefaultFont = new FontDefinition("Times New Roman");
			userSettingsDataMapper.Write(ws1);

			Assert.That(userSettingsXml, Is.EqualTo(
@"<LexiconUserSettings>
  <WritingSystems>
    <WritingSystem id=""en-US"">
      <LocalKeyboard>en-US_English-IPA</LocalKeyboard>
      <DefaultFontName>Times New Roman</DefaultFontName>
    </WritingSystem>
  </WritingSystems>
</LexiconUserSettings>"));
		}

		[Test]
		public void Write_ValidXml_XmlUpdated()
		{
			string userSettingsXml =
@"<LexiconUserSettings>
  <WritingSystems>
    <WritingSystem id=""en-US"">
      <LocalKeyboard>en-US_English-IPA</LocalKeyboard>
      <DefaultFontName>Times New Roman</DefaultFontName>
    </WritingSystem>
  </WritingSystems>
</LexiconUserSettings>";

			var userSettingsDataMapper = new UserSettingsWritingSystemDataMapper(() => userSettingsXml, xml => userSettingsXml = xml);
			var ws1 = new WritingSystemDefinition("en-US");
			ws1.LocalKeyboard = Keyboard.Controller.CreateKeyboardDefinition("en-US_English", KeyboardFormat.Unknown, Enumerable.Empty<string>());
			ws1.DefaultFont = null;
			ws1.DefaultFontSize = 12;
			ws1.IsGraphiteEnabled = false;
			userSettingsDataMapper.Write(ws1);

			Assert.That(userSettingsXml, Is.EqualTo(
@"<LexiconUserSettings>
  <WritingSystems>
    <WritingSystem id=""en-US"">
      <LocalKeyboard>en-US_English</LocalKeyboard>
      <DefaultFontSize>12</DefaultFontSize>
      <IsGraphiteEnabled>false</IsGraphiteEnabled>
    </WritingSystem>
  </WritingSystems>
</LexiconUserSettings>"));
		}

		[Test]
		public void Write_InvalidXml_Throws()
		{
			var userSettingsDataMapper = new UserSettingsWritingSystemDataMapper(() => "Bad XML", xml => { });
			var ws1 = new WritingSystemDefinition("en-US");
			Assert.That(() => userSettingsDataMapper.Write(ws1), Throws.TypeOf<XmlException>());
		}

		[Test]
		public void Remove_ExistingWritingSystem_UpdatesXml()
		{
			string userSettingsXml =
@"<LexiconUserSettings>
  <WritingSystems>
    <WritingSystem id=""en-US"">
      <LocalKeyboard>en-US_English-IPA</LocalKeyboard>
      <DefaultFontName>Times New Roman</DefaultFontName>
    </WritingSystem>
    <WritingSystem id=""fr-FR"">
      <DefaultFontSize>12</DefaultFontSize>
      <IsGraphiteEnabled>false</IsGraphiteEnabled>
    </WritingSystem>
  </WritingSystems>
</LexiconUserSettings>";

			var userSettingsDataMapper = new UserSettingsWritingSystemDataMapper(() => userSettingsXml, xml => userSettingsXml = xml);
			userSettingsDataMapper.Remove("fr-FR");
			Assert.That(userSettingsXml, Is.EqualTo(
@"<LexiconUserSettings>
  <WritingSystems>
    <WritingSystem id=""en-US"">
      <LocalKeyboard>en-US_English-IPA</LocalKeyboard>
      <DefaultFontName>Times New Roman</DefaultFontName>
    </WritingSystem>
  </WritingSystems>
</LexiconUserSettings>"));

			userSettingsDataMapper.Remove("en-US");
			Assert.That(userSettingsXml, Is.EqualTo("<LexiconUserSettings />"));
		}

		[Test]
		public void Remove_NonexistentWritingSystem_DoesNotUpdateXml()
		{
			string userSettingsXml =
@"<LexiconUserSettings>
  <WritingSystems>
    <WritingSystem id=""en-US"">
      <LocalKeyboard>en-US_English-IPA</LocalKeyboard>
      <DefaultFontName>Times New Roman</DefaultFontName>
    </WritingSystem>
  </WritingSystems>
</LexiconUserSettings>";

			var userSettingsDataMapper = new UserSettingsWritingSystemDataMapper(() => userSettingsXml, xml => userSettingsXml = xml);
			userSettingsDataMapper.Remove("fr-FR");
			Assert.That(userSettingsXml, Is.EqualTo(
@"<LexiconUserSettings>
  <WritingSystems>
    <WritingSystem id=""en-US"">
      <LocalKeyboard>en-US_English-IPA</LocalKeyboard>
      <DefaultFontName>Times New Roman</DefaultFontName>
    </WritingSystem>
  </WritingSystems>
</LexiconUserSettings>"));
		}
	}
}
