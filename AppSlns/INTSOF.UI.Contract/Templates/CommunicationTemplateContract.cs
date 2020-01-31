using System;
using System.Collections.Generic;
using Entity;
using INTSOF.Utils;
using System.Collections;

namespace INTSOF.UI.Contract.Templates
{
    /// <summary>
    /// Contract used to get the data of the System Communication Entity
    /// </summary>
    public class CommunicationTemplateContract
    {
        public Int32 SystemCommunicationID { get; set; }
        public Int32? CommunicationSubEventID { get; set; }
        public Int32 SystemCommunicationDeliveryID { get; set; }
        public Int32 CurrentUserId { get; set; }
        public String SenderEmailID { get; set; }
        public String Name { get; set; }
        public String SenderName { get; set; }
        public Int32 ReceiverOrganizationUserId { get; set; }
        public String RecieverEmailID { get; set; }
        public String RecieverName { get; set; }
        public DateTime? DispatchedDate { get; set; }
        public Boolean IsDispatched { get; set; }
        public String Subject { get; set; }
        public String RecipientType { get; set; }
        public Int32 TotalRecordCount { get; set; }
        //UAT-2628
        public Boolean IsToUser { get; set; }
    }

    /// <summary>
    /// Contract used to get the data of the Communication Template and System events
    /// </summary>
    [Serializable]
    public class SystemEventTemplatesContract
    {
        public Int32 CommunicationTemplateId { get; set; }
        public Int32 SubEventId { get; set; }
        public Int32 EventId { get; set; }
        public Int32 CommunicationTypeId { get; set; }
        public Int32 CurrentUserId { get; set; }
        public String CommunicationTypeName { get; set; }
        public String EventName { get; set; }
        public String SubEventName { get; set; }
        public String TemplateName { get; set; }
        public String Subject { get; set; }
        public String TemplateDescription { get; set; }
        public String TemplateContent { get; set; }

        public Int32? CategoryId { get; set; }
        public Int32? ItemId { get; set; }

        public String InstitutionName { get; set; }
        public String CategoryName { get; set; }
        public String ItemName { get; set; }

        public SystemEventSetting EventSettings { get; set; }


        public Boolean IsCategoryLevel { get; set; }
        public Boolean IsNotificationBlocked { get; set; }//UAT-3656

        public int CommunicationLanguageId { get; set; }

        public List<SystemEventSetting> lstAgencyHierarcEventSettings { get; set; } //UAT-3704
        public String AgencyHierarchyID { get; set; } //UAT-3704
        public String AgencyHierarchy { get; set; } //UAT-3704

    }

    [Serializable]
    public class CommunicationCCUserContract
    {
        public Int32 CommunicationCCUsersID { get; set; }
        public Int32 CommunicationCCMasterID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        public String UserMiddleName { get; set; }
        public String UserEmailAddress { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsSaveUpdateRequired { get; set; }
        public Int16? SelectedCopyTypeID { get; set; }
        public Boolean? IsForCommunicationCentre { get; set; }
        public Boolean? IsForEmail { get; set; }
        public String CopyTypeName { get; set; }
        public String CopyTypeCode { get; set; }
        public List<int> SelectedRecordIds { get; set; }
        public String SelectedRecordIdsStr { get; set; }
        public int SelectedRecordTypeId { get; set; }
        public String SelectedRecordNames { get; set; }
        public string SelectedRecordTypeCode { get; set; }
        public string SelectedRecordTypeName { get; set; }
        public Boolean? IsOnlyRotationCreatedNotification { get; set; }
    }

    /// <summary>
    /// Contract used to get the data of the Communication Template and System events
    /// </summary>
    public class NodeTemplatesContract
    {
        public Int32 CommunicationTemplateId { get; set; }
        public Int32 SubEventId { get; set; }
        public Int32 EventId { get; set; }
        public Int32 Frequency { get; set; }
        public Int32 NoOfDays { get; set; }

        public Int32 CommunicationTypeId { get; set; }
        public Int32 CurrentUserId { get; set; }
        public String CommunicationTypeName { get; set; }
        public String EventName { get; set; }
        public String SubEventName { get; set; }
        public String TemplateName { get; set; }
        public String Subject { get; set; }
        public String TemplateDescription { get; set; }
        public String TemplateContent { get; set; }
    }

    /// <summary>
    /// Class to represent the NodeNotificationType based data fetched using the dynamic query in SP 'usp_GetNodeTemplates'
    /// </summary>
    public class NodeNotificationSpecificTemplates
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
    }

    [Serializable]
    public class ExternalCopyUsersContract
    {
        public String UserName { get; set; }
        public String UserEmailAddress { get; set; }
        public String CopyTypeName { get; set; }
        public String CopyTypeCode { get; set; }
        public Int32 UserID { get; set; }
    }

    [Serializable]
    public class SearchCommunicationTemplateContract
    {
        private String _emailtype = String.Empty;

        public String EmailType
        {
            get
            {
                return (_emailtype);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var countAposNo = value.Split('\'').Length - 1;
                    if (countAposNo == 1 || countAposNo % 2 != 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            //Even
                            _emailtype = (value.Replace("'", "''"));
                        }
                        else
                        {
                            _emailtype = value;
                        }
                    }
                    else if (countAposNo % 2 == 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            _emailtype = (value.Replace("'", "''"));
                        }
                        else
                        {
                            _emailtype = value;
                        }
                    }
                }
            }
        }

        public Boolean DisallowApostropheConversion { get; set; }
        private String _reciever = String.Empty;

        public String Receiver
        {
            get
            {
                return (_reciever);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var countAposNo = value.Split('\'').Length - 1;
                    if (countAposNo == 1 || countAposNo % 2 != 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            //Even
                            _reciever = (value.Replace("'", "''"));
                        }
                        else
                        {
                            _reciever = value;
                        }
                    }
                    else if (countAposNo % 2 == 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            _reciever = (value.Replace("'", "''"));
                        }
                        else
                        {
                            _reciever = value;
                        }
                    }
                }
            }
        }
        public String ReceiverEmailId { get; set; }
        public DateTime? DispatchDate { get; set; }
        private String _subject = String.Empty;

        public String Subject
        {
            get
            {
                return (_subject);
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var countAposNo = value.Split('\'').Length - 1;
                    if (countAposNo == 1 || countAposNo % 2 != 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            //Even
                            _subject = (value.Replace("'", "''"));
                        }
                        else
                        {
                            _subject = value;
                        }
                    }
                    else if (countAposNo % 2 == 0)
                    {
                        if (!DisallowApostropheConversion)
                        {
                            _subject = (value.Replace("'", "''"));
                        }
                        else
                        {
                            _subject = value;
                        }
                    }
                }
            }
        }
        public Boolean? IsBcc { get; set; }
        public Boolean? IsCc { get; set; }
        public Boolean? IsTo { get; set; }
        public Int32 TotalRecordCount { get; set; }
        public List<String> FilterColumns { get; set; }
        public List<String> FilterOperators { get; set; }
        public ArrayList FilterValues { get; set; }
        public List<String> FilterTypes { get; set; }

        public CustomPagingArgsContract GridCustomPagingArguments
        {
            get;
            set;
        }



    }
}
