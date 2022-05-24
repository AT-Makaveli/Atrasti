
import { BaseCategory_Res } from './BaseCategory_Res';

export interface ManageProfile_Req {
    logo: string;
    address: string;
    city: string;
    county: string;
    country: string;
    website: string;
    companyDesc: string;
    phoneNumber: string;
    businessType: string;
    mainProducts: string;
    mainMarkets: string;
    certificates: string;
    yearEstablished: string;
    capacity: string;
    newCategories: BaseCategory_Res[];
}