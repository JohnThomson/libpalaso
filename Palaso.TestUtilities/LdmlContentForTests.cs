using System;
using Palaso.WritingSystems;
using Palaso.WritingSystems.Migration.WritingSystemsLdmlV0To1Migration;

namespace Palaso.TestUtilities
{
	public class LdmlContentForTests
	{
		public static string Version0English()
		{
			return Version0("en", String.Empty, String.Empty, String.Empty);
		}

		public static string Version1English()
		{
			return Version1("en", String.Empty, String.Empty, String.Empty);
		}

		static public string Version0(string language, string script, string region, string variant)
		{
			return String.Format(
				@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='{0}' />
	<script type='{1}' />
	<territory type='{2}' />
	<variant type='{3}' />
</identity>
<collations />
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
</special>
</ldml>".Replace('\'', '"'), language, script, region, variant);
		}

		static public string CurrentVersion(string language, string script, string region, string variant)
		{
			return String.Format(
				@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='{0}' />
	<script type='{1}' />
	<territory type='{2}' />
	<variant type='{3}' />
</identity>
<collations />
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:version value='{4}' />
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
</special>
</ldml>".Replace('\'', '"'), language, script, region, variant, WritingSystemDefinition.LatestWritingSystemDefinitionVersion);
		}

		static public string Version0WithLanguageSubtagAndName(string languageSubtag, string languageName)
		{
			return String.Format(
				@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='{0}' />
</identity>
<collations />
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:languageName value='{1}' />
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
</special>
</ldml>".Replace('\'', '"'), languageSubtag, languageName);
		}

		static public string Version99Default()
		{
			return
				@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='en' />
</identity>
<collations />
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:version value='99' />
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
</special>
</ldml>".Replace('\'', '"');
		}

		static public string Version0WithAllSortsOfDatathatdoesNotNeedSpecialAttention(string language, string script, string region, string variant)
		{
			return String.Format(
				@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='{0}' />
	<script type='{1}' />
	<territory type='{2}' />
	<variant type='{3}' />
</identity>
<layout>
	<orientation characters='left-to-right'/>
</layout>
<collations>
	<collation>
		<base>
			<alias source=''/>
		</base>
		<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
			<palaso:sortRulesType value='OtherLanguage' />
		</special>
	</collation>
</collations>
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:abbreviation value='la' />
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
	<palaso:defaultKeyboard value='bogusKeyboard' />
	<palaso:isLegacyEncoded value='true' />
	<palaso:languageName value='language' />
	<palaso:spellCheckingId value='ol' />
</special>
</ldml>".Replace('\'', '"'), language, script, region, variant);
		}

		static public string Version0WithCollationInfo(WritingSystemDefinitionV0.SortRulesType sortType)
		{
			string collationelement = GetCollationElementXml(sortType);

			return String.Format(
				@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='en' />
</identity>
<layout>
	<orientation characters='left-to-right'/>
</layout>
<collations>
	{0}
</collations>
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:abbreviation value='la' />
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
	<palaso:defaultKeyboard value='bogusKeyboard' />
	<palaso:isLegacyEncoded value='true' />
	<palaso:languageName value='language' />
	<palaso:spellCheckingId value='ol' />
</special>
</ldml>".Replace('\'', '"'), collationelement);
		}

		static public string Version0WithLdmlInfoWeDontCareAbout(string language, string script, string region, string variant)
		{
			return String.Format(
				@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='{0}' />
	<script type='{1}' />
	<territory type='{2}' />
	<variant type='{3}' />
</identity>
<fallback><testing>fallback</testing></fallback>
<localeDisplayNames><testing>localeDisplayNames</testing></localeDisplayNames>
<layout><testing>layout</testing></layout>
<characters><testing>characters</testing></characters>
<delimiters><testing>delimiters</testing></delimiters>
<measurement><testing>measurement</testing></measurement>
<dates><testing>dates</testing></dates>
<numbers><testing>numbers</testing></numbers>
<units><testing>units</testing></units>
<listPatterns><testing>listPatterns</testing></listPatterns>
<collations />
<posix><testing>posix</testing></posix>
<segmentations><testing>segmentations</testing></segmentations>
<rbnf><testing>rbnf</testing></rbnf>
<references><testing>references</testing></references>
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
</special>
</ldml>".Replace('\'', '"'), language, script, region, variant);
		}

		private static string GetCollationElementXml(WritingSystemDefinitionV0.SortRulesType sortType)
		{
			string collationelement = String.Empty;
			switch (sortType)
			{
				case WritingSystemDefinitionV0.SortRulesType.DefaultOrdering:
					collationelement = String.Empty;
					break;
				case WritingSystemDefinitionV0.SortRulesType.CustomICU:
					collationelement =
						@"<collation>
	<base>
		<alias source='' />
	</base>
	<rules>
		<reset>ab</reset><s>q</s><t>Q</t><reset>ad</reset><t>AD</t><p>x</p><t>X</t>
	</rules>
	<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
		<palaso:sortRulesType value='CustomICU' />
	</special>
</collation>";
					break;
				case WritingSystemDefinitionV0.SortRulesType.CustomSimple:
					collationelement =
						@"<collation>
	<base>
		<alias source='' />
	</base>
	<rules>
		<reset before='primary'><first_non_ignorable /></reset><p>a</p><s>A</s><p>b</p><s>B</s><p>o</p><s>O</s><p>m</p><s>M</s>
	</rules>
	<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
		<palaso:sortRulesType value='CustomSimple' />
	</special>
</collation>";
					break;
				case WritingSystemDefinitionV0.SortRulesType.OtherLanguage:
					collationelement =
						@"<collation>
		<base>
			<alias source='en'/>
		</base>
		<rules>
			<reset before='primary'><first_non_ignorable /></reset><p>a</p><s>A</s><p>b</p><s>B</s><p>o</p><s>O</s><p>m</p><s>M</s>
		</rules>
		<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
			<palaso:sortRulesType value='OtherLanguage' />
		</special>
	</collation>
";
					break;
			}
			return collationelement;
		}

		static private string Version1(string language, string script, string region, string variant)
		{
			return
				String.Format(@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='{0}' />
	<script type='{1}' />
	<territory type='{2}' />
	<variant type='{3}' />
</identity>
<collations />
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:version value='1' />
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
</special>
</ldml>".Replace('\'', '"'), language, script, region, variant);
		}

		static public string Version2(string language, string script, string region, string variant)
		{
			return
				String.Format(@"<?xml version='1.0' encoding='utf-8'?>
<ldml>
<identity>
	<version number='' />
	<generation date='0001-01-01T00:00:00' />
	<language type='{0}' />
	<script type='{1}' />
	<territory type='{2}' />
	<variant type='{3}' />
</identity>
<collations />
<special xmlns:palaso='urn://palaso.org/ldmlExtensions/v1'>
	<palaso:version value='2' />
	<palaso:defaultFontFamily value='Arial' />
	<palaso:defaultFontSize value='12' />
</special>
</ldml>".Replace('\'', '"'), language, script, region, variant);
		}
	}
}