﻿namespace MvvmFx.CaliburnMicro
{
    using System;
#if WISEJ
    using Wisej.Web;
#else
    using System.Windows.Forms;
#endif

    /// <summary>
    /// Specifies the contextual information about an application.
    /// </summary>
#if WISEJ
    public class ApplicationContext
    {
        private ApplicationContext()
        {
            // disable parameterless constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class, using a given main windows Page.
        /// </summary>
        /// <param name="mainPage">The main Page.</param>
        public ApplicationContext(Page mainPage)
        {
            if (mainPage == null)
                throw new ArgumentNullException(nameof(mainPage));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class, using a given main windows Form.
        /// </summary>
        /// <param name="mainWindow">The main Form.</param>
        public ApplicationContext(Form mainWindow)
        {
            if (mainWindow == null)
                throw new ArgumentNullException(nameof(mainWindow));

            Wisej.Base.ApplicationBase.Session.MainWindow = mainWindow;
        }

        private static Form SessionMainWindow
        {
            get { return Wisej.Base.ApplicationBase.Session.MainWindow; }
            set { Wisej.Base.ApplicationBase.Session.MainWindow = value; }
        }

        /// <summary>
        /// Gets or sets the application main Form.
        /// </summary>
        /// <value>
        /// The application main Form.
        /// </value>
        public static Form MainWindow
        {
            get { return SessionMainWindow; }
            set
            {
                if (SessionMainWindow != value)
                {
                    if (SessionMainWindow != null)
                    {
                        SessionMainWindow.Dispose();
                    }

                    SessionMainWindow = value;
                }
            }
        }

        /// <summary>
        /// Gets the application main Page.
        /// </summary>
        /// <value>
        /// The application main Page.
        /// </value>
        public static Page MainPage
        {
            get { return Application.MainPage; }
        }
    }

#else
    public class ApplicationContext : System.Windows.Forms.ApplicationContext
    {
        private static Form _mainWindow;

        /// <summary>
        /// Gets or sets the application main window.
        /// </summary>
        /// <value>
        /// The application main window.
        /// </value>
        public static Form MainWindow
        {
            get { return _mainWindow; }
            set
            {
                if (_mainWindow != value)
                {
                    if (_mainWindow != null)
                    {
                        _mainWindow.Dispose();
                    }

                    _mainWindow = value;
                }
            }
        }

        private ApplicationContext()
        {
            // disable parameterless constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class, using a given main windows Form.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        /// <remarks>This also sets the base <see cref="System.Windows.Forms.ApplicationContext.MainForm"/> to <paramref name="mainWindow"/></remarks>
        public ApplicationContext(Form mainWindow)
            : base(mainWindow)
        {
            if (mainWindow == null)
                throw new ArgumentNullException(nameof(mainWindow));

            _mainWindow = mainWindow;
        }
    }
#endif
}