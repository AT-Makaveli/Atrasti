import { UserService, UserServiceSingleton } from "./UserService";
import axios, { AxiosResponse } from "axios";

export class HttpService {
    constructor(private readonly _userService: UserService) {
    }

    post<T = any, R = AxiosResponse<T>, D = any>(url: string, data?: D): Promise<R> {
        let config;

        if(this._userService.isSignedIn()) {
            config = {
                headers: {Authorization: `Bearer ${this._userService.getToken()}`},
            };
        }

        return axios.post(url, data, config);
    }

    get<T = any, R = AxiosResponse<T>, D = any>(url: string): Promise<R> {
        let config;

        if(this._userService.isSignedIn()) {
            config = {
                headers: {Authorization: `Bearer ${this._userService.getToken()}`},
            };
        }

        return axios.get(url, config);
    }
}

export const HttpServiceSingleton = new HttpService(UserServiceSingleton);
