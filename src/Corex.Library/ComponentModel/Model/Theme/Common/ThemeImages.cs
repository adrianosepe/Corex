#region License

// Copyright(c) 2023 GrappTec
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

namespace Corex.Library.ComponentModel.Model.Theme.Common;

public static class ThemeImages
{
    public static class Modules
    {
        public const string None = null;
        public const string GeoMunicipio = "Modules/GeoMunicipio";
        public const string GeoEstado = "Modules/GeoEstado";
        public const string GeoPais = "Modules/GeoPais";
        public const string Empresa = "Modules/Empresa";
        public const string Administracao = "Modules/Administracao";
        public const string AuditoriaOperacao = "Modules/AuditoriaOperacao";
        public const string Banco = "Modules/Banco";
        public const string CampanhaPromocional = "Modules/CampanhaPromocional";
        public const string Fabricante = "Modules/Fabricante";
        public const string FaturaDevolucao = "Modules/FaturaDevolucao";
        public const string FaturaInterna = "Modules/FaturaInterna";
        public const string FaturaPedido = "Modules/FaturaPedido";
        public const string FaturaPessoa = "Modules/FaturaPessoa";
        public const string FaturaPoS = "Modules/FaturaPoS";
        public const string FinanceiroMovi = "Modules/FinanceiroMovi";
        public const string LiberacaoCredito = "Modules/LiberacaoCredito";
        public const string Mercadoria = "Modules/Mercadoria";
        public const string MercadoriaEmbalagem = "Modules/MercadoriaEmbalagem";
        public const string MercadoriaGrupo = "Modules/MercadoriaGrupo";
        public const string MercadoriaGrupoPreparo = "Modules/MercadoriaGrupoPreparo";
        public const string MercadoriaPerfilComercializacao = "Modules/MercadoriaPerfilComercializacao";
        public const string MercadoriaPreco = "Modules/MercadoriaPreco";
        public const string MercadoriaPromocao = "Modules/MercadoriaPromocao";
        public const string MercadoriaTabelaPreco = "Modules/MercadoriaTabelaPreco";
        public const string NumeracaoDocumento = "Modules/NumeracaoDocumento";
        public const string PdvBalanca = "Modules/PdvBalanca";
        public const string PdvBalancaModelo = "Modules/PdvBalancaModelo";
        public const string PdvImpressora = "Modules/PdvImpressora";
        public const string Pedido = "Modules/Pedido";
        public const string PedidoCancelamento = "Modules/PedidoCancelamento";
        public const string PedidoMotivoCancelamento = "Modules/PedidoMotivoCancelamento";
        public const string Pessoa = "Modules/Pessoa";
        public const string PessoaClasse = "Modules/PessoaClasse";
        public const string PlanoContas = "Modules/PlanoContas";
        public const string TerminalRecebimento = "Modules/TerminalRecebimento";
        public const string TerminalSangria = "Modules/TerminalSangria";
        public const string TerminalSuprimento = "Modules/TerminalSuprimento";
        public const string TerminalVenda = "Modules/TerminalVenda";
        public const string TipoContato = "Modules/TipoContato";
        public const string TipoEndereco = "Modules/TipoEndereco";
        public const string TipoTelefone = "Modules/TipoTelefone";
        public const string UnidadeMedida = "Modules/UnidadeMedida";
        public const string NotaFiscal = "Modules/NotaFiscal";
        public const string DeliveryTaxa = "Modules/DeliveryTaxa";
        public const string Filial = "Modules/Filial";
        public const string Departamento = "Modules/Departamento";
        public const string Terminal = "Modules/Terminal";
        public const string FinanceiroSessao = "Modules/FinanceiroSessao";
        public const string AjustarEstoque = "Modules/AjustarEstoque";
        public const string ExForm = "Modules/ExForm";
        public const string AnaliseEstoqueReport = "Modules/AnaliseEstoqueReport";
        public const string AnaliseVendasReport = "Modules/AnaliseVendasReport";
        public const string ListaMercadoriasReport = "Modules/ListaMercadoriasReport";
        public const string NfTransmitidasReport = "Modules/NFTransmitidasReport";
        public const string AniversariantesReport = "Modules/AniversariantesReport";
        public const string AnaliseFinanceiroSessaoReport = "Modules/AnaliseFinanceiroSessaoReport";
        public const string ImpostoPy = "Modules/ImpostoPy";
        public const string Moeda = "Modules/Moeda";
        public const string ExtrairOperacoesFaturaPy = "Modules/ExtrairOperacoesFaturaPy";

        public const string MvFinancPosicaoContas = "Modules/MvFinancPosicoesConsolidadas";
        public const string MvFinancExtratoSimples = "Modules/MvFinancExtratoSimples";
        public const string MvFinancExtratoCaixaBanco = "Modules/MvFinancExtratoCaixaBanco";
        public const string MvFinancContasPagarReceber = "Modules/MvFinancContasPagarReceber";

        public const string MercadoriaFichaReport = "Modules/MercadoriaFichaReport";
    }

    public static class Financeiro
    {
        public static class FormaPagamento
        {
            public const string Dinheiro = "FormaPagamento/Dinheiro";
            public const string Cheque = "FormaPagamento/Cheque";
            public const string CartaoCredito = "FormaPagamento/CartaoCredito";
            public const string CartaoDebito = "FormaPagamento/CartaoDebito";
            public const string CreditoLoja = "FormaPagamento/CreditoLoja";
            public const string RecebCreditoLoja = "FormaPagamento/RecebCreditoLoja";
            public const string Troco = "FormaPagamento/Troco";
        }
    }

    public static class Comandas
    {
        public static class Status
        {
            public const string ComandaDisponivel = "Comanda/Status/ComandaDisponivel";
            public const string ComandaIndisponivel = "Comanda/Status/ComandaIndisponivel";
        }

        public static class Modules
        {
            public const string Caixa = "Comanda/Modules/Caixa";
            public const string MapaComandas = "Comanda/Modules/MapaComandas";
            public const string PedidosDelivery = "Comanda/Modules/PedidosDelivery";
            public const string SugerirMelhorias = "Comanda/Modules/SugerirMelhorias";
            public const string ObterSuporte = "Comanda/Modules/ObterSuporte";
        }

        public static class Actions
        {
            public const string Excluir = "Comanda/Actions/Excluir";
            public const string Finalizar = "Comanda/Actions/Finalizar";
            public const string FinalizarImprimir = "Comanda/Actions/FinalizarImprimir";
            public const string Imprimir = "Comanda/Actions/Imprimir";
            public const string ImprimirMarcarEmFechamento = "Comanda/Actions/ImprimirMarcarEmFechamento";
            public const string AdminFinanceiro = "Comanda/Actions/AdminFinanceiro";
            public const string TaxaDesconto = "Comanda/Actions/TaxaDesconto";
            public const string TrocarMesa = "Comanda/Actions/TrocarMesa";
            public const string VisualizarDetalhesConsumo = "Comanda/Actions/VisualizarDetalhesConsumo";

            public const string Fechar = "Comanda/Actions/FastClose";
        }
    }

    public static class TerminalStore
    {
        public const string NovoPedido = "TerminalStore/NovoPedido";
        public const string NovoPedidoBalcao = "TerminalStore/NovoPedidoBalcao";
    }

    public static class Actions
    {
        public const string AddFile = "Dev/Actions/AddFile";
        public const string AddItem = "Dev/Actions/AddItem";
        public const string Add = "Dev/Actions/Add";
        public const string Apply = "Dev/Actions/Apply";
        public const string Cancel = "Dev/Actions/Cancel";
        public const string ClearFormatting = "Dev/Actions/ClearFormatting";
        public const string Clear = "Dev/Actions/Clear";
        public const string Clip = "Dev/Actions/Clip";
        public const string Close = "Dev/Actions/Close";
        public const string Convert = "Dev/Actions/Convert";
        public const string DeleteList2 = "Dev/Actions/DeleteList2";
        public const string DeleteList = "Dev/Actions/DeleteList";
        public const string Download = "Dev/Actions/Download";
        public const string Group = "Dev/Actions/Group";
        public const string Hide = "Dev/Actions/Hide";
        public const string ImportImage = "Dev/Actions/ImportImage";
        public const string Insert = "Dev/Actions/Insert";
        public const string LoadFrom = "Dev/Actions/LoadFrom";
        public const string Merge = "Dev/Actions/Merge";
        public const string New = "Dev/Actions/New";
        public const string Open = "Dev/Actions/Open";
        public const string Refresh2 = "Dev/Actions/Refresh2";
        public const string Refresh = "Dev/Actions/Refresh";
        public const string RemoveItem = "Dev/Actions/RemoveItem";
        public const string Reset = "Dev/Actions/Reset";
        public const string SelectAll = "Dev/Actions/SelectAll";
        public const string Show = "Dev/Actions/Show";
        public const string Squeeze = "Dev/Actions/Squeeze";
        public const string Stretch = "Dev/Actions/Stretch";
    }

    public static class Alignment
    {
        public const string AlignHorizontalBottom2 = "Dev/Alignment/AlignHorizontalBottom2";
        public const string AlignHorizontalBottom = "Dev/Alignment/AlignHorizontalBottom";
        public const string AlignHorizontalCenter2 = "Dev/Alignment/AlignHorizontalCenter2";
        public const string AlignHorizontalCenter = "Dev/Alignment/AlignHorizontalCenter";
        public const string AlignHorizontalTop2 = "Dev/Alignment/AlignHorizontalTop2";
        public const string AlignHorizontalTop = "Dev/Alignment/AlignHorizontalTop";
        public const string AlignVerticalCenter2 = "Dev/Alignment/AlignVerticalCenter2";
        public const string AlignVerticalCenter = "Dev/Alignment/AlignVerticalCenter";
        public const string AlignVerticalLeft2 = "Dev/Alignment/AlignVerticalLeft2";
        public const string AlignVerticalLeft = "Dev/Alignment/AlignVerticalLeft";
        public const string AlignVerticalRight2 = "Dev/Alignment/AlignVerticalRight2";
        public const string AlignVerticalRight = "Dev/Alignment/AlignVerticalRight";
        public const string ContentArrangeInColums = "Dev/Alignment/ContentArrangeInColums";
        public const string ContentArrangeInRows = "Dev/Alignment/ContentArrangeInRows";
        public const string ContentAutoArrange = "Dev/Alignment/ContentAutoArrange";
    }

    public static class Arrange
    {
        public const string BehindText = "Dev/Arrange/BehindText";
        public const string BringForward = "Dev/Arrange/BringForward";
        public const string BringToFrontOfText = "Dev/Arrange/BringToFrontOfText";
        public const string BringToFront = "Dev/Arrange/BringToFront";
        public const string EditWrapPoints = "Dev/Arrange/EditWrapPoints";
        public const string InFrontOfText = "Dev/Arrange/InFrontOfText";
        public const string InLineWithText = "Dev/Arrange/InLineWithText";
        public const string MoreLayoutOptions = "Dev/Arrange/MoreLayoutOptions";
        public const string SendBackward = "Dev/Arrange/SendBackward";
        public const string SendBehindText = "Dev/Arrange/SendBehindText";
        public const string SendToBack = "Dev/Arrange/SendToBack";
        public const string Square = "Dev/Arrange/Square";
        public const string Through = "Dev/Arrange/Through";
        public const string Tight = "Dev/Arrange/Tight";
        public const string TopAndBottom = "Dev/Arrange/TopAndBottom";
        public const string WithTextWrappingBottomCenter = "Dev/Arrange/WithTextWrapping_BottomCenter";
        public const string WithTextWrappingBottomLeft = "Dev/Arrange/WithTextWrapping_BottomLeft";
        public const string WithTextWrappingBottomRight = "Dev/Arrange/WithTextWrapping_BottomRight";
        public const string WithTextWrappingCenterCenter = "Dev/Arrange/WithTextWrapping_CenterCenter";
        public const string WithTextWrappingCenterLeft = "Dev/Arrange/WithTextWrapping_CenterLeft";
        public const string WithTextWrappingCenterRight = "Dev/Arrange/WithTextWrapping_CenterRight";
        public const string WithTextWrappingTopCenter = "Dev/Arrange/WithTextWrapping_TopCenter";
        public const string WithTextWrappingTopLeft = "Dev/Arrange/WithTextWrapping_TopLeft";
        public const string WithTextWrappingTopRight = "Dev/Arrange/WithTextWrapping_TopRight";
        public const string WrapText = "Dev/Arrange/WrapText";
    }

    public static class Arrows
    {
        public const string DoubleFirst = "Dev/Arrows/DoubleFirst";
        public const string DoubleLast = "Dev/Arrows/DoubleLast";
        public const string DoubleNext = "Dev/Arrows/DoubleNext";
        public const string DoublePrev = "Dev/Arrows/DoublePrev";
        public const string First = "Dev/Arrows/First";
        public const string Last = "Dev/Arrows/Last";
        public const string MoveDown = "Dev/Arrows/MoveDown";
        public const string MoveUp = "Dev/Arrows/MoveUp";
        public const string Next = "Dev/Arrows/Next";
        public const string Prev = "Dev/Arrows/Prev";
    }

    public static class Charts
    {
        public const string Area = "Dev/Chart/Area";
        public const string Bar = "Dev/Chart/Bar";
        public const string Bubble = "Dev/Chart/Bubble";
        public const string ChartsRotate = "Dev/Chart/ChartsRotate";
        public const string ChartsShowLegend = "Dev/Chart/ChartsShowLegend";
        public const string ChartYAxisSettings = "Dev/Chart/ChartYAxisSettings";
        public const string Chart = "Dev/Chart/Chart";
        public const string DrillDownOnArgumentsChart = "Dev/Chart/DrillDownOnArguments_Chart";
        public const string DrillDownOnArgumentsPie = "Dev/Chart/DrillDownOnArguments_Pie";
        public const string DrillDownOnSeriesChart = "Dev/Chart/DrillDownOnSeries_Chart";
        public const string DrillDownOnSeriesPie = "Dev/Chart/DrillDownOnSeries_Pie";
        public const string DrillDown = "Dev/Chart/DrillDown";
        public const string FullStackedArea = "Dev/Chart/FullStackedArea";
        public const string FullStackedBar = "Dev/Chart/FullStackedBar";
        public const string FullStackedLine = "Dev/Chart/FullStackedLine";
        public const string FullStackedSplineArea = "Dev/Chart/FullStackedSplineArea";
        public const string HighLowClose = "Dev/Chart/HighLowClose";
        public const string Kpi = "Dev/Chart/KPI";
        public const string Line = "Dev/Chart/Line";
        public const string OpenHighLowCloseCandleStick = "Dev/Chart/OpenHighLowCloseCandleStick";
        public const string OpenHighLowCloseStock = "Dev/Chart/OpenHighLowCloseStock";
        public const string PieLabelsDataLabels = "Dev/Chart/PieLabelsDataLabels";
        public const string PieLabelsTooltips = "Dev/Chart/PieLabelsTooltips";
        public const string PieStyleDonut = "Dev/Chart/PieStyleDonut";
        public const string PieStylePie = "Dev/Chart/PieStylePie";
        public const string Pie = "Dev/Chart/Pie";
        public const string Point = "Dev/Chart/Point";
        public const string PreviewChart = "Dev/Chart/PreviewChart";
        public const string RangeArea = "Dev/Chart/RangeArea";
        public const string RangeBar = "Dev/Chart/RangeBar";
        public const string SideBySideRangeBar = "Dev/Chart/SideBySideRangeBar";
        public const string SplineArea = "Dev/Chart/SplineArea";
        public const string Spline = "Dev/Chart/Spline";
        public const string StackedArea = "Dev/Chart/StackedArea";
        public const string StackedBar = "Dev/Chart/StackedBar";
        public const string StackedLine = "Dev/Chart/StackedLine";
        public const string StackedSplineArea = "Dev/Chart/StackedSplineArea";
        public const string StepArea = "Dev/Chart/StepArea";
        public const string Stepline = "Dev/Chart/Stepline";
    }

    public static class Content
    {
        public const string Barcode = "Dev/Content/Barcode";
        public const string CheckBox = "Dev/Content/CheckBox";
        public const string Image = "Dev/Content/Image";
        public const string Notes = "Dev/Content/Notes";
        public const string TextBox = "Dev/Content/TextBox";
        public const string Watermark = "Dev/Content/Watermark";
    }

    public static class Data
    {
        public const string AddNewDataSource = "Dev/Data/AddNewDataSource";
        public const string Database = "Dev/Data/Database";
        public const string DeleteDataSource = "Dev/Data/DeleteDataSource";
        public const string EditDataSource = "Dev/Data/EditDataSource";
        public const string SortAsc = "Dev/Data/SortAsc";
        public const string SortDesc = "Dev/Data/SortDesc";
        public const string Summary = "Dev/Data/Summary";
    }

    public static class Edits
    {
        public const string Copy = "Dev/Edit/Copy";
        public const string Customization = "Dev/Edit/Customization";
        public const string Cut = "Dev/Edit/Cut";
        public const string Delete = "Dev/Edit/Delete";
        public const string Edit = "Dev/Edit/Edit";
        public const string Paste = "Dev/Edit/Paste";
    }

    public static class Exports
    {
        public const string ExportFile = "Dev/Export/ExportFile";
        public const string ExportToCsv = "Dev/Export/ExportToCSV";
        public const string ExportToDocx = "Dev/Export/ExportToDOCX";
        public const string ExportToDoc = "Dev/Export/ExportToDOC";
        public const string ExportToEpub = "Dev/Export/ExportToEPUB";
        public const string ExportToHtml = "Dev/Export/ExportToHTML";
        public const string ExportToImg = "Dev/Export/ExportToIMG";
        public const string ExportToMht = "Dev/Export/ExportToMHT";
        public const string ExportToOdt = "Dev/Export/ExportToODT";
        public const string ExportToPdf = "Dev/Export/ExportToPDF";
        public const string ExportToRtf = "Dev/Export/ExportToRTF";
        public const string ExportToTxt = "Dev/Export/ExportToTXT";
        public const string ExportToXlsx = "Dev/Export/ExportToXLSX";
        public const string ExportToXls = "Dev/Export/ExportToXLS";
        public const string ExportToXml = "Dev/Export/ExportToXML";
        public const string ExportToXps = "Dev/Export/ExportToXPS";
        public const string Export = "Dev/Export/Export";
    }

    public static class Filters
    {
        public const string ClearFilter = "Dev/Filter/ClearFilter";
        public const string CrossDataSourceFiltering = "Dev/Filter/CrossDataSourceFiltering";
        public const string FilterByArgumentChart = "Dev/Filter/FilterByArgument_Chart";
        public const string FilterByArgumentPie = "Dev/Filter/FilterByArgument_Pie";
        public const string FilterBySeriesChart = "Dev/Filter/FilterBySeries_Chart";
        public const string FilterBySeriesPie = "Dev/Filter/FilterBySeries_Pie";
        public const string Filter = "Dev/Filter/Filter";
        public const string IgnoreMasterFilter = "Dev/Filter/IgnoreMasterFilter";
        public const string InsertRangeFilter = "Dev/Filter/InsertRangeFilter";
        public const string MasterFilter = "Dev/Filter/MasterFilter";
    }

    public static class Finds
    {
        public const string FindByID = "Dev/Find/FindByID";
        public const string FindCustomers = "Dev/Find/FindCustomers";
        public const string Find = "Dev/Find/Find";
    }

    public static class Format
    {
        public const string AlignCenter = "Dev/Format/AlignCenter";
        public const string AlignJustify = "Dev/Format/AlignJustify";
        public const string AlignLeft = "Dev/Format/AlignLeft";
        public const string AlignRight = "Dev/Format/AlignRight";
        public const string Bold = "Dev/Format/Bold";
        public const string ChangeFontStyle = "Dev/Format/ChangeFontStyle";
        public const string Columns = "Dev/Format/Columns";
        public const string FillBackground = "Dev/Format/FillBackground";
        public const string FontSizeDecrease = "Dev/Format/FontSizeDecrease";
        public const string FontSizeIncrease = "Dev/Format/FontSizeIncrease";
        public const string FontSize = "Dev/Format/FontSize";
        public const string HiddenText = "Dev/Format/HiddenText";
        public const string HighlightField = "Dev/Format/HighlightField";
        public const string IndentDecrease = "Dev/Format/IndentDecrease";
        public const string IndentIncrease = "Dev/Format/IndentIncrease";
        public const string Italic = "Dev/Format/Italic";
        public const string ListBullets = "Dev/Format/ListBullets";
        public const string ListMultilevel = "Dev/Format/ListMultilevel";
        public const string ListNumbers = "Dev/Format/ListNumbers";
        public const string PictureShapeFillColor = "Dev/Format/PictureShapeFillColor";
        public const string PictureShapeOutlineColor = "Dev/Format/PictureShapeOutlineColor";
        public const string Replace = "Dev/Format/Replace";
        public const string ShowHidden = "Dev/Format/ShowHidden";
        public const string SpellCheckAsYouType = "Dev/Format/SpellCheckAsYouType";
        public const string StrikeoutDouble = "Dev/Format/StrikeoutDouble";
        public const string Strikeout = "Dev/Format/Strikeout";
        public const string Subscript = "Dev/Format/Subscript";
        public const string Superscript = "Dev/Format/Superscript";
        public const string UnderlineDouble = "Dev/Format/UnderlineDouble";
        public const string UnderlineWord = "Dev/Format/UnderlineWord";
        public const string Underline = "Dev/Format/Underline";
    }

    public static class Gauges
    {
        public const string GaugeStyleFullCircular = "Dev/Gauges/GaugeStyleFullCircular";
        public const string GaugeStyleHalfCircular = "Dev/Gauges/GaugeStyleHalfCircular";
        public const string GaugeStyleLeftQuarterCircular = "Dev/Gauges/GaugeStyleLeftQuarterCircular";
        public const string GaugeStyleLinearHorizontal = "Dev/Gauges/GaugeStyleLinearHorizontal";
        public const string GaugeStyleLinearVertical = "Dev/Gauges/GaugeStyleLinearVertical";
        public const string GaugeStyleRightQuarterCircular = "Dev/Gauges/GaugeStyleRightQuarterCircular";
        public const string GaugeStyleThreeForthCircular = "Dev/Gauges/GaugeStyleThreeForthCircular";
    }

    public static class Grids
    {
        public const string Cards = "Dev/Grid/Cards";
        public const string CustomizeGrid = "Dev/Grid/CustomizeGrid";
        public const string Grid = "Dev/Grid/Grid";
        public const string Pivot = "Dev/Grid/Pivot";
    }

    public static class History
    {
        public const string HistoryItem = "Dev/History/HistoryItem";
        public const string Redo = "Dev/History/Redo";
        public const string Undo = "Dev/History/Undo";
    }

    public static class Mails
    {
        public const string Announcement = "Dev/Mail/Announcement";
        public const string Attachment = "Dev/Mail/Attachment";
        public const string Contact = "Dev/Mail/Contact";
        public const string EditContact = "Dev/Mail/EditContact";
        public const string EditFeed = "Dev/Mail/EditFeed";
        public const string EmailTemplate = "Dev/Mail/EmailTemplate";
        public const string Feeds = "Dev/Mail/Feeds";
        public const string Mail = "Dev/Mail/Mail";
        public const string NewContact = "Dev/Mail/NewContact";
        public const string NewFeed = "Dev/Mail/NewFeed";
        public const string NewMail = "Dev/Mail/NewMail";
        public const string SendCsv = "Dev/Mail/SendCSV";
        public const string SendImg = "Dev/Mail/SendIMG";
        public const string SendMht = "Dev/Mail/SendMHT";
        public const string SendPdf = "Dev/Mail/SendPDF";
        public const string SendRtf = "Dev/Mail/SendRTF";
        public const string SendTxt = "Dev/Mail/SendTXT";
        public const string SendXlsx = "Dev/Mail/SendXLSX";
        public const string SendXls = "Dev/Mail/SendXLS";
        public const string Send = "Dev/Mail/Send";
    }

    public static class Miscellaneous
    {
        public const string Colors = "Dev/Miscellaneous/Colors";
        public const string Comment = "Dev/Miscellaneous/Comment";
        public const string Content = "Dev/Miscellaneous/Content";
        public const string Country = "Dev/Miscellaneous/Country";
        public const string Currency = "Dev/Miscellaneous/Currency";
        public const string Design = "Dev/Miscellaneous/Design";
        public const string HighlightActiveElements = "Dev/Miscellaneous/HighlightActiveElements";
        public const string Language = "Dev/Miscellaneous/Language";
        public const string Palette = "Dev/Miscellaneous/Palette";
        public const string Publish = "Dev/Miscellaneous/Publish";
        public const string Video = "Dev/Miscellaneous/Video";
        public const string ViewOnWeb = "Dev/Miscellaneous/ViewOnWeb";
        public const string Windows = "Dev/Miscellaneous/Windows";
        public const string Wizard = "Dev/Miscellaneous/Wizard";
    }

    public static class Navigation
    {
        public const string Backward = "Dev/Navigation/Backward";
        public const string DocumentMap = "Dev/Navigation/DocumentMap";
        public const string Forward = "Dev/Navigation/Forward";
        public const string Home = "Dev/Navigation/Home";
        public const string NavigationBar = "Dev/Navigation/NavigationBar";
        public const string Next = "Dev/Navigation/Next";
        public const string Previous = "Dev/Navigation/Previous";
        public const string Up = "Dev/Navigation/Up";
    }

    public static class Pages
    {
        public const string InsertPageBreak = "Dev/Pages/InsertPageBreak";
        public const string MultiplePages = "Dev/Pages/MultiplePages";
        public const string PageMargins = "Dev/Pages/PageMargins";
        public const string PageOrientationLandscape = "Dev/Pages/PageOrientationLandscape";
        public const string PageOrientationPortrait = "Dev/Pages/PageOrientationPortrait";
        public const string PageOrientation = "Dev/Pages/PageOrientation";
        public const string PaperKindA3 = "Dev/Pages/PaperKind_A3";
        public const string PaperKindA4 = "Dev/Pages/PaperKind_A4";
        public const string PaperKindA5 = "Dev/Pages/PaperKind_A5";
        public const string PaperKindA6 = "Dev/Pages/PaperKind_A6";
        public const string PaperKindExecutive = "Dev/Pages/PaperKind_Executive";
        public const string PaperKindLegal = "Dev/Pages/PaperKind_Legal";
        public const string PaperKindLetter = "Dev/Pages/PaperKind_Letter";
        public const string PaperKindTabloid = "Dev/Pages/PaperKind_Tabloid";
        public const string PaperSize = "Dev/Pages/PaperSize";
    }

    public static class People
    {
        public const string AssignToMe = "Dev/People/AssignToMe";
        public const string AssignTo = "Dev/People/AssignTo";
        public const string Customer = "Dev/People/Customer";
        public const string Employee = "Dev/People/Employee";
        public const string PublicFix = "Dev/People/PublicFix";
        public const string Role = "Dev/People/Role";
        public const string Team = "Dev/People/Team";
        public const string UserGroup = "Dev/People/UserGroup";
    }

    public static class Prints
    {
        public const string Preview = "Dev/Print/Preview";
        public const string PrintDialog = "Dev/Print/PrintDialog";
        public const string PrintViaPdf = "Dev/Print/PrintViaPDF";
        public const string Print = "Dev/Print/Print";
    }

    public static class Programming
    {
        public const string BugReport = "Dev/Programming/BugReport";
        public const string Build = "Dev/Programming/Build";
        public const string CSharp = "Dev/Programming/CSharp";
        public const string ForceTesting = "Dev/Programming/ForceTesting";
        public const string Ide = "Dev/Programming/IDE";
        public const string OperatingSystem = "Dev/Programming/OperatingSystem";
        public const string ProgrammingLanguage = "Dev/Programming/ProgrammingLanguage";
        public const string ProjectDirectory = "Dev/Programming/ProjectDirectory";
        public const string ProjectFile = "Dev/Programming/ProjectFile";
        public const string Project = "Dev/Programming/Project";
        public const string Scripts = "Dev/Programming/Scripts";
        public const string ShowTestReport = "Dev/Programming/ShowTestReport";
        public const string Solution = "Dev/Programming/Solution";
        public const string Tag = "Dev/Programming/Tag";
        public const string Technology = "Dev/Programming/Technology";
        public const string Vb = "Dev/Programming/VB";
    }

    public static class Reports
    {
        public const string AddFooter = "Dev/Reports/AddFooter";
        public const string AddGroupFooter = "Dev/Reports/AddGroupFooter";
        public const string AddGroupHeader = "Dev/Reports/AddGroupHeader";
        public const string AddHeader = "Dev/Reports/AddHeader";
        public const string ConvertToParagraphs = "Dev/Reports/ConvertToParagraphs";
        public const string DeleteFooter = "Dev/Reports/DeleteFooter";
        public const string DeleteGroupFooter = "Dev/Reports/DeleteGroupFooter";
        public const string DeleteGroupHeader = "Dev/Reports/DeleteGroupHeader";
        public const string DeleteHeader = "Dev/Reports/DeleteHeader";
        public const string EditPageHf = "Dev/Reports/EditPageHF";
        public const string EmptyParagraphSeparator = "Dev/Reports/EmptyParagraphSeparator";
        public const string EmptyTableRowSeparator = "Dev/Reports/EmptyTableRowSeparator";
        public const string GroupFieldCollection = "Dev/Reports/GroupFieldCollection";
        public const string GroupFooter = "Dev/Reports/GroupFooter";
        public const string GroupHeader = "Dev/Reports/GroupHeader";
        public const string InsertFooter = "Dev/Reports/InsertFooter";
        public const string InsertHeader = "Dev/Reports/InsertHeader";
        public const string None = "Dev/Reports/None";
        public const string Parameters = "Dev/Reports/Parameters";
        public const string Report = "Dev/Reports/Report";
        public const string Separator = "Dev/Reports/Separator";
    }

    public static class Saving
    {
        public const string SaveAll = "Dev/Save/SaveAll";
        public const string SaveAndClose = "Dev/Save/SaveAndClose";
        public const string SaveAndNew = "Dev/Save/SaveAndNew";
        public const string SaveAs = "Dev/Save/SaveAs";
        public const string SaveDialog = "Dev/Save/SaveDialog";
        public const string SaveTo = "Dev/Save/SaveTo";
        public const string Save = "Dev/Save/Save";
    }

    public static class Scheduling
    {
        public const string Calendar = "Dev/Scheduling/Calendar";
        public const string CellsAutoHeight = "Dev/Scheduling/CellsAutoHeight_";
        public const string DayView = "Dev/Scheduling/DayView";
        public const string MonthView = "Dev/Scheduling/MonthView";
        public const string Recurrence = "Dev/Scheduling/Recurrence";
        public const string ShowWorkTimeOnly = "Dev/Scheduling/ShowWorkTimeOnly";
        public const string SnapToCells = "Dev/Scheduling/SnapToCells";
        public const string SplitAppointment = "Dev/Scheduling/SplitAppointment";
        public const string SwitchTimeScalesTo = "Dev/Scheduling/SwitchTimeScalesTo";
        public const string TimeLineView = "Dev/Scheduling/TimeLineView";
        public const string Time = "Dev/Scheduling/Time";
        public const string Today = "Dev/Scheduling/Today";
        public const string WeekEnd = "Dev/Scheduling/WeekEnd";
        public const string WeekView = "Dev/Scheduling/WeekView";
        public const string WorkWeekView = "Dev/Scheduling/WorkWeekView";
    }

    public static class Setup
    {
        public const string LoadPageSetup = "Dev/Setup/LoadPageSetup";
        public const string PageSetup = "Dev/Setup/PageSetup";
        public const string Properties = "Dev/Setup/Properties";
        public const string SavePageSetup = "Dev/Setup/SavePageSetup";
    }

    public static class Support
    {
        public const string Article = "Dev/Support/Article";
        public const string BreakingChange = "Dev/Support/BreakingChange";
        public const string Example = "Dev/Support/Example";
        public const string Feature = "Dev/Support/Feature";
        public const string Index = "Dev/Support/Index";
        public const string Info = "Dev/Support/Info";
        public const string Issue = "Dev/Support/Issue";
        public const string KnowledgeBaseArticle = "Dev/Support/KnowledgeBaseArticle";
        public const string PackageProduct = "Dev/Support/PackageProduct";
        public const string Product = "Dev/Support/Product";
        public const string Question = "Dev/Support/Question";
        public const string Suggestion = "Dev/Support/Suggestion";
        public const string Template = "Dev/Support/Template";
        public const string Version = "Dev/Support/Version";
    }

    public static class Tasks
    {
        public const string EditTask = "Dev/Tasks/EditTask";
        public const string NewTask = "Dev/Tasks/NewTask";
        public const string Status = "Dev/Tasks/Status";
        public const string Task = "Dev/Tasks/Task";
    }

    public static class Zooms
    {
        public const string Zoom100 = "Dev/Zoom/Zoom100";
        public const string Zoom2 = "Dev/Zoom/Zoom2";
        public const string ZoomIn = "Dev/Zoom/ZoomIn";
        public const string ZoomOut = "Dev/Zoom/ZoomOut";
        public const string Zoom = "Dev/Zoom/Zoom";
    }

    public static class Game
    {
        public const string Roulette = "Dev/Game/Roulette";
    }

    public static class FlatIcons
    {
        public static class Reports
        {
            public const string Chart1 = "FlatIcons/Reports/Chart1";
            public const string Chart2 = "FlatIcons/Reports/Chart2";

            public const string Report1 = "FlatIcons/Reports/Report1";
            public const string Report2 = "FlatIcons/Reports/Report2";
            public const string Report3 = "FlatIcons/Reports/Report3";
            public const string Report4 = "FlatIcons/Reports/Report4";
            public const string Report5 = "FlatIcons/Reports/Report5";
            public const string Report6 = "FlatIcons/Reports/Report6";
            public const string Report7 = "FlatIcons/Reports/Report7";
            public const string Report8 = "FlatIcons/Reports/Report8";
            public const string Report9 = "FlatIcons/Reports/Report9";
        }
    }
}