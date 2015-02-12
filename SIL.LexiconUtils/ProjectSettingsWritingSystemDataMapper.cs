﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using SIL.WritingSystems;

namespace SIL.LexiconUtils
{
	public class ProjectSettingsWritingSystemDataMapper : ICustomDataMapper
	{
		private readonly Func<string> _getSettings;
		private readonly Action<string> _setSettings;

		public ProjectSettingsWritingSystemDataMapper(Func<string> getSettings, Action<string> setSettings)
		{
			_getSettings = getSettings;
			_setSettings = setSettings;
		}

		public void Read(WritingSystemDefinition ws)
		{
			string projectSettings = (_getSettings() ?? string.Empty).Trim();
			if (string.IsNullOrEmpty(projectSettings))
				return;

			XElement projectSettingsElem = XElement.Parse(projectSettings);
			Debug.Assert(projectSettingsElem != null);
			XElement wsElem = projectSettingsElem.Elements("WritingSystems").Elements("WritingSystem").FirstOrDefault(e => (string) e.Attribute("id") == ws.Id);
			if (wsElem == null)
				return;

			var abbreviation = (string) wsElem.Element("Abbreviation");
			if (!string.IsNullOrEmpty(abbreviation))
				ws.Abbreviation = abbreviation;

			var languageName = (string) wsElem.Element("LanguageName");
			if (!string.IsNullOrEmpty(languageName) && ws.Language != null && ws.Language.IsPrivateUse)
				ws.Language = new LanguageSubtag(ws.Language, languageName);

			var scriptName = (string) wsElem.Element("ScriptName");
			if (!string.IsNullOrEmpty(scriptName) && ws.Script != null && ws.Script.IsPrivateUse)
				ws.Script = new ScriptSubtag(ws.Script, scriptName);

			var regionName = (string) wsElem.Element("RegionName");
			if (!string.IsNullOrEmpty(regionName) && ws.Region != null && ws.Region.IsPrivateUse)
				ws.Region = new RegionSubtag(ws.Region, regionName);

			XElement[] variantNameElems = wsElem.Elements("VariantNames").Elements("VariantName").ToArray();
			if (variantNameElems.Length > 0)
			{
				int nameIndex = 0;
				for (int i = 0; i < ws.Variants.Count; i++)
				{
					if (ws.Variants[i].IsPrivateUse)
					{
						ws.Variants[i] = new VariantSubtag(ws.Variants[i], (string) variantNameElems[nameIndex]);
						nameIndex++;
						if (nameIndex == variantNameElems.Length)
							break;
					}
				}
			}

			var spellCheckingId = (string) wsElem.Element("SpellCheckingId");
			if (!string.IsNullOrEmpty(spellCheckingId))
				ws.SpellCheckingId = spellCheckingId;

			var legacyMapping = (string) wsElem.Element("LegacyMapping");
			if (!string.IsNullOrEmpty(legacyMapping))
				ws.LegacyMapping = legacyMapping;

			var keyboard = (string) wsElem.Element("Keyboard");
			if (!string.IsNullOrEmpty(keyboard))
				ws.Keyboard = keyboard;
		}

		public void Write(WritingSystemDefinition ws)
		{
			string projectSettings = (_getSettings() ?? string.Empty).Trim();
			XElement projectSettingsElem = !string.IsNullOrEmpty(projectSettings) ? XElement.Parse(projectSettings) : new XElement("LexiconProjectSettings");
			XElement wssElem = projectSettingsElem.Element("WritingSystems");
			if (wssElem == null)
			{
				wssElem = new XElement("WritingSystems");
				projectSettingsElem.Add(wssElem);
			}
			XElement wsElem = wssElem.Elements("WritingSystem").FirstOrDefault(e => (string) e.Attribute("id") == ws.Id);

			if (wsElem == null)
			{
				wsElem = new XElement("WritingSystem", new XAttribute("id", ws.Id));
				wssElem.Add(wsElem);
			}
			wsElem.RemoveNodes();

			if (!string.IsNullOrEmpty(ws.Abbreviation))
				wsElem.Add(new XElement("Abbreviation", ws.Abbreviation));
			if (ws.Language != null && ws.Language.IsPrivateUse && !string.IsNullOrEmpty(ws.Language.Name))
				wsElem.Add(new XElement("LanguageName", ws.Language.Name));
			if (ws.Script != null && ws.Script.IsPrivateUse && !string.IsNullOrEmpty(ws.Script.Name))
				wsElem.Add(new XElement("ScriptName", ws.Script.Name));
			if (ws.Region != null && ws.Region.IsPrivateUse && !string.IsNullOrEmpty(ws.Region.Name))
				wsElem.Add(new XElement("RegionName", ws.Region.Name));
			string[] variantNames = ws.Variants.Where(v => v.IsPrivateUse).Select(v => v.Name).ToArray();
			if (variantNames.Length > 0)
				wsElem.Add(new XElement("VariantNames", variantNames.Select(n => string.IsNullOrEmpty(n) ? new XElement("VariantName") : new XElement("VariantName", n))));
			if (!string.IsNullOrEmpty(ws.SpellCheckingId))
				wsElem.Add(new XElement("SpellCheckingId", ws.SpellCheckingId));
			if (!string.IsNullOrEmpty(ws.LegacyMapping))
				wsElem.Add(new XElement("LegacyMapping", ws.LegacyMapping));
			if (!string.IsNullOrEmpty(ws.Keyboard))
				wsElem.Add(new XElement("Keyboard", ws.Keyboard));

			_setSettings(projectSettingsElem.ToString());
		}

		public void Remove(string wsId)
		{
			string projectSettings = (_getSettings() ?? string.Empty).Trim();
			if (string.IsNullOrEmpty(projectSettings))
				return;

			XElement projectSettingsElem = XElement.Parse(projectSettings);
			Debug.Assert(projectSettingsElem != null);
			XElement wssElem = projectSettingsElem.Element("WritingSystems");
			if (wssElem == null)
				return;

			XElement wsElem = wssElem.Elements("WritingSystem").FirstOrDefault(e => (string) e.Attribute("id") == wsId);
			if (wsElem != null)
			{
				wsElem.Remove();
				if (!wssElem.HasElements)
					wssElem.Remove();

				_setSettings(projectSettingsElem.ToString());
			}
		}
	}
}
