
import { Product_Res } from './Product_Res';
import { Company_Res } from './Company_Res';
import { CompanyInfo_Res } from './CompanyInfo_Res';

export interface CompanyPage_Res {
    products: Product_Res[];
    company: Company_Res;
    companyInfo: CompanyInfo_Res;
}