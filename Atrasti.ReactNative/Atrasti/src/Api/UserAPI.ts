import { HttpServiceSingleton } from "../Services/HttpService";
import { UserToken } from "../Services/Models/UserToken";
import { USER_DATA_SET_FCM_TOKEN } from "./APIData";
import { getErrors } from "../utils/ErrorHelpers";
import { SetFcmToken_Req } from "./Models/Requests/SetFcmToken_Req";

export interface User {
    firstName: string;
    lastName: string;
    company: string;
}

export const FCM_TOKEN_NOT_SET = "FCM_TOKEN_NOT_SET";

export function setFcmToken(fcmToken: string): Promise<any> {
    const req = {
        fcmToken: fcmToken
    } as SetFcmToken_Req;

    return new Promise<null>((resolve, reject) => {
        HttpServiceSingleton.post<UserToken>(USER_DATA_SET_FCM_TOKEN, req).then(response => {
            resolve(null);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}
