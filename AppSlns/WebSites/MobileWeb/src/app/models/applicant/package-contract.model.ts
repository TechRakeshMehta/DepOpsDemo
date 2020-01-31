 
export class PackageDetail {
    PackageSubscriptionId: number;
    PackageId: number = 0;
    PackageName: string = "";
    PackageStatus: string = "";
    lstCategory: Array<CategoryDetail>;
}


export class CategoryDetail {
    CategoryId: number = 0;
    CategoryName: string = "";
    CategoryNonCompliancedate: string = "";
    CategoryStatus: string = "";
    lstItem: Array<ItemDetail>;
}

export class ItemDetail {
    ItemId: number = 0;
    ItemName: string = "";
    ItemStatus: string = "";
    lstAttribute: Array<AttributeDetail>;
}

export class AttributeDetail {
    AttributeId: number = 0;
    AttributeName: string = "";
    AttributeValue: string = "";
}
 export class UserRegistration {
      Name :string;
      Id:number;  
      Email :String ;
 }
  