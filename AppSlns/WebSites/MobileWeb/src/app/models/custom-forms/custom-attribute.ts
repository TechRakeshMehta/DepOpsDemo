export class AttributesForCustomForm {
    PackageID: number
    AtrributeGroupMappingId: number
    ParentAttributeGroupMappingId: number
    ValidateExpression: string = "";
    ValidationMessage: string = "";
    AttributeGroupId: number
    AttriButeGroupName: string = "";
    AttributeId: number
    AttributeName: string = "";
    AttributeType: string = "";
    IsDisplay: Boolean
    IsRequired: Boolean
    SectionTitle: string = "";
    CustomFieldsDisplaySequence: number
    CustomHtml: string = "";
    AttributeTypeCode: string = "";
    DisplayColumns: number
    Sequence: number
    Occurence: number
    MinimumOccurence: number
    MaximumOccurence: number
    AttributeGroupMappingCode: string = "";
    MaximumValue: string = "";
    MinimumValue: string = "";
    InstructionText: string = "";
    AttributeCode: string = "";
    IsDecisionField: Boolean
    AttributeDataValue: string = "";
    InstanceID: number
    IsHiddenFromUI: Boolean
    Name: string = "";
}

export class ClientSettingCustomAttributeContract {
    CustomAttributeID: number
    SettingName: string
    SettingOverrideText: string = ''
    SettingValue: boolean
    CustomAttributeDatatypeCode: string
    CustomAttributeClientSettingMappingID: number
    SettingID: number
    IsRequired: boolean = true;
    DisplayLabel:boolean = true;
    ValidateExpression: string
    ValidationMessage: string
    AttributeName: string
    AttributeDataValue: string = "";
}

export class mailingInfoAtt {
    AttributeGroupID: number
    AttributeID: number
    InstanceId: number
    HeaderLabel: string
    IsRequired: boolean
    IsEnabled: boolean
    IsAttributeHidden: boolean
    IsAttributeGroupHidden: boolean
}
export class PaymentTypeDetails {
    PaymentType: string
    Amount: string
    PaymentTypeCode: string
    InstructionText: string
}