using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.SearchAbstractFactory;
using INTSOF.Utils;

namespace CoreWeb.Search.Services
{
    public class SearchService : ISearchService
    {
        public IQueryable SearchByText(string SearchEntity, string SearchText, List<string> lstFields, string orderby = "", bool exactmatch = false)
        {
            IQueryable results = null;
            SearchFactory<Message> MessageFactory = new SearchFactory<Message>();
            SearchFactory<NonMessage> NonMessageFactory = new SearchFactory<NonMessage>();
            switch (SearchEntity.ToLower().Trim())
            {
                case SysXSearchConsts.Emails:
                    Email<Message> emailProduct = MessageFactory.Create<Email<Message>>();
                    results = emailProduct.SearchByText(SearchText, lstFields);
                    break;
                case SysXSearchConsts.Alerts:
                    Alert<Message> alertProduct = MessageFactory.Create<Alert<Message>>();
                    results = alertProduct.SearchByText(SearchText, lstFields);
                    break;
                case SysXSearchConsts.SMS:
                    SMS<Message> smsProduct = MessageFactory.Create<SMS<Message>>();
                    results = smsProduct.SearchByText(SearchText, lstFields);
                    break;
                case SysXSearchConsts.Chats:
                    Chat<Message> chatProduct = MessageFactory.Create<Chat<Message>>();
                    results = chatProduct.SearchByText(SearchText, lstFields);
                    break;
                case SysXSearchConsts.Applicants:
                    Applicants<NonMessage> applicantProduct = NonMessageFactory.Create<Applicants<NonMessage>>();
                    results = applicantProduct.SearchByText(SearchText, lstFields);
                    break;
                case SysXSearchConsts.Students:
                    Student<NonMessage> studentProduct = NonMessageFactory.Create<Student<NonMessage>>();
                    results = studentProduct.SearchByText(SearchText, lstFields);
                    break;
                case SysXSearchConsts.CommunicationTypes:
                    CommunicationType<NonMessage> communicationTypes = NonMessageFactory.Create<CommunicationType<NonMessage>>();
                    results = communicationTypes.SearchByText(SearchText, lstFields);
                    break;
                case SysXSearchConsts.Messages:
                    Messages<NonMessage> messages = NonMessageFactory.Create<Messages<NonMessage>>();
                    results = messages.SearchByText(SearchText, lstFields);
                    break;
                default:
                    break;
            }
            return results;
        }
    }
}
