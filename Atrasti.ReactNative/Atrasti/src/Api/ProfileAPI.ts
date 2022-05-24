import {
    CATEGORY_ALLBASECATEGORIES,
    PROFILE_MANAGE_AGENT_PAGE,
    PROFILE_MANAGE_COMPANY_PAGE,
    PROFILE_MANAGE_PROFILE_PAGE,
    PROFILE_PROFILE_PAGE
} from "./APIData";
import { HttpServiceSingleton } from "../Services/HttpService";
import { getErrors } from "../utils/ErrorHelpers";
import { ProfilePage_Res } from "./Models/Responds/ProfilePage_Res";
import { ManageProfile_Res } from "./Models/Responds/ManageProfile_Res";
import { ManageAgent_Req } from "./Models/Requests/ManageAgent_Req";
import { AgentPage_Res } from "./Models/Responds/AgentPage_Res";
import { ProfilePage_Req } from "./Models/Requests/ProfilePage_Req";
import { ManageProfile_Req } from "./Models/Requests/ManageProfile_Req";
import { Company_Res } from "./Models/Responds/Company_Res";
import { BaseCategories_Res } from "./Models/Responds/BaseCategories_Res";
export const USER_NOT_SET = "USER_NOT_SET";

export function getProfilePage(userId: string | null): Promise<ProfilePage_Res> {
    const req: ProfilePage_Req = {
        userId: userId
    };

    return new Promise<ProfilePage_Res>((resolve, reject) => {
        HttpServiceSingleton.post(PROFILE_PROFILE_PAGE, req).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function manageProfilePage(): Promise<ManageProfile_Res> {
    return new Promise<ManageProfile_Res>((resolve, reject) => {
        HttpServiceSingleton.get(PROFILE_MANAGE_PROFILE_PAGE).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function getManageAgentPage(): Promise<AgentPage_Res> {
    return new Promise<AgentPage_Res>((resolve, reject) => {
        HttpServiceSingleton.get(PROFILE_MANAGE_AGENT_PAGE).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function postManageAgentPage(req: ManageAgent_Req): Promise<AgentPage_Res> {
    return new Promise<AgentPage_Res>((resolve, reject) => {
        HttpServiceSingleton.post<AgentPage_Res>(PROFILE_MANAGE_AGENT_PAGE, req).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function postManageCompanyPage(req: ManageProfile_Req): Promise<Company_Res> {
    return new Promise<Company_Res>((resolve, reject) => {
        HttpServiceSingleton.post<Company_Res>(PROFILE_MANAGE_COMPANY_PAGE, req).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function getAllBaseCategories(): Promise<BaseCategories_Res> {
    return new Promise<BaseCategories_Res>((resolve, reject) => {
        HttpServiceSingleton.get<BaseCategories_Res>(CATEGORY_ALLBASECATEGORIES).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}
