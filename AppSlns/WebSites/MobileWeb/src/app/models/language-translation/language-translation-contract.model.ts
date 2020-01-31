export class LanguageTranslation {
  Key: string;
  value: string;
}

export class LanguageTranslationContract {
  lstApiResourceContract: Array<LanguageTranslation>;
  languageCode: string;
}

export class LanguageParams {
  LangCode: string="AAAB";
  LangName: string="Spanish";
  SelectedLangCode:string="AAAA";
}
