using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using DevExpress.CodeParser;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using System.Xml;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace XmlToDT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow
    {
        public static DataSet DataSource = new DataSet();
        public static DataTable LanguageTable = new DataTable();
        public static ObservableCollection<XmlToDTViewModel> tabSource = new ObservableCollection<XmlToDTViewModel> { };
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            xmlTab.ItemsSource = tabSource;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Translate.xls";
            if (File.Exists(filePath))
                LanguageTable = DealXLS.ExcelToDS(filePath).Tables[0];
        }

        private void TableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {

        }

        #region Command-打开XML文件
        /// <summary>
        /// 语言选择命令
        /// </summary>
        private static RoutedUICommand loadNewXML = new RoutedUICommand("LoadNewXML", "LoadNewXML", typeof(MainWindow));
        public static RoutedUICommand LoadNewXML
        {
            get { return loadNewXML; }
        }


        private void LoadNewXML_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void LoadNewXML_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter.ToString() == "File")
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "XML文件|*.xml";
                DialogResult result = op.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    LoadXML(op.FileName);
                    bPath.EditValue = op.FileName;
                }
            }
            else
            {
                string filePath = bPath.EditValue.ToString();
                if (File.Exists(filePath))
                    LoadXML(filePath);
                else
                    DXMessageBox.Show("未找到：" + filePath, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadXML(string fileName)
        {
            DataSource = DealXML.XmlToDataTableByFile(fileName);
            tabSource.Clear();
            foreach (DataTable item in DataSource.Tables)
            {
                if (Models.groupName.Contains(item.TableName))
                {
                    XmlToDTViewModel viewModel = new XmlToDTViewModel(item.TableName);
                    viewModel.DetailTable = item;
                    foreach (DataColumn cokumn in item.Columns)
                    {
                        GridColumn dc = new GridColumn();
                        dc.Header = cokumn.ColumnName;
                        dc.FieldName = cokumn.ColumnName;
                        if (cokumn.ColumnName.Contains("Id"))
                        {
                            dc.CellStyle = (Style)this.FindResource("cellStyle");
                        }
                        viewModel.ListColumn.Add(dc);
                    }
                    tabSource.Add(viewModel);
                }
            }
            richEdit.LoadDocument(fileName, DocumentFormat.PlainText);
        }
        #endregion

        #region Command-导出XML文件
        /// <summary>
        /// 语言选择命令
        /// </summary>
        private static RoutedUICommand exportNewXML = new RoutedUICommand("ExportNewXML", "ExportNewXML", typeof(MainWindow));
        public static RoutedUICommand ExportNewXML
        {
            get { return exportNewXML; }
        }


        private void ExportNewXML_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void ExportNewXML_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "*.xml";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "xml files|*.xml";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.FileName = "BMS___Protocol["+DateTime.Now.Date+"]";
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && saveFileDialog.FileName != null) //打开保存文件对话框
            {
                DealXML.ConvertDataSetToXMLFile(DataSource, saveFileDialog.FileName);
                richEdit.LoadDocument(saveFileDialog.FileName, DocumentFormat.PlainText);
            }
        }
        #endregion

        #region Command-预览XML文件
        /// <summary>
        /// 语言选择命令
        /// </summary>
        private static RoutedUICommand previewXML = new RoutedUICommand("PreviewXML", "PreviewXML", typeof(MainWindow));
        public static RoutedUICommand PreviewXML
        {
            get { return previewXML; }
        }


        private void PreviewXML_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void PreviewXML_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            xmlFile.AutoHideExpandState = DevExpress.Xpf.Docking.Base.AutoHideExpandState.Expanded;
            string fileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + "interimXML.xml";
            if (File.Exists(fileName))
                File.Delete(fileName);
            DealXML.ConvertDataSetToXMLFile(DataSource, fileName);
            richEdit.LoadDocument(fileName, DocumentFormat.PlainText);
            File.Delete(fileName);
        }
        #endregion

        #region Command-翻译XML
        /// <summary>
        /// 语言选择命令
        /// </summary>
        private static RoutedUICommand translateComm = new RoutedUICommand("TranslateComm", "TranslateComm", typeof(MainWindow));
        public static RoutedUICommand TranslateComm
        {
            get { return translateComm; }
        }


        private void TranslateComm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void TranslateComm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (DataRow dr_langu in LanguageTable.Rows)
            {
                foreach (var item in tabSource)
                {
                    foreach (DataRow dr in item.DetailTable.Rows)
                    {
                        for (int j = 0; j < item.DetailTable.Columns.Count; j++)
                        {
                            if (e.Parameter.ToString()=="Zh_Ru")
                            {
                                if (dr[j].ToString() == dr_langu[0].ToString())
                                {
                                    dr[j] = dr_langu[1].ToString();
                                }
                                //else if(dr[j].ToString() == dr_langu[1].ToString())
                                //{
                                //        dr[j] = dr_langu[0].ToString();
                                //}
                            }
                            else if (e.Parameter.ToString() == "Zh_En")
                            {
                                if (dr[j].ToString() == dr_langu[0].ToString())
                                {
                                    dr[j] = dr_langu[2].ToString();
                                }
                                //else if (dr[j].ToString() == dr_langu[2].ToString())
                                //{
                                //    dr[j] = dr_langu[0].ToString();
                                //}
                            }
                            else
                            {
                                if (dr[j].ToString() == dr_langu[1].ToString())
                                {
                                    dr[j] = dr_langu[2].ToString();
                                }
                                //else if (dr[j].ToString() == dr_langu[2].ToString())
                                //{
                                //    dr[j] = dr_langu[1].ToString();
                                //}
                            }
                            
                        }
                    }
                }
            }

        }
        #endregion


        #region RichEdit初始化
        void richEdit_Loaded(object sender, RoutedEventArgs e)
        {
            richEdit.Options.HorizontalRuler.Visibility = RichEditRulerVisibility.Hidden;
            richEdit.AddService(typeof(ISyntaxHighlightService), new SyntaxHighlightService(richEdit));
            IRichEditCommandFactoryService commandFactory = richEdit.GetService<IRichEditCommandFactoryService>();
        }

        #region SyntaxHighlightService
        public class SyntaxHighlightService : ISyntaxHighlightService
        {
            #region Fields
            readonly DevExpress.Xpf.RichEdit.RichEditControl editor;
            readonly SyntaxHighlightInfo syntaxHighlightInfo;
            #endregion


            public SyntaxHighlightService(DevExpress.Xpf.RichEdit.RichEditControl editor)
            {
                this.editor = editor;
                this.syntaxHighlightInfo = new SyntaxHighlightInfo();
            }


            #region ISyntaxHighlightService Members
            void ISyntaxHighlightService.ForceExecute()
            {
                this.Execute();
            }
            void ISyntaxHighlightService.Execute()
            {
                this.Execute();
            }
            #endregion

            void Execute()
            {
                TokenCollection tokens = Parse(editor.Text);
                HighlightSyntax(tokens);
            }
            TokenCollection Parse(string code)
            {
                if (string.IsNullOrEmpty(code))
                    return null;
                ITokenCategoryHelper tokenizer = CreateTokenizer();
                if (tokenizer == null)
                    return new TokenCollection();
                return tokenizer.GetTokens(code);
            }

            ITokenCategoryHelper CreateTokenizer()
            {
                string fileName = editor.Options.DocumentSaveOptions.CurrentFileName;
                if (String.IsNullOrEmpty(fileName))
                    return null;
                ITokenCategoryHelper result = TokenCategoryHelperFactory.CreateHelperForFileExtensions(System.IO.Path.GetExtension(fileName));
                if (result != null)
                    return result;
                else
                    return null;
            }

            void HighlightSyntax(TokenCollection tokens)
            {
                if (tokens == null || tokens.Count == 0)
                    return;
                Document document = editor.Document;
                CharacterProperties cp = document.BeginUpdateCharacters(0, 1);

                List<SyntaxHighlightToken> syntaxTokens = new List<SyntaxHighlightToken>(tokens.Count);
                foreach (Token token in tokens)
                {
                    HighlightCategorizedToken((CategorizedToken)token, syntaxTokens);
                }
                document.ApplySyntaxHighlight(syntaxTokens);
                document.EndUpdateCharacters(cp);
            }
            void HighlightCategorizedToken(CategorizedToken token, List<SyntaxHighlightToken> syntaxTokens)
            {
                SyntaxHighlightProperties highlightProperties = syntaxHighlightInfo.CalculateTokenCategoryHighlight(token.Category);
                SyntaxHighlightToken syntaxToken = SetTokenColor(token, highlightProperties, editor.ActiveView.BackColor);
                if (syntaxToken != null)
                    syntaxTokens.Add(syntaxToken);
            }
            SyntaxHighlightToken SetTokenColor(Token token, SyntaxHighlightProperties foreColor, System.Drawing.Color backColor)
            {
                if (editor.Document.Paragraphs.Count < token.Range.Start.Line)
                    return null;
                int paragraphStart = DocumentHelper.GetParagraphStart(editor.Document.Paragraphs[token.Range.Start.Line - 1]);
                int tokenStart = paragraphStart + token.Range.Start.Offset - 1;
                if (token.Range.End.Line != token.Range.Start.Line)
                    paragraphStart = DocumentHelper.GetParagraphStart(editor.Document.Paragraphs[token.Range.End.Line - 1]);

                int tokenEnd = paragraphStart + token.Range.End.Offset - 1;
                System.Diagnostics.Debug.Assert(tokenEnd > tokenStart);
                return new SyntaxHighlightToken(tokenStart, tokenEnd - tokenStart, foreColor);
            }
        }
        #endregion

        #region SyntaxHighlightInfo
        public class SyntaxHighlightInfo
        {
            readonly Dictionary<TokenCategory, SyntaxHighlightProperties> properties;

            public SyntaxHighlightInfo()
            {
                this.properties = new Dictionary<TokenCategory, SyntaxHighlightProperties>();
                Reset();
            }
            public void Reset()
            {
                properties.Clear();
                Add(TokenCategory.Text, System.Drawing.Color.Black);
                Add(TokenCategory.Keyword, System.Drawing.Color.Blue);
                Add(TokenCategory.String, System.Drawing.Color.Brown);
                Add(TokenCategory.Comment, System.Drawing.Color.Green);
                Add(TokenCategory.Identifier, System.Drawing.Color.Black);
                Add(TokenCategory.PreprocessorKeyword, System.Drawing.Color.Blue);
                Add(TokenCategory.Number, System.Drawing.Color.Red);
                Add(TokenCategory.Operator, System.Drawing.Color.Black);
                Add(TokenCategory.Unknown, System.Drawing.Color.Black);
                Add(TokenCategory.XmlComment, System.Drawing.Color.Gray);

                Add(TokenCategory.CssComment, System.Drawing.Color.Green);
                Add(TokenCategory.CssKeyword, System.Drawing.Color.Brown);
                Add(TokenCategory.CssPropertyName, System.Drawing.Color.Red);
                Add(TokenCategory.CssPropertyValue, System.Drawing.Color.Blue);
                Add(TokenCategory.CssSelector, System.Drawing.Color.Blue);
                Add(TokenCategory.CssStringValue, System.Drawing.Color.Blue);

                Add(TokenCategory.HtmlAttributeName, System.Drawing.Color.Red);
                Add(TokenCategory.HtmlAttributeValue, System.Drawing.Color.Blue);
                Add(TokenCategory.HtmlComment, System.Drawing.Color.Green);
                Add(TokenCategory.HtmlElementName, System.Drawing.Color.Brown);
                Add(TokenCategory.HtmlEntity, System.Drawing.Color.Gray);
                Add(TokenCategory.HtmlOperator, System.Drawing.Color.Black);
                Add(TokenCategory.HtmlServerSideScript, System.Drawing.Color.Black);
                Add(TokenCategory.HtmlString, System.Drawing.Color.Blue);
                Add(TokenCategory.HtmlTagDelimiter, System.Drawing.Color.Blue);
            }
            void Add(TokenCategory category, System.Drawing.Color foreColor)
            {
                SyntaxHighlightProperties item = new SyntaxHighlightProperties();
                item.ForeColor = foreColor;
                properties.Add(category, item);
            }

            public SyntaxHighlightProperties CalculateTokenCategoryHighlight(TokenCategory category)
            {
                SyntaxHighlightProperties result = null;
                if (properties.TryGetValue(category, out result))
                    return result;
                else
                    return properties[TokenCategory.Text];
            }
        }
        #endregion

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var vm = (XmlToDTViewModel)xmlTab.SelectedItem;
            vm.AddRow();
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var vm = (XmlToDTViewModel)xmlTab.SelectedItem;
            vm.RemoveRow();
        }

        #endregion

    }



    #region Convert-bLoadEnable
    public class IsEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsEnable = false;
            if (parameter != null)
            {
                if (parameter.ToString() == "Load")
                {
                    if (value != null)
                    {
                        if (value.ToString() != string.Empty)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (value != null)
                    {
                        if ((int)value > 0)
                        {
                            return true;
                        }
                    }
                }

            }
            return IsEnable;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
