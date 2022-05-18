import { HttpServiceSingleton } from "../Services/HttpService";
import { SEARCH_CACHE_SEARCH, SEARCH_SEARCH, SEARCH_SEARCH_HISTORY } from "./APIData";
import { getErrors } from "../utils/ErrorHelpers";
import { Search_Req } from "./Models/Requests/Search_Req";
import { Search_Res } from "./Models/Responds/Search_Res";
import { CacheSearch_Req } from "./Models/Requests/CacheSearch_Req";

export function search(searchQuery: string): Promise<Search_Res> {
    const req = {
        searchQuery: searchQuery
    } as Search_Req;

    return new Promise<Search_Res>((resolve, reject) => {
        HttpServiceSingleton.post(SEARCH_SEARCH, req).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function cacheSearch(companyId: number): Promise<void> {
    const req: CacheSearch_Req = {
        companyId: companyId
    };

    return new Promise<void>((resolve, reject) => {
        HttpServiceSingleton.post(SEARCH_CACHE_SEARCH, req).then(() => {
            resolve();
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function getSearchHistory(): Promise<Search_Res> {
    return new Promise<Search_Res>((resolve, reject) => {
        HttpServiceSingleton.get(SEARCH_SEARCH_HISTORY).then(response => {
            resolve(response.data);
        }).catch(error => {
            console.log(error);
        });
    });
}
