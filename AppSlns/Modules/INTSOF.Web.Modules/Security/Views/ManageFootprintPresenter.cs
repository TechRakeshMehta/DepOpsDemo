#region Header Comment Block

// 
// Copyright BestX, Inc. 2011
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageFootprintPresenter.cs
// Purpose:   
//
// Revisions:
// Author           Date                Comment
// ------           ----------          -------------------------------------------------
// Security Team   09/11/2012           Initial.
//

#endregion

#region Namespace

#region System Defined

using System;
using Microsoft.Practices.CompositeWeb;
using System.Linq;
using System.Collections.Generic;

#endregion

#region Application Specific

using INTSOF.Utils;
using BESTX.Entity;
using BESTX.Business.RepoManagers;
#endregion

#endregion
namespace  BESTX.WEB.IntsofSecurityModel.Views
{
    public class ManageFootprintPresenter : Presenter<IManageFootprintView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        public void RetrievingServiceFootprintStatesCounties()
        {
            List<State> statesList = SecurityManager.GetStates().Where(cond => cond.StateID != SysXConsts.NONE).ToList();

            List<ServiceFootprint> serviceFootprintList = new List<ServiceFootprint>();
            List<ServiceFootprint> serviceFootPrintTreeListState = View.ServiceFootprintTreeListState;    //View.ViewContract.ServiceFootprintTreeListState;

            foreach (State stateItem in statesList)
            {
                ServiceFootprint serviceFootprint = new ServiceFootprint();
                serviceFootprint.Name = stateItem.StateName;
                serviceFootprint.StateID = stateItem.StateID;
                serviceFootprint.ID = stateItem.StateID;
                if (stateItem.StateFootprints.Any(cond => cond.SFP_TenantID == View.ViewContract.TenantId && cond.SFP_StateID == stateItem.StateID))
                {
                    if (!serviceFootPrintTreeListState.IsNull() && serviceFootPrintTreeListState.Any(cond => cond.StateID != SysXConsts.NONE && cond.StateID == serviceFootprint.StateID))
                    {
                        //Check box UI state
                        Boolean selectedCheckedCurrentState = serviceFootPrintTreeListState.FirstOrDefault(cond => cond.StateID != SysXConsts.NONE
                                                                && cond.StateID == serviceFootprint.StateID).Selected;

                        //check box DB selected Value state
                        Boolean selectedCheckedPreviousState = stateItem.StateFootprints.FirstOrDefault(cond => cond.SFP_TenantID == View.ViewContract.TenantId
                                                                && cond.SFP_StateID == stateItem.StateID).SFP_IsActive;

                        if (selectedCheckedCurrentState && selectedCheckedPreviousState)
                        {
                            serviceFootprint.Selected = true;
                        }
                        else if (selectedCheckedCurrentState && !selectedCheckedPreviousState)
                        {
                            serviceFootprint.Selected = true;
                        }
                        else
                        {
                            serviceFootprint.Selected = false;
                        }
                    }
                    else
                    {
                        serviceFootprint.Selected = stateItem.StateFootprints.FirstOrDefault(cond => cond.SFP_TenantID == View.ViewContract.TenantId && cond.SFP_StateID == stateItem.StateID).SFP_IsActive;
                    }
                }
                else
                {
                    if (!serviceFootPrintTreeListState.IsNull() && serviceFootPrintTreeListState.Any(cond => cond.StateID != SysXConsts.NONE && cond.StateID == serviceFootprint.StateID))
                    {
                        //Check box UI state
                        serviceFootprint.Selected = serviceFootPrintTreeListState.FirstOrDefault(cond => cond.StateID != SysXConsts.NONE
                                                                && cond.StateID == serviceFootprint.StateID).Selected;
                    }
                    else
                    {
                        serviceFootprint.Selected = false;
                    }
                }
                serviceFootprintList.Add(serviceFootprint);
            }
            View.ServiceFootprint = serviceFootprintList;
        }


        public List<ServiceFootprint> RetrievingServiceFootprintCounties(Int32 stateID)
        {
            StateFootprint stateFootprint = SecurityManager.GetStateFootPrint(View.ViewContract.TenantId, stateID);
            Int32 stateFootPrintID = stateFootprint != null ? stateFootprint.SFP_StateFootprintID : SysXConsts.NONE;
            List<ServiceFootprint> serviceFootPrintTreeListState = View.ServiceFootprintTreeListState;   //View.ViewContract.ServiceFootprintTreeListState;
            List<ServiceFootprint> serviceFootprintList = new List<ServiceFootprint>();
            List<County> countiesList = SecurityManager.GetServiceFootprintCounties(stateID).Where(cond => cond.CountyID != SysXConsts.NONE).ToList();

            foreach (County cItem in countiesList)
            {
                ServiceFootprint serviceFootprint = new ServiceFootprint();
                serviceFootprint.Name = cItem.CountyName;
                serviceFootprint.CountyID = cItem.CountyID;
                serviceFootprint.ID = cItem.CountyID;
                serviceFootprint.ParentID = cItem.StateID;
                serviceFootprint.StateID = cItem.StateID;

                if (cItem.CountyFootprints.Any(cond => cond.CFP_CountyID == cItem.CountyID && cond.CFP_StateFootprintID == stateFootPrintID))
                {
                    //check box DB selected Value state
                    Boolean selectedCheckedPreviousState = cItem.CountyFootprints.FirstOrDefault(cond => cond.CFP_CountyID == cItem.CountyID
                                                            && cond.CFP_StateFootprintID == stateFootPrintID).CFP_IsActive;
                    if (!serviceFootPrintTreeListState.IsNull() && serviceFootPrintTreeListState.Any(cond => cond.CountyID != SysXConsts.NONE
                                                                                                    && cond.CountyID == serviceFootprint.CountyID))
                    {
                        //Check box UI state
                        Boolean selectedCheckedCurrentState = serviceFootPrintTreeListState.FirstOrDefault(cond => cond.CountyID != SysXConsts.NONE
                                                                    && cond.CountyID == serviceFootprint.CountyID).Selected;

                        if (selectedCheckedCurrentState && selectedCheckedPreviousState)
                        {
                            serviceFootprint.Selected = true;
                        }
                        else if (selectedCheckedCurrentState && !selectedCheckedPreviousState)
                        {
                            serviceFootprint.Selected = true;
                        }
                        else
                        {
                            serviceFootprint.Selected = false;
                        }
                    }
                    else
                    {
                        serviceFootprint.Selected = selectedCheckedPreviousState;
                    }
                }
                else
                {
                    if (!serviceFootPrintTreeListState.IsNull() && serviceFootPrintTreeListState.Any(cond => cond.CountyID != SysXConsts.NONE
                                                                                                    && cond.CountyID == serviceFootprint.CountyID))
                    {
                        //Check box UI state
                        serviceFootprint.Selected = serviceFootPrintTreeListState.FirstOrDefault(cond => cond.CountyID != SysXConsts.NONE
                                                                && cond.CountyID == serviceFootprint.CountyID).Selected;
                    }
                    else
                    {
                        serviceFootprint.Selected = false;
                    }
                }
                serviceFootprintList.Add(serviceFootprint);
            }
            return serviceFootprintList;
        }

        public List<ServiceFootprint> RetrievingServiceFootprintJudges(Int32 countyID)
        {
            Int32 countyFootPrintID = SecurityManager.GetCountyFootPrintID(View.ViewContract.TenantId, countyID);
            List<Judge> judgesList = SecurityManager.GetServiceFootprintJudges(countyID).Where(cond => cond.JDG_JudgeID != SysXConsts.NONE).ToList();
            List<ServiceFootprint> serviceFootPrintTreeListState = View.ServiceFootprintTreeListState;                 //View.ViewContract.ServiceFootprintTreeListState;
            List<ServiceFootprint> serviceFootprintList = new List<ServiceFootprint>();
            foreach (Judge jItem in judgesList)
            {
                ServiceFootprint serviceFootprint = new ServiceFootprint();
                serviceFootprint.Name = jItem.JDG_Name;
                serviceFootprint.JudgeID = jItem.JDG_JudgeID;
                serviceFootprint.ID = jItem.JDG_JudgeID;
                serviceFootprint.ParentID = jItem.JDG_CountyID;
                serviceFootprint.CountyID = jItem.JDG_CountyID;

                if (jItem.JudgeFootprints.Any(cond => cond.JFP_JudgeID == jItem.JDG_JudgeID && cond.JFP_CountyFootprintID == countyFootPrintID))
                {
                    //check box DB selected Value state
                    Boolean selectedCheckedPreviousState = jItem.JudgeFootprints.FirstOrDefault(cond => cond.JFP_JudgeID == jItem.JDG_JudgeID
                                                            && cond.JFP_CountyFootprintID == countyFootPrintID).JFP_IsActive;

                    if (!serviceFootPrintTreeListState.IsNull() && serviceFootPrintTreeListState.Any(cond => cond.JudgeID != SysXConsts.NONE
                                                                                                                         && cond.JudgeID == jItem.JDG_JudgeID))
                    {
                        //Check box UI state
                        Boolean selectedCheckedCurrentState = serviceFootPrintTreeListState.FirstOrDefault(cond => cond.JudgeID != SysXConsts.NONE
                                                                    && cond.JudgeID == jItem.JDG_JudgeID).Selected;

                        if (selectedCheckedCurrentState && selectedCheckedPreviousState)
                        {
                            serviceFootprint.Selected = true;
                        }
                        else if (selectedCheckedCurrentState && !selectedCheckedPreviousState)
                        {
                            serviceFootprint.Selected = true;
                        }
                        else
                        {
                            serviceFootprint.Selected = false;
                        }
                    }
                    else
                    {
                        serviceFootprint.Selected = selectedCheckedPreviousState;
                    }


                }
                else
                {
                    if (!serviceFootPrintTreeListState.IsNull() && serviceFootPrintTreeListState.Any(cond => cond.JudgeID != SysXConsts.NONE
                                                                                                                       && cond.JudgeID == jItem.JDG_JudgeID))
                    {
                        //Check box UI state
                        serviceFootprint.Selected = serviceFootPrintTreeListState.FirstOrDefault(cond => cond.JudgeID != SysXConsts.NONE
                                                                                                 && cond.JudgeID == jItem.JDG_JudgeID).Selected;
                    }
                    else
                    {
                        serviceFootprint.Selected = false;
                    }
                }
                serviceFootprintList.Add(serviceFootprint);
            }
            return serviceFootprintList;           
        }

        public void SaveServiceFootPrint()
        {
            //List<ServiceFootprint> serviceFootprintList = View.ViewContract.ServiceFootprint;
            //if (SecurityManager.SaveServiceFootPrint(View.ViewContract.ServiceFootprint, View.ViewContract.TenantId, View.ViewContract.CurrentUserID))
            if (SecurityManager.SaveServiceFootPrint(View.ServiceFootprintTreeListState, View.ViewContract.TenantId, View.ViewContract.CurrentUserID))
            {
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.SERVICE_FOOTPRINTS) + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            }
        }


        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
