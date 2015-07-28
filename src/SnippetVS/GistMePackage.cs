//------------------------------------------------------------------------------
// <copyright file="GistMePackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using System.Text.RegularExpressions;
using System.IO;

namespace SnippetVS
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [Guid(GistMePackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class GistMePackage : Package
    {
        /// <summary>
        /// GistMePackage GUID string.
        /// </summary>
        public const string PackageGuidString = "a403d8d5-767d-4575-9ad3-fd83db87ebc3";
        IVsStatusbar _statusBar;

        /// <summary>
        /// Initializes a new instance of the <see cref="GistMe"/> class.
        /// </summary>
        public GistMePackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            GistMe.Initialize(this);
            base.Initialize();
            _statusBar = ServiceProvider.GlobalProvider.GetService(typeof(SVsStatusbar)) as IVsStatusbar;

            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel));
            var workspace = componentModel.GetService<VisualStudioWorkspace>();
            SolutionManager.CurrentWorkspace = workspace;
        }

        #endregion

        public void ShowStatus(string message)
        {
            int frozen;
            _statusBar.IsFrozen(out frozen);
            if (frozen == 0)
            {
                _statusBar.SetText(message);
            }
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        internal void MenuItemCallback(object sender, EventArgs e)
        {
            IVsTextManager textManager = (IVsTextManager)GetService(typeof(SVsTextManager));
            int startPosition, endPosition;
            string filePath;
            if (TextManagerHelpers.TryFindDocumentAndPosition(textManager, out filePath, out startPosition, out endPosition))
            {
                var target = PrepareGist(filePath, startPosition, endPosition);
                GoToGist(target);
            }
            else
            {
                ShowStatus("To capture a gist, place the cursor in C# code first.");
            }
        }

        private string PrepareGist(string filePath, int startPosition, int endPosition)
        {
            var analyzer = new DocumentAnalyzer();
            var element = analyzer.FindContainedMethods(filePath, startPosition, endPosition);
            File.WriteAllText(@"D:\test.txt", element);
            return element;
        }

        private void GoToGist(string contents)
        {
            System.Diagnostics.Process.Start(
                @"C:\Users\Amadeus\Documents\GitHub\SnippetVS\gitIntegration\gitIntegration\bin\Debug\gitIntegration.exe");
        }

    }
}
