using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motore_Catasto
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
        public DateTime InizioEstrazioneFabVSTit { get; set; }
        public DateTime FineEstrazioneFabVSTit { get; set; }
        public string EsitoEstrazioneFabVSTit { get; set; }
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
            IDEnte = IDCatastale = default(string);
            EsitoUpload = EsitoImport = EsitoConvert = EsitoIncrocio = EsitoEstrazioneDichWork = EsitoEstrazioneTitVSSog = EsitoEstrazioneSogVSTit = EsitoEstrazioneTitVSFab = EsitoEstrazioneFabVSTit = EsitoEstrazionePossMancante = EsitoEstrazioneComunioneMancante = default(string);
            InizioUpload = FineUpload = InizioImport = FineImport = InizioConvert = FineConvert = InizioIncrocio = FineIncrocio = InizioEstrazioneDichWork = FineEstrazioneDichWork = InizioEstrazioneTitVSSog = FineEstrazioneTitVSSog = InizioEstrazioneSogVSTit = FineEstrazioneSogVSTit = InizioEstrazioneTitVSFab = FineEstrazioneTitVSFab = InizioEstrazioneFabVSTit = FineEstrazioneFabVSTit = InizioEstrazionePossMancante = FineEstrazionePossMancante = InizioEstrazioneComunioneMancante = FineEstrazioneComunioneMancante = DateTime.MaxValue;
            ListFiles = new List<ElaborazioneFile>();
        }
        #endregion
    }
    public class ElaborazioneFile
    {
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
            ID =IDElaborazione= default(int);
            NameFile=  EsitoImport = default(string);
            InizioImport = FineImport = DateTime.MaxValue;
        }
        #endregion
    }
    public class Fabbricato
    {
        #region Variables
        public int ID { get; set; }
        public string IDEnte { get; set; }
        public string Sezione { get; set; }
        public string IDImmobile { get; set; }
        public string Tipoimmobile { get; set; }
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
        public string DataInizioefficacia { get; set; }
        public string DataInizioregistrazioneinatti { get; set; }
        public string TiponotaInizio { get; set; }
        public string NumeronotaInizio { get; set; }
        public string ProgressivonotaInizio { get; set; }
        public string AnnonotaInizio { get; set; }
        public string DataFineefficacia { get; set; }
        public string DataFineregistrazioneatti { get; set; }
        public string TiponotaFine { get; set; }
        public string NumeronotaFine { get; set; }
        public string ProgressivonotaFine { get; set; }
        public string AnnonotaFine { get; set; }
        public string Partita { get; set; }
        public string Annotazione { get; set; }
        public string Identificativomutazioneiniziale { get; set; }
        public string Identificativomutazionefinale { get; set; }
        public string Protocollonotifica { get; set; }
        public string Datanotifica { get; set; }
        public string Codicecausaleattogenerante { get; set; }
        public string Descrizioneattogenerante { get; set; }
        public string Codicecausaleattoconclusivo { get; set; }
        public string Descrizioneattoconclusivo { get; set; }
        public string Flagclassamento { get; set; }
        #region "tabella identificativi(max10elementi)"
        public string Sezioneurbana1 { get; set; }
        public string Foglio1 { get; set; }
        public string Numero1 { get; set; }
        public string Denominatore1 { get; set; }
        public string Subalterno1 { get; set; }
        public string Edificialita1 { get; set; }
        public string Sezioneurbana2 { get; set; }
        public string Foglio2 { get; set; }
        public string Numero2 { get; set; }
        public string Denominatore2 { get; set; }
        public string Subalterno2 { get; set; }
        public string Edificialita2 { get; set; }
        public string Sezioneurbana3 { get; set; }
        public string Foglio3 { get; set; }
        public string Numero3 { get; set; }
        public string Denominatore3 { get; set; }
        public string Subalterno3 { get; set; }
        public string Edificialita3 { get; set; }
        public string Sezioneurbana4 { get; set; }
        public string Foglio4 { get; set; }
        public string Numero4 { get; set; }
        public string Denominatore4 { get; set; }
        public string Subalterno4 { get; set; }
        public string Edificialita4 { get; set; }
        public string Sezioneurbana5 { get; set; }
        public string Foglio5 { get; set; }
        public string Numero5 { get; set; }
        public string Denominatore5 { get; set; }
        public string Subalterno5 { get; set; }
        public string Edificialita5 { get; set; }
        public string Sezioneurbana6 { get; set; }
        public string Foglio6 { get; set; }
        public string Numero6 { get; set; }
        public string Denominatore6 { get; set; }
        public string Subalterno6 { get; set; }
        public string Edificialita6 { get; set; }
        public string Sezioneurbana7 { get; set; }
        public string Foglio7 { get; set; }
        public string Numero7 { get; set; }
        public string Denominatore7 { get; set; }
        public string Subalterno7 { get; set; }
        public string Edificialita7 { get; set; }
        public string Sezioneurbana8 { get; set; }
        public string Foglio8 { get; set; }
        public string Numero8 { get; set; }
        public string Denominatore8 { get; set; }
        public string Subalterno8 { get; set; }
        public string Edificialita8 { get; set; }
        public string Sezioneurbana9 { get; set; }
        public string Foglio9 { get; set; }
        public string Numero9 { get; set; }
        public string Denominatore9 { get; set; }
        public string Subalterno9 { get; set; }
        public string Edificialita9 { get; set; }
        public string Sezioneurbana10 { get; set; }
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
        public string Codicestrada1 { get; set; }
        public string Toponimo2 { get; set; }
        public string Indirizzo2 { get; set; }
        public string Civico12 { get; set; }
        public string Civico22 { get; set; }
        public string Civico32 { get; set; }
        public string Codicestrada2 { get; set; }
        public string Toponimo3 { get; set; }
        public string Indirizzo3 { get; set; }
        public string Civico13 { get; set; }
        public string Civico23 { get; set; }
        public string Civico33 { get; set; }
        public string Codicestrada3 { get; set; }
        public string Toponimo4 { get; set; }
        public string Indirizzo4 { get; set; }
        public string Civico14 { get; set; }
        public string Civico24 { get; set; }
        public string Civico34 { get; set; }
        public string Codicestrada4 { get; set; }
        #endregion
        #region "tabella utilita' comuni(10elementi)"
        public string UC_Sezioneurbana1 { get; set; }
        public string UC_Foglio1 { get; set; }
        public string UC_Numero1 { get; set; }
        public string UC_Denominatore1 { get; set; }
        public string UC_Subalterno1 { get; set; }
        public string UC_Sezioneurbana2 { get; set; }
        public string UC_Foglio2 { get; set; }
        public string UC_Numero2 { get; set; }
        public string UC_Denominatore2 { get; set; }
        public string UC_Subalterno2 { get; set; }
        public string UC_Sezioneurbana3 { get; set; }
        public string UC_Foglio3 { get; set; }
        public string UC_Numero3 { get; set; }
        public string UC_Denominatore3 { get; set; }
        public string UC_Subalterno3 { get; set; }
        public string UC_Sezioneurbana4 { get; set; }
        public string UC_Foglio4 { get; set; }
        public string UC_Numero4 { get; set; }
        public string UC_Denominatore4 { get; set; }
        public string UC_Subalterno4 { get; set; }
        public string UC_Sezioneurbana5 { get; set; }
        public string UC_Foglio5 { get; set; }
        public string UC_Numero5 { get; set; }
        public string UC_Denominatore5 { get; set; }
        public string UC_Subalterno5 { get; set; }
        public string UC_Sezioneurbana6 { get; set; }
        public string UC_Foglio6 { get; set; }
        public string UC_Numero6 { get; set; }
        public string UC_Denominatore6 { get; set; }
        public string UC_Subalterno6 { get; set; }
        public string UC_Sezioneurbana7 { get; set; }
        public string UC_Foglio7 { get; set; }
        public string UC_Numero7 { get; set; }
        public string UC_Denominatore7 { get; set; }
        public string UC_Subalterno7 { get; set; }
        public string UC_Sezioneurbana8 { get; set; }
        public string UC_Foglio8 { get; set; }
        public string UC_Numero8 { get; set; }
        public string UC_Denominatore8 { get; set; }
        public string UC_Subalterno8 { get; set; }
        public string UC_Sezioneurbana9 { get; set; }
        public string UC_Foglio9 { get; set; }
        public string UC_Numero9 { get; set; }
        public string UC_Denominatore9 { get; set; }
        public string UC_Subalterno9 { get; set; }
        public string UC_Sezioneurbana10 { get; set; }
        public string UC_Foglio10 { get; set; }
        public string UC_Numero10 { get; set; }
        public string UC_Denominatore10 { get; set; }
        public string UC_Subalterno10 { get; set; }
        #endregion
        #region "tabella riserve(10elementi)"
        public string Codiceriserva1 { get; set; }
        public string Partitaiscrizioneriserva1 { get; set; }
        public string Codiceriserva2 { get; set; }
        public string Partitaiscrizioneriserva2 { get; set; }
        public string Codiceriserva3 { get; set; }
        public string Partitaiscrizioneriserva3 { get; set; }
        public string Codiceriserva4 { get; set; }
        public string Partitaiscrizioneriserva4 { get; set; }
        public string Codiceriserva5 { get; set; }
        public string Partitaiscrizioneriserva5 { get; set; }
        public string Codiceriserva6 { get; set; }
        public string Partitaiscrizioneriserva6 { get; set; }
        public string Codiceriserva7 { get; set; }
        public string Partitaiscrizioneriserva7 { get; set; }
        public string Codiceriserva8 { get; set; }
        public string Partitaiscrizioneriserva8 { get; set; }
        public string Codiceriserva9 { get; set; }
        public string Partitaiscrizioneriserva9 { get; set; }
        public string Codiceriserva10 { get; set; }
        public string Partitaiscrizioneriserva10 { get; set; }
        #endregion
        #endregion
        #region Construtor
        public Fabbricato()
        {
            Reset();
        }
        public void Reset()
        {
            ID = default(int);
            IDEnte = Sezione = IDImmobile = Tipoimmobile = Progressivo = Zona = Categoria = Classe = Consistenza = Superficie = RenditaLire = RenditaEuro = Lotto = Edificio = Scala = Interno1 = Interno2 = Piano1 = Piano2 = Piano3 = Piano4 = DataInizioefficacia = DataInizioregistrazioneinatti = TiponotaInizio = NumeronotaInizio = ProgressivonotaInizio = AnnonotaInizio =DataFineefficacia= DataFineregistrazioneatti = TiponotaFine = NumeronotaFine = ProgressivonotaFine = AnnonotaFine = Partita = Annotazione = Identificativomutazioneiniziale = Identificativomutazionefinale = Protocollonotifica = Datanotifica = Codicecausaleattogenerante = Descrizioneattogenerante = Codicecausaleattoconclusivo = Descrizioneattoconclusivo = Flagclassamento = default(string);
            Sezioneurbana1 = Foglio1 = Numero1 = Denominatore1 = Subalterno1 = Edificialita1 = Sezioneurbana2 = Foglio2 = Numero2 = Denominatore2 = Subalterno2 = Edificialita2 = Sezioneurbana3 = Foglio3 = Numero3 = Denominatore3 = Subalterno3 = Edificialita3 = Sezioneurbana4 = Foglio4 = Numero4 = Denominatore4 = Subalterno4 = Edificialita4 = Sezioneurbana5 = Foglio5 = Numero5 = Denominatore5 = Subalterno5 = Edificialita5 = Sezioneurbana6 = Foglio6 = Numero6 = Denominatore6 = Subalterno6 = Edificialita6 = Sezioneurbana7 = Foglio7 = Numero7 = Denominatore7 = Subalterno7 = Edificialita7 = Sezioneurbana8 = Foglio8 = Numero8 = Denominatore8 = Subalterno8 = Edificialita8 = Sezioneurbana9 = Foglio9 = Numero9 = Denominatore9 = Subalterno9 = Edificialita9 = Sezioneurbana10 = Foglio10 = Numero10 = Denominatore10 = Subalterno10 = Edificialita10 = default(string);
            Toponimo1 = Indirizzo1 = Civico11 = Civico21 = Civico31 = Codicestrada1 = Toponimo2 = Indirizzo2 = Civico12 = Civico22 = Civico32 = Codicestrada2 = Toponimo3 = Indirizzo3 = Civico13 = Civico23 = Civico33 = Codicestrada3 = Toponimo4 = Indirizzo4 = Civico14 = Civico24 = Civico34 = Codicestrada4 = default(string);
            UC_Sezioneurbana1 = UC_Foglio1 = UC_Numero1 = UC_Denominatore1 = UC_Subalterno1 = UC_Sezioneurbana2 = UC_Foglio2 = UC_Numero2 = UC_Denominatore2 = UC_Subalterno2 = UC_Sezioneurbana3 = UC_Foglio3 = UC_Numero3 = UC_Denominatore3 = UC_Subalterno3 = UC_Sezioneurbana4 = UC_Foglio4 = UC_Numero4 = UC_Denominatore4 = UC_Subalterno4 = UC_Sezioneurbana5 = UC_Foglio5 = UC_Numero5 = UC_Denominatore5 = UC_Subalterno5 = UC_Sezioneurbana6 = UC_Foglio6 = UC_Numero6 = UC_Denominatore6 = UC_Subalterno6 = UC_Sezioneurbana7 = UC_Foglio7 = UC_Numero7 = UC_Denominatore7 = UC_Subalterno7 = UC_Sezioneurbana8 = UC_Foglio8 = UC_Numero8 = UC_Denominatore8 = UC_Subalterno8 = UC_Sezioneurbana9 = UC_Foglio9 = UC_Numero9 = UC_Denominatore9 = UC_Subalterno9 = UC_Sezioneurbana10 = UC_Foglio10 = UC_Numero10 = UC_Denominatore10 = UC_Subalterno10 = default(string);
            Codiceriserva1 = Partitaiscrizioneriserva1 = Codiceriserva2 = Partitaiscrizioneriserva2 = Codiceriserva3 = Partitaiscrizioneriserva3 = Codiceriserva4 = Partitaiscrizioneriserva4 = Codiceriserva5 = Partitaiscrizioneriserva5 = Codiceriserva6 = Partitaiscrizioneriserva6 = Codiceriserva7 = Partitaiscrizioneriserva7 = Codiceriserva8 = Partitaiscrizioneriserva8 = Codiceriserva9 = Partitaiscrizioneriserva9 = Codiceriserva10 = Partitaiscrizioneriserva10 = default(string);
        }
        #endregion
    }
    public class Titoli
    {
        #region Variables
        public int ID { get; set; }
        public string IDEnte { get; set; }
        public string Sezione { get; set; }
        public string Identificativosoggetto { get; set; }
        public string Tiposoggetto { get; set; }
        public string Identificativoimmobile { get; set; }
        public string Tipoimmobile { get; set; }
        public string Codicediritto { get; set; }
        public string Titolononcodificato { get; set; }
        public string Quotanumeratore { get; set; }
        public string Quotadenominatore { get; set; }
        public string Regime { get; set; }
        public string Soggettodiriferimento { get; set; }
        public string DataInizioefficacia { get; set; }
        public string TiponotaInizio { get; set; }
        public string NumeronotaInizio { get; set; }
        public string ProgressivonotaInizio { get; set; }
        public string AnnonotaInizio { get; set; }
        public string DataInizioregistrazioneinatti { get; set; }
        public string Partita { get; set; }
        public string DataFineEfficacia { get; set; }
        public string TiponotaFine { get; set; }
        public string NumeronotaFine { get; set; }
        public string ProgressivonotaFine { get; set; }
        public string AnnonotaFine { get; set; }
        public string DataFineregistrazioneinatti { get; set; }
        public string Identificativomutazioneiniziale { get; set; }
        public string Identificativomutazionefinale { get; set; }
        public string Identificativotitolarita { get; set; }
        public string Codicecausaleattogenerante { get; set; }
        public string Descrizioneattogenerante { get; set; }
        public string Codicecausaleattoconclusivo { get; set; }
        public string Descrizioneattoconclusivo { get; set; }
        #endregion
        #region Construtor
        public Titoli()
        {
            Reset();
        }
        public void Reset()
        {
            ID = default(int);
            IDEnte = Sezione = Identificativosoggetto = Tiposoggetto = Identificativoimmobile = Tipoimmobile = Codicediritto = Titolononcodificato = Quotanumeratore = Quotadenominatore = Regime = Soggettodiriferimento = DataInizioefficacia = TiponotaInizio = NumeronotaInizio = ProgressivonotaInizio = AnnonotaInizio = DataInizioregistrazioneinatti = Partita = DataFineEfficacia = TiponotaFine = NumeronotaFine = ProgressivonotaFine = AnnonotaFine = DataFineregistrazioneinatti = Identificativomutazioneiniziale = Identificativomutazionefinale = Identificativotitolarita = Codicecausaleattogenerante = Descrizioneattogenerante = Codicecausaleattoconclusivo = Descrizioneattoconclusivo = default(string);
        }
        #endregion
    }
    public class Soggetto
    {
        #region Variables
        public int ID { get; set; }
        public string IDEnte { get; set; }
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
        #endregion
        #region Construtor
        public Soggetto()
        {
            Reset();
        }
        public void Reset()
        {
            ID = default(int);
            IDEnte = Sezione = IDSoggetto =TipoSoggetto=Cognome =Nome = Sesso = DataNascita=LuogoNascita = CodFiscalePIVA = Denominazione = Sede = default(string);
        }
        #endregion
    }
    public class Dichiarazione
    {
        #region Variables
        public int ID { get; set; }
        public string IDEnte { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string CodFiscalePIVA { get; set; }
        public string NDichiarazione { get; set; }
        public string DataDichiarazione { get; set; }
        public string IDImmobile { get; set; }
        public string IDStrada { get; set; }
        public string Indirizzo { get; set; }
        public string Civico { get; set; }
        public string Scala { get; set; }
        public string Piano { get; set; }
        public string Interno { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Subalterno { get; set; }
        public string Inizio { get; set; }
        public string Fine { get; set; }
        public string QuotaPossesso { get; set; }
        public string MesiPossesso { get; set; }
        public string TipoPossesso { get; set; }
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
        public string NFigli { get; set; }
        public string Note { get; set; }
        #endregion
        #region Construtor
        public Dichiarazione()
        {
            Reset();
        }
        public void Reset()
        {
            ID = default(int);
            IDEnte = Cognome = Nome = CodFiscalePIVA = NDichiarazione = DataDichiarazione = IDImmobile = IDStrada = Indirizzo = Civico = Scala = Piano = Interno = Foglio = Numero = Subalterno = Inizio = Fine = QuotaPossesso = MesiPossesso = TipoPossesso = TipoUtilizzo = TipoRendita = Zona = Categoria = Classe = Valore = Rendita = Consistenza = FlagPrincipale = FlagPertinenza = FlagEsente = FlagRiduzione = FlagColDir = NUtilizzatori = NFigli = Note= default(string);
        }
        #endregion
    }
}
