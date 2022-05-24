import { ManageProfile_Req } from "../Api/Models/Requests/ManageProfile_Req";
import { ManageProfilePageState } from "./Views/Company/ManageProfileView";

export function mapSaveCompanyRequest(state: ManageProfilePageState): ManageProfile_Req {
    return {
        logo: state.image?.data as string,
        address: state.address,
        city: state.city,
        county: state.county,
        country: state.country,
        website: state.website,
        companyDesc: state.companyDesc,
        phoneNumber: state.phoneNumber,
        businessType: state.businessType,
        mainProducts: state.mainProducts,
        mainMarkets: state.mainMarkets,
        certificates: state.certificates,
        yearEstablished: state.yearEstablished,
        capacity: state.capacity,
        newCategories: state.selectedCategories
    }
}
