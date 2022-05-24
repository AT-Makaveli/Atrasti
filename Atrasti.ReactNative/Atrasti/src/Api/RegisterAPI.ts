import { HttpServiceSingleton } from "../Services/HttpService";
import { UserToken } from "../Services/Models/UserToken";
import { AUTH_REGISTER, AUTH_VALID_COMPANY, AUTH_VALID_EMAIL } from "./APIData";
import { getErrors } from "../utils/ErrorHelpers";
import { ValidEmail_Req } from "./Models/Requests/ValidEmail_Req";
import { ValidCompany_Req } from "./Models/Requests/ValidCompany_Req";
import { Register_Req } from "./Models/Requests/Register_Req";

export const COMPANY_EMPTY = "COMPANY_EMPTY";
export const COMPANY_IN_USE = "COMPANY_IN_USE";

export const EMAIL_EMPTY = "EMAIL_EMPTY";
export const EMAIL_INVALID = "EMAIL_INVALID";
export const EMAIL_IN_USE = "EMAIL_IN_USE";

export const FIRST_NAME_EMPTY = "FIRST_NAME_EMPTY";
export const LAST_NAME_EMPTY = "LAST_NAME_EMPTY";

export const PASSWORD_INVALID = "PASSWORD_INVALID";

export const UNKNOWN_ERROR = "UNKNOWN_ERROR";

export function register(email: string, firstName: string, lastName: string, company: string, password: string, userType: number): Promise<any> {
    const req = {
        email: email,
        firstName: firstName,
        lastName: lastName,
        company: company,
        password: password,
        userType: userType
    } as Register_Req;

    return new Promise<null>((resolve, reject) => {
        HttpServiceSingleton.post<UserToken>(AUTH_REGISTER, req).then(response => {
            resolve(null);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function validateEmail(email: string): Promise<null> {
    const req = {
        email: email
    } as ValidEmail_Req;

    return new Promise<null>((resolve, reject) => {
        HttpServiceSingleton.post<UserToken>(AUTH_VALID_EMAIL, req).then(response => {
            resolve(null);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function validateCompany(company: string): Promise<null> {
    const req = {
        company: company
    } as ValidCompany_Req;

    return new Promise<null>((resolve, reject) => {
        HttpServiceSingleton.post<UserToken>(AUTH_VALID_COMPANY, req).then(response => {
            resolve(null);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}
