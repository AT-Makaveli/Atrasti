import { HttpServiceSingleton } from "../Services/HttpService";
import { PRODUCT_UPLOAD_PRODUCT, PRODUCT_USER_CATEGORIES, PROFILE_LIKE_INTERACT } from "./APIData";
import { getErrors } from "../utils/ErrorHelpers";
import { Product_Res } from "./Models/Responds/Product_Res";
import { ProductLike_Req } from "./Models/Requests/ProductLike_Req";
import { UploadProduct_Req } from "./Models/Requests/UploadProduct_Req";
import { ProductCategories_Res } from "./Models/Responds/ProductCategories_Res";

export const PRODUCT_NOT_SET = "PRODUCT_NOT_SET";

export const PRODUCT_IMAGE_NOT_SET = "IMAGE_NOT_SET";
export const PRODUCT_TITLE_NOT_SET = "PRODUCT_TITLE_NOT_SET";
export const PRODUCT_DESC_NOT_SET = "PRODUCT_DESC_NOT_SET";
export const PRODUCT_TAGS_NOT_SET = "PRODUCT_TAGS_NOT_SET";

export function getUserCategories() {
    return new Promise<ProductCategories_Res>((resolve, reject) => {
        HttpServiceSingleton.get<ProductCategories_Res>(PRODUCT_USER_CATEGORIES).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        })
    });
}

export function likeInteract(productId: number): Promise<Product_Res> {
    const req = {
        productId: productId
    } as ProductLike_Req;

    return new Promise<Product_Res>((resolve, reject) => {
        HttpServiceSingleton.post(PROFILE_LIKE_INTERACT, req).then(response => {
            resolve(response.data);
        }).catch(error => {
            reject(getErrors(error.response.data));
        });
    });
}

export function uploadProduct(image: string, title: string, description: string, tags: string[], category: number) {
    const req: UploadProduct_Req = {
        image: image,
        title: title,
        description: description,
        tags: tags,
        category: category
    };

    return new Promise<UploadProduct_Req>((resolve, reject) => {
        HttpServiceSingleton.post(PRODUCT_UPLOAD_PRODUCT, req).then(response => {
            resolve(response.data);
        }).catch(error => {
            console.log(error);
            reject(getErrors(error.response.data));
        })
    });
}

