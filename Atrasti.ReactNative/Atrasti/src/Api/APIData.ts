export const URL = "http://94.46.243.183:5000/";

export const MAIN_PAGE_GET = URL + "MainPage/Get";

export const AUTH_API_LOGIN = URL + "Auth/Login";
export const AUTH_FORGOT_PASSWORD = URL + "Auth/ForgotPassword";

export const AUTH_VALID_COMPANY = URL + "Auth/ValidCompany";
export const AUTH_VALID_EMAIL = URL + "Auth/ValidEmail";
export const AUTH_REGISTER = URL + "Auth/Register";

export const PROFILE_PROFILE_PAGE = URL + "Profile/ProfilePage";
export const PROFILE_LIKE_INTERACT = URL + "Profile/LikeInteract";
export const PROFILE_MANAGE_PROFILE_PAGE = URL + "Profile/ManageProfilePage";
export const PROFILE_MANAGE_AGENT_PAGE = URL + "Profile/ManageAgentPage";
export const PROFILE_MANAGE_COMPANY_PAGE = URL + "Profile/ManageCompanyPage";

export const PRODUCT_UPLOAD_PRODUCT = URL + "Product/UploadProduct";
export const PRODUCT_USER_CATEGORIES = URL + "Product/UserCategories";

export const CATEGORY_ALLBASECATEGORIES = URL + "Category/AllBaseCategories";

export const SEARCH_SEARCH = URL + "Search/Search";
export const SEARCH_CACHE_SEARCH = URL + "Search/CacheSearch";
export const SEARCH_SEARCH_HISTORY = URL + "Search/SearchHistory";

export const COUNTRY_COUNTRIES = URL + "Country/Countries";

export const CHAT_GET_FRIENDS = URL + "Chat/GetFriends";
export const CHAT_SEND_CHAT = URL + "Chat/SendChat";
export const CHAT_GET_MESSAGES = URL + "Chat/ChatMessages";

export const USER_DATA_SET_FCM_TOKEN = URL + "UserData/SetFcmToken";

export const PRODUCT_IMAGE: string = URL + "products/{0}.png"
export const LOGO_IMAGE: string = URL + "logos/{0}"

export function getProductImage(id: number) {
    return format(PRODUCT_IMAGE, id);
}

export function getLogoImage(imageName: any) {
    return format(LOGO_IMAGE, imageName);
}

function format(prev: string, ...args: any[]) {
    return prev.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
            ? args[number]
            : match
            ;
    });
};
