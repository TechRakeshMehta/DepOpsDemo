using System;

namespace CoreWeb.Shell.MasterPages
{
    public interface IChildPageView
    {
        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        void ShowErrorMessage(String errorMessage);

        /// <summary>
        /// Shows the success message.
        /// </summary>
        /// <param name="successMessage"></param>
        void ShowSuccessMessage(String successMessage);

        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="infoMessage">The info message.</param>
        /// <remarks></remarks>
        void ShowInfoMessage(String infoMessage);

        /// <summary>
        /// Hides the error message.
        /// </summary>
        /// <remarks></remarks>
        void HideErrorMessage();

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        void ShowErrorInfoMessage(String errorMessage);
    }
}




