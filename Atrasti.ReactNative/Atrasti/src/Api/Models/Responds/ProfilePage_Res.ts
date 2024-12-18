
import { User_Res } from './User_Res';
import { CompanyPage_Res } from './CompanyPage_Res';
import { AgentPage_Res } from './AgentPage_Res';
import { UserType_Mod } from './UserType_Mod';

export interface ProfilePage_Res {
    setup: boolean;
    isProfileOwner: boolean;
    user: User_Res;
    companyPage: CompanyPage_Res;
    agentPage: AgentPage_Res;
    userType: UserType_Mod;
}