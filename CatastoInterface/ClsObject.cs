using System;
using System.Collections.Generic;

namespace CatastoInterface
{
    public class Elaborazione
    {
        #region  Constant
        public static class Esito
        {
            public static string OK = "OK";
            public static string KO = "KO";
        }
        #endregion
        #region Variables
        public int ID { get; set; }
        public string IDEnte { get; set; }
        public string IDCatastale { get; set; }
        public DateTime InizioUpload { get; set; }
        public DateTime FineUpload { get; set; }
        public string EsitoUpload { get; set; }
        public DateTime InizioImport { get; set; }
        public DateTime FineImport { get; set; }
        public string EsitoImport { get; set; }
        public DateTime InizioConvert { get; set; }
        public DateTime FineConvert { get; set; }
        public string EsitoConvert { get; set; }
        public DateTime InizioIncrocio { get; set; }
        public DateTime FineIncrocio { get; set; }
        public string EsitoIncrocio { get; set; }
        public DateTime InizioEstrazioneDichWork { get; set; }
        public DateTime FineEstrazioneDichWork { get; set; }
        public string EsitoEstrazioneDichWork { get; set; }
        public DateTime InizioEstrazioneTitVSSog { get; set; }
        public DateTime FineEstrazioneTitVSSog { get; set; }
        public string EsitoEstrazioneTitVSSog { get; set; }
        public DateTime InizioEstrazioneSogVSTit { get; set; }
        public DateTime FineEstrazioneSogVSTit { get; set; }
        public string EsitoEstrazioneSogVSTit { get; set; }
        public DateTime InizioEstrazioneTitVSFab { get; set; }
        public DateTime FineEstrazioneTitVSFab { get; set; }
        public string EsitoEstrazioneTitVSFab { get; set; }
        public DateTime InizioEstrazioneTitVSTer { get; set; }
        public DateTime FineEstrazioneTitVSTer { get; set; }
        public string EsitoEstrazioneTitVSTer { get; set; }
        public DateTime InizioEstrazioneFabVSTit { get; set; }
        public DateTime FineEstrazioneFabVSTit { get; set; }
        public string EsitoEstrazioneFabVSTit { get; set; }
        public DateTime InizioEstrazioneTerVSTit { get; set; }
        public DateTime FineEstrazioneTerVSTit { get; set; }
        public string EsitoEstrazioneTerVSTit { get; set; }
        public DateTime InizioEstrazioneDirittoMancante { get; set; }
        public DateTime FineEstrazioneDirittoMancante { get; set; }
        public string EsitoEstrazioneDirittoMancante { get; set; }
        public DateTime InizioEstrazionePossMancante { get; set; }
        public DateTime FineEstrazionePossMancante { get; set; }
        public string EsitoEstrazionePossMancante { get; set; }
        public DateTime InizioEstrazioneComunioneMancante { get; set; }
        public DateTime FineEstrazioneComunioneMancante { get; set; }
        public string EsitoEstrazioneComunioneMancante { get; set; }
        public List<ElaborazioneFile> ListFiles { get; set; }
        #endregion
        #region Construtor
        public Elaborazione()
        {
            Reset();
        }
        public void Reset()
        {
            ID = default(int);
            IDEnte = IDCatastale = string.Empty;
            EsitoUpload = EsitoImport = EsitoConvert = EsitoIncrocio = EsitoEstrazioneDichWork = EsitoEstrazioneTitVSSog = EsitoEstrazioneSogVSTit = EsitoEstrazioneTitVSFab = EsitoEstrazioneFabVSTit = EsitoEstrazioneTitVSTer = EsitoEstrazioneTerVSTit = EsitoEstrazioneDirittoMancante = EsitoEstrazionePossMancante = EsitoEstrazioneComunioneMancante = string.Empty;
            InizioUpload = FineUpload = InizioImport = FineImport = InizioConvert = FineConvert = InizioIncrocio = FineIncrocio = InizioEstrazioneDichWork = FineEstrazioneDichWork = InizioEstrazioneTitVSSog = FineEstrazioneTitVSSog = InizioEstrazioneSogVSTit = FineEstrazioneSogVSTit = InizioEstrazioneTitVSFab = FineEstrazioneTitVSFab = InizioEstrazioneFabVSTit = FineEstrazioneFabVSTit = InizioEstrazioneTitVSTer = FineEstrazioneTitVSTer = InizioEstrazioneTerVSTit = FineEstrazioneTerVSTit = InizioEstrazioneDirittoMancante = FineEstrazioneDirittoMancante = InizioEstrazionePossMancante = FineEstrazionePossMancante = InizioEstrazioneComunioneMancante = FineEstrazioneComunioneMancante = DateTime.MaxValue;
            ListFiles = new List<ElaborazioneFile>();
        }
        #endregion
    }
    public class ElaborazioneFile
    {
        #region  Constant
        public static class Estensioni
        {
            public static string Fabbricati = "FAB";
            public static string Storico = "STO";
            public static string Terreni = "TER";
            public static string Titoli = "TIT";
            public static string Soggetti = "SOG";
            public static string Dichiarazioni = "CSV";
        }
        #endregion
        #region Variables
        public int ID { get; set; }
        public int IDElaborazione { get; set; }
        public string NameFile { get; set; }
        public DateTime InizioImport { get; set; }
        public DateTime FineImport { get; set; }
        public string EsitoImport { get; set; }
        #endregion
        #region Construtor
        public ElaborazioneFile()
        {
            Reset();
        }
        public void Reset()
        {
            ID = IDElaborazione = default(int);
            NameFile = EsitoImport = string.Empty;
            InizioImport = FineImport = DateTime.MaxValue;
        }
        #endregion
    }
    public class Fabbricato
    {
        #region Variables
        public int ID { get; set; }
        public int IDElaborazione { get; set; }
        public string IDCatastale { get; set; }
        public string Sezione { get; set; }
        public string IDImmobile { get; set; }
        public string TipoImmobile { get; set; }
        public string Progressivo { get; set; }
        public string Zona { get; set; }
        public string Categoria { get; set; }
        public string Classe { get; set; }
        public string Consistenza { get; set; }
        public string Superficie { get; set; }
        public string RenditaLire { get; set; }
        public string RenditaEuro { get; set; }
        public string Lotto { get; set; }
        public string Edificio { get; set; }
        public string Scala { get; set; }
        public string Interno1 { get; set; }
        public string Interno2 { get; set; }
        public string Piano1 { get; set; }
        public string Piano2 { get; set; }
        public string Piano3 { get; set; }
        public string Piano4 { get; set; }
        public string DataInizioEfficacia { get; set; }
        public string DataInizioRegistrazioneAtti { get; set; }
        public string TipoNotaInizio { get; set; }
        public string NumeroNotaInizio { get; set; }
        public string ProgressivoNotaInizio { get; set; }
        public string AnnoNotaInizio { get; set; }
        public string DataFineEfficacia { get; set; }
        public string DataFineRegistrazioneAtti { get; set; }
        public string TipoNotaFine { get; set; }
        public string NumeroNotaFine { get; set; }
        public string ProgressivoNotaFine { get; set; }
        public string AnnoNotaFine { get; set; }
        public string Partita { get; set; }
        public string Annotazione { get; set; }
        public string IDMutazioneIniziale { get; set; }
        public string IDMutazioneFinale { get; set; }
        public string ProtocolloNotifica { get; set; }
        public string DataNotifica { get; set; }
        public string CodiceCausaleAttoGenerante { get; set; }
        public string DescrizioneAttoGenerante { get; set; }
        public string CodiceCausaleAttoConclusivo { get; set; }
        public string DescrizioneAttoConclusivo { get; set; }
        public string FlagClassamento { get; set; }
        #region "tabella identificativi(max10elementi)"
        public string SezioneUrbana1 { get; set; }
        public string Foglio1 { get; set; }
        public string Numero1 { get; set; }
        public string Denominatore1 { get; set; }
        public string Subalterno1 { get; set; }
        public string Edificialita1 { get; set; }
        public string SezioneUrbana2 { get; set; }
        public string Foglio2 { get; set; }
        public string Numero2 { get; set; }
        public string Denominatore2 { get; set; }
        public string Subalterno2 { get; set; }
        public string Edificialita2 { get; set; }
        public string SezioneUrbana3 { get; set; }
        public string Foglio3 { get; set; }
        public string Numero3 { get; set; }
        public string Denominatore3 { get; set; }
        public string Subalterno3 { get; set; }
        public string Edificialita3 { get; set; }
        public string SezioneUrbana4 { get; set; }
        public string Foglio4 { get; set; }
        public string Numero4 { get; set; }
        public string Denominatore4 { get; set; }
        public string Subalterno4 { get; set; }
        public string Edificialita4 { get; set; }
        public string SezioneUrbana5 { get; set; }
        public string Foglio5 { get; set; }
        public string Numero5 { get; set; }
        public string Denominatore5 { get; set; }
        public string Subalterno5 { get; set; }
        public string Edificialita5 { get; set; }
        public string SezioneUrbana6 { get; set; }
        public string Foglio6 { get; set; }
        public string Numero6 { get; set; }
        public string Denominatore6 { get; set; }
        public string Subalterno6 { get; set; }
        public string Edificialita6 { get; set; }
        public string SezioneUrbana7 { get; set; }
        public string Foglio7 { get; set; }
        public string Numero7 { get; set; }
        public string Denominatore7 { get; set; }
        public string Subalterno7 { get; set; }
        public string Edificialita7 { get; set; }
        public string SezioneUrbana8 { get; set; }
        public string Foglio8 { get; set; }
        public string Numero8 { get; set; }
        public string Denominatore8 { get; set; }
        public string Subalterno8 { get; set; }
        public string Edificialita8 { get; set; }
        public string SezioneUrbana9 { get; set; }
        public string Foglio9 { get; set; }
        public string Numero9 { get; set; }
        public string Denominatore9 { get; set; }
        public string Subalterno9 { get; set; }
        public string Edificialita9 { get; set; }
        public string SezioneUrbana10 { get; set; }
        public string Foglio10 { get; set; }
        public string Numero10 { get; set; }
        public string Denominatore10 { get; set; }
        public string Subalterno10 { get; set; }
        public string Edificialita10 { get; set; }
        #endregion
        #region "tabella indirizzi(4elementi)"
        public string Toponimo1 { get; set; }
        public string Indirizzo1 { get; set; }
        public string Civico11 { get; set; }
        public string Civico21 { get; set; }
        public string Civico31 { get; set; }
        public string CodiceStrada1 { get; set; }
        public string Toponimo2 { get; set; }
        public string Indirizzo2 { get; set; }
        public string Civico12 { get; set; }
        public string Civico22 { get; set; }
        public string Civico32 { get; set; }
        public string CodiceStrada2 { get; set; }
        public string Toponimo3 { get; set; }
        public string Indirizzo3 { get; set; }
        public string Civico13 { get; set; }
        public string Civico23 { get; set; }
        public string Civico33 { get; set; }
        public string CodiceStrada3 { get; set; }
        public string Toponimo4 { get; set; }
        public string Indirizzo4 { get; set; }
        public string Civico14 { get; set; }
        public string Civico24 { get; set; }
        public string Civico34 { get; set; }
        public string CodiceStrada4 { get; set; }
        #endregion
        #region "tabella utilita' comuni(10elementi)"
        public string UC_SezioneUrbana1 { get; set; }
        public string UC_Foglio1 { get; set; }
        public string UC_Numero1 { get; set; }
        public string UC_Denominatore1 { get; set; }
        public string UC_Subalterno1 { get; set; }
        public string UC_SezioneUrbana2 { get; set; }
        public string UC_Foglio2 { get; set; }
        public string UC_Numero2 { get; set; }
        public string UC_Denominatore2 { get; set; }
        public string UC_Subalterno2 { get; set; }
        public string UC_SezioneUrbana3 { get; set; }
        public string UC_Foglio3 { get; set; }
        public string UC_Numero3 { get; set; }
        public string UC_Denominatore3 { get; set; }
        public string UC_Subalterno3 { get; set; }
        public string UC_SezioneUrbana4 { get; set; }
        public string UC_Foglio4 { get; set; }
        public string UC_Numero4 { get; set; }
        public string UC_Denominatore4 { get; set; }
        public string UC_Subalterno4 { get; set; }
        public string UC_SezioneUrbana5 { get; set; }
        public string UC_Foglio5 { get; set; }
        public string UC_Numero5 { get; set; }
        public string UC_Denominatore5 { get; set; }
        public string UC_Subalterno5 { get; set; }
        public string UC_SezioneUrbana6 { get; set; }
        public string UC_Foglio6 { get; set; }
        public string UC_Numero6 { get; set; }
        public string UC_Denominatore6 { get; set; }
        public string UC_Subalterno6 { get; set; }
        public string UC_SezioneUrbana7 { get; set; }
        public string UC_Foglio7 { get; set; }
        public string UC_Numero7 { get; set; }
        public string UC_Denominatore7 { get; set; }
        public string UC_Subalterno7 { get; set; }
        public string UC_SezioneUrbana8 { get; set; }
        public string UC_Foglio8 { get; set; }
        public string UC_Numero8 { get; set; }
        public string UC_Denominatore8 { get; set; }
        public string UC_Subalterno8 { get; set; }
        public string UC_SezioneUrbana9 { get; set; }
        public string UC_Foglio9 { get; set; }
        public string UC_Numero9 { get; set; }
        public string UC_Denominatore9 { get; set; }
        public string UC_Subalterno9 { get; set; }
        public string UC_SezioneUrbana10 { get; set; }
        public string UC_Foglio10 { get; set; }
        public string UC_Numero10 { get; set; }
        public string UC_Denominatore10 { get; set; }
        public string UC_Subalterno10 { get; set; }
        #endregion
        #region "tabella riserve(10elementi)"
        public string CodiceRiserva1 { get; set; }
        public string PartitaIscrizioneRiserva1 { get; set; }
        public string CodiceRiserva2 { get; set; }
        public string PartitaIscrizioneRiserva2 { get; set; }
        public string CodiceRiserva3 { get; set; }
        public string PartitaIscrizioneRiserva3 { get; set; }
        public string CodiceRiserva4 { get; set; }
        public string PartitaIscrizioneRiserva4 { get; set; }
        public string CodiceRiserva5 { get; set; }
        public string PartitaIscrizioneRiserva5 { get; set; }
        public string CodiceRiserva6 { get; set; }
        public string PartitaIscrizioneRiserva6 { get; set; }
        public string CodiceRiserva7 { get; set; }
        public string PartitaIscrizioneRiserva7 { get; set; }
        public string CodiceRiserva8 { get; set; }
        public string PartitaIscrizioneRiserva8 { get; set; }
        public string CodiceRiserva9 { get; set; }
        public string PartitaIscrizioneRiserva9 { get; set; }
        public string CodiceRiserva10 { get; set; }
        public string PartitaIscrizioneRiserva10 { get; set; }
        #endregion
        #endregion
        #region Construtor
        public Fabbricato()
        {
            Reset();
        }
        public void Reset()
        {
            ID =IDElaborazione= default(int);
            IDCatastale = Sezione = IDImmobile = TipoImmobile = Progressivo = Zona = Categoria = Classe = Consistenza = Superficie = RenditaLire = RenditaEuro = Lotto = Edificio = Scala = Interno1 = Interno2 = Piano1 = Piano2 = Piano3 = Piano4 = DataInizioEfficacia = DataInizioRegistrazioneAtti = TipoNotaInizio = NumeroNotaInizio = ProgressivoNotaInizio = AnnoNotaInizio = DataFineEfficacia = DataFineRegistrazioneAtti = TipoNotaFine = NumeroNotaFine = ProgressivoNotaFine = AnnoNotaFine = Partita = Annotazione = IDMutazioneIniziale = IDMutazioneFinale = ProtocolloNotifica = DataNotifica = CodiceCausaleAttoGenerante = DescrizioneAttoGenerante = CodiceCausaleAttoConclusivo = DescrizioneAttoConclusivo = FlagClassamento = string.Empty;
            SezioneUrbana1 = Foglio1 = Numero1 = Denominatore1 = Subalterno1 = Edificialita1 = SezioneUrbana2 = Foglio2 = Numero2 = Denominatore2 = Subalterno2 = Edificialita2 = SezioneUrbana3 = Foglio3 = Numero3 = Denominatore3 = Subalterno3 = Edificialita3 = SezioneUrbana4 = Foglio4 = Numero4 = Denominatore4 = Subalterno4 = Edificialita4 = SezioneUrbana5 = Foglio5 = Numero5 = Denominatore5 = Subalterno5 = Edificialita5 = SezioneUrbana6 = Foglio6 = Numero6 = Denominatore6 = Subalterno6 = Edificialita6 = SezioneUrbana7 = Foglio7 = Numero7 = Denominatore7 = Subalterno7 = Edificialita7 = SezioneUrbana8 = Foglio8 = Numero8 = Denominatore8 = Subalterno8 = Edificialita8 = SezioneUrbana9 = Foglio9 = Numero9 = Denominatore9 = Subalterno9 = Edificialita9 = SezioneUrbana10 = Foglio10 = Numero10 = Denominatore10 = Subalterno10 = Edificialita10 = string.Empty;
            Toponimo1 = Indirizzo1 = Civico11 = Civico21 = Civico31 = CodiceStrada1 = Toponimo2 = Indirizzo2 = Civico12 = Civico22 = Civico32 = CodiceStrada2 = Toponimo3 = Indirizzo3 = Civico13 = Civico23 = Civico33 = CodiceStrada3 = Toponimo4 = Indirizzo4 = Civico14 = Civico24 = Civico34 = CodiceStrada4 = string.Empty;
            UC_SezioneUrbana1 = UC_Foglio1 = UC_Numero1 = UC_Denominatore1 = UC_Subalterno1 = UC_SezioneUrbana2 = UC_Foglio2 = UC_Numero2 = UC_Denominatore2 = UC_Subalterno2 = UC_SezioneUrbana3 = UC_Foglio3 = UC_Numero3 = UC_Denominatore3 = UC_Subalterno3 = UC_SezioneUrbana4 = UC_Foglio4 = UC_Numero4 = UC_Denominatore4 = UC_Subalterno4 = UC_SezioneUrbana5 = UC_Foglio5 = UC_Numero5 = UC_Denominatore5 = UC_Subalterno5 = UC_SezioneUrbana6 = UC_Foglio6 = UC_Numero6 = UC_Denominatore6 = UC_Subalterno6 = UC_SezioneUrbana7 = UC_Foglio7 = UC_Numero7 = UC_Denominatore7 = UC_Subalterno7 = UC_SezioneUrbana8 = UC_Foglio8 = UC_Numero8 = UC_Denominatore8 = UC_Subalterno8 = UC_SezioneUrbana9 = UC_Foglio9 = UC_Numero9 = UC_Denominatore9 = UC_Subalterno9 = UC_SezioneUrbana10 = UC_Foglio10 = UC_Numero10 = UC_Denominatore10 = UC_Subalterno10 = string.Empty;
            CodiceRiserva1 = PartitaIscrizioneRiserva1 = CodiceRiserva2 = PartitaIscrizioneRiserva2 = CodiceRiserva3 = PartitaIscrizioneRiserva3 = CodiceRiserva4 = PartitaIscrizioneRiserva4 = CodiceRiserva5 = PartitaIscrizioneRiserva5 = CodiceRiserva6 = PartitaIscrizioneRiserva6 = CodiceRiserva7 = PartitaIscrizioneRiserva7 = CodiceRiserva8 = PartitaIscrizioneRiserva8 = CodiceRiserva9 = PartitaIscrizioneRiserva9 = CodiceRiserva10 = PartitaIscrizioneRiserva10 = string.Empty;
        }
        #endregion
    }
    public class Terreno
    {
        #region Variables
        public int ID { get; set; }
        public int IDElaborazione { get; set; }
        public string IDCatastale { get; set; }
        public string Sezione { get; set; }
        public string IDImmobile { get; set; }
        public string TipoImmobile { get; set; }
        public string Progressivo { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Denominatore { get; set; }
        public string Subalterno { get; set; }
        public string Edificialita { get; set; }
        public string Qualita { get; set; }
        public string Classe { get; set; }
        public string Ettari { get; set; }
        public string Are { get; set; }
        public string Centiare { get; set; }
        public string FlagReddito { get; set; }
        public string FlagPorzione { get; set; }
        public string FlagDeduzioni { get; set; }
        public string RedditoDominicaleLire { get; set; }
        public string RedditoAgrarioLire { get; set; }
        public string RedditoDominicaleEuro { get; set; }
        public string RedditoAgrarioEuro { get; set; }
        public string DataInizioEfficacia { get; set; }
        public string DataInizioRegistrazioneAtti { get; set; }
        public string TipoNotaInizio { get; set; }
        public string NumeroNotaInizio { get; set; }
        public string ProgressivoNotaInizio { get; set; }
        public string AnnoNotaInizio { get; set; }
        public string DataFineEfficacia { get; set; }
        public string DataFineRegistrazioneAtti { get; set; }
        public string TipoNotaFine { get; set; }
        public string NumeroNotaFine { get; set; }
        public string ProgressivoNotaFine { get; set; }
        public string AnnoNotaFine { get; set; }
        public string Partita { get; set; }
        public string Annotazione { get; set; }
        public string IDMutazioneIniziale { get; set; }
        public string IDMutazioneFinale { get; set; }
        public string CodiceCausaleAttoGenerante { get; set; }
        public string DescrizioneAttoGenerante { get; set; }
        public string CodiceCausaleAttoConclusivo { get; set; }
        public string DescrizioneAttoConclusivo { get; set; }
        #region "deduzioni della particella(7elementi)"
        public string SimboloDeduzione1 { get; set; }
        public string SimboloDeduzione2 { get; set; }
        public string SimboloDeduzione3 { get; set; }
        public string SimboloDeduzione4 { get; set; }
        public string SimboloDeduzione5 { get; set; }
        public string SimboloDeduzione6 { get; set; }
        public string SimboloDeduzione7 { get; set; }
        #endregion
        #region "riserve della particella(30elementi)"
        public string CodiceRiserva1 { get; set; }
        public string PartitaIscrizioneRiserva1 { get; set; }
        public string CodiceRiserva2 { get; set; }
        public string PartitaIscrizioneRiserva2 { get; set; }
        public string CodiceRiserva3 { get; set; }
        public string PartitaIscrizioneRiserva3 { get; set; }
        public string CodiceRiserva4 { get; set; }
        public string PartitaIscrizioneRiserva4 { get; set; }
        public string CodiceRiserva5 { get; set; }
        public string PartitaIscrizioneRiserva5 { get; set; }
        public string CodiceRiserva6 { get; set; }
        public string PartitaIscrizioneRiserva6 { get; set; }
        public string CodiceRiserva7 { get; set; }
        public string PartitaIscrizioneRiserva7 { get; set; }
        public string CodiceRiserva8 { get; set; }
        public string PartitaIscrizioneRiserva8 { get; set; }
        public string CodiceRiserva9 { get; set; }
        public string PartitaIscrizioneRiserva9 { get; set; }
        public string CodiceRiserva10 { get; set; }
        public string PartitaIscrizioneRiserva10 { get; set; }
        public string CodiceRiserva11 { get; set; }
        public string PartitaIscrizioneRiserva11 { get; set; }
        public string CodiceRiserva12 { get; set; }
        public string PartitaIscrizioneRiserva12 { get; set; }
        public string CodiceRiserva13 { get; set; }
        public string PartitaIscrizioneRiserva13 { get; set; }
        public string CodiceRiserva14 { get; set; }
        public string PartitaIscrizioneRiserva14 { get; set; }
        public string CodiceRiserva15 { get; set; }
        public string PartitaIscrizioneRiserva15 { get; set; }
        public string CodiceRiserva16 { get; set; }
        public string PartitaIscrizioneRiserva16 { get; set; }
        public string CodiceRiserva17 { get; set; }
        public string PartitaIscrizioneRiserva17 { get; set; }
        public string CodiceRiserva18 { get; set; }
        public string PartitaIscrizioneRiserva18 { get; set; }
        public string CodiceRiserva19 { get; set; }
        public string PartitaIscrizioneRiserva19 { get; set; }
        public string CodiceRiserva20 { get; set; }
        public string PartitaIscrizioneRiserva20 { get; set; }
        public string CodiceRiserva21 { get; set; }
        public string PartitaIscrizioneRiserva21 { get; set; }
        public string CodiceRiserva22 { get; set; }
        public string PartitaIscrizioneRiserva22 { get; set; }
        public string CodiceRiserva23 { get; set; }
        public string PartitaIscrizioneRiserva23 { get; set; }
        public string CodiceRiserva24 { get; set; }
        public string PartitaIscrizioneRiserva24 { get; set; }
        public string CodiceRiserva25 { get; set; }
        public string PartitaIscrizioneRiserva25 { get; set; }
        public string CodiceRiserva26 { get; set; }
        public string PartitaIscrizioneRiserva26 { get; set; }
        public string CodiceRiserva27 { get; set; }
        public string PartitaIscrizioneRiserva27 { get; set; }
        public string CodiceRiserva28 { get; set; }
        public string PartitaIscrizioneRiserva28 { get; set; }
        public string CodiceRiserva29 { get; set; }
        public string PartitaIscrizioneRiserva29 { get; set; }
        public string CodiceRiserva30 { get; set; }
        public string PartitaIscrizioneRiserva30 { get; set; }
        #endregion
        #region "porzioni della particella(20elementi)"
        public string IdentificativoPorzione1 { get; set; }
        public string QualitaPorzione1 { get; set; }
        public string ClassePorzione1 { get; set; }
        public string EttariPorzione1 { get; set; }
        public string ArePorzione1 { get; set; }
        public string CentiarePorzione1 { get; set; }
        public string RedditoDominicaleEuroPorzione1 { get; set; }
        public string RedditoAgrarioEuroPorzione1 { get; set; }
        public string IdentificativoPorzione2 { get; set; }
        public string QualitaPorzione2 { get; set; }
        public string ClassePorzione2 { get; set; }
        public string EttariPorzione2 { get; set; }
        public string ArePorzione2 { get; set; }
        public string CentiarePorzione2 { get; set; }
        public string RedditoDominicaleEuroPorzione2 { get; set; }
        public string RedditoAgrarioEuroPorzione2 { get; set; }
        public string IdentificativoPorzione3 { get; set; }
        public string QualitaPorzione3 { get; set; }
        public string ClassePorzione3 { get; set; }
        public string EttariPorzione3 { get; set; }
        public string ArePorzione3 { get; set; }
        public string CentiarePorzione3 { get; set; }
        public string RedditoDominicaleEuroPorzione3 { get; set; }
        public string RedditoAgrarioEuroPorzione3 { get; set; }
        public string IdentificativoPorzione4 { get; set; }
        public string QualitaPorzione4 { get; set; }
        public string ClassePorzione4 { get; set; }
        public string EttariPorzione4 { get; set; }
        public string ArePorzione4 { get; set; }
        public string CentiarePorzione4 { get; set; }
        public string RedditoDominicaleEuroPorzione4 { get; set; }
        public string RedditoAgrarioEuroPorzione4 { get; set; }
        public string IdentificativoPorzione5 { get; set; }
        public string QualitaPorzione5 { get; set; }
        public string ClassePorzione5 { get; set; }
        public string EttariPorzione5 { get; set; }
        public string ArePorzione5 { get; set; }
        public string CentiarePorzione5 { get; set; }
        public string RedditoDominicaleEuroPorzione5 { get; set; }
        public string RedditoAgrarioEuroPorzione5 { get; set; }
        public string IdentificativoPorzione6 { get; set; }
        public string QualitaPorzione6 { get; set; }
        public string ClassePorzione6 { get; set; }
        public string EttariPorzione6 { get; set; }
        public string ArePorzione6 { get; set; }
        public string CentiarePorzione6 { get; set; }
        public string RedditoDominicaleEuroPorzione6 { get; set; }
        public string RedditoAgrarioEuroPorzione6 { get; set; }
        public string IdentificativoPorzione7 { get; set; }
        public string QualitaPorzione7 { get; set; }
        public string ClassePorzione7 { get; set; }
        public string EttariPorzione7 { get; set; }
        public string ArePorzione7 { get; set; }
        public string CentiarePorzione7 { get; set; }
        public string RedditoDominicaleEuroPorzione7 { get; set; }
        public string RedditoAgrarioEuroPorzione7 { get; set; }
        public string IdentificativoPorzione8 { get; set; }
        public string QualitaPorzione8 { get; set; }
        public string ClassePorzione8 { get; set; }
        public string EttariPorzione8 { get; set; }
        public string ArePorzione8 { get; set; }
        public string CentiarePorzione8 { get; set; }
        public string RedditoDominicaleEuroPorzione8 { get; set; }
        public string RedditoAgrarioEuroPorzione8 { get; set; }
        public string IdentificativoPorzione9 { get; set; }
        public string QualitaPorzione9 { get; set; }
        public string ClassePorzione9 { get; set; }
        public string EttariPorzione9 { get; set; }
        public string ArePorzione9 { get; set; }
        public string CentiarePorzione9 { get; set; }
        public string RedditoDominicaleEuroPorzione9 { get; set; }
        public string RedditoAgrarioEuroPorzione9 { get; set; }
        public string IdentificativoPorzione10 { get; set; }
        public string QualitaPorzione10 { get; set; }
        public string ClassePorzione10 { get; set; }
        public string EttariPorzione10 { get; set; }
        public string ArePorzione10 { get; set; }
        public string CentiarePorzione10 { get; set; }
        public string RedditoDominicaleEuroPorzione10 { get; set; }
        public string RedditoAgrarioEuroPorzione10 { get; set; }
        public string IdentificativoPorzione11 { get; set; }
        public string QualitaPorzione11 { get; set; }
        public string ClassePorzione11 { get; set; }
        public string EttariPorzione11 { get; set; }
        public string ArePorzione11 { get; set; }
        public string CentiarePorzione11 { get; set; }
        public string RedditoDominicaleEuroPorzione11 { get; set; }
        public string RedditoAgrarioEuroPorzione11 { get; set; }
        public string IdentificativoPorzione12 { get; set; }
        public string QualitaPorzione12 { get; set; }
        public string ClassePorzione12 { get; set; }
        public string EttariPorzione12 { get; set; }
        public string ArePorzione12 { get; set; }
        public string CentiarePorzione12 { get; set; }
        public string RedditoDominicaleEuroPorzione12 { get; set; }
        public string RedditoAgrarioEuroPorzione12 { get; set; }
        public string IdentificativoPorzione13 { get; set; }
        public string QualitaPorzione13 { get; set; }
        public string ClassePorzione13 { get; set; }
        public string EttariPorzione13 { get; set; }
        public string ArePorzione13 { get; set; }
        public string CentiarePorzione13 { get; set; }
        public string RedditoDominicaleEuroPorzione13 { get; set; }
        public string RedditoAgrarioEuroPorzione13 { get; set; }
        public string IdentificativoPorzione14 { get; set; }
        public string QualitaPorzione14 { get; set; }
        public string ClassePorzione14 { get; set; }
        public string EttariPorzione14 { get; set; }
        public string ArePorzione14 { get; set; }
        public string CentiarePorzione14 { get; set; }
        public string RedditoDominicaleEuroPorzione14 { get; set; }
        public string RedditoAgrarioEuroPorzione14 { get; set; }
        public string IdentificativoPorzione15 { get; set; }
        public string QualitaPorzione15 { get; set; }
        public string ClassePorzione15 { get; set; }
        public string EttariPorzione15 { get; set; }
        public string ArePorzione15 { get; set; }
        public string CentiarePorzione15 { get; set; }
        public string RedditoDominicaleEuroPorzione15 { get; set; }
        public string RedditoAgrarioEuroPorzione15 { get; set; }
        public string IdentificativoPorzione16 { get; set; }
        public string QualitaPorzione16 { get; set; }
        public string ClassePorzione16 { get; set; }
        public string EttariPorzione16 { get; set; }
        public string ArePorzione16 { get; set; }
        public string CentiarePorzione16 { get; set; }
        public string RedditoDominicaleEuroPorzione16 { get; set; }
        public string RedditoAgrarioEuroPorzione16 { get; set; }
        public string IdentificativoPorzione17 { get; set; }
        public string QualitaPorzione17 { get; set; }
        public string ClassePorzione17 { get; set; }
        public string EttariPorzione17 { get; set; }
        public string ArePorzione17 { get; set; }
        public string CentiarePorzione17 { get; set; }
        public string RedditoDominicaleEuroPorzione17 { get; set; }
        public string RedditoAgrarioEuroPorzione17 { get; set; }
        public string IdentificativoPorzione18 { get; set; }
        public string QualitaPorzione18 { get; set; }
        public string ClassePorzione18 { get; set; }
        public string EttariPorzione18 { get; set; }
        public string ArePorzione18 { get; set; }
        public string CentiarePorzione18 { get; set; }
        public string RedditoDominicaleEuroPorzione18 { get; set; }
        public string RedditoAgrarioEuroPorzione18 { get; set; }
        public string IdentificativoPorzione19 { get; set; }
        public string QualitaPorzione19 { get; set; }
        public string ClassePorzione19 { get; set; }
        public string EttariPorzione19 { get; set; }
        public string ArePorzione19 { get; set; }
        public string CentiarePorzione19 { get; set; }
        public string RedditoDominicaleEuroPorzione19 { get; set; }
        public string RedditoAgrarioEuroPorzione19 { get; set; }
        public string IdentificativoPorzione20 { get; set; }
        public string QualitaPorzione20 { get; set; }
        public string ClassePorzione20 { get; set; }
        public string EttariPorzione20 { get; set; }
        public string ArePorzione20 { get; set; }
        public string CentiarePorzione20 { get; set; }
        public string RedditoDominicaleEuroPorzione20 { get; set; }
        public string RedditoAgrarioEuroPorzione20 { get; set; }
        #endregion
        #endregion
        #region Construtor
        public Terreno()
        {
            Reset();
        }
        public void Reset()
        {
            ID = IDElaborazione = default(int);
            IDCatastale = Sezione = IDImmobile = TipoImmobile = Progressivo = Foglio = Numero = Denominatore = Subalterno = Edificialita = Qualita = Classe = Ettari = Are = Centiare = FlagReddito = FlagPorzione = FlagDeduzioni = RedditoDominicaleLire = RedditoAgrarioLire = RedditoDominicaleEuro = RedditoAgrarioEuro = DataInizioEfficacia = DataInizioRegistrazioneAtti = TipoNotaInizio = NumeroNotaInizio = ProgressivoNotaInizio = AnnoNotaInizio = DataFineEfficacia = DataFineRegistrazioneAtti = TipoNotaFine = NumeroNotaFine = ProgressivoNotaFine = AnnoNotaFine = Partita = Annotazione = IDMutazioneIniziale = IDMutazioneFinale = CodiceCausaleAttoGenerante = DescrizioneAttoGenerante = CodiceCausaleAttoConclusivo = DescrizioneAttoConclusivo = string.Empty;
            SimboloDeduzione1 = SimboloDeduzione2 = SimboloDeduzione3 = SimboloDeduzione4 = SimboloDeduzione5 = SimboloDeduzione6 = SimboloDeduzione7 = string.Empty;
            CodiceRiserva1 = PartitaIscrizioneRiserva1 = CodiceRiserva2 = PartitaIscrizioneRiserva2 = CodiceRiserva3 = PartitaIscrizioneRiserva3 = CodiceRiserva4 = PartitaIscrizioneRiserva4 = CodiceRiserva5 = PartitaIscrizioneRiserva5 = CodiceRiserva6 = PartitaIscrizioneRiserva6 = CodiceRiserva7 = PartitaIscrizioneRiserva7 = CodiceRiserva8 = PartitaIscrizioneRiserva8 = CodiceRiserva9 = PartitaIscrizioneRiserva9 = CodiceRiserva10 = PartitaIscrizioneRiserva10 = CodiceRiserva11 = PartitaIscrizioneRiserva11 = CodiceRiserva12 = PartitaIscrizioneRiserva12 = CodiceRiserva13 = PartitaIscrizioneRiserva13 = CodiceRiserva14 = PartitaIscrizioneRiserva14 = CodiceRiserva15 = PartitaIscrizioneRiserva15 = CodiceRiserva16 = PartitaIscrizioneRiserva16 = CodiceRiserva17 = PartitaIscrizioneRiserva17 = CodiceRiserva18 = PartitaIscrizioneRiserva18 = CodiceRiserva19 = PartitaIscrizioneRiserva19 = CodiceRiserva20 = PartitaIscrizioneRiserva20 = CodiceRiserva21 = PartitaIscrizioneRiserva21 = CodiceRiserva22 = PartitaIscrizioneRiserva22 = CodiceRiserva23 = PartitaIscrizioneRiserva23 = CodiceRiserva24 = PartitaIscrizioneRiserva24 = CodiceRiserva25 = PartitaIscrizioneRiserva25 = CodiceRiserva26 = PartitaIscrizioneRiserva26 = CodiceRiserva27 = PartitaIscrizioneRiserva27 = CodiceRiserva28 = PartitaIscrizioneRiserva28 = CodiceRiserva29 = PartitaIscrizioneRiserva29 = CodiceRiserva30 = PartitaIscrizioneRiserva30 = string.Empty;
            IdentificativoPorzione1 = QualitaPorzione1 = ClassePorzione1 = EttariPorzione1 = ArePorzione1 = CentiarePorzione1 = RedditoDominicaleEuroPorzione1 = RedditoAgrarioEuroPorzione1 = IdentificativoPorzione2 = QualitaPorzione2 = ClassePorzione2 = EttariPorzione2 = ArePorzione2 = CentiarePorzione2 = RedditoDominicaleEuroPorzione2 = RedditoAgrarioEuroPorzione2 = IdentificativoPorzione3 = QualitaPorzione3 = ClassePorzione3 = EttariPorzione3 = ArePorzione3 = CentiarePorzione3 = RedditoDominicaleEuroPorzione3 = RedditoAgrarioEuroPorzione3 = IdentificativoPorzione4 = QualitaPorzione4 = ClassePorzione4 = EttariPorzione4 = ArePorzione4 = CentiarePorzione4 = RedditoDominicaleEuroPorzione4 = RedditoAgrarioEuroPorzione4 = IdentificativoPorzione5 = QualitaPorzione5 = ClassePorzione5 = EttariPorzione5 = ArePorzione5 = CentiarePorzione5 = RedditoDominicaleEuroPorzione5 = RedditoAgrarioEuroPorzione5 = IdentificativoPorzione6 = QualitaPorzione6 = ClassePorzione6 = EttariPorzione6 = ArePorzione6 = CentiarePorzione6 = RedditoDominicaleEuroPorzione6 = RedditoAgrarioEuroPorzione6 = IdentificativoPorzione7 = QualitaPorzione7 = ClassePorzione7 = EttariPorzione7 = ArePorzione7 = CentiarePorzione7 = RedditoDominicaleEuroPorzione7 = RedditoAgrarioEuroPorzione7 = IdentificativoPorzione8 = QualitaPorzione8 = ClassePorzione8 = EttariPorzione8 = ArePorzione8 = CentiarePorzione8 = RedditoDominicaleEuroPorzione8 = RedditoAgrarioEuroPorzione8 = IdentificativoPorzione9 = QualitaPorzione9 = ClassePorzione9 = EttariPorzione9 = ArePorzione9 = CentiarePorzione9 = RedditoDominicaleEuroPorzione9 = RedditoAgrarioEuroPorzione9 = IdentificativoPorzione10 = QualitaPorzione10 = ClassePorzione10 = EttariPorzione10 = ArePorzione10 = CentiarePorzione10 = RedditoDominicaleEuroPorzione10 = RedditoAgrarioEuroPorzione10 = IdentificativoPorzione11 = QualitaPorzione11 = ClassePorzione11 = EttariPorzione11 = ArePorzione11 = CentiarePorzione11 = RedditoDominicaleEuroPorzione11 = RedditoAgrarioEuroPorzione11 = IdentificativoPorzione12 = QualitaPorzione12 = ClassePorzione12 = EttariPorzione12 = ArePorzione12 = CentiarePorzione12 = RedditoDominicaleEuroPorzione12 = RedditoAgrarioEuroPorzione12 = IdentificativoPorzione13 = QualitaPorzione13 = ClassePorzione13 = EttariPorzione13 = ArePorzione13 = CentiarePorzione13 = RedditoDominicaleEuroPorzione13 = RedditoAgrarioEuroPorzione13 = IdentificativoPorzione14 = QualitaPorzione14 = ClassePorzione14 = EttariPorzione14 = ArePorzione14 = CentiarePorzione14 = RedditoDominicaleEuroPorzione14 = RedditoAgrarioEuroPorzione14 = IdentificativoPorzione15 = QualitaPorzione15 = ClassePorzione15 = EttariPorzione15 = ArePorzione15 = CentiarePorzione15 = RedditoDominicaleEuroPorzione15 = RedditoAgrarioEuroPorzione15 = IdentificativoPorzione16 = QualitaPorzione16 = ClassePorzione16 = EttariPorzione16 = ArePorzione16 = CentiarePorzione16 = RedditoDominicaleEuroPorzione16 = RedditoAgrarioEuroPorzione16 = IdentificativoPorzione17 = QualitaPorzione17 = ClassePorzione17 = EttariPorzione17 = ArePorzione17 = CentiarePorzione17 = RedditoDominicaleEuroPorzione17 = RedditoAgrarioEuroPorzione17 = IdentificativoPorzione18 = QualitaPorzione18 = ClassePorzione18 = EttariPorzione18 = ArePorzione18 = CentiarePorzione18 = RedditoDominicaleEuroPorzione18 = RedditoAgrarioEuroPorzione18 = IdentificativoPorzione19 = QualitaPorzione19 = ClassePorzione19 = EttariPorzione19 = ArePorzione19 = CentiarePorzione19 = RedditoDominicaleEuroPorzione19 = RedditoAgrarioEuroPorzione19 = IdentificativoPorzione20 = QualitaPorzione20 = ClassePorzione20 = EttariPorzione20 = ArePorzione20 = CentiarePorzione20 = RedditoDominicaleEuroPorzione20 = RedditoAgrarioEuroPorzione20 = string.Empty;
        }
        #endregion
    }
    public class Titoli
    {
        #region Variables
        public int ID { get; set; }
        public int IDElaborazione { get; set; }
        public string IDCatastale { get; set; }
        public string Sezione { get; set; }
        public string IDSoggetto { get; set; }
        public string TipoSoggetto { get; set; }
        public string IDImmobile { get; set; }
        public string TipoImmobile { get; set; }
        public string CodiceDiritto { get; set; }
        public string TitoloNonCodificato { get; set; }
        public string QuotaNumeratore { get; set; }
        public string QuotaDenominatore { get; set; }
        public string Regime { get; set; }
        public string SoggettoDiRiferimento { get; set; }
        public string DataInizioEfficacia { get; set; }
        public string TipoNotaInizio { get; set; }
        public string NumeroNotaInizio { get; set; }
        public string ProgressivoNotaInizio { get; set; }
        public string AnnoNotaInizio { get; set; }
        public string DataInizioRegistrazioneAtti { get; set; }
        public string Partita { get; set; }
        public string DataFineEfficacia { get; set; }
        public string TipoNotaFine { get; set; }
        public string NumeroNotaFine { get; set; }
        public string ProgressivoNotaFine { get; set; }
        public string AnnoNotaFine { get; set; }
        public string DataFineRegistrazioneAtti { get; set; }
        public string IDMutazioneIniziale { get; set; }
        public string IDMutazioneFinale { get; set; }
        public string IDTitolarita { get; set; }
        public string CodiceCausaleAttoGenerante { get; set; }
        public string DescrizioneAttoGenerante { get; set; }
        public string CodicecausaleAttoConclusivo { get; set; }
        public string DescrizioneAttoConclusivo { get; set; }
        #endregion
        #region Construtor
        public Titoli()
        {
            Reset();
        }
        public void Reset()
        {
            ID =IDElaborazione= default(int);
            IDCatastale = Sezione = IDSoggetto = TipoSoggetto = IDImmobile = TipoImmobile = CodiceDiritto = TitoloNonCodificato = QuotaNumeratore = QuotaDenominatore = Regime = SoggettoDiRiferimento = DataInizioEfficacia = TipoNotaInizio = NumeroNotaInizio = ProgressivoNotaInizio = AnnoNotaInizio = DataInizioRegistrazioneAtti = Partita = DataFineEfficacia = TipoNotaFine = NumeroNotaFine = ProgressivoNotaFine = AnnoNotaFine = DataFineRegistrazioneAtti = IDMutazioneIniziale = IDMutazioneFinale = IDTitolarita = CodiceCausaleAttoGenerante = DescrizioneAttoGenerante = CodicecausaleAttoConclusivo = DescrizioneAttoConclusivo = string.Empty;
        }
        #endregion
    }
    public class Soggetto
    {
        #region Variables
        public int ID { get; set; }
        public int IDElaborazione { get; set; }
        public string IDCatastale { get; set; }
        public string Sezione { get; set; }
        public string IDSoggetto { get; set; }
        public string TipoSoggetto { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Sesso { get; set; }
        public string DataNascita { get; set; }
        public string LuogoNascita { get; set; }
        public string CodFiscalePIVA { get; set; }
        public string Denominazione { get; set; }
        public string Sede { get; set; }
        public string Note { get; set; }
        #endregion
        #region Construtor
        public Soggetto()
        {
            Reset();
        }
        public void Reset()
        {
            ID=IDElaborazione = default(int);
            IDCatastale = Sezione = IDSoggetto = TipoSoggetto = Cognome = Nome = Sesso = DataNascita = LuogoNascita = CodFiscalePIVA = Denominazione = Sede =Note= string.Empty;
        }
        #endregion
    }
    public class Dichiarazione
    {
        #region Variables
        public int ID { get; set; }
        public int IDElaborazione { get; set; }
        public string IDCatastale { get; set; }
        public  string IDSoggetto { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string CodFiscalePIVA { get; set; }
        public string NumeroDichiarazione { get; set; }
        public string DataDichiarazione { get; set; }
        public string IDImmobile { get; set; }
        public string IDImmobileCat { get; set; }
        public string IDStrada { get; set; }
        public string Indirizzo { get; set; }
        public string Civico { get; set; }
        public string Scala { get; set; }
        public string Piano { get; set; }
        public string Interno { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Subalterno { get; set; }
        public string DataInizio { get; set; }
        public string DataFine { get; set; }
        public string QuotaPossesso { get; set; }
        public string MesiPossesso { get; set; }
        public string TipoPossesso { get; set; }
        public string RegimePossesso { get; set; }
        public string TipoUtilizzo { get; set; }
        public string TipoRendita { get; set; }
        public string Zona { get; set; }
        public string Categoria { get; set; }
        public string Classe { get; set; }
        public string Valore { get; set; }
        public string Rendita { get; set; }
        public string Consistenza { get; set; }
        public string FlagPrincipale { get; set; }
        public string FlagPertinenza { get; set; }
        public string FlagEsente { get; set; }
        public string FlagRiduzione { get; set; }
        public string FlagColDir { get; set; }
        public string NUtilizzatori { get; set; }
        public string NFigliMinori26Anni { get; set; }
        public string Note { get; set; }
        public string Azione { get; set; }
        #endregion
        #region Construtor
        public Dichiarazione()
        {
            Reset();
        }
        public void Reset()
        {
            ID =IDElaborazione= default(int);
            IDCatastale = Cognome = Nome = CodFiscalePIVA = NumeroDichiarazione = DataDichiarazione = IDImmobile = IDStrada = Indirizzo = Civico = Scala = Piano = Interno = Foglio = Numero = Subalterno = DataInizio = DataFine = QuotaPossesso = MesiPossesso = TipoPossesso = TipoUtilizzo = TipoRendita = Zona = Categoria = Classe = Valore = Rendita = Consistenza = FlagPrincipale = FlagPertinenza = FlagEsente = FlagRiduzione = FlagColDir = NUtilizzatori = NFigliMinori26Anni = Note = IDSoggetto = IDImmobileCat = RegimePossesso = Azione = string.Empty;
        }
        #endregion
    }
}
