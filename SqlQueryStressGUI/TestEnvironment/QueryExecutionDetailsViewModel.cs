using SqlQueryStressEngine;
using SqlQueryStressEngine.Parameters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Clipboard = System.Windows.Clipboard;

namespace SqlQueryStressGUI.TestEnvironment
{
    public class QueryExecutionDetailsViewModel : ViewModel
    {
        public QueryExecutionDetailsViewModel()
        {
            CopyExecutionPlanCommand = new CommandHandler((_) => Clipboard.SetText(ExecutionPlan));
            SaveExecutionPlanCommand = new CommandHandler((_) => SaveExecutionPlan());
        }

        private QueryExecution _queryExecution;
        public QueryExecution QueryExecution
        {
            get => _queryExecution;
            set
            {
                SetProperty(value, ref _queryExecution);

                NotifyPropertyChanged(nameof(ExecutionStatisticsTable));
                NotifyPropertyChanged(nameof(QueryParameters));
                NotifyPropertyChanged(nameof(ExecutionError));
                NotifyPropertyChanged(nameof(ExecutionPlan));
            }
        }

        public QueryExecutionStatisticsTable ExecutionStatisticsTable
        {
            get
            {
                if(QueryExecution == null)
                {
                    return new QueryExecutionStatisticsTable();
                }
                else
                {
                    return QueryExecutionStatisticsTable.CreateFromExecutionResult(QueryExecution);
                }
            }
        }

        public IEnumerable<ParameterValue> QueryParameters
        {
            get => QueryExecution.Parameters?.Parameters ?? Array.Empty<ParameterValue>();
        }

        public string ExecutionError
        {
            get => QueryExecution.ExecutionError?.Message ?? "N/A";
        }

        public string ExecutionPlan
        {
            get => FormatXml(QueryExecution.ExecutionPlan);
        }

        private string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                // Handle and throw if fatal exception here; don't just ignore them
                return xml;
            }
        }

        public CommandHandler CopyExecutionPlanCommand { get; }
        
        public CommandHandler SaveExecutionPlanCommand { get; }

        private void SaveExecutionPlan()
        {
            var saveFileDialog = new SaveFileDialog();
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, ExecutionPlan);
            }
        }
    }
}
