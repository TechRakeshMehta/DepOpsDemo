using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;

using System.Linq;
using System.Web;
using CoreWeb.Search.Services;
using INTSOF.Utils;
using INTSOF.SharedObjects;
using INTSOF.Contracts;



namespace CoreWeb.Search.Views
{
    public class SearchMainPresenter : Presenter<ISearchMainView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private ISearchController _controller;
        // public SearchMainPresenter([CreateNew] ISearchController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        public IList<SearchType> LoadSearchType()
        {
            List<SearchType> SearchTypes = new List<SearchType>();
            SearchTypes.Add(new SearchType("Messages", "Messages"));
            SearchTypes.Add(new SearchType("CommunicationTypes", "CommunicationTypes"));
            //SearchTypes.Add(new SearchType("Emails", "Emails"));
            //SearchTypes.Add(new SearchType("SMS", "SMS"));
            //SearchTypes.Add(new SearchType("Applicants", "Applicants"));
            //SearchTypes.Add(new SearchType("Students", "Students"));
            return SearchTypes;
        }


        public IQueryable PerformSearch()
        {
            IQueryable results = null;
            List<String> lstFields = new List<String>() { "Message", "Subject" };
            List<String> lstCommunicationTypeFields = new List<String>() { "Code", "Name", "Description" };
            List<String> lstStudentFields = new List<String>() { "Message", "Subject" };
            #region "Way I : Search Service Model"
            /* Way I : Search Service Model */
            //SearchFactory<Message> MessageFactory = new SearchFactory<Message>();
            //SearchFactory<NonMessage> NonMessageFactory = new SearchFactory<NonMessage>();

            //switch (this.View.SearchType.ToLower().Trim())
            //{
            //    case Consts.Emails:
            //        Email<Message> emailProduct = MessageFactory.Create<Email<Message>>();
            //        results = emailProduct.SearchByText(this.View.SearchText, lstFields);
            //        break;
            //    case Consts.Alerts:
            //        Alert<Message> alertProduct = MessageFactory.Create<Alert<Message>>();
            //        results = alertProduct.SearchByText(this.View.SearchText, lstFields);
            //        break;
            //    case Consts.SMS:
            //        SMS<Message> smsProduct = MessageFactory.Create<SMS<Message>>();
            //        results = smsProduct.SearchByText(this.View.SearchText, lstFields);
            //        break;
            //    case Consts.Chats:
            //        Chat<Message> chatProduct = MessageFactory.Create<Chat<Message>>();
            //        results = chatProduct.SearchByText(this.View.SearchText, lstFields);
            //        break;
            //    case Consts.Applicants:
            //        Applicants<NonMessage> applicantProduct = NonMessageFactory.Create<Applicants<NonMessage>>();
            //        results = applicantProduct.SearchByText(this.View.SearchText, lstApplicantFields);
            //        break;
            //    case Consts.Students:
            //        Student<NonMessage> studentProduct = NonMessageFactory.Create<Student<NonMessage>>();
            //        results = studentProduct.SearchByText(this.View.SearchText, lstStudentFields);
            //        break;
            //    default:
            //        break;
            //}
            #endregion
            /* Way II : Search Service Model */
            //WebClientApplication applicationInstance = (WebClientApplication)HttpContext.Current.ApplicationInstance;
            //CompositionContainer rootContainer = applicationInstance.RootContainer;
            //ISearchService searchService = rootContainer.Services.Get<ISearchService>(true);

            //switch (this.View.SearchType.ToLower().Trim())
            //{
            //    case SysXSearchConsts.Emails:
            //        results = searchService.SearchByText(SysXSearchConsts.Emails, this.View.SearchText, lstFields);
            //        break;
            //    case SysXSearchConsts.Alerts:
            //        results = searchService.SearchByText(SysXSearchConsts.Alerts, this.View.SearchText, lstFields);
            //        break;
            //    case SysXSearchConsts.SMS:
            //        results = searchService.SearchByText(SysXSearchConsts.SMS, this.View.SearchText, lstFields);
            //        break;
            //    case SysXSearchConsts.Chats:
            //        results = searchService.SearchByText(SysXSearchConsts.Chats, this.View.SearchText, lstFields);
            //        break;
            //    case SysXSearchConsts.CommunicationTypes:
            //        results = searchService.SearchByText(SysXSearchConsts.CommunicationTypes, this.View.SearchText, lstCommunicationTypeFields);
            //        break;
            //    case SysXSearchConsts.Messages:
            //        results = searchService.SearchByText(SysXSearchConsts.Messages, this.View.SearchText, lstFields);
            //        break;
            //    case SysXSearchConsts.Students:
            //        results = searchService.SearchByText(SysXSearchConsts.Students, this.View.SearchText, lstStudentFields);
            //        break;

            //    default:
            //        break;
            //}
            #region "Way I : Search WCF Service"
            /* Way III : Search WCF Service */
            //SearchService svcSearch = new SearchService();
            //switch (this.View.SearchType.ToLower().Trim())
            //{
            //    case Consts.Emails:
            //        results = svcSearch.SearchByText(Consts.Emails, this.View.SearchText, lstFields);
            //        break;
            //    case Consts.Alerts:
            //        results = svcSearch.SearchByText(Consts.Alerts, this.View.SearchText, lstFields);
            //        break;
            //    case Consts.SMS:
            //        results = svcSearch.SearchByText(Consts.SMS, this.View.SearchText, lstFields);
            //        break;
            //    case Consts.Chats:
            //        results = svcSearch.SearchByText(Consts.Chats, this.View.SearchText, lstFields);
            //        break;
            //    case Consts.Applicants:
            //        results = svcSearch.SearchByText(Consts.Applicants, this.View.SearchText, lstApplicantFields);
            //        break;
            //    case Consts.Students:
            //        results = svcSearch.SearchByText(Consts.Students, this.View.SearchText, lstStudentFields);
            //        break;
            //    default:
            //        break;
            //}
            #endregion
            return results;
        }
    }
    public class SearchType
    {
        public SearchType(string Text, string Value)
        {
            SearchTypeText = Text;
            SearchTypeValue = Value;
        }
        public string SearchTypeText { get; set; }
        public string SearchTypeValue { get; set; }
    }
}




