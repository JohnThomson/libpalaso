﻿using System.Linq;
using System.Xml;
using NUnit.Framework;
using SIL.WritingSystems;

namespace SIL.LexiconUtils.Tests
{
	[TestFixture]
	public class ProjectSettingsWritingSystemDataMapperTests
	{
		[Test]
		public void Read_ValidXml_SetsAllProperties()
		{
			const string projectSettingsXml =
@"<LexiconProjectSettings>
  <WritingSystems>
    <WritingSystem id=""qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2"">
      <Abbreviation>kal</Abbreviation>
      <LanguageName>Kalaba</LanguageName>
      <ScriptName>Fake</ScriptName>
      <RegionName>Zolrog</RegionName>
      <VariantNames>
        <VariantName>Custom 1</VariantName>
        <VariantName>Custom 2</VariantName>
      </VariantNames>
    </WritingSystem>
    <WritingSystem id=""fr-FR"">
      <SpellCheckingId>fr_FR</SpellCheckingId>
      <LegacyMapping>converter</LegacyMapping>
      <Keyboard>Old Keyboard</Keyboard>
    </WritingSystem>
  </WritingSystems>
</LexiconProjectSettings>";

			var projectSettingsDataMapper = new ProjectSettingsWritingSystemDataMapper(() => projectSettingsXml, xml => { });

			var ws1 = new WritingSystemDefinition("qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2");
			projectSettingsDataMapper.Read(ws1);

			Assert.That(ws1.Abbreviation, Is.EqualTo("kal"));
			Assert.That(ws1.Language.Name, Is.EqualTo("Kalaba"));
			Assert.That(ws1.Script.Name, Is.EqualTo("Fake"));
			Assert.That(ws1.Region.Name, Is.EqualTo("Zolrog"));
			Assert.That(ws1.Variants.Select(v => v.Name), Is.EqualTo(new[] {"Custom 1", "Custom 2"}));
			Assert.That(ws1.SpellCheckingId, Is.EqualTo(string.Empty));
			Assert.That(ws1.LegacyMapping, Is.EqualTo(string.Empty));
			Assert.That(ws1.Keyboard, Is.EqualTo(string.Empty));

			var ws2 = new WritingSystemDefinition("fr-FR");
			projectSettingsDataMapper.Read(ws2);

			Assert.That(ws2.Abbreviation, Is.EqualTo("fr"));
			Assert.That(ws2.Language.Name, Is.EqualTo("French"));
			Assert.That(ws2.Script, Is.Null);
			Assert.That(ws2.Region.Name, Is.EqualTo("France"));
			Assert.That(ws2.Variants, Is.Empty);
			Assert.That(ws2.SpellCheckingId, Is.EqualTo("fr_FR"));
			Assert.That(ws2.LegacyMapping, Is.EqualTo("converter"));
			Assert.That(ws2.Keyboard, Is.EqualTo("Old Keyboard"));

			var ws3 = new WritingSystemDefinition("es");
			projectSettingsDataMapper.Read(ws3);

			Assert.That(ws3.Abbreviation, Is.EqualTo("es"));
			Assert.That(ws3.Language.Name, Is.EqualTo("Spanish"));
			Assert.That(ws3.Script, Is.Null);
			Assert.That(ws3.Region, Is.Null);
			Assert.That(ws3.Variants, Is.Empty);
			Assert.That(ws3.SpellCheckingId, Is.EqualTo(string.Empty));
			Assert.That(ws3.LegacyMapping, Is.EqualTo(string.Empty));
			Assert.That(ws3.Keyboard, Is.EqualTo(string.Empty));
		}

		[Test]
		public void Read_InvalidXml_Throws()
		{
			var projectSettingsDataMapper = new ProjectSettingsWritingSystemDataMapper(() => "Bad XML", xml => { });
			var ws1 = new WritingSystemDefinition("en-US");
			Assert.That(() => projectSettingsDataMapper.Read(ws1), Throws.TypeOf<XmlException>());
		}

		[Test]
		public void Read_EmptyXml_NothingSet()
		{
			var projectSettingsDataMapper = new ProjectSettingsWritingSystemDataMapper(() => "", xml => { });

			var ws1 = new WritingSystemDefinition("en-US");
			projectSettingsDataMapper.Read(ws1);

			Assert.That(ws1.Abbreviation, Is.EqualTo("en"));
			Assert.That(ws1.Language.Name, Is.EqualTo("English"));
			Assert.That(ws1.Script, Is.Null);
			Assert.That(ws1.Region.Name, Is.EqualTo("United States"));
			Assert.That(ws1.Variants, Is.Empty);
			Assert.That(ws1.SpellCheckingId, Is.EqualTo(string.Empty));
			Assert.That(ws1.LegacyMapping, Is.EqualTo(string.Empty));
			Assert.That(ws1.Keyboard, Is.EqualTo(string.Empty));
		}

		[Test]
		public void Write_EmptyXml_XmlUpdated()
		{
			string userSettingsXml = "";
			var projectSettingsDataMapper = new ProjectSettingsWritingSystemDataMapper(() => userSettingsXml, xml => userSettingsXml = xml);

			var ws1 = new WritingSystemDefinition("qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2");
			ws1.Language = new LanguageSubtag(ws1.Language, "Kalaba");
			ws1.Script = new ScriptSubtag(ws1.Script, "Fake");
			ws1.Region = new RegionSubtag(ws1.Region, "Zolrog");
			ws1.Variants[0] = new VariantSubtag(ws1.Variants[0], "Custom 1");
			ws1.Variants[1] = new VariantSubtag(ws1.Variants[1], "Custom 2");
			projectSettingsDataMapper.Write(ws1);

			Assert.That(userSettingsXml, Is.EqualTo(
@"<LexiconProjectSettings>
  <WritingSystems>
    <WritingSystem id=""qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2"">
      <Abbreviation>kal</Abbreviation>
      <LanguageName>Kalaba</LanguageName>
      <ScriptName>Fake</ScriptName>
      <RegionName>Zolrog</RegionName>
      <VariantNames>
        <VariantName>Custom 1</VariantName>
        <VariantName>Custom 2</VariantName>
      </VariantNames>
    </WritingSystem>
  </WritingSystems>
</LexiconProjectSettings>"));
		}

		[Test]
		public void Write_ValidXml_XmlUpdated()
		{
			string projectSettingsXml =
@"<LexiconProjectSettings>
  <WritingSystems>
    <WritingSystem id=""qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2-var3"">
      <Abbreviation>kal</Abbreviation>
      <LanguageName>Kalaba</LanguageName>
      <ScriptName>Fake</ScriptName>
      <RegionName>Zolrog</RegionName>
      <VariantNames>
        <VariantName>Custom 1</VariantName>
		<VariantName>Custom 2</VariantName>
        <VariantName>Custom 3</VariantName>
      </VariantNames>
    </WritingSystem>
  </WritingSystems>
</LexiconProjectSettings>";

			var userSettingsDataMapper = new ProjectSettingsWritingSystemDataMapper(() => projectSettingsXml, xml => projectSettingsXml = xml);
			var ws1 = new WritingSystemDefinition("qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2-var3");
			ws1.Abbreviation = "ka";
			ws1.Variants[0] = new VariantSubtag(ws1.Variants[0], "Custom 1");
			ws1.Variants[2] = new VariantSubtag(ws1.Variants[2], "Custom 3");
			ws1.SpellCheckingId = "en_US";
			ws1.LegacyMapping = "converter";
			ws1.Keyboard = "Old Keyboard";
			userSettingsDataMapper.Write(ws1);

			Assert.That(projectSettingsXml, Is.EqualTo(
@"<LexiconProjectSettings>
  <WritingSystems>
    <WritingSystem id=""qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2-var3"">
      <Abbreviation>ka</Abbreviation>
      <VariantNames>
        <VariantName>Custom 1</VariantName>
        <VariantName />
        <VariantName>Custom 3</VariantName>
      </VariantNames>
      <SpellCheckingId>en_US</SpellCheckingId>
      <LegacyMapping>converter</LegacyMapping>
      <Keyboard>Old Keyboard</Keyboard>
    </WritingSystem>
  </WritingSystems>
</LexiconProjectSettings>"));
		}

		[Test]
		public void Write_InvalidXmlFile_Throws()
		{
			var projectSettingsDataMapper = new ProjectSettingsWritingSystemDataMapper(() => "Bad XML", xml => { });
			var ws1 = new WritingSystemDefinition("en-US");
			Assert.That(() => projectSettingsDataMapper.Write(ws1), Throws.TypeOf<XmlException>());
		}

		[Test]
		public void Remove_ExistingWritingSystem_UpdatesXml()
		{
			string projectSettingsXml =
@"<LexiconProjectSettings>
  <WritingSystems>
    <WritingSystem id=""qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2"">
      <Abbreviation>kal</Abbreviation>
      <LanguageName>Kalaba</LanguageName>
      <ScriptName>Fake</ScriptName>
      <RegionName>Zolrog</RegionName>
      <VariantNames>
        <VariantName>Custom 1</VariantName>
        <VariantName>Custom 2</VariantName>
      </VariantNames>
    </WritingSystem>
    <WritingSystem id=""fr-FR"">
      <SpellCheckingId>fr_FR</SpellCheckingId>
      <LegacyMapping>converter</LegacyMapping>
      <Keyboard>Old Keyboard</Keyboard>
    </WritingSystem>
  </WritingSystems>
</LexiconProjectSettings>";

			var projectSettingsDataMapper = new ProjectSettingsWritingSystemDataMapper(() => projectSettingsXml, xml => projectSettingsXml = xml);
			projectSettingsDataMapper.Remove("fr-FR");
			Assert.That(projectSettingsXml, Is.EqualTo(
@"<LexiconProjectSettings>
  <WritingSystems>
    <WritingSystem id=""qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2"">
      <Abbreviation>kal</Abbreviation>
      <LanguageName>Kalaba</LanguageName>
      <ScriptName>Fake</ScriptName>
      <RegionName>Zolrog</RegionName>
      <VariantNames>
        <VariantName>Custom 1</VariantName>
        <VariantName>Custom 2</VariantName>
      </VariantNames>
    </WritingSystem>
  </WritingSystems>
</LexiconProjectSettings>"));

			projectSettingsDataMapper.Remove("qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2");
			Assert.That(projectSettingsXml, Is.EqualTo("<LexiconProjectSettings />"));
		}

		[Test]
		public void Remove_NonexistentWritingSystem_DoesNotUpdateFile()
		{
			string projectSettingsXml =
@"<LexiconProjectSettings>
  <WritingSystems>
    <WritingSystem id=""qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2"">
      <Abbreviation>kal</Abbreviation>
      <LanguageName>Kalaba</LanguageName>
      <ScriptName>Fake</ScriptName>
      <RegionName>Zolrog</RegionName>
      <VariantNames>
        <VariantName>Custom 1</VariantName>
        <VariantName>Custom 2</VariantName>
      </VariantNames>
    </WritingSystem>
  </WritingSystems>
</LexiconProjectSettings>";

			var projectSettingsDataMapper = new ProjectSettingsWritingSystemDataMapper(() => projectSettingsXml, xml => projectSettingsXml = xml);
			projectSettingsDataMapper.Remove("fr-FR");
			Assert.That(projectSettingsXml, Is.EqualTo(
@"<LexiconProjectSettings>
  <WritingSystems>
    <WritingSystem id=""qaa-Qaaa-QM-x-kal-Fake-ZG-var1-var2"">
      <Abbreviation>kal</Abbreviation>
      <LanguageName>Kalaba</LanguageName>
      <ScriptName>Fake</ScriptName>
      <RegionName>Zolrog</RegionName>
      <VariantNames>
        <VariantName>Custom 1</VariantName>
        <VariantName>Custom 2</VariantName>
      </VariantNames>
    </WritingSystem>
  </WritingSystems>
</LexiconProjectSettings>"));
		}
	}
}
