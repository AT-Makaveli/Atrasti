
import { Company_Res } from './Company_Res';
import { CompanyInfo_Res } from './CompanyInfo_Res';
import { BaseCategory_Res } from './BaseCategory_Res';

export interface ManageProfile_Res {
    company: Company_Res;
    companyInfo: CompanyInfo_Res;
    usedCategories: BaseCategory_Res[];
    allCategories: BaseCategory_Res[];
}