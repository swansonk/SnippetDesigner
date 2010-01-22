﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Microsoft.SnippetDesigner.OptionPages
{
    /// <summary>
    /// Custom control for the Reset options page
    /// </summary>
    public partial class ResetOptionsControl : UserControl
    {
        public ResetOptionsControl()
        {
            InitializeComponent();
            SnippetDesignerPackage.Instance.SnippetIndex.PropertyChanged += new PropertyChangedEventHandler(SnippetIndex_PropertyChanged);
        }

        void SnippetIndex_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                if (e.PropertyName.Equals("IsIndexLoading", StringComparison.Ordinal) ||
                    e.PropertyName.Equals("IsIndexUpdating", StringComparison.Ordinal)
                    )
                {

                    resetIndexDirectoriesButton.Enabled = rebuildIndexButton.Enabled = !SnippetDesignerPackage.Instance.SnippetIndex.IsIndexLoading &&
                                                 !SnippetDesignerPackage.Instance.SnippetIndex.IsIndexUpdating;
                }
            }
        }

        void SetStatusOfButtons()
        {
            resetIndexDirectoriesButton.Enabled = rebuildIndexButton.Enabled = !SnippetDesignerPackage.Instance.SnippetIndex.IsIndexLoading &&
                                                !SnippetDesignerPackage.Instance.SnippetIndex.IsIndexUpdating;
        }

        private void resetIndexDirectoriesButton_Click(object sender, EventArgs e)
        {
            if (!SnippetDesignerPackage.Instance.SnippetIndex.IsIndexLoading &&
                !SnippetDesignerPackage.Instance.SnippetIndex.IsIndexUpdating)
            {
                SnippetDesignerPackage.Instance.Settings.IndexedSnippetDirectories = new List<string>(SnippetDirectories.Instance.AllSnippetDirectories);
            }
        }

        private void rebuildIndexButton_Click(object sender, EventArgs e)
        {
            if (!SnippetDesignerPackage.Instance.SnippetIndex.IsIndexLoading &&
               !SnippetDesignerPackage.Instance.SnippetIndex.IsIndexUpdating)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    SnippetDesignerPackage.Instance.SnippetIndex.RebuildSnippetIndex();
                });
            }
        }
    }
}