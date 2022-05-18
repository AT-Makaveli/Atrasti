import { AUTH_API_LOGIN, AUTH_FORGOT_PASSWORD } from "./APIData";
import { HttpServiceSingleton } from "../Services/HttpService";
import { UserServiceSingleton } from "../Services/UserService";
import { getErrors } from "../utils/ErrorHelpers";
import { UserToken } from "../Services/Models/UserToken";
import { Login_Req } from "./Models/Requests/Login_Req";
import { ForgotPassword_Req } from "./Models/Requests/ForgotPassword_Req";
import { ForgotPassword_Res } from "./Models/Responds/ForgotPassword_Res";

export interface LoginResult {
    code: string,
    data: string
}

export const LOGIN_SUCCESS = "SUCCESS";
export const LOGIN_FAILED = "FAILED";
export const AUTH_ALL_FIELDS = "AUTH_ALL_FIELDS";
export const INVALID_EMAIL_OR_PASSWORD = "INVALID_EMAIL_OR_PASSWORD";

export function login(username: string, password: string): Promise<LoginResult> {
    const req = {
        email: username,
        password: password
    } as Login_Req;

    return new Promise<LoginResult>((resolve, reject) => {
        HttpServiceSingleton.post<UserToken>(AUTH_API_LOGIN, req).then(async response => {
            if(response.data.token !== undefined) {
                await UserServiceSingleton.signIn(response.data);
                resolve({
                    code: LOGIN_SUCCESS,
                    data: ""
                });
            }
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function forgotPassword(email: string): Promise<ForgotPassword_Res> {
    const req: ForgotPassword_Req = {
        email: email
    };

    return new Promise<ForgotPassword_Res>((resolve, reject) => {
        HttpServiceSingleton.post<ForgotPassword_Res>(AUTH_FORGOT_PASSWORD, req).then(response => {
            console.log(response.data);
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        })
    });
}
