﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="CoreWeb.ApplicantRotationRequirement.Views.SharedUserRequirementRowControl" CodeBehind="SharedUserRequirementRowControl.ascx.cs" %>
<%@ Register TagName="AttributeControl" TagPrefix="infsu" Src="~/ApplicantRotationRequirement/UserControl/SharedUserRequirementAttributeControl.ascx" %>

<%@ Register TagPrefix="infs" Namespace="INTERSOFT.WEB.UI.WebControls" Assembly="INTERSOFT.WEB.UI" %>
<style>
    .instBox {
        background-color: #ff6;
        background-color: rgba(200,200,200,0.5);
        border: 1px solid #ccc;
        padding: 10px;
        border-radius: 10px;
        box-shadow: 0 5px 10px rgba(0, 0, 0, 0.2);
        position: absolute;
        /*top: 170px;*/
        z-index: 2000;
        float: left;
        /*width: 250px;*/
        max-width: 250px;
        font-family: Verdana;
        color: black;
    }

    .sxlbHideBackground {
        background: inherit !important;
    }
</style>
<div id="divInstructionTextMain" style="display: none;" runat="server" class="row">
    <div id="divInstructionTextControl" runat="server">
    </div>
    <div class='sxroend'>
    </div>
</div>
<div class="row">
    <div class="col-md-12">
        <div class="row">
            <div id="div2Control" runat="server">
            </div>
        </div>
    </div>
</div>