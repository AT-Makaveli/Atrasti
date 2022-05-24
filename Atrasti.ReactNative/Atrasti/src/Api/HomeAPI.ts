import { HttpServiceSingleton } from "../Services/HttpService";
import { getErrors } from "../utils/ErrorHelpers";
import { Home_Res } from "./Models/Responds/Home_Res";
import { MAIN_PAGE_GET } from './APIData';

export function getMainPage(): Promise<Home_Res> {
    return new Promise<Home_Res>((resolve, reject) => {
        HttpServiceSingleton.get<Home_Res>(MAIN_PAGE_GET).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        })
    });
}
